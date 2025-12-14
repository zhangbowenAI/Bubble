
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecordManager
{
    public const string c_directoryName = "Record";

    public const string c_expandName = "json";

    private static Dictionary<string, RecordTable> s_RecordCache = new Dictionary<string, RecordTable>();

    private static Deserializer des = new Deserializer();

    public static RecordTable GetData(string RecordName)
    {
        if (s_RecordCache.ContainsKey(RecordName))
        {
            return s_RecordCache[RecordName];
        }
        RecordTable recordTable = null;
        string text = string.Empty;
        string absolutePath = PathTool.GetAbsolutePath(ResLoadLocation.Persistent, PathTool.GetRelativelyPath("Record", RecordName, "json"));
        if (File.Exists(absolutePath))
        {
            text = ResourceIOTool.ReadStringByFile(absolutePath);
        }
        recordTable = ((!(text == string.Empty)) ? RecordTable.Analysis(text) : new RecordTable());
        s_RecordCache.Add(RecordName, recordTable);
        return recordTable;
    }

    public static void SaveData(string RecordName, RecordTable data)
    {
        ResourceIOTool.WriteStringByFile(PathTool.GetAbsolutePath(ResLoadLocation.Persistent, PathTool.GetRelativelyPath("Record", RecordName, "json")), RecordTable.Serialize(data));
    }

    public static void CleanRecord(string recordName)
    {
        RecordTable data = GetData(recordName);
        data.Clear();
        SaveData(recordName, data);
    }

    public static void CleanAllRecord()
    {
        FileTool.DeleteDirectory(Application.persistentDataPath + "/Record");
        Debug.LogError(Application.persistentDataPath + "/Record");
        CleanCache();
    }

    public static void CleanCache()
    {
        s_RecordCache.Clear();
    }

    public static void SaveRecord(string RecordName, string key, string value)
    {
        RecordTable data = GetData(RecordName);
        data.SetRecord(key, value);
        SaveData(RecordName, data);
    }

    public static void SaveRecord(string RecordName, string key, int value)
    {
        RecordTable data = GetData(RecordName);
        data.SetRecord(key, value);
        SaveData(RecordName, data);
    }

    public static void SaveRecord(string RecordName, string key, bool value)
    {
        RecordTable data = GetData(RecordName);
        data.SetRecord(key, value);
        SaveData(RecordName, data);
    }

    public static void SaveRecord(string RecordName, string key, float value)
    {
        RecordTable data = GetData(RecordName);
        data.SetRecord(key, value);
        SaveData(RecordName, data);
    }

    public static void SaveRecord(string RecordName, string key, Vector2 value)
    {
        RecordTable data = GetData(RecordName);
        data.SetRecord(key, value);
        SaveData(RecordName, data);
    }

    public static void SaveRecord(string RecordName, string key, Vector3 value)
    {
        RecordTable data = GetData(RecordName);
        data.SetRecord(key, value);
        SaveData(RecordName, data);
    }

    public static void SaveRecord(string RecordName, string key, Color value)
    {
        RecordTable data = GetData(RecordName);
        data.SetRecord(key, value);
        SaveData(RecordName, data);
    }

    public static void SaveRecord<T>(string RecordName, string key, T value)
    {
        string value2 = Serializer.Serialize(value);
        SaveRecord(RecordName, key, value2);
    }

    public static int GetIntRecord(string RecordName, string key, int defaultValue)
    {
        RecordTable data = GetData(RecordName);
        return data.GetRecord(key, defaultValue);
    }

    public static string GetStringRecord(string RecordName, string key, string defaultValue)
    {
        RecordTable data = GetData(RecordName);
        return data.GetRecord(key, defaultValue);
    }

    public static bool GetBoolRecord(string RecordName, string key, bool defaultValue)
    {
        RecordTable data = GetData(RecordName);
        return data.GetRecord(key, defaultValue);
    }

    public static float GetFloatRecord(string RecordName, string key, float defaultValue)
    {
        RecordTable data = GetData(RecordName);
        return data.GetRecord(key, defaultValue);
    }

    public static Vector2 GetVector2Record(string RecordName, string key, Vector2 defaultValue)
    {
        RecordTable data = GetData(RecordName);
        return data.GetRecord(key, defaultValue);
    }

    public static Vector3 GetVector3Record(string RecordName, string key, Vector3 defaultValue)
    {
        RecordTable data = GetData(RecordName);
        return data.GetRecord(key, defaultValue);
    }

    public static Color GetColorRecord(string RecordName, string key, Color defaultValue)
    {
        RecordTable data = GetData(RecordName);
        return data.GetRecord(key, defaultValue);
    }

    public static T GetTRecord<T>(string RecordName, string key, T defaultValue)
    {
        string stringRecord = GetStringRecord(RecordName, key, null);
        if (stringRecord == null)
        {
            return defaultValue;
        }
        return des.Deserialize<T>(stringRecord);
    }
}
