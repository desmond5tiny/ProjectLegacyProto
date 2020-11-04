using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : IState
{
    private PlayerUnit unit;
    private float buildSpeed;

    private float nextBuild;

    public BuildState(PlayerUnit _unit, float _buildSkill)
    {
        unit = _unit;
        buildSpeed = 1 - (_buildSkill / 10);
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
