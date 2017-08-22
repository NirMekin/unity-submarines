using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Globals : MonoBehaviour
{
    public Dictionary<string, GameObject> boardObjects;
    void Awake()
    {
        Init();
    }

    public void Init()
    {
        boardObjects = new Dictionary<string, GameObject>();
        GameObject[] _boardObjects = GameObject.FindGameObjectsWithTag("BoardObject");
        foreach (GameObject g in _boardObjects)
        {
            Debug.Log(g.name);
            boardObjects.Add(g.name, g);
        }
    }

   
}