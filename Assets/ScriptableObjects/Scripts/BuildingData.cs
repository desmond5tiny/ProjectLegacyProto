using UnityEngine;

[CreateAssetMenu(fileName = "New Builing", menuName = "Building")]
public class BuildingData : ScriptableObject
{
    public new string name;
    public int maxHealth;
    public int capacity;
    public int maxStorage;
    public int storagePriority;
    public bool buildBase;
    [Space]
    public bool produceItems;

    [Space]
    [Header("Techical")]
    public int TileSizeX;
    public int TileSizeZ;

    public float interactionRadius;

    [Header("Path Prefabs")]
    public GameObject buildingFloor;
    public GameObject buildingMain;
}
