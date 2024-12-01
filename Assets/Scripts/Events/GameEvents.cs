using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents
{
    public event Action onPointsAdded;
    public void OnPointsAdded()
    {
        if(onPointsAdded != null)
        {
            onPointsAdded();
        }
    }

    public event Action koi;
    public void Koi()
    {
        if(koi != null)
        {
            koi();
        }
    }

    public event Action resetSetup;
    public void ResetSetup()
    {
        if(resetSetup != null)
        {
            resetSetup();
        }
    }
    public event Action resetGame;
    public void ResetGame()
    {
        if (resetGame != null)
        {
            resetGame();
        }
    }


    public event Action<Win_States> onWin;
    public void OnWin(Win_States win)
    {
        if (onWin != null)
        {
            onWin(win);
        }
    }

    public event Action<int> onRoundChange;
    public void OnRoundChange(int round)
    {
        if(onRoundChange != null)
        {
            onRoundChange(round);
        }
    }
}
