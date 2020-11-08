using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class Chunk : MonoBehaviour
{
    public Vector2Int chunkSize = Vector2Int.zero;
    [SerializeField]
    private NavMeshSurface surface;
    private Point[,] pointArray; // = new Point[20,20];

    private void Awake()
    {
        pointArray = new Point[chunkSize.x, chunkSize.y];
        if (surface == null) { surface = GetComponent<NavMeshSurface>(); }
    }

    public void SetGridPointContent(Vector2 pos, Point.PointContent newContent)
    {
        int posX = Mathf.FloorToInt((pos.x - transform.position.x) / WorldManager.gridSize);
        int posY = Mathf.FloorToInt((pos.y - transform.position.z) / WorldManager.gridSize);

        pointArray[posX,posY].contains = newContent;

        if (newContent == Point.PointContent.Path) { pointArray[posX, posY].buildable = false; }
        if (newContent == Point.PointContent.Building) { pointArray[posX, posY].buildable = false; }
        if (newContent == Point.PointContent.Resource) { pointArray[posX, posY].buildable = false; }
        if (newContent == Point.PointContent.Empty) { pointArray[posX, posY].buildable = true; }

        //Debug.Log("added: "+pointArray[posX, posY].contains + " at: " + posX + "," + posY);
    }
    public Point GetPoint(Vector2 pos)
    {
        int posX = Mathf.FloorToInt((pos.x - transform.position.x) / WorldManager.gridSize);
        int posY = Mathf.FloorToInt((pos.y - transform.position.z) / WorldManager.gridSize);
        return pointArray[posX, posY];
    }

    public void NavMeshUpdate()
    {
        surface.BuildNavMesh();
    }

    public void SetGrid()
    {
        for (int x = 0; x < 20; x++)
        {
            for (int z = 0; z < 20; z++)
            {
                Point gridPoint = new Point();
                gridPoint.contains = Point.PointContent.Empty;
                gridPoint.buildHeight = 0;
                gridPoint.buildable = true;
                pointArray[x, z] = gridPoint;
            }
        }
    }

}
