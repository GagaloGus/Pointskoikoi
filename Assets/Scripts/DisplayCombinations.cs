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

    List<CardCombination> cardCombinations = new List<CardCombination>();

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

        List<CardCombination> cardCombinations = new List<CardCombination>
        {
            new CardCombination("10 cartas basicas", new List<int>{1,2,4,5,13,16,17,25,29,45}, "10 cartas basicas y 1 pt extra por cada carta adicional", 1),
            new CardCombination("5 animales", new List<int>{6,14,18,26,31}, "5 cartas de animales y 1 pt extra por cada carta adicional", 1),
            new CardCombination("5 cintas", new List<int>{3,7,11,15,19}, "5 cartas de cinta y 1 pt extra por cada carta adicional", 1),
            new CardCombination("Estaciones", new List<int>{0,1,2,3}, "Las 4 cartas del mes que corresponde a la ronda", 4),
            new CardCombination("Cintas Azules", new List<int>{23,35,39}, "Las 3 cintas azules", 6),
            new CardCombination("Cintas Poeticas", new List<int>{11,3,7}, "Las 3 cintas poeticas", 6),
            new CardCombination("<i>Ino-Shika-Cho</i>", new List<int>{26,38,22}, "El jabalí, el ciervo y la mariposa", 6),
            new CardCombination("Sake bajo la luna", new List<int>{34, 28}, "La copa de sake y la luna", 5),
            new CardCombination("Sake bajo los cerezos", new List<int>{34, 8}, "La copa de sake y los cerezos", 5),
            new CardCombination("Tres Luces", new List<int>{0,28,44}, "3 cartas brillantes (el hombre bajo la luna no cuenta)", 6),
            new CardCombination("Cuatro luces mojadas", new List<int>{0,28,44,40}, "4 cartas brillantes incluyendo al hombre bajo la luna", 7),
            new CardCombination("Cuatro luces", new List<int>{0,28,44,8}, "4 cartas brillantes sin el hombre bajo la luna", 8),
            new CardCombination("Cinco luces", new List<int>{0,28,44,8,40}, "Las 5 cartas brillantes", 10)
        };

        foreach (CardCombination c in cardCombinations)
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

[System.Serializable]
public class CardCombination
{
    public List<int> cardSpritesIndex;
    public string title, info;
    public int points;

    public CardCombination(string title, List<int> cardSpritesIndex, string info, int points)
    {
        this.cardSpritesIndex = cardSpritesIndex;
        this.title = title;
        this.info = info;
        this.points = points;
    }
}
