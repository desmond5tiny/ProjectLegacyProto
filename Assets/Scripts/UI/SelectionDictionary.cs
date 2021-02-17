using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionDictionary : MonoBehaviour
{
    public ParticleSystem unitSelectPrefab;

    public Dictionary<int, GameObject> selectedDict = new Dictionary<int, GameObject>();

    public void addSelected(GameObject go)
    {
        int id = go.GetInstanceID();

        if (!(selectedDict.ContainsKey(id)))
        {
            selectedDict.Add(id, go);
            go.AddComponent<UI_SelectedUnit>().selectionPrefab = unitSelectPrefab; //add selection ui feedback
        }
    }
    public void deselect(int id)
    {
        Destroy(selectedDict[id].GetComponent<UI_SelectedUnit>());
        selectedDict.Remove(id);
    }

    public void deselectAll()
    {
        foreach (KeyValuePair<int, GameObject> pair in selectedDict)
        {
            if (pair.Value != null)
            {
                Destroy(selectedDict[pair.Key].GetComponent<UI_SelectedUnit>());
            }
        }
        selectedDict.Clear();
    }
}
