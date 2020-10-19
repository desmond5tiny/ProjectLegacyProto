using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    #region Singleton
    public static UnitManager Instance { get; private set; }
    public int unitsToSpawn;

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
    public Transform UnitSpawnPoint;

    private InputManager inputManager;
    private Dictionary<int, GameObject> unitDict;

    private void OnEnable()
    {
        InputManager.OnRightMouseUp += MoveUnits;
        InputManager.OnKeyT += TaskUnits;
    }
    void Start()
    {
        inputManager = InputManager.Instance;
        unitDict = new Dictionary<int, GameObject>();

        StartSpawn(UnitSpawnPoint.position, unitsToSpawn);
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

    public void StartSpawn(Vector3 spawnPoint, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * 2;
            SpawnUnit(new Vector3(spawnPoint.x+ randomPoint.x, spawnPoint.y, spawnPoint.z + randomPoint.y));
        }
    }

    public void MoveUnits()
    {
        var hitResult = inputManager.RaycastGround();
        if (hitResult != null && GlobalSelection.Instance.selectionDictionary.selectedDict != null)
        {
            Vector3 target = hitResult.Value;
            //Debug.Log("clickTarget: " + target);
            int i = 0;
            foreach (KeyValuePair<int,GameObject> pair in GlobalSelection.Instance.selectionDictionary.selectedDict)
            {
                GameObject unit = GlobalSelection.Instance.selectionDictionary.selectedDict.ElementAt(i).Value;
                Unit unitMove = unit.GetComponent<Unit>();

                unitMove.MoveTo(target, 0.4f);
                i++;
            }
        }
    }

    public void TaskUnits()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5000.0f))
        {
            if (hit.transform.parent != null && hit.transform.parent.gameObject.GetComponent<ResourceObject>())
            {
                GameObject ParentObject = hit.transform.parent.gameObject;
                //Debug.Log(ParentObject.name);
                int i = 0;
                foreach (KeyValuePair<int, GameObject> pair in GlobalSelection.Instance.selectionDictionary.selectedDict)
                {
                    GameObject unit = GlobalSelection.Instance.selectionDictionary.selectedDict.ElementAt(i).Value;
                    unit.GetComponent<Unit>().SetResourceTarget(ParentObject.GetComponent<ResourceObject>());
                    i++;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(UnitSpawnPoint.position.x, UnitSpawnPoint.position.y+0.6f, UnitSpawnPoint.position.z), 0.5f);
    }
}
