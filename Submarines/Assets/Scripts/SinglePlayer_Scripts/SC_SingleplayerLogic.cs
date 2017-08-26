using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_SingleplayerLogic : MonoBehaviour {

    #region Singleton

    static SC_SingleplayerLogic instance;
    public static SC_SingleplayerLogic Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_SingleplayerLogic").GetComponent<SC_SingleplayerLogic>();
            return instance;
        }
    }

    #endregion

    void start()
    {
        init();
    }

    public void init()
    {

    }

    public void MainBoard_Slot_Click(int _index)
    {
        Debug.Log("Main Board Button Num " + _index + " Clicked");
    }

    public void EnemyBoard_Slot_Click(int _index)
    {
        Debug.Log("Enemy Board Button Num " + _index + " Clicked");
    }
}
