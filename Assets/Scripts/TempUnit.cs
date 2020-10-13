using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
[RequireComponent(typeof(NavMeshAgent))]
public class NewUnit : MonoBehaviour
{
    public int inventorySize = 4;
    private int inventorySpace = 0;
    List<Item> inventory = new List<Item>();

    public float dPS;
    public float chopSpeed;
    private float dealDamage = 0;
    public enum UnitState { Idle, ExecutingTask, MovingToTarget, Storing }
    UnitState unitState;

    Dictionary<GameObject, string> taskDict = new Dictionary<GameObject, string>();

    private Vector3 destination;
    Action DoTask;
    private NavMeshAgent agent;
    private GameObject taskTarget;
    Interactable targetIMethod;

    string currentTask;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        unitState = UnitState.Idle;

        inventorySpace = inventorySize;
    }

    void Update()
    {
        switch (unitState)
        {
            case UnitState.Idle:
                if (currentTask == null && taskDict.Count > 0)
                {
                    SetTask();
                    MoveTo(taskTarget.transform.position, taskTarget.GetComponent<Interactable>().interactionRadius + 4.0f);
                }
                break;
            case UnitState.MovingToTarget:
                //Debug.Log("Moving");
                if (agent.remainingDistance - agent.stoppingDistance <= 0)
                {
                    Debug.Log("setstate Idle");
                    unitState = UnitState.ExecutingTask;
                }
                break;
            case UnitState.ExecutingTask:
                if (currentTask != null)
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

        if (currentTask == "ChopTree") { DoTask += ChopTree; Debug.Log("add choptask"); }
    }

    public void ChopTree()
    {


        //StopTask();
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
        taskTarget = null;
        DoTask = null;
        dealDamage = 0;
        agent.isStopped = true;
        unitState = UnitState.Idle;
    }

    public bool CheckInvForItem(string itemName)
    {
        bool result = false;
        if (inventory != null)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].name == itemName) { result = true; }
            }
        }
        return result;
    }
}
