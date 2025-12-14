
public class BoxPool : PoolObject
{
	public override void OnCreate()
	{
	}

	public override void OnFetch()
	{
	}

	public override void OnRecycle()
	{
		GetComponent<BoxObj>().SetBubble(null);
	}

	public override void OnObjectDestroy()
	{
	}
}
