using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RainbowText : MonoBehaviour
{
    List<string> colors = new List<string>();
    public string og = "";

    void Start()
    {
        Revert();
        Rainbow();
    }

    public void Rainbow()
    {
        colors = new List<string>
        {
            "ff1100", "f58f0a", "ffea00", "09ff00", "00e5ff", "2b00ff", "bb00ff",
        };

        TMP_Text text = GetComponent<TMP_Text>();
        char[] charArray = text.text.ToCharArray();

        string rainbowText = "";
        int colCount = 0;

        for (int i = 0; i < charArray.Length; i++)
        {
            char c = charArray[i];
            string s = colors[colCount];

            if (!string.IsNullOrWhiteSpace(c.ToString())) //Si no es un espacio cuenta
                colCount++;

            colCount = colCount == colors.Count ? 0 : colCount;

            rainbowText += $"<color=#{s}>{c}</color>";
        }

        text.text = rainbowText;
    }

    public void Revert()
    {
        TMP_Text text = GetComponent<TMP_Text>();
        if(og != "")
        {
            text.text = og;
        }
    }
}

#if UNITY_EDITOR_WIN
[CustomEditor(typeof(RainbowText))]
class BotonTrucoParaAñadirOrderANombres : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RainbowText myscript = (RainbowText)target;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Rainbow-ify", GUILayout.Height(30)))
        {
            myscript.Rainbow();
        }
        if (GUILayout.Button("Revert", GUILayout.Height(30)))
        {
            myscript.Revert();
        }

        GUILayout.EndHorizontal();

    }
}
#endif
