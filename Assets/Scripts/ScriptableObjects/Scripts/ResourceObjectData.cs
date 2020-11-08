using UnityEngine;

[CreateAssetMenu(fileName = "New Tree", menuName = "Tree")]
public class ResourceObjectData : ScriptableObject
{
    public new string name;
    public int maxHealth;
    public ItemData resourceItemData;
    public int maxItemAmount;

    public float interactRadius = 2f;

    [Header("Prefabs")]
    public GameObject prefabBase;
    public GameObject prefabTop;
}
