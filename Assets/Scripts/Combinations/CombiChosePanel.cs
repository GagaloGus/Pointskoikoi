using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CombiChosePanel : MonoBehaviour
{
    TMP_Text infoText, pointText;
    [SerializeField] List<CombiPointsPair> combinationsChosen = new();

    private void OnEnable()
    {
        combinationsChosen.Clear();
        infoText.text = "";
        pointText.text = "0pts";
    }

    private void Awake()
    {
        infoText = transform.Find("pointstring").GetComponent<TMP_Text>();
        pointText = transform.Find("pointall").GetComponent<TMP_Text>();
    }

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

        pointText.text = $"{AllPoints()} pts";
    }

    public int AllPoints()
    {
        int pts = 0;

        foreach (CombiPointsPair combi in combinationsChosen)
        {
            pts += combi.combi.points + combi.extraCards;
        }

        return pts;
    }

    
}

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
