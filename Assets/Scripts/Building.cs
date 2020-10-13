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


    Inventory storage;

    private void Awake()
    {
        storage = new Inventory(buildingData.storage);

        buildingFloor = Instantiate(buildingData.buildingFloor, transform);
        if (buildingData.buildingMain != null)
        {
            buildingMain = Instantiate(buildingData.buildingMain, transform);
        }
    }

    void Start()
    {
        currentHealth = buildingData.maxHealth;
    }

    
    public int StoreItem(ItemData item, int amount)
    {
        int surplus;
        if (!storage.AddItem(item, amount, out surplus))
        {
            return surplus;
        }
        else { return 0; }
    }

    /*public Item GetItem(string itemName, int amount)
    {
        
        return returnItem;
    }*/

    public void RemoveItem(Item item)
    {

    }

    public void SetInhabitant(Unit newHabitant)
    {
        //add unit as living here
    }
}
