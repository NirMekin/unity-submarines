using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Control_Singleplayer : MonoBehaviour {

    private int numberOfShips = 5;
    void Update()
    {
        if (this.numberOfShips > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                switch (this.numberOfShips)
                {
                    case 5:
                        SC_SingleplayerLogic.Instance.moveShipVeritcal(SC_Globals.Instance.shipObjects["ship1"], GameEnums.Arrow.left);
                        break;
                }
                Debug.Log("LeftArrow key was pressed");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.numberOfShips--;
                Debug.Log("space key was pressed");
            }
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

    //move to logic
    public void moveShip()
    {
        Debug.Log("TEST");
    }
}
