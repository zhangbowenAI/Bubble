
using System;
using System.Collections.Generic;
using UnityEngine;

public class RecordTable : Dictionary<string, SingleField>
{
	public static RecordTable Analysis(string data)
	{
		RecordTable recordTable = new RecordTable();
		Dictionary<string, SingleField> dictionary = JsonTool.Json2Dictionary<SingleField>(data);
		List<string> list = new List<string>(dictionary.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			recordTable.Add(list[i], dictionary[list[i]]);
		}
		return recordTable;
	}

	public static string Serialize(RecordTable table)
	{
		return JsonTool.Dictionary2Json(table);
	}

	public SingleField GetRecord(string key)
	{
		if (ContainsKey(key))
		{
			return base[key];
		}
		throw new Exception("RecordTable Not Find " + key);
	}

	public string GetRecord(string key, string defaultValue)
	{
		if (ContainsKey(key))
		{
			return base[key].GetString();
		}
		return defaultValue;
	}

	public bool GetRecord(string key, bool defaultValue)
	{
		if (ContainsKey(key))
		{
			return base[key].GetBool();
		}
		return defaultValue;
	}

	public int GetRecord(string key, int defaultValue)
	{
		if (ContainsKey(key))
		{
			return base[key].GetInt();
		}
		return defaultValue;
	}

	public float GetRecord(string key, float defaultValue)
	{
		if (ContainsKey(key))
		{
			return base[key].GetFloat();
		}
		return defaultValue;
	}

	public Vector2 GetRecord(string key, Vector2 defaultValue)
	{
		if (ContainsKey(key))
		{
			return base[key].GetVector2();
		}
		return defaultValue;
	}

	public Vector3 GetRecord(string key, Vector3 defaultValue)
	{
		if (ContainsKey(key))
		{
			return base[key].GetVector3();
		}
		return defaultValue;
	}

	public Color GetRecord(string key, Color defaultValue)
	{
		if (ContainsKey(key))
		{
			return base[key].GetColor();
		}
		return defaultValue;
	}

	public T GetEnumRecord<T>(string key, T defaultValue) where T : struct
	{
		if (ContainsKey(key))
		{
			return base[key].GetEnum<T>();
		}
		return defaultValue;
	}

	public void SetRecord(string key, string value)
	{
		if (ContainsKey(key))
		{
			base[key] = new SingleField(value);
		}
		else
		{
			Add(key, new SingleField(value));
		}
	}

	public void SetRecord(string key, int value)
	{
		if (ContainsKey(key))
		{
			base[key] = new SingleField(value);
		}
		else
		{
			Add(key, new SingleField(value));
		}
	}

	public void SetRecord(string key, bool value)
	{
		if (ContainsKey(key))
		{
			base[key] = new SingleField(value);
		}
		else
		{
			Add(key, new SingleField(value));
		}
	}

	public void SetRecord(string key, float value)
	{
		if (ContainsKey(key))
		{
			base[key] = new SingleField(value);
		}
		else
		{
			Add(key, new SingleField(value));
		}
	}

	public void SetRecord(string key, Vector2 value)
	{
		if (ContainsKey(key))
		{
			base[key] = new SingleField(value);
		}
		else
		{
			Add(key, new SingleField(value));
		}
	}

	public void SetRecord(string key, Vector3 value)
	{
		if (ContainsKey(key))
		{
			base[key] = new SingleField(value);
		}
		else
		{
			Add(key, new SingleField(value));
		}
	}

	public void SetRecord(string key, Color value)
	{
		if (ContainsKey(key))
		{
			base[key] = new SingleField(value);
		}
		else
		{
			Add(key, new SingleField(value));
		}
	}

	public void SetEnumRecord(string key, Enum value)
	{
		if (ContainsKey(key))
		{
			base[key] = new SingleField(value.ToString());
		}
		else
		{
			Add(key, new SingleField(value.ToString()));
		}
	}
}
