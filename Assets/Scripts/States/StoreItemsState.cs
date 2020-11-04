public class StoreItemsState : IState
{
    private readonly PlayerUnit unit;

    public StoreItemsState(PlayerUnit _unit)
    {
        unit = _unit;
    }
    public void Tick()
    {
        unit.StoreResource();
    }

    public void OnEnter()
    {    }

    public void OnExit()
    {    }

}
