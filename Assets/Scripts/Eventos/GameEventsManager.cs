using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance;

    public GameEvents gameEvents;

    private void Awake()
    {
        if (!instance) //instance  != null  //Detecta que no haya otro manager en la escena.
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); //Si hay otro manager lo destruye.
        }
        instance = this;

        gameEvents = new GameEvents();
    }
}
