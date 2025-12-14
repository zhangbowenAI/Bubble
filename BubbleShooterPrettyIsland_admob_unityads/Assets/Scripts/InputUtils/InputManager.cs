
using System;
using System.Collections.Generic;

public class InputManager
{
	private static Dictionary<string, IInputDispatcher> s_dispatcher = new Dictionary<string, IInputDispatcher>();

	private static InputEventCallBack s_OnEventDispatch;

	private static string m_DispatcherName;

	private static IInputDispatcher dispatcher;

	public static InputEventCallBack OnEveryEventDispatch
	{
		get
		{
			return s_OnEventDispatch;
		}
		set
		{
			s_OnEventDispatch = value;
		}
	}

	public static void Init()
	{
		InputOperationEventProxy.Init();
	}

	public static InputDispatcher<T> LoadDispatcher<T>() where T : IInputEventBase
	{
		string key = typeof(T).ToString();
		if (s_dispatcher.ContainsKey(key))
		{
			return (InputDispatcher<T>)s_dispatcher[key];
		}
		InputDispatcher<T> inputDispatcher = new InputDispatcher<T>();
		inputDispatcher.m_OnAllEventDispatch = s_OnEventDispatch;
		s_dispatcher.Add(key, inputDispatcher);
		return inputDispatcher;
	}

	public static IInputDispatcher LoadDispatcher(string DispatcherName)
	{
		if (s_dispatcher.ContainsKey(DispatcherName))
		{
			throw new Exception(DispatcherName + " Dispatcher has exist!");
		}
		Type type = Type.GetType(DispatcherName);
		if (type.IsSubclassOf(typeof(IInputDispatcher)))
		{
			throw new Exception(DispatcherName + " is not IInputDispatcher subclass!");
		}
		Type typeFromHandle = typeof(InputDispatcher<>);
		Type type2 = typeFromHandle.MakeGenericType(type);
		IInputDispatcher inputDispatcher = (IInputDispatcher)Activator.CreateInstance(type2);
		s_dispatcher.Add(DispatcherName, inputDispatcher);
		inputDispatcher.m_OnAllEventDispatch = s_OnEventDispatch;
		return inputDispatcher;
	}

	public static void UnLoadDispatcher<T>() where T : IInputEventBase
	{
		string key = typeof(T).ToString();
		if (s_dispatcher.ContainsKey(key))
		{
			s_dispatcher.Remove(key);
		}
	}

	public static IInputDispatcher GetDispatcher(string DispatcherName)
	{
		if (s_dispatcher.TryGetValue(DispatcherName, out dispatcher))
		{
			return dispatcher;
		}
		return LoadDispatcher(DispatcherName);
	}

	public static InputDispatcher<T> GetDispatcher<T>() where T : IInputEventBase
	{
		m_DispatcherName = typeof(T).Name;
		if (s_dispatcher.TryGetValue(m_DispatcherName, out dispatcher))
		{
			return (InputDispatcher<T>)dispatcher;
		}
		return LoadDispatcher<T>();
	}

	public static void RemoveDispatcher(string DispatcherName)
	{
		if (s_dispatcher.ContainsKey(DispatcherName))
		{
			s_dispatcher.Remove(DispatcherName);
		}
	}

	public static void RemoveDispatcher<T>() where T : IInputEventBase
	{
		string key = typeof(T).ToString();
		if (s_dispatcher.ContainsKey(key))
		{
			s_dispatcher.Remove(key);
		}
	}

	public static void Dispatch<T>(T inputEvent) where T : IInputEventBase
	{
		InputDispatcher<T> inputDispatcher = GetDispatcher<T>();
		inputDispatcher.Dispatch(inputEvent);
	}

	public static void Dispatch(string eventName, IInputEventBase inputEvent)
	{
		IInputDispatcher inputDispatcher = GetDispatcher(eventName);
		inputDispatcher.Dispatch(inputEvent);
	}

	public static void AddAllEventListener(string eventName, InputEventCallBack callback)
	{
		IInputDispatcher inputDispatcher = GetDispatcher(eventName);
		IInputDispatcher inputDispatcher2 = inputDispatcher;
		inputDispatcher2.m_OnAllEventDispatch = (InputEventCallBack)Delegate.Combine(inputDispatcher2.m_OnAllEventDispatch, callback);
	}

	public static void AddAllEventListener<T>(InputEventHandle<T> callback) where T : IInputEventBase
	{
		InputDispatcher<T> inputDispatcher = GetDispatcher<T>();
		InputDispatcher<T> inputDispatcher2 = inputDispatcher;
		inputDispatcher2.OnEventDispatch = (InputEventHandle<T>)Delegate.Combine(inputDispatcher2.OnEventDispatch, callback);
	}

	public static void AddListener(string eventName, string eventKey, InputEventHandle<IInputEventBase> callback)
	{
		IInputDispatcher inputDispatcher = GetDispatcher(eventName);
		inputDispatcher.AddListener(eventKey, callback);
	}

	public static void AddListener<T>(string eventKey, InputEventHandle<T> callback) where T : IInputEventBase
	{
		InputDispatcher<T> inputDispatcher = GetDispatcher<T>();
		inputDispatcher.AddListener(eventKey, callback);
	}

	public static void AddListener<T>(InputEventHandle<T> callback) where T : IInputEventBase
	{
		InputDispatcher<T> inputDispatcher = GetDispatcher<T>();
		inputDispatcher.AddListener(typeof(T).Name, callback);
	}

	public static void RemoveAllEventListener<T>(InputEventHandle<T> callback) where T : IInputEventBase
	{
		InputDispatcher<T> inputDispatcher = GetDispatcher<T>();
		InputDispatcher<T> inputDispatcher2 = inputDispatcher;
		inputDispatcher2.OnEventDispatch = (InputEventHandle<T>)Delegate.Remove(inputDispatcher2.OnEventDispatch, callback);
	}

	public static void RemoveAllEventListener(string eventName, InputEventCallBack callback)
	{
		IInputDispatcher inputDispatcher = GetDispatcher(eventName);
		IInputDispatcher inputDispatcher2 = inputDispatcher;
		inputDispatcher2.m_OnAllEventDispatch = (InputEventCallBack)Delegate.Remove(inputDispatcher2.m_OnAllEventDispatch, callback);
	}

	public static void RemoveListener(string eventName, string eventKey, InputEventHandle<IInputEventBase> callback)
	{
		IInputDispatcher inputDispatcher = GetDispatcher(eventName);
		inputDispatcher.RemoveListener(eventKey, callback);
	}

	public static void RemoveListener<T>(string eventKey, InputEventHandle<T> callback) where T : IInputEventBase
	{
		InputDispatcher<T> inputDispatcher = GetDispatcher<T>();
		inputDispatcher.RemoveListener(eventKey, callback);
	}

	public static void RemoveListener<T>(InputEventHandle<T> callback) where T : IInputEventBase
	{
		InputDispatcher<T> inputDispatcher = GetDispatcher<T>();
		inputDispatcher.RemoveListener(typeof(T).Name, callback);
	}
}
