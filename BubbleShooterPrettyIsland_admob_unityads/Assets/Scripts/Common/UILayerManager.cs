
using System;
using System.Collections.Generic;
using UnityEngine;

public class UILayerManager : MonoBehaviour
{
	[Serializable]
	public struct UICameraData
	{
		public string m_key;

		public GameObject m_root;

		public Camera m_camera;

		public Transform m_GameUILayerParent;

		public Transform m_FixedLayerParent;

		public Transform m_NormalLayerParent;

		public Transform m_TopbarLayerParent;

		public Transform m_PopUpLayerParent;
	}

	public List<UICameraData> UICameraList = new List<UICameraData>();

	public void Awake()
	{
		for (int i = 0; i < UICameraList.Count; i++)
		{
			UICameraData uICameraData = UICameraList[i];
			uICameraData.m_root.transform.localPosition = new Vector3(0f, 0f, i * -2000);
			if (uICameraData.m_root == null)
			{
				UnityEngine.Debug.LogError("UILayerManager :Root is null!  key : " + uICameraData.m_key + " index : " + i);
			}
			if (uICameraData.m_camera == null)
			{
				UnityEngine.Debug.LogError("UILayerManager :Camera is null!  key : " + uICameraData.m_key + " index : " + i);
			}
			if (uICameraData.m_GameUILayerParent == null)
			{
				UnityEngine.Debug.LogError("UILayerManager :GameUILayerParent is null! key : " + uICameraData.m_key + " index : " + i);
			}
			if (uICameraData.m_FixedLayerParent == null)
			{
				UnityEngine.Debug.LogError("UILayerManager :FixedLayerParent is null! key : " + uICameraData.m_key + " index : " + i);
			}
			if (uICameraData.m_NormalLayerParent == null)
			{
				UnityEngine.Debug.LogError("UILayerManager :NormalLayerParent is null! key : " + uICameraData.m_key + " index : " + i);
			}
			if (uICameraData.m_TopbarLayerParent == null)
			{
				UnityEngine.Debug.LogError("UILayerManager :TopbarLayerParent is null! key : " + uICameraData.m_key + " index : " + i);
			}
			if (uICameraData.m_PopUpLayerParent == null)
			{
				UnityEngine.Debug.LogError("UILayerManager :popUpLayerParent is null! key : " + uICameraData.m_key + " index : " + i);
			}
		}
	}

	public void SetLayer(UIWindowBase ui, string cameraKey = null)
	{
		UICameraData uICameraDataByKey = GetUICameraDataByKey(cameraKey);
		uICameraDataByKey = ((cameraKey != null) ? GetUICameraDataByKey(cameraKey) : GetUICameraDataByKey(ui.cameraKey));
		RectTransform component = ui.GetComponent<RectTransform>();
		switch (ui.m_UIType)
		{
		case UIType.GameUI:
			ui.transform.SetParent(uICameraDataByKey.m_GameUILayerParent);
			break;
		case UIType.Fixed:
			ui.transform.SetParent(uICameraDataByKey.m_FixedLayerParent);
			break;
		case UIType.Normal:
			ui.transform.SetParent(uICameraDataByKey.m_NormalLayerParent);
			break;
		case UIType.TopBar:
			ui.transform.SetParent(uICameraDataByKey.m_TopbarLayerParent);
			break;
		case UIType.PopUp:
			ui.transform.SetParent(uICameraDataByKey.m_PopUpLayerParent);
			break;
		}
		component.localScale = Vector3.one;
		component.sizeDelta = Vector2.zero;
		if (ui.m_UIType != 0)
		{
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector3.one;
			component.sizeDelta = Vector2.zero;
			component.transform.localPosition = Vector3.zero;
			component.anchoredPosition = Vector3.zero;
			component.SetAsLastSibling();
		}
	}

	public UICameraData GetUICameraDataByKey(string key)
	{
		if (key == null || key == string.Empty)
		{
			if (UICameraList.Count > 0)
			{
				return UICameraList[0];
			}
			throw new Exception("UICameraList is null ! " + key);
		}
		for (int i = 0; i < UICameraList.Count; i++)
		{
			UICameraData uICameraData = UICameraList[i];
			if (uICameraData.m_key == key)
			{
				return UICameraList[i];
			}
		}
		throw new Exception("Dont Find UILayerData by " + key);
	}
}
