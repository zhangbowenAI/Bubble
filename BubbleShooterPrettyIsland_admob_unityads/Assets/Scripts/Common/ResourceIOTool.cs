
using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

public class ResourceIOTool : MonoBehaviour
{
	private static ResourceIOTool instance;

	private LoadState m_loadState = new LoadState();

	public static ResourceIOTool GetInstance()
	{
		if (instance == null)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "ResourceIO";
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			instance = gameObject.AddComponent<ResourceIOTool>();
		}
		return instance;
	}

	public static string ReadStringByFile(string path)
	{
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			if (!File.Exists(path))
			{
				UnityEngine.Debug.Log("path dont exists ! : " + path);
				return string.Empty;
			}
			StreamReader streamReader = File.OpenText(path);
			stringBuilder.Append(streamReader.ReadToEnd());
			streamReader.Close();
			streamReader.Dispose();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("Load text fail ! message:" + ex.Message);
		}
		return stringBuilder.ToString();
	}

	public static void ResourceLoadAsync(string path, LoadCallBack callback)
	{
		GetInstance().MonoLoadMethod(path, callback);
	}

	public void MonoLoadMethod(string path, LoadCallBack callback)
	{
		StartCoroutine(MonoLoadByResourcesAsync(path, callback));
	}

	public IEnumerator MonoLoadByResourcesAsync(string path, LoadCallBack callback)
	{
		ResourceRequest status = Resources.LoadAsync(path);
		while (!status.isDone)
		{
			m_loadState.UpdateProgress(status);
			callback(m_loadState, null);
			yield return 0;
		}
		m_loadState.UpdateProgress(status);
		callback(m_loadState, status.asset);
	}

	public static void AssetsBundleLoadAsync(string path, AssetBundleLoadCallBack callback)
	{
		GetInstance().MonoLoadAssetsBundleMethod(path, callback);
	}

	public void MonoLoadAssetsBundleMethod(string path, AssetBundleLoadCallBack callback)
	{
		StartCoroutine(MonoLoadByAssetsBundleAsync(path, callback));
	}

	public IEnumerator MonoLoadByAssetsBundleAsync(string path, AssetBundleLoadCallBack callback)
	{
		AssetBundleCreateRequest status = AssetBundle.LoadFromFileAsync(path);
		LoadState loadState = new LoadState();
		while (!status.isDone)
		{
			loadState.UpdateProgress(status);
			callback(loadState, null);
			yield return 0;
		}
		if (status.assetBundle != null)
		{
			status.assetBundle.name = path;
		}
		loadState.UpdateProgress(status);
		callback(loadState, status.assetBundle);
	}

	public static void WriteStringByFile(string path, string content)
	{
		byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(content);
		CreateFile(path, bytes);
	}

	public static void WriteTexture2DByFile(string path, Texture2D texture)
	{
		byte[] byt = texture.EncodeToPNG();
		CreateFile(path, byt);
	}

	public static void DeleteFile(string path)
	{
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		else
		{
			UnityEngine.Debug.Log("File:[" + path + "] dont exists");
		}
	}

	public static void CreateFile(string path, byte[] byt)
	{
		// Debug.LogError(path);
		try
		{
			FileTool.CreatFilePath(path);
			File.WriteAllBytes(path, byt);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("File Create Fail! \n" + ex.Message);
		}
	}
}
