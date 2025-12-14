
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataSkillPrice : Singleton<DataSkillPrice>
{
	public const string loadPath = "Data/GameData/";

	public Dictionary<SkillPriceKey, Dictionary<SkillPriceType, string>> dDataObj;

	public override void Init()
	{
		if (dDataObj == null)
		{
			InitDate();
		}
	}

	public Dictionary<SkillPriceType, string> GetContentByKey(SkillPriceKey _key)
	{
		return dDataObj[_key];
	}

	public string GetContentByKeyAndType(SkillPriceKey _key, SkillPriceType _type)
	{
		return dDataObj[_key][_type];
	}

	public Dictionary<SkillPriceType, string> GetContentByKey(string _key)
	{
		return dDataObj[(SkillPriceKey)Enum.Parse(typeof(SkillPriceKey), _key)];
	}

	public string GetContentByKeyAndType(string _key, SkillPriceType _type)
	{
		return dDataObj[(SkillPriceKey)Enum.Parse(typeof(SkillPriceKey), _key)][_type];
	}

	public string[] GetContentArr(SkillPriceKey _key, SkillPriceType _type, char str)
	{
		return dDataObj[_key][_type].Split(str);
	}

	private void InitDate()
	{
		if (dDataObj == null)
		{
			dDataObj = new Dictionary<SkillPriceKey, Dictionary<SkillPriceType, string>>();
			string empty = string.Empty;

			TextAsset textAsset = (TextAsset)Resources.Load("Data/GameData/SkillPrice", typeof(TextAsset));
			empty = textAsset.ToString();

			string[] array = empty.Split('\n');
			Dictionary<SkillPriceType, string> dictionary = null;
			IEnumerator enumerator = Enum.GetValues(typeof(SkillPriceKey)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					SkillPriceKey skillPriceKey = (SkillPriceKey)enumerator.Current;
					string text = array[(int)(skillPriceKey + 2)];
					int num = text.Split(',').Length;
					dictionary = new Dictionary<SkillPriceType, string>();
					IEnumerator enumerator2 = Enum.GetValues(typeof(SkillPriceType)).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							SkillPriceType skillPriceType = (SkillPriceType)enumerator2.Current;
							dictionary.Add(skillPriceType, text.Split(',')[(int)skillPriceType]);
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
					dDataObj.Add(skillPriceKey, dictionary);
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
