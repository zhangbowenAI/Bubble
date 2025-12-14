
using Lean;
using System;
using UnityEngine;

public class SimpleSplitScreen : MonoBehaviour
{
	public Transform LeftObject;

	public Transform RightObject;

	protected virtual void OnEnable()
	{
		LeanTouch.OnFingerSet = (Action<LeanFinger>)Delegate.Combine(LeanTouch.OnFingerSet, new Action<LeanFinger>(OnFingerSet));
	}

	protected virtual void OnDisable()
	{
		LeanTouch.OnFingerSet = (Action<LeanFinger>)Delegate.Remove(LeanTouch.OnFingerSet, new Action<LeanFinger>(OnFingerSet));
	}

	public void OnFingerSet(LeanFinger finger)
	{
		if (finger.ScreenPosition.x > (float)(Screen.width / 2))
		{
			if (RightObject != null)
			{
				RightObject.position = finger.GetWorldPosition(10f);
			}
		}
		else if (RightObject != null)
		{
			LeftObject.position = finger.GetWorldPosition(10f);
		}
	}
}
