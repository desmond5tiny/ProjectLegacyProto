using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ResourceObject))]
public class GatherTask : MonoBehaviour, ITask
{
    public void SetTask()
    {
        int i = 0;
        foreach (KeyValuePair<int, GameObject> pair in GlobalSelection.Instance.selectionDictionary.selectedDict)
        {
            GameObject unit = GlobalSelection.Instance.selectionDictionary.selectedDict.ElementAt(i).Value;
            unit.GetComponent<PlayerUnit>().SetResourceTarget(transform.GetComponent<ResourceObject>());
            i++;
        }
    }
}
