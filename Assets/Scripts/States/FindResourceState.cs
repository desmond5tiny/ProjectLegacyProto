using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindResourceState : IState
{
    private Unit unit;
    private NavMeshAgent agent;
    private Vector3 searchPos;
    private List<ResourceObject> nearByResources = new List<ResourceObject>();

    public FindResourceState(Unit _unit, NavMeshAgent _agent)
    {
        unit = _unit;
        agent = _agent;
    }
    public void Tick()
    {
        if (unit.resourceTarget == null)
        {
            if (Vector3.Distance(unit.transform.position, searchPos) <= 2f)
            {
                nearByResources = unit.GetNearbyResources();
                if (nearByResources.Count > 0)
                {
                    nearByResources.Sort(SortByDistance);
                    unit.SetResourceTarget(nearByResources[0]);
                }
            }
        }
    }

    public void OnEnter()
    {
        searchPos = unit.prevResourcePos;
        //nearByResources.Sort(SortByDistance);
        if (Vector3.Distance(unit.transform.position, searchPos)>2f)
        {
            agent.SetDestination(searchPos);
            agent.stoppingDistance = 2f;
        }
    }

    public void OnExit()
    {    }

    private int SortByDistance(ResourceObject a, ResourceObject b)
    {
        if (Vector3.Distance(unit.transform.position, a.transform.position) < Vector3.Distance(unit.transform.position, b.transform.position))
        {
            return -1;
        }
        else if (Vector3.Distance(unit.transform.position, a.transform.position) > Vector3.Distance(unit.transform.position, b.transform.position))
        {
            return 1;
        }
        else
            return 0;
    }

}
