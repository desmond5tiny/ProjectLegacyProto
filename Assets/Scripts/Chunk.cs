using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class Chunk : MonoBehaviour
{
    private NavMeshSurface surface;
    private Point[,] pointArray = new Point[20,20];
    public float gridSize;

    void Start()
    {
        surface = GetComponent<NavMeshSurface>();
        //SetGrid();
    }

    public void SetGridPointContent(Vector2 pos,Point.PointContent newContent)
    {

        int posX = Mathf.RoundToInt((pos.x - transform.position.x) / gridSize);
        int posY = Mathf.RoundToInt((pos.y - transform.position.z) / gridSize); ;

        pointArray[posX,posY].contains = newContent;
        Debug.Log("added: "+pointArray[posX, posY].contains + " at: " + posX + "," + posY);
    }
    public Point GetPoint(Vector2 pos)
    {
        int posX = Mathf.RoundToInt((pos.x - transform.position.x) / gridSize);
        int posY = Mathf.RoundToInt((pos.y - transform.position.z) / gridSize);
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
                pointArray[x, z] = gridPoint;
            }
        }
    }
}
