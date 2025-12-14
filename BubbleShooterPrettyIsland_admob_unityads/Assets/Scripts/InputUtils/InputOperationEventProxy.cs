
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InputOperationEventProxy : IInputProxyBase
{
	private static List<IInputOperationEventCreater> s_creates = new List<IInputOperationEventCreater>();

	
	public static void Init()
	{
		ApplicationManager.s_OnApplicationUpdate = (ApplicationVoidCallback)Delegate.Combine(ApplicationManager.s_OnApplicationUpdate, new ApplicationVoidCallback(Update));
	}

	public static IInputOperationEventCreater LoadEventCreater<T>() where T : IInputOperationEventCreater, new()
	{
		for (int i = 0; i < s_creates.Count; i++)
		{
			if (s_creates[i] is T)
			{
				throw new Exception(typeof(T).Name + " Creater has Exits!");
			}
		}
		IInputOperationEventCreater inputOperationEventCreater = new T();
		s_creates.Add(inputOperationEventCreater);
		return inputOperationEventCreater;
	}

	public static void UnLoadEventCreater<T>() where T : IInputOperationEventCreater, new()
	{
		for (int i = 0; i < s_creates.Count; i++)
		{
			if (s_creates[i] is T)
			{
				s_creates.RemoveAt(i);
			}
		}
	}

	public static void Update()
	{
		if (IInputProxyBase.IsActive)
		{
			for (int i = 0; i < s_creates.Count; i++)
			{
				try
				{
					s_creates[i].EventTriggerLogic();
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError(ex.ToString());
				}
			}
		}
	}

	public static void DispatchInputOperationEvent(IInputOperationEventBase inputOperationEventBase, string eventName)
	{
		if (IInputProxyBase.IsActive)
		{
			InputManager.Dispatch(eventName, inputOperationEventBase);
		}
	}
}
