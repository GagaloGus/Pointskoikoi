using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Points")]
    public int offsetPoints;
    public int originalPointAmount = 30;
    public int koiCounter = 1, maxRounds = 6;
    public Color[] koiIncreaseColors;
    public int roundCount;

    [Header("References")]
    public GameObject pointTransfer;

    GameObject FinishText, leftButtons, rightButtons, meses;
    TMP_Text leftPointText, rightPointText, koiText, roundCountText;
    Button LB_1pt, LB_5pt, LB_10pt, RB_1pt, RB_5pt, RB_10pt, KoiButton, EndRoundButton;

    public static readonly List<string> MONTHS = new List<string>() { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

    public static readonly string ROUNDCOUNT_KEY = "ROUNDKEY", ROUNDTEXT_KEY = "ROUNDTEXTKEY", OFFSETPOINTS_KEY = "OFFSET";

    private void Awake()
    {
        Transform PanelCont = FindObjectOfType<Canvas>().transform.Find("Panel Contadores");

        FinishText = PanelCont.Find("FINISH").gameObject;
        leftButtons = PanelCont.Find("LeftPointsButton").gameObject;
        rightButtons = PanelCont.Find("RightPointsButton").gameObject;
        meses = PanelCont.Find("Month").gameObject;
        leftPointText = PanelCont.Find("LeftPointsDisplay").GetComponentInChildren<TMP_Text>();
        rightPointText = PanelCont.Find("RightPointsDisplay").GetComponentInChildren<TMP_Text>();
        koiText = PanelCont.Find("KoiText").GetComponent<TMP_Text>();
        roundCountText = PanelCont.Find("RoundCount").GetComponent<TMP_Text>();
        KoiButton = PanelCont.Find("KoiButton").GetComponent<Button>();
        EndRoundButton = PanelCont.Find("EndRoundButton").GetComponent<Button>();
        LB_1pt = leftButtons.transform.Find("+1").GetComponent<Button>();
        LB_5pt = leftButtons.transform.Find("+5").GetComponent<Button>();
        LB_10pt = leftButtons.transform.Find("+10").GetComponent<Button>();
        RB_1pt = rightButtons.transform.Find("+1").GetComponent<Button>();
        RB_5pt = rightButtons.transform.Find("+5").GetComponent<Button>();
        RB_10pt = rightButtons.transform.Find("+10").GetComponent<Button>();

        LB_1pt.onClick.AddListener(() => { SetPoints(-1); });
        LB_5pt.onClick.AddListener(() => { SetPoints(-5); });
        LB_10pt.onClick.AddListener(() => { SetPoints(-10); });
        RB_1pt.onClick.AddListener(() => { SetPoints(1); });
        RB_5pt.onClick.AddListener(() => { SetPoints(5); });
        RB_10pt.onClick.AddListener(() => { SetPoints(10); });

        KoiButton.onClick.RemoveAllListeners();
        KoiButton.onClick.AddListener(() => { Koi(); });

        leftPointText.text = originalPointAmount.ToString();
        rightPointText.text = originalPointAmount.ToString();
        ResetGame();
    }

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

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

    private void OnApplicationQuit()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;


        PlayerPrefs.SetInt(OFFSETPOINTS_KEY, offsetPoints);
        PlayerPrefs.SetInt(ROUNDCOUNT_KEY, roundCount);
        PlayerPrefs.SetString(ROUNDTEXT_KEY, roundCountText.text);
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

    public void ResetGame()
    {
        offsetPoints = 0;
        roundCount = 1;

        roundCountText.text = "";

        EndRoundButton.enabled = true;

        StopAllCoroutines();
        StartCoroutine(PointNumberCounting(0.05f));

        ResetSetup();
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
        LB_1pt.enabled = enable;
        LB_5pt.enabled = enable;
        LB_10pt.enabled = enable;
        RB_1pt.enabled = enable;
        RB_5pt.enabled = enable;
        RB_10pt.enabled = enable;
        KoiButton.enabled = enable;
    }

    public void Koi()
    {
        int index = koiCounter % koiIncreaseColors.Length;

        koiCounter++;
        koiText.color = koiIncreaseColors[index];

        koiText.text = $"<size=60>koi count</size>\r\nx{koiCounter}";
    }

    IEnumerator PointNumberCounting(float speed)
    {
        Animator L_anim = leftPointText.gameObject.GetComponent<Animator>(), R_anim = rightPointText.gameObject.GetComponent<Animator>();

        int L_newNum = originalPointAmount - offsetPoints, R_newNum = originalPointAmount + offsetPoints,
            L_oldNum = int.Parse(leftPointText.text), R_oldNum = int.Parse(rightPointText.text);

        float speedRed = Mathf.Abs(L_newNum - L_oldNum);

        if (speedRed <= 0) { speedRed = 1; }
        speedRed = 1 / (Mathf.Log(speedRed, 8));

        while (L_oldNum != L_newNum)
        {
            if (L_oldNum < L_newNum)
            {
                L_oldNum++; R_oldNum--;
            }
            else if (L_oldNum > L_newNum)
            {
                L_oldNum--; R_oldNum++;
            }

            leftPointText.text = L_oldNum.ToString();
            rightPointText.text = R_oldNum.ToString();

            if (int.Parse(leftPointText.text) > originalPointAmount * 2) { leftPointText.text = (originalPointAmount * 2).ToString(); }
            else if (int.Parse(leftPointText.text) < 0) { leftPointText.text = 0.ToString(); }
            else
            {
                L_anim.SetTrigger("bounce");

            }

            if (int.Parse(rightPointText.text) > originalPointAmount * 2) { rightPointText.text = (originalPointAmount * 2).ToString(); }
            else if (int.Parse(rightPointText.text) < 0) { rightPointText.text = 0.ToString(); }
            else
            {
                R_anim.SetTrigger("bounce");

            }

            yield return new WaitForSeconds(speed * speedRed);
        }
    }

}
