using System.Collections;
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

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
