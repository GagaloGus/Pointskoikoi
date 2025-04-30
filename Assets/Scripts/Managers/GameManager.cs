using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Win_States { Player1, Player2, Tie }
public enum GameMode { PointThief, Classic }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Puntos")]
    public int pts1;
    public int pts2;
    public int koi;

    [Header("Config PointThief")]
    public int originalPts = 30;
    public int offsetPoints, pointsToAdd;

    [Header("Config Classic")]
    public int koiPointsForDouble;


    [Header("Rounds")]
    public int round;
    public int maxRounds;

    [Header("Settings")]
    public bool p1Choose;
    public bool p1LastChoose;
    public GameMode gameMode;
    public List<Vector2> scores;
    

    [Header("UI")]
    public Color[] koiColors;
    public List<Sprite> CardSprites;

    public static readonly List<string> MONTHS = new List<string>() { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
    public static readonly string
        ROUNDCOUNT_KEY = "ROUNDKEY",
        OFFSET_PTS = "OFFSETPTS",
        P1_PTS = "P1PTS",
        P2_PTS = "P2PTS";

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
        scores.Clear();
    }

    public int get_MonthNumber()
    {
        return Mathf.Clamp(round - 1, 0, 11);
    }
    public void Koi()
    {
        koi++;

        GameEventsManager.instance.gameEvents.Koi();
    }

    /// <summary>
    /// Si los puntos a añadir son mayor de 0, añade los puntos al jugador respectivo
    /// y llama al evento ONPOINTSADDED
    /// </summary>
    public void AddPoints()
    {
        if(pointsToAdd != 0)
        {
            if (gameMode == GameMode.PointThief)
            {   
                //Si pointsToAdd es negativo -> gana player1 (izquierda)
                pointsToAdd *= koi;
                pts1 = Mathf.Clamp(pts1 - pointsToAdd, 0, originalPts * 2);
                pts2 = Mathf.Clamp(pts2 + pointsToAdd, 0, originalPts * 2);
                offsetPoints += pointsToAdd;   
            }
            else if (gameMode == GameMode.Classic)
            {
                pointsToAdd = Mathf.Abs(pointsToAdd);

                if (pointsToAdd >= koiPointsForDouble)
                    pointsToAdd *= 2;

                if (p1LastChoose)
                    pts1 += pointsToAdd;
                else 
                    pts2 += pointsToAdd;
            }

            print("Putons");
            GameEventsManager.instance.gameEvents.OnPointsAdded();
            scores.Add(new Vector2(pts1, pts2));
        }  
    }

    /// <summary>
    /// Funcion que se llama justo al terminar la ronda
    /// </summary>
    public void AfterPointsUI()
    {
        if (Mathf.Abs(offsetPoints) >= originalPts && gameMode == GameMode.PointThief)
        {
            GameEventsManager.instance.gameEvents.OnWin(Get_WinCondition());
        }
        else
        {
            NextRound();
        }
    }

    public void EndRound()
    {
        scores.Add(new Vector2(pts1, pts2));
        GameEventsManager.instance.gameEvents.OnRoundEnd();

        NextRound();
    }

    public void NextRound()
    {
        if (round >= maxRounds)
        {
            GameEventsManager.instance.gameEvents.OnWin(Get_WinCondition());
        }
        else
        {
            round++;
            print("Next round " + round);

            GameEventsManager.instance.gameEvents.OnRoundChange(round);
            ResetSetup();
        }
    }

    public void StartUpGame()
    {
        ResetParameters();
        GameEventsManager.instance.gameEvents.OnStartGame();
    }

    void ResetParameters()
    {
        offsetPoints = 0;
        scores.Clear();

        pts1 = pts2 = gameMode == GameMode.PointThief ? originalPts : 0;
        round = 1;
    }

    public void ResetSetup()
    {
        koi = 1;
        pointsToAdd = 0;
        GameEventsManager.instance.gameEvents.ResetSetup();
    }

    public void ResetGame()
    {
        ResetParameters();
        ResetSetup();
        GameEventsManager.instance.gameEvents.ResetGame();

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
        GameEventsManager.instance.visualEvents.
            OnStartFadeCircle(true, () => { SceneManager.LoadScene(sceneName); });
    }
}
