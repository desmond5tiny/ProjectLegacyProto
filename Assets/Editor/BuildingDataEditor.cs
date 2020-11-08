using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildingData))]
public class BuildingDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); //calls original and executes
       /* BuildingData buildingData = (BuildingData)target;


        GUILayout.Space(10);
        DrawProducts(buildingData);

        GUILayout.Space(10);
        buildingData.showBuildList = EditorGUILayout.Foldout(buildingData.showBuildList, "Build Materials", true);
        if (buildingData.showBuildList)
        {
            List<ItemData> itemList = buildingData.buildItemList;
            List<int> itemAmountList = buildingData.buildItemAmountList;
            EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-", GUILayout.MaxWidth(50))) 
                {
                    if (itemList.Count > 0)
                    {
                        itemList.RemoveAt(itemList.Count - 1);
                        itemAmountList.RemoveAt(itemAmountList.Count - 1);
                        buildingData.buildItemList = itemList;
                        buildingData.buildItemAmountList = itemAmountList;
                    }
                }
                EditorGUILayout.LabelField(itemList.Count.ToString(),GUILayout.MaxWidth(30));
                if (GUILayout.Button("+", GUILayout.MaxWidth(40))) 
                {
                    itemList.Add(buildingData.buildItem);
                    itemAmountList.Add(new int());
                    buildingData.buildItemList = itemList;
                    buildingData.buildItemAmountList = itemAmountList;
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
    private static void DrawProducts(BuildingData buildingData)
    {
        buildingData.ShowProduction = EditorGUILayout.Foldout(buildingData.ShowProduction, "Items", true);
        if (buildingData.ShowProduction)
        {
            
            List<ItemData> list = buildingData.produceItems;
            buildingData.produce = EditorGUILayout.Toggle("Produce Items", buildingData.produce);
            if (buildingData.produce)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUI.indentLevel++;
                if (buildingData.produceItems.Count < 1) { buildingData.AddProduceItem(); }
                if (GUILayout.Button("-", GUILayout.MaxWidth(50))) { buildingData.RemoveProduceItem(); }
                if (GUILayout.Button("+", GUILayout.MaxWidth(50))) { buildingData.AddProduceItem(); }
                EditorGUILayout.EndHorizontal();

                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = (ItemData)EditorGUILayout.ObjectField(list[i],typeof(ItemData),false);
                }
                EditorGUI.indentLevel--;
            }
            else
            {
                buildingData.produceItems.Clear();
            }
        }
    }
}
