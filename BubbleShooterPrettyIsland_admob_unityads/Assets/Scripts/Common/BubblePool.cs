
using UnityEngine;

public class BubblePool : PoolObject
{
	public override void OnCreate()
	{
	}

	public override void OnFetch()
	{
	}

	public override void OnRecycle()
	{
		UnityEngine.Object.Destroy(GetComponent<Rigidbody2D>());
		GetComponent<BubbleObj>().RemoveFx();
	}

	public override void OnObjectDestroy()
	{
	}
}
