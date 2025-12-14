
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataPayGold : Singleton<DataPayGold>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<PayGoldKey, Dictionary<PayGoldType, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<PayGoldType, string> GetContentByKey(PayGoldKey _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(PayGoldKey _key, PayGoldType _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<PayGoldType, string> GetContentByKey(string _key)
	{
		return dDataObj[(PayGoldKey)Enum.Parse(typeof(PayGoldKey), _key)];
	}

	public string GetContentByKeyAndType(string _key, PayGoldType _type)
	{
		return dDataObj[(PayGoldKey)Enum.Parse(typeof(PayGoldKey), _key)][_type];
	}

	public string[] GetContentArr(PayGoldKey _key, PayGoldType _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<PayGoldKey, Dictionary<PayGoldType, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/PayGold", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<PayGoldType, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(PayGoldKey)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					PayGoldKey payGoldKey = (PayGoldKey)enumerator.Current;
					string text = array[(int)(payGoldKey + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<PayGoldType, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(PayGoldType)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							PayGoldType payGoldType = (PayGoldType)enumerator2.Current;
							dictionary.Add(payGoldType, text.Split(',')[(int)payGoldType]);
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
					dDataObj.Add(payGoldKey, dictionary);
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
