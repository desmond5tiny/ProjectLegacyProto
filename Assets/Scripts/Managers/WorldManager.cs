using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }
    public static float gridSize = 2.5f;

    [SerializeField]
    private bool worldChunks = false;
    public Vector2 mapSize;
    public Chunk sceneChunk;

    [SerializeField]
    private Map currentMap;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else {Debug.LogError("more then once instance of WorldManager found!");}
        if(currentMap.getGrid() == null) { GetComponent<GridGenerator>().SetGridMap(); }
        mapSize = currentMap.mapSize;
    }

    void Start()
    {
        StartCoroutine("AddTrees");
    }

    public static Map GetMap()
    {
        return WorldManager.Instance.currentMap;
    }

    IEnumerator AddTrees()//temp method
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree"); ;
        int i = 0;
        foreach (GameObject tree in trees)
        {
            //Debug.Log("found tree at: " + trees[i].transform.position.x +","+ trees[i].transform.position.z);
            currentMap.SetGridPointContent(new Vector2(trees[i].transform.position.x, trees[i].transform.position.z), Point.PointContent.Tree);
            i++;
        }
        yield return null ;
        sceneChunk.NavMeshUpdate();
        StopCoroutine("AddTrees");
    }

}
