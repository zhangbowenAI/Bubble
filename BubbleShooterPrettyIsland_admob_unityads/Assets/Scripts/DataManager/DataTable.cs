
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DataTable : Dictionary<string, SingleData>
{
	private const char c_split = '\t';

	private const string c_newline = "\r\n";

	private const string c_defaultValueTableTitle = "default";

	private const string c_noteTableTitle = "note";

	private const string c_fieldTypeTableTitle = "type";

	private const char c_EnumSplit = '|';

	private const char c_DataFieldAssetTypeSplit = '&';

	public string m_tableName;

	public Dictionary<string, string> m_defaultValue = new Dictionary<string, string>();

	public Dictionary<string, string> m_noteValue = new Dictionary<string, string>();

	public Dictionary<string, FieldType> m_tableTypes = new Dictionary<string, FieldType>();

	public Dictionary<string, string> m_tableEnumTypes = new Dictionary<string, string>();

	public List<string> TableKeys = new List<string>();

	public List<string> TableIDs = new List<string>();

	public Dictionary<string, DataFieldAssetType> m_fieldAssetTypes = new Dictionary<string, DataFieldAssetType>();

	public static DataTable Analysis(string stringData)
	{
		string text = string.Empty;
		int num = 0;
		int num2 = 0;
		string text2 = string.Empty;
		string text3 = string.Empty;
		try
		{
			int num3 = 0;
			DataTable dataTable = new DataTable();
			string[] array = stringData.Split("\r\n".ToCharArray());
			text = "Parsing Key";
			dataTable.TableKeys = new List<string>();
			string[] array2 = ConvertStringArray(array[0]);
			for (int i = 0; i < array2.Length; i++)
			{
				num2 = i;
				if (!array2[i].Equals(string.Empty))
				{
					dataTable.TableKeys.Add(array2[i]);
				}
			}
			for (num3 = 1; num3 < array.Length; num3++)
			{
				if (array[num3] != string.Empty && array[num3] != null)
				{
					num = num3;
					string[] array3 = ConvertStringArray(array[num3]);
					if (array3[0].Equals("note"))
					{
						text = "Parsing comments";
						AnalysisNoteValue(dataTable, array3);
					}
					else if (array3[0].Equals("default"))
					{
						text = "Parsing default values";
						AnalysisDefaultValue(dataTable, array3);
					}
					else
					{
						if (!array3[0].Equals("type"))
						{
							text = "Parsing the text";
							break;
						}
						text = "Resolution type";
						AnalysisFieldType(dataTable, array3);
					}
				}
			}
			dataTable.TableIDs = new List<string>();
			for (int j = num3; j < array.Length; j++)
			{
				num = j;
				SingleData singleData = new SingleData();
				singleData.data = dataTable;
				if (array[j] != string.Empty && array[j] != null)
				{
					string[] array4 = ConvertStringArray(array[j]);
					for (int k = 0; k < dataTable.TableKeys.Count; k++)
					{
						num2 = k;
						text2 = array4[0];
						if (!array4[k].Equals(string.Empty))
						{
							text3 = dataTable.TableKeys[k];
							singleData.Add(dataTable.TableKeys[k], array4[k]);
						}
					}
					dataTable.AddData(singleData);
				}
			}
			return dataTable;
		}
		catch (Exception ex)
		{
			throw new Exception("DataTable Analysis Error: Wrong location：" + text + " Row:" + num / 2 + " Column：" + num2 + " key:->" + text2 + "<- PropertyName：->" + text3 + "<-\n" + ex.ToString());
		}
	}

	public static void AnalysisNoteValue(DataTable l_data, string[] l_lineData)
	{
		l_data.m_noteValue = new Dictionary<string, string>();
		for (int i = 0; i < l_lineData.Length && i < l_data.TableKeys.Count; i++)
		{
			if (!l_lineData[i].Equals(string.Empty))
			{
				l_data.m_noteValue.Add(l_data.TableKeys[i], l_lineData[i]);
			}
		}
	}

	public static void AnalysisDefaultValue(DataTable l_data, string[] l_lineData)
	{
		l_data.m_defaultValue = new Dictionary<string, string>();
		for (int i = 0; i < l_lineData.Length && i < l_data.TableKeys.Count; i++)
		{
			if (!l_lineData[i].Equals(string.Empty))
			{
				l_data.m_defaultValue.Add(l_data.TableKeys[i], l_lineData[i]);
			}
		}
	}

	public static void AnalysisFieldType(DataTable l_data, string[] l_lineData)
	{
		l_data.m_tableTypes = new Dictionary<string, FieldType>();
		for (int i = 1; i < l_lineData.Length && i < l_data.TableKeys.Count; i++)
		{
			if (!l_lineData[i].Equals(string.Empty))
			{
				string key = l_data.TableKeys[i];
				string[] array = l_lineData[i].Split('&');
				string[] array2 = array[0].Split('|');
				try
				{
					l_data.m_tableTypes.Add(key, (FieldType)Enum.Parse(typeof(FieldType), array2[0]));
					if (array2.Length > 1)
					{
						l_data.m_tableEnumTypes.Add(key, array2[1]);
					}
				}
				catch (Exception ex)
				{
					throw new Exception("AnalysisFieldType Exception: " + array2 + "\n" + ex.ToString());
				}
				if (array.Length > 1)
				{
					l_data.m_fieldAssetTypes.Add(key, (DataFieldAssetType)Enum.Parse(typeof(DataFieldAssetType), array[1]));
				}
				else
				{
					l_data.m_fieldAssetTypes.Add(key, DataFieldAssetType.Data);
				}
			}
		}
	}

	public static string Serialize(DataTable data)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < data.TableKeys.Count; i++)
		{
			stringBuilder.Append(data.TableKeys[i]);
			if (i != data.TableKeys.Count - 1)
			{
				stringBuilder.Append('\t');
			}
			else
			{
				stringBuilder.Append("\r\n");
			}
		}
		List<string> list = new List<string>(data.m_tableTypes.Keys);
		UnityEngine.Debug.Log("type count " + list.Count);
		stringBuilder.Append("type");
		if (list.Count > 0)
		{
			stringBuilder.Append('\t');
			for (int j = 1; j < data.TableKeys.Count; j++)
			{
				string key = data.TableKeys[j];
				string empty = string.Empty;
				if (data.m_tableTypes.ContainsKey(key))
				{
					empty = data.m_tableTypes[key].ToString();
					if (data.m_tableEnumTypes.ContainsKey(key))
					{
						empty = empty + '|' + data.m_tableEnumTypes[key];
					}
				}
				else
				{
					empty = FieldType.String.ToString();
				}
				if (data.m_fieldAssetTypes.ContainsKey(key) && data.m_fieldAssetTypes[key] != 0)
				{
					empty = empty + "&" + data.m_fieldAssetTypes[key];
				}
				stringBuilder.Append(empty);
				if (j != data.TableKeys.Count - 1)
				{
					stringBuilder.Append('\t');
				}
				else
				{
					stringBuilder.Append("\r\n");
				}
			}
		}
		else
		{
			stringBuilder.Append("\r\n");
		}
		List<string> list2 = new List<string>(data.m_noteValue.Keys);
		stringBuilder.Append("note");
		if (list2.Count > 0)
		{
			stringBuilder.Append('\t');
			for (int k = 1; k < data.TableKeys.Count; k++)
			{
				string key2 = data.TableKeys[k];
				string empty2 = string.Empty;
				empty2 = ((!data.m_noteValue.ContainsKey(key2)) ? string.Empty : data.m_noteValue[key2]);
				stringBuilder.Append(empty2);
				if (k != data.TableKeys.Count - 1)
				{
					stringBuilder.Append('\t');
				}
				else
				{
					stringBuilder.Append("\r\n");
				}
			}
		}
		else
		{
			stringBuilder.Append("\r\n");
		}
		List<string> list3 = new List<string>(data.m_defaultValue.Keys);
		stringBuilder.Append("default");
		if (list3.Count > 0)
		{
			stringBuilder.Append('\t');
			for (int l = 1; l < data.TableKeys.Count; l++)
			{
				string key3 = data.TableKeys[l];
				string empty3 = string.Empty;
				empty3 = ((!data.m_defaultValue.ContainsKey(key3)) ? string.Empty : data.m_defaultValue[key3]);
				stringBuilder.Append(empty3);
				if (l != data.TableKeys.Count - 1)
				{
					stringBuilder.Append('\t');
				}
				else
				{
					stringBuilder.Append("\r\n");
				}
			}
		}
		else
		{
			stringBuilder.Append("\r\n");
		}
		foreach (string key5 in data.Keys)
		{
			SingleData singleData = data[key5];
			for (int m = 0; m < data.TableKeys.Count; m++)
			{
				string value = string.Empty;
				string key4 = data.TableKeys[m];
				string b = string.Empty;
				if (data.m_defaultValue.ContainsKey(key4))
				{
					b = data.m_defaultValue[key4];
				}
				if (singleData.ContainsKey(key4) && singleData[key4] != b)
				{
					value = singleData[key4];
				}
				stringBuilder.Append(value);
				if (m != data.TableKeys.Count - 1)
				{
					stringBuilder.Append('\t');
				}
				else
				{
					stringBuilder.Append("\r\n");
				}
			}
		}
		return stringBuilder.ToString();
	}

	public static string[] ConvertStringArray(string lineContent)
	{
		List<string> list = new List<string>();
		int num = 0;
		bool flag = true;
		for (int i = 0; i < lineContent.Length; i++)
		{
			if (flag)
			{
				if (lineContent[i] == '\t')
				{
					list.Add(lineContent.Substring(num, i - num));
					num = i + 1;
				}
				else if (lineContent[i] == '"')
				{
					flag = false;
				}
			}
			else if (lineContent[i] == '"')
			{
				flag = true;
			}
		}
		list.Add(lineContent.Substring(num, lineContent.Length - num));
		return list.ToArray();
	}

	public FieldType GetFieldType(string key)
	{
		if (key == TableKeys[0])
		{
			return FieldType.String;
		}
		if (m_tableTypes.ContainsKey(key))
		{
			return m_tableTypes[key];
		}
		return FieldType.String;
	}

	public void SetFieldType(string key, FieldType type, string enumType)
	{
		if (key == TableKeys[0])
		{
			return;
		}
		if (m_tableTypes.ContainsKey(key))
		{
			m_tableTypes[key] = type;
		}
		else
		{
			m_tableTypes.Add(key, type);
		}
		if (enumType != null)
		{
			if (m_tableEnumTypes.ContainsKey(key))
			{
				m_tableEnumTypes[key] = enumType;
			}
			else
			{
				m_tableEnumTypes.Add(key, enumType);
			}
		}
	}

	public SingleData GetLineFromKey(string key)
	{
		SingleData result = null;
		if (ContainsKey(key))
		{
			result = base[key];
		}
		return result;
	}

	public string GetEnumType(string key)
	{
		if (m_tableEnumTypes.ContainsKey(key))
		{
			return m_tableEnumTypes[key];
		}
		return null;
	}

	public string GetDefault(string key)
	{
		if (m_defaultValue.ContainsKey(key))
		{
			return m_defaultValue[key];
		}
		return null;
	}

	public void SetDefault(string key, string value)
	{
		if (!m_defaultValue.ContainsKey(key))
		{
			m_defaultValue.Add(key, value);
		}
		else
		{
			m_defaultValue[key] = value;
		}
	}

	public void SetNote(string key, string note)
	{
		if (!m_noteValue.ContainsKey(key))
		{
			m_noteValue.Add(key, note);
		}
		else
		{
			m_noteValue[key] = note;
		}
	}

	public string GetNote(string key)
	{
		if (!m_noteValue.ContainsKey(key))
		{
			return null;
		}
		return m_noteValue[key];
	}

	public void AddData(SingleData data)
	{
		if (data.ContainsKey(TableKeys[0]))
		{
			data.m_SingleDataKey = data[TableKeys[0]];
			Add(data[TableKeys[0]], data);
			TableIDs.Add(data[TableKeys[0]]);
			return;
		}
		throw new Exception("Add SingleData fail! The dataTable dont have MainKey!");
	}

	public void SetData(SingleData data)
	{
		string key = TableKeys[0];
		if (data.ContainsKey(key))
		{
			string text = data[key];
			if (ContainsKey(text))
			{
				base[text] = data;
				return;
			}
			Add(text, data);
			TableIDs.Add(text);
			return;
		}
		throw new Exception("Add SingleData fail! The dataTable dont have MainKeyKey!");
	}

	public void RemoveData(string key)
	{
		if (ContainsKey(key))
		{
			Remove(key);
			TableIDs.Remove(key);
			return;
		}
		throw new Exception("Add SingleData fail!");
	}
}
