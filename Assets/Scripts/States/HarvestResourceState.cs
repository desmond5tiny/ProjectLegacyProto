using UnityEngine;

public class HarvestResourceState : IState
{
    private Unit unit;
    private float harvestSpeed =1;

    private float nextHarvest;

    public HarvestResourceState(Unit _unit)
    {
        unit = _unit;
    }
    public void Tick()
    {
        if (unit.resourceTarget != null)
        {
            if(nextHarvest <= (Time.time + harvestSpeed))
            {
                nextHarvest += harvestSpeed;
                unit.TakeResource();
            }
        }
    }

    public void OnEnter()
    {
        nextHarvest = Time.time;
    }

    public void OnExit()
    {
        //unit.resourceTarget = null;
    }

}
