using UnityEngine;

public class HarvestResourceState : IState
{
    private PlayerUnit unit;
    private UnitStats stats;

    private ResourceObject target;
    private float harvestSpeed;
    private float harvestStrength;

    private float nextHarvest;

    public HarvestResourceState(PlayerUnit _unit) //add _harvestSpeed based on unit skills&attributes
    {
        unit = _unit;
    }
    public void Tick()
    {
        if (unit.resourceTarget != null)
        {
            if(nextHarvest <= (Time.time + harvestSpeed))
            {
                int takeResult, surplus;
                if (unit.inventory.CanAdd(unit.resourceItem)) //redundant?
                {
                    if (target.GetRescource(harvestStrength, out takeResult)) //if target has resources; do damage to target and out itemsget
                    {
                        surplus = unit.inventory.AddItem(unit.resourceItem, takeResult);
                        unit.storeItem = unit.resourceItem;
                        if (surplus > 0) { unit.DropResource(unit.resourceItem, surplus); }
                    }
                    else //target is empty
                    {
                        unit.resourceTarget = null;
                    }
                }
                nextHarvest += harvestSpeed;
            }
        }
    }

    public void OnEnter()
    {
        nextHarvest = Time.time;
        stats = unit.stats;
        target = unit.resourceTarget;
        harvestSpeed = 3 - stats.GetSkillLevel("Ga") * 0.1f - (stats.attStrength + stats.attEndurance) * 0.025f;
        harvestStrength = stats.attStrength * 1.2f + (stats.GetSkillLevel("Ga") * 1.5f) + (stats.attEndurance * 0.5f);
    }

    public void OnExit()
    {    }

}
