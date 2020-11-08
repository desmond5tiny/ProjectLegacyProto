using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }
    public static float gridSize = 2.5f;

    private Vector2 mapSize;
    [SerializeField]
    private Map currentMap;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else {Debug.LogError("more then once instance of WorldManager found!");}

        mapSize = currentMap.mapSize;
        if (currentMap.getGrid() != null)
        {
            Debug.Log(currentMap.getGrid().Length);
            if(currentMap.getGrid().Length < mapSize.x * mapSize.y) { currentMap.ClearGrid(); }
        }
        if (currentMap.getGrid() == null) { GridGenerator.SetGridMap(currentMap); }
    }

    void Start()
    {
        currentMap.NavMeshUpdate();
    }

    public static Map GetMap()
    {
        return WorldManager.Instance.currentMap;
    }

    IEnumerator AddTrees()//temp method
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Resource"); ;
        int i = 0;
        foreach (GameObject tree in trees)
        {
            //Debug.Log("found tree at: " + trees[i].transform.position.x +","+ trees[i].transform.position.z);
            currentMap.SetGridPointContent(new Vector2(trees[i].transform.position.x, trees[i].transform.position.z), Point.PointContent.Resource);
            i++;
        }
        yield return null ;
        currentMap.NavMeshUpdate();
        StopCoroutine("AddTrees");
    }

}
