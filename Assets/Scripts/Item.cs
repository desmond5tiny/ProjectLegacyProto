﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Item : Interactable
{
    public ItemData itemData;

    [HideInInspector]
    public int currentStackSize=1;

    private GameObject basePrefab;
    private GameObject stackMPrefab;
    private GameObject stackLPrefab;

    void Start()
    {
        name = itemData.name;
        basePrefab = Instantiate(itemData.basePrefab, transform);

        if (itemData.maxStackSize > 1)
        {
            stackMPrefab = Instantiate(itemData.stackMPrefab, transform);
            stackMPrefab.SetActive(false);
            stackLPrefab = Instantiate(itemData.StackLPrefab, transform);
            stackLPrefab.SetActive(false);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z),0.5f);
    }

    public void SetItemData(ItemData data)
    {
        if (itemData == null)
        {
            itemData = data;
        }
    }

    public void Inspect()
    {

    }

    public void IUnitInteract()
    {

    }
}
