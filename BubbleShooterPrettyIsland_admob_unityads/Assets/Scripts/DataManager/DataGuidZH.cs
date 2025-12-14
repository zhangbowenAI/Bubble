
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataGuidZH : Singleton<DataGuidZH>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<GuidZHKey, Dictionary<GuidZHType, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<GuidZHType, string> GetContentByKey(GuidZHKey _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(GuidZHKey _key, GuidZHType _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<GuidZHType, string> GetContentByKey(string _key)
	{
		return dDataObj[(GuidZHKey)Enum.Parse(typeof(GuidZHKey), _key)];
	}

	public string GetContentByKeyAndType(string _key, GuidZHType _type)
	{
		return dDataObj[(GuidZHKey)Enum.Parse(typeof(GuidZHKey), _key)][_type];
	}

	public string[] GetContentArr(GuidZHKey _key, GuidZHType _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<GuidZHKey, Dictionary<GuidZHType, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/GuidZH", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<GuidZHType, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(GuidZHKey)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GuidZHKey guidZHKey = (GuidZHKey)enumerator.Current;
					string text = array[(int)(guidZHKey + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<GuidZHType, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(GuidZHType)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							GuidZHType guidZHType = (GuidZHType)enumerator2.Current;
							dictionary.Add(guidZHType, text.Split(',')[(int)guidZHType]);
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
					dDataObj.Add(guidZHKey, dictionary);
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
