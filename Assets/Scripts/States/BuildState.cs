using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : IState
{
    private PlayerUnit unit;
    private UnitStats stats;
    private BuildFence target;
    private float buildSpeed;
    private float buildStrength;

    private float nextBuild;

    public BuildState(PlayerUnit _unit)
    {
        unit = _unit;
    }

    public void Tick()
    {
        if (unit.buildTarget != null)
        {
            if (nextBuild <= Time.time + buildSpeed)
            {
                target.AddHealth(buildStrength);
                nextBuild += buildSpeed; 
            }
        }
    }

    public void OnEnter()
    {
        nextBuild = Time.time;
        stats = unit.stats;
        target = unit.buildTarget;
        buildSpeed = 3 - stats.GetSkillLevel("Bu") * 0.1f - (stats.attDexterity + stats.attIntelligence) * 0.025f;
        buildStrength = 10 + stats.GetSkillLevel("Bu") * 10 + ((stats.attDexterity * 0.5f) + stats.attStrength) * 0.1f;
    }

    public void OnExit()
    {    }

}
