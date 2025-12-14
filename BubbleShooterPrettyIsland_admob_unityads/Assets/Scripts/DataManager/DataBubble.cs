
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataBubble : Singleton<DataBubble>
{
    public const string loadPath = "Data/GameData/";

    public Dictionary<BubbleKey, Dictionary<BubbleType, string>> dDataObj;

    public override void Init()
    {
        if (dDataObj == null)
        {
            InitDate();
        }
    }

    public Dictionary<BubbleType, string> GetContentByKey(BubbleKey _key)
    {
        return dDataObj[_key];
    }

    public string GetContentByKeyAndType(BubbleKey _key, BubbleType _type)
    {
        return dDataObj[_key][_type];
    }

    public Dictionary<BubbleType, string> GetContentByKey(string _key)
    {
        return dDataObj[(BubbleKey)Enum.Parse(typeof(BubbleKey), _key)];
    }

    public string GetContentByKeyAndType(string _key, BubbleType _type)
    {
        return dDataObj[(BubbleKey)Enum.Parse(typeof(BubbleKey), _key)][_type];
    }

    public string[] GetContentArr(BubbleKey _key, BubbleType _type, char str)
    {
        return dDataObj[_key][_type].Split(str);
    }

    private void InitDate()
    {
        if (dDataObj == null)
        {
            dDataObj = new Dictionary<BubbleKey, Dictionary<BubbleType, string>>();
            string empty = string.Empty;

            TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/Bubble", typeof(TextAsset));
            empty = textAsset.ToString();

            string[] array = empty.Split('\n');
            Dictionary<BubbleType, string> dictionary = null;
            IEnumerator enumerator = Enum.GetValues(typeof(BubbleKey)).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    BubbleKey bubbleKey = (BubbleKey)enumerator.Current;
                    string text = array[(int)(bubbleKey + 2)];
                    int num = text.Split(',').Length;
                    dictionary = new Dictionary<BubbleType, string>();
                    IEnumerator enumerator2 = Enum.GetValues(typeof(BubbleType)).GetEnumerator();
                    try
                    {
                        while (enumerator2.MoveNext())
                        {
                            BubbleType bubbleType = (BubbleType)enumerator2.Current;
                            dictionary.Add(bubbleType, text.Split(',')[(int)bubbleType]);
                        }
                    }
                    finally
                    {
                        IDisposable disposable;
                        if ((disposable = (enumerator2 as IDisposable)) != null)
                        {
                            disposable.Dispose();
                        }
                    }
                    dDataObj.Add(bubbleKey, dictionary);
                }
            }
            finally
            {
                IDisposable disposable2;
                if ((disposable2 = (enumerator as IDisposable)) != null)
                {
                    disposable2.Dispose();
                }
            }
        }
    }
}
