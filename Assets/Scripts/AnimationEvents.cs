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

    public void ChangeMonthCard()
    {   
        int i = transform.GetSiblingIndex();
        int month = Mathf.Clamp(GameManager.instance.round - 1, 0, 11);

        List<Sprite> CardSprites = GameManager.instance.CardSprites;

        transform.Find("F").GetComponent<Image>().sprite = CardSprites[i + (month * 4)];

    }
}
