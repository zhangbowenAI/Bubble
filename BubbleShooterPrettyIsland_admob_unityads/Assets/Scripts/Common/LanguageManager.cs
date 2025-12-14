
using System;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager
{
    public const string c_configFileName = "LanguageConfig";

    public const string c_defaultLanguageKey = "DefaultLanguage";

    public const string c_languageListKey = "languageFields";

    public const string c_moduleListKey = "Module";

    public const string c_defaultModuleKey = "default";

    public const string c_DataFilePrefix = "LangData_";

    public const string c_mainKey = "key";

    public const string c_valueKey = "value";

    public static List<string> s_LanguageList = new List<string>();

    public static List<string> s_modelList = new List<string>();

    public static SystemLanguage s_defaultlanguage = SystemLanguage.ChineseSimplified;

    public static SystemLanguage s_currentLanguage = SystemLanguage.ChineseSimplified;

    public static Dictionary<string, DataTable> s_languageDataDict = new Dictionary<string, DataTable>();

    private static bool isInit = false;

    public static bool IsInit
    {
        get
        {
            return isInit;
        }
        set
        {
            isInit = value;
        }
    }

    public static void Init()
    {
        if (!isInit)
        {
            isInit = true;
            UnityEngine.Debug.LogError("Current language: " + Application.systemLanguage);
            LoadConfig();
            SetLanguage(Application.systemLanguage);
        }
    }

    public static void SetLanguage(SystemLanguage lang)
    {
        s_currentLanguage = lang;
        if (s_LanguageList.Contains(lang.ToString()))
        {
            Loadlanguage(lang);
        }
        else
        {
            Loadlanguage(s_defaultlanguage);
        }
        GlobalEvent.DispatchEvent(LanguageEventEnum.LanguageChange, lang);
    }

    private static void LoadConfig()
    {
        s_LanguageList.Clear();
        s_modelList.Clear();
        Dictionary<string, SingleField> data = ConfigManager.GetData("LanguageConfig");
        s_defaultlanguage = data["DefaultLanguage"].GetEnum<SystemLanguage>();
        string[] stringArray = data["languageFields"].GetStringArray();
        for (int i = 0; i < stringArray.Length; i++)
        {
            s_LanguageList.Add(stringArray[i]);
        }
        string[] stringArray2 = data["Module"].GetStringArray();
        for (int j = 0; j < stringArray2.Length; j++)
        {
            s_modelList.Add(stringArray2[j]);
        }
    }

    private static void Loadlanguage(SystemLanguage language)
    {
        s_languageDataDict.Clear();
        for (int i = 0; i < s_modelList.Count; i++)
        {
            s_languageDataDict.Add(s_modelList[i], DataManager.GetData(GetLanguageDataSaveName(language.ToString(), s_modelList[i])));
        }
    }

    [Obsolete]
    public static string GetContent(string contentID, List<object> contentParams)
    {
        return GetContent("default", contentID, contentParams.ToArray());
    }

    [Obsolete]
    public static string GetContent(string contentID, params object[] contentParams)
    {
        return GetContent("default", contentID, contentParams);
    }

    public static string GetContent(string moduleName, string contentID, List<object> contentParams)
    {
        return GetContent(moduleName, contentID, contentParams.ToArray());
    }

    public static string GetContentByKey(string moduleName_key, params object[] contentParams)
    {
        if (string.IsNullOrEmpty(moduleName_key))
        {
            return "Error : key is null";
        }
        string[] array = ChangeKey2ModuleID(moduleName_key);
        if (array == null)
        {
            return "Error : format is error！";
        }
        return GetContent(array[0], array[1], contentParams);
    }

    public static bool HaveKey(string moduleName_key)
    {
        Init();
        if (string.IsNullOrEmpty(moduleName_key))
        {
            return false;
        }
        string[] array = ChangeKey2ModuleID(moduleName_key);
        if (array == null)
        {
            return false;
        }
        string key = array[0];
        string key2 = array[1];
        if (!s_languageDataDict.ContainsKey(key))
        {
            return false;
        }
        DataTable dataTable = s_languageDataDict[key];
        if (!dataTable.ContainsKey(key2))
        {
            return false;
        }
        return true;
    }

    private static string[] ChangeKey2ModuleID(string moduleName_key)
    {
        if (!moduleName_key.Contains("/"))
        {
            UnityEngine.Debug.LogError("Multilingual key format error ：" + moduleName_key);
            return null;
        }
        string[] array = moduleName_key.Split('/');
        string text = string.Empty;
        string empty = string.Empty;
        if (array.Length > 2)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                text += array[i];
                if (i != array.Length - 2)
                {
                    text += "_";
                }
            }
            empty = array[array.Length - 1];
        }
        else
        {
            text = array[0];
            empty = array[1];
        }
        return new string[2]
        {
            text,
            empty
        };
    }

    public static string GetContent(string moduleName, string contentID, params object[] contentParams)
    {
        Init();
        string text = null;
        if (!s_languageDataDict.ContainsKey(moduleName))
        {
            UnityEngine.Debug.LogError("Dont find language moduleName:->" + moduleName + "<- contentID: ->" + contentID + "<-");
            return "Dont find language : ->" + contentID + "<-";
        }
        DataTable dataTable = s_languageDataDict[moduleName];
        if (dataTable.ContainsKey(contentID))
        {
            text = dataTable[contentID].GetString("value");
            if (contentParams != null && contentParams.Length > 0)
            {
                for (int i = 0; i < contentParams.Length; i++)
                {
                    string oldValue = "{" + i + "}";
                    text = text.Replace(oldValue, contentParams[i].ToString());
                }
            }
            if (ApplicationManager.Instance != null && ApplicationManager.appMode == AppMode.Developing)
            {
                text = "[" + text + "]";
            }
            return text;
        }
        UnityEngine.Debug.LogError("Dont find language moduleName:->" + moduleName + "<- contentID: ->" + contentID + "<-");
        return "Dont find language : ->" + contentID + "<-";
    }

    public static string GetLanguageDataSaveName(string langeuageName, string modelName)
    {
        if (Application.isPlaying)
        {
            return GetLanguageDataName(langeuageName, modelName);
        }
        return "Language/" + langeuageName + "/" + GetLanguageDataName(langeuageName, modelName);
    }

    public static string GetLanguageDataName(string langeuageName, string modelName)
    {
        return "LangData_" + langeuageName + "_" + modelName;
    }
}
