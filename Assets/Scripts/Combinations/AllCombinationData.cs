using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Combinations
{
    DiezBasicas, CincoCintas, CincoAnimales,
    Estaciones, CintasAzules, CintasPoeticas,
    InoShikaCho, SakeLuna, SakeCerezo,
    TresLuces, CuatroLuces, CuatroLucesMojadas, CincoLuces
}

public static class AllCombinationData
{
    public static List<CardCombination> allCombinations =
        new List<CardCombination> {
            new CardCombination("10 cartas basicas", new List<int>{1,2,4,5,13,16,17,25,29,45}, "10 cartas basicas y 1pt extra por cada carta adicional", 1, true, 10),
            new CardCombination("5 animales", new List<int>{6,14,18,26,31}, "5 cartas de animal y 1pt extra por cada carta adicional", 1, true, 5),
            new CardCombination("5 cintas", new List<int>{35,7,11,15,19}, "5 cartas de cinta y 1pt extra por cada carta adicional", 1, true, 5),
            new CardCombination("Estaciones", new List<int>{0,1,2,3}, "Las 4 cartas del mes que corresponde a la ronda", 4, false, 0),
            new CardCombination("Cintas Azules", new List<int>{23,35,39}, "Las 3 cintas azules", 6, false, 0),
            new CardCombination("Cintas Poeticas", new List<int>{11,3,7}, "Las 3 cintas poeticas", 6, false, 0),
            new CardCombination("<i>Ino-Shika-Cho</i>", new List<int>{26,38,22}, "El jabalí, el ciervo y la mariposa", 6, false, 0),
            new CardCombination("Sake bajo la luna", new List<int>{34, 28}, "La copa de sake y la luna", 5, false, 0),
            new CardCombination("Sake bajo los cerezos", new List<int>{34, 8}, "La copa de sake y los cerezos", 5, false, 0),
            new CardCombination("Tres Luces", new List<int>{0,28,44}, "3 cartas brillantes (el hombre bajo la luna no cuenta)", 6, false, 0),
            new CardCombination("Cuatro luces", new List<int>{0,28,44,8}, "4 cartas brillantes sin el hombre bajo la luna", 8, false, 0),
            new CardCombination("Cuatro luces mojadas", new List<int>{0,28,44,40}, "4 cartas brillantes incluyendo al hombre bajo la luna", 7, false, 0),
            new CardCombination("Cinco luces", new List<int>{0,28,44,8,40}, "Las 5 cartas brillantes", 10, false, 0)
        };

    public static CardCombination GetData(Combinations combi)
    {
        return allCombinations[(int)combi];
    }
}

[System.Serializable]
public class CardCombination
{
    public List<int> cardSpritesIndex;
    public string title;
    [TextArea(2,1)]public string info;
    public int points;
    public bool canAddExtra;
    public int cardsNeeded;

    public CardCombination(string title, List<int> cardSpritesIndex, string info, int points, bool canAddExtra, int cardsNeeded)
    {
        this.cardSpritesIndex = cardSpritesIndex;
        this.title = title;
        this.info = info;
        this.points = points;
        this.canAddExtra = canAddExtra;
        this.cardsNeeded = cardsNeeded;
    }
}