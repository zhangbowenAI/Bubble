
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputDispatcher<Event> : IInputDispatcher where Event : IInputEventBase
{
	protected Dictionary<string, InputEventHandle<Event>> m_Listeners = new Dictionary<string, InputEventHandle<Event>>();

	public InputEventHandle<Event> OnEventDispatch;

	private Dictionary<InputEventHandle<IInputEventBase>, InputEventHandle<Event>> m_ListenerHash = new Dictionary<InputEventHandle<IInputEventBase>, InputEventHandle<Event>>();

	private InputEventHandle<Event> m_handle;

	private string m_eventKey;

	public override void AddListener(string eventKey, InputEventHandle<IInputEventBase> callBack)
	{
		InputEventHandle<Event> inputEventHandle = delegate(Event inputEvent)
		{
			callBack(inputEvent);
		};
		m_ListenerHash.Add(callBack, inputEventHandle);
		AddListener(eventKey, inputEventHandle);
	}

	public override void RemoveListener(string eventKey, InputEventHandle<IInputEventBase> callBack)
	{
		if (!m_ListenerHash.ContainsKey(callBack))
		{
			throw new Exception("RemoveListener Exception: dont find Listener Hash ! eventKey: ->" + eventKey + "<-");
		}
		InputEventHandle<Event> callBack2 = m_ListenerHash[callBack];
		m_ListenerHash.Remove(callBack);
		RemoveListener(eventKey, callBack2);
	}

	public override void Dispatch(IInputEventBase inputEvent)
	{
		Dispatch((Event)inputEvent);
	}

	public void AddListener(string eventKey, InputEventHandle<Event> callBack)
	{
		if (!m_Listeners.ContainsKey(eventKey))
		{
			m_Listeners.Add(eventKey, callBack);
		}
		else
		{
			Dictionary<string, InputEventHandle<Event>> listeners;
			string key;
			(listeners = m_Listeners)[key = eventKey] = (InputEventHandle<Event>)Delegate.Combine(listeners[key], callBack);
		}
	}

	public void RemoveListener(string eventKey, InputEventHandle<Event> callBack)
	{
		if (m_Listeners.ContainsKey(eventKey))
		{
			Dictionary<string, InputEventHandle<Event>> listeners;
			string key;
			(listeners = m_Listeners)[key = eventKey] = (InputEventHandle<Event>)Delegate.Remove(listeners[key], callBack);
		}
	}

	public void Dispatch(Event inputEvent)
	{
		m_eventKey = inputEvent.EventKey;
		if (m_Listeners.TryGetValue(m_eventKey, out m_handle))
		{
			DispatchSingleEvent(inputEvent, m_handle);
		}
		DispatchSingleEvent(inputEvent, OnEventDispatch);
		AllEventDispatch(m_eventKey, inputEvent);
	}

	private void DispatchSingleEvent(Event inputEvent, InputEventHandle<Event> callBack)
	{
		if (callBack != null)
		{
			try
			{
				callBack(inputEvent);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex.ToString());
			}
		}
	}
}
