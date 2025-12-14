
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataWinVideoChest : Singleton<DataWinVideoChest>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<WinVideoChestKey, Dictionary<WinVideoChestType, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<WinVideoChestType, string> GetContentByKey(WinVideoChestKey _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(WinVideoChestKey _key, WinVideoChestType _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<WinVideoChestType, string> GetContentByKey(string _key)
	{
		return dDataObj[(WinVideoChestKey)Enum.Parse(typeof(WinVideoChestKey), _key)];
	}

	public string GetContentByKeyAndType(string _key, WinVideoChestType _type)
	{
		return dDataObj[(WinVideoChestKey)Enum.Parse(typeof(WinVideoChestKey), _key)][_type];
	}

	public string[] GetContentArr(WinVideoChestKey _key, WinVideoChestType _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<WinVideoChestKey, Dictionary<WinVideoChestType, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/WinVideoChest", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<WinVideoChestType, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(WinVideoChestKey)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					WinVideoChestKey winVideoChestKey = (WinVideoChestKey)enumerator.Current;
					string text = array[(int)(winVideoChestKey + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<WinVideoChestType, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(WinVideoChestType)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							WinVideoChestType winVideoChestType = (WinVideoChestType)enumerator2.Current;
							dictionary.Add(winVideoChestType, text.Split(',')[(int)winVideoChestType]);
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
					dDataObj.Add(winVideoChestKey, dictionary);
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
