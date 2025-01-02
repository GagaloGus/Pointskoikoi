using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KoiText : MonoBehaviour
{
    Color[] koiColors;
    TMP_Text text, textGlow;
    Animator animator;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        textGlow = transform.GetChild(0).GetComponent<TMP_Text>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.gameEvents.koi += Koi;
        GameEventsManager.instance.gameEvents.resetGame += ResetGame;
        GameEventsManager.instance.gameEvents.resetSetup += ResetSetup;        
    }

    private void OnDisable()
    {
        GameEventsManager.instance.gameEvents.koi -= Koi;
        GameEventsManager.instance.gameEvents.resetGame -= ResetGame;
        GameEventsManager.instance.gameEvents.resetSetup -= ResetSetup;
    }


    private void Start()
    {
        koiColors = GameManager.instance.koiColors;
    }

    void ResetGame()
    {

    }

    void ResetSetup()
    {
        text.color = Color.white;
        text.text = $"<size=80>koi</size>\nx1";
        animator.SetTrigger("reset");
    }

    void Koi()
    {
        int index = GameManager.instance.koi % koiColors.Length;
        text.color = koiColors[index];
        text.text = $"<size=80>koi</size>\nx{GameManager.instance.koi}";
        textGlow.text = text.text;
    }

    //Cuando se acaba la ronda y hay mas de x1 koi, hace una animacion
    public void Tingle()
    {
        animator.SetTrigger("addkoi");
    }

    //Cuanto termina, manda un mensaje al pointhandler
    public void SendEndTingleMessage()
    {
        FindObjectOfType<PointHandler>().KoiText_IncreasePts();
    }
}
