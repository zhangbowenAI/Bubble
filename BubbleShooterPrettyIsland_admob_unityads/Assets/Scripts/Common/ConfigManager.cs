
using System;
using System.Collections.Generic;

public static class ConfigManager
{
    public const string c_directoryName = "Config";

    public const string c_expandName = "json";

    private static Dictionary<string, Dictionary<string, SingleField>> s_configCache = new Dictionary<string, Dictionary<string, SingleField>>();

    public static Dictionary<string, SingleField> GetData(string ConfigName)
    {
        if (s_configCache.ContainsKey(ConfigName))
        {
            return s_configCache[ConfigName];
        }
        string empty = string.Empty;
        empty = ResourceManager.ReadTextFile(ConfigName);
        if (empty == string.Empty)
        {
            throw new Exception("ConfigManager GetData not find " + ConfigName);
        }
        Dictionary<string, SingleField> dictionary = JsonTool.Json2Dictionary<SingleField>(empty);
        s_configCache.Add(ConfigName, dictionary);
        return dictionary;
    }

    public static void CleanCache()
    {
        s_configCache.Clear();
    }
}
