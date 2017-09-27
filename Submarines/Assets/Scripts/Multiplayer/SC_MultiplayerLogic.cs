using UnityEngine;
using System.Collections;
using com.shephertz.app42.gaming.multiplayer.client;
using AssemblyCSharp;
using UnityEngine.UI;
using com.shephertz.app42.gaming.multiplayer.client.events;
using System.Collections.Generic;
using System;

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

    void OnEnable()
    {
        
        Listener.OnConnect += OnConnect;
        Listener.OnRoomsInRange += OnRoomsInRange;
        Listener.OnCreateRoom += OnCreateRoom;
        Listener.OnGetLiveRoomInfo += OnGetLiveRoomInfo;
    }

    

    void OnDisable()
    {
        Listener.OnConnect -= OnConnect;
        Listener.OnRoomsInRange -= OnRoomsInRange;
        Listener.OnCreateRoom -= OnCreateRoom;
        Listener.OnGetLiveRoomInfo -= OnGetLiveRoomInfo;
    }

   

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
    private void OnConnect(bool _IsSuccess)
    {
        Debug.Log("OnConnect: " + _IsSuccess);
        WarpClient.GetInstance().GetRoomsInRange(1, 2);
    }
    public void OnRoomsInRange(bool _IsSuccess, MatchedRoomsEvent eventObj)
    {
        Debug.Log("OnRoomsInRange: " + _IsSuccess + " " + eventObj.getRoomsData());
        if (_IsSuccess)
        {
            List<string> rooms = new List<string>();
            // SC_MenuGlobals.rooms = new List<string>();
            foreach (var roomData in eventObj.getRoomsData())
            {
                Debug.Log("Getting Live info on room " + roomData.getId());
                Debug.Log("Room Owner " + roomData.getRoomOwner());
                Debug.Log("Room Name" + roomData.getName());
                if(roomData.getName().Contains("Easy"))
                {
                    Debug.Log("Found room for my level~~");
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
                Debug.Log("No rooms were availible, create a room");
                WarpClient.GetInstance().CreateTurnRoom("Room " + DateTime.UtcNow.ToString()+ " Easy", SC_Multyplayer_Globals.userName, 2, null, 60);
            }
        }
       // else GameObject.Find("Btn_Play").GetComponent<Button>().interactable = true;
    }
    public void OnCreateRoom(bool _IsSuccess, string _RoomId)
    {
        Debug.Log("OnCreateRoom " + _IsSuccess + " " + _RoomId);
        if (_IsSuccess)
        {
            WarpClient.GetInstance().JoinRoom(_RoomId);

            //so i can get events when other users join my room
            WarpClient.GetInstance().SubscribeRoom(_RoomId);
        }
    }

    public void OnGetLiveRoomInfo(LiveRoomInfoEvent eventObj)
    {
        Debug.Log("OnGetLiveRoomInfo " + eventObj.getData().getId());
        WarpClient.GetInstance().JoinRoom(eventObj.getData().getId());
    }

    //copy-paste from single player
    public int shipLength = 2;
    private int currentButton = 56;
    private int previousButton = 56;

    public void MainBoard_Slot_Click(int _index)
    {
        Debug.Log("Main Board Button Num " + _index + " Clicked");
    }

    public void EnemyBoard_Slot_Click(int _index)
    {
        int i, j;
        bool isHeat;
        i = (_index / 10);
        j = (_index % 10);
        isHeat = SC_Computer.Instance.IsHeat(i, j);
        if (isHeat)
        {
            SC_Multiplayer_View.Instance.markBtn(_index, "red");
            if (SC_Multyplayer_Globals.Instance.EnemyShips == 0)
            {
                SC_Multiplayer_View.Instance.EndGame("You Win!");
            }
        }
        else
        {
            SC_Multiplayer_View.Instance.markBtn(_index, "gray");
        }
    }


    private Vector3 getButtonPosition(GameObject g)
    {
        return g.transform.position;
    }

    public void moveShipVeritcal(GameObject g, Multiplayer_Enums.Arrow a)
    {
        float screenWidth = (float)Screen.width / 1024;
        float screenHeight = (float)Screen.height / 768;
        Debug.Log(screenHeight);
        Debug.Log("the current button is " + currentButton);
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
                Debug.Log(this.currentButton);
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

    /////////////////////////

}
