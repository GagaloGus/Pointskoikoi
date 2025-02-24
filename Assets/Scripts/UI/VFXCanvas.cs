using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VFXCanvas : MonoBehaviour
{
    Image shock;
    GameObject fadeCircle;

    private void OnEnable()
    {
        GameEventsManager.instance.visualEvents.onPointsAddedToDisplay += ShockEffect;
        GameEventsManager.instance.visualEvents.onStartFadeCircle += FadeStart;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.visualEvents.onPointsAddedToDisplay -= ShockEffect;
        GameEventsManager.instance.visualEvents.onStartFadeCircle -= FadeStart;
    }

    // Start is called before the first frame update
    void Start()
    {
        shock = transform.Find("Shock").GetComponent<Image>();
        shock.gameObject.SetActive(false);

        fadeCircle = transform.Find("Fade").gameObject;
        fadeCircle.SetActive(true);
    }

    public void ShockEffect(Color col)
    {
        shock.color = col;
        shock.gameObject.SetActive(true);
    }

    public void FadeStart(bool In, Action f, float delay)
    {
        Animator animator = fadeCircle.GetComponentInChildren<Animator>(true);
        animator.gameObject.SetActive(true);

        animator.SetBool("in", In);

        CoolFunctions.Invoke(this, f, delay);
    }
}
