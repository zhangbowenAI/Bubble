
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputUIEventProxy : IInputProxyBase
{
    public static InputButtonClickRegisterInfo GetOnClickListener(Button button, string UIName, string ComponentName, string parm, InputEventHandle<InputUIOnClickEvent> callback)
    {
        InputButtonClickRegisterInfo @object = HeapObjectPool<InputButtonClickRegisterInfo>.GetObject();
        @object.eventKey = InputUIOnClickEvent.GetEventKey(UIName, ComponentName, parm);
        @object.callBack = callback;
        @object.m_button = button;
        @object.m_OnClick = delegate
        {
            DispatchOnClickEvent(UIName, ComponentName, parm);
        };
        return @object;
    }

    public static InputEventRegisterInfo<InputUILongPressEvent> GetLongPressListener(LongPressAcceptor acceptor, string UIName, string ComponentName, string parm, InputEventHandle<InputUILongPressEvent> callback)
    {
        InputlongPressRegisterInfo @object = HeapObjectPool<InputlongPressRegisterInfo>.GetObject();
        @object.eventKey = InputUILongPressEvent.GetEventKey(UIName, ComponentName, parm);
        @object.callBack = callback;
        @object.m_acceptor = acceptor;
        @object.m_OnLongPress = delegate (InputUIEventType type)
        {
            DispatchLongPressEvent(UIName, ComponentName, parm, type);
        };
        return @object;
    }

    public static InputEventRegisterInfo<InputUIOnScrollEvent> GetOnScrollListener(string UIName, string ComponentName, InputEventHandle<InputUIOnScrollEvent> callback)
    {
        InputEventRegisterInfo<InputUIOnScrollEvent> @object = HeapObjectPool<InputEventRegisterInfo<InputUIOnScrollEvent>>.GetObject();
        @object.eventKey = InputUIOnScrollEvent.GetEventKey(UIName, ComponentName);
        @object.callBack = callback;
        InputManager.AddListener(InputUIOnScrollEvent.GetEventKey(UIName, ComponentName), callback);
        return @object;
    }

    public static InputEventRegisterInfo<InputUIOnDragEvent> GetOnDragListener(DragAcceptor acceptor, string UIName, string ComponentName, string parm, InputEventHandle<InputUIOnDragEvent> callback)
    {
        InputDragRegisterInfo @object = HeapObjectPool<InputDragRegisterInfo>.GetObject();
        @object.eventKey = InputUIOnDragEvent.GetEventKey(UIName, ComponentName);
        @object.callBack = callback;
        @object.m_acceptor = acceptor;
        @object.m_OnDrag = delegate (PointerEventData data)
        {
            DispatchDragEvent(UIName, ComponentName, parm, data);
        };
        return @object;
    }

    public static InputBeginDragRegisterInfo GetOnBeginDragListener(DragAcceptor acceptor, string UIName, string ComponentName, string parm, InputEventHandle<InputUIOnBeginDragEvent> callback)
    {
        InputBeginDragRegisterInfo @object = HeapObjectPool<InputBeginDragRegisterInfo>.GetObject();
        @object.eventKey = InputUIOnBeginDragEvent.GetEventKey(UIName, ComponentName);
        @object.callBack = callback;
        @object.m_acceptor = acceptor;
        @object.m_OnBeginDrag = delegate (PointerEventData data)
        {
            DispatchBeginDragEvent(UIName, ComponentName, parm, data);
        };
        return @object;
    }

    public static InputEndDragRegisterInfo GetOnEndDragListener(DragAcceptor acceptor, string UIName, string ComponentName, string parm, InputEventHandle<InputUIOnEndDragEvent> callback)
    {
        InputEndDragRegisterInfo @object = HeapObjectPool<InputEndDragRegisterInfo>.GetObject();
        @object.eventKey = InputUIOnEndDragEvent.GetEventKey(UIName, ComponentName);
        @object.callBack = callback;
        @object.m_acceptor = acceptor;
        @object.m_OnEndDrag = delegate (PointerEventData data)
        {
            DispatchEndDragEvent(UIName, ComponentName, parm, data);
        };
        return @object;
    }

    public static void DispatchOnClickEvent(string UIName, string ComponentName, string parm)
    {
        if (IInputProxyBase.IsActive)
        {
            AudioPlayManager.PlaySFX2D("button");
            InputUIOnClickEvent uIEvent = GetUIEvent<InputUIOnClickEvent>(UIName, ComponentName, parm);
            InputManager.Dispatch("InputUIOnClickEvent", uIEvent);
        }
    }

    public static void DispatchLongPressEvent(string UIName, string ComponentName, string parm, InputUIEventType type)
    {
        if (IInputProxyBase.IsActive)
        {
            InputUILongPressEvent uIEvent = GetUIEvent<InputUILongPressEvent>(UIName, ComponentName, parm);
            uIEvent.m_type = type;
            uIEvent.EventKey = InputUILongPressEvent.GetEventKey(UIName, ComponentName, parm);
            InputManager.Dispatch("InputUILongPressEvent", uIEvent);
        }
    }

    public static void DispatchScrollEvent(string UIName, string ComponentName, string parm, Vector2 position)
    {
        if (IInputProxyBase.IsActive)
        {
            InputUIOnScrollEvent onScrollEvent = GetOnScrollEvent(UIName, ComponentName, parm, position);
            InputManager.Dispatch("InputUIOnScrollEvent", onScrollEvent);
        }
    }

    public static void DispatchDragEvent(string UIName, string ComponentName, string parm, PointerEventData data)
    {
        if (IInputProxyBase.IsActive)
        {
            InputUIOnDragEvent uIEvent = GetUIEvent<InputUIOnDragEvent>(UIName, ComponentName, parm);
            uIEvent.m_dragPosition = data.position;
            uIEvent.m_delta = data.delta;
            InputManager.Dispatch("InputUIOnDragEvent", uIEvent);
        }
    }

    public static void DispatchBeginDragEvent(string UIName, string ComponentName, string parm, PointerEventData data)
    {
        if (IInputProxyBase.IsActive)
        {
            InputUIOnBeginDragEvent uIEvent = GetUIEvent<InputUIOnBeginDragEvent>(UIName, ComponentName, parm);
            uIEvent.m_dragPosition = data.position;
            uIEvent.m_delta = data.delta;
            InputManager.Dispatch("InputUIOnBeginDragEvent", uIEvent);
        }
    }

    public static void DispatchEndDragEvent(string UIName, string ComponentName, string parm, PointerEventData data)
    {
        if (IInputProxyBase.IsActive)
        {
            InputUIOnEndDragEvent uIEvent = GetUIEvent<InputUIOnEndDragEvent>(UIName, ComponentName, parm);
            uIEvent.m_dragPosition = data.position;
            uIEvent.m_delta = data.delta;
            InputManager.Dispatch("InputUIOnEndDragEvent", uIEvent);
        }
    }

    private static T GetUIEvent<T>(string UIName, string ComponentName, string parm) where T : InputUIEventBase, new()
    {
        T @object = HeapObjectPool<T>.GetObject();
        @object.Reset();
        @object.m_name = UIName;
        @object.m_compName = ComponentName;
        @object.m_pram = parm;
        return @object;
    }

    private static InputUIOnScrollEvent GetOnScrollEvent(string UIName, string ComponentName, string parm, Vector2 position)
    {
        InputUIOnScrollEvent uIEvent = GetUIEvent<InputUIOnScrollEvent>(UIName, ComponentName, parm);
        uIEvent.m_pos = position;
        return uIEvent;
    }
}
