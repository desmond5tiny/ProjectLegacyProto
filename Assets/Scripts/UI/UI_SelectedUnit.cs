using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SelectedUnit : MonoBehaviour
{
    public ParticleSystem selectionPrefab;
    private ParticleSystem selectionObject;

    private void Start()
    {
        selectionObject = Instantiate(selectionPrefab, gameObject.transform.position, Quaternion.identity, transform);
    }

    private void OnDestroy()
    {
        Destroy(selectionObject);
    }
}
