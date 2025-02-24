using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEvents
{
    public event Action<Color> onPointsAddedToDisplay;
    public void OnPointsAddedToDisplay(Color col)
    {
        if(onPointsAddedToDisplay != null)
        {
            onPointsAddedToDisplay(col);
        }
    }

    public event Action<bool, Action, float> onStartFadeCircle;
    public void OnStartFadeCircle(bool In, Action f = null, float delay = 1.75f)
    {
        if (onStartFadeCircle != null)
        {
            onStartFadeCircle(In, (f == null ? () => { } : f), delay);
        }
    }
}
