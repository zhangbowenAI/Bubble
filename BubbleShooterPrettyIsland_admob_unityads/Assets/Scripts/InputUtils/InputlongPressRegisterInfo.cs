
using System;

public class InputlongPressRegisterInfo : InputEventRegisterInfo<InputUILongPressEvent>
{
	public LongPressAcceptor m_acceptor;

	public InputUIEventLongPressCallBack m_OnLongPress;

	public override void RemoveListener()
	{
		base.RemoveListener();
		LongPressAcceptor acceptor = m_acceptor;
		acceptor.OnLongPress = (InputUIEventLongPressCallBack)Delegate.Remove(acceptor.OnLongPress, m_OnLongPress);
	}

	public override void AddListener()
	{
		base.AddListener();
		LongPressAcceptor acceptor = m_acceptor;
		acceptor.OnLongPress = (InputUIEventLongPressCallBack)Delegate.Combine(acceptor.OnLongPress, m_OnLongPress);
	}
}
