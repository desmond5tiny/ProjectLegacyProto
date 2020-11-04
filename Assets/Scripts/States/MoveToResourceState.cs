using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToResourceState : IState
{
    private readonly PlayerUnit unit;
    private NavMeshAgent agent;

    public MoveToResourceState(PlayerUnit _unit, NavMeshAgent _agent)
    {
        unit = _unit;
        agent = _agent;
    }

    public void OnEnter()
    {
        agent.SetDestination(unit.resourceTarget.transform.position);
    }

    public void OnExit()
    {    }

    public void Tick()
    {    }
}
