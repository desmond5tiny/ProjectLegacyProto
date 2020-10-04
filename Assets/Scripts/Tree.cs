using UnityEngine;

[SelectionBase]
public class Tree : MonoBehaviour
{
    [SerializeField]
    private TreeData treeData;
    private GameObject treeBase;
    private GameObject treeTop;

    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public int currentWoodAmount;


    void Start()
    {
        treeBase = Instantiate(treeData.treeBase, transform);
        treeTop = Instantiate(treeData.treeTop, transform);

        currentHealth = treeData.maxHealth;
        currentWoodAmount = treeData.maxWoodAmount;

    }

    void Update()
    {
        
    }

    public void Damage(float dam)
    {
        currentHealth -= dam;
    }

    public void FellTree()
    {
        //tree fall
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y+.8f, transform.position.z), 0.8f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + 2.4f, transform.position.z), 0.8f);
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + 4.0f, transform.position.z), 0.8f);
    }
}
