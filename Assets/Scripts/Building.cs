using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Building : MonoBehaviour
{
    public BuildingData buildingData;

    private GameObject buildingFloor;
    private GameObject buildingMain;
    [HideInInspector]
    public int currentHealth;

    void Start()
    {
        buildingFloor = Instantiate(buildingData.buildingFloor, transform);
        if (buildingData.buildingMain!=null)
        {
            buildingMain = Instantiate(buildingData.buildingMain, transform);
        }
        currentHealth = buildingData.maxHealth;
    }

    void Update()
    {
        
    }
}
