
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataSignmap7 : Singleton<DataSignmap7>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<Signmap7Key, Dictionary<Signmap7Type, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<Signmap7Type, string> GetContentByKey(Signmap7Key _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(Signmap7Key _key, Signmap7Type _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<Signmap7Type, string> GetContentByKey(string _key)
	{
		return dDataObj[(Signmap7Key)Enum.Parse(typeof(Signmap7Key), _key)];
	}

	public string GetContentByKeyAndType(string _key, Signmap7Type _type)
	{
		return dDataObj[(Signmap7Key)Enum.Parse(typeof(Signmap7Key), _key)][_type];
	}

	public string[] GetContentArr(Signmap7Key _key, Signmap7Type _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<Signmap7Key, Dictionary<Signmap7Type, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/Signmap7", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<Signmap7Type, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(Signmap7Key)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Signmap7Key signmap7Key = (Signmap7Key)enumerator.Current;
					string text = array[(int)(signmap7Key + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<Signmap7Type, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(Signmap7Type)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							Signmap7Type signmap7Type = (Signmap7Type)enumerator2.Current;
							dictionary.Add(signmap7Type, text.Split(',')[(int)signmap7Type]);
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
					dDataObj.Add(signmap7Key, dictionary);
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
