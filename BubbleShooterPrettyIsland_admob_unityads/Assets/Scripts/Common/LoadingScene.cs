
using System;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
	public static LoadingScene Instance;

	public RectTransform img;

	public void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		UIManager.CloseAllUI();
		GlobalEvent.RemoveAllEvent();
		GC.Collect();
		SingletonMonoBehaviour<TransitionManager>.Instance.LoadSceneOut(ApplicationManager.SceneName);
	}

	private void Update()
	{
	}
}
