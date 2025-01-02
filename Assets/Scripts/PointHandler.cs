using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointHandler : MonoBehaviour
{
    Transform PanelJuego;

    [Header("References")]
    public GameObject pointTransfer, PointPanel;

    TMP_Text player1Pt, player2Pt, player1ptAdd, player2ptAdd;
    public int finalPoints;

    Button p1addpt, p2addpt;

    [SerializeField]
    List<CombiPointsPair> 
        pairsP1 = new List<CombiPointsPair>(),
        pairsP2 = new List<CombiPointsPair>();

    GameHandler gameHandler;
    bool resetGame;

    private void OnEnable()
    {
        GameEventsManager.instance.gameEvents.onPointsAdded += AddPoints;
        GameEventsManager.instance.gameEvents.resetGame += ResetGame;
        GameEventsManager.instance.gameEvents.resetSetup += ResetSetup;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.gameEvents.onPointsAdded -= AddPoints;
        GameEventsManager.instance.gameEvents.resetGame -= ResetGame;
        GameEventsManager.instance.gameEvents.resetSetup += ResetSetup;
    }

    private void Awake()
    {
        PanelJuego = GameObject.FindGameObjectWithTag("PanelJuego").transform;
        Transform PanelCont = GameObject.FindGameObjectWithTag("PanelPrincipal").transform;
        Transform p1 = PanelCont.Find("LeftPointsDisplay"), p2 = PanelCont.Find("RightPointsDisplay");

        PointPanel = FindObjectOfType<CombiChosePanel>(true).gameObject;
        player1Pt = p1.Find("point").GetComponent<TMP_Text>();
        player2Pt = p2.Find("point").GetComponent<TMP_Text>();
        player1ptAdd = p1.Find("point_add").GetComponent<TMP_Text>();
        player2ptAdd = p2.Find("point_add").GetComponent<TMP_Text>();

        p1addpt = PanelCont.Find("AddLeftPoints").GetComponent<Button>();
        p2addpt = PanelCont.Find("AddRightPoints").GetComponent<Button>();

        gameHandler = GetComponent<GameHandler>();
    }

    void Start()
    {
        player1Pt.text = GameManager.instance.originalPts.ToString();
        player2Pt.text = GameManager.instance.originalPts.ToString();
        player1ptAdd.gameObject.SetActive(false);
        player2ptAdd.gameObject.SetActive(false);
        PointPanel.SetActive(false);
    }

    void AddPoints()
    {
        resetGame = false;
        Animator t = (GameManager.instance.p1Choose ? player1ptAdd : player2ptAdd).GetComponent<Animator>();

        t.SetBool("koi", GameManager.instance.koi > 1);

        //Mueve el textokoi
        if(GameManager.instance.koi > 1)
        {
            PanelJuego.GetComponent<Animator>().SetTrigger("zoomkoi");
            FindObjectOfType<KoiText>().Tingle();
        }
        else
        {
            t.SetTrigger("add");
        }
    }

    //Llamado por el evento de animacion del koitext
    public void KoiText_IncreasePts()
    {
        StartCoroutine(IncreasePointsToAdd_Corr());
    }

    //multplica los puntos a añadir por el koi
    IEnumerator IncreasePointsToAdd_Corr()
    {
        TMP_Text t = (GameManager.instance.p1Choose ? player1ptAdd : player2ptAdd);

        int originalpt = Mathf.Abs(GameManager.instance.pointsToAdd/GameManager.instance.koi),
            finalpt = Mathf.Abs(GameManager.instance.pointsToAdd);

        float speedRed = Mathf.Abs(finalpt - originalpt)/3;

        while (originalpt != finalpt) 
        {
            originalpt++;
            t.GetComponent<Animator>().SetTrigger("koiadded");

            t.text = $"+{originalpt}";

            yield return new WaitForSeconds(0.2f/speedRed);
        }

        yield return new WaitForSeconds(0.5f);
        t.GetComponent<Animator>().SetTrigger("pointsadded");
    }

    void ResetGame()
    {
        resetGame = true;
        UpdateUIPoints();
    }

    void ResetSetup()
    {
        pairsP1 = new List<CombiPointsPair>();
        pairsP2 = new List<CombiPointsPair>();
        player1ptAdd.gameObject.SetActive(false);
        player2ptAdd.gameObject.SetActive(false);
        finalPoints = 0;
    }

    public void ActivatePointPanel(bool player1)
    {
        gameHandler.EnableButtons(false);
        GameManager.instance.p1Choose = player1;

        PointPanel.SetActive(true);

        List<CombiPointsPair> p = (player1 ? pairsP1 : pairsP2);
        PointPanel.GetComponent<CombiChosePanel>().ActivateChoices(p);
    }

    public void CancelHoldPoints()
    {
        gameHandler.EnableButtons(true);
        GameManager.instance.p1Choose = GameManager.instance.p1LastChoose;
        print("Cancelao");
    }

    public void HoldPoints(List<CombiPointsPair> pairs, int pts)
    {
        gameHandler.EnableButtons(true);
        GameManager.instance.p1LastChoose = GameManager.instance.p1Choose;
        GameManager.instance.pointsToAdd = pts * (GameManager.instance.p1Choose ? -1 : 1);
        finalPoints = pts;

        if (GameManager.instance.p1Choose)
        {
            pairsP1 = new List<CombiPointsPair>(pairs);
        }
        else
        {
            pairsP2 = new List<CombiPointsPair>(pairs);
        }

        if (pts != 0)
        {
            TMP_Text t = (GameManager.instance.p1Choose ? player1ptAdd : player2ptAdd);

            player1ptAdd.gameObject.SetActive(GameManager.instance.p1Choose);
            player2ptAdd.gameObject.SetActive(!GameManager.instance.p1Choose);

            t.text = "+" + pts;
        }
        else
        {
            player1ptAdd.gameObject.SetActive(false);
            player2ptAdd.gameObject.SetActive(false);
        }
        
    }

    public void UpdateUIPoints()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateUIPointsCoroutine());
    }

    IEnumerator UpdateUIPointsCoroutine()
    {
        //Si pointdif es negativo -> gana player1 (izquierda)
        int pointDif = GameManager.instance.pts1 - GameManager.instance.pts2;
        bool player1 = pointDif < 0;

        if (!resetGame)
        {
            GameEventsManager.instance.visualEvents.OnPointsAddedToDisplay(Color.green);
        }

        yield return StartCoroutine(PointNumberCounting());
        yield return new WaitForSeconds(1);

        if (!resetGame)
        {
            GameManager.instance.AfterPointsUI();
        }
    }

    //Hace la animacion de a�adir puntos a los contadores progresivamente
    IEnumerator PointNumberCounting()
    {
        int originalPointAmount = GameManager.instance.originalPts,
            offsetPoints = GameManager.instance.offsetPoints;

        Animator L_anim = player1Pt.gameObject.GetComponent<Animator>(), 
                 R_anim = player2Pt.gameObject.GetComponent<Animator>();

        int L_newNum = originalPointAmount - offsetPoints, 
            R_newNum = originalPointAmount + offsetPoints,
            L_oldNum = int.Parse(player1Pt.text), 
            R_oldNum = int.Parse(player2Pt.text);

        float speedRed = Mathf.Abs(L_newNum - L_oldNum)/3;

        if (speedRed <= 0) { speedRed = 1; }

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

            player1Pt.text = L_oldNum.ToString();
            player2Pt.text = R_oldNum.ToString();

            L_anim.SetTrigger("bounce");
            R_anim.SetTrigger("bounce");

            if(int.Parse(player1Pt.text) > originalPointAmount * 2)
            {
                player1Pt.text = (originalPointAmount * 2).ToString();
                player2Pt.text = 0.ToString();
                break;
            }
            else if(int.Parse(player1Pt.text) < 0)
            {
                player1Pt.text = 0.ToString();
                player2Pt.text = (originalPointAmount * 2).ToString();
                break;
            }

            yield return new WaitForSeconds(0.3f/speedRed);
        }
    }
}
