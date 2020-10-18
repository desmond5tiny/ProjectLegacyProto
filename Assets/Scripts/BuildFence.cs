using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildFence : Interactable
{
    public GameObject prefab1TileS;
    public GameObject prefab1TileL;
    public GameObject prefab2Tile;

    [HideInInspector]
    public float currentHealth;
    private float maxHealth;

    //building prefab

    private void Awake()
    {
        SetSize(1);

        currentHealth = 1;
        task = "Build";
    }

    public void SetSize(int _size)
    {
        prefab1TileS.SetActive(false);
        prefab1TileL.SetActive(false);
        prefab2Tile.SetActive(false);
        if (_size == 1) { prefab1TileS.SetActive(true); }
        if (_size == 2) { prefab1TileL.SetActive(true); }
        if (_size == 3) { prefab2Tile.SetActive(true); }
        if(_size == 0 || _size > 3) { Debug.LogError("No Such Size!"); }
    }

    public void AddHealth(float _addHealth)
    {
        currentHealth += _addHealth;

        if (currentHealth >= maxHealth) { ConstructionCompleet(); }
    }

    private void ConstructionCompleet()
    {
        //create new gameobject instatiating building prefab
        Destroy(this);
    }

    public void SetHealth(float _maxHealth) => maxHealth = _maxHealth;

}
