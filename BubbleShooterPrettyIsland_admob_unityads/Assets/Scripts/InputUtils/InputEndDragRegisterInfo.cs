
using System;

public class InputEndDragRegisterInfo : InputEventRegisterInfo<InputUIOnEndDragEvent>
{
	public DragAcceptor m_acceptor;

	public InputUIEventDragCallBack m_OnEndDrag;

	public override void AddListener()
	{
		base.AddListener();
		DragAcceptor acceptor = m_acceptor;
		acceptor.m_OnEndDrag = (InputUIEventDragCallBack)Delegate.Combine(acceptor.m_OnEndDrag, m_OnEndDrag);
	}

	public override void RemoveListener()
	{
		base.RemoveListener();
		DragAcceptor acceptor = m_acceptor;
		acceptor.m_OnEndDrag = (InputUIEventDragCallBack)Delegate.Remove(acceptor.m_OnEndDrag, m_OnEndDrag);
	}
}
