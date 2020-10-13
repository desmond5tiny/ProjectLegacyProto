using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
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

    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null) interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, interactionRadius);
    }
}
