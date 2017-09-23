using UnityEngine;
using System.Collections;
using com.shephertz.app42.gaming.multiplayer.client;
using AssemblyCSharp;
using UnityEngine.UI;
using com.shephertz.app42.gaming.multiplayer.client.events;
using System.Collections.Generic;

public class SC_MultiplayerLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        init();	
	}

    public void init()
    {

        WarpClient.initialize(DefinedVariabels.apiKey, DefinedVariabels.secretKey);
        WarpClient.GetInstance().AddConnectionRequestListener(SC_Multyplayer_Globals.listener);
        WarpClient.GetInstance().AddChatRequestListener(SC_Multyplayer_Globals.listener);
        WarpClient.GetInstance().AddUpdateRequestListener(SC_Multyplayer_Globals.listener);
        WarpClient.GetInstance().AddLobbyRequestListener(SC_Multyplayer_Globals.listener);
        WarpClient.GetInstance().AddNotificationListener(SC_Multyplayer_Globals.listener);
        WarpClient.GetInstance().AddRoomRequestListener(SC_Multyplayer_Globals.listener);
        WarpClient.GetInstance().AddZoneRequestListener(SC_Multyplayer_Globals.listener);
        WarpClient.GetInstance().AddTurnBasedRoomRequestListener(SC_Multyplayer_Globals.listener);

        SC_Multyplayer_Globals.userName = System.DateTime.UtcNow.Ticks.ToString();
        Debug.Log(SC_Multyplayer_Globals.userName);
        WarpClient.GetInstance().Connect(SC_Multyplayer_Globals.userName);
        Debug.Log("connecting..."); 
    }
}
