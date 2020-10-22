using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButton : MonoBehaviour
{

    public GameObject constructPrefab;
    public ConstructionManager.ConstructType construcType;

    public bool isLocked=false;

    private BuildUI buildUI;

    private void Start()
    {
        buildUI = BuildUI.Instance;
    }
    public void SendConstruct()
    {
        buildUI.ResetConstructManager();
        if (!isLocked)
        {
            buildUI.SendToConstructM(construcType, constructPrefab);
        }
    }

}
