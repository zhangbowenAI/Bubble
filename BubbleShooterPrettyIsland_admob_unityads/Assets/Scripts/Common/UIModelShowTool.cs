
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public static class UIModelShowTool
{
	public static GameObject Create(string prefabName, out RenderTexture tex)
	{
		GameObject gameObject = new GameObject("UIModelShow");
		GameObject gameObject2 = new GameObject("Camera");
		gameObject2.transform.SetParent(gameObject.transform);
		gameObject2.transform.localPosition = new Vector3(0f, 5000f, 0f);
		Camera camera = gameObject2.AddComponent<Camera>();
		camera.clearFlags = CameraClearFlags.Color;
		camera.backgroundColor = new Color(0f, 0f, 0f, 0.0196078438f);
		camera.orthographic = true;
		camera.orthographicSize = 0.72f;
		camera.depth = 100f;
		camera.cullingMask = 1 << LayerMask.NameToLayer("UI");
		GameObject gameObject3 = new GameObject("Root");
		gameObject3.transform.SetParent(gameObject2.transform);
		gameObject3.transform.localPosition = new Vector3(0f, 0f, 100f);
		gameObject3.transform.eulerAngles = new Vector3(0f, 180f, 0f);
		GameObject gameObject4 = GameObjectManager.CreateGameObject(prefabName);
		gameObject4.transform.SetParent(gameObject3.transform);
		gameObject4.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject4.transform.localEulerAngles = Vector3.zero;
		Transform[] componentsInChildren = gameObject4.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = LayerMask.NameToLayer("UI");
		}
		SkinnedMeshRenderer[] componentsInChildren2 = gameObject4.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].shadowCastingMode = ShadowCastingMode.Off;
			componentsInChildren2[j].receiveShadows = false;
		}
		tex = new RenderTexture(512, 512, 100);
		tex.autoGenerateMips = false;
		tex.anisoLevel = 1;
		camera.targetTexture = tex;
		return gameObject4;
	}

	public static void AddDrag(GameObject UIObj, GameObject modelObj)
	{
		ModelRotate @object = modelObj.AddComponent<ModelRotate>();
		EventTrigger eventTrigger = UIObj.GetComponent<EventTrigger>();
		if ((bool)eventTrigger)
		{
			eventTrigger.triggers.Clear();
		}
		else
		{
			eventTrigger = UIObj.AddComponent<EventTrigger>();
		}
		eventTrigger.triggers.Add(GetEvent(EventTriggerType.Drag, @object.OnDrag));
	}

	private static EventTrigger.Entry GetEvent(EventTriggerType type, UnityAction<BaseEventData> eventFun)
	{
		UnityAction<BaseEventData> call = eventFun.Invoke;
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.Drag;
		entry.callback.AddListener(call);
		return entry;
	}
}
