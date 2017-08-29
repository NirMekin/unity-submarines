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
    //public int shipLength = 2;
    public int currentButton = 46;

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

  /*  public void Ship_btn_press_postion(GameObject g)
    {
        Vector2 btnShip = new Vector2(g.transform.position.x, g.transform.position.y);
        Debug.Log(btnShip);
    }*/

    public void moveShipVeritcal(GameObject g, GameEnums.Arrow a)
    {
        switch (a)
        {
            case GameEnums.Arrow.left:
                if (currentButton % 10 != 9)
                {
                    this.currentButton++;
                    g.transform.position = new Vector3(g.transform.position.x - 36, g.transform.position.y, g.transform.position.z);
                    
                }
                break;
        }
        Debug.Log(this.currentButton);
    }
}
