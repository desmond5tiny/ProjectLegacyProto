using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Building : Interactable, IStructure
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
        //AddToGrid();
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

    public float GetMaxHealth()
    {
        return buildingData.maxHealth;
    }

    public void AddToGrid()
    {
        Vector3 pos = transform.position;
        float gridSize = WorldManager.gridSize;
        Chunk chunk = WorldManager.Instance.GetChunk(transform.position);

        float offsetX = ((buildingData.TileSizeX + 1) % 2) * (gridSize / 2);
        float offsetZ = ((buildingData.TileSizeZ + 1) % 2) * (gridSize / 2);

        for (int i = 0; i < buildingData.TileSizeX; i++)
        {
            for (int j = 0; j < buildingData.TileSizeZ; j++)
            {
                Vector3 pointPos = new Vector3(pos.x - ((Mathf.FloorToInt((buildingData.TileSizeX - 1) / 2) * gridSize) + offsetX) + (gridSize * i), pos.y,
                                                pos.z - ((Mathf.FloorToInt((buildingData.TileSizeZ - 1) / 2) * gridSize) + offsetZ) + (gridSize * j));
                chunk.SetGridPointContent(new Vector2(pointPos.x, pointPos.z), Point.PointContent.Building);
            }
        }
        CityManager.Instance.AddConstruct(transform.position, gameObject);
    }

    public void RemoveFromGrid()
    {
        throw new NotImplementedException();
    }
}
