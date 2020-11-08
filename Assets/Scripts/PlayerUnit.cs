using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase] [RequireComponent(typeof(NavMeshAgent))]
public class PlayerUnit : MonoBehaviour, ITakeDamage
{
    public UnitStats stats;
    public int inventorySize = 0;
    public Inventory inventory;
    public Collider resourceSearchColl;

    //temp //temp get from unit stats
    public float dPS;
    private readonly float attSpeed = 5;
    private readonly float skillGather = 4;
    private readonly float skillBuild = 2;

    private StateMachine stateMachine;
    public enum unitTask { None, Move, Gather, Build}
    unitTask currentTask = unitTask.None;

    private NavMeshAgent agent;
    [HideInInspector]
    public ResourceObject resourceTarget;
    [HideInInspector]
    public Vector3 prevResourcePos;
    private ItemData resourceItem;
    List<ResourceObject> nearbyRescources = new List<ResourceObject>();

    [HideInInspector]
    public Building storeTarget;
    private ItemData storeItem;

    [HideInInspector]
    public BuildFence buildTarget;

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
        MoveToBuildsite moveToBuild = new MoveToBuildsite(this, agent);
        HarvestResourceState harvestResource = new HarvestResourceState(this);
        FindStorageState findStorage = new FindStorageState(this);
        FindResourceState findResource = new FindResourceState(this, agent);
        MoveToStorageState toStorage = new MoveToStorageState(this, agent);
        StoreItemsState storeItems = new StoreItemsState(this);
        BuildState buildState = new BuildState(this);

        stateMachine.SetState(idleState);

        //transitions
        AddNewTransition(moveToPoint, idleState, Stuck());
        AddNewTransition(idleState, toResource, HasRTarget());
        AddNewTransition(idleState, moveToBuild, HasBuildTarget());
        AddNewTransition(toResource, harvestResource, ReachedResource());
        AddNewTransition(harvestResource, findStorage, InventoryFull());
        AddNewTransition(harvestResource, findResource, ResourceIsEmpty());
        AddNewTransition(findStorage, toStorage, HasStoreTarget());
        AddNewTransition(toStorage, storeItems, ReachedStorage());
        AddNewTransition(storeItems, findStorage, cantStore());
        AddNewTransition(storeItems, toResource, GatherAndTarget());
        AddNewTransition(storeItems, findResource, GatherAndNoTarget());
        AddNewTransition(findResource, toResource, HasRTarget());
        AddNewTransition(findResource, idleState, NoResourceNearbyEmpty());
        AddNewTransition(findResource, findStorage, NoResourceNearbyNotEmpty());
        AddNewTransition(moveToBuild, buildState, ReachedBuildsite());
        AddNewTransition(buildState, idleState, () => buildTarget == null);

        stateMachine.AddAnyTransition(idleState, () => currentTask == unitTask.None);
        stateMachine.AddAnyTransition(moveToPoint, () => clickMove);
        AddNewTransition(moveToPoint, idleState, ReachedPoint());

        void AddNewTransition(IState fromState, IState toState, Func<bool> condition) => stateMachine.AddTransition(fromState, toState, condition);

        //condition functions
        Func<bool> Stuck() => () => moveToPoint.stuckTime > 1f;
        Func<bool> HasRTarget() => () => resourceTarget != null;
        Func<bool> HasStoreTarget() => () => storeTarget != null;
        Func<bool> HasBuildTarget() => () => buildTarget != null;
        Func<bool> ReachedPoint() => () => Vector3.Distance(transform.position, clickTarget) < stopDistance;
        Func<bool> ReachedResource() => () => resourceTarget != null && Vector3.Distance(transform.position, resourceTarget.transform.position) < stopDistance;
        Func<bool> ReachedBuildsite() => () => buildTarget != null && Vector3.Distance(transform.position, buildTarget.transform.position) < stopDistance;
        Func<bool> ResourceIsEmpty() => () => resourceTarget == null;
        Func<bool> InventoryFull() => () => !inventory.CanAdd(resourceItem);
        Func<bool> ReachedStorage() => () => Vector3.Distance(transform.position, storeTarget.transform.position) < stopDistance;
        Func<bool> cantStore() => () => storeTarget == null && inventory.container.Count > 0;
        Func<bool> GatherAndTarget() => () => inventory.container.Count == 0 && resourceTarget != null;
        Func<bool> GatherAndNoTarget() => () => inventory.container.Count == 0 && resourceTarget == null && currentTask == unitTask.Gather;
        Func<bool> NoResourceNearbyEmpty() => () => inventory.container.Count == 0 && resourceTarget == null && nearbyRescources.Count == 0;
        Func<bool> NoResourceNearbyNotEmpty() => () => inventory.container.Count > 0 && resourceTarget == null;
    }

    public void Initialize(string _dna)
    {
        stats = new UnitStats(_dna);
    }

    void Update() => stateMachine.Tick();

    public void MoveTo(Vector3 _destination, float _stopDistance)
    {
        if (!moving)
        {
            currentTask = unitTask.Move;
            clickTarget = _destination;
            stopDistance = _stopDistance;
            resourceTarget = null;
            buildTarget = null;
            clickMove = true;
        }
    }

    public void SetBuildTarget(BuildFence _target)
    {
        currentTask = unitTask.Build;
        buildTarget = _target;
        stopDistance = _target.interactionRadius;
    }

    public void Build()
    {
        //Debug.Log("Build");
        buildTarget.AddHealth(((skillBuild / 2) + (attSpeed / 4)) * 2);
    }

    public ItemData GetStoreItem() { return storeItem; }

    public void SetResourceTarget(ResourceObject _target)
    {
        currentTask = unitTask.Gather;
        resourceTarget = _target;
        resourceItem = resourceTarget.GetResourceItemData();
        stopDistance = resourceTarget.interactionRadius;
    }

    public List<ResourceObject> GetNearbyResources()
    {
        nearbyRescources.Clear();
        Collider[] colls = Physics.OverlapSphere(transform.position, 20f, 1 << 11);

        foreach (var col in colls)
        {
            if (col.GetComponentInParent<ResourceObject>())
            {
                if (col.GetComponentInParent<ResourceObject>().GetResourceItemData() == resourceItem 
                    && col.GetComponentInParent<ResourceObject>().currentResourceAmount>0)
                {
                    nearbyRescources.Add(col.GetComponentInParent<ResourceObject>());
                }
            }
        }
        if (nearbyRescources.Count == 0)
        {
            //currentTask = unitTask.None;
        }
        return nearbyRescources;
    }

    public void TakeResource()
    {
        int takeResult, surplus;
        if(inventory.CanAdd(resourceItem)) //redundant?
        {
            if (resourceTarget.GetRescource(skillGather * (dPS / 10), out takeResult)) //if target has resources; do damage to target and out itemsget
            {
                surplus = inventory.AddItem(resourceItem, takeResult);
                storeItem = resourceItem;
                if (surplus > 0) { DropResource(resourceItem, surplus); }
            }
            else //target is empty
            {
                prevResourcePos = resourceTarget.transform.position;
                resourceTarget = null;
            }
        }
    }

    public void SetStoreTarget(Building _target)
    {
        storeTarget = _target;
        stopDistance = _target.interactionRadius;
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
    } //move to inventory class

    public void DropResource(ItemData _item, int _amount) //move to inventory class
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

    public float GetCurrentHealth()
    {
        return stats.currentHealth;
    }

    public void TakeDamage(float dam)
    {
        if(stats.currentHealth < 0) { return; }
        stats.currentHealth -= dam;
        if (stats.currentHealth <= 0) { DestroyObject(); }
    }

    public void DestroyObject()
    {
        //play death anim
        Destroy(gameObject);
    }
}
