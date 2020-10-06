using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButton : MonoBehaviour
{

    public ScriptableObject constructData;
    public ConstructionManager.ConstructType construcType;

    public bool isLocked=false;

    private BuildUI buildUI;

    private void Start()
    {
        buildUI = BuildUI.Instance;
    }
    public void SendConstruct()
    {
        if (!isLocked)
        {
            buildUI.SendToConstructM(construcType, constructData);
        }
    }

}
