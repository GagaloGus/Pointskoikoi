using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Lleva el contador de las rondas, los botones
public class GameHandler : MonoBehaviour
{
    GameObject roundAnnounceText, roundEndText, finalScore;
    Button KoiButton, EndRoundButton, AddPointsButton, p1addpt, p2addpt;

    private void OnEnable()
    {
        GameEventsManager.instance.gameEvents.onWin += Win;
        GameEventsManager.instance.gameEvents.onRoundChange += ChangeRound;
        GameEventsManager.instance.gameEvents.onRoundEnd += EndRound;
        GameEventsManager.instance.gameEvents.resetSetup += ResetSetup;
        GameEventsManager.instance.gameEvents.resetGame += ResetGame;
        GameEventsManager.instance.gameEvents.onPointsAdded += AddPoints;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.gameEvents.onWin -= Win;
        GameEventsManager.instance.gameEvents.onRoundChange -= ChangeRound;
        GameEventsManager.instance.gameEvents.onRoundEnd -= EndRound;
        GameEventsManager.instance.gameEvents.resetSetup -= ResetSetup;
        GameEventsManager.instance.gameEvents.resetGame -= ResetGame;
        GameEventsManager.instance.gameEvents.onPointsAdded -= AddPoints;
    }

    private void Awake()
    {
        Transform PanelCont = GameObject.FindGameObjectWithTag("PanelPrincipal").transform;

        roundAnnounceText = PanelCont.parent.Find("Round").gameObject;
        roundEndText = PanelCont.parent.Find("RoundEnd").gameObject;

        KoiButton = PanelCont.Find("KoiButton").GetComponent<Button>();
        EndRoundButton = PanelCont.Find("EndRoundButton").GetComponent<Button>();
        AddPointsButton = PanelCont.Find("AddPointsButton").GetComponent<Button>();
        p1addpt = PanelCont.Find("AddLeftPoints").GetComponent<Button>();
        p2addpt = PanelCont.Find("AddRightPoints").GetComponent<Button>();

        finalScore = FindObjectOfType<FinalScore>(true).gameObject;

        roundAnnounceText.SetActive(false);
        roundEndText.SetActive(false);
        finalScore.SetActive(false);
    }

    private void Start()
    {
        GameManager.instance.StartUpGame();
        GameEventsManager.instance.visualEvents.OnStartFadeCircle(false);

        ShowRoundText(1);
    }

    void ShowRoundText(int round)
    {
        string month = GameManager.MONTHS[Mathf.Clamp(GameManager.instance.round - 1, 0, 11)];

        roundAnnounceText.SetActive(true);
        roundAnnounceText.GetComponentInChildren<TMP_Text>().text = $"Ronda {round}\n<size=70>{month}";
        roundAnnounceText.GetComponent<Animator>().SetTrigger("a");

        FindObjectOfType<MonthCards>().ChangeCardTextures();
    }
    
    public void EndRound()
    {
        /*roundEndText.SetActive(true);
        EnableButtons(false);*/
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

    public void EnableButtons(bool enable)
    {
        KoiButton.interactable = enable;
        EndRoundButton.interactable = enable;
        AddPointsButton.interactable = enable;
        p1addpt.interactable = enable;
        p2addpt.interactable = enable;
    }

    void Win(Win_States win)
    {
        EnableButtons(false);

        finalScore.SetActive(true);
    }

}
