
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataMapBtnConfig_min : Singleton<DataMapBtnConfig_min>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<MapBtnConfig_minKey, Dictionary<MapBtnConfig_minType, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<MapBtnConfig_minType, string> GetContentByKey(MapBtnConfig_minKey _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(MapBtnConfig_minKey _key, MapBtnConfig_minType _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<MapBtnConfig_minType, string> GetContentByKey(string _key)
	{
		return dDataObj[(MapBtnConfig_minKey)Enum.Parse(typeof(MapBtnConfig_minKey), _key)];
	}

	public string GetContentByKeyAndType(string _key, MapBtnConfig_minType _type)
	{
		return dDataObj[(MapBtnConfig_minKey)Enum.Parse(typeof(MapBtnConfig_minKey), _key)][_type];
	}

	public string[] GetContentArr(MapBtnConfig_minKey _key, MapBtnConfig_minType _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<MapBtnConfig_minKey, Dictionary<MapBtnConfig_minType, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/MapBtnConfig_min", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<MapBtnConfig_minType, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(MapBtnConfig_minKey)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MapBtnConfig_minKey mapBtnConfig_minKey = (MapBtnConfig_minKey)enumerator.Current;
					string text = array[(int)(mapBtnConfig_minKey + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<MapBtnConfig_minType, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(MapBtnConfig_minType)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							MapBtnConfig_minType mapBtnConfig_minType = (MapBtnConfig_minType)enumerator2.Current;
							dictionary.Add(mapBtnConfig_minType, text.Split(',')[(int)mapBtnConfig_minType]);
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
					dDataObj.Add(mapBtnConfig_minKey, dictionary);
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
