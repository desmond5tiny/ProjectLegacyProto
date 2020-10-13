using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    #region Singleton
    public static CityManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Debug.LogError("more then once instance of CityManager found!"); }
    }
    #endregion

    public Dictionary<Vector3, GameObject> cityDict = new Dictionary<Vector3, GameObject>();

    public Dictionary<string, int> stockPile = new Dictionary<string, int>();

    //temp
    public GameObject Camp;

    private void Start()
    {
        
    }

    public void AddConstruct(Vector3 pos, GameObject newConstruct)
    {
        cityDict.Add(pos, newConstruct);
        //Debug.Log("add path at: " + pos);
    }

    

}
