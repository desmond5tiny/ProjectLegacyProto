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
    public Stockpile stockpile;

    public List<Building> storeableBuildings = new List<Building>();

    //temp
    public GameObject Camp;

    private void OnEnable()
    {
        Building.ItemTaken += SortStorageBuildings;
    }
    private void Start()
    {
        if (stockpile == null) { stockpile = GetComponent<Stockpile>(); } //overly cautious 
        //AddConstruct(Camp.transform.position, Camp);
    }

    public void AddConstruct(Vector3 _pos, GameObject _newConstruct)
    {
        cityDict.Add(_pos, _newConstruct);
        WorldManager.Instance.GetChunk(_newConstruct.transform.position).NavMeshUpdate();

        if (_newConstruct.CompareTag("Building"))
        {
            if (_newConstruct.GetComponent<Building>().buildingData.storagePriority > 0)
            {
                storeableBuildings.Add(_newConstruct.GetComponent<Building>());
                storeableBuildings.Sort(SortByStorePriority);
            }
        }
    }

    public void RemoveConstruct(Vector3 _key)
    {
        if(cityDict.ContainsKey(_key)) { cityDict.Remove(_key); }
        else { Debug.LogError("Construct not found in City!"); }
    }

    public void SortStorageBuildings(ItemData _item, int _amount) => storeableBuildings.Sort(SortBySpaceLeft);

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

    public int GetStockpileItem(ItemData _item)
    {
        return stockpile.GetItemAmount(_item);
    }
}
