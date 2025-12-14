
using UnityEngine;

public abstract class IInputEventBase
{
	public float m_t;

	protected string m_eventKey;

	public string EventKey
	{
		get
		{
			if (m_eventKey == null)
			{
				m_eventKey = GetEventKey();
			}
			return m_eventKey;
		}
		set
		{
			m_eventKey = value;
		}
	}

	public IInputEventBase()
	{
		Reset();
	}

	public virtual void Reset()
	{
		m_eventKey = null;
		m_t = 0f;
	}

	protected virtual string GetEventKey()
	{
		if (m_eventKey == null)
		{
			m_eventKey = ToString();
		}
		return m_eventKey;
	}

	public virtual string Serialize()
	{
		return Serializer.Serialize(this);
	}

	public IInputEventBase Analysis(string data)
	{
		return JsonUtility.FromJson<IInputEventBase>(data);
	}
}
