
using System;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static int GetNowTime_FF()
    {
        return int.Parse(DateTime.Now.ToString("mmssfff").ToString());
    }

    public static int GetNowTime()
    {
        return int.Parse(GetTimeStamp());
    }

    public static string GetTimeStamp()
    {
        return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
    }

    public static Sprite GetResourcesSprite(string srcPath, int iSize1, int iSize2)
    {
        Texture2D texture = (Texture2D)Resources.Load(srcPath, typeof(Texture2D));
        return Sprite.Create(texture, new Rect(0f, 0f, iSize1, iSize2), new Vector2(0.5f, 0.5f));
    }

    public static Sprite GetResourcesSprite(string srcPath)
    {
        Texture2D texture2D = (Texture2D)Resources.Load(srcPath, typeof(Texture2D));
        return Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
    }

    public static string GetNowTime_Day()
    {
        return DateTime.Now.ToString("yyyyMMdd");
    }

    public static string GetNowTime_Hours()
    {
        return DateTime.Now.ToString("HH_mm_ss");
    }

    public static string GetNowTime2()
    {
        return DateTime.Now.ToString("yyyyMMddHHmm");
    }

    public static string ReplaceText(string str)
    {
        if (str.Split('_').Length > 1)
        {
            return str.Replace("_", " ");
        }
        return str;
    }

    public static string ReplaceTextLine(string str)
    {
        return str.Replace("\\n", "\n");
    }
}
