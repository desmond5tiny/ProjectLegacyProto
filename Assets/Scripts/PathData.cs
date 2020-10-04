using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Path",menuName = "Path")]
public class PathData : ScriptableObject
{
    public new string name;
    public int MaxHealth;
    public float speedBuff;
}
