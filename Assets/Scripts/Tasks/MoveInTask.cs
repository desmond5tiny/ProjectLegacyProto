using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class MoveInTask : MonoBehaviour, ITask
{
    public void SetTask()
    {
        int i = 0;
        foreach (KeyValuePair<int, GameObject> pair in GlobalSelection.Instance.selectionDictionary.selectedDict)
        {
            GameObject unit = GlobalSelection.Instance.selectionDictionary.selectedDict.ElementAt(i).Value;
            transform.GetComponent<Building>().AddInhabitant(unit.GetComponent<PlayerUnit>());
            i++;
        }
    }
}
