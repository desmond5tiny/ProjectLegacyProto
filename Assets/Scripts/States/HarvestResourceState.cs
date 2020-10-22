using UnityEngine;

public class HarvestResourceState : IState
{
    private Unit unit;
    private float harvestSpeed =1; //set to _harvestSpeed

    private float nextHarvest;

    public HarvestResourceState(Unit _unit) //add _harvestSpeed based on unit skills&attributes
    {
        unit = _unit;
    }
    public void Tick()
    {
        if (unit.resourceTarget != null)
        {
            if(nextHarvest <= (Time.time + harvestSpeed))
            {
                unit.TakeResource();
                nextHarvest += harvestSpeed;
            }
        }
    }

    public void OnEnter()
    {
        nextHarvest = Time.time;
    }

    public void OnExit()
    {    }

}
