using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stockpile : MonoBehaviour
{
    private Dictionary<string, int> itemDict = new Dictionary<string, int>();
    public static Action StockpileChanged;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Building.ItemStored += AddItem;
        Building.ItemTaken += RemoveItem;
    }
    private void OnDisable()
    {
        Building.ItemStored -= AddItem;
        Building.ItemTaken -= RemoveItem;
    }
    private void Initialize()
    {
        itemDict.Add("Wooden Logs", 0);
        itemDict.Add("Stone Blocks", 0);
    }

    public void AddItem(ItemData _item, int _amount)
    {
        if (itemDict.ContainsKey(_item.name)) { itemDict[_item.name] += _amount; }
        else { itemDict.Add(_item.name, _amount); }
        StockpileChanged?.Invoke();
    }

    public void RemoveItem(ItemData _item, int _amount)
    {
        if(!itemDict.ContainsKey(_item.name) || itemDict[_item.name] < _amount)
        {
            Debug.LogError("Insufficient Amount Available!");
            return;
        }
        itemDict[_item.name] -= _amount;
        StockpileChanged?.Invoke();
    }

    public bool CheckItemAmount(ItemData _item, int _amount)
    {
        if(itemDict.ContainsKey(_item.name))
        {
            if(itemDict[_item.name] >= _amount) { return true; }
        }
        return false;
    }

    public int GetItemAmount(ItemData _item)
    {
        if (itemDict.ContainsKey(_item.name)) { return itemDict[_item.name]; }
        return 0;
    }
}
