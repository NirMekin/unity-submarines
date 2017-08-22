using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_SinglePlayerLogic : MonoBehaviour {

    #region Singleton

    static SC_SinglePlayerLogic instance;
    public static SC_SinglePlayerLogic Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_SinglePlayerLogic").GetComponent<SC_SinglePlayerLogic>();
            return instance;
        }
    }
    #endregion

    void Start () {
		
	}
	

	void Update () {
		
	}

    public void Board_Btn_Press_Logic()
    {
        Debug.Log("Pressed");
    }
}
