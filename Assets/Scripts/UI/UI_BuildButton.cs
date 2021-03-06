﻿using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BuildButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject constructPrefab;
    public ConstructionManager.ConstructType construcType;
    public bool isLocked=false;
    [Space]
    public GameObject costPanel;
    public TextMeshProUGUI textPrefab;

    private UI_BuildMenu buildUI;
    private UnityEngine.UI.Button button;
    private Dictionary<ItemData, int> buildCost = new Dictionary<ItemData, int>();

    private void Awake() => button = transform.GetComponent<UnityEngine.UI.Button>();

    private void Start()
    {
        buildUI = UI_BuildMenu.Instance;
        if (isLocked) { button.interactable = false; }
        buildCost = constructPrefab.GetComponent<IStructure>().GetBuildDict();
        FillCostPanel();
        costPanel.gameObject.SetActive(false);
    }

    public void SendConstruct()
    {
        buildUI.ResetConstructManager();
        if (!isLocked)
        {
            buildUI.SendToConstructM(construcType, constructPrefab, buildCost);
        }
    }

    public void Unlock() => button.interactable = true;

    public void OnPointerEnter(PointerEventData eventData) //shows the cost of buildiing
    {
        if (!isLocked) { costPanel.gameObject.SetActive(true); }
    }

    public void OnPointerExit(PointerEventData eventData) //hides the cost of buildiing
    {
        costPanel.gameObject.SetActive(false);
    }

    public void FillCostPanel()
    {       
        if(buildCost == null || buildCost.Count == 0)
        {
            TextMeshProUGUI costInfo = Instantiate(textPrefab, costPanel.transform);
            costInfo.transform.parent = costPanel.transform;
            costInfo.text = "No Cost";
        }
        else
        {
            for (int i = 0; i < buildCost.Count; i++)
            {
                TextMeshProUGUI costInfo = Instantiate(textPrefab, costPanel.transform);
                costInfo.transform.parent = costPanel.transform;
            
                costInfo.text = buildCost.ElementAt(i).Key.name + ": " + buildCost.ElementAt(i).Value.ToString();

            }
        }
    }
}
