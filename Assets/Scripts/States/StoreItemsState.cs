public class StoreItemsState : IState
{
    private readonly Unit unit;

    public StoreItemsState(Unit _unit)
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
