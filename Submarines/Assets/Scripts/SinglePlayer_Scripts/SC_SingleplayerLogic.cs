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
    public int shipLength = 2;
    public int currentButton = 56;

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


    private Vector3 getButtonPosition(GameObject g)
    {
        return g.transform.position;
    }

    public void moveShipVeritcal(GameObject g, GameEnums.Arrow a)
    {
        float screenWidth =  (float)Screen.width / 1024;
        float screenHeight = (float)Screen.height / 768;
        Debug.Log(screenHeight);
        Vector3 tempHeight = getButtonPosition(SC_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
        switch (a)
        {
            case GameEnums.Arrow.left:
                if (currentButton % 10 != 9)
                {
                    this.currentButton++;
                    g.transform.position = new Vector3(tempHeight.x + 15, g.transform.position.y, g.transform.position.z);

                }
                break;
            case GameEnums.Arrow.right:
                if(((currentButton % 10 ) + this.shipLength -1 ) > this.shipLength)
                {
                    this.currentButton--;
                    g.transform.position = new Vector3(tempHeight.x + 15, g.transform.position.y, g.transform.position.z);
                }
                break;
            case GameEnums.Arrow.up:
                Debug.Log(this.currentButton);
                if (currentButton > 10)
                {
                    this.currentButton -= 10;
                    g.transform.position = new Vector3(g.transform.position.x, tempHeight.y, g.transform.position.z);
                }
                break;
            case GameEnums.Arrow.down:
                if (currentButton < 90)
                {
                    this.currentButton += 10;
                    g.transform.position = new Vector3(g.transform.position.x, tempHeight.y, g.transform.position.z);
                }
                break;
            case GameEnums.Arrow.space:
                int i = 1;
                if (g.transform.localEulerAngles.z >= (float)90)
                    i = -1;
                g.transform.localEulerAngles = new Vector3(0, 0, g.transform.localEulerAngles.z + 90*i);

                if (currentButton >= 10 && currentButton <= 89)
                {
                    g.transform.position = new Vector3(tempHeight.x + 15, tempHeight.y, g.transform.position.z);
                }
                break;

        }
        Debug.Log(this.currentButton);
    }
}
