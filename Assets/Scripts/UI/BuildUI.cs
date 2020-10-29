using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void SendToConstructM(ConstructionManager.ConstructType _constructType, GameObject _constructPrefab, Dictionary<ItemData, int> _buildCost)
    {
        if (inputManager.GetInputMode() != InputManager.InputMode.BuildMode)
        {
            if (checkAvailableResources(_buildCost))
            {
                //inputManager.buildMode = true;
                inputManager.SetInputMode(InputManager.InputMode.BuildMode);
                constructionManager.SetConstructPrefab(_constructType, _constructPrefab);
            }
        }
    }

    public bool checkAvailableResources(Dictionary<ItemData,int> _resources)
    {
        for (int i = 0; i < _resources.Count; i++)
        {
            if(cityManager.GetStockpileItem(_resources.ElementAt(i).Key) < _resources.ElementAt(i).Value)
            {
                return false;
            }
            
        }
        return true;
    }

    public void ResetConstructManager()
    {
        constructionManager.StopPreview();
        //inputManager.buildMode = false;
        inputManager.SetInputMode(InputManager.InputMode.SelectMode);
    }

}
