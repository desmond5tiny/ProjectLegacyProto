using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Path : MonoBehaviour
{
    public PathData pathData;

    private GameObject childSingle;
    private GameObject childDeadend;
    private GameObject childCorner;
    private GameObject childStraight;
    private GameObject childSplit;
    private GameObject childCross;
    private GameObject childCornerFill;
    private GameObject childSideFill;
    private GameObject childCenterFill;

    private List<GameObject> children;

    public enum PathShape { Single,Deadend,Corner,Straight,Split,Cross,CornerFill,SideFill,CenterFill}
    private PathShape myShape;


    void Start()
    {

        childSingle = Instantiate(pathData.single,transform);
        children.Add(childSingle);
        childDeadend = Instantiate(pathData.deadend, transform);
        children.Add(childDeadend);
        childCorner = Instantiate(pathData.corner, transform);
        children.Add(childCorner);
        childStraight = Instantiate(pathData.straight, transform);
        children.Add(childStraight);
        childSplit = Instantiate(pathData.split, transform);
        children.Add(childSplit);
        childCross = Instantiate(pathData.cross, transform);
        children.Add(childCross);
        childCornerFill = Instantiate(pathData.cornerFill, transform);
        children.Add(childCornerFill);
        childSideFill = Instantiate(pathData.sideFill, transform);
        children.Add(childSideFill);
        childCenterFill = Instantiate(pathData.centerFill, transform);
        children.Add(childCenterFill);

        SetPathShape(PathShape.Single);
    }

    public void SetPathShape(PathShape newShape)
    {
        if (myShape != newShape)
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].SetActive(false);
            }

            if (newShape == PathShape.Single) { children[0].SetActive(true); }
            if (newShape == PathShape.Deadend) { children[1].SetActive(true); }
            if (newShape == PathShape.Corner) { children[2].SetActive(true); }
            if (newShape == PathShape.Straight) { children[3].SetActive(true); }
            if (newShape == PathShape.Split) { children[4].SetActive(true); }
            if (newShape == PathShape.Cross) { children[5].SetActive(true); }
            if (newShape == PathShape.CornerFill) { children[6].SetActive(true); }
            if (newShape == PathShape.SideFill) { children[7].SetActive(true); }
            if (newShape == PathShape.CenterFill) { children[8].SetActive(true); }
            myShape = newShape;
        }
    }
}
