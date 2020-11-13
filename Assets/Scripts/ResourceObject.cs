using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[SelectionBase]
public class ResourceObject : Interactable, ITakeDamage
{
    [SerializeField]
    private ResourceObjectData resourceData;
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
        currentHealth = resourceData.maxHealth;
        currentResourceAmount = resourceData.maxItemAmount;

        resourceBase = Instantiate(resourceData.prefabBase, transform);
        resourceBase.layer = 11;
        resourceTop = Instantiate(resourceData.prefabTop, transform);
        resourceTop.layer = 11;

        damagePerDrop = resourceData.maxHealth / resourceData.maxItemAmount;

        interactionRadius = resourceData.interactRadius;
        if (interactionTransform == null) { interactionTransform = transform; }

        BoxCollider myCollider = transform.gameObject.AddComponent<BoxCollider>();
        myCollider.size = resourceTop.GetComponent<BoxCollider>().size;
        myCollider.center = resourceTop.GetComponent<BoxCollider>().center;
        resourceTop.GetComponent<BoxCollider>().enabled = false;
    }
    private void OnEnable() => InputManager.OnKeyN += KillRescource;
    private void OnDisable() => InputManager.OnKeyN -= KillRescource;

    public void TakeDamage(float dam)
    {
        currentHealth -= dam;
        if (currentHealth <=0)
        {
            isEmpty = true;
            currentHealth = 0;
            if (currentResourceAmount > 0) { DropResource(currentResourceAmount); }
            Collapse();
        }
    }

    public void Collapse()
    {
        resourceTop.SetActive(false);
        Destroy(transform.GetComponent<BoxCollider>());
        DestroyObject();
    }

    public bool GetRescource(float damage, out int woodDrop) //change to out a list of drop items
    {
        woodDrop = 0;
        if (!isEmpty)
        {
            if(Mathf.CeilToInt((currentHealth - damage) / damagePerDrop) < currentResourceAmount)
            {
                woodDrop = (currentResourceAmount - Mathf.CeilToInt((currentHealth - damage) / damagePerDrop));
                if(woodDrop>currentResourceAmount) { woodDrop = currentResourceAmount; }
                currentResourceAmount -= woodDrop;
            }
            TakeDamage(damage);
            return true;
        }
        return false;
    }

    public void DropResource(int amount)
    {
        int fullStacks = Mathf.FloorToInt(currentResourceAmount / resourceData.resourceItemData.maxStackSize);
        for (int i = 0; i < fullStacks; i++)
        {
            GameObject woodenLogs = new GameObject(resourceData.resourceItemData.name);
            woodenLogs.AddComponent<Item>().SetItemData(resourceData.resourceItemData);
            woodenLogs.transform.position = transform.position;
            woodenLogs.GetComponent<Item>().currentStackSize = resourceData.resourceItemData.maxStackSize;
            currentResourceAmount -= resourceData.resourceItemData.maxStackSize;
        }
        if (currentResourceAmount>0)
        {
            GameObject woodenLogs = new GameObject(resourceData.resourceItemData.name);
            woodenLogs.AddComponent<Item>().SetItemData(resourceData.resourceItemData);
            woodenLogs.transform.position = transform.position;
            woodenLogs.GetComponent<Item>().currentStackSize = currentResourceAmount;
            currentResourceAmount = 0;
        }
    }

    public ItemData GetResourceItemData() // change to return a list of resource itemData's
    {
        return resourceData.resourceItemData;
    }

    public override void Inspect() => Debug.Log("Inspect " + resourceData.name);


    void OnDrawGizmos()
    {
        if (interactionRadius == 0) { interactionRadius = resourceData.interactRadius; }
        if (interactionTransform == null) { interactionTransform = transform; }

        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), new Vector3(0.8f,2,0.8f));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (interactionTransform != transform) { Gizmos.DrawSphere(interactionTransform.position, 0.2f); }

        Gizmos.DrawWireSphere(interactionTransform.position, interactionRadius);
    }

    public void KillRescource()
    {
        TakeDamage(currentHealth);
    }

    public void DestroyObject()
    {
        //remove from grid
        WorldManager.GetMap().SetGridPointContent(new Vector2(transform.position.x, transform.position.z), Point.PointContent.Empty);
        Destroy(gameObject, 2);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
