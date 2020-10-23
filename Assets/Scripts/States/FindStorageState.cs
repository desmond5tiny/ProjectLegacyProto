﻿using UnityEngine;

public class FindStorageState : IState
{
    private Unit unit;
    private ItemData item;

    public FindStorageState(Unit _unit)
    {
        unit = _unit;
    }
    public void OnEnter()
    {
        if (CityManager.Instance.GetStorageBuilding(unit.GetStoreItem())!=null)
        {
            unit.SetStoreTarget(CityManager.Instance.GetStorageBuilding(unit.GetStoreItem()));
            
        }
        else { Debug.Log("Find Tile in cityLimits to place item on"); }
    }

    public void OnExit()
    {    }

    public void Tick()
    {    }
}
