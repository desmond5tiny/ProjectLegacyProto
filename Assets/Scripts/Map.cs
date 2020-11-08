using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class Map : MonoBehaviour
{
    public Vector2Int mapSize = Vector2Int.zero;
    public bool showMapSize = false;
    public GameObject NatureContainer;
    private NavMeshSurface navSurface;
    private Point[,] gridMap;

    private void Awake()
    {
        if (navSurface == null) { navSurface = GetComponent<NavMeshSurface>(); }
    }

    public void SetGridPointContent(Vector2 pos, Point.PointContent newContent)
    {
        int posX = Mathf.FloorToInt((pos.x - transform.position.x) / WorldManager.gridSize);
        int posY = Mathf.FloorToInt((pos.y - transform.position.z) / WorldManager.gridSize);
        gridMap[posX, posY].contains = newContent;

        if (newContent == Point.PointContent.Path) { gridMap[posX, posY].buildable = false; }
        if (newContent == Point.PointContent.Building) { gridMap[posX, posY].buildable = false; }
        if (newContent == Point.PointContent.Resource) { gridMap[posX, posY].buildable = false; }
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

    public void ClearGrid()
    {
        gridMap = null;
    }

    private void OnDrawGizmos()
    {
        if (showMapSize)
        {
            Vector3 startPoint = new Vector3(0 - (WorldManager.gridSize / 2), 0, 0 - (WorldManager.gridSize / 2));
            Vector3 endPoint = new Vector3(mapSize.x * WorldManager.gridSize - (WorldManager.gridSize / 2), 0, mapSize.y * WorldManager.gridSize - (WorldManager.gridSize / 2));

            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(startPoint.x, .1f, startPoint.z), new Vector3(endPoint.x, .1f, startPoint.z));
            Gizmos.DrawLine(new Vector3(endPoint.x, .1f, startPoint.z), new Vector3(endPoint.x, .1f, endPoint.z));
            Gizmos.DrawLine(new Vector3(endPoint.x, .1f, endPoint.z), new Vector3(startPoint.x, .1f, endPoint.z));
            Gizmos.DrawLine(new Vector3(startPoint.x, .1f, endPoint.z), new Vector3(startPoint.x, .1f, startPoint.z));
        }
    }

}
