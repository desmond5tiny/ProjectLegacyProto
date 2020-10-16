using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase][RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    public int inventorySize = 0;
    public Inventory inventory;
    public float dPS;
    private float gatherSkill = 4; //temp use speed with modifier skill "Gather"

    private StateMachine stateMachine;
    public IState currentState;

    private NavMeshAgent agent;
    [HideInInspector]
    public ResourceObject resourceTarget = null;
    private ItemData resourceItem;

    [HideInInspector]
    public Vector3 clickTarget = Vector3.zero;
    private float stopDistance = 0;
    [HideInInspector]
    public bool clickMove = false;

    
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


        stateMachine.SetState(idleState);

        //transitions
        AddNewTransition(idleState, toResource, HasRTarget());
        AddNewTransition(toResource, harvestResource, ReachedResource());
        AddNewTransition(harvestResource, findResource, ResourceIsEmpty());
        AddNewTransition(harvestResource, findStorage, InventoryFull());

        stateMachine.AddAnyTransition(moveToPoint, () => clickMove == true);
        AddNewTransition(moveToPoint, idleState, ReachedPoint());

        void AddNewTransition(IState fromState, IState toState, Func<bool> condition) => stateMachine.AddTransition(fromState, toState, condition);

        //condition functions
        Func<bool> HasRTarget() => () => resourceTarget != null;
        Func<bool> ReachedPoint() => () => Vector3.Distance(transform.position, clickTarget) < stopDistance;
        Func<bool> ReachedResource() => () => resourceTarget != null && Vector3.Distance(transform.position, resourceTarget.transform.position) < stopDistance;
        Func<bool> ResourceIsEmpty() => () => resourceTarget == null && !inventory.CanAdd(resourceItem);
        Func<bool> InventoryFull() => () => !inventory.CanAdd(resourceItem);
    } 


    void Update() => stateMachine.Tick();

    public void Build()
    {
        Debug.Log("Build");
    }

    /*public void ChopTree()
    {
        float doDamage = 0;
        int resourceGet;
        int surplus=0;
        ItemData resourceData = taskTarget.GetComponent<IResource>().GetResourceData();
        if (Input.GetKeyDown(KeyCode.K)) { doDamage = 5; }

        if (doDamage >= 1 && inventory.CanAdd(resourceData))
        {
            if(taskTarget.GetComponent<IResource>().GetRescource(doDamage, out resourceGet)) //do damage to target
            {
                if(!inventory.AddItem(resourceData, resourceGet, out surplus))//attempt to store item in inv
                {
                    Debug.Log("inventory full");
                    DropResource(resourceData, surplus);
                    //store cargo
                }

                Debug.Log("surplusWood: " + surplusWood);
                for (int i = 0; i < inventory.container.Count; i++)
                {
                    Debug.Log("Inv "+i+": "+inventory.container[i].amount);
                }
            }
            else // target is empty //if inv not full
            {
                Debug.Log("Change Tree Target");
                //change target
            }
        }
    }*/


    public void MoveTo(Vector3 _destination, float _stopDistance)
    {
        clickTarget = _destination;
        stopDistance = _stopDistance;
        clickMove = true;
    }

    public void SetResourceTarget(ResourceObject _target)
    {
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
                inventory.AddItem(resourceItem, takeResult, out surplus);
                if (surplus > 0) { DropResource(resourceItem, surplus); }
            }
            else //target is empty
            {
                Debug.Log("find new target");
                resourceTarget = null;
            }
        }
    }

    public void DropResource(ItemData _item, int _amount)
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
