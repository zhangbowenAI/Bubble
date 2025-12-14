
using UnityEngine;

public class PoolObject : MonoBehaviour
{
	public bool SetActive = true;

	public virtual void OnCreate()
	{
	}

	public virtual void OnFetch()
	{
	}

	public virtual void OnRecycle()
	{
	}

	public virtual void OnObjectDestroy()
	{
	}
}
