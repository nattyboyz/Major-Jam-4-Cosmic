using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    private static System.Random rng = new System.Random();
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static float GetSortingLayerModifier(string layerName)
    {
        float modifier = 0;

        if (string.Compare(layerName, "Ground1") == 0)
        {
            modifier = 4;
        }
        else if(string.Compare(layerName, "Ground2") == 0)
        {
            modifier = 3;
        }
        else if (string.Compare(layerName, "Ground3") == 0)
        {
            modifier = 2;
        }
        else if (string.Compare(layerName, "Object1") == 0)
        {
            modifier = 1;
        }
        else if (string.Compare(layerName, "Object2") == 0)
        {
            modifier = 0;
        }
        else if (string.Compare(layerName, "Object3") == 0)
        {
            modifier = -1;
        }
        return modifier;
    }

    public static int SortingLayerToEditorNumber(string layerName)
    {
        if (string.Compare(layerName, "Object1") == 0)
        {
            return 1;
        }
        else if (string.Compare(layerName, "Object2") == 0)
        {
            return  2;
        }
        else if (string.Compare(layerName, "Object3") == 0)
        {
            return  3;
        }
        else if (string.Compare(layerName, "Object4") == 0)
        {
            return 4;
        }
        else if (string.Compare(layerName, "Object5") == 0)
        {
            return 5;
        }
        else if (string.Compare(layerName, "Object6") == 0)
        {
            return 6;
        }
        return -1;
    }

    public static string EditorNumberToSortingLayer(int order)
    {
        if(order == 1)
        {
            return "Object1";
        }
        else if (order == 2)
        {
            return "Object2";
        }
        else if(order == 3)
        {
            return "Object3";
        }
        else if (order == 4)
        {
            return "Object4";
        }
        else if (order == 5)
        {
            return "Object5";
        }
        else if (order == 6)
        {
            return "Object6";
        }

        return "default";

    }

    public static void SetZBaseOnY(Transform transform, string layerName, int layerOrder = 0)
    {
        float modifier = Util.GetSortingLayerModifier(layerName);
        transform.position = new Vector3(
                     transform.position.x,
                     transform.position.y,
                     (transform.position.y * 0.001f) + modifier - (0.05f*layerOrder));
    }

    public static bool AbovePecentage(int check,int full, float percent)
    {
        //Debug.Log(objectiveLeft.Count + " " + (objectives.Count - (int)objectives.Count * 0.8f));
        if (check >= (int)(full * percent)) return true;
        return false;
    }

    public static int Percent(int number, float percent)
    {
        //Debug.Log(objectiveLeft.Count + " " + (objectives.Count - (int)objectives.Count * 0.8f));
        return (int)(number * percent);
    }
}


public static class Vector
{
    public static Vector3 zero = new Vector3(0, 0, 0);
    public static Vector3 one = new Vector3(1, 1, 1);
}
