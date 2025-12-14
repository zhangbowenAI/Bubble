
public abstract class IInputProxyBase
{
	private static bool s_isActive = true;

	public static bool IsActive
	{
		get
		{
			return s_isActive;
		}
		set
		{
			s_isActive = value;
		}
	}

	public static T GetEvent<T>(string eventKey) where T : IInputEventBase, new()
	{
		T @object = HeapObjectPool<T>.GetObject();
		@object.EventKey = eventKey;
		return @object;
	}
}
