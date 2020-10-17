using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<InventorySlot> container = new List<InventorySlot>();
    private readonly int maxSize;
    private bool isFull;

    public Inventory(int _maxSize)
    {
        maxSize = _maxSize;
        isFull = false;
    }

    public int AddItem(ItemData item, int amount)
    {
        int surplus = 0;
        for (int i = 0; i < container.Count; i++)
        {
            if (container[i].item == item && container[i].amount < item.maxStackSize) //if it contains item and itemStack isn't full
            {
                container[i].AddAmount(amount);
                if (container[i].amount > item.maxStackSize) //if stack is larger then max size
                {
                    amount = container[i].amount - item.maxStackSize;
                    container[i].RemoveAmount(container[i].amount - item.maxStackSize);
                }
                else { amount = 0; }
            }
        }
        surplus = amount;
        
        for (int i = 0; i < Mathf.FloorToInt(surplus/item.maxStackSize)+1; i++)
        {
            if (amount > 0 && container.Count < maxSize)
            {
                if (amount > item.maxStackSize)
                {
                    container.Add(new InventorySlot(item, item.maxStackSize));
                    amount -= item.maxStackSize;
                }
                else
                {
                    container.Add(new InventorySlot(item, amount));
                    amount = 0;
                }
            }
        }
        surplus = amount;
        return surplus;
    }

    public bool CanAdd(ItemData item) //return true if the item can be added to inv
    {
        if (container.Count < maxSize) { return true; }

        for (int i = 0; i < container.Count; i++)
        {
            if (container[i].item == item && container[i].amount < item.maxStackSize) { return true; }
        }
        return false;
    }

    public bool RemoveItem(ItemData _item, int _amount)
    {
        if(_amount > _item.maxStackSize) { Debug.LogError("Amount Exceeds MaxStackSize"); return false; }

        bool hasAmount = false;
        for (int i = 0; i < container.Count; i++)
        {
            if (container[i].item == _item)
            {
                if (container[i].amount >= _amount)
                {
                    container[i].RemoveAmount(_amount);
                    if (container[i].amount <= 0) { container.RemoveAt(i); }
                    hasAmount = true;
                }
            }
        }
        return hasAmount;
    }
    
    public ItemData RemoveFirstStack(out int amount)
    {
        amount = container[0].amount;
        ItemData firstItem = container[0].item;
        container.RemoveAt(0);
        return firstItem;
    }

    public bool GetIsFull()
    {
        return isFull;
    }

    public void CheckIfFull()
    {
        isFull = true;
        if (container.Count < maxSize) { isFull = false; }
        else
        {
            for (int i = 0; i < container.Count; i++)
            {
                if (container[i].amount < container[i].item.maxStackSize)
                {
                    isFull = false;
                }
            }
        }
    }

    public void DropAllItems()
    {
        //empty inventory
    }
}

public class InventorySlot
{
    public ItemData item;
    public int amount;
    public InventorySlot(ItemData _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int addAmount)
    {
        amount += addAmount;    
    }
    public void RemoveAmount(int remAmount)
    {
        amount -= remAmount;
    }
}
