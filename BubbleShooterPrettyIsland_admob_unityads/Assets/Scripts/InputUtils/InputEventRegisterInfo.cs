
public class InputEventRegisterInfo : IHeapObjectInterface
{
	public string eventKey;

	public void OnInit()
	{
	}

	public void OnPop()
	{
	}

	public void OnPush()
	{
	}

	public virtual void AddListener()
	{
	}

	public virtual void RemoveListener()
	{
	}
}
public class InputEventRegisterInfo<T> : InputEventRegisterInfo where T : IInputEventBase
{
	public InputEventHandle<T> callBack;

	public override void AddListener()
	{
		InputManager.AddListener(eventKey, callBack);
	}

	public override void RemoveListener()
	{
		InputManager.RemoveListener(eventKey, callBack);
		HeapObjectPool<InputEventRegisterInfo<T>>.PutObject(this);
	}
}
