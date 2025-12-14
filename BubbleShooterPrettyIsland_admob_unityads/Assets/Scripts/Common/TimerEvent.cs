
using System;
using UnityEngine;

public class TimerEvent
{
	public string m_timerName = string.Empty;

	public int m_repeatCount;

	public int m_currentRepeat;

	public bool m_isIgnoreTimeScale;

	public TimerCallBack m_callBack;

	public object[] m_objs;

	public float m_timerSpace;

	public float m_currentTimer;

	public bool m_isDone;

	public bool m_isStop;

	public void Update()
	{
		if (m_isIgnoreTimeScale)
		{
			m_currentTimer += Time.unscaledDeltaTime;
		}
		else
		{
			m_currentTimer += Time.deltaTime;
		}
		if (m_currentTimer >= m_timerSpace)
		{
			m_isDone = true;
		}
	}

	public void CompleteTimer()
	{
		CallBackTimer();
		if (m_repeatCount > 0)
		{
			m_currentRepeat++;
		}
		if (m_currentRepeat != m_repeatCount)
		{
			m_isDone = false;
			m_currentTimer = 0f;
		}
	}

	public void CallBackTimer()
	{
		if (this == Timer.test)
		{
			UnityEngine.Debug.Log("CallBackTimer " + (m_callBack == null));
		}
		if (m_callBack != null)
		{
			try
			{
				m_callBack(m_objs);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex.ToString());
			}
		}
	}

	public void ResetTimer()
	{
		m_currentTimer = 0f;
		m_currentRepeat = 0;
	}

	public void OnInit()
	{
	}

	public void OnPop()
	{
		m_timerName = string.Empty;
		m_repeatCount = 0;
		m_currentRepeat = 0;
		m_isIgnoreTimeScale = false;
		m_callBack = null;
		m_objs = null;
		m_timerSpace = 0f;
		m_currentTimer = 0f;
		m_isDone = false;
		m_isStop = false;
	}

	public void OnPush()
	{
		m_isStop = true;
	}
}
