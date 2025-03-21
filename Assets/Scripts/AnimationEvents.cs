using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationEvents : MonoBehaviour
{
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }

    public void AddedPoints()
    {
        FindObjectOfType<PointHandler>().UpdateUIPoints();
        gameObject.SetActive(false);
    }

    //Llamado por el texto de roundend cuando acaba su animacion
    public void ContinueRound()
    {
        GameManager.instance.NextRound();
    }

    //Solo sirve para las cartas del mes
    public void ChangeMonthCard()
    {   
        int i = transform.GetSiblingIndex();

        transform.Find("F").GetComponent<Image>().sprite = 
            GameManager.instance.CardSprites[i + (GameManager.instance.get_MonthNumber() * 4)];
    }

    //Evento de animacion de roundcounttext
    public void ChangeRound_Animation()
    {
        string month = GameManager.MONTHS[GameManager.instance.get_MonthNumber()];
        GetComponent<TMP_Text>().text = $"Ronda {GameManager.instance.round}\n<size=55>{month}";
    }

    //Evento de animacion del koi text
    //Cuanto termina, manda un mensaje al pointhandler
    public void SendEndKoiEndMessage()
    {
        FindObjectOfType<PointHandler>().KoiText_IncreasePts();
    }
}
