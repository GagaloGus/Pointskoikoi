using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RainbowText : MonoBehaviour
{
    List<string> colors = new List<string>();

    private void Awake()
    {
        colors = new List<string>
        {
            "ff1100", "f58f0a", "ffea00", "09ff00", "00e5ff", "2b00ff", "bb00ff", 
        };
    }

    void Start()
    {
        TMP_Text text = GetComponent<TMP_Text>();
        char[] charArray = text.text.ToCharArray();

        string rainbowText = "";
        int colCount = 0;

        for (int i = 0; i < charArray.Length; i++)
        {
            char c = charArray[i];
            string s = colors[colCount];

            if(!string.IsNullOrWhiteSpace(c.ToString())) //Si no es un espacio cuenta
            colCount++;

            colCount = colCount == colors.Count ? 0 : colCount;

            rainbowText += $"<color=#{s}>{c}</color>";
        }

        text.text = rainbowText;
    }
}
