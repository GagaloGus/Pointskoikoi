using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//Lleva el contador de las rondas, los botones
public class GameHandler : MonoBehaviour
{
    GameObject roundAnnounceText, gameModeText, finalScore;
    Button p1addpt, p2addpt;
    Transform roundButtonsParent, PanelCont;

    private void OnEnable()
    {
        GameEventsManager.instance.gameEvents.onWin += Win;
        GameEventsManager.instance.gameEvents.onRoundChange += ChangeRound;
        GameEventsManager.instance.gameEvents.resetSetup += ResetSetup;
        GameEventsManager.instance.gameEvents.resetGame += ResetGame;
        GameEventsManager.instance.gameEvents.onPointsAdded += AddPoints;
        GameEventsManager.instance.gameEvents.onStartGame += StartGame;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.gameEvents.onWin -= Win;
        GameEventsManager.instance.gameEvents.onRoundChange -= ChangeRound;
        GameEventsManager.instance.gameEvents.resetSetup -= ResetSetup;
        GameEventsManager.instance.gameEvents.resetGame -= ResetGame;
        GameEventsManager.instance.gameEvents.onPointsAdded -= AddPoints;
        GameEventsManager.instance.gameEvents.onStartGame -= StartGame;
    }

    private void Awake()
    {
        PanelCont = GameObject.FindGameObjectWithTag("PanelPrincipal").transform;

        roundAnnounceText = PanelCont.parent.Find("Round").gameObject;
        gameModeText = PanelCont.parent.Find("GameMode").gameObject;

        roundButtonsParent = PanelCont.Find("RoundButtons");
        p1addpt = PanelCont.Find("AddLeftPoints").GetComponent<Button>();
        p2addpt = PanelCont.Find("AddRightPoints").GetComponent<Button>();

        finalScore = FindObjectOfType<FinalScore>(true).gameObject;

        roundAnnounceText.SetActive(false);
        gameModeText.SetActive(false);
        finalScore.SetActive(false);
    }

    private void Start()
    {
        GameEventsManager.instance.visualEvents.OnStartFadeCircle(false);
    }

    public void StartGame()
    {      
        ShowRoundText(1);

        PanelCont.GetComponent<Animator>().SetFloat("speed", 1);
        if(GameManager.instance.gameMode == GameMode.Classic)
        {
            roundButtonsParent.Find("Koi").gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Actualiza la informacion de la ronda al empezar una nueva
    /// </summary>
    /// <param name="round"></param>
    void ShowRoundText(int round)
    {
        string month = GameManager.MONTHS[Mathf.Clamp(GameManager.instance.round - 1, 0, 11)];

        roundAnnounceText.SetActive(true);
        roundAnnounceText.GetComponentInChildren<TMP_Text>().text = $"Ronda {round}\n<size=70>{month}";
        roundAnnounceText.GetComponent<Animator>().SetTrigger("a");

        FindObjectOfType<MonthCards>().ChangeCardTextures();

        if(round == 1)
        {
            roundAnnounceText.GetComponent<Animator>().SetFloat("speed", 0.6f);
            gameModeText.GetComponent<Animator>().SetFloat("speed", 0.6f);

            CoolFunctions.Invoke(this, () =>
            {
                gameModeText.SetActive(true);
                gameModeText.GetComponentInChildren<TMP_Text>().text = $"- {CoolFunctions.Get_GamemodeName(GameManager.instance.gameMode)} -";
                gameModeText.GetComponent<Animator>().SetTrigger("a");
            }, 0.35f);
        }
        else
        {
            roundAnnounceText.GetComponent<Animator>().SetFloat("speed", 1);
        }
    }

    public void ChangeRound(int round)
    {
        ShowRoundText(round);
    }

    void AddPoints()
    {
        EnableButtons(false);
    }

    void ResetGame()
    {
        finalScore.SetActive(false);
        ShowRoundText(1);
    }

    void ResetSetup()
    {
        EnableButtons(true);
    }

    /// <summary>
    /// Activa o desactiva botones
    /// </summary>
    /// <param name="enable"></param>
    public void EnableButtons(bool enable)
    {
        foreach(Transform t in roundButtonsParent)
            t.GetComponent<Button>().interactable = enable;

        p1addpt.interactable = enable;
        p2addpt.interactable = enable;
    }

    void Win(Win_States win)
    {
        EnableButtons(false);

        finalScore.SetActive(true);
    }

}
