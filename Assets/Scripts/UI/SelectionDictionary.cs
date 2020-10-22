using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionDictionary : MonoBehaviour
{
    public Dictionary<int, GameObject> selectedDict = new Dictionary<int, GameObject>();

    public void addSelected(GameObject go)
    {
        int id = go.GetInstanceID();

        if (!(selectedDict.ContainsKey(id)))
        {
            selectedDict.Add(id, go);
            //go.AddComponent<Selection_Component>(); //add selection ui feedback
        }
    }
    public void deselect(int id)
    {
        //Destroy(selectedDict[id].GetComponent<Selection_Component>());
        selectedDict.Remove(id);
    }

    public void deselectAll()
    {
        /*foreach (KeyValuePair<int, GameObject> pair in selectedDict)
        {
            if (pair.Value != null)
            {
                Destroy(selectedDict[pair.Key].GetComponent<Selection_Component>());
            }
        }*/
        selectedDict.Clear();
    }

}
