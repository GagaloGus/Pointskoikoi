using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class CoolFunctions
{
    #region Math
    public static float MapValues(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }

    public static Vector3 FlattenVector3(Vector3 value, float newYValue = 0)
    {
        value.y = newYValue;
        return value;
    }

    public static Vector3 MoveAlongAxis(Transform axis, Vector3 margin)
    {
        return axis.right * margin.x + axis.up * margin.y + axis.forward * margin.z;
    }
    #endregion

    public static string StringContentOfList<T>(List<T> list, bool saltoDeLinea)
    {
        string content = "";
        foreach (T item in list)
        {
            content += item.ToString();

            if (saltoDeLinea)
            {
                content += "\n";
            }
            else
            {
                content += ", ";
            }
        }

        return content;
    }

    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static System.Collections.IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }

    public static List<T> ShuffleList<T>(List<T> lista)
    {
        int n = lista.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T valor = lista[k];
            lista[k] = lista[n];
            lista[n] = valor;
        }

        return lista;
    }

    public static string RemoveNumberFromString(string str)
    {
        return Regex.Replace(str, @"^\d+\s*", "");
    }
}