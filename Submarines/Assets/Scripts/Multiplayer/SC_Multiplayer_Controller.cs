using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_Multiplayer_Controller : MonoBehaviour
{

   
   // private bool whosTurn = true;       //True = player turn False = Computer Turn
    public GameObject gShip;
    void Update()
    {
        if (Input.anyKeyDown & SC_Multyplayer_Globals.Instance.numberOfShips > 0)
        {
            moveShip(SC_Multyplayer_Globals.Instance.numberOfShips);
        }
     
    }


    public void Main_Board_Clicked(int Main_Board_index)
    {
        SC_MultiplayerLogic.Instance.MainBoard_Slot_Click(Main_Board_index);
    }

    public void Enemy_Board_Clicked(int Enemy_Board_index)
    {

        Color transparentColor = new Color(0, 0, 0, 0);
        Color EnemyBtnColor = SC_Multyplayer_Globals.Instance.EnemyBtnObjects["Enemy_Btn (" + Enemy_Board_index + ")"].GetComponent<Image>().color;
        if (SC_MultiplayerLogic.Instance.myTurn && EnemyBtnColor == transparentColor)
        {
            SC_MultiplayerLogic.Instance.EnemyBoard_Slot_Click(Enemy_Board_index);
        }


    }

    public void Ship_Select(string shipName)
    {
        SC_Multiplayer_View.Instance.ShipSelector(shipName);
    }

    public void moveShip(int shipObjectIndex)
    {

        switch (shipObjectIndex)
        {
            case 5: gShip = SC_Multyplayer_Globals.Instance.shipObjects["ship1"]; break;
            case 4:
                gShip = SC_Multyplayer_Globals.Instance.shipObjects["ship2"];

                break;
            case 3: gShip = SC_Multyplayer_Globals.Instance.shipObjects["ship3"]; break;
            case 2: gShip = SC_Multyplayer_Globals.Instance.shipObjects["ship4"]; break;
            case 1: gShip = SC_Multyplayer_Globals.Instance.shipObjects["ship5"]; break;
            default: break;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SC_MultiplayerLogic.Instance.moveShipVeritcal(gShip, Multiplayer_Enums.Arrow.left);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SC_MultiplayerLogic.Instance.moveShipVeritcal(gShip, Multiplayer_Enums.Arrow.right);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SC_MultiplayerLogic.Instance.moveShipVeritcal(gShip, Multiplayer_Enums.Arrow.up);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SC_MultiplayerLogic.Instance.moveShipVeritcal(gShip, Multiplayer_Enums.Arrow.down);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SC_MultiplayerLogic.Instance.moveShipVeritcal(gShip, Multiplayer_Enums.Arrow.space);
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {  
            if (SC_MultiplayerLogic.Instance.isValidSlot(gShip))
            { 
                SC_MultiplayerLogic.Instance.moveShipVeritcal(gShip, Multiplayer_Enums.Arrow.enter);
                SC_Multyplayer_Globals.Instance.numberOfShips--;
                if (SC_Multyplayer_Globals.Instance.numberOfShips > 0)
                {
                    SC_Multyplayer_Globals.Instance.shipObjects["ship" + (5 - SC_Multyplayer_Globals.Instance.numberOfShips + 1)].SetActive(true);
                }
                switch (SC_Multyplayer_Globals.Instance.numberOfShips)
                {
                    case 4: SC_MultiplayerLogic.Instance.shipLength = 2; break;
                    case 3: SC_MultiplayerLogic.Instance.shipLength = 3; break;
                    case 2: SC_MultiplayerLogic.Instance.shipLength = 3; break;
                    case 1: SC_MultiplayerLogic.Instance.shipLength = 4; break;
                    default: break;
                }
            }
            if (SC_Multyplayer_Globals.Instance.numberOfShips == 0) {
                if(SC_MultiplayerLogic.Instance.myTurn)
                SC_MultiplayerLogic.Instance.statusStartGame();
            }
             
            Debug.Log("enter key was pressed");
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene_Multiplayer");
    }





}
//Created By Nir Mekin and Or Adar