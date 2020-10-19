using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Building : Interactable
{
    public BuildingData buildingData;

    private GameObject buildingFloor;
    private GameObject buildingMain;
    [HideInInspector]
    public int currentHealth;
    public Inventory storage;

    public static Action<ItemData,int> ItemStored;
    public static Action ItemTaken;

    private void Awake()
    {
        storage = new Inventory(buildingData.maxStorage);

        interactionRadius = buildingData.interactionRadius;
        if (interactionTransform == null) { interactionTransform = transform; }
    }

    void Start()
    {
        currentHealth = buildingData.maxHealth;
    }

    public void SetInhabitant(Unit newHabitant)
    {
        //add unit as living here
    }
    
    public int GetStorageSpaceLeft()
    {
        return (buildingData.maxStorage - storage.container.Count);
    }

    public bool StoreItem(ItemData item, int amount, out int result)
    {
        result = 0;
        if (storage.CanAdd(item))
        {
            result = storage.AddItem(item, amount);
            ItemStored?.Invoke(item, amount - result);
            Debug.Log("Add "+ item + ": " + (amount-result));
            return true;
        }
        else { return false; }
    }

    /*public Item GetItem(string itemName, int amount)
    {
        ItemTaken?.Invoke();
        return returnItem;
    }*/

    /*public void RemoveItem(Item item)
    {

    }*/

    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null) { interactionTransform = transform; }
        if (interactionRadius != buildingData.interactionRadius) { interactionRadius = buildingData.interactionRadius; }
        Gizmos.color = Color.yellow;
        if (interactionTransform != transform) { Gizmos.DrawSphere(interactionTransform.position, 0.2f); }
        Gizmos.DrawWireSphere(interactionTransform.position, interactionRadius);
    }

}
