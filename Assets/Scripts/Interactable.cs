using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [HideInInspector]
    public float interactionRadius;
    public Transform interactionTransform;

    public string task =  "Task";
    public Action<GameObject> EndTask;

    public virtual void Inspect()
    {
        //override this method
        Debug.Log("inspect: " + transform.name);
    }

    public virtual string UnitInteract()
    {
        //Debug.Log("Interact with: " + transform.name);
        return task;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (interactionTransform != null && interactionTransform != transform) { Gizmos.DrawSphere(interactionTransform.position, 0.2f); }
        else { interactionTransform = transform; }

        Gizmos.DrawWireSphere(interactionTransform.position, interactionRadius);
    }
}
