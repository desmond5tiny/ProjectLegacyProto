using UnityEngine.AI;

public class MoveToStorageState : IState
{
    private readonly Unit unit;
    private NavMeshAgent agent;

    public MoveToStorageState(Unit _unit, NavMeshAgent _agent)
    {
        unit = _unit;
        agent = _agent;
    }

    public void OnEnter()
    {
        agent.SetDestination(unit.storeTarget.transform.position);
    }

    public void OnExit()
    {    }

    public void Tick()
    {    }
}
