using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : IState
{
    private PlayerUnit unit;
    private UnitStats stats;
    private float buildSpeed;

    private float nextBuild;

    public BuildState(PlayerUnit _unit)
    {
        unit = _unit;
        stats = unit.stats;
    }
    public void Tick()
    {
        if (unit.buildTarget != null)
        {
            if (nextBuild <= Time.time + buildSpeed)
            {
                unit.Build();
                nextBuild += buildSpeed; 
            }
        }
    }

    public void OnEnter()
    {
        nextBuild = Time.time;
    }

    public void OnExit()
    {    }

}
