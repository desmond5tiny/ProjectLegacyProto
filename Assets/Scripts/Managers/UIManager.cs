using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject buildMenu;
    public GameObject villagersMenu;
    public GameObject stockpileMenu;
    public GameObject characterMenu;

    private List<GameObject> toggleMenus = new List<GameObject>();

    public enum leftMenu { Build, Command, Villagers}

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Debug.LogError("more then once instance of UIManager found!"); }

        toggleMenus.Add(buildMenu);
        toggleMenus.Add(villagersMenu);
        toggleMenus.Add(stockpileMenu);
        toggleMenus.Add(characterMenu);
    }

    public void ToggleBuildMenu()
    {
        closeToggleMenus(buildMenu);
        buildMenu.SetActive(!buildMenu.activeSelf);
    }

    public void ToggleVillagersMenu()
    {
        closeToggleMenus(villagersMenu);
        villagersMenu.SetActive(!villagersMenu.activeSelf);
    }

    public void ToggleStockpileMenu()
    {
        closeToggleMenus(stockpileMenu);
        stockpileMenu.SetActive(!stockpileMenu.activeSelf);
    }
    public void ToggleCharacterMenu()
    {
        closeToggleMenus(characterMenu);
        characterMenu.SetActive(!characterMenu.activeSelf);
    }

    private void closeToggleMenus(GameObject _currentToggleMenu)
    {
        for (int i = 0; i < toggleMenus.Count; i++)
        {
            if (toggleMenus[i] != _currentToggleMenu)
            {
                if(toggleMenus[i].activeSelf == true) { toggleMenus[i].SetActive(false); }
            }
        }
    }
}
