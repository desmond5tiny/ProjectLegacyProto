using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Building : MonoBehaviour
{
    public BuildingData buildingData;

    private GameObject buildingFloor;
    private GameObject buildingMain;
    [HideInInspector]
    public int currentHealth;

    List<Item> storageList;

    void Start()
    {
        buildingFloor = Instantiate(buildingData.buildingFloor, transform);
        if (buildingData.buildingMain!=null)
        {
            buildingMain = Instantiate(buildingData.buildingMain, transform);
        }
        currentHealth = buildingData.maxHealth;

        storageList = new List<Item>();
    }

    
    public bool StoreItem(Item newItem)
    {
        if (storageList.Count<buildingData.storage)
        {

            return true;
        }
        else { return false; }
    }

    public Item GetItem(string itemName, int amount)
    {
        Item returnItem = null;
        for (int i = 0; i < storageList.Count; i++)
        {
            if (storageList[i].name == itemName)
            {
                returnItem = storageList[i];
            }
        }
        return returnItem;
    }

    public void RemoveItem(Item item)
    {

    }

    public void SetInhabitant(Unit newHabitant)
    {
        //add unit as living here
    }
}
