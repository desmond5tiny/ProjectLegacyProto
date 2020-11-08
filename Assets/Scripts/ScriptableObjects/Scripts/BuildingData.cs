using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Builing", menuName = "Building")]
public class BuildingData : ScriptableObject
{
    public new string name;
    [Header("Stats")]
    public int maxHealth;
    [Tooltip("The amount of people that can live here")]
    public int capacity;
    public int maxStorage;
    public int storagePriority;
    [Tooltip("This determines if building can be build on top of this building")]
    public bool buildBase;

    [Space]
    [Header("Techical")]
    public int TileSizeX;
    public int TileSizeZ;
    public float interactionRadius;


    //production
    [HideInInspector]
    public bool ShowProduction=false;
    //[HideInInspector]
    public bool produce;
    [HideInInspector]
    public ItemData product;
    //[HideInInspector]
    public List<ItemData> produceItems = new List<ItemData>();


    //build cost
    [HideInInspector]
    public bool showBuildList=false;
    [HideInInspector]
    public ItemData buildItem;
    //[HideInInspector]
    public List<ItemData> buildItemList = new List<ItemData>();
    //[HideInInspector]
    public List<int> buildItemAmountList = new List<int>();



    public void AddProduceItem() => produceItems.Add(product);

    public void RemoveProduceItem()
    {
        if (produceItems.Count > 1)
        {
            produceItems.RemoveAt(produceItems.Count - 1);
        }
    }
}

public class BuildCostEntry
{
    public ItemData costItem;
    public int costAmount;
}
