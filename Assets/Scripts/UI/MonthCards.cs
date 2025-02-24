using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonthCards : MonoBehaviour
{
    private void OnEnable()
    {
        GameEventsManager.instance.gameEvents.resetSetup += ChangeCardTextures;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.gameEvents.resetSetup -= ChangeCardTextures;
    }

    public void ChangeCardTextures()
    {
        StopAllCoroutines();
        StartCoroutine(RotateCards());
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
