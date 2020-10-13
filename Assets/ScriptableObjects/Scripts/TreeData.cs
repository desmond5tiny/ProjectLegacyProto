using UnityEngine;

[CreateAssetMenu(fileName = "New Tree", menuName = "Tree")]
public class TreeData : ScriptableObject
{
    public new string name;
    public int maxHealth;
    public ItemData woodLogsData;
    public int maxWoodAmount;

    public float stopRadius = 2f;

    [Header("Prefabs")]
    public GameObject treeBase;
    public GameObject treeTop;
}
