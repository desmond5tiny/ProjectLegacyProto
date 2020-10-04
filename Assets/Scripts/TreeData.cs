using UnityEngine;

[CreateAssetMenu(fileName = "New Tree", menuName = "Tree")]
public class TreeData : ScriptableObject
{
    public new string name;
    public int maxHealth;
    public int maxWoodAmount;

    [Header("Prefabs")]
    public GameObject treeBase;
    public GameObject treeTop;
}
