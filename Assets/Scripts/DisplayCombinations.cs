using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCombinations : MonoBehaviour
{
    [Header("References")]
    public Transform Display;
    public List<Sprite> CardSprites;
    public GameObject DisplayCPrefab, DisplayMPrefab, CardPrefab;

    private void Start()
    {
        Display.gameObject.SetActive(false);
        Display.Find("Combi").gameObject.SetActive(true);
        Display.Find("Meses").gameObject.SetActive(false);

        Months();
        Combinations();
    }
    void Months()
    {
        Transform Content = Display.transform.Find("Meses").Find("Content");

        foreach (Transform transform in Content)
        {
            Destroy(transform.gameObject);
        }

        for (int i = 0; i < 12; i++)
        {
            Transform m = Instantiate(DisplayMPrefab).transform;

            m.SetParent(Content, false);
            m.Find("Mes").GetComponent<TMP_Text>().text = GameManager.MONTHS[i];

            for (int j = 0; j < 4; j++)
            {
                m.Find($"Card-{j}").GetComponent<Image>().sprite = CardSprites[(i*4)+j];
            }
        }
    }

    void Combinations()
    {
        Transform Content = Display.Find("Combi").Find("Scroll View").Find("Viewport").Find("Content");

        foreach (Transform transform in Content)
        {
            Destroy(transform.gameObject);
        }

        foreach (CardCombination c in AllCombinationData.allCombinations)
        {
            Transform disp = Instantiate(DisplayCPrefab).transform;
            disp.SetParent(Content, false);

            disp.Find("Titulo").GetComponent<TMP_Text>().text = c.title;
            disp.Find("Info").GetComponent<TMP_Text>().text = $"<size=60>{c.info}</size>\r\n<color=yellow>{c.points} pt.</color>";

            Transform cardDisp = disp.Find("Cards");

            foreach (int i in c.cardSpritesIndex)
            {
                int index = i;

                Transform card = Instantiate(CardPrefab).transform;
                card.SetParent(cardDisp, false);

                if (index >= CardSprites.Count) { index = 0; }
                card.GetComponent<Image>().sprite = CardSprites[index];
            }
        }
    }
}


