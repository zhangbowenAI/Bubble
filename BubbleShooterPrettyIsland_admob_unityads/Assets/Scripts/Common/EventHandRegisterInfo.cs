
using System;

public class EventHandRegisterInfo
{
	public Enum m_EventKey;

	public EventHandle m_hande;

	public void RemoveListener()
	{
		GlobalEvent.RemoveEvent(m_EventKey, m_hande);
	}
}
