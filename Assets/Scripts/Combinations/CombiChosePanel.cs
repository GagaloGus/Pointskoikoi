using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CombiChosePanel : MonoBehaviour
{
    TMP_Text infoText, pointText;
    [SerializeField] List<CombiPointsPair> combinationsChosen = new();
    public int finalPoints;
    Transform Content;

    PointHandler pointHandler;

    private void OnEnable()
    {
        combinationsChosen.Clear();
        infoText.text = "";
        pointText.text = "0 pts";
        finalPoints = 0;
    }

    private void Awake()
    {
        infoText = transform.Find("pointstring").GetComponent<TMP_Text>();
        pointText = transform.Find("pointall").GetComponent<TMP_Text>();
        Content = GetComponentInChildren<ScrollRect>().transform.Find("Viewport").Find("Content");
        pointHandler = FindObjectOfType<PointHandler>();
    }

    /// <summary>
    /// Activa las combinaciones pasadas en la lista al activar el panel de puntos
    /// </summary>
    /// <param name="pairs">Las combinaciones guardadas</param>
    public void ActivateChoices(List<CombiPointsPair> pairs)
    {
        foreach(Transform t in Content)
        {
            Combination c = t.GetComponent<Combination>();
            Toggle tog = t.GetComponentInChildren<Toggle>();

            c.CloseInfoButton();
            c.SetExtraPoints(0);

            CombiPointsPair card = Array.Find(
                pairs.ToArray(), 
                x => x.combi == AllCombinationData.GetData(c.CombinationType));

            if (card != null)
            {
                tog.isOn = true;
                SelectCombination(card.combi, card.extraCards, true);
                c.SetExtraPoints(card.extraCards);
            }
            else
            {
                tog.isOn = false;
            }
        }
    }

    /// <summary>
    /// Añade o quita una combinacion de la lista
    /// </summary>
    /// <param name="combi"></param>
    /// <param name="extraCards"></param>
    /// <param name="add"></param>
    public void SelectCombination(CardCombination combi, int extraCards, bool add)
    {       
        if (add)
        {
            combinationsChosen.Add(new CombiPointsPair(combi, extraCards));
        }
        else
        {
            CombiPointsPair pair = Array.Find(combinationsChosen.ToArray(), x => x.combi == combi);
            combinationsChosen.Remove(pair);
        }

        WriteCombinations();
    }

    /// <summary>
    /// Actualiza la combinacion al añadir cartas extras
    /// </summary>
    /// <param name="combi">La combinacion en question</param>
    /// <param name="extraCards">Cuantas cartas se añaden</param>
    public void UpdateCombination(CardCombination combi, int extraCards)
    {
        foreach (CombiPointsPair pair in combinationsChosen)
        {
            if (pair.combi == combi)
            {
                pair.extraCards = extraCards;
                break;
            }
        }

        WriteCombinations();
    }

    /// <summary>
    /// Escribe las combinaciones y puntos en la parte inferior del panel
    /// </summary>
    void WriteCombinations()
    {
        infoText.text = "";

        for (int i = 0; i < combinationsChosen.Count; i++)
        {
            CombiPointsPair pair = combinationsChosen[i];

            string title = CoolFunctions.RemoveNumberFromString(pair.combi.title);

            infoText.text +=
                $"{(pair.combi.cardsNeeded == 0 ? "" : pair.combi.cardsNeeded+pair.extraCards + " ")}{title} {(i == combinationsChosen.Count - 1 ? "" : "+ ")}";
        }

        finalPoints = AllPoints();
        pointText.text = $"{finalPoints} pts";
    }

    /// <summary>
    /// Calcula todos los puntos de la lista de combinaciones
    /// </summary>
    /// <returns>Los puntos totales</returns>
    public int AllPoints()
    {
        int pts = 0;

        foreach (CombiPointsPair combi in combinationsChosen)
        {
            pts += combi.combi.points + combi.extraCards;
        }

        return pts;
    }

    /// <summary>
    /// -funcion de boton-
    /// Acepta las combinaciones y las manda para que se mantengan en pantalla
    /// </summary>
    public void AcceptCombinations()
    {
        print($"{finalPoints} pts totales");
        pointHandler.HoldPoints(combinationsChosen, finalPoints);
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// -funcion de boton-
    /// Sale sin hacer nada
    /// </summary>
    public void Leave()
    {
        pointHandler.CancelHoldPoints();
        gameObject.SetActive(false);
    }
}

/// <summary>
/// Clase de combinacion + cartas extra que tiene
/// </summary>
[System.Serializable]
public class CombiPointsPair
{
    public CardCombination combi;
    public int extraCards;

    public CombiPointsPair(CardCombination combi, int extraCards)
    {
        this.combi = combi;
        this.extraCards = extraCards;
    }
}
