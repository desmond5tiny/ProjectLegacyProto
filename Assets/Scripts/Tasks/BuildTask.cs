using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BuildFence))]
public class BuildTask : MonoBehaviour, ITask
{
    public void SetTask()
    {
        int i = 0;
        foreach (KeyValuePair<int, GameObject> pair in GlobalSelection.Instance.selectionDictionary.selectedDict)
        {
            GameObject unit = GlobalSelection.Instance.selectionDictionary.selectedDict.ElementAt(i).Value;
            unit.GetComponent<PlayerUnit>().SetBuildTarget(transform.GetComponent<BuildFence>());
            i++;
        }
    }
}
