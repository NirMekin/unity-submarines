﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SC_Controller : MonoBehaviour
{
    public void Screen_Main_Btn_SinglePlayer()
    {
        SC_MenuLogic.Instance.Screen_Main_Btn_SinglePlayerLogic();
    }

    public void Screen_Main_Btn_MultiPlayer()
    {
        SC_MenuLogic.Instance.Screen_Main_Btn_MultiplayerLogic();
    }

    public void Screen_Main_Btn_StudentInfo()
    {
        SC_MenuLogic.Instance.Screen_Main_Btn_StudentInfoLogic();
    }

    public void Screen_Student_Info_Btn_StudentInfo()
    {
        SC_MenuLogic.Instance.Screen_Student_Info_Btn_StudentInfoLogic();
    }
    
    public void Screen_Main_Btn_Options()
    {
        SC_MenuLogic.Instance.Screen_Main_Btn_OptionsLogic();
    }

    public void Screen_Main_Btn_Back()
    {
        SC_MenuLogic.Instance.Screen_Main_Btn_BackLogic();
    }

    public void Screen_Options_Slider_Music()
    {
        float _value = SC_MenuLogic.Instance.unityObjects["Slider_Music"].GetComponent<Slider>().value;
        SC_MenuLogic.Instance.Screen_Options_Slider_MusicLogic(_value);
    }

    public void Screen_Options_Slider_SFX()
    {
        float _value = SC_MenuLogic.Instance.unityObjects["Slider_SFX"].GetComponent<Slider>().value;
        SC_MenuLogic.Instance.Screen_Options_Slider_SFXLogic(_value);
    }
    public void Screen_Multiplayer_Slider()
    {
        float _value = SC_MenuLogic.Instance.unityObjects["Slider_Multiplayer"].GetComponent<Slider>().value;
        SC_MenuLogic.Instance.Screen_Multiplayer_Slider_MultiplayerLogic(_value);
    }

    public void Board_Btn_Press(int _index)
    {
         Debug.Log("INDEX: " + _index);
        SC_SinglePlayerLogic.Instance.Board_Btn_Press_Logic();
    }
    




}
