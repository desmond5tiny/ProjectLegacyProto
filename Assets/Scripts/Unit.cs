using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    public int inventorySize = 0;
    public Inventory inventory;

    public float dPS;
    public float chopSpeed;
    private float totalDamage = 0;
    public enum UnitState { Idle, ExecutingTask, MovingToTarget, Storing}
    UnitState unitState;

    Dictionary<GameObject,string > taskDict = new Dictionary<GameObject, string>();

    private Vector3 destination;
    Action DoTask;
    private NavMeshAgent agent;
    private GameObject taskTarget;
    Interactable targetIMethod;

    string currentTask;

    int resourceReturn=0;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        unitState = UnitState.Idle;
        inventory = new Inventory(inventorySize);
    }

    void Update()
    {
        switch(unitState)
        {
            case UnitState.Idle:
                if (currentTask == null && taskDict.Count > 0)
                {
                    SetTask();
                    MoveTo(taskTarget.transform.position, taskTarget.GetComponent<Interactable>().interactionRadius +4.0f);
                }
                    break;
            case UnitState.MovingToTarget:
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (currentTask != null) { unitState = UnitState.ExecutingTask; }
                    else { unitState = UnitState.Idle; }
                }
                break;
            case UnitState.ExecutingTask:
                if (currentTask!= null)
                {
                    DoTask.Invoke();
                }
                break;
            case UnitState.Storing:

                break;
        }
    }


    public void AddTask(GameObject newTarget)
    {
        taskDict.Add(newTarget, newTarget.GetComponent<Interactable>().UnitInteract());
    }

    public void SetTask()
    {
        if (currentTask == null)
        {
            currentTask = taskDict.ElementAt(0).Value;
            taskTarget = taskDict.ElementAt(0).Key;
            targetIMethod = taskTarget.GetComponent<Interactable>();
        }

        if(currentTask == "ChopTree") 
        { 
            DoTask += ChopTree; 

            //change to set next resource target, only stop when canceled
            targetIMethod.EndTask += StopTask; 
        }
    }

    public void Build()
    {
        Debug.Log("Buil");
    }

    public void ChopTree()
    {
        float doDamage = 0;
        int resourceGet;
        int surplus=0;
        ItemData resourceData = taskTarget.GetComponent<IResource>().GetResourceData();
        if (Input.GetKeyDown(KeyCode.K)) { doDamage = 5; }

        if (doDamage >= 1 && inventory.CanAdd(resourceData))
        {
            if(taskTarget.GetComponent<IResource>().GetRescource(doDamage, out resourceGet))
            {
                if(!inventory.AddItem(resourceData, resourceGet, out surplus))
                {
                    Debug.Log("inventory full");
                    DropResource(resourceData, surplus);
                    //store cargo
                }

                /*Debug.Log("surplusWood: " + surplusWood);
                for (int i = 0; i < inventory.container.Count; i++)
                {
                    Debug.Log("Inv "+i+": "+inventory.container[i].amount);
                }*/
            }
            else //if inv not full
            {
                Debug.Log("Change Tree Target");
                //change target
            }
        }
    }


    public void MoveTo(Vector3 destination, float stopDis)
    {
        agent.stoppingDistance = stopDis;
        agent.SetDestination(destination);
        unitState = UnitState.MovingToTarget;
    }

    public void StopTask(GameObject key)
    {
        Debug.Log("stoptask");
        taskDict.Remove(key);

        currentTask = null;
        targetIMethod.EndTask = null;
        taskTarget = null;
        DoTask = null;
        totalDamage = 0;
        agent.ResetPath();
        unitState = UnitState.Idle;
    }

    public void DropResource(ItemData item, int amount)
    {
        GameObject newResourceItem = new GameObject(item.name);
        newResourceItem.AddComponent<Item>().SetItemData(item);
        for (int i = 0; i < Mathf.FloorToInt(amount /item.maxStackSize)+1; i++)
        {
            if (amount > item.maxStackSize)
            {
                newResourceItem.GetComponent<Item>().currentStackSize = item.maxStackSize;
                Instantiate(newResourceItem);
                amount -= item.maxStackSize;
            }
            else if(amount > 0)
            {
                newResourceItem.GetComponent<Item>().currentStackSize = amount;
                Instantiate(newResourceItem);
                amount = 0;
            }
        }
    }
}
