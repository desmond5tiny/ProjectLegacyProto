﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public enum PointContent { Empty, Building, Path, Tree, Rock }
    public PointContent contains;
    public bool buildable=true;
    public float buildHeight;
    //public Item item;
}
