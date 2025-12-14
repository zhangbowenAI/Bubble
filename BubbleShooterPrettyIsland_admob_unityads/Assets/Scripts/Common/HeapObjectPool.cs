
using System.Collections.Generic;

public class HeapObjectPool
{
	public static Dictionary<string, object> GetSODict()
	{
		return HeapObjectPool<Dictionary<string, object>>.GetObject();
	}

	public static void PutSODict(Dictionary<string, object> dict)
	{
		dict.Clear();
		HeapObjectPool<Dictionary<string, object>>.PutObject(dict);
	}
}
public static class HeapObjectPool<T> where T : new()
{
	private static Stack<T> s_pool = new Stack<T>();

	public static int GetCount()
	{
		return s_pool.Count;
	}

	public static T GetObject()
	{
		T val;
		IHeapObjectInterface heapObjectInterface;
		if (s_pool.Count > 0)
		{
			val = s_pool.Pop();
			heapObjectInterface = (val as IHeapObjectInterface);
		}
		else
		{
			val = new T();
			heapObjectInterface = (val as IHeapObjectInterface);
			heapObjectInterface?.OnInit();
		}
		heapObjectInterface?.OnPop();
		return val;
	}

	public static void PutObject(T obj)
	{
		(obj as IHeapObjectInterface)?.OnPush();
		s_pool.Push(obj);
	}
}
