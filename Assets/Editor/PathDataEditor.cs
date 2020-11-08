using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

[CustomEditor(typeof(PathData))]
public class PathDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        /*PathData pathData = (PathData)target;

        pathData.showBuildList = EditorGUILayout.Foldout(pathData.showBuildList, "Build Materials", true);
        if (pathData.showBuildList)
        {
            List<ItemData> itemList = pathData.buildItemList;
            List<int> itemAmountList = pathData.buildItemAmountList;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
            {
                if (itemList.Count > 0)
                {
                    itemList.RemoveAt(itemList.Count - 1);
                    itemAmountList.RemoveAt(itemAmountList.Count - 1);
                }
            }
            EditorGUILayout.LabelField(itemList.Count.ToString(), GUILayout.MaxWidth(30));
            if (GUILayout.Button("+", GUILayout.MaxWidth(40)))
            {
                itemList.Add(pathData.buildItem);
                itemAmountList.Add(new int());
                //buildingData.UpdateDictionary();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Resources required for building: ");
            for (int i = 0; i < itemList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                itemList[i] = (ItemData)EditorGUILayout.ObjectField(itemList[i], typeof(ItemData), false);
                itemAmountList[i] = EditorGUILayout.IntField(itemAmountList[i]);
                EditorGUILayout.EndHorizontal();
            }
        }*/

    }
}
