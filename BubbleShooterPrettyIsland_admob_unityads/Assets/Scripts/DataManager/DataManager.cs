
using System;
using System.Collections.Generic;

public class DataManager
{
	public const string c_directoryName = "Data";

	public const string c_expandName = "txt";

	private static Dictionary<string, DataTable> s_dataCache = new Dictionary<string, DataTable>();

	public static DataTable GetData(string DataName)
	{
		try
		{
			if (s_dataCache.ContainsKey(DataName))
			{
				return s_dataCache[DataName];
			}
			DataTable dataTable = null;
			string empty = string.Empty;
			empty = ResourceManager.ReadTextFile(DataName);
			if (empty == string.Empty)
			{
				throw new Exception("Dont Find ->" + DataName + "<-");
			}
			dataTable = DataTable.Analysis(empty);
			dataTable.m_tableName = DataName;
			s_dataCache.Add(DataName, dataTable);
			return dataTable;
		}
		catch (Exception ex)
		{
			throw new Exception("GetData Exception ->" + DataName + "<- : " + ex.ToString());
		}
	}

	public static void CleanCache()
	{
		s_dataCache.Clear();
	}
}
