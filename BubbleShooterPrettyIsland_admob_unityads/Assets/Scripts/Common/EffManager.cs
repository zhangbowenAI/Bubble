
using Spine.Unity;
using UnityEngine;

public class EffManager : Singleton<EffManager>
{
	public void PlayRemoveEff(Point point, string path)
	{
		if (point.X != -1)
		{
			PoolObject poolObject = GameObjectManager.GetPoolObject(path);
			poolObject.transform.SetParent(GameManager.Instance.FXParent.transform, worldPositionStays: false);
			poolObject.transform.position = GameManager.Instance.GetBox(point).transform.position;
			GameObjectManager.DestroyPoolObject(poolObject, 2f);
		}
	}

	public void PlayRemoveEff(BubbleObj bubble, string path)
	{
		PoolObject poolObject = GameObjectManager.GetPoolObject(path);
		poolObject.transform.SetParent(GameManager.Instance.FXParent.transform, worldPositionStays: false);
		poolObject.transform.position = bubble.transform.position;
		GameObjectManager.DestroyPoolObject(poolObject, 2f);
	}

	public void PlayRemoveEffBySpine(Point point, string path, string animName = "")
	{
		PoolObject poolObject = GameObjectManager.GetPoolObject(path);
		poolObject.transform.SetParent(GameManager.Instance.FXParent.transform, worldPositionStays: false);
		poolObject.transform.position = GameManager.Instance.GetBox(point).transform.position;
		SkeletonAnimation component = poolObject.GetComponent<SkeletonAnimation>();
		component.Initialize(overwrite: true);
		if (animName != string.Empty)
		{
			component.state.SetAnimation(0, animName, loop: false);
		}
		GameObjectManager.DestroyPoolObject(poolObject, 4f);
	}

	public void PlayEff(BubbleObj bubble, string path)
	{
		GameObject gameObject = GameObjectManager.CreateGameObject(path);
		gameObject.transform.SetParent(bubble.FXParent.transform, worldPositionStays: false);
		
		Object.Destroy(gameObject, 2f);
	}

	public void PlayEffByUI(GameObject obj, string path)
	{
		GameObject gameObject = GameObjectManager.CreateGameObject(path);
		gameObject.transform.SetParent(obj.transform, worldPositionStays: false);
		gameObject.transform.localPosition = Vector3.zero;
		
		Object.Destroy(gameObject, 2f);
	}

	public void PlayEff4(BubbleObj bubble, string path, float removetiem = 2f, bool isCenter = false)
	{
		GameObject gameObject = GameObjectManager.CreateGameObject(path);
		gameObject.transform.SetParent(GameManager.Instance.FXParent.transform, worldPositionStays: false);
		gameObject.transform.position = bubble.transform.position;
		if (isCenter)
		{
			Transform transform = gameObject.transform;
			Vector3 position = gameObject.transform.position;
			float y = position.y;
			Vector3 position2 = gameObject.transform.position;
			transform.position = new Vector3(0f, y, position2.z);
		}
		
		Object.Destroy(gameObject, removetiem);
	}

	public void PlayEff2(Point point, string path, int type, float removetiem = 2f, bool isCenter = false)
	{
		switch (type)
		{
		case 1:
		{
			GameObject gameObject = GameObjectManager.CreateGameObject(path);
			gameObject.transform.SetParent(GameManager.Instance.FXParent.transform, worldPositionStays: false);
			gameObject.transform.position = GameManager.Instance.GetBox(point).transform.position;
			if (isCenter)
			{
				Transform transform = gameObject.transform;
				Vector3 position = gameObject.transform.position;
				float y = position.y;
				Vector3 position2 = gameObject.transform.position;
				transform.position = new Vector3(0f, y, position2.z);
			}
			Object.Destroy(gameObject, removetiem);
			break;
		}
		case 2:
			Timer.DelayCallBack(0.1f, delegate
			{
				GameObjectManager.CreatePoolObjectAsync(path, delegate(PoolObject value)
				{
					if (value.GetComponent<SkeletonAnimation>() != null)
					{
						value.GetComponent<SkeletonAnimation>().Initialize(overwrite: true);
					}
					value.transform.SetParent(GameManager.Instance.FXParent.transform, worldPositionStays: false);
					value.transform.position = GameManager.Instance.GetBox(point).transform.position;
					if (isCenter)
					{
						Transform transform2 = value.transform;
						Vector3 position3 = value.transform.position;
						float y2 = position3.y;
						Vector3 position4 = value.transform.position;
						transform2.position = new Vector3(0f, y2, position4.z);
					}
					GameObjectManager.DestroyPoolObject(value, 2f);
				});
			});
			break;
		default:
			Timer.DelayCallBack(0.1f, delegate
			{
				GameObjectManager.CreatePoolObjectAsync(path, delegate(PoolObject value)
				{
					if (value.GetComponent<SkeletonAnimation>() != null)
					{
						value.GetComponent<SkeletonAnimation>().Initialize(overwrite: true);
					}
					value.transform.SetParent(GameManager.Instance.FXSkill4.transform, worldPositionStays: false);
					value.transform.localPosition = Vector3.zero;
					if (isCenter)
					{
						Transform transform3 = value.transform;
						Vector3 position5 = value.transform.position;
						float y3 = position5.y;
						Vector3 position6 = value.transform.position;
						transform3.position = new Vector3(0f, y3, position6.z);
					}
					GameObjectManager.DestroyPoolObject(value, 2f);
				});
			});
			break;
		}
	}

	public GameObject PlayEff3(Point point, string path)
	{
		GameObject gameObject = GameObjectManager.CreateGameObject(path);
		gameObject.transform.SetParent(GameManager.Instance.FXParent.transform, worldPositionStays: false);
		gameObject.transform.position = GameManager.Instance.GetBox(point).transform.position;
		return gameObject;
	}
}
