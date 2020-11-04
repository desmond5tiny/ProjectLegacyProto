using UnityEngine;
using UnityEngine.AI;

public class MoveToPointState : IState
{
    private readonly PlayerUnit unit;
    private NavMeshAgent agent;
    private Vector3 lastPos = Vector3.zero;
    public float stuckTime;

    public MoveToPointState(PlayerUnit _unit, NavMeshAgent _agent)
    {
        unit = _unit;
        agent = _agent;
    }

    public void Tick()
    {
        //Debug.Log(Vector3.Distance(unit.transform.position, unit.clickTarget));
        if (Vector3.Distance(unit.transform.position, lastPos) <= 0f) { stuckTime += Time.deltaTime; }

        lastPos = unit.transform.position;
    }

    public void OnEnter()
    {
        stuckTime = 0f;
        agent.SetDestination(unit.clickTarget);
        unit.clickMove = false;
        unit.moving = true;
    }

    public void OnExit()
    {
        unit.moving = false;
    }


}
