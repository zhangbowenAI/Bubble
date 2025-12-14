
using System;

public class InputBeginDragRegisterInfo : InputEventRegisterInfo<InputUIOnBeginDragEvent>
{
	public DragAcceptor m_acceptor;

	public InputUIEventDragCallBack m_OnBeginDrag;

	public override void AddListener()
	{
		base.AddListener();
		DragAcceptor acceptor = m_acceptor;
		acceptor.m_OnBeginDrag = (InputUIEventDragCallBack)Delegate.Combine(acceptor.m_OnBeginDrag, m_OnBeginDrag);
	}

	public override void RemoveListener()
	{
		base.RemoveListener();
		DragAcceptor acceptor = m_acceptor;
		acceptor.m_OnBeginDrag = (InputUIEventDragCallBack)Delegate.Remove(acceptor.m_OnBeginDrag, m_OnBeginDrag);
	}
}
