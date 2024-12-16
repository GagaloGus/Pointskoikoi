using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
