
using System;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
	public static string SceneName = "MainScene";

	private static ApplicationManager instance;

	public static AppMode appMode;

	public static ApplicationVoidCallback s_OnApplicationUpdate;

	public static ApplicationManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.FindObjectOfType<ApplicationManager>();
			}
			return instance;
		}
		set
		{
			instance = value;
		}
	}

	public static void Init()
	{
		GameObject gameObject = new GameObject("[ApplicationManager]");
		ApplicationManager applicationManager = gameObject.AddComponent<ApplicationManager>();
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		AppLaunch();
	}

	private static void AppLaunch()
	{
		MemoryManager.Init();
		Timer.Init();
		
	}

	public void Update()
	{
		if (s_OnApplicationUpdate != null)
		{
			s_OnApplicationUpdate();
		}
	}

}
