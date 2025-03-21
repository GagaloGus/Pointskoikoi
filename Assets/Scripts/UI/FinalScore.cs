using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalScore : MonoBehaviour
{
    List<Vector2> scores;
    public GameObject textPrefab;
    GameObject winP1, winP2, options;
    RainbowText title;
    private void OnEnable()
    {
        title = transform.Find("Titulo").GetComponent<RainbowText>();
        title.ChangeText("Puntuacion final");
        scores = new List<Vector2>(GameManager.instance.scores);
        foreach(Transform t in transform.Find("Punts"))
        {
            Destroy(t.gameObject);
        }

        winP1 = transform.Find("Win").Find("1").gameObject;
        winP2 = transform.Find("Win").Find("2").gameObject;
        options = transform.Find("Opciones").gameObject;

        winP1.SetActive(false);
        winP2.SetActive(false);
        options.SetActive(false);

        StartCoroutine(ShowScores_Corr());
    }

    IEnumerator ShowScores_Corr()
    {
        yield return new WaitForSeconds(1f);
        int index = 1;
        foreach (Vector2 s in scores)
        {
            yield return new WaitForSeconds(0.6f);
            Transform t = Instantiate(textPrefab).transform;
            t.SetParent(transform.Find("Punts"), false);

            //Colores de la puntuacion
            string pt1 = "white";
            string pt2 = "white";
            if(s.x > s.y)
            {
                pt1 = "green"; pt2 = "red";
            }
            else if(s.x < s.y)
            {
                pt1 = "red"; pt2 = "green";
            }

            t.GetComponent<TMP_Text>().text = 
                $"<color=yellow>Ronda {index}</color>    <size=80><color={pt1}>{s.x}</color> - <color={pt2}>{s.y}</color></size>";

            index++;
        }

        yield return new WaitForSeconds(1);
        ShowWinner();
        options.SetActive(true);
    }

    void ShowWinner()
    {
        //Temporal, habra animaciones y cosas asi
        switch (GameManager.instance.Get_WinCondition())
        {
            case Win_States.Player1:
                title.ChangeText("Gana el Jugador 1");
                winP1.SetActive(true);
                winP2.SetActive(false);
                print("Gana jugador 1");
                break;
            case Win_States.Player2:
                title.ChangeText("Gana el Jugador 2");
                winP1.SetActive(false);
                winP2.SetActive(true);
                print("Gana jugador 2");
                break;
            case Win_States.Tie:
                title.ChangeText("Empate");
                winP1.SetActive(true);
                winP2.SetActive(true);
                print("Empate");
                break;
        }
    }
}
