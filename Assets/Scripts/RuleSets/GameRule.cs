using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameRule : MonoBehaviour
{
    public RuleSet_Option rs;
    public int startValue;
    public Vector2Int limits;

    TMP_Text points;
    Animator textAnimator;

    private void OnEnable()
    {
        points = transform.Find("Points").Find("count").GetChild(0).GetComponent<TMP_Text>();
        textAnimator = points.GetComponent<Animator>();

        points.text = startValue.ToString();
    }

    /// <summary>
    /// Funcion de boton para sumar o bajar el numero
    /// </summary>
    public void ChangeNumber(bool add)
    {
        int pts = int.Parse(points.text),
            ptsNew = pts + (add ? 1 : -1);

        if (ptsNew < limits.x)
        {
            pts = limits.x;
            textAnimator.SetTrigger("deny");
        }
        else if (ptsNew > limits.y)
        {
            pts = limits.y;
            textAnimator.SetTrigger("deny");
        }
        else
            pts = ptsNew;

        points.text = pts.ToString();
    }

    /// <summary>
    /// Establece los valores en el GameManager
    /// </summary>
    public void AcceptValues()
    {
        switch (rs)
        {
            case RuleSet_Option.MaxRounds:
                GameManager.instance.maxRounds = int.Parse(points.text);
                break;
            case RuleSet_Option.StartPoints:
                GameManager.instance.originalPts = int.Parse(points.text);
                break;
            case RuleSet_Option.KoiPointsNeeded:
                GameManager.instance.koiPointsForDouble = int.Parse(points.text);
                break;
        }
    }
}

