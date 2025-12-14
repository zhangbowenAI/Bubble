using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationData : ICSVData<LocalizationData>
{

    public LocalizationData()
    {
        base.InitData("LocalizationData");
    }

    public string GetText(string key,LanguageType language)
    {
       return GetProperty(language.ToString(), key);
    }
}