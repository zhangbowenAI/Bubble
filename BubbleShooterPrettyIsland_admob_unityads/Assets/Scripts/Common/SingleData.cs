
using System;
using System.Collections.Generic;
using UnityEngine;

public class SingleData : Dictionary<string, string>
{
    public DataTable data;

    public string m_SingleDataKey;

    public int GetInt(string key)
    {
        string text = null;
        try
        {
            if (ContainsKey(key))
            {
                text = base[key];
                return int.Parse(text);
            }
            if (data.m_defaultValue.ContainsKey(key))
            {
                text = data.m_defaultValue[key];
                return int.Parse(text);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SingleData GetInt Error TableName is :->" + data.m_tableName + "<- key : ->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- content: ->" + text + "<- \n" + ex.ToString());
        }
        throw new Exception("Don't Exist Value or DefaultValue TableName is :->" + data.m_tableName + "<- key : ->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<-");
    }

    public int[] GetIntArray(string key)
    {
        string text = null;
        try
        {
            if (ContainsKey(key))
            {
                text = base[key];
                return ParseTool.String2IntArray(text);
            }
            if (data.m_defaultValue.ContainsKey(key))
            {
                text = data.m_defaultValue[key];
                return ParseTool.String2IntArray(text);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SingleData GetIntArray Error TableName is :->" + data.m_tableName + "<- key : ->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- content: ->" + text + "<- \n" + ex.ToString());
        }
        throw new Exception("Don't Exist Value or DefaultValue TableName is :->" + data.m_tableName + "<- key : ->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<-");
    }

    public float GetFloat(string key)
    {
        try
        {
            if (ContainsKey(key))
            {
                return float.Parse(base[key]);
            }
            if (data.m_defaultValue.ContainsKey(key))
            {
                return float.Parse(data.m_defaultValue[key]);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SingleData GetFloat Error TableName is :->" + data.m_tableName + "<- key :->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + ex.ToString());
        }
        throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-");
    }

    public float[] GetFloatArray(string key)
    {
        try
        {
            if (ContainsKey(key))
            {
                return ParseTool.String2FloatArray(base[key]);
            }
            if (data.m_defaultValue.ContainsKey(key))
            {
                return ParseTool.String2FloatArray(data.m_defaultValue[key]);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SingleData GetFloatArray Error TableName is :->" + data.m_tableName + "<- key :->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + ex.ToString());
        }
        throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-");
    }

    public bool GetBool(string key)
    {
        string text = null;
        try
        {
            if (ContainsKey(key))
            {
                text = base[key];
                return bool.Parse(text);
            }
            if (data.m_defaultValue.ContainsKey(key))
            {
                text = data.m_defaultValue[key];
                return bool.Parse(text);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SingleData GetBool Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- content: ->" + text + "<- \n" + ex.ToString());
        }
        throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-");
    }

    public bool[] GetBoolArray(string key)
    {
        try
        {
            if (ContainsKey(key))
            {
                return ParseTool.String2BoolArray(base[key]);
            }
            if (data.m_defaultValue.ContainsKey(key))
            {
                return ParseTool.String2BoolArray(data.m_defaultValue[key]);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SingleData GetBoolArray Error TableName is :->" + data.m_tableName + "<- key :->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + ex.ToString());
        }
        throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-");
    }

    public string GetString(string key)
    {
        try
        {
            if (ContainsKey(key))
            {
                return StringFilter(base[key]);
            }
            if (data.m_defaultValue.ContainsKey(key))
            {
                return StringFilter(data.m_defaultValue[key]);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SingleData GetString Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + ex.ToString());
        }
        throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-");
    }

    private string StringFilter(string content)
    {
        if (content == "Null" || content == "null" || content == "NULL" || content == "nu11" || content == "none" || content == "nil" || content == string.Empty)
        {
            return null;
        }
        return content;
    }

    public Vector2 GetVector2(string key)
    {
        try
        {
            if (ContainsKey(key))
            {
                return ParseTool.String2Vector2(base[key]);
            }
            if (data.m_defaultValue.ContainsKey(key))
            {
                return ParseTool.String2Vector2(data.m_defaultValue[key]);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SingleData GetVector2 Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + ex.ToString());
        }
        throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-");
    }

    public Vector2[] GetVector2Array(string key)
    {
        try
        {
            if (ContainsKey(key))
            {
                return ParseTool.String2Vector2Array(base[key]);
            }
            if (data.m_defaultValue.ContainsKey(key))
            {
                return ParseTool.String2Vector2Array(data.m_defaultValue[key]);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SingleData GetVector2 Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + ex.ToString());
        }
        throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-");
    }

    public Vector3[] GetVector3Array(string key)
    {
        try
        {
            if (ContainsKey(key))
            {
                return ParseTool.String2Vector3Array(base[key]);
            }
            if (data.m_defaultValue.ContainsKey(key))
            {
                return ParseTool.String2Vector3Array(data.m_defaultValue[key]);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SingleData GetVector3Array Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + ex.ToString());
        }
        throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-");
    }

    public Vector3 GetVector3(string key)
    {
        try
        {
            if (ContainsKey(key))
            {
                return ParseTool.String2Vector3(base[key]);
            }
            if (data.m_defaultValue.ContainsKey(key))
            {
                return ParseTool.String2Vector3(data.m_defaultValue[key]);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SingleData GetVector3 Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + ex.ToString());
        }
        throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-");
    }

    public Color GetColor(string key)
    {
        try
        {
            if (ContainsKey(key))
            {
                return ParseTool.String2Color(base[key]);
            }
            if (data.m_defaultValue.ContainsKey(key))
            {
                return ParseTool.String2Color(data.m_defaultValue[key]);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SingleData GetColor Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + ex.ToString());
        }
        throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-");
    }

    public T GetEnum<T>(string key) where T : struct
    {
        try
        {
            if (ContainsKey(key))
            {
                return (T)Enum.Parse(typeof(T), base[key]);
            }
            if (data.m_defaultValue.ContainsKey(key))
            {
                return (T)Enum.Parse(typeof(T), data.m_defaultValue[key]);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SingleData GetEnum Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + ex.ToString());
        }
        throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-");
    }

    public string[] GetStringArray(string key)
    {
        try
        {
            if (ContainsKey(key))
            {
                return ParseTool.String2StringArray(base[key]);
            }
            if (data.m_defaultValue.ContainsKey(key))
            {
                return ParseTool.String2StringArray(data.m_defaultValue[key]);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("SingleData GetStringArray Error TableName is :->" + data.m_tableName + "<- key->" + key + "<-  singleDataName : ->" + m_SingleDataKey + "<- \n" + ex.ToString());
        }
        throw new Exception("Don't Exist Value or DefaultValue by ->" + key + "<- TableName is : ->" + data.m_tableName + "<- singleDataName : ->" + m_SingleDataKey + "<-");
    }
}
