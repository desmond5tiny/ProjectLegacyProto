using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }
    public int unitsToSpawn;

    [SerializeField]
    private GameObject unitBase;
    [SerializeReference]
    public Transform UnitSpawnPoint;
    public static Action<PlayerUnit> PopulationIncreased;
    public static Action<PlayerUnit> PopulationDecreased;

    private InputManager inputManager;
    public Dictionary<int, GameObject> unitDict { get; private set; }

    private void Awake()
    {
        if (Instance == null){ Instance = this; }
        else { Debug.LogError("more then once instance of UnitManager found!"); }
    }

    private void OnEnable()
    {
        InputManager.OnRightMouseUp += MoveUnits;
    }
    private void OnDisable()
    {
        InputManager.OnRightMouseUp -= MoveUnits;
    }

    void Start()
    {
        inputManager = InputManager.Instance;
        unitDict = new Dictionary<int, GameObject>();
        StartSpawn(UnitSpawnPoint.position, unitsToSpawn);
    }

    public void AddUnit(GameObject _unit)
    {
        int id = _unit.GetInstanceID();
        if (!(unitDict.ContainsKey(id)))
        {
            unitDict.Add(id, _unit);
            PopulationIncreased?.Invoke(_unit.GetComponent<PlayerUnit>());
        }
    }

    public void RemoveUnit(GameObject _unit)
    {
        var id = _unit.GetInstanceID();
        if(unitDict.ContainsKey(id))
        {
            unitDict.Remove(id);
            PopulationDecreased?.Invoke(_unit.GetComponent<PlayerUnit>());
        }
    }

    public GameObject GetUnit(int id)
    {
        return unitDict[id];
    }

    public void SpawnRandomUnit(Vector3 _target)
    {
        GameObject newUnit = Instantiate(unitBase, _target, Quaternion.identity);
        newUnit.GetComponent<PlayerUnit>().Initialize(DnaGenerator.CreateDna(), UnityEngine.Random.Range(18,30));
        AddUnit(newUnit);
    }

    public void StartSpawn(Vector3 spawnPoint, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * 2;
            SpawnRandomUnit(new Vector3(spawnPoint.x + randomPoint.x, spawnPoint.y, spawnPoint.z + randomPoint.y));
        }
    }

    
    public string GetOffspringDna(GameObject _parentA, GameObject _parentB)
    {
        string dnaA = _parentA.GetComponent<PlayerUnit>().stats.dna;
        string dnaB = _parentB.GetComponent<PlayerUnit>().stats.dna;

        return DnaGenerator.CreateDna(dnaA, dnaB);
    }

    public void MoveUnits()
    {
        var hitResult = inputManager.RaycastAll();
        if (hitResult.transform.gameObject.layer == 8 && GlobalSelection.Instance.selectionDictionary.selectedDict != null)
        {
            Vector3 target = hitResult.point;
            int i = 0;
            foreach (KeyValuePair<int,GameObject> pair in GlobalSelection.Instance.selectionDictionary.selectedDict)
            {
                GameObject unit = GlobalSelection.Instance.selectionDictionary.selectedDict.ElementAt(i).Value;
                PlayerUnit unitMove = unit.GetComponent<PlayerUnit>();

                unitMove.MoveTo(target, 0.4f);
                i++;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(UnitSpawnPoint.position.x, UnitSpawnPoint.position.y+0.6f, UnitSpawnPoint.position.z), 0.5f);
    }
}
