using UnityEngine;

[CreateAssetMenu(fileName = "New Builing", menuName = "Building")]
public class BuildingData : ScriptableObject
{
    public new string name;
    public int maxHealth;
    public int capacity;
    public int storage;
    public bool buildBase;
    [Space]
    public bool produceItems;

    [Space]
    public int TileSizeX;
    public int TileSizeZ;

    //prefabs
}
