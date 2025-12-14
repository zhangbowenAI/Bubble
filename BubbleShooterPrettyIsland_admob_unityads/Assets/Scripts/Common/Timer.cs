
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Timer
{
	public static List<TimerEvent> m_timers = new List<TimerEvent>();

	public static TimerEvent test;

	public static void Init()
	{
		ApplicationManager.s_OnApplicationUpdate = (ApplicationVoidCallback)Delegate.Combine(ApplicationManager.s_OnApplicationUpdate, new ApplicationVoidCallback(Update));
	}

	private static void Update()
	{
		for (int i = 0; i < m_timers.Count; i++)
		{
			TimerEvent timerEvent = m_timers[i];
			timerEvent.Update();
			if (timerEvent.m_isDone)
			{
				timerEvent.CompleteTimer();
				if (timerEvent.m_repeatCount == 0)
				{
					m_timers.Remove(timerEvent);
				}
			}
		}
		if (test != null)
		{
			UnityEngine.Debug.Log("Test " + test.m_timerName + " " + test.m_currentTimer + " " + m_timers.Contains(test) + " isDone " + test.m_isDone);
		}
	}

	public static bool GetIsExistTimer(string timerName)
	{
		for (int i = 0; i < m_timers.Count; i++)
		{
			TimerEvent timerEvent = m_timers[i];
			if (timerEvent.m_timerName == timerName)
			{
				return true;
			}
		}
		return false;
	}

	public static TimerEvent GetTimer(string timerName)
	{
		for (int i = 0; i < m_timers.Count; i++)
		{
			TimerEvent timerEvent = m_timers[i];
			if (timerEvent.m_timerName == timerName)
			{
				return timerEvent;
			}
		}
		throw new Exception("Get Timer  Exception not find ->" + timerName + "<-");
	}

	public static TimerEvent DelayCallBack(float delayTime, TimerCallBack callBack, params object[] objs)
	{
		return AddTimer(delayTime, isIgnoreTimeScale: false, 0, null, callBack, objs);
	}

	public static TimerEvent DelayCallBack(float delayTime, bool isIgnoreTimeScale, TimerCallBack callBack, params object[] objs)
	{
		return AddTimer(delayTime, isIgnoreTimeScale, 0, null, callBack, objs);
	}

	public static TimerEvent CallBackOfIntervalTimer(float spaceTime, TimerCallBack callBack, params object[] objs)
	{
		return AddTimer(spaceTime, isIgnoreTimeScale: false, -1, null, callBack, objs);
	}

	public static TimerEvent CallBackOfIntervalTimer(float spaceTime, bool isIgnoreTimeScale, TimerCallBack callBack, params object[] objs)
	{
		return AddTimer(spaceTime, isIgnoreTimeScale, -1, null, callBack, objs);
	}

	public static TimerEvent CallBackOfIntervalTimer(float spaceTime, bool isIgnoreTimeScale, string timerName, TimerCallBack callBack, params object[] objs)
	{
		return AddTimer(spaceTime, isIgnoreTimeScale, -1, timerName, callBack, objs);
	}

	public static TimerEvent CallBackOfIntervalTimer(float spaceTime, int callBackCount, TimerCallBack callBack, params object[] objs)
	{
		return AddTimer(spaceTime, isIgnoreTimeScale: false, -1, null, callBack, objs);
	}

	public static TimerEvent CallBackOfIntervalTimer(float spaceTime, bool isIgnoreTimeScale, int callBackCount, TimerCallBack callBack, params object[] objs)
	{
		return AddTimer(spaceTime, isIgnoreTimeScale, -1, null, callBack, objs);
	}

	public static TimerEvent CallBackOfIntervalTimer(float spaceTime, bool isIgnoreTimeScale, int callBackCount, string timerName, TimerCallBack callBack, params object[] objs)
	{
		return AddTimer(spaceTime, isIgnoreTimeScale, -1, timerName, callBack, objs);
	}

	public static TimerEvent AddTimer(float spaceTime, bool isIgnoreTimeScale, int callBackCount, string timerName, TimerCallBack callBack, params object[] objs)
	{
		TimerEvent timerEvent = new TimerEvent();
		timerEvent.m_timerName = (timerName ?? timerEvent.GetHashCode().ToString());
		timerEvent.m_currentTimer = 0f;
		timerEvent.m_timerSpace = spaceTime;
		timerEvent.m_callBack = callBack;
		timerEvent.m_objs = objs;
		timerEvent.m_isIgnoreTimeScale = isIgnoreTimeScale;
		timerEvent.m_repeatCount = callBackCount;
		m_timers.Add(timerEvent);
		return timerEvent;
	}

	public static void DestroyTimer(TimerEvent timer, bool isCallBack = false)
	{
		if (m_timers.Contains(timer))
		{
			if (isCallBack)
			{
				timer.CallBackTimer();
			}
			m_timers.Remove(timer);
		}
		else
		{
			UnityEngine.Debug.LogError("Timer DestroyTimer error: dont exist timer " + timer);
		}
	}

	public static void DestroyTimer(string timerName, bool isCallBack = false)
	{
		for (int i = 0; i < m_timers.Count; i++)
		{
			TimerEvent timerEvent = m_timers[i];
			if (timerEvent.m_timerName.Equals(timerName))
			{
				DestroyTimer(timerEvent, isCallBack);
			}
		}
	}

	public static void DestroyAllTimer(bool isCallBack = false)
	{
		for (int i = 0; i < m_timers.Count; i++)
		{
			if (isCallBack)
			{
				m_timers[i].CallBackTimer();
			}
		}
		m_timers.Clear();
	}

	public static void ResetTimer(TimerEvent timer)
	{
		if (m_timers.Contains(timer))
		{
			timer.ResetTimer();
		}
		else
		{
			UnityEngine.Debug.LogError("Timer ResetTimer error: dont exist timer " + timer);
		}
	}

	public static void ResetTimer(string timerName)
	{
		for (int i = 0; i < m_timers.Count; i++)
		{
			TimerEvent timerEvent = m_timers[i];
			if (timerEvent.m_timerName.Equals(timerName))
			{
				ResetTimer(timerEvent);
			}
		}
	}
}
