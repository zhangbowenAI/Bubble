
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataMapBtnConfig : Singleton<DataMapBtnConfig>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<MapBtnConfigKey, Dictionary<MapBtnConfigType, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<MapBtnConfigType, string> GetContentByKey(MapBtnConfigKey _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(MapBtnConfigKey _key, MapBtnConfigType _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<MapBtnConfigType, string> GetContentByKey(string _key)
	{
		return dDataObj[(MapBtnConfigKey)Enum.Parse(typeof(MapBtnConfigKey), _key)];
	}

	public string GetContentByKeyAndType(string _key, MapBtnConfigType _type)
	{
		return dDataObj[(MapBtnConfigKey)Enum.Parse(typeof(MapBtnConfigKey), _key)][_type];
	}

	public string[] GetContentArr(MapBtnConfigKey _key, MapBtnConfigType _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<MapBtnConfigKey, Dictionary<MapBtnConfigType, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/MapBtnConfig", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<MapBtnConfigType, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(MapBtnConfigKey)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MapBtnConfigKey mapBtnConfigKey = (MapBtnConfigKey)enumerator.Current;
					string text = array[(int)(mapBtnConfigKey + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<MapBtnConfigType, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(MapBtnConfigType)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							MapBtnConfigType mapBtnConfigType = (MapBtnConfigType)enumerator2.Current;
							dictionary.Add(mapBtnConfigType, text.Split(',')[(int)mapBtnConfigType]);
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator2 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					dDataObj.Add(mapBtnConfigKey, dictionary);
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
		}
	}
}
