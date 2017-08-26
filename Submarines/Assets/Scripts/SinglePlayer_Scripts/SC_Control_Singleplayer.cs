using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Control_Singleplayer : MonoBehaviour {

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
}
