
using System;

public class InputDragRegisterInfo : InputEventRegisterInfo<InputUIOnDragEvent>
{
	public DragAcceptor m_acceptor;

	public InputUIEventDragCallBack m_OnDrag;

	public override void AddListener()
	{
		base.AddListener();
		DragAcceptor acceptor = m_acceptor;
		acceptor.m_OnDrag = (InputUIEventDragCallBack)Delegate.Combine(acceptor.m_OnDrag, m_OnDrag);
	}

	public override void RemoveListener()
	{
		base.RemoveListener();
		DragAcceptor acceptor = m_acceptor;
		acceptor.m_OnDrag = (InputUIEventDragCallBack)Delegate.Remove(acceptor.m_OnDrag, m_OnDrag);
	}
}
