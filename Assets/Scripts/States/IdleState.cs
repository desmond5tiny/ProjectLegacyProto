public class IdleState : IState
{
    private readonly Unit unit;

    public IdleState(Unit _unit)
    {
        unit = _unit;
    }

    public void Tick()
    {

    }
    public void OnEnter()
    {
        unit.resourceTarget = null;
    }

    public void OnExit()
    {
    }

}
