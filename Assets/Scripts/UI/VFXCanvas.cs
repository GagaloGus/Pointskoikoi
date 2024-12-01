using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VFXCanvas : MonoBehaviour
{
    Image shock;

    private void OnEnable()
    {
        GameEventsManager.instance.visualEvents.onPointsAddedToDisplay += ShockEffect;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.visualEvents.onPointsAddedToDisplay -= ShockEffect;
    }

    // Start is called before the first frame update
    void Start()
    {
        shock = transform.Find("Shock").GetComponent<Image>();
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
    }

    public void ShockEffect(Color col)
    {
        shock.color = col;
        shock.gameObject.SetActive(true);
    }
}
