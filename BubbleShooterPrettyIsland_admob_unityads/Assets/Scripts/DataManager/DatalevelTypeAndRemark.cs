
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DatalevelTypeAndRemark : Singleton<DatalevelTypeAndRemark>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<levelTypeAndRemarkKey, Dictionary<levelTypeAndRemarkType, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<levelTypeAndRemarkType, string> GetContentByKey(levelTypeAndRemarkKey _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(levelTypeAndRemarkKey _key, levelTypeAndRemarkType _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<levelTypeAndRemarkType, string> GetContentByKey(string _key)
	{
		return dDataObj[(levelTypeAndRemarkKey)Enum.Parse(typeof(levelTypeAndRemarkKey), _key)];
	}

	public string GetContentByKeyAndType(string _key, levelTypeAndRemarkType _type)
	{
		return dDataObj[(levelTypeAndRemarkKey)Enum.Parse(typeof(levelTypeAndRemarkKey), _key)][_type];
	}

	public string[] GetContentArr(levelTypeAndRemarkKey _key, levelTypeAndRemarkType _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<levelTypeAndRemarkKey, Dictionary<levelTypeAndRemarkType, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/levelTypeAndRemark", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<levelTypeAndRemarkType, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(levelTypeAndRemarkKey)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					levelTypeAndRemarkKey levelTypeAndRemarkKey = (levelTypeAndRemarkKey)enumerator.Current;
					string text = array[(int)(levelTypeAndRemarkKey + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<levelTypeAndRemarkType, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(levelTypeAndRemarkType)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							levelTypeAndRemarkType levelTypeAndRemarkType = (levelTypeAndRemarkType)enumerator2.Current;
							dictionary.Add(levelTypeAndRemarkType, text.Split(',')[(int)levelTypeAndRemarkType]);
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
					dDataObj.Add(levelTypeAndRemarkKey, dictionary);
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
