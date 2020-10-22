using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class BuildFence : Interactable , ITakeDamage, IStructure
{

    private GameObject meshCorner;
    private GameObject meshRope;
    private GameObject meshGround;

    //[HideInInspector]
    public float currentHealth;
    private float maxHealth=2;
    private List<GameObject> children = new List<GameObject>();

    [SerializeField]
    GameObject construct;

    private void Awake()
    {
        interactionTransform = transform;
        currentHealth = 1;
        task = "Build";
    }

    public void SetMeshes(GameObject _meshCorner, GameObject _meshRope, GameObject _meshGround)
    {
        meshCorner = _meshCorner;
        meshRope = _meshRope;
        meshGround = _meshGround;
    }

    public void Initialize(Vector2 _area, float _gridSize, GameObject _construct, Chunk _chunk)
    {
        transform.gameObject.layer = 10;
        construct = _construct;
        interactionRadius = ((_area.x + _area.y) / 2 * 1.5f) + 1.5f;
        IStructure structure = _construct.GetComponent<IStructure>();
        maxHealth = structure.GetMaxHealth();

        Debug.Log("Buildfence maxHealth: " + maxHealth);

        SetSize(_area, _gridSize);
        transform.gameObject.AddComponent<BoxCollider>().size = new Vector3(_area.x * _gridSize, 1.5f, _area.y * _gridSize);
        transform.gameObject.GetComponent<BoxCollider>().center = new Vector3(0, 0.75f, 0);
        transform.gameObject.AddComponent<NavMeshModifier>().overrideArea = true;
        transform.gameObject.GetComponent<NavMeshModifier>().area = 1;

        _chunk.NavMeshUpdate();
        //add to chunkgrid && update navmesh
    }

    public void SetSize(Vector2 _area, float _gridSize)
    {
        if(_area.x < 1 || _area.y < 1) { Debug.LogError("No Such Size!"); return; }
        int xPoints = Mathf.RoundToInt(_area.x +1);
        int yPoints = Mathf.RoundToInt(_area.y + 1);

        float startX = transform.position.x - ((_area.x / 2) * _gridSize);
        float startZ = transform.position.z - ((_area.y / 2) * _gridSize);

        for (int i = 0; i < xPoints; i++)
        {
            for (int j = 0; j < yPoints; j++)
            {
                float posX = 0;
                float posZ = 0;

                if (i == 0 || j == 0 || i == xPoints - 1 || j == yPoints - 1)
                {
                    if (i == 0) { posX = 0.3f; }
                    if (i == xPoints - 1) { posX = -0.3f; }
                    if (j == 0) { posZ = 0.3f; }
                    if (j == yPoints - 1) { posZ = -0.3f; }

                    GameObject corner = Instantiate(meshCorner, transform);
                    corner.transform.position = new Vector3(startX + posX + (i * _gridSize), transform.position.y, startZ + posZ + (j * _gridSize));
                    children.Add(corner);

                    posX = 0;
                    posZ = 0;
                    float rot = 0;

                    if (i == 0 && j != yPoints - 1) { posZ = _gridSize / 2; posX = 0.3f; rot = 90; }
                    if (i == xPoints - 1 && j != 0) { posZ = -_gridSize / 2; posX = -0.3f; rot = 90; }
                    if (j == 0 && i != 0) { posX = -_gridSize / 2; posZ = 0.3f; }
                    if (j == yPoints-1 && i != xPoints-1) { posX = _gridSize / 2; posZ = -0.3f; }

                    GameObject rope = Instantiate(meshRope, transform);
                    rope.transform.position = new Vector3(startX + posX + (i * _gridSize), transform.position.y, startZ + posZ + (j * _gridSize));
                    rope.transform.rotation = Quaternion.Euler(-90, rot, 0);
                    rope.transform.localScale = new Vector3(1.2f, 1, 1);
                    children.Add(rope);
                }
            }
        }

        GameObject ground = Instantiate(meshGround, transform);
        ground.transform.localScale = new Vector3(_area.x, _area.y, 1);
        children.Add(ground);
    }

    public void AddHealth(float _addHealth)
    {
        currentHealth += _addHealth;
        Debug.Log("Buildfence health: " + currentHealth);
        if (currentHealth >= maxHealth) { ConstructionCompleet(); }
    }

    private void ConstructionCompleet()
    {
        //play constructio complete anim/effect
        construct.SetActive(true);
        construct.transform.position = transform.position;

        //add to citydata and chunkgrid

        Destroy(gameObject);
    }

    public void SetHealth(float _maxHealth) => maxHealth = _maxHealth;

    public void TakeDamage(float dam)
    {
        currentHealth -= dam;
        if (currentHealth <= 0)
        {
            Destroy(this);
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, interactionRadius);
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
