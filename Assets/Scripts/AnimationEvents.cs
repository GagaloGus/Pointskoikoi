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
}
