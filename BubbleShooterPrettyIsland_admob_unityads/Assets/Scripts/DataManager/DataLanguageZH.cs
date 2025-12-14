
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataLanguageZH : Singleton<DataLanguageZH>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<LanguageZHKey, Dictionary<LanguageZHType, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<LanguageZHType, string> GetContentByKey(LanguageZHKey _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(LanguageZHKey _key, LanguageZHType _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<LanguageZHType, string> GetContentByKey(string _key)
	{
		return dDataObj[(LanguageZHKey)Enum.Parse(typeof(LanguageZHKey), _key)];
	}

	public string GetContentByKeyAndType(string _key, LanguageZHType _type)
	{
		return dDataObj[(LanguageZHKey)Enum.Parse(typeof(LanguageZHKey), _key)][_type];
	}

	public string[] GetContentArr(LanguageZHKey _key, LanguageZHType _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<LanguageZHKey, Dictionary<LanguageZHType, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/LanguageZH", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<LanguageZHType, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(LanguageZHKey)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					LanguageZHKey languageZHKey = (LanguageZHKey)enumerator.Current;
					string text = array[(int)(languageZHKey + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<LanguageZHType, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(LanguageZHType)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							LanguageZHType languageZHType = (LanguageZHType)enumerator2.Current;
							dictionary.Add(languageZHType, text.Split(',')[(int)languageZHType]);
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
					dDataObj.Add(languageZHKey, dictionary);
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
