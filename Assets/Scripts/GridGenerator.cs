using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Vector2Int mapSize = new Vector2Int(1,1);
    private Point[,] gridMap;
    public bool drawDebug = false;
    public Map map;

    public void SetGridMap()
    {
        CreateEmptyGrid();

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

        AddNatureLayer();

        map.SetGrid(gridMap);
    }

    public void AddNatureLayer()
    {
        //Go over all object in nature layer and set !Buildable for tiles
        
    }

    public void CreateEmptyGrid()
    {
        gridMap = new Point[mapSize.x, mapSize.y];

        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                Point gridPoint = new Point();
                gridPoint.contains = Point.PointContent.Empty;
                gridPoint.buildHeight = 0;
                gridPoint.buildable = false;
                gridMap[i, j] = gridPoint;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (drawDebug)
        {
            Vector3 startPoint = new Vector3(0 - (WorldManager.gridSize / 2),0, 0 - (WorldManager.gridSize / 2));
            Vector3 endPoint = new Vector3(mapSize.x * WorldManager.gridSize - (WorldManager.gridSize / 2), 0, mapSize.y * WorldManager.gridSize - (WorldManager.gridSize / 2));

            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(startPoint.x, .1f, startPoint.z), new Vector3(endPoint.x, .1f, startPoint.z));
            Gizmos.DrawLine(new Vector3(endPoint.x, .1f, startPoint.z), new Vector3(endPoint.x, .1f, endPoint.z));
            Gizmos.DrawLine(new Vector3(endPoint.x, .1f, endPoint.z), new Vector3(startPoint.x, .1f, endPoint.z));
            Gizmos.DrawLine(new Vector3(startPoint.x, .1f, endPoint.z), new Vector3(startPoint.x, .1f, startPoint.z));
        }
    }

    public void drawPoints()
    {
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                if (gridMap[i,j].buildable)
                {
                    Debug.DrawLine(new Vector3(i * WorldManager.gridSize, gridMap[i, j].buildHeight, j * WorldManager.gridSize), new Vector3(i * WorldManager.gridSize, gridMap[i, j].buildHeight + 2, j * WorldManager.gridSize), Color.green, 10);
                }
                else
                {
                    Debug.DrawLine(new Vector3(i * WorldManager.gridSize, gridMap[i, j].buildHeight, j * WorldManager.gridSize), new Vector3(i * WorldManager.gridSize, gridMap[i, j].buildHeight + 2, j * WorldManager.gridSize), Color.red, 10);
                }
            }
        }
    }

}

[CustomEditor(typeof(GridGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        GridGenerator mapGen = (GridGenerator)target;

        mapGen.map = (Map)EditorGUILayout.ObjectField("Map: ", mapGen.map, typeof(Map), true);
        if (mapGen.map != null) 
        {
            mapGen.mapSize = mapGen.map.mapSize;
            EditorGUILayout.LabelField("Map Size: " + mapGen.mapSize.x + "," + mapGen.mapSize.y); 
        }
        else { EditorGUILayout.LabelField("Map Size: No Map!"); }

        //mapGen.mapSize = EditorGUILayout.Vector2IntField("Map Size", mapGen.mapSize);

        if (GUILayout.Button("Create Grid")) { mapGen.SetGridMap(); }

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        mapGen.drawDebug = EditorGUILayout.Toggle("Draw Debug", mapGen.drawDebug, GUILayout.MaxWidth(150));
        if(GUILayout.Button("Show Points")) { mapGen.drawPoints(); }
        EditorGUILayout.EndHorizontal();

    }

}