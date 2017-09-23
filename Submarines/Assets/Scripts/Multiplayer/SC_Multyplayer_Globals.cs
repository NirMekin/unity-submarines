using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

public class SC_Multyplayer_Globals : MonoBehaviour {

    public static Listener listener;
    public static List<string> rooms;
    public static string userName = string.Empty;

    public Dictionary<string, GameObject> mainBtnObjects;
    public Dictionary<string, GameObject> EnemyBtnObjects;
    public Dictionary<string, GameObject> SinglePlayerObjects;
    public Dictionary<string, GameObject> shipObjects;
    public int PlayerShips = 14;
    public int EnemyShips = 14;

    static SC_Multyplayer_Globals instance;
    public static SC_Multyplayer_Globals Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_Multyplayer_Globals").GetComponent<SC_Multyplayer_Globals>();
            return instance;
        }
    }

    void Awake()
    {
        listener = new Listener();
        rooms = new List<string>();
        init();
    }

    private void init()
    {
        mainBtnObjects = new Dictionary<string, GameObject>();
        GameObject[] _mainBtnObjects = GameObject.FindGameObjectsWithTag("mainBtnObject");
        foreach (GameObject g in _mainBtnObjects)
        {
            mainBtnObjects.Add(g.name, g);
        }

        shipObjects = new Dictionary<string, GameObject>();
        GameObject[] _shipObjects = GameObject.FindGameObjectsWithTag("shipObject");
        foreach (GameObject g in _shipObjects)
        {
            shipObjects.Add(g.name, g);
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
