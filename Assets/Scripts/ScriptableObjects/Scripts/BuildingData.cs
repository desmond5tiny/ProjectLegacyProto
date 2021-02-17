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
    [Tooltip("The maximum bumber of buildings can exist of this type; 0 means infinite.")]
    public int maxAmount = 0;

    [Header("Techical")]
    public int TileSizeX;
    public int TileSizeZ;
    public bool buildBase;
    [Tooltip("This determines if building can be build on top of this building")]
    public float interactionRadius;

    //production
    [HideInInspector]
    public bool ShowProduction=false;
    [Header("Production")]
    public bool produce;
    [HideInInspector]
    public ItemData product;
    public List<ItemData> produceItems = new List<ItemData>();


    //public bool showBuildList=false;
    [HideInInspector]
    public ItemData buildItem;

    [Header("Build Cost")]
    public List<ItemData> buildItemList = new List<ItemData>();
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
