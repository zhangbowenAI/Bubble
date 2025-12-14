
using UnityEngine;

public class BoxObj : MonoBehaviour
{
	private BubbleObj mBubble;

	public Point point;

	public void InitBox(Point _point)
	{
		point.X = _point.X;
		point.Y = _point.Y;
	}

	public void SetBubble(BubbleObj obj)
	{
		if (!(mBubble != null) || !(mBubble == obj))
		{
			if (mBubble != null && obj != null)
			{
				UnityEngine.Debug.Log("   There is already a ball in this position  " + obj.point.X + "      " + obj.point.Y);
				mBubble.RemoveBubble(RemoveType.BeRmove);
			}
			mBubble = obj;
			if (obj == null)
			{
				GameManager.Instance.RemoveBubble(obj);
			}
			else
			{
				GameManager.Instance.AddBubble(obj);
			}
		}
	}

	public void RemoveBubble(BubbleObj obj)
	{
		mBubble = null;
		GameManager.Instance.RemoveBubble(obj);
	}

	public BubbleObj GetBubble()
	{
		return mBubble;
	}
}
