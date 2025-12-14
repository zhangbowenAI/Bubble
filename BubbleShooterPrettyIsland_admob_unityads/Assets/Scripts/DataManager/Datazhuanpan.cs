
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Datazhuanpan : Singleton<Datazhuanpan>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<zhuanpanKey, Dictionary<zhuanpanType, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<zhuanpanType, string> GetContentByKey(zhuanpanKey _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(zhuanpanKey _key, zhuanpanType _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<zhuanpanType, string> GetContentByKey(string _key)
	{
		return dDataObj[(zhuanpanKey)Enum.Parse(typeof(zhuanpanKey), _key)];
	}

	public string GetContentByKeyAndType(string _key, zhuanpanType _type)
	{
		return dDataObj[(zhuanpanKey)Enum.Parse(typeof(zhuanpanKey), _key)][_type];
	}

	public string[] GetContentArr(zhuanpanKey _key, zhuanpanType _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<zhuanpanKey, Dictionary<zhuanpanType, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/zhuanpan", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<zhuanpanType, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(zhuanpanKey)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					zhuanpanKey zhuanpanKey = (zhuanpanKey)enumerator.Current;
					string text = array[(int)(zhuanpanKey + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<zhuanpanType, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(zhuanpanType)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							zhuanpanType zhuanpanType = (zhuanpanType)enumerator2.Current;
							dictionary.Add(zhuanpanType, text.Split(',')[(int)zhuanpanType]);
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
					dDataObj.Add(zhuanpanKey, dictionary);
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
