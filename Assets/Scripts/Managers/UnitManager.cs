using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    #region Singleton
    public static UnitManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Debug.LogError("more then once instance of UnitManager found!"); }
    }
    #endregion

    [SerializeField]
    private GameObject unitBase;

    private Dictionary<int, GameObject> unitDict;

    void Start()
    {
        unitDict = new Dictionary<int, GameObject>();
    }

    void Update()
    {
        
    }

    public void AddUnit(GameObject newUnit)
    {
        int id = newUnit.GetInstanceID();
        if (!(unitDict.ContainsKey(id)))
        {
            unitDict.Add(id, newUnit);
            //increase population
        }
    }

    public void RemoveUnit(int id)
    {
        unitDict.Remove(id);
        //decrease population
    }

    public GameObject GetUnit(int id)
    {
        return unitDict[id];
    }

    public void SpawnUnit(Vector3 target)
    {
        GameObject newUnit = Instantiate(unitBase);
        newUnit.transform.position = target;
        AddUnit(newUnit);
    }

    public void MoveUnits(Vector3 target)
    {
        int i = 0;
        foreach (KeyValuePair<int,GameObject> pair in GlobalSelection.Instance.selectionDictionary.selectedDict)
        {

            GameObject unit = GlobalSelection.Instance.selectionDictionary.selectedDict.ElementAt(i).Value;
            Unit unitMove = unit.GetComponent<Unit>();
            unitMove.move(target);
            i++;
        }
    }
}
