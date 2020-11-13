using System;
using System.Collections;
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
    public static Action<int> PopulationChanged;

    private InputManager inputManager;
    private Dictionary<int, GameObject> unitDict;

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

    public void AddUnit(GameObject _newUnit)
    {
        int id = _newUnit.GetInstanceID();
        if (!(unitDict.ContainsKey(id)))
        {
            unitDict.Add(id, _newUnit);
            PopulationChanged?.Invoke(unitDict.Count);
        }
    }

    public void RemoveUnit(int id)
    {
        if(unitDict.ContainsKey(id))
        {
            unitDict.Remove(id);
            PopulationChanged?.Invoke(unitDict.Count);
        }
    }

    public GameObject GetUnit(int id)
    {
        return unitDict[id];
    }

    public void SpawnUnit(Vector3 _target)
    {
        GameObject newUnit = Instantiate(unitBase, _target, Quaternion.identity);
        newUnit.GetComponent<PlayerUnit>().Initialize(GetRandomDna());
        AddUnit(newUnit);
    }

    public void StartSpawn(Vector3 spawnPoint, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * 2;
            //SpawnUnit(new Vector3(spawnPoint.x + randomPoint.x, spawnPoint.y, spawnPoint.z + randomPoint.y));
            SpawnUnit(new Vector3(spawnPoint.x, spawnPoint.y, spawnPoint.z));
        }
    }

    
    public string GetRandomDna()
    {
        Debug.Log(DnaGenerator.CreateDna());
        return ("a121231m0203040303052Bu1Ga22Ma1Iv2");
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
