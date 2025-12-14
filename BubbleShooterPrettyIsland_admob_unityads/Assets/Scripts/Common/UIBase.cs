
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
	protected struct GuideChangeData
	{
		public bool isCreateCanvas;

		public bool isCreateGraphic;

		public string oldSortingLayerName;

		public int OldSortingOrder;

		public bool OldOverrideSorting;
	}

	public Canvas m_canvas;

	private int m_UIID = -1;

	private string m_UIName;

	public List<GameObject> m_objectList = new List<GameObject>();

	private Dictionary<string, UIBase> m_uiBases = new Dictionary<string, UIBase>();

	private Dictionary<string, GameObject> m_objects = new Dictionary<string, GameObject>();

	private Dictionary<string, Vector2> m_objectsPostion = new Dictionary<string, Vector2>();

	private Dictionary<string, Image> m_images = new Dictionary<string, Image>();

	private Dictionary<string, Sprite> m_Sprites = new Dictionary<string, Sprite>();

	private Dictionary<string, Text> m_texts = new Dictionary<string, Text>();

	private Dictionary<string, TextMesh> m_textmeshs = new Dictionary<string, TextMesh>();

	private Dictionary<string, Button> m_buttons = new Dictionary<string, Button>();

	private Dictionary<string, ScrollRect> m_scrollRects = new Dictionary<string, ScrollRect>();

	private Dictionary<string, RawImage> m_rawImages = new Dictionary<string, RawImage>();

	private Dictionary<string, RectTransform> m_rectTransforms = new Dictionary<string, RectTransform>();

	private Dictionary<string, InputField> m_inputFields = new Dictionary<string, InputField>();

	private Dictionary<string, Slider> m_Sliders = new Dictionary<string, Slider>();

	private Dictionary<string, Canvas> m_Canvas = new Dictionary<string, Canvas>();

	private Dictionary<string, Toggle> m_Toggle = new Dictionary<string, Toggle>();

	private Dictionary<string, LongPressAcceptor> m_longPressList = new Dictionary<string, LongPressAcceptor>();

	private RectTransform m_rectTransform;

	protected List<Enum> m_EventNames = new List<Enum>();

	protected List<EventHandRegisterInfo> m_EventListeners = new List<EventHandRegisterInfo>();

	protected List<InputEventRegisterInfo> m_OnClickEvents = new List<InputEventRegisterInfo>();

	protected List<InputEventRegisterInfo> m_LongPressEvents = new List<InputEventRegisterInfo>();

	protected List<InputEventRegisterInfo> m_DragEvents = new List<InputEventRegisterInfo>();

	protected List<InputEventRegisterInfo> m_BeginDragEvents = new List<InputEventRegisterInfo>();

	protected List<InputEventRegisterInfo> m_EndDragEvents = new List<InputEventRegisterInfo>();

	private List<UIBase> m_ChildList = new List<UIBase>();

	private int m_childUIIndex;

	protected List<GameObject> m_GuideList = new List<GameObject>();

	protected Dictionary<GameObject, GuideChangeData> m_CreateCanvasDict = new Dictionary<GameObject, GuideChangeData>();

	public int UIID => m_UIID;

	public string UIEventKey => UIName + m_UIID;

	public string UIName
	{
		get
		{
			if (m_UIName == null)
			{
				m_UIName = base.name;
			}
			return m_UIName;
		}
		set
		{
			m_UIName = value;
		}
	}

	public RectTransform RectTransform
	{
		get
		{
			if (m_rectTransform == null)
			{
				m_rectTransform = GetComponent<RectTransform>();
			}
			return m_rectTransform;
		}
		set
		{
			m_rectTransform = value;
		}
	}

	public virtual void OnInit()
	{
	}

	public void DestroyUI()
	{
		ClearGuideModel();
		RemoveAllListener();
		CleanItem();
		OnUIDestroy();
	}

	protected virtual void OnUIDestroy()
	{
	}

	public virtual void Init(int id)
	{
		m_UIID = id;
		m_canvas = GetComponent<Canvas>();
		m_UIName = null;
		CreateObjectTable();
		OnInit();
	}

	private void CreateObjectTable()
	{
		m_objects.Clear();
		m_images.Clear();
		m_Sprites.Clear();
		m_texts.Clear();
		m_textmeshs.Clear();
		m_buttons.Clear();
		m_scrollRects.Clear();
		m_rawImages.Clear();
		m_rectTransforms.Clear();
		m_inputFields.Clear();
		m_Sliders.Clear();
		m_longPressList.Clear();
		m_Canvas.Clear();
		for (int i = 0; i < m_objectList.Count; i++)
		{
			if (m_objectList[i] != null)
			{
				if (m_objects.ContainsKey(m_objectList[i].name))
				{
					UnityEngine.Debug.LogError("CreateObjectTable ContainsKey ->" + m_objectList[i].name + "<-");
					continue;
				}
				m_objects.Add(m_objectList[i].name, m_objectList[i]);
				m_objectsPostion.Add(m_objectList[i].name, GetRectTransform(m_objectList[i].name).anchoredPosition);
			}
			else
			{
				UnityEngine.Debug.LogWarning(base.name + " m_objectList[" + i + "] is Null !");
			}
		}
	}

	public bool HaveObject(string name)
	{
		//bool flag = false;
		return m_objects.ContainsKey(name);
	}

	public GameObject GetGameObject(string name)
	{
		if (m_objects.Count == 0)
		{
			CreateObjectTable();
		}
		if (m_objects.ContainsKey(name))
		{
			GameObject gameObject = m_objects[name];
			if (gameObject == null)
			{
				throw new Exception("UIWindowBase GetGameObject error: " + UIName + " m_objects[" + name + "] is null !!");
			}
			return gameObject;
		}
		throw new Exception("UIWindowBase GetGameObject error: " + UIName + " dont find ->" + name + "<-");
	}

	public Vector2 GetGameObjectPostion(string name)
	{
		if (m_objectsPostion.Count == 0)
		{
			CreateObjectTable();
		}
		if (m_objectsPostion.ContainsKey(name))
		{
			return m_objectsPostion[name];
		}
		throw new Exception("UIWindowBase GetGameObjectPostion error: " + UIName + " dont find ->" + name + "<-");
	}

	public RectTransform GetRectTransform(string name)
	{
		if (m_rectTransforms.ContainsKey(name))
		{
			return m_rectTransforms[name];
		}
		RectTransform component = GetGameObject(name).GetComponent<RectTransform>();
		if (component == null)
		{
			throw new Exception(m_EventNames + " GetRectTransform ->" + name + "<- is Null !");
		}
		m_rectTransforms.Add(name, component);
		return component;
	}

	public UIBase GetUIBase(string name)
	{
		if (m_uiBases.ContainsKey(name))
		{
			return m_uiBases[name];
		}
		UIBase component = GetGameObject(name).GetComponent<UIBase>();
		if (component == null)
		{
			throw new Exception(m_EventNames + " GetUIBase ->" + name + "<- is Null !");
		}
		m_uiBases.Add(name, component);
		return component;
	}

	public Sprite GetSprite(string name)
	{
		if (m_Sprites.ContainsKey(name))
		{
			return m_Sprites[name];
		}
		Sprite component = GetGameObject(name).GetComponent<Sprite>();
		if (component == null)
		{
			throw new Exception(m_EventNames + " GetImage ->" + name + "<- is Null !");
		}
		m_Sprites.Add(name, component);
		return component;
	}

	public Image GetImage(string name)
	{
		if (m_images.ContainsKey(name))
		{
			return m_images[name];
		}
		Image component = GetGameObject(name).GetComponent<Image>();
		if (component == null)
		{
			throw new Exception(m_EventNames + " GetImage ->" + name + "<- is Null !");
		}
		m_images.Add(name, component);
		return component;
	}

	public TextMesh GetTextMesh(string name)
	{
		if (m_textmeshs.ContainsKey(name))
		{
			return m_textmeshs[name];
		}
		TextMesh component = GetGameObject(name).GetComponent<TextMesh>();
		if (component == null)
		{
			throw new Exception(m_EventNames + " GetText ->" + name + "<- is Null !");
		}
		m_textmeshs.Add(name, component);
		return component;
	}

	public Text GetText(string name)
	{
		if (m_texts.ContainsKey(name))
		{
			return m_texts[name];
		}
		Text component = GetGameObject(name).GetComponent<Text>();
		if (component == null)
		{
			throw new Exception(m_EventNames + " GetText ->" + name + "<- is Null !");
		}
		m_texts.Add(name, component);
		return component;
	}

	public Toggle GetToggle(string name)
	{
		if (m_Toggle.ContainsKey(name))
		{
			return m_Toggle[name];
		}
		Toggle component = GetGameObject(name).GetComponent<Toggle>();
		if (component == null)
		{
			throw new Exception(m_EventNames + " GetText ->" + name + "<- is Null !");
		}
		m_Toggle.Add(name, component);
		return component;
	}

	public Button GetButton(string name)
	{
		if (m_buttons.ContainsKey(name))
		{
			return m_buttons[name];
		}
		Button component = GetGameObject(name).GetComponent<Button>();
		if (component == null)
		{
			throw new Exception(m_EventNames + " GetButton ->" + name + "<- is Null !");
		}
		m_buttons.Add(name, component);
		return component;
	}

	public InputField GetInputField(string name)
	{
		if (m_inputFields.ContainsKey(name))
		{
			return m_inputFields[name];
		}
		InputField component = GetGameObject(name).GetComponent<InputField>();
		if (component == null)
		{
			throw new Exception(m_EventNames + " GetInputField ->" + name + "<- is Null !");
		}
		m_inputFields.Add(name, component);
		return component;
	}

	public ScrollRect GetScrollRect(string name)
	{
		if (m_scrollRects.ContainsKey(name))
		{
			return m_scrollRects[name];
		}
		ScrollRect component = GetGameObject(name).GetComponent<ScrollRect>();
		if (component == null)
		{
			throw new Exception(m_EventNames + " GetScrollRect ->" + name + "<- is Null !");
		}
		m_scrollRects.Add(name, component);
		return component;
	}

	public RawImage GetRawImage(string name)
	{
		if (m_rawImages.ContainsKey(name))
		{
			return m_rawImages[name];
		}
		RawImage component = GetGameObject(name).GetComponent<RawImage>();
		if (component == null)
		{
			throw new Exception(m_EventNames + " GetRawImage ->" + name + "<- is Null !");
		}
		m_rawImages.Add(name, component);
		return component;
	}

	public Slider GetSlider(string name)
	{
		if (m_Sliders.ContainsKey(name))
		{
			return m_Sliders[name];
		}
		Slider component = GetGameObject(name).GetComponent<Slider>();
		if (component == null)
		{
			throw new Exception(m_EventNames + " GetSlider ->" + name + "<- is Null !");
		}
		m_Sliders.Add(name, component);
		return component;
	}

	public Canvas GetCanvas(string name)
	{
		if (m_Canvas.ContainsKey(name))
		{
			return m_Canvas[name];
		}
		Canvas component = GetGameObject(name).GetComponent<Canvas>();
		if (component == null)
		{
			throw new Exception(m_EventNames + " GetSlider ->" + name + "<- is Null !");
		}
		m_Canvas.Add(name, component);
		return component;
	}

	public Vector3 GetPosition(string name, bool islocal)
	{
		Vector3 result = Vector3.zero;
		GameObject gameObject = GetGameObject(name);
		if (gameObject != null)
		{
			result = ((!islocal) ? GetGameObject(name).transform.position : GetGameObject(name).transform.localPosition);
		}
		return result;
	}

	public void SetSizeDelta(float w, float h)
	{
		RectTransform.sizeDelta = new Vector2(w, h);
	}

	public virtual void RemoveAllListener()
	{
		for (int i = 0; i < m_EventListeners.Count; i++)
		{
			m_EventListeners[i].RemoveListener();
		}
		m_EventListeners.Clear();
		for (int j = 0; j < m_OnClickEvents.Count; j++)
		{
			m_OnClickEvents[j].RemoveListener();
		}
		m_OnClickEvents.Clear();
		for (int k = 0; k < m_LongPressEvents.Count; k++)
		{
			m_LongPressEvents[k].RemoveListener();
		}
		m_LongPressEvents.Clear();
		for (int l = 0; l < m_DragEvents.Count; l++)
		{
			m_DragEvents[l].RemoveListener();
		}
		m_DragEvents.Clear();
		for (int m = 0; m < m_BeginDragEvents.Count; m++)
		{
			m_BeginDragEvents[m].RemoveListener();
		}
		m_BeginDragEvents.Clear();
		for (int n = 0; n < m_EndDragEvents.Count; n++)
		{
			m_EndDragEvents[n].RemoveListener();
		}
		m_EndDragEvents.Clear();
	}

	private bool GetRegister(List<InputEventRegisterInfo> list, string eventKey)
	{
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].eventKey == eventKey)
			{
				num++;
			}
		}
		return num == 0;
	}

	public void AddOnClickListener(string buttonName, InputEventHandle<InputUIOnClickEvent> callback, string parm = null)
	{
		InputButtonClickRegisterInfo onClickListener = InputUIEventProxy.GetOnClickListener(GetButton(buttonName), UIEventKey, buttonName, parm, callback);
		onClickListener.AddListener();
		m_OnClickEvents.Add(onClickListener);
	}

	public void AddOnClickListenerByCreate(Button button, string compName, InputEventHandle<InputUIOnClickEvent> callback, string parm = null)
	{
		InputButtonClickRegisterInfo onClickListener = InputUIEventProxy.GetOnClickListener(button, UIEventKey, compName, parm, callback);
		onClickListener.AddListener();
		m_OnClickEvents.Add(onClickListener);
	}

	public void AddEventListener(Enum EventEnum, EventHandle handle)
	{
		EventHandRegisterInfo eventHandRegisterInfo = new EventHandRegisterInfo();
		eventHandRegisterInfo.m_EventKey = EventEnum;
		eventHandRegisterInfo.m_hande = handle;
		GlobalEvent.AddEvent(EventEnum, handle);
		m_EventListeners.Add(eventHandRegisterInfo);
	}

	public InputButtonClickRegisterInfo GetClickRegisterInfo(string buttonName, InputEventHandle<InputUIOnClickEvent> callback, string parm)
	{
		string eventKey = InputUIOnClickEvent.GetEventKey(UIEventKey, buttonName, parm);
		for (int i = 0; i < m_OnClickEvents.Count; i++)
		{
			InputButtonClickRegisterInfo inputButtonClickRegisterInfo = (InputButtonClickRegisterInfo)m_OnClickEvents[i];
			if (inputButtonClickRegisterInfo.eventKey == eventKey && inputButtonClickRegisterInfo.callBack == callback)
			{
				return inputButtonClickRegisterInfo;
			}
		}
		throw new Exception("GetClickRegisterInfo Exception not find RegisterInfo by " + buttonName + " parm " + parm);
	}

	public void RemoveOnClickListener(string buttonName, InputEventHandle<InputUIOnClickEvent> callback, string parm = null)
	{
		InputButtonClickRegisterInfo clickRegisterInfo = GetClickRegisterInfo(buttonName, callback, parm);
		m_OnClickEvents.Remove(clickRegisterInfo);
		clickRegisterInfo.RemoveListener();
	}

	public UIBase CreateItem(string itemName, string prantName, bool isActive)
	{
		GameObject gameObject = GameObjectManager.CreateGameObject(itemName, GetGameObject(prantName));
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		UIBase component = gameObject.GetComponent<UIBase>();
		if (component == null)
		{
			throw new Exception("CreateItem Error : ->" + itemName + "<- don't have UIBase Component!");
		}
		component.Init(m_childUIIndex++);
		component.UIName = UIEventKey + "_" + component.UIName;
		m_ChildList.Add(component);
		return component;
	}

	public UIBase CreateItem(GameObject itemObj, GameObject parent, bool isActive)
	{
		GameObject gameObject = GameObjectManager.CreateGameObject(itemObj, parent);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		UIBase component = gameObject.GetComponent<UIBase>();
		if (component == null)
		{
			throw new Exception("CreateItem Error : ->" + itemObj.name + "<- don't have UIBase Component!");
		}
		component.Init(m_childUIIndex++);
		component.UIName = UIEventKey + "_" + component.UIName;
		m_ChildList.Add(component);
		return component;
	}

	public UIBase CreateItem(string itemName, string prantName)
	{
		return CreateItem(itemName, prantName, isActive: true);
	}

	public void DestroyItem(UIBase item)
	{
		DestroyItem(item, isActive: true);
	}

	public void DestroyItem(UIBase item, bool isActive)
	{
		if (m_ChildList.Contains(item))
		{
			m_ChildList.Remove(item);
			item.RemoveAllListener();
			item.OnUIDestroy();
			GameObjectManager.CreateGameObject(item.gameObject);
		}
	}

	public void DestroyItem(UIBase item, float t)
	{
		if (m_ChildList.Contains(item))
		{
			m_ChildList.Remove(item);
			item.RemoveAllListener();
			item.OnUIDestroy();
			GameObjectManager.CreateGameObject(item.gameObject);
		}
	}

	public void CleanItem()
	{
		CleanItem(isActive: true);
	}

	public void CleanItem(bool isActive)
	{
		for (int i = 0; i < m_ChildList.Count; i++)
		{
			m_ChildList[i].RemoveAllListener();
			m_ChildList[i].OnUIDestroy();
			GameObjectManager.CreateGameObject(m_ChildList[i].gameObject);
		}
		m_ChildList.Clear();
		m_childUIIndex = 0;
	}

	public GameObject GetItem(string itemName)
	{
		if (!itemName.Contains("."))
		{
			int num = 0;
			if (itemName.Contains("["))
			{
				num = int.Parse(itemName.Substring(itemName.IndexOf("[") + 1, 1));
			}
			int num2 = 0;
			for (int i = 0; i < m_ChildList.Count; i++)
			{
				if (m_ChildList[i].name == itemName)
				{
					if (num == num2)
					{
						return m_ChildList[i].gameObject;
					}
					num2++;
				}
			}
		}
		else
		{
			string[] array = itemName.Split('.');
			List<Transform> list = new List<Transform>();
			for (int j = 0; j < array.Length; j++)
			{
				int num3 = 0;
				string text = array[j];
				if (text.Contains("["))
				{
					num3 = int.Parse(text.Substring(text.IndexOf("[") + 1, 1));
				}
				text = text.Substring(0, text.Length - 3);
				if (j == 0)
				{
					Transform[] componentsInChildren = GetGameObject(text).GetComponentsInChildren<Transform>();
					list = new List<Transform>(componentsInChildren);
					continue;
				}
				int num4 = 0;
				for (int k = 0; k < list.Count; k++)
				{
					if (!(list[k].name == text))
					{
						continue;
					}
					if (num3 == num4)
					{
						if (j == array.Length - 1)
						{
							return list[k].gameObject;
						}
						Transform[] componentsInChildren2 = list[k].GetComponentsInChildren<Transform>();
						list = new List<Transform>(componentsInChildren2);
						break;
					}
					num4++;
				}
			}
		}
		throw new Exception(UIName + " GetItem Exception Dont find Item: " + itemName);
	}

	public UIBase GetItemByIndex(string itemName, int index)
	{
		for (int i = 0; i < m_ChildList.Count; i++)
		{
			if (m_ChildList[i].name == itemName)
			{
				index--;
				if (index == 0)
				{
					return m_ChildList[i];
				}
			}
		}
		throw new Exception(UIName + " GetItem Exception Dont find Item: " + itemName);
	}

	public UIBase GetItemByKey(string uiEvenyKey)
	{
		for (int i = 0; i < m_ChildList.Count; i++)
		{
			if (m_ChildList[i].UIEventKey == uiEvenyKey)
			{
				return m_ChildList[i];
			}
		}
		throw new Exception(UIName + " GetItemByKey Exception Dont find Item: " + uiEvenyKey);
	}

	public bool GetItemIsExist(string itemName)
	{
		for (int i = 0; i < m_ChildList.Count; i++)
		{
			if (m_ChildList[i].name == itemName)
			{
				return true;
			}
		}
		return false;
	}

	public void SetText(string TextID, string content)
	{
		GetText(TextID).text = content.Replace("\\n", "\n");
	}

	public void SetImageColor(string ImageID, Color color)
	{
		GetImage(ImageID).color = color;
	}

	public void SetTextColor(string TextID, Color color)
	{
		GetText(TextID).color = color;
	}

	public void SetImageAlpha(string ImageID, float alpha)
	{
		Color color = GetImage(ImageID).color;
		color.a = alpha;
		GetImage(ImageID).color = color;
	}

	public void SetInputText(string TextID, string content)
	{
		GetInputField(TextID).text = content;
	}

	[Obsolete]
	public void SetTextByLangeage(string textID, string contentID, params object[] objs)
	{
		GetText(textID).text = LanguageManager.GetContent("default", contentID, objs);
	}

	public void SetTextByLangeage(string textID, string moduleName, string contentID, params object[] objs)
	{
		GetText(textID).text = LanguageManager.GetContent(moduleName, contentID, objs);
	}

	public void SetSlider(string sliderID, float value)
	{
		GetSlider(sliderID).value = value;
	}

	public void SetActive(string gameObjectID, bool isShow)
	{
		GetGameObject(gameObjectID).SetActive(isShow);
	}

	public void SetEnabeled(string ID, bool enable)
	{
		GetButton(ID).enabled = enable;
	}

	public void SetButtonInteractable(string ID, bool enable)
	{
		GetButton(ID).interactable = enable;
	}

	public void SetRectWidth(string TextID, float value, float height)
	{
		GetRectTransform(TextID).sizeDelta = Vector2.right * (0f - value) * 2f + Vector2.up * height;
	}

	public void SetWidth(string TextID, float width, float height)
	{
		GetRectTransform(TextID).sizeDelta = Vector2.right * width + Vector2.up * height;
	}

	public void SetPosition(string TextID, float x, float y, float z, bool islocal)
	{
		if (islocal)
		{
			GetRectTransform(TextID).localPosition = Vector3.right * x + Vector3.up * y + Vector3.forward * z;
		}
		else
		{
			GetRectTransform(TextID).position = Vector3.right * x + Vector3.up * y + Vector3.forward * z;
		}
	}

	public void SetAnchoredPosition(string ID, float x, float y)
	{
		GetRectTransform(ID).anchoredPosition = Vector2.right * x + Vector2.up * y;
	}

	public void SetScale(string TextID, float x, float y, float z)
	{
		GetGameObject(TextID).transform.localScale = Vector3.right * x + Vector3.up * y + Vector3.forward * z;
	}

	public void SetMeshText(string TextID, string txt)
	{
		GetTextMesh(TextID).text = txt;
	}

	public void SetGuideMode(string objName, int order = 1)
	{
		SetGuideMode(GetGameObject(objName), order);
	}

	public void SetItemGuideMode(string itemName, int order = 1)
	{
		SetGuideMode(GetItem(itemName), order);
	}

	public void SetItemGuideModeByIndex(string itemName, int index, int order = 1)
	{
		SetGuideMode(GetItemByIndex(itemName, index).gameObject, order);
	}

	public void SetSelfGuideMode(int order = 1)
	{
		SetGuideMode(base.gameObject, order);
	}

	public void SetGuideMode(GameObject go, int order = 1)
	{
		Canvas canvas = go.GetComponent<Canvas>();
		GraphicRaycaster component = go.GetComponent<GraphicRaycaster>();
		GuideChangeData value = default(GuideChangeData);
		if (canvas == null)
		{
			canvas = go.AddComponent<Canvas>();
			value.isCreateCanvas = true;
		}
		if (component == null)
		{
			component = go.AddComponent<GraphicRaycaster>();
			value.isCreateGraphic = true;
		}
		value.OldOverrideSorting = canvas.overrideSorting;
		value.OldSortingOrder = canvas.sortingOrder;
		value.oldSortingLayerName = canvas.sortingLayerName;
		bool activeSelf = go.activeSelf;
		if (!activeSelf)
		{
			go.SetActive(value: true);
		}
		canvas.overrideSorting = true;
		canvas.sortingOrder = order;
		canvas.sortingLayerName = "Guide";
		if (!activeSelf)
		{
			go.SetActive(value: false);
		}
		if (!m_CreateCanvasDict.ContainsKey(go))
		{
			m_CreateCanvasDict.Add(go, value);
			m_GuideList.Add(go);
		}
		else
		{
			UnityEngine.Debug.LogError("m_CreateCanvasDict " + go);
		}
	}

	public void CancelGuideModel(GameObject go)
	{
		if (go == null)
		{
			UnityEngine.Debug.LogError("go is null");
			return;
		}
		Canvas component = go.GetComponent<Canvas>();
		GraphicRaycaster component2 = go.GetComponent<GraphicRaycaster>();
		if (m_CreateCanvasDict.ContainsKey(go))
		{
			GuideChangeData guideChangeData = m_CreateCanvasDict[go];
			if (component2 != null && guideChangeData.isCreateGraphic)
			{
				UnityEngine.Object.DestroyImmediate(component2);
			}
			if (component != null && guideChangeData.isCreateCanvas)
			{
				UnityEngine.Object.DestroyImmediate(component);
			}
			else if (component != null)
			{
				component.overrideSorting = guideChangeData.OldOverrideSorting;
				component.sortingOrder = guideChangeData.OldSortingOrder;
				component.sortingLayerName = guideChangeData.oldSortingLayerName;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("m_CreateCanvasDict.ContainsKey(go) is error");
		}
	}

	public void ClearGuideModel()
	{
		for (int i = 0; i < m_GuideList.Count; i++)
		{
			CancelGuideModel(m_GuideList[i]);
		}
		for (int j = 0; j < m_ChildList.Count; j++)
		{
			m_ChildList[j].ClearGuideModel();
		}
		m_GuideList.Clear();
		m_CreateCanvasDict.Clear();
	}

	[ContextMenu("ObjectList Deduplication")]
	public void ClearObject()
	{
		List<GameObject> list = new List<GameObject>();
		int count = m_objectList.Count;
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = m_objectList[i];
			if (gameObject != null && !list.Contains(gameObject))
			{
				list.Add(gameObject);
			}
		}
		list.Sort((GameObject a, GameObject b) => a.name.CompareTo(b.name));
		m_objectList = list;
	}
}
