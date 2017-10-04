using UnityEngine;
using System.Collections;

public class Multiplayer_Enums : MonoBehaviour {

    public enum SlotState
    {
        Occupied, Bombed, Free, Empty
    };

    public enum Arrow
    {
        left, right, up, down, space, enter
    };

    public enum GameState 
    {
        Winner
    };
}
