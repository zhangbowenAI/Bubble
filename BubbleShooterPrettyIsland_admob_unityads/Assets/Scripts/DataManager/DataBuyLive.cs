
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataBuyLive : Singleton<DataBuyLive>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<BuyLiveKey, Dictionary<BuyLiveType, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<BuyLiveType, string> GetContentByKey(BuyLiveKey _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(BuyLiveKey _key, BuyLiveType _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<BuyLiveType, string> GetContentByKey(string _key)
	{
		return dDataObj[(BuyLiveKey)Enum.Parse(typeof(BuyLiveKey), _key)];
	}

	public string GetContentByKeyAndType(string _key, BuyLiveType _type)
	{
		return dDataObj[(BuyLiveKey)Enum.Parse(typeof(BuyLiveKey), _key)][_type];
	}

	public string[] GetContentArr(BuyLiveKey _key, BuyLiveType _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<BuyLiveKey, Dictionary<BuyLiveType, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/BuyLive", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<BuyLiveType, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(BuyLiveKey)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					BuyLiveKey buyLiveKey = (BuyLiveKey)enumerator.Current;
					string text = array[(int)(buyLiveKey + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<BuyLiveType, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(BuyLiveType)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							BuyLiveType buyLiveType = (BuyLiveType)enumerator2.Current;
							dictionary.Add(buyLiveType, text.Split(',')[(int)buyLiveType]);
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
					dDataObj.Add(buyLiveKey, dictionary);
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
