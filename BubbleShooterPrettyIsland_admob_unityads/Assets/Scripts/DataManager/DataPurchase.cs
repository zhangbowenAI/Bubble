
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataPurchase : Singleton<DataPurchase>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<PurchaseKey, Dictionary<PurchaseType, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<PurchaseType, string> GetContentByKey(PurchaseKey _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(PurchaseKey _key, PurchaseType _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<PurchaseType, string> GetContentByKey(string _key)
	{
		return dDataObj[(PurchaseKey)Enum.Parse(typeof(PurchaseKey), _key)];
	}

	public string GetContentByKeyAndType(string _key, PurchaseType _type)
	{
		return dDataObj[(PurchaseKey)Enum.Parse(typeof(PurchaseKey), _key)][_type];
	}

	public string[] GetContentArr(PurchaseKey _key, PurchaseType _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<PurchaseKey, Dictionary<PurchaseType, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/Purchase", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<PurchaseType, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(PurchaseKey)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					PurchaseKey purchaseKey = (PurchaseKey)enumerator.Current;
					string text = array[(int)(purchaseKey + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<PurchaseType, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(PurchaseType)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							PurchaseType purchaseType = (PurchaseType)enumerator2.Current;
							dictionary.Add(purchaseType, text.Split(',')[(int)purchaseType]);
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
					dDataObj.Add(purchaseKey, dictionary);
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
