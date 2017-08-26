using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_Globals : MonoBehaviour
{

    public Dictionary<string, GameObject> mainBtnObjects;
    public Dictionary<string, GameObject> EnemyBtnObjects;
    public Dictionary<string, GameObject> SinglePlayerObjects;

    #region Singleton

    static SC_Globals instance;
    public static SC_Globals Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_Globals").GetComponent<SC_Globals>();
            return instance;
        }
    }

    #endregion

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        mainBtnObjects = new Dictionary<string, GameObject>();
        GameObject[] _mainBtnObjects = GameObject.FindGameObjectsWithTag("mainBtnObject");
        foreach (GameObject g in _mainBtnObjects)
        {
            mainBtnObjects.Add(g.name, g);
        }

        EnemyBtnObjects = new Dictionary<string, GameObject>();
        GameObject[] _EnemyBtnObjects = GameObject.FindGameObjectsWithTag("EnemyBtnObject");
        foreach (GameObject g in _EnemyBtnObjects)
        {
            EnemyBtnObjects.Add(g.name, g);
        }

        SinglePlayerObjects = new Dictionary<string, GameObject>();
        GameObject[] _SinglePlayerObjects = GameObject.FindGameObjectsWithTag("SinglePlayerObject");
        foreach (GameObject g in _SinglePlayerObjects)
        {
            SinglePlayerObjects.Add(g.name, g);
        }

    }
}
