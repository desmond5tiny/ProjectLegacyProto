using Boo.Lang;
using System;
using UnityEngine;
public class Interactable : MonoBehaviour
{
    [HideInInspector]
    public float interactionRadius = 0;
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
}
