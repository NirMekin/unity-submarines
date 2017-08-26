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
}
