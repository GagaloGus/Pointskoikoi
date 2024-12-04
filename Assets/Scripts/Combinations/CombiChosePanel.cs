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
    Button Accept;
    Transform Content;

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
        Accept = transform.Find("Accept").GetComponent<Button>();
        Content = GetComponentInChildren<ScrollRect>().transform.Find("Viewport").Find("Content");
    }

    public void ActivateChoices(List<CombiPointsPair> pairs)
    {
        foreach(Transform t in Content)
        {
            Combination c = t.GetComponent<Combination>();
            Toggle tog = t.GetComponentInChildren<Toggle>();

            c.CloseInfoButton();
            c.SetExtraPoints(0);

            CombiPointsPair card = Array.Find(pairs.ToArray(), x => x.combi == AllCombinationData.GetData(c.CombinationType));

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

        finalPoints = AllPoints();
        pointText.text = $"{finalPoints} pts";
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

    public void AcceptCombinations()
    {
        print($"{finalPoints} pts totales");
        FindObjectOfType<PointHandler>().HoldPoints(combinationsChosen, finalPoints);
        gameObject.SetActive(false);
    }
    
    public void Leave()
    {
        FindObjectOfType<PointHandler>().CancelHoldPoints();
        gameObject.SetActive(false);
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
