using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class ResourceObject : Interactable, ITakeDamage
{
    [SerializeField]
    private TreeData resourceData;
    private GameObject resourceBase;
    private GameObject resourceTop;


    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public int currentResourceAmount;

    private float damagePerDrop;
    private bool isEmpty = false;

    private void Awake()
    {
        resourceBase = Instantiate(resourceData.treeBase, transform);
        resourceTop = Instantiate(resourceData.treeTop, transform);
        InputManager.OnKeyN += KillRescource;

        damagePerDrop = resourceData.maxHealth / resourceData.maxWoodAmount;
    }

    void Start()
    {
        currentHealth = resourceData.maxHealth;
        currentResourceAmount = resourceData.maxWoodAmount;
        interactionRadius = resourceData.stopRadius;
    }

    public void TakeDamage(float dam)
    {
        currentHealth -= dam;
        if (currentHealth <=0)
        {
            isEmpty = true;
            currentHealth = 0;
            Collapse();
            if (currentResourceAmount > 0) { DropResource(currentResourceAmount); }
            Destroy(this, 5);
        }
    }

    public void Collapse()
    {
        resourceTop.SetActive(false);
        Debug.Log("Resource Empty");
    }

    public bool GetRescource(float damage, out int woodDrop) //change to out a list of drop items
    {
        woodDrop = 0;
        if (!isEmpty)
        {
            if(Mathf.CeilToInt((currentHealth - damage) / damagePerDrop) < currentResourceAmount)
            {
                woodDrop = (currentResourceAmount - Mathf.CeilToInt((currentHealth - damage) / damagePerDrop));
                currentResourceAmount -= woodDrop;
            }
            TakeDamage(damage);
            return true;
        }
        return false;
    }

    public void DropResource(int amount)
    {
        int fullStacks = Mathf.FloorToInt(currentResourceAmount / resourceData.woodLogsData.maxStackSize);
        for (int i = 0; i < fullStacks; i++)
        {
            GameObject woodenLogs = new GameObject(resourceData.woodLogsData.name);
            woodenLogs.AddComponent<Item>().SetItemData(resourceData.woodLogsData);
            woodenLogs.transform.position = transform.position;
            woodenLogs.GetComponent<Item>().currentStackSize = resourceData.woodLogsData.maxStackSize;
            currentResourceAmount -= resourceData.woodLogsData.maxStackSize;
        }
        if (currentResourceAmount>0)
        {
            GameObject woodenLogs = new GameObject(resourceData.woodLogsData.name);
            woodenLogs.AddComponent<Item>().SetItemData(resourceData.woodLogsData);
            woodenLogs.transform.position = transform.position;
            woodenLogs.GetComponent<Item>().currentStackSize = currentResourceAmount;
            currentResourceAmount = 0;
        }
    }

    public ItemData GetResourceItemData() // change to return a list of resource itemData's
    {
        return resourceData.woodLogsData;
    }

    public override void Inspect() => Debug.Log("Inspect " + resourceData.name);


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y + 4f, transform.position.z), new Vector3(0.8f,8,0.8f));
    }

    public void KillRescource()
    {
        TakeDamage(currentHealth);
    }
}
