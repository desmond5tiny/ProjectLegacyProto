using UnityEngine;

[CreateAssetMenu(fileName = "New Builing", menuName = "Building")]
public class BuildingData : ScriptableObject
{
    public new string name;
    public int maxHealth;
    public int capacity;
    public int storage;
    public int storagePriority;
    public bool buildBase;
    [Space]
    public bool produceItems;

    [Space]
    public int TileSizeX;
    public int TileSizeZ;

    [Header("Path Prefabs")]
    public GameObject buildingFloor;
    public GameObject buildingMain;
}
