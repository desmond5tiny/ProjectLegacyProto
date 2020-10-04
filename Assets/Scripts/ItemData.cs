using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemData : ScriptableObject
{
    public new string name;
    public int maxStackSize;
    public bool consumable;
    //effect
    public bool equipable;
    //stats

    [Header("Prefab Objects")]
    public GameObject basePrefab;
    public GameObject stackMPrefab;
    public GameObject StackLPrefab;
}
