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
        text.text = $"Ronda {GameManager.instance.round}";
    }
}
