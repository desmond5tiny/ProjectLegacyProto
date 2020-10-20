using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildFence : Interactable
{
    public GameObject meshCorner;
    public GameObject meshRope;
    public GameObject meshGround;

    [HideInInspector]
    public float currentHealth;
    private float maxHealth;
    private List<GameObject> children = new List<GameObject>();

    GameObject construct;

    private void Awake()
    {
        //SetSize(new Vector2(3,3), 2.5f);

        currentHealth = 1;
        task = "Build";
    }

    public BuildFence(Vector2 _area, float _gridsize, GameObject _construct)
    {
        construct = _construct;
        SetSize(_area, _gridsize);
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

        if (currentHealth >= maxHealth) { ConstructionCompleet(); }
    }

    private void ConstructionCompleet()
    {
        //play constructio complete anim/effect
        GameObject newConstruct = Instantiate(construct);
        newConstruct.transform.position = transform.position;
        Destroy(this);
    }

    public void SetHealth(float _maxHealth) => maxHealth = _maxHealth;

}
