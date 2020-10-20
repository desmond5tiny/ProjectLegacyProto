﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConstructionManager : MonoBehaviour
{
    #region Singleton
    public static ConstructionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Debug.LogError("more then once instance of ContructionManager found!"); }
    }
    #endregion

    [SerializeField]
    private Material previewMat;
    [SerializeField]
    private Camera mainCam;

    private WorldManager worldManager;
    private InputManager inputManager;
    private CityManager cityManager;
    private Chunk currentChunk;
    private LayerMask groundLayer;
    private float gridSize = 2.5f;

    private Vector2 currentPos = new Vector2(0,0);
    private Vector2 prevPos = new Vector2(0, 0);
    private Vector3 buildPos;
    private float xOffset = 0, zOffset = 0;
    private Vector2 buildArea;

    private Point.PointContent pointFillType;
    private bool canBuild=true;
    private bool preview = false;
    [HideInInspector]
    private bool matSet = false;


    private GameObject construct;
    private GameObject previewConstruct;
    public enum ConstructType { Building, Path, Wall}
    private ConstructType currentType;

    private void OnEnable()
    {
        InputManager.OnLeftMouseClick += PlaceContruct;
        InputManager.OnRightMouseUp += StopPreview;
    }

    private void Start()
    {
        worldManager = WorldManager.Instance;
        inputManager = InputManager.Instance;
        cityManager = CityManager.Instance;
        groundLayer = inputManager.groundLayer;
        gridSize = worldManager.gridSize;
        currentChunk = worldManager.sceneChunk;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        if (!matSet && previewConstruct != null) { SetPreviewMat(previewConstruct); }

        if (preview)
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 50000.0f, groundLayer);
            currentPos = new Vector2((Mathf.RoundToInt((hit.point.x - xOffset)/gridSize) * gridSize)+xOffset, (Mathf.RoundToInt((hit.point.z-zOffset)/gridSize) * gridSize)+zOffset);
            Vector2 hitChunkPos = new Vector2(Mathf.FloorToInt(currentPos.x / worldManager.chunkSize), Mathf.FloorToInt(currentPos.y / worldManager.chunkSize)) * worldManager.chunkSize;


            if (currentPos != prevPos)
            {
                buildPos = new Vector3(currentPos.x, currentChunk.GetPoint(currentPos).buildHeight, currentPos.y);

                previewConstruct.transform.position = buildPos;

                canBuild = true;
                for (int x = 0; x < buildArea.x; x++)
                {
                    for (int y = 0; y < buildArea.y; y++)
                    {
                        Vector3 pointPos = new Vector3(buildPos.x - (xOffset * (buildArea.x - 1)) + (gridSize * x), buildPos.y, buildPos.z - (zOffset * (buildArea.y - 1)) + (gridSize * y));
                        //Debug.Log(currentChunk.GetPoint(new Vector2(pointPos.x, pointPos.z)).buildable);
                        if(!currentChunk.GetPoint(new Vector2(pointPos.x, pointPos.z)).buildable)
                        {
                            canBuild = false;
                        }
                    }
                }

                //Debug.Log("can build: " + canBuild);
                DragPlace(inputManager.GetDragMode());

                if (new Vector2(currentChunk.transform.position.x, currentChunk.transform.position.z) != hitChunkPos)
                {
                    currentChunk = worldManager.GetChunk(hit.transform.position);
                }

                prevPos = currentPos;
            }
        }
    }

    public void DragPlace(InputManager.DragMode mode)
    {
        if (mode == InputManager.DragMode.LeftDrag && canBuild)
        {
            if (currentType == ConstructType.Path || currentType == ConstructType.Wall)
            {
                PlaceContruct(InputManager.InputMode.BuildMode);
            }
        }
    }

    public void SetConstruct( ConstructType strucType, GameObject constructPrefab)
    {
        xOffset = 0;
        zOffset = 0;
        buildArea = new Vector2(1, 1);
        if (strucType == ConstructType.Path)
        {
            PathData newPathData = ScriptableObject.CreateInstance(typeof(PathData)) as PathData;
            
            if (constructPrefab.GetComponent<Path>())
            {
                newPathData = constructPrefab.GetComponent<Path>().pathData;
            }
            else { Debug.LogError("structData is not a PathData"); }
            
            construct = new GameObject(constructPrefab.name);
            construct.AddComponent<Path>().pathData = newPathData;
            construct.SetActive(false);
            pointFillType = Point.PointContent.Path;
        }
        if (strucType == ConstructType.Building)
        {
            BuildingData newBuildingData = constructPrefab.GetComponent<Building>().buildingData;

            if (newBuildingData.TileSizeX % 2 == 0) { xOffset = gridSize / 2; }
            if (newBuildingData.TileSizeZ % 2 == 0) { zOffset = gridSize / 2; }
            buildArea = new Vector2(newBuildingData.TileSizeX, newBuildingData.TileSizeZ); //used for checking if the area is empty 

            construct = Instantiate(constructPrefab);
            construct.SetActive(false);
            pointFillType = Point.PointContent.Building;
        }

        matSet = false;

        previewConstruct = Instantiate(construct);
        previewConstruct.name = "PreviewObject";
        previewConstruct.SetActive(true);
        SetPreviewMat(previewConstruct);

        currentType = strucType;
        preview = true;
    }

    public void StopPreview()
    {
        if (preview)
        {
            Destroy(previewConstruct);
            Destroy(construct);
            preview = false;
        }
    }

    public void PlaceContruct(InputManager.InputMode mode)
    {
        //Debug.Log(canBuild + " ," + mode);
        if (canBuild && mode == InputManager.InputMode.BuildMode)
        {
            GameObject newConstruct = Instantiate(construct);
            newConstruct.name = construct.name;
            newConstruct.transform.position = buildPos;
            newConstruct.SetActive(true);

            for (int x = 0; x < buildArea.x; x++)
            {
                for (int y = 0; y < buildArea.y; y++)
                {
                    Vector3 pointPos = new Vector3(buildPos.x - ((Mathf.FloorToInt((buildArea.x - 1) / 2)*gridSize) + xOffset) + (gridSize * x), buildPos.y,
                                                    buildPos.z - ((Mathf.FloorToInt((buildArea.y - 1) / 2)*gridSize) + zOffset) + (gridSize * y));
                    //Debug.DrawLine(new Vector3(pointPos.x, pointPos.y, pointPos.z), new Vector3(pointPos.x, pointPos.y + 6, pointPos.z), Color.red, 60);
                    currentChunk.SetGridPointContent(new Vector2(pointPos.x, pointPos.z), pointFillType);
                    //cityManager.AddConstruct(new Vector3(buildPos.x-(xOffset*(buildArea.x-1)) + (gridSize*x), buildPos.y, buildPos.z-(zOffset*(buildArea.y-1)) + (gridSize*y)), newConstruct);
                }
            }

            cityManager.AddConstruct(buildPos,newConstruct);
            //Debug.DrawLine(buildPos, new Vector3(buildPos.x, buildPos.y + 5, buildPos.z),Color.blue,60);
        }
    }

    private void SetPreviewMat(GameObject parent)
    {

        if (parent.transform.childCount != 0)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Renderer[] childMats = parent.transform.GetChild(i).GetComponentsInChildren<MeshRenderer>();

                foreach (Renderer rend in childMats)
                {
                    Material[] mats = new Material[rend.materials.Length];

                    for (int j = 0; j < rend.materials.Length; j++)
                    {
                        mats[j] = previewMat;
                    }
                    rend.materials = mats;
                }
            }
            matSet = true;
        }
    }
}
