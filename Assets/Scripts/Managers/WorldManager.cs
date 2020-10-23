using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else {Debug.LogError("more then once instance of WorldManager found!");}

        world = new World();
        AddChunk();
    }

    public static float gridSize = 2.5f;
    public float chunkSize = 50;
    public Chunk sceneChunk;

    private World world;

    void Start()
    {
        StartCoroutine("AddTrees");
    }

    void Update()
    {    }

    public void AddChunk()
    {
        Vector2 chunkPos = new Vector2(sceneChunk.transform.position.x, sceneChunk.transform.position.z);
        //Debug.Log("chunk key: " + chunkPos);
        world.AddChunk(chunkPos, sceneChunk);
        sceneChunk.SetGrid();
    }

    public Chunk GetChunk(Vector3 pos)
    {
        Vector2 key = new Vector2(Mathf.FloorToInt(pos.x / chunkSize), Mathf.FloorToInt(pos.z / chunkSize)) *chunkSize;
        //Debug.Log("chunk getchunk:" + key);
        return world.GetChunk(key);
    }

    IEnumerator AddTrees()//temp method
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree"); ;
        int i = 0;
        foreach (GameObject tree in trees)
        {
            //Debug.Log("found tree at: " + trees[i].transform.position.x +","+ trees[i].transform.position.z);
            sceneChunk.SetGridPointContent(new Vector2(trees[i].transform.position.x, trees[i].transform.position.z), Point.PointContent.Tree);
            //sceneChunk.GetPoint(new Vector2(trees[i].transform.position.x, trees[i].transform.position.z)).buildable = false; ;
            i++;
        }
        yield return null ;
        sceneChunk.NavMeshUpdate();
        StopCoroutine("AddTrees");
    }

}
