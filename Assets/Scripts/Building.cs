using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Building : MonoBehaviour
{
    public BuildingData buildingData;

    [HideInInspector]
    public int currentHealth;

    void Start()
    {
        currentHealth = buildingData.maxHealth;
    }

    void Update()
    {
        
    }
}
