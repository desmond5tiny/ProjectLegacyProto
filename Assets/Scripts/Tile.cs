using UnityEditor;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject prefab;
    public bool buildable; 

    private void Awake()
    {
    }

    public void SetMesh()
    {
        if(prefab == null) { Debug.LogError("No Tile prefab!"); return; }

        transform.GetComponent<MeshFilter>().sharedMesh = prefab.GetComponent<MeshFilter>().sharedMesh;

        Material[] mats = new Material[prefab.GetComponent<MeshRenderer>().sharedMaterials.Length];
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = prefab.GetComponent<MeshRenderer>().sharedMaterials[i];
        }

        MeshRenderer meshRend = transform.GetComponent<MeshRenderer>();
        meshRend.materials = mats;
    }



}

[CustomEditor(typeof(Tile))]
public class  TileEditor:Editor
{


    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        Tile tile = (Tile)target;

        tile.prefab = (GameObject)EditorGUILayout.ObjectField("Tile prefab ", tile.prefab, typeof(GameObject), false);

        if (GUILayout.Button("Set Tile Mesh"))
        {
            tile.SetMesh();
        }

        EditorGUILayout.Space(10);
        tile.buildable = EditorGUILayout.Toggle("Build Tile ", tile.buildable);
    }
}
