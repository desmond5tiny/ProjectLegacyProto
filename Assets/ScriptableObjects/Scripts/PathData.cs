using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Path",menuName = "Path")]
public class PathData : ScriptableObject
{
    public new string name;
    public int MaxHealth;
    public float speedBuff;

    [Header("Path Prefabs")]
    public GameObject single;
    public GameObject deadend;
    public GameObject corner;
    public GameObject straight;
    public GameObject split;
    public GameObject cross;
    public GameObject cornerFill;
    public GameObject sideFill;
    public GameObject centerFill;
}
