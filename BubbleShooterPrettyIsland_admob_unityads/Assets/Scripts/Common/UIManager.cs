
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIStackManager))]
[RequireComponent(typeof(UILayerManager))]
[RequireComponent(typeof(UIAnimManager))]
public class UIManager : MonoBehaviour
{
    private static GameObject s_UIManagerGo;

    private static UILayerManager s_UILayerManager;

    private static UIAnimManager s_UIAnimManager;

    private static UIStackManager s_UIStackManager;

    private static EventSystem s_EventSystem;

    public static Dictionary<string, List<UIWindowBase>> s_UIs = new Dictionary<string, List<UIWindowBase>>();

    public static Dictionary<string, List<UIWindowBase>> s_hideUIs = new Dictionary<string, List<UIWindowBase>>();

    private static bool isInit = false;

    private static List<RewardType> mReawrd;

    private static Regex uiKey = new Regex("(\\S+)\\d+");

    public static UILayerManager UILayerManager
    {
        get
        {
            if (s_UILayerManager == null)
            {
                Init();
            }
            return s_UILayerManager;
        }
        set
        {
            s_UILayerManager = value;
        }
    }

    public static UIAnimManager UIAnimManager
    {
        get
        {
            if (s_UILayerManager == null)
            {
                Init();
            }
            return s_UIAnimManager;
        }
        set
        {
            s_UIAnimManager = value;
        }
    }

    public static UIStackManager UIStackManager
    {
        get
        {
            if (s_UIStackManager == null)
            {
                Init();
            }
            return s_UIStackManager;
        }
        set
        {
            s_UIStackManager = value;
        }
    }

    public static EventSystem EventSystem
    {
        get
        {
            if (s_EventSystem == null)
            {
                Init();
            }
            return s_EventSystem;
        }
        set
        {
            s_EventSystem = value;
        }
    }

    public static GameObject UIManagerGo
    {
        get
        {
            if (s_UIManagerGo == null)
            {
                Init();
            }
            return s_UIManagerGo;
        }
        set
        {
            s_UIManagerGo = value;
        }
    }

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        if (!isInit)
        {
            GameObject gameObject = GameObject.Find("UIManager");
            if (gameObject == null)
            {
                gameObject = GameObjectManager.CreateGameObject("UI/UIManager");
            }
            UIManagerGo = gameObject;
            s_UILayerManager = gameObject.GetComponent<UILayerManager>();
            s_UIAnimManager = gameObject.GetComponent<UIAnimManager>();
            s_UIStackManager = gameObject.GetComponent<UIStackManager>();
            s_EventSystem = gameObject.GetComponentInChildren<EventSystem>();
            if (Application.isPlaying)
            {
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
            }
        }
    }

    public static void InitAsync()
    {
        GameObject gameObject = GameObject.Find("UIManager");
        if (gameObject == null)
        {
            ResourceManager.LoadAsync("UIManager", delegate (LoadState status, object res)
            {
                if (status.isDone)
                {
                    SetUIManager(res as GameObject);
                }
            });
        }
        else
        {
            SetUIManager(gameObject);
        }
    }

    private static void SetUIManager(GameObject instance)
    {
        UIManagerGo = instance;
        UILayerManager = instance.GetComponent<UILayerManager>();
        UIAnimManager = instance.GetComponent<UIAnimManager>();
        UnityEngine.Object.DontDestroyOnLoad(instance);
    }

    public static void SetEventSystemEnable(bool enable)
    {
        if (EventSystem != null)
        {
            EventSystem.enabled = enable;
        }
        else
        {
            UnityEngine.Debug.LogError("EventSystem.current is null !");
        }
    }

    public static string[] GetCameraNames()
    {
        string[] array = new string[UILayerManager.UICameraList.Count];
        for (int i = 0; i < UILayerManager.UICameraList.Count; i++)
        {
            string[] array2 = array;
            int num = i;
            UILayerManager.UICameraData uICameraData = UILayerManager.UICameraList[i];
            array2[num] = uICameraData.m_key;
        }
        return array;
    }

    public static Camera GetCamera(string CameraKey = null)
    {
        UILayerManager.UICameraData uICameraDataByKey = UILayerManager.GetUICameraDataByKey(CameraKey);
        return uICameraDataByKey.m_camera;
    }

    public static void ChangeUICamera(UIWindowBase ui, string cameraKey)
    {
        UILayerManager.SetLayer(ui, cameraKey);
    }

    public static void ResetUICamera(UIWindowBase ui)
    {
        UILayerManager.SetLayer(ui, ui.cameraKey);
    }

    public static T CreateUIWindow<T>() where T : UIWindowBase
    {
        return (T)CreateUIWindow(typeof(T).Name);
    }

    public static UIWindowBase CreateUIWindow(string UIName)
    {
        string gameObjectName = "UI/" + UIName + "/" + UIName;
        GameObject gameObject = GameObjectManager.CreateGameObject(gameObjectName, UIManagerGo);
        UIWindowBase component = gameObject.GetComponent<UIWindowBase>();
        UISystemEvent.Dispatch(component, UIEvent.OnInit);
        component.windowStatus = UIWindowBase.WindowStatus.Create;
        try
        {
            component.Init(GetUIID(UIName));
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("OnInit Exception: " + ex.ToString());
        }
        AddHideUI(component);
        UILayerManager.SetLayer(component);
        gameObject.transform.localPosition = new Vector3(0f, Screen.height, 0f);
        return component;
    }

    public static UIWindowBase OpenUIWindow(string UIName, UICallBack callback = null, params object[] objs)
    {
        if (UIStackManager.GetLastUI(UIType.Normal) != null && UIStackManager.GetLastUI(UIType.Normal).UIName == UIName)
        {
            return null;
        }
        UIWindowBase uIWindowBase = GetHideUI(UIName);
        if (uIWindowBase == null)
        {
            uIWindowBase = CreateUIWindow(UIName);
        }
        RemoveHideUI(uIWindowBase);
        AddUI(uIWindowBase);
        UIWindowBase lastUI = UIStackManager.GetLastUI(uIWindowBase.m_UIType);
        if (lastUI != null)
        {
            UIAnimManager.StartPauseAnim(lastUI, null);
        }
        UIStackManager.OnUIOpen(uIWindowBase);
        UILayerManager.SetLayer(uIWindowBase);
        uIWindowBase.windowStatus = UIWindowBase.WindowStatus.OpenAnim;
        UISystemEvent.Dispatch(uIWindowBase, UIEvent.OnOpen);
        try
        {
            uIWindowBase.OnOpen();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError(UIName + " OnOpen Exception: " + ex.ToString());
        }
        UIAnimManager.StartEnterAnim(uIWindowBase, callback, objs);
        return uIWindowBase;
    }

    public static T OpenUIWindow<T>() where T : UIWindowBase
    {
        return (T)OpenUIWindow(typeof(T).Name, null);
    }

    public static void CloseUIWindow(UIWindowBase UI, bool isPlayAnim = true, UICallBack callback = null, params object[] objs)
    {
        RemoveUI(UI);
        UI.RemoveAllListener();
        if (isPlayAnim)
        {
            callback = ((callback == null) ? new UICallBack(CloseUIWindowCallBackPlayAnim) : ((UICallBack)Delegate.Combine(callback, new UICallBack(CloseUIWindowCallBackPlayAnim))));
            UI.windowStatus = UIWindowBase.WindowStatus.CloseAnim;
            UIAnimManager.StartExitAnim(UI, callback, objs);
        }
        else
        {
            CloseUIWindowCallBack(UI, objs);
        }
    }

    private static void CloseUIWindowCallBackPlayAnim(UIWindowBase UI, params object[] objs)
    {
        UI.windowStatus = UIWindowBase.WindowStatus.Close;
        UISystemEvent.Dispatch(UI, UIEvent.OnClose);
        try
        {
            UI.OnClose();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError(UI.UIName + " OnClose Exception: " + ex.ToString());
        }
        UIStackManager.OnUIClose(UI);
        AddHideUI(UI);
        UIWindowBase lastUI = UIStackManager.GetLastUI(UI.m_UIType);
        if (lastUI != null && lastUI.windowStatus == UIWindowBase.WindowStatus.Pause)
        {
            UIAnimManager.StartEnterAnim(lastUI, null);
        }
    }

    private static void CloseUIWindowCallBack(UIWindowBase UI, params object[] objs)
    {
        UI.windowStatus = UIWindowBase.WindowStatus.Close;
        UISystemEvent.Dispatch(UI, UIEvent.OnClose);
        try
        {
            UI.OnClose();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError(UI.UIName + " OnClose Exception: " + ex.ToString());
        }
        UIStackManager.OnUIClose(UI);
        AddHideUI(UI);
    }

    public static void CloseUIWindow(string UIname, bool isPlayAnim = true, UICallBack callback = null, params object[] objs)
    {
        UIWindowBase uI = GetUI(UIname);
        if (uI == null)
        {
            UnityEngine.Debug.LogError("CloseUIWindow Error UI ->" + UIname + "<-  not Exist!");
        }
        else
        {
            CloseUIWindow(GetUI(UIname), isPlayAnim, callback, objs);
        }
    }

    public static void CloseUIWindow<T>(bool isPlayAnim = true, UICallBack callback = null, params object[] objs) where T : UIWindowBase
    {
        CloseUIWindow(typeof(T).Name, isPlayAnim, callback, objs);
    }

    public static UIWindowBase ShowUI(string UIname)
    {
        UIWindowBase uI = GetUI(UIname);
        return ShowUI(uI);
    }

    public static UIWindowBase ShowUI(UIWindowBase ui)
    {
        ui.windowStatus = UIWindowBase.WindowStatus.Open;
        UISystemEvent.Dispatch(ui, UIEvent.OnShow);
        try
        {
            ui.Show();
            ui.OnShow();
            return ui;
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError(ui.UIName + " OnShow Exception: " + ex.ToString());
            return ui;
        }
    }

    public static UIWindowBase HideUI(string UIname)
    {
        UIWindowBase uI = GetUI(UIname);
        return HideUI(uI);
    }

    public static UIWindowBase HideUI(UIWindowBase ui)
    {
        ui.windowStatus = UIWindowBase.WindowStatus.Hide;
        UISystemEvent.Dispatch(ui, UIEvent.OnHide);
        try
        {
            ui.Hide();
            ui.OnHide();
            return ui;
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError(ui.UIName + " OnShow Exception: " + ex.ToString());
            return ui;
        }
    }

    public static void HideOtherUI(string UIName)
    {
        List<string> list = new List<string>(s_UIs.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            List<UIWindowBase> list2 = s_UIs[list[i]];
            for (int j = 0; j < list2.Count; j++)
            {
                if (list2[j].UIName != UIName)
                {
                    HideUI(list2[j]);
                }
            }
        }
    }

    public static void ShowOtherUI(string UIName)
    {
        List<string> list = new List<string>(s_UIs.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            List<UIWindowBase> list2 = s_UIs[list[i]];
            for (int j = 0; j < list2.Count; j++)
            {
                if (list2[j].UIName != UIName)
                {
                    ShowUI(list2[j]);
                }
            }
        }
    }

    public static void Reward(List<RewardType> reawrd)
    {
        mReawrd = reawrd;
        OpenUIAsync<RewardWindow>(delegate (UIWindowBase ui, object[] objs)
        {
            ui.GetComponent<RewardWindow>().InitReward(mReawrd);
        }, new object[0]);
    }

    public static void CloseAllUI(bool isPlayerAnim = false)
    {
        List<string> list = new List<string>(s_UIs.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            List<UIWindowBase> list2 = s_UIs[list[i]];
            for (int j = 0; j < list2.Count; j++)
            {
                CloseUIWindow(list2[j], isPlayerAnim, null);
            }
        }
    }

    public static void CloseLastUI(UIType uiType = UIType.Normal)
    {
        UIStackManager.CloseLastUIWindow(uiType);
    }

    public static void OpenUIAsync<T>(UICallBack callback, params object[] objs) where T : UIWindowBase
    {
        string name = typeof(T).Name;
        OpenUIAsync(name, callback, objs);
    }

    public static void OpenUIAsync(string UIName, UICallBack callback, params object[] objs)
    {
        ResourceManager.LoadAsync(UIName, delegate (LoadState loadState, object resObject)
        {
            if (loadState.isDone)
            {
                OpenUIWindow(UIName, callback, objs);
            }
        });
    }

    public static void DestroyUI(UIWindowBase UI)
    {
        UnityEngine.Debug.Log("UIManager DestroyUI " + UI.name);
        if (GetIsExitsHide(UI))
        {
            RemoveHideUI(UI);
        }
        else if (GetIsExits(UI))
        {
            RemoveUI(UI);
        }
        UISystemEvent.Dispatch(UI, UIEvent.OnDestroy);
        try
        {
            UI.DestroyUI();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("OnDestroy :" + ex.ToString());
        }
        UnityEngine.Object.Destroy(UI.gameObject);
    }

    public static void DestroyAllUI()
    {
        DestroyAllActiveUI();
        DestroyAllHideUI();
    }

    public static void DestroyAllActiveUI()
    {
        foreach (List<UIWindowBase> value in s_UIs.Values)
        {
            for (int i = 0; i < value.Count; i++)
            {
                UISystemEvent.Dispatch(value[i], UIEvent.OnDestroy);
                try
                {
                    value[i].DestroyUI();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError("OnDestroy :" + ex.ToString());
                }
                UnityEngine.Object.Destroy(value[i].gameObject);
            }
        }
        s_UIs.Clear();
    }

    public static T GetUI<T>() where T : UIWindowBase
    {
        return (T)GetUI(typeof(T).Name);
    }

    public static UIWindowBase GetUI(string UIname)
    {
        if (!s_UIs.ContainsKey(UIname))
        {
            return null;
        }
        if (s_UIs[UIname].Count == 0)
        {
            return null;
        }
        return s_UIs[UIname][s_UIs[UIname].Count - 1];
    }

    public static UIBase GetUIBaseByEventKey(string eventKey)
    {
        string text = eventKey.Split('.')[0];
        string[] array = text.Split('_');
        string text2 = string.Empty;
        UIBase uIBase = null;
        for (int i = 0; i < array.Length; i++)
        {
            if (i == 0)
            {
                text2 = array[0];
                uIBase = GetUIWindowByEventKey(text2);
            }
            else
            {
                text2 = text2 + "_" + array[i];
                uIBase = uIBase.GetItemByKey(text2);
            }
            UnityEngine.Debug.Log("uiEventKey " + text2);
        }
        return uIBase;
    }

    private static UIWindowBase GetUIWindowByEventKey(string eventKey)
    {
        string value = uiKey.Match(eventKey).Groups[1].Value;
        if (!s_UIs.ContainsKey(value))
        {
            throw new Exception("UIManager: GetUIWindowByEventKey error dont find UI name: ->" + eventKey + "<-  " + value);
        }
        List<UIWindowBase> list = s_UIs[value];
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].UIEventKey == eventKey)
            {
                return list[i];
            }
        }
        throw new Exception("UIManager: GetUIWindowByEventKey error dont find UI name: ->" + eventKey + "<-  " + value);
    }

    private static bool GetIsExits(UIWindowBase UI)
    {
        if (!s_UIs.ContainsKey(UI.name))
        {
            return false;
        }
        return s_UIs[UI.name].Contains(UI);
    }

    private static void AddUI(UIWindowBase UI)
    {
        if (!s_UIs.ContainsKey(UI.name))
        {
            s_UIs.Add(UI.name, new List<UIWindowBase>());
        }
        s_UIs[UI.name].Add(UI);
        UI.Show();
    }

    private static void RemoveUI(UIWindowBase UI)
    {
        if (UI == null)
        {
            throw new Exception("UIManager: RemoveUI error l_UI is null: !");
        }
        if (!s_UIs.ContainsKey(UI.name))
        {
            throw new Exception("UIManager: RemoveUI error dont find UI name: ->" + UI.name + "<-  " + UI);
        }
        if (!s_UIs[UI.name].Contains(UI))
        {
            throw new Exception("UIManager: RemoveUI error dont find UI: ->" + UI.name + "<-  " + UI);
        }
        s_UIs[UI.name].Remove(UI);
    }

    private static int GetUIID(string UIname)
    {
        if (!s_UIs.ContainsKey(UIname))
        {
            return 0;
        }
        int num = s_UIs[UIname].Count;
        for (int i = 0; i < s_UIs[UIname].Count; i++)
        {
            if (s_UIs[UIname][i].UIID == num)
            {
                num++;
                i = 0;
            }
        }
        return num;
    }

    public static int GetNormalUICount()
    {
        return UIStackManager.m_normalStack.Count;
    }

    public static int GetFixedUICount()
    {
        return UIStackManager.m_fixedStack.Count;
    }

    public static void DestroyAllHideUI()
    {
        foreach (List<UIWindowBase> value in s_hideUIs.Values)
        {
            for (int i = 0; i < value.Count; i++)
            {
                UISystemEvent.Dispatch(value[i], UIEvent.OnDestroy);
                try
                {
                    value[i].DestroyUI();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError("OnDestroy :" + ex.ToString());
                }
                UnityEngine.Object.Destroy(value[i].gameObject);
            }
        }
        s_hideUIs.Clear();
    }

    public static bool GetWindowActive(string UIname)
    {
        UIWindowBase uI = GetUI(UIname);
        if (uI != null && uI.gameObject.activeSelf)
        {
            return true;
        }
        return false;
    }

    public static T GetHideUI<T>() where T : UIWindowBase
    {
        string name = typeof(T).Name;
        return (T)GetHideUI(name);
    }

    public static UIWindowBase GetHideUI(string UIname)
    {
        if (!s_hideUIs.ContainsKey(UIname))
        {
            return null;
        }
        if (s_hideUIs[UIname].Count == 0)
        {
            return null;
        }
        return s_hideUIs[UIname][s_hideUIs[UIname].Count - 1];
    }

    private static bool GetIsExitsHide(UIWindowBase UI)
    {
        if (!s_hideUIs.ContainsKey(UI.name))
        {
            return false;
        }
        return s_hideUIs[UI.name].Contains(UI);
    }

    private static void AddHideUI(UIWindowBase UI)
    {
        if (!s_hideUIs.ContainsKey(UI.name))
        {
            s_hideUIs.Add(UI.name, new List<UIWindowBase>());
        }
        s_hideUIs[UI.name].Add(UI);
        UI.Hide();
    }

    private static void RemoveHideUI(UIWindowBase UI)
    {
        if (UI == null)
        {
            throw new Exception("UIManager: RemoveUI error l_UI is null: !");
        }
        if (!s_hideUIs.ContainsKey(UI.name))
        {
            throw new Exception("UIManager: RemoveUI error dont find: " + UI.name + "  " + UI);
        }
        if (!s_hideUIs[UI.name].Contains(UI))
        {
            throw new Exception("UIManager: RemoveUI error dont find: " + UI.name + "  " + UI);
        }
        s_hideUIs[UI.name].Remove(UI);
    }
}
