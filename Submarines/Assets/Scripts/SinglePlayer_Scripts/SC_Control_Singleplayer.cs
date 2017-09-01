using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Control_Singleplayer : MonoBehaviour {
    
    private int numberOfShips = 5;
    public GameObject gShip;
    void Update()
    {
        if (Input.anyKeyDown & this.numberOfShips > 0)
        {
            moveShip(this.numberOfShips);
        }
        
    }

    public void Main_Board_Clicked(int Main_Board_index)
    {
        SC_SingleplayerLogic.Instance.MainBoard_Slot_Click(Main_Board_index);
    }
	
	public void Enemy_Board_Clicked(int Enemy_Board_index)
    {
        SC_SingleplayerLogic.Instance.EnemyBoard_Slot_Click(Enemy_Board_index);
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
            case 4: gShip = SC_Globals.Instance.shipObjects["ship2"]; break;
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
       }
}
