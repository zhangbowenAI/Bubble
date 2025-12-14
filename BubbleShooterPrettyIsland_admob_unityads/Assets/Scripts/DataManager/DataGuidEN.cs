
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataGuidEN : Singleton<DataGuidEN>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<GuidENKey, Dictionary<GuidENType, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<GuidENType, string> GetContentByKey(GuidENKey _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(GuidENKey _key, GuidENType _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<GuidENType, string> GetContentByKey(string _key)
	{
		return dDataObj[(GuidENKey)Enum.Parse(typeof(GuidENKey), _key)];
	}

	public string GetContentByKeyAndType(string _key, GuidENType _type)
	{
		return dDataObj[(GuidENKey)Enum.Parse(typeof(GuidENKey), _key)][_type];
	}

	public string[] GetContentArr(GuidENKey _key, GuidENType _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<GuidENKey, Dictionary<GuidENType, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/GuidEN", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<GuidENType, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(GuidENKey)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GuidENKey guidENKey = (GuidENKey)enumerator.Current;
					string text = array[(int)(guidENKey + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<GuidENType, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(GuidENType)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							GuidENType guidENType = (GuidENType)enumerator2.Current;
							dictionary.Add(guidENType, text.Split(',')[(int)guidENType]);
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
					dDataObj.Add(guidENKey, dictionary);
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
