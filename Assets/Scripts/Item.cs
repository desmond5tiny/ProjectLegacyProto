﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemData itemData;

    [HideInInspector]
    public int currentStackSize=1;

    private GameObject basePrefab;
    private GameObject stackMPrefab;
    private GameObject stackLPrefab;

    void Start()
    {
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
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z),0.5f);
    }
}
