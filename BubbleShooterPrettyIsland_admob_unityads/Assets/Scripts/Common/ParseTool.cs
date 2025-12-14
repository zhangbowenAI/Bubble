
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class ParseTool
{
    private static string[] c_NullStringArray = new string[0];

    public static float[] String2FloatArray(string value)
    {
        string[] array = String2StringArray(value);
        float[] array2 = new float[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            float num = array2[i] = float.Parse(array[i]);
        }
        return array2;
    }

    public static bool[] String2BoolArray(string value)
    {
        string[] array = String2StringArray(value);
        bool[] array2 = new bool[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            bool flag = array2[i] = bool.Parse(array[i]);
        }
        return array2;
    }

    public static Vector2 String2Vector2(string value)
    {
        try
        {
            string[] array = value.Split(',');
            float x = float.Parse(array[0]);
            float y = float.Parse(array[1]);
            return new Vector2(x, y);
        }
        catch (Exception ex)
        {
            throw new Exception("ParseVector2: Don't convert value to Vector2 value:" + value + "\n" + ex.ToString());
        }
    }

    public static Vector2[] String2Vector2Array(string value)
    {
        string[] array = String2StringArray(value);
        Vector2[] array2 = new Vector2[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            string[] array3 = array[i].Split(',');
            float x = float.Parse(array3[0]);
            float y = float.Parse(array3[1]);
            array2[i] = new Vector2(x, y);
        }
        return array2;
    }

    public static Vector3 String2Vector3(string value)
    {
        try
        {
            string[] array = value.Split(',');
            float x = float.Parse(array[0]);
            float y = float.Parse(array[1]);
            float z = float.Parse(array[2]);
            return new Vector3(x, y, z);
        }
        catch (Exception ex)
        {
            throw new Exception("ParseVector3: Don't convert value to Vector3 value:" + value + "\n" + ex.ToString());
        }
    }

    public static Vector3[] String2Vector3Array(string value)
    {
        string[] array = String2StringArray(value);
        Vector3[] array2 = new Vector3[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            string[] array3 = array[i].Split(',');
            float x = float.Parse(array3[0]);
            float y = float.Parse(array3[1]);
            float z = float.Parse(array3[2]);
            array2[i] = new Vector3(x, y, z);
        }
        return array2;
    }

    public static Color String2Color(string value)
    {
        try
        {
            string[] array = value.Split(',');
            float r = float.Parse(array[0]);
            float g = float.Parse(array[1]);
            float b = float.Parse(array[2]);
            float a = 1f;
            if (array.Length > 3)
            {
                a = float.Parse(array[3]);
            }
            return new Color(r, g, b, a);
        }
        catch (Exception ex)
        {
            throw new Exception("ParseColor: Don't convert value to Color value:" + value + "\n" + ex.ToString());
        }
    }

    public static string[] String2StringArray(string value)
    {
        if (value != null && value != string.Empty && value != "null" && value != "Null" && value != "NULL" && value != "None")
        {
            return value.Split('|');
        }
        return c_NullStringArray;
    }

    public static int[] String2IntArray(string value)
    {

        if (!string.IsNullOrEmpty(value))
        {
            string[] array2 = value.Split('|');
            return Array.ConvertAll(array2, int.Parse);
        }
        return new int[0];
    }
}
