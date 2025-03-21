using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonthCards : MonoBehaviour
{
    Animator animator;
    private void OnEnable()
    {
        GameEventsManager.instance.gameEvents.resetSetup += ChangeCardTextures;
        GameEventsManager.instance.gameEvents.onStartGame += StartGame;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.gameEvents.resetSetup -= ChangeCardTextures;
        GameEventsManager.instance.gameEvents.onStartGame -= StartGame;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeCardTextures()
    {
        StopAllCoroutines();
        StartCoroutine(RotateCards());
    }

    public void StartGame()
    {
        StopAllCoroutines();
        StartCoroutine(RevealCards());
    }

    IEnumerator RevealCards()
    {
        for (int i = 0; i <transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            t.Find("F").GetComponent<Image>().sprite = 
                GameManager.instance.CardSprites[i + (GameManager.instance.get_MonthNumber() * 4)];
        }

        yield return new WaitForSeconds(2f);
        animator.SetFloat("speed", 1);
    }

    IEnumerator RotateCards()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i <= 3; i++)
        {
            Transform t = transform.Find($"Card-{i}");

            t.GetComponent<Animator>().SetTrigger("rotate");

            yield return new WaitForSeconds(0.1f);
        }    
    }
}
