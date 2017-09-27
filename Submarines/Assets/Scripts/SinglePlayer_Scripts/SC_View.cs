using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_View : MonoBehaviour {

    #region Singleton

    static SC_View instance;
    public static SC_View Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_View").GetComponent<SC_View>();
            return instance;
        }
    }

    #endregion
    Color transparentColor = new Color(0,0,0,0);
    Color SelectedShip = new Color(255, 0, 0, 255);
    Color NotSelectedShip = new Color(255, 255, 255, 255);
    Color LocatedShip = new Color(0, 255, 8, 255);
    Color Heat = new Color(255, 0, 0, 255);
    Color Miss = new Color(113, 113, 113, 255);
    void Start()
    {
        Init();
    }

    public void Init()
    {
       for(int i=0; i<SC_Globals.Instance.mainBtnObjects.Count; i++)
        {
            
            SC_Globals.Instance.mainBtnObjects["Main_Btn (" + i + ")"].GetComponent<Image>().color = transparentColor;
        }
        for (int i = 0; i < SC_Globals.Instance.EnemyBtnObjects.Count; i++)
        {
            SC_Globals.Instance.EnemyBtnObjects["Enemy_Btn (" + i + ")"].GetComponent<Image>().color = transparentColor;
        }

        setToInActive();
    }

    public void ShipSelector(string shipName)
    {
        switch (shipName)
        {
            case "small":
                SC_Globals.Instance.SinglePlayerObjects["Small_Ship"].GetComponent<Image>().color = SelectedShip;
                SC_Globals.Instance.SinglePlayerObjects["Medium_Ship"].GetComponent<Image>().color = NotSelectedShip;
                SC_Globals.Instance.SinglePlayerObjects["Large_Ship"].GetComponent<Image>().color = NotSelectedShip;
                break;
            case "medium":
                SC_Globals.Instance.SinglePlayerObjects["Medium_Ship"].GetComponent<Image>().color = SelectedShip;
                SC_Globals.Instance.SinglePlayerObjects["Small_Ship"].GetComponent<Image>().color = NotSelectedShip;
                SC_Globals.Instance.SinglePlayerObjects["Large_Ship"].GetComponent<Image>().color = NotSelectedShip;
                break;
            case "large":
                SC_Globals.Instance.SinglePlayerObjects["Large_Ship"].GetComponent<Image>().color = SelectedShip;
                SC_Globals.Instance.SinglePlayerObjects["Small_Ship"].GetComponent<Image>().color = NotSelectedShip;
                SC_Globals.Instance.SinglePlayerObjects["Medium_Ship"].GetComponent<Image>().color = NotSelectedShip;
                break;
            default:
                break;
        }
    }

    public void ActivateBtn(GameObject g)
    {
        g.GetComponent<Image>().color = LocatedShip;
    }

    public void setToInActive()
    {
        for (int i = 5; i > 1; i--)
        {
            SC_Globals.Instance.shipObjects["ship" + i].SetActive(false);
        }
        SC_Globals.Instance.SinglePlayerObjects["Panel_EndGame"].SetActive(false);
    }

    public void markBtn(int index, string color)
    {
        switch (color)
        {
            case "red":
                SC_Globals.Instance.EnemyBtnObjects["Enemy_Btn (" + index + ")"].GetComponent<Image>().color = Heat;
                break;
            case "gray":
                SC_Globals.Instance.EnemyBtnObjects["Enemy_Btn (" + index + ")"].GetComponent<Image>().color = Miss;
                break;
        }
    }

    public void markPlayerBtn(int index, string color)
    {
        switch (color)
        {
            case "red":
                SC_Globals.Instance.mainBtnObjects["Main_Btn (" + index + ")"].GetComponent<Image>().color = Heat;
                break;
            case "gray":
                SC_Globals.Instance.mainBtnObjects["Main_Btn (" + index + ")"].GetComponent<Image>().color = Miss;
                break;
        }
    }

    public void changeStatusText(string txt)
    {
        SC_Globals.Instance.SinglePlayerObjects["Text_Status"].GetComponent<Text>().text = txt;
    }

   public void EndGame(string endGame)
    {
        SC_Globals.Instance.SinglePlayerObjects["Txt_EndGame"].GetComponent<Text>().text = endGame;
        SC_Globals.Instance.SinglePlayerObjects["Panel_EndGame"].SetActive(true);
    }
}
