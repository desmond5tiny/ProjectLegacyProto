public class IdleState : IState
{
    private readonly PlayerUnit unit;

    public IdleState(PlayerUnit _unit)
    {
        unit = _unit;
    }

    public void Tick()
    {

    }
    public void OnEnter()
    {
        //unit.resourceTarget = null;
    }

    public void OnExit()
    {
    }

}
