
using System;
using UnityEngine;

public static class ResourceManager
{
	public static ResLoadLocation m_gameLoadType;

	public static ResLoadLocation GetLoadType(ResLoadLocation loadType)
	{
		if (m_gameLoadType == ResLoadLocation.Resource)
		{
			return ResLoadLocation.Resource;
		}
		return loadType;
	}

	public static string ReadTextFile(string textName)
	{
		TextAsset textAsset = Load<TextAsset>(textName);
		if (textAsset == null)
		{
			throw new Exception("ReadTextFile not find " + textName);
		}
		return textAsset.text;
	}

	public static object Load(string path)
	{
		return Resources.Load(path);
	}

	public static T Load<T>(string path) where T : UnityEngine.Object
	{
		return Resources.Load<T>(path);
	}

	public static void LoadAsync(string path, LoadCallBack callBack)
	{
		ResourceIOTool.ResourceLoadAsync(path, callBack);
	}

	public static void UnLoad(string name)
	{
	}
}
