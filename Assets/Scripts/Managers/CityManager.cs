using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    #region Singleton
    public static CityManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Debug.LogError("more then once instance of CityManager found!"); }
    }
    #endregion

    public Dictionary<Vector3, GameObject> cityDict = new Dictionary<Vector3, GameObject>();

    public Dictionary<string, int> stockPile = new Dictionary<string, int>();

    public List<Building> storeableBuildings = new List<Building>();

    //temp
    public GameObject Camp;

    private void OnEnable()
    {
        Building.ItemTaken += SortStorageBuildings;
    }
    private void Start()
    {
        AddConstruct(Camp.transform.position, Camp); // temp
    }

    public void AddConstruct(Vector3 pos, GameObject newConstruct)
    {
        cityDict.Add(pos, newConstruct);

        if (newConstruct.GetComponent<Building>().buildingData.storagePriority > 0)
        {
            storeableBuildings.Add(newConstruct.GetComponent<Building>());
            storeableBuildings.Sort(SortByStorePriority);
        }
        //Debug.Log("add path at: " + pos);
    }

    public void SortStorageBuildings() => storeableBuildings.Sort(SortBySpaceLeft);

    public Building GetStorageBuilding(ItemData _storeItem)
    {
        for (int i = 0; i < storeableBuildings.Count; i++)
        {
            if (storeableBuildings[i].storage.CanAdd(_storeItem))
            {
                return storeableBuildings[i];
            }
        }
        return null;
    }

    public int SortByStorePriority(Building a, Building b)
    {
        if (a.buildingData.storagePriority < b.buildingData.storagePriority)
        {
            return -1;
        }
        else if (a.buildingData.storagePriority > b.buildingData.storagePriority)
        {
            return 1;
        }
        else
            return 0;
    }

    public int SortBySpaceLeft(Building a, Building b)
    {
        if (a.buildingData.storagePriority == b.buildingData.storagePriority)
        {
            if (a.GetStorageSpaceLeft() < b.GetStorageSpaceLeft())
            {
                return -1;
            }
            else if (a.GetStorageSpaceLeft() > b.GetStorageSpaceLeft())
            {
                return 1;
            }
            else
                return 0;
        }
        else
            return 0;
    }

}
