using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_Computer : MonoBehaviour {

    private int[,] computerMatrix = new int[10, 10];
    #region Singleton

    static SC_Computer instance;
    public static SC_Computer Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_Computer").GetComponent<SC_Computer>();
            return instance;
        }
    }

    #endregion

    void Awake () {
        init();
	}
	
    public void init()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                computerMatrix[i, j] = 0;
            }
        }

        computerMatrix[1, 1] = 1;
        computerMatrix[1, 2] = 1;

        computerMatrix[4, 4] = 1;
        computerMatrix[5, 4] = 1;

        computerMatrix[3, 3] = 1;
        computerMatrix[3, 4] = 1;
        computerMatrix[3, 5] = 1;

        computerMatrix[5, 6] = 1;
        computerMatrix[5, 7] = 1;
        computerMatrix[5, 8] = 1;

        computerMatrix[0, 0] = 1;
        computerMatrix[0, 1] = 1;
        computerMatrix[0, 2] = 1;
        computerMatrix[0, 3] = 1;

    }

	public bool IsHeat(int i,int j)
    {
        if (computerMatrix[i,j] == 1)
        {
            computerMatrix[i, j] = 0;
            SC_Globals.Instance.EnemyShips--;
            return true;
        }else
        {
            return false;
        }
    }

    public void ComputerGuess()
    {
        int guess = Random.Range(0, 99);
        Debug.Log("computer guess " + guess);
        Color missColor = new Color(0, 0, 0, 0);
        Color heatColor = new Color(0, 255, 8, 255);
        Color Heat = new Color(255, 0, 0, 255);
        Color Miss = new Color(113, 113, 113, 255);
        Color playerBoardColor = SC_Globals.Instance.mainBtnObjects["Main_Btn (" + guess + ")"].GetComponent<Image>().color;


        while (playerBoardColor == Heat || playerBoardColor == Miss)
        {
            guess = Random.Range(0, 99);
            playerBoardColor = SC_Globals.Instance.mainBtnObjects["Main_Btn (" + guess + ")"].GetComponent<Image>().color;
           
        }

        if (SC_Globals.Instance.mainBtnObjects["Main_Btn (" + guess + ")"].GetComponent<Image>().color == heatColor)
        {
            SC_Globals.Instance.PlayerShips--;
            SC_View.Instance.markPlayerBtn(guess, "red");
            if (SC_Globals.Instance.PlayerShips == 0)
            {
                SC_View.Instance.EndGame("You Loose!");
            }
        }else
        {
            SC_View.Instance.markPlayerBtn(guess, "gray");
        }

    }
}
