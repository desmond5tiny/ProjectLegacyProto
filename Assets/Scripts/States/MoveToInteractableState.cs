public class MoveToInteractableState : IState
{
    private readonly Unit unit;
    public MoveToInteractableState(Unit _unit)
    {
        unit = _unit;
    }
    public void Tick()
    {    }

    public void OnEnter()
    {    
        
    }

    public void OnExit()
    {    }

}
