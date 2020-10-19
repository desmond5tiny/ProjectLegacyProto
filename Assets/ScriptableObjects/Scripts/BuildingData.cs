using UnityEngine;

[CreateAssetMenu(fileName = "New Builing", menuName = "Building")]
public class BuildingData : ScriptableObject
{
    [Header("Stats")]
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
}
