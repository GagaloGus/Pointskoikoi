using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundText : MonoBehaviour
{
    TMP_Text text;
    Animator animator;
    private void OnEnable()
    {
        GameEventsManager.instance.gameEvents.onRoundChange += ChangeRound;
        GameEventsManager.instance.gameEvents.resetGame += ResetGame;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.gameEvents.onRoundChange -= ChangeRound;
        GameEventsManager.instance.gameEvents.resetGame -= ResetGame;
    }

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        animator = GetComponent<Animator>();
    }

    private void ChangeRound(int round)
    {
        animator.SetTrigger("change");
    }
    private void ResetGame()
    {
        animator.SetTrigger("change");
    }

    //Evento de animacion
    public void ChangeRound_Animation()
    {
        string month = GameManager.MONTHS[Mathf.Clamp(GameManager.instance.round - 1, 0, 11)];
        text.text = $"Ronda {GameManager.instance.round}\n<size=55>{month}";
    }
}
