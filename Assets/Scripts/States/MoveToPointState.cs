using UnityEngine;
using UnityEngine.AI;

public class MoveToPointState : IState
{
    private readonly Unit unit;
    private NavMeshAgent agent;

    public MoveToPointState(Unit _unit, NavMeshAgent _agent)
    {
        unit = _unit;
        agent = _agent;
    }

    public void Tick()
    {
        //Debug.Log(Vector3.Distance(unit.transform.position, unit.clickTarget));
    }

    public void OnEnter()
    {
        agent.SetDestination(unit.clickTarget);
        unit.clickMove = false;
    }

    public void OnExit()
    {
        
    }


}
