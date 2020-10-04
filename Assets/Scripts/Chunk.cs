using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class Chunk : MonoBehaviour
{
    private NavMeshSurface surface;
    private Point[,] pointArray;

    void Start()
    {
        surface = GetComponent<NavMeshSurface>();

        pointArray = new Point[20, 20];
        for (int x = 0; x < 20; x++)
        {
            for (int z = 0; z < 20; z++)
            {
                Point newPoint = new Point();
                pointArray[x, z] = newPoint;
            }
        }
    }

    public void NavMeshUpdate()
    {
        surface.BuildNavMesh();
    }
}

public class Point
{
    public enum pointContent { Empty, Building, Path, Tree, Rock}
    pointContent contains;



}
