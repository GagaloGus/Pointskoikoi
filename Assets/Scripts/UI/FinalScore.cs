using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalScore : MonoBehaviour
{
    List<Vector2> scores;
    public GameObject textPrefab;
    private void Awake()
    {
        scores = GameManager.instance.scores;
        foreach(Transform t in transform.Find("Punts"))
        {
            Destroy(t.gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(ShowScores_Corr());
    }

    IEnumerator ShowScores_Corr()
    {
        int index = 1;
        foreach (Vector2 s in scores)
        {
            yield return new WaitForSeconds(0.3f);
            Transform t = Instantiate(textPrefab).transform;
            t.SetParent(transform.Find("Punts"), false);

            //Colores de la puntuacion
            string pt1 = "white";
            string pt2 = "white";
            if(s.x > GameManager.instance.originalPts)
            {
                pt1 = "green"; pt2 = "red";
            }
            else if(s.x < GameManager.instance.originalPts)
            {
                pt1 = "red"; pt2 = "green";
            }

            t.GetComponent<TMP_Text>().text = 
                $"<color=yellow>Ronda {index}</color>    <size=80><color={pt1}>{s.x}</color> - <color={pt2}>{s.y}</color></size>";

            index++;
        }

        yield return new WaitForSeconds(0.5f);


    }

    void ShowWinner()
    {
        switch (GameManager.instance.Win)
        {
            case Win_States.Player1:
                transform.Find("2").gameObject.SetActive(false);
                break;
            case Win_States.Player2:
                transform.Find("1").gameObject.SetActive(false);
                break;
        }
    }
}
