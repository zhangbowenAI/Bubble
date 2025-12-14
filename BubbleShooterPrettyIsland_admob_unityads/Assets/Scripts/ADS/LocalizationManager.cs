using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LanguageType
{
    CN,
    EN,
}

public class LocalizationManager : MonoSingletonBase<LocalizationManager>
{

    public LanguageType curLanguage = LanguageType.CN;
    public Action languageChangeEvent;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        languageChangeEvent = null;
        curLanguage = BuildSetting.Instance.language;
    }

    public string GetText(string key)
    {
        return LocalizationData.Instance.GetText(key, curLanguage).Replace(@"\n", " \n");
    }

    public void SetLanguage(LanguageType language)
    {
        Debug.Log("设置语种:" + language);
        curLanguage = language;
        if (languageChangeEvent != null)
        {
            languageChangeEvent.Invoke();
        }
    }
}


