
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	public bool isDontDestroy;

	private static T instance;

	public static T Instance
	{
		get
		{
			if ((Object)instance == (Object)null)
			{
				instance = (T)UnityEngine.Object.FindObjectOfType(typeof(T));
				if ((Object)instance == (Object)null)
				{
					UnityEngine.Debug.LogError(typeof(T) + "is nothing");
				}
			}
			return instance;
		}
	}

	protected virtual void Awake()
	{
		if (CheckInstance() && isDontDestroy)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	protected bool CheckInstance()
	{
		if (this == Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(this);
		return false;
	}
}
