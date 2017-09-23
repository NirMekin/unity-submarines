using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_Control_Singleplayer : MonoBehaviour {

    private int numberOfShips = 5;
    private bool whosTurn = true;       //True = player turn False = Computer Turn
    public GameObject gShip;
    void Update()
    {
        if (Input.anyKeyDown & this.numberOfShips > 0)
        {
            moveShip(this.numberOfShips);
        }
       /* if (numberOfShips == 0)
        {
            if (!whosTurn)
            {
                SC_View.Instance.changeStatusText("Computer's Turn");
                StartCoroutine(Wait(2));
                whosTurn = !whosTurn;

            }
        }*/
    }

   private void stratPlay()
    {
        if (!whosTurn)
        {
            SC_View.Instance.changeStatusText("Computer's Turn");
            StartCoroutine(Wait(2));
        }
        else
        {
            SC_View.Instance.changeStatusText("Your Turn");
        }
    }

    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);   //Wait
        SC_Computer.Instance.ComputerGuess();
        SC_View.Instance.changeStatusText("Your Turn");
        whosTurn = !whosTurn;
    }

    public void Main_Board_Clicked(int Main_Board_index)
    {
        SC_SingleplayerLogic.Instance.MainBoard_Slot_Click(Main_Board_index);
    }

    public void Enemy_Board_Clicked(int Enemy_Board_index)
    {
        Color transparentColor = new Color(0, 0, 0, 0);
        Color EnemyBtnColor = SC_Globals.Instance.EnemyBtnObjects["Enemy_Btn (" + Enemy_Board_index + ")"].GetComponent<Image>().color;
        if (whosTurn && EnemyBtnColor == transparentColor)
        {
            SC_SingleplayerLogic.Instance.EnemyBoard_Slot_Click(Enemy_Board_index);
            whosTurn = !whosTurn;
            stratPlay();
        }


    }

    public void Ship_Select(string shipName)
    {
        SC_View.Instance.ShipSelector(shipName);
    }

    public void moveShip(int shipObjectIndex)
    {

        switch (shipObjectIndex)
        {
            case 5: gShip = SC_Globals.Instance.shipObjects["ship1"]; break;
            case 4: gShip = SC_Globals.Instance.shipObjects["ship2"];
                Debug.Log("got in");

                break;
            case 3: gShip = SC_Globals.Instance.shipObjects["ship3"]; break;
            case 2: gShip = SC_Globals.Instance.shipObjects["ship4"]; break;
            case 1: gShip = SC_Globals.Instance.shipObjects["ship5"]; break;
            default: break;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SC_SingleplayerLogic.Instance.moveShipVeritcal(gShip, GameEnums.Arrow.left);
            Debug.Log("LeftArrow key was pressed");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SC_SingleplayerLogic.Instance.moveShipVeritcal(gShip, GameEnums.Arrow.right);
            Debug.Log("RightArrow key was pressed");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SC_SingleplayerLogic.Instance.moveShipVeritcal(gShip, GameEnums.Arrow.up);
            Debug.Log("UpArrow key was pressed");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SC_SingleplayerLogic.Instance.moveShipVeritcal(gShip, GameEnums.Arrow.down);
            Debug.Log("DownArrow key was pressed");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SC_SingleplayerLogic.Instance.moveShipVeritcal(gShip, GameEnums.Arrow.space);
            Debug.Log("space key was pressed");
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (SC_SingleplayerLogic.Instance.isValidSlot(gShip))
            {
                SC_SingleplayerLogic.Instance.moveShipVeritcal(gShip, GameEnums.Arrow.enter);
                numberOfShips--;
                if (numberOfShips > 0)
                {
                    SC_Globals.Instance.shipObjects["ship" + (5 - numberOfShips + 1)].SetActive(true);
                }
                switch (numberOfShips)
                {
                    case 4: SC_SingleplayerLogic.Instance.shipLength = 2; break;
                    case 3: SC_SingleplayerLogic.Instance.shipLength = 3; break;
                    case 2: SC_SingleplayerLogic.Instance.shipLength = 3; break;
                    case 1: SC_SingleplayerLogic.Instance.shipLength = 4; break;
                    default: break;
                }
                if (numberOfShips == 0)
                {
                    stratPlay();
                }
            }
            
            Debug.Log("enter key was pressed");
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene_SinglePlayer");
    }

    public void Exit()
    {
        Application.LoadLevel(0);
    }

    

}
//Created By Nir Mekin and Or Adar