
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataChestBox : Singleton<DataChestBox>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<ChestBoxKey, Dictionary<ChestBoxType, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<ChestBoxType, string> GetContentByKey(ChestBoxKey _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(ChestBoxKey _key, ChestBoxType _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<ChestBoxType, string> GetContentByKey(string _key)
	{
		return dDataObj[(ChestBoxKey)Enum.Parse(typeof(ChestBoxKey), _key)];
	}

	public string GetContentByKeyAndType(string _key, ChestBoxType _type)
	{
		return dDataObj[(ChestBoxKey)Enum.Parse(typeof(ChestBoxKey), _key)][_type];
	}

	public string[] GetContentArr(ChestBoxKey _key, ChestBoxType _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<ChestBoxKey, Dictionary<ChestBoxType, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/ChestBox", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<ChestBoxType, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(ChestBoxKey)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ChestBoxKey chestBoxKey = (ChestBoxKey)enumerator.Current;
					string text = array[(int)(chestBoxKey + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<ChestBoxType, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(ChestBoxType)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							ChestBoxType chestBoxType = (ChestBoxType)enumerator2.Current;
							dictionary.Add(chestBoxType, text.Split(',')[(int)chestBoxType]);
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
					dDataObj.Add(chestBoxKey, dictionary);
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
