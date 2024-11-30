using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointHandler : MonoBehaviour
{
    [Header("References")]
    public GameObject pointTransfer, PointPanel;

    TMP_Text player1Pt, player2Pt, koiText, player1ptAdd, player2ptAdd;
    public int finalPoints;

    Button p1addpt, p2addpt;

    [SerializeField]
    List<CombiPointsPair> 
        pairsP1 = new List<CombiPointsPair>(),
        pairsP2 = new List<CombiPointsPair>();

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
        Transform PanelCont = FindObjectOfType<Canvas>().transform.Find("Panel Contadores");
        Transform p1 = PanelCont.Find("LeftPointsDisplay"), p2 = PanelCont.Find("RightPointsDisplay");

        PointPanel = FindObjectOfType<CombiChosePanel>(true).gameObject;
        player1Pt = p1.Find("point").GetComponent<TMP_Text>();
        player2Pt = p2.Find("point").GetComponent<TMP_Text>();
        player1ptAdd = p1.Find("point_add").GetComponent<TMP_Text>();
        player2ptAdd = p2.Find("point_add").GetComponent<TMP_Text>();

        p1addpt = PanelCont.Find("AddLeftPoints").GetComponent<Button>();
        p2addpt = PanelCont.Find("AddRightPoints").GetComponent<Button>();
    }

    void Start()
    {
        player1ptAdd.gameObject.SetActive(false);
        player2ptAdd.gameObject.SetActive(false);
        PointPanel.gameObject.SetActive(false);
    }

    void AddPoints()
    {
        (GameManager.instance.p1Choose ? player1ptAdd : player2ptAdd).
            GetComponent<Animator>().SetTrigger("add");
        float t = CalculatePointCountingDuration(0.1f);
        print(t);
        CoolFunctions.Invoke(this, () =>
        {
            GameManager.instance.AfterPointsUI();
        }, t+1);
    }

    float CalculatePointCountingDuration(float speed)
    {
        // Diferencia absoluta entre los puntos iniciales y finales
        int totalSteps = Mathf.Abs(GameManager.instance.pointsToAdd) * 2 + 1;

        // Velocidad reducida promedio basada en el rango de valores
        float averageSpeedRed = 1 / Mathf.Log(Mathf.Max(totalSteps / 2f, 1), 8);

        // Tiempo total es pasos multiplicados por tiempo por paso
        return totalSteps * speed * averageSpeedRed;
    }

    void ResetGame()
    {
        player1Pt.text = GameManager.instance.originalPts.ToString();
        player2Pt.text = GameManager.instance.originalPts.ToString();
    }

    void ResetSetup()
    {
        pairsP1 = new List<CombiPointsPair>();
        pairsP2 = new List<CombiPointsPair>();
        player1ptAdd.gameObject.SetActive(false);
        player2ptAdd.gameObject.SetActive(false);
        UpdateUIPoints();
    }

    public void ActivatePointPanel(bool player1)
    {
        GameManager.instance.p1Choose = player1;

        PointPanel.SetActive(true);

        List<CombiPointsPair> p = (player1 ? pairsP1 : pairsP2);
        PointPanel.GetComponent<CombiChosePanel>().ActivateChoices(p);

        print("Activao");
    }

    public void CancelHoldPoints()
    {
        GameManager.instance.p1Choose = GameManager.instance.p1LastChoose;
        print("Cancelao");
    }

    public void HoldPoints(List<CombiPointsPair> pairs, int pts)
    {
        GameManager.instance.p1LastChoose = GameManager.instance.p1Choose;
        GameManager.instance.pointsToAdd = pts * (GameManager.instance.p1Choose ? -1 : 1); ;
        print($"Holdear puntos {pairs.Count}");
        TMP_Text t = (GameManager.instance.p1Choose ? player1ptAdd : player2ptAdd);

        player1ptAdd.gameObject.SetActive(GameManager.instance.p1Choose);
        player2ptAdd.gameObject.SetActive(!GameManager.instance.p1Choose);

        t.text = "+" + pts;

        if (GameManager.instance.p1Choose)
        {
            pairsP1 = new List<CombiPointsPair>(pairs);
        }
        else
        {
            pairsP2 = new List<CombiPointsPair>(pairs);
        }
    }

    Coroutine pointNumberCountingCoroutine;

    public void UpdateUIPoints()
    {
        //Si pointdif es negativo -> gana player1 (izquierda)
        int pointDif = GameManager.instance.pts1 - GameManager.instance.pts2;
        bool player1 = pointDif < 0;

        if(pointNumberCountingCoroutine != null)
        {
            StopCoroutine(pointNumberCountingCoroutine);
            pointNumberCountingCoroutine = null;
        }

        pointNumberCountingCoroutine = StartCoroutine(PointNumberCounting(0.1f));

        /*//Lanza un numerito entre puntuaciones
        Transform pt = Instantiate(pointTransfer).transform;
        pt.SetParent(FindObjectOfType<Canvas>().transform);
        pt.localScale = Vector3.one;
        pt.position = player1Pt.gameObject.transform.position;
        pt.GetComponent<Animator>().SetBool("reverse", player1);
        pt.GetComponent<TMP_Text>().text = $"+{Mathf.Abs(pointDif)}";

        Debug.Log($"Added {Mathf.Abs(pointDif)} points to Player {(player1 ? "1" : "2")}");*/
    }

    //Hace la animacion de añadir puntos a los contadores progresivamente
    IEnumerator PointNumberCounting(float speed)
    {
        int originalPointAmount = GameManager.instance.originalPts,
            offsetPoints = GameManager.instance.offsetPoints;

        Animator L_anim = player1Pt.gameObject.GetComponent<Animator>(), 
                 R_anim = player2Pt.gameObject.GetComponent<Animator>();

        int L_newNum = originalPointAmount - offsetPoints, 
            R_newNum = originalPointAmount + offsetPoints,
            L_oldNum = int.Parse(player1Pt.text), 
            R_oldNum = int.Parse(player2Pt.text);

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

            player1Pt.text = L_oldNum.ToString();
            player2Pt.text = R_oldNum.ToString();

            if (int.Parse(player1Pt.text) > originalPointAmount * 2) { player1Pt.text = (originalPointAmount * 2).ToString(); }
            else if (int.Parse(player1Pt.text) < 0) { player1Pt.text = 0.ToString(); }
            else
            {
                L_anim.SetTrigger("bounce");

            }

            if (int.Parse(player2Pt.text) > originalPointAmount * 2) { player2Pt.text = (originalPointAmount * 2).ToString(); }
            else if (int.Parse(player2Pt.text) < 0) { player2Pt.text = 0.ToString(); }
            else
            {
                R_anim.SetTrigger("bounce");

            }

            yield return new WaitForSeconds(speed * speedRed);
        }

        print("Se termino la corrutina");
    }
}
