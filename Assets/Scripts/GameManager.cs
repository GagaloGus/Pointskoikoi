using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Win_States { Player1, Player2, Tie }
public enum GameMode { PointThief, MostPoints }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int pts1, pts2, offsetPoints, pointsToAdd, originalPts = 30, round, maxRounds, koi;
    public bool p1Choose, p1LastChoose;
    public GameMode gameMode;

    public Color[] koiColors;

    public List<Sprite> CardSprites;

    public static readonly List<string> MONTHS = new List<string>() { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
    public static readonly string ROUNDCOUNT_KEY = "ROUNDKEY", ROUNDTEXT_KEY = "ROUNDTEXTKEY", OFFSETPOINTS_KEY = "OFFSET";

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (!instance) //instance  != null  //Detecta que no haya otro manager en la escena.
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); //Si hay otro manager lo destruye.
        }
        instance = this;

        koi = 1;
        offsetPoints = 0;
        pts1 = pts2 = offsetPoints;
        p1LastChoose = true;
    }

    public void Koi()
    {
        koi++;

        GameEventsManager.instance.gameEvents.Koi();
    }

    public void AddPoints()
    {
        pointsToAdd*=koi;

        if(pointsToAdd != 0)
        {
            pts1 -= pointsToAdd;
            pts2 += pointsToAdd;
            offsetPoints += pointsToAdd;

            GameEventsManager.instance.gameEvents.OnPointsAdded();
        }
        //Si pointsToAdd es negativo -> gana player1 (izquierda)
    }

    public void AfterPointsUI()
    {
        if (Mathf.Abs(offsetPoints) >= originalPts)
        {
            GameEventsManager.instance.gameEvents.OnWin(Get_WinCondition());
        }
        else
        {
            NextRound();
        }
    }

    public void NextRound()
    {
        round++;
        print("Next round "+ round);
        if(round > maxRounds)
        {
            GameEventsManager.instance.gameEvents.OnWin(Get_WinCondition());
        }
        else
        {
            GameEventsManager.instance.gameEvents.OnRoundChange(round);
            ResetSetup();
        }
    }

    public void ResetSetup()
    {
        koi = 1;
        pointsToAdd = 0;
        GameEventsManager.instance.gameEvents.ResetSetup();
    }

    public void ResetGame()
    {
        offsetPoints = 0;
        pts1 = pts2 = originalPts;
        round = 1;
        GameEventsManager.instance.gameEvents.ResetGame();
        GameEventsManager.instance.gameEvents.ResetSetup();
    }

    public Win_States Get_WinCondition()
    {
        Win_States win;

        if (Mathf.Abs(pts1) > originalPts)
            win = Win_States.Player1;
        else if (Mathf.Abs(pts2) > originalPts)
            win = Win_States.Player2;
        else
            win = Win_States.Tie;

        return win;
    }

    private void OnApplicationQuit()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;

        //Añadir playerprefs
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
