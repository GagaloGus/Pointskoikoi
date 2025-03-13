using System.Collections;
using System.Collections.Generic;
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
        int month = Mathf.Clamp(GameManager.instance.round - 1, 0, 11);

        transform.Find("F").GetComponent<Image>().sprite = GameManager.instance.CardSprites[i + (month * 4)];

    }
}
