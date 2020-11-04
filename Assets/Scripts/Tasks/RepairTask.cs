using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IStructure))]
public class RepairTask : MonoBehaviour, ITask
{
    public void SetTask()
    {
        throw new System.NotImplementedException();
    }

    public bool structureDamaged()
    {
        if (transform.GetComponent<ITakeDamage>().GetCurrentHealth() < transform.GetComponent <IStructure>().GetMaxHealth())
        {
            return true;
        }
        return false;
    }

}
