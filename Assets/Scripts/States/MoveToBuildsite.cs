using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToBuildsite : IState
{
    private Unit unit;
    private NavMeshAgent agent;

    public MoveToBuildsite(Unit _unit, NavMeshAgent _agent)
    {
        unit = _unit;
        agent = _agent;
    }

    public void OnEnter()
    {
        agent.SetDestination(unit.buildTarget.transform.position);
    }

    public void OnExit()
    {    }

    public void Tick()
    {    }
}
