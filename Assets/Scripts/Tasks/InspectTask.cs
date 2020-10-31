using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectTask : MonoBehaviour, ITask
{
    public void SetTask()
    {
        Debug.Log("instpect " + transform.name);
    }
}
