using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Building : Interactable, IStructure, ITakeDamage
{
    public BuildingData buildingData;

    private GameObject buildingFloor;
    private GameObject buildingMain;
    [HideInInspector]
    public float currentHealth;
    public int occupancy;
    public Inventory storage;

    public List<PlayerUnit> occupents = new List<PlayerUnit>();

    public static Action<ItemData,int> ItemStored;
    public static Action<ItemData, int> ItemTaken;

    private Dictionary<ItemData, int> buildCost = new Dictionary<ItemData, int>();

    private void Awake()
    {
        occupancy = buildingData.capacity;
        storage = new Inventory(buildingData.maxStorage);
        interactionRadius = buildingData.interactionRadius;
        if (interactionTransform == null) { interactionTransform = transform; }
    }

    void Start()
    {
        currentHealth = buildingData.maxHealth;
        AddToGrid();
    }

    public void AddInhabitant(PlayerUnit _unit)
    {
        if (occupancy > 0)
        {
            occupents.Add(_unit);
            occupancy--;
        }
    }
    
    public void RemoveInhabitant(PlayerUnit _unit)
    {
        occupents.Remove(_unit);
        occupancy++;
    }

    public int GetStorageSpaceLeft()
    {
        return (buildingData.maxStorage - storage.container.Count);
    }

    public bool StoreItem(ItemData _item, int _amount, out int _result)
    {
        _result = 0;
        if (storage.CanAdd(_item))
        {
            _result = storage.AddItem(_item, _amount);
            ItemStored?.Invoke(_item, _amount - _result);
            //Debug.Log("Add "+ _item + ": " + (_amount-_result));
            return true;
        }
        else { return false; }
    }

    /*public Item GetItem(string itemName, int amount)
    {
        ItemTaken?.Invoke();
        return returnItem;
    }*/

    public void RemoveItem(Item _item, int _amout)
    {
        //if(storage.)
    }

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
        SetGridPoints(Point.PointContent.Building);
        CityManager.Instance.AddConstruct(transform.position, gameObject);
    }

    public void RemoveFromGrid()
    {
        SetGridPoints(Point.PointContent.Empty);
        CityManager.Instance.RemoveConstruct(transform.position);
    }

    private void SetGridPoints(Point.PointContent _fill)
    {
        Vector3 pos = transform.position;
        //Debug.Log(pos);
        float gridSize = WorldManager.gridSize;
        Map worldMap = WorldManager.GetMap();

        float offsetX = ((buildingData.TileSizeX + 1) % 2) * (gridSize / 2);
        float offsetZ = ((buildingData.TileSizeZ + 1) % 2) * (gridSize / 2);

        for (int i = 0; i < buildingData.TileSizeX; i++)
        {
            for (int j = 0; j < buildingData.TileSizeZ; j++)
            {
                Vector3 pointPos = new Vector3(pos.x - ((Mathf.FloorToInt((buildingData.TileSizeX - 1) / 2) * gridSize) + offsetX) + (gridSize * i), pos.y,
                                                pos.z - ((Mathf.FloorToInt((buildingData.TileSizeZ - 1) / 2) * gridSize) + offsetZ) + (gridSize * j));
                worldMap.SetGridPointContent(new Vector2(pointPos.x, pointPos.z), _fill);
            }
        }
    }

    public Dictionary<ItemData, int> GetBuildDict()
    {
        buildCost.Clear();
        for (int i = 0; i < buildingData.buildItemList.Count; i++)
        {
            buildCost.Add(buildingData.buildItemList[i], buildingData.buildItemAmountList[i]);
        }
        return buildCost;
    }

    public void TakeDamage(float dam)
    {
        currentHealth -= dam;
        if (currentHealth <= 0) { DestroyObject(); }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void DestroyObject()
    {
        //play destroyed anim
        RemoveFromGrid();
        Destroy(gameObject);
    }
}
