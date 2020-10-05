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

    private WorldManager worldManager;
    private CityManager cityManager;
    private LayerMask groundLayer;
    private float gridSize = 2.5f;
    private Chunk currentChunk;
    private Vector2 currentPoint = new Vector2(0,0);
    private Vector2 prevPoint = new Vector2(0, 0);
    private Vector3 buildPos;
    private Point.PointContent pointFillType;
    private bool canBuild=true;

    private bool preview = false;
    [HideInInspector]
    public bool dragBuild = false;
    private bool matSet = false;
    private GameObject construct;
    private GameObject previewConstruct;
    public enum ConstructType { Building, Path, Wall}

    private void Start()
    {
        worldManager = WorldManager.Instance;
        cityManager = CityManager.Instance;
        groundLayer = InputManager.Instance.groundLayer;
        gridSize = worldManager.gridSize;
        currentChunk = worldManager.sceneChunk;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        if (!matSet && previewConstruct != null) { SetPreviewMat(previewConstruct); }

        if (preview)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 50000.0f, groundLayer);
            currentPoint = new Vector2(Mathf.RoundToInt(hit.point.x/gridSize), Mathf.RoundToInt(hit.point.z/gridSize))*gridSize;
            Vector2 hitChunkPos = new Vector2(Mathf.FloorToInt(currentPoint.x / worldManager.chunkSize), Mathf.FloorToInt(currentPoint.y / worldManager.chunkSize)) * worldManager.chunkSize;


            if (currentPoint != prevPoint)
            {
                canBuild = currentChunk.GetPoint(currentPoint).buildable;
                //Debug.Log(canBuild +" at: " + currentPoint);

                buildPos = new Vector3(currentPoint.x, currentChunk.GetPoint(currentPoint).buildHeight, currentPoint.y);
                previewConstruct.transform.position = buildPos;

                if (dragBuild && canBuild)
                {
                   PlaceContruct();
                }

                if (new Vector2(currentChunk.transform.position.x, currentChunk.transform.position.z) != hitChunkPos)
                {
                    currentChunk = worldManager.GetChunk(hit.transform.position);
                }

                prevPoint = currentPoint;
            }
        }
    }

    public void PreviewPlacement( ConstructType strucType, ScriptableObject structData)
    {
        if (strucType == ConstructType.Path)
        {
            PathData newPathData = ScriptableObject.CreateInstance(typeof(PathData)) as PathData;
            
            if (structData is PathData)
            {
                newPathData = structData as PathData;
            }
            else { Debug.LogError("structData is not a PathData"); }
            
            construct = new GameObject(structData.name);
            construct.AddComponent<Path>().pathData = newPathData;
            construct.SetActive(false);
            pointFillType = Point.PointContent.Path;
        }
        if (strucType == ConstructType.Building)
        {
            BuildingData newBuildingData = ScriptableObject.CreateInstance(typeof(BuildingData)) as BuildingData;

            if (structData is BuildingData)
            {
                newBuildingData = structData as BuildingData;
            }
            else { Debug.LogError("structData is not a BuildingData"); }

            construct = new GameObject(structData.name);
            construct.AddComponent<Building>().buildingData = newBuildingData;
            construct.SetActive(false);
            pointFillType = Point.PointContent.Building;
        }

        matSet = false;

        previewConstruct = Instantiate(construct);
        previewConstruct.name = "PreviewObject";
        previewConstruct.SetActive(true);
        SetPreviewMat(previewConstruct);

        preview = true;
    }

    public void StopPreview()
    {
        Destroy(previewConstruct);
        Destroy(construct);
        preview = false;
    }

    public void PlaceContruct()
    {
        //canBuild = currentChunk.GetPoint(currentPoint).buildable;
        if (canBuild)
        {
            //Debug.Log("build: " + construct.name);
            GameObject newConstruct = Instantiate(construct);
            newConstruct.name = construct.name;
            newConstruct.transform.position = buildPos;
            newConstruct.SetActive(true);

            cityManager.AddConstruct(buildPos,newConstruct);
            currentChunk.SetGridPointContent(new Vector2(buildPos.x,buildPos.z), pointFillType);
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
