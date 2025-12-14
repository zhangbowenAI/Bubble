using UnityEngine;

public class LoadState
{
	private static LoadState completeState;

	public bool isDone;

	public float progress;

	public static LoadState CompleteState
	{
		get
		{
			if (completeState == null)
			{
				completeState = new LoadState();
				completeState.isDone = true;
				completeState.progress = 1f;
			}
			return completeState;
		}
	}

	public void UpdateProgress(ResourceRequest resourceRequest)
	{
		isDone = resourceRequest.isDone;
		progress = resourceRequest.progress;
	}

	public void UpdateProgress(AssetBundleCreateRequest assetBundleCreateRequest)
	{
		isDone = assetBundleCreateRequest.isDone;
		progress = assetBundleCreateRequest.progress;
	}

}
