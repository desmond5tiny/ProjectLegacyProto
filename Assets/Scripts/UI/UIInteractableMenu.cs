using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Interactable))]
public class UIInteractableMenu : MonoBehaviour
{
    public Transform taskMenu;
    //public Transform taskPanel;
    //public Button roundButton;
    public Interactable interactable;

    private InputManager inputManager;
    private UnitManager unitManager;
    private bool menuActive = false;

    private void Awake() => DisableMenu();
    private void OnMouseEnter()
    {
        InputManager.OnRightMouseUp += EnableMenu;
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
        unitManager = UnitManager.Instance;
    }

    private void Update()
    {
        if (menuActive)
        {
            //look to camera
        }

        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        if (!menuActive) { DisableMenu(); }
    }

    public void SetTaskButtons() //dynamically fill the taskpanel with buttons
    {
        //grab all script with ITask interface component

        //add button for each task to taskPanel
            //grab icon from taskScript
    }

    public void EnableMenu()
    {
        taskMenu.gameObject.SetActive(true);
        menuActive = true;
    }

    public void DisableMenu()
    {
        taskMenu.gameObject.SetActive(false);
        menuActive = false;
    }

    private void OnMouseExit()
    {
        menuActive = false;
        InputManager.OnRightMouseUp -= EnableMenu;
    }
}
