
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Datazhuanpanios : Singleton<Datazhuanpanios>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<zhuanpaniosKey, Dictionary<zhuanpaniosType, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<zhuanpaniosType, string> GetContentByKey(zhuanpaniosKey _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(zhuanpaniosKey _key, zhuanpaniosType _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<zhuanpaniosType, string> GetContentByKey(string _key)
	{
		return dDataObj[(zhuanpaniosKey)Enum.Parse(typeof(zhuanpaniosKey), _key)];
	}

	public string GetContentByKeyAndType(string _key, zhuanpaniosType _type)
	{
		return dDataObj[(zhuanpaniosKey)Enum.Parse(typeof(zhuanpaniosKey), _key)][_type];
	}

	public string[] GetContentArr(zhuanpaniosKey _key, zhuanpaniosType _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<zhuanpaniosKey, Dictionary<zhuanpaniosType, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/zhuanpanios", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<zhuanpaniosType, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(zhuanpaniosKey)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					zhuanpaniosKey zhuanpaniosKey = (zhuanpaniosKey)enumerator.Current;
					string text = array[(int)(zhuanpaniosKey + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<zhuanpaniosType, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(zhuanpaniosType)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							zhuanpaniosType zhuanpaniosType = (zhuanpaniosType)enumerator2.Current;
							dictionary.Add(zhuanpaniosType, text.Split(',')[(int)zhuanpaniosType]);
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
					dDataObj.Add(zhuanpaniosKey, dictionary);
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
