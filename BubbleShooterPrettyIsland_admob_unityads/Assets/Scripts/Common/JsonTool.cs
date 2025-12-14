
using FrameWork;
using System.Collections.Generic;
using UnityEngine;

public class JsonTool
{
    public static T Json2Object<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }

    public static string Object2Json(object obj)
    {
        if (obj is List<object>)
        {
            return List2Json(obj as List<object>);
        }
        if (obj is Dictionary<string, object>)
        {
            return Dictionary2Json(obj as Dictionary<string, object>);
        }
        return JsonUtility.ToJson(obj);
    }

    public static List<T> Json2List<T>(string jsonData)
    {
        List<T> list = new List<T>();
        if (!string.IsNullOrEmpty(jsonData))
        {
            List<object> list2 = Json.Deserialize(jsonData) as List<object>;
            if (list2 == null)
            {
                return list;
            }
            for (int i = 0; i < list2.Count; i++)
            {
                list.Add(Json2Object<T>(list2[i].ToString()));
            }
        }
        return list;
    }

    public static string List2Json<T>(List<T> datas)
    {
        List<object> list = new List<object>();
        for (int i = 0; i < datas.Count; i++)
        {
            list.Add(Object2Json(datas[i]));
        }
        return Json.Serialize(list);
    }

    public static Dictionary<string, T> Json2Dictionary<T>(string jsonData)
    {
        Dictionary<string, T> dictionary = new Dictionary<string, T>();
        if (!string.IsNullOrEmpty(jsonData))
        {
            Dictionary<string, object> dictionary2 = Json.Deserialize(jsonData) as Dictionary<string, object>;
            if (dictionary2 == null)
            {
                return dictionary;
            }
            {
                foreach (string key in dictionary2.Keys)
                {
                    dictionary.Add(key, Json2Object<T>(dictionary2[key].ToString()));
                }
                return dictionary;
            }
        }
        return dictionary;
    }

    public static string Dictionary2Json<T>(Dictionary<string, T> datas)
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        foreach (string key in datas.Keys)
        {
            dictionary.Add(key, Object2Json(datas[key]));
        }
        return Json.Serialize(dictionary);
    }
}
