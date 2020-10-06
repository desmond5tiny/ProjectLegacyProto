﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUI : MonoBehaviour
{
    #region Singleton
    public static BuildUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Debug.LogError("more then once instance of BuildUI found!"); }
    }
    #endregion

    private InputManager inputManager;
    private ConstructionManager constructionManager;
    private CityManager cityManager;
    void Start()
    {
        inputManager = InputManager.Instance;
        constructionManager = ConstructionManager.Instance;
        cityManager = CityManager.Instance;
    }

    public void SendToConstructM(ConstructionManager.ConstructType constructType, ScriptableObject constructData)
    {
        if (!inputManager.buildMode)
        {
            inputManager.buildMode = true;
            constructionManager.PreviewPlacement(constructType,constructData);
        }
    }

}
