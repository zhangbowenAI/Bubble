
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
	private static Vector3 s_OutOfRange = new Vector3(9000f, 9000f, 9000f);

	private static Transform s_poolParent;

	private static Dictionary<string, List<PoolObject>> s_objectPool_new = new Dictionary<string, List<PoolObject>>();

	public static Transform PoolParent
	{
		get
		{
			if (s_poolParent == null)
			{
				GameObject gameObject = new GameObject("ObjectPool");
				s_poolParent = gameObject.transform;
				if (Application.isPlaying)
				{
					UnityEngine.Object.DontDestroyOnLoad(s_poolParent);
				}
			}
			return s_poolParent;
		}
	}

	private static PoolObject CreatePoolObject(string gameObjectName, GameObject parent = null)
	{
		GameObject gameObject = ResourceManager.Load<GameObject>(gameObjectName);
		if (gameObject == null)
		{
			throw new Exception("CreatPoolObject error dont find : ->" + gameObjectName + "<-");
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
		gameObject2.name = gameObject.name;
		PoolObject component = gameObject2.GetComponent<PoolObject>();
		if (component == null)
		{
			throw new Exception("CreatPoolObject error : ->" + gameObjectName + "<- not is PoolObject !");
		}
		component.OnCreate();
		if (parent != null)
		{
			gameObject2.transform.SetParent(parent.transform);
		}
		gameObject2.SetActive(value: true);
		return component;
	}

	public static GameObject CreateGameObject(string gameObjectName, GameObject parent = null)
	{
		GameObject gameObject = ResourceManager.Load<GameObject>(gameObjectName);
		if (gameObject == null)
		{
			throw new Exception("CreateGameObject error dont find :" + gameObjectName);
		}
		return CreateGameObject(gameObject, parent);
	}

	public static GameObject CreateGameObject(GameObject prefab, GameObject parent = null)
	{
		if (prefab == null)
		{
			throw new Exception("CreateGameObject error : l_prefab  is null");
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(prefab);
		gameObject.name = prefab.name;
		if (parent != null)
		{
			gameObject.transform.SetParent(parent.transform);
		}
		return gameObject;
	}

	public static void PutPoolObject(string gameObjectName)
	{
		DestroyPoolObject(CreatePoolObject(gameObjectName));
	}

	public static bool IsExist_New(string objectName)
	{
		if (objectName == null)
		{
			UnityEngine.Debug.LogError("IsExist_New error : objectName is null!");
			return false;
		}
		if (s_objectPool_new.ContainsKey(objectName) && s_objectPool_new[objectName].Count > 0)
		{
			return true;
		}
		return false;
	}

	public static PoolObject GetPoolObject(string _name, GameObject parent = null)
	{
		string[] array = _name.Split('/');
		string text = array[array.Length - 1];
		PoolObject poolObject;
		if (IsExist_New(text))
		{
			poolObject = s_objectPool_new[text][0];
			s_objectPool_new[text].RemoveAt(0);
			if ((bool)poolObject && poolObject.SetActive)
			{
				poolObject.gameObject.SetActive(value: true);
			}
			if (parent == null)
			{
				poolObject.transform.SetParent(null);
			}
			else
			{
				poolObject.transform.SetParent(parent.transform);
			}
		}
		else
		{
			poolObject = CreatePoolObject(_name, parent);
		}
		poolObject.OnFetch();
		return poolObject;
	}

	public static void DestroyPoolObject(PoolObject obj)
	{
		string text = obj.name.Replace("(Clone)", string.Empty);
		if (!s_objectPool_new.ContainsKey(text))
		{
			s_objectPool_new.Add(text, new List<PoolObject>());
		}
		if (s_objectPool_new[text].Contains(obj))
		{
			throw new Exception("DestroyPoolObject:-> Repeat Destroy GameObject !" + obj);
		}
		s_objectPool_new[text].Add(obj);
		if (obj.SetActive)
		{
			obj.gameObject.SetActive(value: false);
		}
		else
		{
			obj.transform.position = s_OutOfRange;
		}
		obj.OnRecycle();
		obj.name = text;
		obj.transform.SetParent(PoolParent);
	}

	public static void DestroyPoolObject(PoolObject go, float time)
	{
		Timer.DelayCallBack(time, delegate
		{
			if (go != null)
			{
				DestroyPoolObject(go);
			}
		});
	}

	public static void CleanPool_New()
	{
		foreach (string key in s_objectPool_new.Keys)
		{
			if (s_objectPool_new.ContainsKey(key))
			{
				List<PoolObject> list = s_objectPool_new[key];
				for (int i = 0; i < list.Count; i++)
				{
					try
					{
						list[i].OnObjectDestroy();
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.Log(ex.ToString());
					}
					UnityEngine.Object.Destroy(list[i].gameObject);
				}
				list.Clear();
			}
		}
		s_objectPool_new.Clear();
	}

	public static void CleanPoolByName_New(string name)
	{
		if (s_objectPool_new.ContainsKey(name))
		{
			List<PoolObject> list = s_objectPool_new[name];
			for (int i = 0; i < list.Count; i++)
			{
				try
				{
					list[i].OnObjectDestroy();
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.Log(ex.ToString());
				}
				UnityEngine.Object.Destroy(list[i].gameObject);
			}
			list.Clear();
			s_objectPool_new.Remove(name);
		}
	}

	public static void CreatePoolObjectAsync(string path, CallBack<PoolObject> callback, GameObject parent = null)
	{
		ResourceManager.LoadAsync(path, delegate(LoadState status, object res)
		{
			try
			{
				if (status.isDone)
				{
					callback(CreatePoolObject(path, parent));
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("CreatePoolObjectAsync Exception: " + ex.ToString());
			}
		});
	}
}
