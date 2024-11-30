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

    public int pts1, pts2, offsetPoints, originalPts = 30, round, maxRounds, koi;
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

        offsetPoints = 0;
        pts1 = pts2 = offsetPoints;
        p1LastChoose = true;
    }

    public void Koi()
    {
        koi++;

        GameEventsManager.instance.gameEvents.Koi();
    }

    public void AddPoints(List<CombiPointsPair> pairs)
    {
        int pts = 0;
        foreach (CombiPointsPair combi in pairs)
        {
            pts += combi.combi.points + combi.extraCards;
        }
        offsetPoints += pts;
        AddPoints();
    }
    public void AddPoints()
    {
        //Si pointdif es negativo -> gana player1 (izquierda)
        int pointDif = offsetPoints * (p1Choose ? -1 : 1);

        pts1 -= pointDif;
        pts2 += pointDif;

        GameEventsManager.instance.gameEvents.OnPointsAdded();

        CoolFunctions.Invoke(this,() =>
        {
            if (pointDif >= Mathf.Abs(originalPts))
            {
                GameEventsManager.instance.gameEvents.OnWin(Get_WinCondition());
            }
            else
            {
                NextRound();
            }
        }, 1);

        
    }

    public void NextRound()
    {
        round++;
        GameEventsManager.instance.gameEvents.OnRoundChange(round);
        if(round >= maxRounds)
        {
            GameEventsManager.instance.gameEvents.OnWin(Get_WinCondition());
        }
        else
        {
            ResetSetup();
        }
    }

    public void ResetSetup()
    {
        GameEventsManager.instance.gameEvents.ResetSetup();
    }

    public void ResetGame()
    {
        offsetPoints = 0;
        pts1 = pts2 = originalPts;
        round = 1;
        koi = 1;
        GameEventsManager.instance.gameEvents.ResetGame();
        GameEventsManager.instance.gameEvents.ResetSetup();
    }

    public Win_States Get_WinCondition()
    {
        Win_States win;

        if (Mathf.Abs(pts1) >= originalPts)
            win = Win_States.Player1;
        else if (Mathf.Abs(pts2) >= originalPts)
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


    /*private void Awake()
    {
        Transform PanelCont = FindObjectOfType<Canvas>().transform.Find("Panel Contadores");

        FinishText = PanelCont.Find("FINISH").gameObject;
        meses = PanelCont.Find("Month").gameObject;
        leftPointText = PanelCont.Find("LeftPointsDisplay").GetComponentInChildren<TMP_Text>();
        rightPointText = PanelCont.Find("RightPointsDisplay").GetComponentInChildren<TMP_Text>();
        koiText = PanelCont.Find("KoiText").GetComponent<TMP_Text>();
        roundCountText = PanelCont.Find("RoundCount").GetComponent<TMP_Text>();
        KoiButton = PanelCont.Find("KoiButton").GetComponent<Button>();
        EndRoundButton = PanelCont.Find("EndRoundButton").GetComponent<Button>();

        KoiButton.onClick.RemoveAllListeners();
        KoiButton.onClick.AddListener(() => { Koi(); });

        leftPointText.text = originalPointAmount.ToString();
        rightPointText.text = originalPointAmount.ToString();
        ResetSetup();
    }

    private void Start()
    {

        if (PlayerPrefs.HasKey(ROUNDTEXT_KEY)) { roundCountText.text = PlayerPrefs.GetString(ROUNDTEXT_KEY); }
        if (PlayerPrefs.HasKey(OFFSETPOINTS_KEY))
        {
            SetPoints(offsetPoints+PlayerPrefs.GetInt(OFFSETPOINTS_KEY));
        }
        if (PlayerPrefs.HasKey(ROUNDCOUNT_KEY))
        {
            roundCount = PlayerPrefs.GetInt(ROUNDCOUNT_KEY);
            ChangeMonthCards();
        }

    }



    public void SetPoints(int pointDif)
    {
        offsetPoints += pointDif;

        //offpoints = og negativo = gana Left
        StopAllCoroutines();
        StartCoroutine(PointNumberCounting(0.1f));

        Transform pt = Instantiate(pointTransfer).transform;
        pt.SetParent(FindObjectOfType<Canvas>().transform);
        pt.localScale = Vector3.one;
        pt.position = leftPointText.gameObject.transform.position;
        pt.GetComponent<Animator>().SetBool("reverse", pointDif < 0);
        pt.GetComponent<TMP_Text>().text = $"+{Mathf.Abs(pointDif)}";

        Debug.Log($"Added {Mathf.Abs(pointDif)} points to Player {(pointDif < 0 ? "1" : "2")}");

        if (offsetPoints <= -originalPointAmount || offsetPoints >= originalPointAmount)
        {
            Win();
        }
    }

    public void NextRound()
    {
        string color1 = "white", color2 = "white";

        if (offsetPoints < 0) { color1 = "green"; color2 = "red"; }
        else if (offsetPoints > 0) { color1 = "red"; color2 = "green"; }

        roundCountText.text += $"<b>Ronda {roundCount}</b>\r\n<color={color1}>{originalPointAmount - offsetPoints}</color> / <color={color2}>{originalPointAmount + offsetPoints}</color>\r\n\r\n";

        roundCount++;

        if (roundCount > maxRounds)
        {
            Win();
        }
        else
        {
            ResetSetup();
        }

    }

    void Win()
    {
        FinishText.SetActive(true);
        EnableButtons(false);
        EndRoundButton.enabled = false;

        if (offsetPoints < 0)
        {
            FinishText.transform.Find("1").gameObject.SetActive(true);
        }
        else if (offsetPoints > 0)
        {
            FinishText.transform.Find("2").gameObject.SetActive(true);
        }
        else
        {
            FinishText.transform.Find("E").gameObject.SetActive(true);
        }
    }



    void ResetSetup()
    {
        EnableButtons(true);

        FinishText.SetActive(false);
        FinishText.transform.Find("1").gameObject.SetActive(false);
        FinishText.transform.Find("2").gameObject.SetActive(false);
        FinishText.transform.Find("E").gameObject.SetActive(false);

        koiCounter = 1;
        koiText.color = Color.white;
        koiText.text = $"<size=60>koi count</size>\r\nx{koiCounter}";

        ChangeMonthCards();
    }

    void ChangeMonthCards()
    {
        int month = roundCount - 1;

        List<Sprite> CardSprites = FindObjectOfType<DisplayCombinations>(true).CardSprites;

        month = Mathf.Clamp(month, 0, 11);
        meses.transform.Find("Mes").GetComponent<TMP_Text>().text = MONTHS[month];

        for (int i = 0; i < 4; i++)
        {
            meses.transform.Find($"Card-{i}").GetComponent<Image>().sprite = CardSprites[i + (month * 4)];
        }
    }

    void EnableButtons(bool enable)
    {
        KoiButton.enabled = enable;
    }

    public void Koi()
    {
        int index = koiCounter % koiIncreaseColors.Length;

        koiCounter++;
        koiText.color = koiIncreaseColors[index];

        koiText.text = $"<size=60>koi count</size>\r\nx{koiCounter}";
    }

    
*/
}
