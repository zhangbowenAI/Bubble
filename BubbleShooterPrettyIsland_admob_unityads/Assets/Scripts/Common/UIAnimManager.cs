
using System;
using UnityEngine;

public class UIAnimManager : MonoBehaviour
{
	public void StartEnterAnim(UIWindowBase UIbase, UICallBack callBack, params object[] objs)
	{
		UISystemEvent.Dispatch(UIbase, UIEvent.OnStartEnterAnim);
		UIbase.Show();
		StartCoroutine(UIbase.EnterAnim(EndEnterAnim, callBack, objs));
	}

	public void EndEnterAnim(UIWindowBase UIbase, UICallBack callBack, params object[] objs)
	{
		UISystemEvent.Dispatch(UIbase, UIEvent.OnCompleteEnterAnim);
		UIbase.OnCompleteEnterAnim();
		UIbase.windowStatus = UIWindowBase.WindowStatus.Open;
		try
		{
			callBack?.Invoke(UIbase, objs);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError(ex.ToString());
		}
	}

	public void StartExitAnim(UIWindowBase UIbase, UICallBack callBack, params object[] objs)
	{
		UISystemEvent.Dispatch(UIbase, UIEvent.OnStartExitAnim);
		StartCoroutine(UIbase.ExitAnim(EndExitAnim, callBack, objs));
	}

	public void EndExitAnim(UIWindowBase UIbase, UICallBack callBack, params object[] objs)
	{
		UISystemEvent.Dispatch(UIbase, UIEvent.OnCompleteExitAnim);
		UIbase.OnCompleteExitAnim();
		UIbase.windowStatus = UIWindowBase.WindowStatus.Close;
		try
		{
			callBack?.Invoke(UIbase, objs);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError(ex.ToString());
		}
	}

	public void StartPauseAnim(UIWindowBase UIbase, UICallBack callBack, params object[] objs)
	{
		UISystemEvent.Dispatch(UIbase, UIEvent.OnStartPauseAnim);
		StartCoroutine(UIbase.PauserAnim(EndPauserAnim, callBack, objs));
	}

	public void EndPauserAnim(UIWindowBase UIbase, UICallBack callBack, params object[] objs)
	{
		UISystemEvent.Dispatch(UIbase, UIEvent.OnCompletePauseAnim);
		UIbase.OnCompletePauserAnim();
		UIbase.windowStatus = UIWindowBase.WindowStatus.Pause;
		try
		{
			callBack?.Invoke(UIbase, objs);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError(ex.ToString());
		}
	}
}
