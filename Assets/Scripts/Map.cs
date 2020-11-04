using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class Map : MonoBehaviour
{
    public Vector2Int mapSize = Vector2Int.zero;
    private NavMeshSurface navSurface;
    private Point[,] gridMap;

    private void Awake()
    {
        //gridMap = new Point[mapSize.x, mapSize.y];
        if (navSurface == null) { navSurface = GetComponent<NavMeshSurface>(); }
    }

    public void SetGridPointContent(Vector2 pos, Point.PointContent newContent)
    {
        int posX = Mathf.FloorToInt((pos.x - transform.position.x) / WorldManager.gridSize);
        int posY = Mathf.FloorToInt((pos.y - transform.position.z) / WorldManager.gridSize);
        gridMap[posX, posY].contains = newContent;

        if (newContent == Point.PointContent.Path) { gridMap[posX, posY].buildable = false; }
        if (newContent == Point.PointContent.Building) { gridMap[posX, posY].buildable = false; }
        if (newContent == Point.PointContent.Tree) { gridMap[posX, posY].buildable = false; }
        if (newContent == Point.PointContent.Empty) { gridMap[posX, posY].buildable = true; }

    }
    public Point GetPoint(Vector2 pos)
    {
        int posX = Mathf.FloorToInt((pos.x - transform.position.x) / WorldManager.gridSize);
        int posY = Mathf.FloorToInt((pos.y - transform.position.z) / WorldManager.gridSize);
        return gridMap[posX, posY];
    }

    public void NavMeshUpdate()
    {
        navSurface.BuildNavMesh();
    }

    public void SetGrid(Point[,] _gridMap)
    {
        gridMap = _gridMap;
    }

    public Point[,] getGrid()
    {
        return gridMap;
    }

}
