using System.Collections.Generic;
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

    public GameObject buildFence;

    private InputManager inputManager;
    private Map worldMap;
    private LayerMask groundLayer;
    private float gridSize = 2.5f;

    private Vector2 currentPos = new Vector2(0,0);
    private Vector2 prevPos = new Vector2(0, 0);
    private Vector3 buildPos;
    private float xOffset = 0, zOffset = 0;
    private Vector2 buildArea;

    private bool canBuild=true;
    private bool preview = false;
    [HideInInspector]
    private bool matSet = false;

    private GameObject constructPrefab;
    private GameObject previewConstruct;

    public enum ConstructType { Building, Path, Wall}
    private ConstructType currentType;

    private void OnEnable()
    {
        InputManager.OnLeftMouseClick += PlaceBuildSite;
        InputManager.OnRightMouseUp += StopPreview;
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
        groundLayer = inputManager.groundLayer;
        gridSize = WorldManager.gridSize;
        worldMap = WorldManager.GetMap();
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

            //if(currentPos.x < 0 || currentPos.y< 0 || currentPos.x > worldMap.mapSize.x * gridSize || currentPos.y > worldMap.mapSize.y * gridSize) { return; } //return of mouse is out of map bounds

            if (currentPos != prevPos) //only executes when the mouse goes to a new gridpoint
            {
                if(previewConstruct == null) { SetConstructPrefab(); }

                buildPos = new Vector3(currentPos.x, worldMap.GetPoint(currentPos).buildHeight, currentPos.y);

                previewConstruct.transform.position = buildPos;

                canBuild = true;
                for (int x = 0; x < buildArea.x; x++)
                {
                    for (int y = 0; y < buildArea.y; y++)
                    {
                        Vector3 pointPos = new Vector3(buildPos.x - (xOffset * (buildArea.x - 1)) + (gridSize * x), buildPos.y, buildPos.z - (zOffset * (buildArea.y - 1)) + (gridSize * y));
                        if(!worldMap.GetPoint(new Vector2(pointPos.x, pointPos.z)).buildable)
                        {
                            canBuild = false;
                        }
                    }
                }
                DragPlace(inputManager.GetDragMode());
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
                PlaceBuildSite(InputManager.InputMode.BuildMode);
            }
        }
    }

    public void StartPreview(ConstructType _strucType, GameObject _constructPrefab)
    {
        constructPrefab = _constructPrefab;
        currentType = _strucType;
        preview = true;
    }

    public void SetConstructPrefab()
    {
        xOffset = 0;
        zOffset = 0;
        buildArea = new Vector2(1, 1);

        if (currentType == ConstructType.Building)
        {
            BuildingData newBuildingData = constructPrefab.GetComponent<Building>().buildingData;

            if (newBuildingData.TileSizeX % 2 == 0) { xOffset = gridSize / 2; }
            if (newBuildingData.TileSizeZ % 2 == 0) { zOffset = gridSize / 2; }
            buildArea = new Vector2(newBuildingData.TileSizeX, newBuildingData.TileSizeZ); //used for checking if the area is empty 
        }

        matSet = false;
        previewConstruct = Instantiate(constructPrefab);
        previewConstruct.name = "PreviewObject";
        previewConstruct.SetActive(true);
        SetPreviewMat(previewConstruct);
    }

    public void StopPreview()
    {
        if (preview)
        {
            Destroy(previewConstruct);
            preview = false;
        }
    }

    public void PlaceBuildSite(InputManager.InputMode mode)
    {
        if (canBuild && mode == InputManager.InputMode.BuildMode)
        {
            GameObject buildSite = Instantiate(buildFence);
            buildSite.transform.position = buildPos;
            buildSite.GetComponent<BuildFence>().Initialize( buildArea, gridSize, constructPrefab, worldMap, currentType);
            buildSite.SetActive(true);
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
