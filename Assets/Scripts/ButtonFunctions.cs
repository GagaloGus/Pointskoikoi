using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
public class ButtonFunctions : MonoBehaviour
{
    public void NextRound()
    {
        GameManager.instance.NextRound();
    }

    public void Koi()
    {
        GameManager.instance.Koi();
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
        Application.Quit();
    }
}
