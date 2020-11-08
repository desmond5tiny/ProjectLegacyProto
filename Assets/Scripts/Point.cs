using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point //change to struct?
{
    public GameObject pointTile;
    public enum PointContent { Empty, Building, Path, Resource }
    public PointContent contains;
    public bool buildable=true;
    public float buildHeight;
    //public Item item;
}
