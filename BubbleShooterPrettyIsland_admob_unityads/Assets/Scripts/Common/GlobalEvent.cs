
using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvent
{
	private abstract class EventDispatcher
	{
	}

	private class EventDispatcher<T> : EventDispatcher
	{
		public EventHandle<T> m_CallBack;

		public void Call(T e, params object[] args)
		{
			if (m_CallBack != null)
			{
				try
				{
					m_CallBack(e, args);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError(ex.ToString());
				}
			}
		}
	}

	private static Dictionary<Enum, EventHandle> mEventDic = new Dictionary<Enum, EventHandle>();

	private static Dictionary<Enum, List<EventHandle>> mUseOnceEventDic = new Dictionary<Enum, List<EventHandle>>();

	private static Dictionary<string, List<EventHandle>> m_stringEventDic = new Dictionary<string, List<EventHandle>>();

	private static Dictionary<string, List<EventHandle>> m_stringOnceEventDic = new Dictionary<string, List<EventHandle>>();

	private static Dictionary<Type, EventDispatcher> mTypeEventDic = new Dictionary<Type, EventDispatcher>();

	private static Dictionary<Type, EventDispatcher> mTypeUseOnceEventDic = new Dictionary<Type, EventDispatcher>();

	public static void AddEvent(Enum type, EventHandle handle, bool isUseOnce = false)
	{
		if (isUseOnce)
		{
			if (mUseOnceEventDic.ContainsKey(type))
			{
				if (!mUseOnceEventDic[type].Contains(handle))
				{
					mUseOnceEventDic[type].Add(handle);
				}
				else
				{
					UnityEngine.Debug.LogWarning("already existing EventType: " + type + " handle: " + handle);
				}
			}
			else
			{
				List<EventHandle> list = new List<EventHandle>();
				list.Add(handle);
				mUseOnceEventDic.Add(type, list);
			}
		}
		else if (mEventDic.ContainsKey(type))
		{
			Dictionary<Enum, EventHandle> dictionary;
			Enum key;
			(dictionary = mEventDic)[key = type] = (EventHandle)Delegate.Combine(dictionary[key], handle);
		}
		else
		{
			mEventDic.Add(type, handle);
		}
	}

	public static void RemoveEvent(Enum type, EventHandle handle)
	{
		if (mUseOnceEventDic.ContainsKey(type) && mUseOnceEventDic[type].Contains(handle))
		{
			mUseOnceEventDic[type].Remove(handle);
			if (mUseOnceEventDic[type].Count == 0)
			{
				mUseOnceEventDic.Remove(type);
			}
		}
		if (mEventDic.ContainsKey(type))
		{
			Dictionary<Enum, EventHandle> dictionary;
			Enum key;
			(dictionary = mEventDic)[key = type] = (EventHandle)Delegate.Remove(dictionary[key], handle);
		}
	}

	internal static void AddTypeEvent<T>()
	{
		throw new NotImplementedException();
	}

	public static void RemoveEvent(Enum type)
	{
		if (mUseOnceEventDic.ContainsKey(type))
		{
			mUseOnceEventDic.Remove(type);
		}
		if (mEventDic.ContainsKey(type))
		{
			mEventDic.Remove(type);
		}
	}

	public static void DispatchEvent(Enum type, params object[] args)
	{
		if (mEventDic.ContainsKey(type))
		{
			try
			{
				if (mEventDic[type] != null)
				{
					mEventDic[type](args);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex.ToString());
			}
		}
		if (!mUseOnceEventDic.ContainsKey(type))
		{
			return;
		}
		for (int i = 0; i < mUseOnceEventDic[type].Count; i++)
		{
			Delegate[] invocationList = mUseOnceEventDic[type][i].GetInvocationList();
			for (int j = 0; j < invocationList.Length; j++)
			{
				EventHandle eventHandle = (EventHandle)invocationList[j];
				try
				{
					eventHandle(args);
				}
				catch (Exception ex2)
				{
					UnityEngine.Debug.LogError(ex2.ToString());
				}
			}
		}
		RemoveEvent(type);
	}

	public static void AddEvent(string eventKey, EventHandle handle, bool isUseOnce = false)
	{
		if (isUseOnce)
		{
			if (m_stringOnceEventDic.ContainsKey(eventKey))
			{
				if (!m_stringOnceEventDic[eventKey].Contains(handle))
				{
					m_stringOnceEventDic[eventKey].Add(handle);
				}
				else
				{
					UnityEngine.Debug.LogWarning("already existing EventType: " + eventKey + " handle: " + handle);
				}
			}
			else
			{
				List<EventHandle> list = new List<EventHandle>();
				list.Add(handle);
				m_stringOnceEventDic.Add(eventKey, list);
			}
		}
		else if (m_stringEventDic.ContainsKey(eventKey))
		{
			if (!m_stringEventDic[eventKey].Contains(handle))
			{
				m_stringEventDic[eventKey].Add(handle);
			}
			else
			{
				UnityEngine.Debug.LogWarning("already existing EventType: " + eventKey + " handle: " + handle);
			}
		}
		else
		{
			List<EventHandle> list2 = new List<EventHandle>();
			list2.Add(handle);
			m_stringEventDic.Add(eventKey, list2);
		}
	}

	public static void RemoveEvent(string eventKey, EventHandle handle)
	{
		if (m_stringEventDic.ContainsKey(eventKey) && m_stringEventDic[eventKey].Contains(handle))
		{
			m_stringEventDic[eventKey].Remove(handle);
		}
		if (m_stringOnceEventDic.ContainsKey(eventKey) && m_stringOnceEventDic[eventKey].Contains(handle))
		{
			m_stringOnceEventDic[eventKey].Remove(handle);
		}
	}

	public static void RemoveEvent(string eventKey)
	{
		if (m_stringEventDic.ContainsKey(eventKey))
		{
			m_stringEventDic.Remove(eventKey);
		}
		if (m_stringOnceEventDic.ContainsKey(eventKey))
		{
			m_stringOnceEventDic.Remove(eventKey);
		}
	}

	public static void RemoveAllEvent()
	{
		mUseOnceEventDic.Clear();
		mEventDic.Clear();
		m_stringEventDic.Clear();
	}

	public static void DispatchEvent(string eventKey, params object[] args)
	{
		if (m_stringEventDic.ContainsKey(eventKey))
		{
			for (int i = 0; i < m_stringEventDic[eventKey].Count; i++)
			{
				Delegate[] invocationList = m_stringEventDic[eventKey][i].GetInvocationList();
				for (int j = 0; j < invocationList.Length; j++)
				{
					EventHandle eventHandle = (EventHandle)invocationList[j];
					try
					{
						eventHandle(args);
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogError(ex.ToString());
					}
				}
			}
		}
		if (!m_stringOnceEventDic.ContainsKey(eventKey))
		{
			return;
		}
		for (int k = 0; k < m_stringOnceEventDic[eventKey].Count; k++)
		{
			Delegate[] invocationList2 = m_stringOnceEventDic[eventKey][k].GetInvocationList();
			for (int l = 0; l < invocationList2.Length; l++)
			{
				EventHandle eventHandle2 = (EventHandle)invocationList2[l];
				try
				{
					eventHandle2(args);
				}
				catch (Exception ex2)
				{
					UnityEngine.Debug.LogError(ex2.ToString());
				}
			}
		}
		RemoveEvent(eventKey);
	}

	public static void AddTypeEvent<T>(EventHandle<T> handle, bool isUseOnce = false)
	{
		EventDispatcher<T> eventDispatcher = GetEventDispatcher<T>(isUseOnce);
		eventDispatcher.m_CallBack = (EventHandle<T>)Delegate.Combine(eventDispatcher.m_CallBack, handle);
	}

	public static void RemoveTypeEvent<T>(EventHandle<T> handle, bool isUseOnce = false)
	{
		EventDispatcher<T> eventDispatcher = GetEventDispatcher<T>(isUseOnce);
		eventDispatcher.m_CallBack = (EventHandle<T>)Delegate.Remove(eventDispatcher.m_CallBack, handle);
	}

	public static void RemoveTypeEvent<T>(bool isUseOnce = false)
	{
		GetEventDispatcher<T>(isUseOnce).m_CallBack = null;
	}

	public static void DispatchTypeEvent<T>(T e, params object[] args)
	{
		GetEventDispatcher<T>(isOnce: false).Call(e, args);
		GetEventDispatcher<T>(isOnce: true).Call(e, args);
		GetEventDispatcher<T>(isOnce: true).m_CallBack = null;
	}

	private static EventDispatcher<T> GetEventDispatcher<T>(bool isOnce)
	{
		Type typeFromHandle = typeof(T);
		if (isOnce)
		{
			if (mTypeUseOnceEventDic.ContainsKey(typeFromHandle))
			{
				return (EventDispatcher<T>)mTypeUseOnceEventDic[typeFromHandle];
			}
			EventDispatcher<T> eventDispatcher = new EventDispatcher<T>();
			mTypeUseOnceEventDic.Add(typeFromHandle, eventDispatcher);
			return eventDispatcher;
		}
		if (mTypeEventDic.ContainsKey(typeFromHandle))
		{
			return (EventDispatcher<T>)mTypeEventDic[typeFromHandle];
		}
		EventDispatcher<T> eventDispatcher2 = new EventDispatcher<T>();
		mTypeEventDic.Add(typeFromHandle, eventDispatcher2);
		return eventDispatcher2;
	}
}
