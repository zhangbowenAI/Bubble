
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAcceptor : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IEventSystemHandler
{
	public InputUIEventDragCallBack m_OnBeginDrag;

	public InputUIEventDragCallBack m_OnDrag;

	public InputUIEventDragCallBack m_OnEndDrag;

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (m_OnBeginDrag != null)
		{
			m_OnBeginDrag(eventData);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (m_OnDrag != null)
		{
			m_OnDrag(eventData);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (m_OnEndDrag != null)
		{
			m_OnEndDrag(eventData);
		}
	}
}
