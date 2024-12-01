using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//Lleva el contador de las rondas y el koi
public class GameHandler : MonoBehaviour
{
    TMP_Text koiText, roundCountText, player1Pt, player2Pt;
    GameObject FinishText, meses, roundAnnounceText;
    Button KoiButton, EndRoundButton, AddPointsButton, p1addpt, p2addpt;

    private void OnEnable()
    {
        GameEventsManager.instance.gameEvents.onWin += Win;
        GameEventsManager.instance.gameEvents.onRoundChange += ChangeRound;
        GameEventsManager.instance.gameEvents.resetSetup += ResetSetup;
        GameEventsManager.instance.gameEvents.resetGame += ResetGame;
        GameEventsManager.instance.gameEvents.koi += Koi;
        GameEventsManager.instance.gameEvents.onPointsAdded += AddPoints;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.gameEvents.onWin -= Win;
        GameEventsManager.instance.gameEvents.onRoundChange -= ChangeRound;
        GameEventsManager.instance.gameEvents.resetSetup -= ResetSetup;
        GameEventsManager.instance.gameEvents.resetGame -= ResetGame;
        GameEventsManager.instance.gameEvents.koi -= Koi;
        GameEventsManager.instance.gameEvents.onPointsAdded -= AddPoints;
    }

    private void Awake()
    {
        Transform PanelCont = transform.Find("Panel Contadores");

        KoiButton = PanelCont.Find("KoiButton").GetComponent<Button>();
        EndRoundButton = PanelCont.Find("EndRoundButton").GetComponent<Button>();
        AddPointsButton = PanelCont.Find("AddPointsButton").GetComponent<Button>();
        p1addpt = PanelCont.Find("AddLeftPoints").GetComponent<Button>();
        p2addpt = PanelCont.Find("AddRightPoints").GetComponent<Button>();

        roundCountText = PanelCont.Find("RoundCount").GetComponent<TMP_Text>();
        roundAnnounceText = transform.Find("Round").gameObject;
        koiText = PanelCont.Find("KoiText").GetComponent<TMP_Text>();
        meses = PanelCont.Find("Month").gameObject;
        FinishText = PanelCont.Find("FINISH").gameObject;
    }

    private void Start()
    {
        GameManager.instance.ResetGame();
        ShowRoundText(1);
    }

    void ShowRoundText(int round)
    {
        roundAnnounceText.SetActive(true);
        roundAnnounceText.GetComponentInChildren<TMP_Text>().text = $"Ronda {round}";
        roundAnnounceText.GetComponent<Animator>().SetTrigger("a");
    }


    public void ChangeRound(int round)
    {
        ShowRoundText(round);

        int originalPointAmount = GameManager.instance.originalPts,
            offsetPoints = GameManager.instance.offsetPoints;

        string color1 = "white", color2 = "white";

        if (offsetPoints < 0) { color1 = "green"; color2 = "red"; }
        else if (offsetPoints > 0) { color1 = "red"; color2 = "green"; }

        roundCountText.text = $"<b>Ronda {round}</b>\n<color={color1}>{originalPointAmount - offsetPoints}</color> / <color={color2}>{originalPointAmount + offsetPoints}</color>\n\n";
    }

    void ChangeMonthCards()
    {
        int month = GameManager.instance.round - 1;

        List<Sprite> CardSprites = GameManager.instance.CardSprites;

        month = Mathf.Clamp(month, 0, 11);
        meses.transform.Find("Mes").GetComponent<TMP_Text>().text = GameManager.MONTHS[month];

        for (int i = 0; i < 4; i++)
        {
            meses.transform.Find($"Card-{i}").GetComponent<Image>().sprite = CardSprites[i + (month * 4)];
        }
    }

    void AddPoints()
    {
        EnableButtons(false);
    }

    void ResetGame()
    {
        FinishText.SetActive(false);
        FinishText.transform.Find("1").gameObject.SetActive(false);
        FinishText.transform.Find("2").gameObject.SetActive(false);
        FinishText.transform.Find("E").gameObject.SetActive(false);

        roundCountText.text = $"<b>Ronda 1</b>\n";
    }

    void ResetSetup()
    {
        EnableButtons(true);

        koiText.color = Color.white;
        koiText.text = $"<size=60>koi count</size>\nx1";

        ChangeMonthCards();
    }

    void Koi()
    {
        Color[] koiColors = GameManager.instance.koiColors;

        int index = GameManager.instance.koi % koiColors.Length;

        koiText.color = koiColors[index];

        koiText.text = $"<size=60>koi count</size>\nx{GameManager.instance.koi}";
    }

    public void EnableButtons(bool enable)
    {
        KoiButton.enabled = enable;
        EndRoundButton.enabled = enable;
        AddPointsButton.enabled = enable;
        p1addpt.enabled = enable;
        p2addpt.enabled = enable;
    }

    void Win(Win_States win)
    {
        FinishText.SetActive(true);
        EndRoundButton.enabled = false;

        switch (win)
        {
            case Win_States.Player1:
                FinishText.transform.Find("1").gameObject.SetActive(true);
                break;
            case Win_States.Player2:
                FinishText.transform.Find("2").gameObject.SetActive(true);
                break;
            case Win_States.Tie:
                FinishText.transform.Find("E").gameObject.SetActive(true);
                break;
        }
    }

}
