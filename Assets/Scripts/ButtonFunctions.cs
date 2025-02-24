using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
public class ButtonFunctions : MonoBehaviour
{
    public void EndRound()
    {
        GameManager.instance.EndRound();
    }

    public void Koi()
    {
        GameManager.instance.Koi();
    }

    public void ResetGame()
    {
        GameManager.instance.ResetGame();
    }

    public void AddPoints()
    {
        GameManager.instance.AddPoints();
    }

    public void ChangeScene(string sceneName)
    {
        GameManager.instance.ChangeScene(sceneName);
    }

    public void Exit()
    {
        GameEventsManager.instance.visualEvents.
            OnStartFadeCircle(true, () => { Application.Quit(); });
    }
}
