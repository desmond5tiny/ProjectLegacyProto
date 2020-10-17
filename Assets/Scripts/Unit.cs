using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase] [RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    public int inventorySize = 0;
    public Inventory inventory;
    public float dPS;
    public Collider resourceSearchColl;
    //temp / change
    private readonly float gatherSkill = 4; //temp use speed with modifier skill "Gather"

    private StateMachine stateMachine;
    public enum unitTask { None, Gather, Build}
    unitTask currentTask;

    private NavMeshAgent agent;
    [HideInInspector]
    public ResourceObject resourceTarget;
    private ItemData resourceItem;
    [HideInInspector]
    public Building storeTarget;
    private ItemData storeItem;

    [HideInInspector]
    public Vector3 clickTarget = Vector3.zero;
    private float stopDistance = 0;
    [HideInInspector]
    public bool clickMove = false;
    public bool moving = false;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        inventory = new Inventory(inventorySize);
        stateMachine = new StateMachine();

        //states
        IdleState idleState = new IdleState(this);
        MoveToPointState moveToPoint = new MoveToPointState(this, agent);
        MoveToResourceState toResource = new MoveToResourceState(this, agent);
        HarvestResourceState harvestResource = new HarvestResourceState(this);
        FindStorageState findStorage = new FindStorageState(this);
        FindResourceState findResource = new FindResourceState(this);
        MoveToStorageState toStorage = new MoveToStorageState(this, agent);
        StoreItemsState storeItems = new StoreItemsState(this);


        stateMachine.SetState(idleState);

        //transitions
        AddNewTransition(idleState, toResource, HasRTarget());
        AddNewTransition(toResource, harvestResource, ReachedResource());
        AddNewTransition(harvestResource, findResource, ResourceIsEmpty());
        AddNewTransition(harvestResource, findStorage, InventoryFull());
        AddNewTransition(findStorage, toStorage, HasStoreTarget());
        AddNewTransition(toStorage, storeItems, ReachedStorage());
        AddNewTransition(storeItems, findStorage, cantStore());
        AddNewTransition(storeItems, toResource, GatherAndTarget());
        AddNewTransition(storeItems, findResource, GatherAndNoTarget());

        stateMachine.AddAnyTransition(moveToPoint, () => clickMove);
        AddNewTransition(moveToPoint, idleState, ReachedPoint());

        void AddNewTransition(IState fromState, IState toState, Func<bool> condition) => stateMachine.AddTransition(fromState, toState, condition);

        //condition functions
        Func<bool> HasRTarget() => () => resourceTarget != null;
        Func<bool> HasStoreTarget() => () => storeTarget != null;
        Func<bool> ReachedPoint() => () => Vector3.Distance(transform.position, clickTarget) < stopDistance;
        Func<bool> ReachedResource() => () => resourceTarget != null && Vector3.Distance(transform.position, resourceTarget.transform.position) < stopDistance;
        Func<bool> ResourceIsEmpty() => () => resourceTarget == null && !inventory.CanAdd(resourceItem);
        Func<bool> InventoryFull() => () => !inventory.CanAdd(resourceItem);
        Func<bool> ReachedStorage() => () => Vector3.Distance(transform.position, storeTarget.transform.position) < 1f;
        Func<bool> cantStore() => () => storeTarget == null && inventory.container.Count > 0;
        Func<bool> GatherAndTarget() => () => inventory.container.Count == 0 && resourceTarget != null;
        Func<bool> GatherAndNoTarget() => () => inventory.container.Count == 0 && resourceTarget == null && currentTask == unitTask.Gather;
    }


    void Update() => stateMachine.Tick();

    public void Build()
    {
        Debug.Log("Build");
    }

    public void MoveTo(Vector3 _destination, float _stopDistance)
    {
        if (!moving)
        {
            currentTask = unitTask.None;
            clickTarget = _destination;
            stopDistance = _stopDistance;
            clickMove = true;
        }
    }

    public ItemData GetStoreItem() { return storeItem; }

    public void SetResourceTarget(ResourceObject _target)
    {
        currentTask = unitTask.Gather;
        resourceTarget = _target;
        resourceItem = resourceTarget.GetResourceItemData();
        stopDistance = resourceTarget.interactionRadius;
    }

    public void TakeResource()
    {
        int takeResult, surplus;

        if(inventory.CanAdd(resourceItem)) //redundant?
        {
            if (resourceTarget.GetRescource(gatherSkill * (dPS / 10), out takeResult)) //if target has resources; do damage to target and out itemsget
            {
                surplus = inventory.AddItem(resourceItem, takeResult);
                storeItem = resourceItem;
                if (surplus > 0) { DropResource(resourceItem, surplus); }
            }
            else //target is empty
            {
                Debug.Log("find new target");
                resourceTarget = null;
            }
        }
    }

    public void StoreResource()
    {
        if (inventory.container.Count > 0)
        {
            int surplus;
            if (storeTarget.StoreItem(inventory.container[0].item, inventory.container[0].amount, out surplus))
            {
                inventory.RemoveItem(inventory.container[0].item, inventory.container[0].amount - surplus);
            }
            else
            {
                storeItem = inventory.container[0].item;
                storeTarget = null;
            }
        }
    }

    public void DropResource(ItemData _item, int _amount) //refactor
    {
        GameObject newResourceItem = new GameObject(_item.name);
        newResourceItem.AddComponent<Item>().SetItemData(_item);
        for (int i = 0; i < Mathf.FloorToInt(_amount /_item.maxStackSize)+1; i++)
        {
            if (_amount > _item.maxStackSize)
            {
                newResourceItem.GetComponent<Item>().currentStackSize = _item.maxStackSize;
                Instantiate(newResourceItem);
                _amount -= _item.maxStackSize;
            }
            else if(_amount > 0)
            {
                newResourceItem.GetComponent<Item>().currentStackSize = _amount;
                Instantiate(newResourceItem);
                _amount = 0;
            }
        }
    }
}
