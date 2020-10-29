using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[SelectionBase]
public class Path : MonoBehaviour , IStructure
{
    public PathData pathData;

    private GameObject childSingle;
    private GameObject childDeadend;
    private GameObject childCorner;
    private GameObject childStraight;
    private GameObject childSplit;
    private GameObject childCross;
    private GameObject childCornerFill;
    private GameObject childSideFill;
    private GameObject childCenterFill;

    private List<GameObject> children = new List<GameObject>();
    public Dictionary<Vector2, GameObject> neighbours = new Dictionary<Vector2, GameObject>();
    private float gridSize;
    private NavMeshModifier navMod;

    private Dictionary<ItemData, int> buildCost = new Dictionary<ItemData, int>();


    public enum PathShape { Single,Deadend,Corner,Straight,Split,Cross,CornerFill,SideFill,CenterFill}
    private PathShape myShape;
    private void Awake()
    {
        gridSize = WorldManager.gridSize;

        
    }

    void Start()
    {
        AddToGrid();

        childSingle = Instantiate(pathData.single,transform);
        children.Add(childSingle);
        childDeadend = Instantiate(pathData.deadend, transform);
        children.Add(childDeadend);
        childCorner = Instantiate(pathData.corner, transform);
        children.Add(childCorner);
        childStraight = Instantiate(pathData.straight, transform);
        children.Add(childStraight);
        childSplit = Instantiate(pathData.split, transform);
        children.Add(childSplit);
        childCross = Instantiate(pathData.cross, transform);
        children.Add(childCross);
        childCornerFill = Instantiate(pathData.cornerFill, transform);
        children.Add(childCornerFill);
        childSideFill = Instantiate(pathData.sideFill, transform);
        children.Add(childSideFill);
        childCenterFill = Instantiate(pathData.centerFill, transform);
        children.Add(childCenterFill);

        myShape = PathShape.CenterFill;
        SetPathShape(PathShape.Single,0);
        FindNeighbours();
    }

    public void UpdateShape()
    {
        //Debug.Log("update shape");
        if(neighbours != null)
        {
            if (neighbours.ContainsKey(new Vector2(0, 1))) //north
            {
                if (neighbours.ContainsKey(new Vector2(1, 0))) //east
                {
                    if (neighbours.ContainsKey(new Vector2(0, -1))) //south
                    {
                        if (neighbours.ContainsKey(new Vector2(-1, 0))) //west
                        {
                            if (neighbours.ContainsKey(new Vector2(-1, 1)) 
                                || neighbours.ContainsKey(new Vector2(1, 1)) 
                                || neighbours.ContainsKey(new Vector2(1, -1)) 
                                || neighbours.ContainsKey(new Vector2(-1, -1)))
                            {
                                SetPathShape(PathShape.CenterFill,0);
                                return;
                            }
                            SetPathShape(PathShape.Cross,0);
                            return;
                        }
                        if (neighbours.ContainsKey(new Vector2(1, 1)) || neighbours.ContainsKey(new Vector2(1, -1))) //northEast & southEast
                        {
                            SetPathShape(PathShape.SideFill, 180);
                            return;
                        }
                        SetPathShape(PathShape.Split, 180);
                        return;
                    }
                    if (neighbours.ContainsKey(new Vector2(-1, 0))) //west
                    {
                        if (neighbours.ContainsKey(new Vector2(-1, 1)) || neighbours.ContainsKey(new Vector2(1, 1))) //northWest & northEast
                        {
                            SetPathShape(PathShape.SideFill, 90);
                            return;
                        }
                        SetPathShape(PathShape.Split, 90);
                        return;
                    }
                    if (neighbours.ContainsKey(new Vector2(1, 1))) //northEast
                    {
                        SetPathShape(PathShape.CornerFill, 90);
                        return;
                    }
                    SetPathShape(PathShape.Corner, 90);
                    return;
                }
                if (neighbours.ContainsKey(new Vector2(0, -1)))   //south
                {
                    if (neighbours.ContainsKey(new Vector2(-1, 0))) //West
                    {
                        if (neighbours.ContainsKey(new Vector2(-1, 1)) || neighbours.ContainsKey(new Vector2(-1, -1))) //northWest & southWest
                        {
                            SetPathShape(PathShape.SideFill, 0);
                            return;
                        }
                        SetPathShape(PathShape.Split, 0);
                        return;
                    }
                    SetPathShape(PathShape.Straight, 0);
                    return;
                }
                if (neighbours.ContainsKey(new Vector2(-1, 0))) //west
                {
                    if (neighbours.ContainsKey(new Vector2(-1, 1))) //northWest
                    {
                        SetPathShape(PathShape.CornerFill, 0);
                        return;
                    }
                    SetPathShape(PathShape.Corner, 0);
                    return;
                }
                SetPathShape(PathShape.Deadend, 0);
                return;
            }
            if (neighbours.ContainsKey(new Vector2(1, 0))) //east
            {
                if (neighbours.ContainsKey(new Vector2(0, -1))) //south
                {
                    if (neighbours.ContainsKey(new Vector2(-1, 0))) //west
                    {
                        if (neighbours.ContainsKey(new Vector2(1, -1)) || neighbours.ContainsKey(new Vector2(-1, -1))) //southEast & southWest
                        {
                            SetPathShape(PathShape.SideFill, 270);
                            return;
                        }
                        SetPathShape(PathShape.Split, 270);
                        return;
                    }
                    if (neighbours.ContainsKey(new Vector2(1, -1))) //southEast
                    {
                        SetPathShape(PathShape.CornerFill, 180);
                        return;
                    }
                    SetPathShape(PathShape.Corner, 180);
                    return;
                }
                if (neighbours.ContainsKey(new Vector2(-1, 0))) //west
                {
                    SetPathShape(PathShape.Straight, 90);
                    return;
                }
                SetPathShape(PathShape.Deadend, 90);
                return;
            }
            if (neighbours.ContainsKey(new Vector2(0, -1))) //south
            {
                if (neighbours.ContainsKey(new Vector2(-1, 0))) //west
                {
                    if (neighbours.ContainsKey(new Vector2(-1, -1))) //southWest
                    {
                        SetPathShape(PathShape.CornerFill, 270);
                        return;
                    }
                    SetPathShape(PathShape.Corner, 270);
                    return;
                }
                SetPathShape(PathShape.Deadend, 180);
                return;
            }
            if (neighbours.ContainsKey(new Vector2(-1, 0))) //west
            {
                SetPathShape(PathShape.Deadend, 270);
                return;
            }
        }
        SetPathShape(PathShape.Single, 0);
    }

    public void AddNeighbour(Vector2 pos, GameObject neighbour)
    {
        neighbours.Add(pos, neighbour);
    }

    public void FindNeighbours()
    {
        Vector3 myPos = transform.position;
        for (int x = -1; x < 2; x++)
        {
            for (int z = -1; z < 2; z++)
            {
                Vector3 pos = new Vector3(myPos.x + (x * gridSize), myPos.y, myPos.z + (z * gridSize));
                if (CityManager.Instance.cityDict.ContainsKey(pos) )
                {
                    if(CityManager.Instance.cityDict[pos].name == name)
                    {
                        AddNeighbour(new Vector2(x, z), CityManager.Instance.cityDict[pos]);
                    }
                }
            }
        }
        UpdateShape();

        foreach (KeyValuePair<Vector2,GameObject> pair in neighbours)
        {
            if (pair.Key != new Vector2(0,0))
            {
                Vector2 nPos = pair.Key*-1;

                pair.Value.GetComponent<Path>().AddNeighbour(nPos, transform.gameObject);
                pair.Value.GetComponent<Path>().UpdateShape();
            }
        }

    }

    public void SetPathShape(PathShape newShape, float rot)
    {
        if (myShape != newShape)
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].SetActive(false);
            }

            if (newShape == PathShape.Single) { children[0].SetActive(true); children[0].transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0)); }
            if (newShape == PathShape.Deadend) { children[1].SetActive(true); children[1].transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0)); }
            if (newShape == PathShape.Corner) { children[2].SetActive(true); children[2].transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0)); }
            if (newShape == PathShape.Straight) { children[3].SetActive(true); children[3].transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0)); }
            if (newShape == PathShape.Split) { children[4].SetActive(true); children[4].transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0)); }
            if (newShape == PathShape.Cross) { children[5].SetActive(true); children[5].transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0)); }
            if (newShape == PathShape.CornerFill) { children[6].SetActive(true); children[6].transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0)); }
            if (newShape == PathShape.SideFill) { children[7].SetActive(true); children[7].transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0)); }
            if (newShape == PathShape.CenterFill) { children[8].SetActive(true); children[8].transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0)); }
            myShape = newShape;
        }
    }
    public List<GameObject> GetChildList()
    {
        return children;
    }

    public float GetMaxHealth()
    {
        return pathData.MaxHealth;
    }

    public void AddToGrid()
    {
        Chunk chunk = WorldManager.Instance.GetChunk(transform.position);
        chunk.SetGridPointContent(new Vector2(transform.position.x, transform.position.z), Point.PointContent.Path);
        CityManager.Instance.AddConstruct(transform.position, gameObject);
    }

    public void RemoveFromGrid()
    {
        Chunk chunk = WorldManager.Instance.GetChunk(transform.position);
        chunk.SetGridPointContent(new Vector2(transform.position.x, transform.position.z), Point.PointContent.Empty);
        CityManager.Instance.RemoveConstruct(transform.position);
    }

    public Dictionary<ItemData, int> GetBuildDict()
    {
        buildCost.Clear();
        for (int i = 0; i < pathData.buildItemList.Count; i++)
        {
            buildCost.Add(pathData.buildItemList[i], pathData.buildItemAmountList[i]);
        }
        return buildCost;
    }
}
