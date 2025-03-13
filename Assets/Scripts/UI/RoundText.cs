using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundText : MonoBehaviour
{
    GameObject child;
    TMP_Text text;
    Animator animator;
    private void OnEnable()
    {
        GameEventsManager.instance.gameEvents.onRoundChange += ChangeRound;
        GameEventsManager.instance.gameEvents.resetGame += ResetGame;
        GameEventsManager.instance.gameEvents.onStartGame += StartGame;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.gameEvents.onRoundChange -= ChangeRound;
        GameEventsManager.instance.gameEvents.resetGame -= ResetGame;
        GameEventsManager.instance.gameEvents.onStartGame -= StartGame;
    }

    private void Awake()
    {
        child = transform.GetChild(0).gameObject;
        text = child.GetComponent<TMP_Text>();
        animator = child.GetComponent<Animator>();

        child.SetActive(false);
    }

    private void StartGame()
    {
        child.SetActive(true);
        animator.SetTrigger("enter");
    }

    private void ChangeRound(int round)
    {
        animator.SetTrigger("change");
    }
    private void ResetGame()
    {
        animator.SetTrigger("change");
    }
}
