
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataLanguageEN : Singleton<DataLanguageEN>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<LanguageENKey, Dictionary<LanguageENType, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<LanguageENType, string> GetContentByKey(LanguageENKey _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(LanguageENKey _key, LanguageENType _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<LanguageENType, string> GetContentByKey(string _key)
	{
		return dDataObj[(LanguageENKey)Enum.Parse(typeof(LanguageENKey), _key)];
	}

	public string GetContentByKeyAndType(string _key, LanguageENType _type)
	{
		return dDataObj[(LanguageENKey)Enum.Parse(typeof(LanguageENKey), _key)][_type];
	}

	public string[] GetContentArr(LanguageENKey _key, LanguageENType _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<LanguageENKey, Dictionary<LanguageENType, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/LanguageEN", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<LanguageENType, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(LanguageENKey)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					LanguageENKey languageENKey = (LanguageENKey)enumerator.Current;
					string text = array[(int)(languageENKey + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<LanguageENType, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(LanguageENType)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							LanguageENType languageENType = (LanguageENType)enumerator2.Current;
							dictionary.Add(languageENType, text.Split(',')[(int)languageENType]);
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
					dDataObj.Add(languageENKey, dictionary);
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
