using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//Lleva el contador de las rondas, los botones
public class GameHandler : MonoBehaviour
{
    GameObject FinishText, meses, roundAnnounceText, roundEndText;
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

        meses = PanelCont.Find("Month").gameObject;
        FinishText = PanelCont.Find("FINISH").gameObject;

        roundAnnounceText.SetActive(false);
        roundEndText.SetActive(false);
    }

    private void Start()
    {
        GameManager.instance.StartUpGame();
        ShowRoundText(1);
    }

    void ShowRoundText(int round)
    {
        roundAnnounceText.SetActive(true);
        roundAnnounceText.GetComponentInChildren<TMP_Text>().text = $"Ronda {round}";
        roundAnnounceText.GetComponent<Animator>().SetTrigger("a");
    }

    public void EndRound()
    {
        roundEndText.SetActive(true);
        EnableButtons(false);
    }

    public void ChangeRound(int round)
    {
        ShowRoundText(round);
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
    }

    void ResetSetup()
    {
        EnableButtons(true);

        ChangeMonthCards();
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
