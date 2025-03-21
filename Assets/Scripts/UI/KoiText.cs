using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class KoiText : MonoBehaviour
{
    Color[] koiColors;
    TMP_Text text;
    Animator animator;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        animator = GetComponentInChildren<Animator>();
    }
    private void OnEnable()
    {
        GameEventsManager.instance.gameEvents.koi += Koi;
        GameEventsManager.instance.gameEvents.onStartGame += StartUpGame;
        GameEventsManager.instance.gameEvents.resetSetup += ResetSetup;        
    }

    private void OnDisable()
    {
        GameEventsManager.instance.gameEvents.koi -= Koi;
        GameEventsManager.instance.gameEvents.onStartGame -= StartUpGame;
        GameEventsManager.instance.gameEvents.resetSetup -= ResetSetup;
    }

    private void Start()
    {
        koiColors = GameManager.instance.koiColors;
        text.gameObject.SetActive(false);
    }

    void StartUpGame()
    {
        if(GameManager.instance.gameMode == GameMode.PointThief) 
        {
            text.gameObject.SetActive(true);
            ChangeText(true);
        }
    }

    void ResetSetup()
    {
        ChangeText(true);
        ChangeChildText();
        animator.SetTrigger("increasekoi");
    }

    void Koi()
    {
        ChangeText(false);
        ChangeChildText();
        animator.SetTrigger("increasekoi");
    }

    void ChangeText(bool reset)
    {
        text.color = reset ? Color.white : koiColors[GameManager.instance.koi % koiColors.Length];
        text.text = $"<size=80>koi</size>\nx{GameManager.instance.koi}";
    }

    //Cuando se acaba la ronda y hay mas de x1 koi, hace una animacion
    public void Boom()
    {
        animator.SetTrigger("addkoi");
    }

    void ChangeChildText()
    {
        foreach (Transform child in animator.transform)
        {
            child.GetComponent<TMP_Text>().text = text.text;
        }
    }
}
