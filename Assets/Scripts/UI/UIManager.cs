using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject villagersMenu;
    public GameObject stockpileMenu;
    public GameObject characterMenu;

    [Header("Build Windows")]
    public GameObject bld_houseWindow;
    public GameObject bld_productionWindow;
    public GameObject bld_researchMenu;
    public GameObject bld_pathWindow;
    public GameObject bld_specialWindow;


    private List<GameObject> toggleWindows = new List<GameObject>();

    public enum leftMenu { Build, Command, Villagers}

    public static UIManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Debug.LogError("more then once instance of UIManager found!"); }

        MakeToggleList();
    }

    private void MakeToggleList()
    {
        toggleWindows.Add(villagersMenu);
        toggleWindows.Add(characterMenu);

        toggleWindows.Add(bld_houseWindow);
        toggleWindows.Add(bld_productionWindow);
        toggleWindows.Add(bld_researchMenu);
        toggleWindows.Add(bld_pathWindow);
        toggleWindows.Add(bld_specialWindow);
    }

    public void ToggleSingleWindow(GameObject _window)
    {
        _window.SetActive(!_window.activeSelf);
    }

    public void ToggleGroupWindow(GameObject _window)
    {
        for (int i = 0; i < toggleWindows.Count; i++)
        {
            if (toggleWindows[i].activeSelf && toggleWindows[i] != _window)
            {
                toggleWindows[i].SetActive(false);
            }
        }
        ToggleSingleWindow(_window);
    }

    private void closeToggleMenus(GameObject _currentToggleMenu)
    {
        for (int i = 0; i < toggleWindows.Count; i++)
        {
            if (toggleWindows[i] != _currentToggleMenu)
            {
                if(toggleWindows[i].activeSelf == true) { toggleWindows[i].SetActive(false); }
            }
        }
    }
    
    public void AddToToggleList(GameObject _window)
    {
        toggleWindows.Add(_window);
    }
}
