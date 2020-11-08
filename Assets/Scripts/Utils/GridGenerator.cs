using UnityEngine;

public class GridGenerator
{
    public static void SetGridMap(Map _map)
    {
        Vector2Int mapSize = _map.mapSize;

        Point[,] gridMap = GridGenerator.CreateEmptyGrid(mapSize);

        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        for (int i = 0; i < tiles.Length; i++)
        {
            var tilePos = tiles[i].transform.position;
            if (tilePos.x >= 0 && tilePos.z >= 0 && tilePos.x <= mapSize.x * WorldManager.gridSize && tilePos.z <= mapSize.y * WorldManager.gridSize)
            {
                var gridPos = new Vector2Int(Mathf.RoundToInt(tilePos.x / WorldManager.gridSize), Mathf.RoundToInt(tilePos.z / WorldManager.gridSize));
                var gridPoint = gridMap[gridPos.x, gridPos.y];
                gridPoint.pointTile = tiles[i];
                gridPoint.buildHeight = tiles[i].transform.position.y;
                gridPoint.buildable = true;
            }
        }
        GridGenerator.AddNatureLayer(gridMap);

        _map.SetGrid(gridMap);
    }

    private static void AddNatureLayer(Point[,] _gridMap)
    {
        GameObject[] resources = GameObject.FindGameObjectsWithTag("Resource");

        int x = 0;
        foreach (var resource in resources)
        {
            var gridPos = new Vector2Int(Mathf.FloorToInt(resource.transform.position.x / WorldManager.gridSize), Mathf.FloorToInt(resource.transform.position.z / WorldManager.gridSize));
            _gridMap[gridPos.x, gridPos.y].contains = Point.PointContent.Resource;
            x++;
        }
    }

    public static Point[,] CreateEmptyGrid(Vector2Int _mapSize)
    {
        Point[,] gridMap = new Point[_mapSize.x, _mapSize.y];

        for (int i = 0; i < _mapSize.x; i++)
        {
            for (int j = 0; j < _mapSize.y; j++)
            {
                Point gridPoint = new Point();
                gridPoint.contains = Point.PointContent.Empty;
                gridPoint.buildHeight = 0;
                gridPoint.buildable = false;
                gridMap[i, j] = gridPoint;
            }
        }
        return gridMap;
    }
}