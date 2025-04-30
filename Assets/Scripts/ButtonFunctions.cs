using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
public class ButtonFunctions : MonoBehaviour
{
    /// <summary>
    /// Funcion de boton para terminar la ronda
    /// </summary>
    public void EndRound()
    {
        GameManager.instance.EndRound();
    }

    /// <summary>
    /// Funcion de boton para hacer KOI
    /// </summary>
    public void Koi()
    {
        if(GameManager.instance.gameMode == GameMode.PointThief)
            GameManager.instance.Koi();
    }

    /// <summary>
    /// Funcion de boton para restear el juego totalmente
    /// </summary>
    public void ResetGame()
    {
        GameManager.instance.ResetGame();
    }

    public void GoToSetup()
    {
        GameEventsManager.instance.gameEvents.OnSetupOpen();
    }

    /// <summary>
    /// Funcion de boton de añadir los puntos
    /// </summary>
    public void AddPoints()
    {
        GameManager.instance.AddPoints();
    }

    /// <summary>
    /// Funcion de boton para terminar la ronda
    /// </summary>
    /// <param name="sceneName">nombre de la escena</param>
    public void ChangeScene(string sceneName)
    {
        GameManager.instance.ChangeScene(sceneName);
    }

    /// <summary>
    /// Funcion de boton para salir del juego con un fundido a negro
    /// </summary>
    public void Exit()
    {
        GameEventsManager.instance.visualEvents.
            OnStartFadeCircle(true, () => { Application.Quit(); });
    }
}
