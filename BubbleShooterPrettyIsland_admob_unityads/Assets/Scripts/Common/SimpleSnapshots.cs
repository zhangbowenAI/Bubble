
using Lean;
using UnityEngine;

public class SimpleSnapshots : MonoBehaviour
{
	public LineRenderer[] LineRenderers;

	protected virtual void LateUpdate()
	{
		if (LineRenderers == null)
		{
			return;
		}
		int i;
		for (i = 0; i < LineRenderers.Length; i++)
		{
			LineRenderer lineRenderer = LineRenderers[i];
			if (!(lineRenderer != null))
			{
				continue;
			}
			LeanFinger leanFinger = LeanTouch.Fingers.Find((LeanFinger f) => f.Index == i);
			if (leanFinger != null)
			{
				lineRenderer.positionCount = leanFinger.Snapshots.Count;
				
				for (int j = 0; j < leanFinger.Snapshots.Count; j++)
				{
					LeanFinger.Snapshot snapshot = leanFinger.Snapshots[j];
					if (snapshot != null)
					{
						lineRenderer.SetPosition(j, snapshot.GetWorldPosition(1f));
					}
				}
			}
			else
			{
				
				lineRenderer.positionCount = 0;
			}
		}
	}
}
