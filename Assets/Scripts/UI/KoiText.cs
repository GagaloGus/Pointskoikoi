using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

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
        koiColors = GameManager.instance.koiColors; ;
    }

    void ResetGame()
    {

    }

    void ResetSetup()
    {
        text.color = Color.white;
        text.text = $"<size=80>koi</size>\nx1";
        animator.SetTrigger("reset");
        SetMaterialGlowValues(0);
        print("res");
    }

    void Koi()
    {
        int index = GameManager.instance.koi % koiColors.Length;
        text.color = koiColors[index];
        text.text = $"<size=80>koi</size>\nx{GameManager.instance.koi}";
        textGlow.text = text.text;
    }

    public void Tingle()
    {
        animator.SetTrigger("addkoi");
        //StartCoroutine(Brillo_Corr());
    }

    IEnumerator Brillo_Corr()
    {
        float val = 0.1f;
        while(val < 0.5)
        {
            val += 0.04f;
            SetMaterialGlowValues(val);
            yield return new WaitForSeconds(0.01f);
        }

        while (val > 0)
        {
            val -= 0.02f;
            SetMaterialGlowValues(val);
            yield return new WaitForSeconds(0.01f);
        }
    }

    void SetMaterialGlowValues(float val)
    {
        text.fontSharedMaterial.SetFloat("_Softness", val/1.5f);
        text.fontSharedMaterial.SetFloat("_FaceDilate", val);
    }

    public void SendEndTingleMessage()
    {
        FindObjectOfType<PointHandler>().KoiText_IncreasePts();
    }
}
