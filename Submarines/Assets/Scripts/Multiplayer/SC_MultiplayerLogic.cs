using UnityEngine;
using System.Collections;
using com.shephertz.app42.gaming.multiplayer.client;
using AssemblyCSharp;
using UnityEngine.UI;
using com.shephertz.app42.gaming.multiplayer.client.events;
using System.Collections.Generic;
using System;
using MiniJSON;

public class SC_MultiplayerLogic : MonoBehaviour {

    #region Singleton

    static SC_MultiplayerLogic instance;
    

    public static SC_MultiplayerLogic Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_MultiplayerLogic").GetComponent<SC_MultiplayerLogic>();
            return instance;
        }
    }

    #endregion
    public int shipLength = 2;
    private int currentButton = 56;
    private int previousButton = 56;
    public bool myTurn = false;
    public bool isOwner = false;
    public bool[] getStatusShipsReady = new bool[] { false, false };
    msgJson temp = new msgJson();
    bool isHit;

    void OnEnable()
    {
        
        Listener.OnConnect += OnConnect;
        Listener.OnRoomsInRange += OnRoomsInRange;
        Listener.OnMoveCompleted += OnMoveCompleted;
        Listener.OnCreateRoom += OnCreateRoom;
        Listener.OnGetLiveRoomInfo += OnGetLiveRoomInfo;
        Listener.OnDisconnect += OnDisconnect;
        Listener.OnJoinRoom += OnJoinRoom;
        Listener.OnUserJoinRoom += OnUserJoinRoom;


    }


    void OnDisable()
    {
        Listener.OnConnect -= OnConnect;
        Listener.OnRoomsInRange -= OnRoomsInRange;
        Listener.OnCreateRoom -= OnCreateRoom;
        Listener.OnJoinRoom -= OnJoinRoom;
        Listener.OnMoveCompleted -= OnMoveCompleted;
        Listener.OnGetLiveRoomInfo -= OnGetLiveRoomInfo;
        Listener.OnUserJoinRoom -= OnUserJoinRoom;
        Listener.OnDisconnect -= OnDisconnect;
        SendPlayerMove(0, null);
    }

   

    // Use this for initialization
    void Start () {
        init();	
	}

    public void init()
    {
        SC_Multyplayer_Globals.Instance.MultiplayerObjects["logs"].GetComponent<Text>().text = "";
        WarpClient.initialize(DefinedVariabels.apiKey, DefinedVariabels.secretKey);
        WarpClient.GetInstance().AddConnectionRequestListener(SC_Multyplayer_Globals.listener);
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

    //--------------------------------------------Listeners-------------------------------------------//
    private void OnConnect(bool _IsSuccess)
    {
        Debug.Log("OnConnect: " + _IsSuccess);
        if (_IsSuccess)
            WarpClient.GetInstance().GetRoomsInRange(1, 2);
        else SendPlayerMove(-1, null);
    }

    public void OnDisconnect(bool _IsSuccess)
    {
        if(_IsSuccess)
            Debug.Log("Dissconnect from server" + _IsSuccess);
    }
    public void OnRoomsInRange(bool _IsSuccess, MatchedRoomsEvent eventObj)
    {
        if (_IsSuccess)
        {
            List<string> rooms = new List<string>();
            foreach (var roomData in eventObj.getRoomsData())
            {
                Debug.Log("Getting Live info on room " + roomData.getId());
                Debug.Log("Room Owner " + roomData.getRoomOwner());
                Debug.Log("Room Name" + roomData.getName());
                if(roomData.getName().Contains(RoomLevel.roomLevel))
                {
                    Debug.Log("Found room for my level" + RoomLevel.roomLevel);
                    rooms.Add(roomData.getId());
                }
               
            }

            int index = 0;
            if (index < rooms.Count)
            {
                Debug.Log("Getting Live Info on room: " + rooms[index]);
                WarpClient.GetInstance().GetLiveRoomInfo(rooms[index]);
            }
            else
            {
                Debug.Log("No rooms were availible, create a room "+RoomLevel.roomLevel);
                isOwner = true;
                myTurn = true;
                WarpClient.GetInstance().CreateTurnRoom("Room " + DateTime.UtcNow.ToString()+ " " + RoomLevel.roomLevel, SC_Multyplayer_Globals.userName, 2, null, 60);
            }
        }
    }
    public void OnCreateRoom(bool _IsSuccess, string _RoomId)
    {
        Debug.Log("OnCreateRoom " + _IsSuccess + " " + _RoomId);
        if (_IsSuccess)
        {
            WarpClient.GetInstance().JoinRoom(_RoomId); 
        }
    }

    public void OnGetLiveRoomInfo(LiveRoomInfoEvent eventObj)
    {
        Debug.Log("OnGetLiveRoomInfo " + eventObj.getData().getId());
        WarpClient.GetInstance().JoinRoom(eventObj.getData().getId());
    }

    public void OnJoinRoom(bool _IsSuccess, string _RoomId)
    {
        if(_IsSuccess){
            WarpClient.GetInstance().SubscribeRoom(_RoomId);
            if (!isOwner)
                SC_Multyplayer_Globals.Instance.MultiplayerObjects["Screen_Loading"].SetActive(false);
        }
    }

    public void OnUserJoinRoom(RoomData eventObj, string _UserName)
    {
        Debug.Log("you joined room " + eventObj.getId());
        WarpClient.GetInstance().SubscribeRoom(eventObj.getId());
        if (_UserName != SC_Multyplayer_Globals.userName && isOwner)
        {
            SC_Multyplayer_Globals.Instance.MultiplayerObjects["Screen_Loading"].SetActive(false);
            WarpClient.GetInstance().startGame();

        }
    }

    

    public void OnMoveCompleted(MoveEvent _Move)
    {
        temp = JsonUtility.FromJson<msgJson>(_Move.getMoveData());
 
        if(temp != null)
        {
            if (temp.msgSend == "OwnerReady" && !isOwner)
            { 
                myTurn = true;
                if (SC_Multyplayer_Globals.Instance.numberOfShips == 0)
                {
                    statusStartGame();
                    return;
                }

            }
            if (temp.msgSend == "Ready")
            {
                startGame();
                return;
            }

            if (temp.msgSend == "shoot" && !myTurn)
            {
                string tempMsg;
                isHit = checkedForHit(temp.index);
                if (isHit)
                {
                    SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + temp.index + ")"].GetComponent<Image>().color = new Color(255, 0, 0, 255);
                    tempMsg = "isHit";
                    SC_Multyplayer_Globals.Instance.PlayerShips--;
                    if (SC_Multyplayer_Globals.Instance.PlayerShips == 0)
                        tempMsg = "EndGame";
                }
                else
                {
                    SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + temp.index + ")"].GetComponent<Image>().color = new Color(113, 113, 113, 255);
                    tempMsg = "miss";

                }
                SendPlayerMove(temp.index, tempMsg);
            }

            if (temp.msgSend == "isHit" && myTurn)
            {

                SC_Multyplayer_Globals.Instance.EnemyBtnObjects["Enemy_Btn (" + temp.index + ")"].GetComponent<Image>().color = new Color(255, 0, 0, 255);

                SendPlayerMove(temp.index, "switchTurn");
            }

            if (temp.msgSend == "miss" && myTurn)
            {
                Debug.Log("miss");
                SC_Multyplayer_Globals.Instance.EnemyBtnObjects["Enemy_Btn (" + temp.index + ")"].GetComponent<Image>().color = new Color(113, 113, 113, 255);
                SendPlayerMove(temp.index, "switchTurn");
            }

            if (temp.msgSend == "EndGame")
            {
                SC_Multyplayer_Globals.Instance.MultiplayerObjects["Panel_EndGame"].SetActive(true);
                if (SC_Multyplayer_Globals.Instance.PlayerShips == 0)
                    SC_Multyplayer_Globals.Instance.MultiplayerObjects["Txt_EndGame"].GetComponent<Text>().text = "You Lose";
                else
                    SC_Multyplayer_Globals.Instance.MultiplayerObjects["Txt_EndGame"].GetComponent<Text>().text = "You Win !";
                WarpClient.GetInstance().stopGame();
            }

            if (temp.msgSend == "Timeout")
            {
                SC_Multyplayer_Globals.Instance.MultiplayerObjects["Panel_EndGame"].SetActive(true);
                SC_Multyplayer_Globals.Instance.MultiplayerObjects["Txt_EndGame"].GetComponent<Text>().text = "Timeout";
            }

            if (temp.msgSend == "switchTurn")
            {
                if (myTurn == true)
                {
                    myTurn = false;
                    SC_Multyplayer_Globals.Instance.MultiplayerObjects["logs"].GetComponent<Text>().text = "Rival Turn";
                }
                else
                {
                    myTurn = true;
                    SC_Multyplayer_Globals.Instance.MultiplayerObjects["logs"].GetComponent<Text>().text = "Your Turn";
                }
            }
        }
        else
        {
            SendPlayerMove(0, "Timeout");
        }
            
    }


    //---------------------------------------------End Listener---------------------------------------------//

    //--------------------------------------------New Functions--------------------------------------------//
    public void statusStartGame()
    {
        if (isOwner && myTurn)
        {
            SendPlayerMove(0, "OwnerReady");
        }
        else
        {
            if (myTurn)
                SendPlayerMove(0, "Ready");
        }
    }

    msgJson tempMSG = new msgJson();
    public void SendPlayerMove(int index, string msg)
    {
        if (msg == null)
            tempMSG.msgSend = "Timeout";
        else tempMSG.msgSend = msg;
        tempMSG.index = index;
        string jsonToSend = JsonUtility.ToJson(tempMSG);
        WarpClient.GetInstance().sendMove(jsonToSend, "");
    }

    public bool checkedForHit(int index)
    {
        if(SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + index + ")"].GetComponent<Image>().color == new Color(0, 255, 8, 255))
        {
            return true;
        }
        return false;
    }
    

    public void startGame(){
        Debug.Log("start game called");
        if (isOwner == true)
        {
            myTurn = true;
            SC_Multyplayer_Globals.Instance.MultiplayerObjects["logs"].GetComponent<Text>().text = "Your Turn";
        }
        else
        {
            myTurn = false;
            SC_Multyplayer_Globals.Instance.MultiplayerObjects["logs"].GetComponent<Text>().text = "Rival Turn";
        }
        SC_Multyplayer_Globals.Instance.MultiplayerObjects["Text_Status"].GetComponent<Text>().text = "Let's Play !";
        SC_Multyplayer_Globals.Instance.MultiplayerObjects["Panel_disableEnemyBoard"].SetActive(false);
        Debug.Log("**** Let's Play *****");
    }

    //-----------------------Same name function from Singleplayer-----------------------//
    public void MainBoard_Slot_Click(int _index)
    {
        Debug.Log("Main Board Button Num " + _index + " Clicked");
    }

    public void EnemyBoard_Slot_Click(int _index)
    {
        temp.index = _index;
        temp.msgSend = "shoot";
        string jsonToSend = JsonUtility.ToJson(temp);
        WarpClient.GetInstance().sendMove(jsonToSend, "");
    }


    private Vector3 getButtonPosition(GameObject g)
    {
        return g.transform.position;
    }

    public void moveShipVeritcal(GameObject g, Multiplayer_Enums.Arrow a)
    {
        float screenWidth = (float)Screen.width / 1024;
        float screenHeight = (float)Screen.height / 768;
        Vector3 tempHeight;
        switch (a)
        { 
            case Multiplayer_Enums.Arrow.left: 
                if (currentButton % 10 != 9)
                {
                    currentButton++;
                    previousButton++;
                    if (g.transform.localEulerAngles.z == 0)
                    {
                        tempHeight = getButtonPosition(SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(tempHeight.x + (15 * screenWidth) + (19 * (shipLength - 2)), g.transform.position.y, g.transform.position.z);
                    }
                    else
                    {
                        tempHeight = getButtonPosition(SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(tempHeight.x, g.transform.position.y, g.transform.position.z);
                    }


                }
                break;
            case Multiplayer_Enums.Arrow.right:
                if (((((currentButton % 10) + this.shipLength - 1) > this.shipLength) && g.transform.localEulerAngles.z == 0) || ((currentButton % 10) != 0 && g.transform.localEulerAngles.z >= 90))
                {
                    currentButton--;
                    previousButton--;
                    if (g.transform.localEulerAngles.z == 0)
                    {
                        tempHeight = getButtonPosition(SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(tempHeight.x + (15 * screenWidth) + (19 * (shipLength - 2)), g.transform.position.y, g.transform.position.z);
                    }
                    else
                    {
                        tempHeight = getButtonPosition(SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(tempHeight.x, g.transform.position.y, g.transform.position.z);
                    }
                }
                break;
            case Multiplayer_Enums.Arrow.up:
                if (currentButton >= 10)
                {
                    currentButton -= 10;
                    previousButton -= 10;
                    if (g.transform.localEulerAngles.z == 0)
                    {
                        tempHeight = getButtonPosition(SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(g.transform.position.x, tempHeight.y, g.transform.position.z);
                    }
                    else
                    {

                        tempHeight = getButtonPosition(SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(g.transform.position.x, tempHeight.y - (16 * screenHeight) - (17 * (shipLength - 2)), g.transform.position.z);
                    }
                }
                break;
            case Multiplayer_Enums.Arrow.down:
                if ((currentButton < 90 && g.transform.localEulerAngles.z == 0) || (currentButton < 80 && g.transform.localEulerAngles.z >= 90))
                {
                    currentButton += 10;
                    previousButton += 10;
                    if (g.transform.localEulerAngles.z == 0)
                    {
                        tempHeight = getButtonPosition(SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(g.transform.position.x, tempHeight.y, g.transform.position.z);
                    }
                    else
                    {

                        tempHeight = getButtonPosition(SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        g.transform.position = new Vector3(g.transform.position.x, tempHeight.y - (16 * screenHeight) - (17 * (shipLength - 2)), g.transform.position.z);
                    }
                }
                break;
            case Multiplayer_Enums.Arrow.space:
                int i = 1;
                if (g.transform.localEulerAngles.z >= (float)90)
                    i = -1;
                int[] _minMax = minMax(shipLength);
                if (currentButton >= _minMax[0] && currentButton <= _minMax[1] && (currentButton % 10 != 0))
                {
                    g.transform.localEulerAngles = new Vector3(0, 0, g.transform.localEulerAngles.z + (90 * i));
                    tempHeight = getButtonPosition(SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);

                    if (g.transform.localEulerAngles.z == 0)
                    {
                        if (currentButton != previousButton)
                        {
                            currentButton = previousButton;
                            tempHeight = getButtonPosition(SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                        }

                        g.transform.position = new Vector3(tempHeight.x + (15 * screenWidth) + (19 * (shipLength - 2)), tempHeight.y, g.transform.position.z);
                    }
                    else
                    {
                        currentButton -= 10;
                        g.transform.position = new Vector3(tempHeight.x, tempHeight.y + (16 * screenHeight) - (19 * (shipLength - 2)), g.transform.position.z);
                    }


                }
                break;
            case Multiplayer_Enums.Arrow.enter:
                for (int j = 0; j < shipLength; j++)
                {
                    SC_Multiplayer_View.Instance.ActivateBtn(SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + currentButton + ")"]);
                    if (g.transform.localEulerAngles.z == 0)
                    {
                        currentButton--;
                    }
                    else
                    {
                        currentButton += 10;
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
                if (SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + currentBTN + ")"].GetComponent<Image>().color == LocatedShip)
                    return false;
                currentBTN--;
            }
            else
            {
                if (SC_Multyplayer_Globals.Instance.mainBtnObjects["Main_Btn (" + currentBTN + ")"].GetComponent<Image>().color == LocatedShip)
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
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//-------msgJson class used for create same msg structers for all "sendMove"-------//
public class msgJson
{
    public string msgSend;
    public int index;
}