using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private int currentButton = 56;
    private int previousButton = 56;

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
        int i,j;
        bool isHeat;
        i = (_index / 10);
        j = (_index % 10);
        isHeat = SC_Computer.Instance.IsHeat(i, j);
        if (isHeat)
        {
            SC_View.Instance.markBtn(_index, "red");
            if (SC_Globals.Instance.EnemyShips == 0)
            {
                SC_View.Instance.EndGame("You Win!");
            }
        }
        else
        {
            SC_View.Instance.markBtn(_index, "gray");
        }
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
        Debug.Log("the current button is " + currentButton);
        Vector3 tempHeight;
        switch (a)
        {
            case GameEnums.Arrow.left:
                if (currentButton % 10 != 9)
                {
                    currentButton++;
                    previousButton++;
                    if (g.transform.localEulerAngles.z == 0) {
                        tempHeight = getButtonPosition(SC_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(tempHeight.x + (15 * screenWidth) + (19 * (shipLength - 2)), g.transform.position.y, g.transform.position.z);
                    }else
                    {
                        tempHeight = getButtonPosition(SC_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(tempHeight.x, g.transform.position.y, g.transform.position.z);
                    }
                

                }
                break;
            case GameEnums.Arrow.right:
                if(((((currentButton % 10 ) + this.shipLength -1 ) > this.shipLength) && g.transform.localEulerAngles.z == 0) || ((currentButton % 10) != 0 && g.transform.localEulerAngles.z >= 90))
                {
                    currentButton--;
                    previousButton--;
                    if (g.transform.localEulerAngles.z == 0)
                    {
                        tempHeight = getButtonPosition(SC_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(tempHeight.x + (15 * screenWidth) + (19*(shipLength-2)), g.transform.position.y, g.transform.position.z);
                    }
                    else
                    {
                        tempHeight = getButtonPosition(SC_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(tempHeight.x, g.transform.position.y, g.transform.position.z);
                    }
                }
                break;
            case GameEnums.Arrow.up:
                Debug.Log(this.currentButton);
                if (currentButton >= 10)
                {
                    currentButton -= 10;
                    previousButton -= 10;
                    if (g.transform.localEulerAngles.z == 0)
                    {
                        tempHeight = getButtonPosition(SC_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(g.transform.position.x, tempHeight.y, g.transform.position.z);
                    }else
                    {
              
                        tempHeight = getButtonPosition(SC_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(g.transform.position.x, tempHeight.y - (16*screenHeight) - (17 * (shipLength - 2)), g.transform.position.z);
                    }
                }
                break;
            case GameEnums.Arrow.down:
                if ((currentButton < 90 && g.transform.localEulerAngles.z == 0) || (currentButton < 80 && g.transform.localEulerAngles.z >= 90))
                {
                    currentButton += 10;
                    previousButton += 10;
                    if (g.transform.localEulerAngles.z == 0)
                    {
                        tempHeight = getButtonPosition(SC_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(g.transform.position.x, tempHeight.y, g.transform.position.z);
                    }
                    else
                    {

                        tempHeight = getButtonPosition(SC_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(g.transform.position.x, tempHeight.y - (16 * screenHeight) - (17 * (shipLength - 2)), g.transform.position.z);
                    }
                }
                break;
            case GameEnums.Arrow.space:
                int i = 1;
                if (g.transform.localEulerAngles.z >= (float)90)
                    i = -1;
                int[] _minMax = minMax(shipLength);
                if (currentButton >= _minMax[0] && currentButton <= _minMax[1] && (currentButton % 10 !=0))
                {
                    g.transform.localEulerAngles = new Vector3(0, 0, g.transform.localEulerAngles.z + (90 * i));
                    tempHeight = getButtonPosition(SC_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                    
                    if (g.transform.localEulerAngles.z == 0)
                    {
                        if (currentButton != previousButton)
                        {
                            currentButton = previousButton;
                            tempHeight = getButtonPosition(SC_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        }

                        g.transform.position = new Vector3(tempHeight.x + (15 * screenWidth) + (19*(shipLength-2)), tempHeight.y, g.transform.position.z);
                    }
                    else
                    {
                        currentButton -= 10;
                        g.transform.position = new Vector3(tempHeight.x, tempHeight.y + (16*screenHeight) - (19*(shipLength-2)), g.transform.position.z);
                    }
                    

                }
                break;
            case GameEnums.Arrow.enter:
                for (int j = 0; j < shipLength; j++)
                {
                    SC_View.Instance.ActivateBtn(SC_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                    if (g.transform.localEulerAngles.z == 0)
                    {
                        currentButton--;
                    }
                    else
                    {
                        currentButton+=10;
                    }
                }
                currentButton = 56;
                previousButton = 56;
                break;
        }
    }

    public bool isValidSlot(GameObject g)
    {
        int currentBTN = currentButton;
        Color LocatedShip = new Color(0, 255, 8, 255);
        for (int j = 0; j < shipLength; j++)
        {
            if (g.transform.localEulerAngles.z == 0)
            {
                if (SC_Globals.Instance.mainBtnObjects["Main_Btn (" + currentBTN + ")"].GetComponent<Image>().color == LocatedShip)
                    return false;
                currentBTN--;
            }
            else
            {
                if (SC_Globals.Instance.mainBtnObjects["Main_Btn (" + currentBTN + ")"].GetComponent<Image>().color == LocatedShip)
                    return false;
                currentBTN += 10;
            }
            
        }
        return true;

    }

    private int[] minMax(int _shipLength)
    {
        int[] _minMax = new int[2];
        switch (shipLength)
        {
            case 2: _minMax[0] = 10; _minMax[1] = 89; break;
            case 3: _minMax[0] = 20; _minMax[1] = 79; break;
            case 4: _minMax[0] = 30; _minMax[1] = 69; break;
            default: break;
        }
        return _minMax;

    }

}
