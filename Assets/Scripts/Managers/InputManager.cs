﻿using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    #region Singleton
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Debug.LogError("more then once instance of InputManager found!"); }
    }
    #endregion

    public LayerMask groundLayer;

    private Vector3 mouseDownClickPos;
    private Vector3 mouseUpClickPos;
    [HideInInspector]
    public bool buildMode;
    private bool dragSelect;

    void Start()
    {
        dragSelect = false;
        buildMode = false;
    }

    void Update()
    {
        //if (EventSystem.current.IsPointerOverGameObject()) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            mouseDownClickPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) 
        {
            if ((mouseDownClickPos - Input.mousePosition).magnitude > 30)
            {
                dragSelect = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseUpClickPos = Input.mousePosition;

            if (!buildMode)
            {
                GlobalSelection.Instance.MakeSelection(dragSelect, mouseDownClickPos, mouseUpClickPos);
            }
            dragSelect = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (!buildMode && GlobalSelection.Instance.selectionDictionary.selectedDict != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, 5000.0f, groundLayer);

                UnitManager.Instance.MoveUnits(hit.point);
            }
        }

        if (Input.GetKeyDown(KeyCode.U)) //temp spawn units
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 5000.0f,groundLayer);
            UnitManager.Instance.SpawnUnit(hit.point);
        }
    }

    /*private void OnGUI()
    {
        if (dragSelect)
        {
            var rect = Utils.GetScreenRect(p1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }*/
}
