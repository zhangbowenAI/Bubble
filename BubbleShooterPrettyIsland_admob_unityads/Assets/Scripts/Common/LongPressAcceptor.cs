
using UnityEngine;
using UnityEngine.EventSystems;

public class LongPressAcceptor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	public float LongPressTime = 1f;

	private float m_Timer;

	private bool isPress;

	private bool isDispatch;

	public InputUIEventLongPressCallBack OnLongPress;

	public void OnPointerDown(PointerEventData eventData)
	{
		isPress = true;
		isDispatch = false;
		m_Timer = 0f;
		if (OnLongPress != null)
		{
			OnLongPress(InputUIEventType.PressDown);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isPress = false;
		if (OnLongPress != null)
		{
			OnLongPress(InputUIEventType.PressUp);
		}
	}

	public void Update()
	{
		if (!isPress || isDispatch)
		{
			return;
		}
		m_Timer += Time.deltaTime;
		if (m_Timer > LongPressTime)
		{
			isDispatch = true;
			if (OnLongPress != null)
			{
				OnLongPress(InputUIEventType.LongPress);
			}
		}
	}
}
