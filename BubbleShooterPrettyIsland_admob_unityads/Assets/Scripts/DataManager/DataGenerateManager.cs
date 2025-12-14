
using System;
using System.Collections.Generic;
//using System.Runtime.CompilerServices;

public class DataGenerateManager<T> where T : DataGenerateBase, new()
{
	private static Dictionary<string, T> s_dict = new Dictionary<string, T>();

	private static bool s_isInit = false;

	public static T GetData(string key)
	{
		if (key == null)
		{
			throw new Exception("DataGenerateManager<" + typeof(T).Name + "> GetData key is Null !");
		}
		if (!s_isInit)
		{
			s_isInit = true;
			GlobalEvent.AddEvent(MemoryEvent.FreeHeapMemory, CleanCache);
		}
		if (s_dict.ContainsKey(key))
		{
			return s_dict[key];
		}
		T val = new T();
		val.LoadData(key);
		s_dict.Add(key, val);
		return val;
	}

	public static void PreLoad()
	{
		if (!s_isInit)
		{
			s_isInit = true;
			GlobalEvent.AddEvent(MemoryEvent.FreeHeapMemory, CleanCache);
		}
		DataTable dataTable = GetDataTable();
		for (int i = 0; i < dataTable.TableIDs.Count; i++)
		{
			GetData(dataTable.TableIDs[i]);
		}
	}

	public static Dictionary<string, T> GetAllData()
	{
		CleanCache();
		PreLoad();
		return s_dict;
	}

	public static DataTable GetDataTable()
	{
		string dataName = typeof(T).Name.Replace("Generate", string.Empty);
		return DataManager.GetData(dataName);
	}

	public static void CleanCache(params object[] objs)
	{
		s_dict.Clear();
	}
}
