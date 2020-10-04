﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    #region Singleton
    public static WorldManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else {Debug.LogError("more then once instance of WorldManager found!");}
    }
    #endregion

    public float gridSize;

    [SerializeField]
    private Chunk sceneChunk;

    private World world;

    void Start()
    {
        world = new World();
        AddChunk();
        AddNature();
    }

    void Update()
    {
        
    }
    public void AddChunk()
    {
        Vector2 chunkPos = new Vector2(sceneChunk.transform.position.x, sceneChunk.transform.position.z);
        world.AddChunk(chunkPos, sceneChunk);
        sceneChunk.gridSize = gridSize;
        sceneChunk.SetGrid();
    }

    void AddNature()//temp method
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");;
        int i = 0;
        foreach (GameObject tree in trees)
        {
            //Debug.Log("found tree at: " + trees[i].transform.position.x +","+ trees[i].transform.position.z);
            sceneChunk.SetGridPointContent(new Vector2(trees[i].transform.position.x,trees[i].transform.position.z), Point.PointContent.Tree);
            //Debug.Log(sceneChunk.GetPoint(new Vector2(trees[i].transform.position.x, trees[i].transform.position.z)));
            i++;
        }
    }
}
