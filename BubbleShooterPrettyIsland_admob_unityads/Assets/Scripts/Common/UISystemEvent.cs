
using System;
using System.Collections.Generic;
using UnityEngine;

public class UISystemEvent
{
    public static Dictionary<UIEvent, UICallBack> s_allUIEvents = new Dictionary<UIEvent, UICallBack>();

    public static Dictionary<string, Dictionary<UIEvent, UICallBack>> s_singleUIEvents = new Dictionary<string, Dictionary<UIEvent, UICallBack>>();

    public static void RegisterAllUIEvent(UIEvent UIEvent, UICallBack l_CallBack)
    {
        if (s_allUIEvents.ContainsKey(UIEvent))
        {
            Dictionary<UIEvent, UICallBack> dictionary;
            UIEvent key;
            (dictionary = s_allUIEvents)[key = UIEvent] = (UICallBack)Delegate.Combine(dictionary[key], l_CallBack);
        }
        else
        {
            s_allUIEvents.Add(UIEvent, l_CallBack);
        }
    }

    public static void RemoveAllUIEvent(UIEvent UIEvent, UICallBack l_CallBack)
    {
        if (s_allUIEvents.ContainsKey(UIEvent))
        {
            Dictionary<UIEvent, UICallBack> dictionary;
            UIEvent key;
            (dictionary = s_allUIEvents)[key = UIEvent] = (UICallBack)Delegate.Remove(dictionary[key], l_CallBack);
        }
        else
        {
            UnityEngine.Debug.LogError("RemoveAllUIEvent don't exits: " + UIEvent);
        }
    }

    public static void RegisterEvent(string l_UIName, UIEvent l_UIEvent, UICallBack l_CallBack)
    {
        if (s_singleUIEvents.ContainsKey(l_UIName))
        {
            if (s_singleUIEvents[l_UIName].ContainsKey(l_UIEvent))
            {
                Dictionary<UIEvent, UICallBack> dictionary;
                UIEvent key;
                (dictionary = s_singleUIEvents[l_UIName])[key = l_UIEvent] = (UICallBack)Delegate.Combine(dictionary[key], l_CallBack);
            }
            else
            {
                s_singleUIEvents[l_UIName].Add(l_UIEvent, l_CallBack);
            }
        }
        else
        {
            s_singleUIEvents.Add(l_UIName, new Dictionary<UIEvent, UICallBack>());
            s_singleUIEvents[l_UIName].Add(l_UIEvent, l_CallBack);
        }
    }

    public static void Dispatch(UIWindowBase l_UI, UIEvent l_UIEvent, params object[] l_objs)
    {
        if (l_UI == null)
        {
            UnityEngine.Debug.LogError("Dispatch l_UI is null!");
            return;
        }
        if (s_allUIEvents.ContainsKey(l_UIEvent))
        {
            try
            {
                if (s_allUIEvents[l_UIEvent] != null)
                {
                    s_allUIEvents[l_UIEvent](l_UI, l_objs);
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("UISystemEvent Dispatch error:" + ex.ToString());
            }
        }
        if (s_singleUIEvents.ContainsKey(l_UI.name) && s_singleUIEvents[l_UI.name].ContainsKey(l_UIEvent))
        {
            try
            {
                if (s_singleUIEvents[l_UI.name][l_UIEvent] != null)
                {
                    s_singleUIEvents[l_UI.name][l_UIEvent](l_UI, l_objs);
                }
            }
            catch (Exception ex2)
            {
                UnityEngine.Debug.LogError("UISystemEvent Dispatch error:" + ex2.ToString());
            }
        }
    }
}
