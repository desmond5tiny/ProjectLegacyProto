using UnityEngine;

public class FindStorageState : IState
{
    private Unit unit;

    public FindStorageState(Unit _unit)
    {
        unit = _unit;
    }
    public void OnEnter()
    {
        Debug.Log("search storage");
    }

    public void OnExit()
    {    }

    public void Tick()
    {    }
}
