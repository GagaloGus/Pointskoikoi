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
}
