
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;

public class MemoryManager
{
    public static bool s_allowDynamicLoad = true;

    public static int s_MaxMemoryUse = 300;

    public static int s_MaxHeapMemoryUse = 80;

    public static GUIStyle bb;

    private static int s_loadCount = 0;

    private static bool isLoading = false;

    private static List<string> s_LoadList = new List<string>();

    private static LoadProgressCallBack s_loadCallBack;

    private static LoadState s_loadStatus = new LoadState();

    private static bool s_isFreeMemory = false;

    private static bool s_isFreeMemory2 = false;

    private static bool s_isFreeHeapMemory = false;

    private static bool s_isFreeHeapMemory2 = false;

    public static void Init()
    {
        ApplicationManager.s_OnApplicationUpdate = (ApplicationVoidCallback)Delegate.Combine(ApplicationManager.s_OnApplicationUpdate, new ApplicationVoidCallback(Update));

        bb = new GUIStyle();
        bb.normal.background = null;
        bb.normal.textColor = new Color(1f, 0.5f, 0f);
        bb.fontSize = 30;
    }

    public static void Update()
    {
        LoadResources();
        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            MonitorMemorySize();
        }
        if (Application.platform == RuntimePlatform.WindowsEditor && UnityEngine.Input.GetKeyDown(KeyCode.F3))
        {
            FreeMemory();
        }
    }

    private static void GUI()
    {
    }

    public static void FreeMemory()
    {
        GlobalEvent.DispatchEvent(MemoryEvent.FreeMemory);
        GameObjectManager.CleanPool_New();
        UIManager.DestroyAllHideUI();
        FreeHeapMemory();
    }

    public static void FreeHeapMemory()
    {
        GlobalEvent.DispatchEvent(MemoryEvent.FreeHeapMemory);
        DataManager.CleanCache();
        ConfigManager.CleanCache();
        RecordManager.CleanCache();
    }

    public static void LoadRes(List<string> resList, LoadProgressCallBack callBack)
    {
        if (ResourceManager.m_gameLoadType != 0)
        {
            s_loadCallBack = (LoadProgressCallBack)Delegate.Combine(s_loadCallBack, callBack);
            s_LoadList.AddRange(resList);
            s_loadCount += resList.Count;
        }
        else
        {
            callBack(LoadState.CompleteState);
        }
    }

    public static void UnLoadRes(List<string> resList)
    {
        if (ResourceManager.m_gameLoadType != 0)
        {
            for (int i = 0; i < resList.Count; i++)
            {
                ResourceManager.UnLoad(resList[i]);
            }
        }
    }

    private static void LoadResources()
    {
        if (isLoading)
        {
            return;
        }
        if (s_LoadList.Count == 0)
        {
            if (s_loadCallBack != null)
            {
                try
                {
                    s_loadCallBack(LoadState.CompleteState);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError("Load Finsih CallBack Error : " + ex.ToString());
                }
                s_loadCallBack = null;
                s_loadCount = 0;
            }
        }
        else
        {
            isLoading = true;
            ResourceManager.LoadAsync(s_LoadList[0], LoadResourcesFinishCallBack);
            s_LoadList.RemoveAt(0);
            s_loadStatus.isDone = false;
            s_loadStatus.progress = 1f - (float)s_LoadList.Count / (float)s_loadCount;
            try
            {
                if (s_loadCallBack != null)
                {
                    s_loadCallBack(s_loadStatus);
                }
            }
            catch (Exception ex2)
            {
                UnityEngine.Debug.LogError("Load Finsih CallBack Error : " + ex2.ToString());
            }
        }
    }

    private static void LoadResourcesFinishCallBack(LoadState state, object res)
    {
        if (state.isDone)
        {
            isLoading = false;
        }
    }

    private static void MonitorMemorySize()
    {
        if (ByteToM(Profiler.GetTotalReservedMemoryLong()) > (float)s_MaxMemoryUse * 0.7f)
        {
            if (!s_isFreeMemory)
            {
                s_isFreeMemory = true;
                FreeMemory();
            }
            if (ByteToM(Profiler.GetMonoHeapSizeLong()) > (float)s_MaxMemoryUse)
            {
                if (!s_isFreeMemory2)
                {
                    s_isFreeMemory2 = true;
                    FreeMemory();
                    UnityEngine.Debug.LogError("Total memory exceeded alarm ！Current total memory usage： " + ByteToM(Profiler.GetTotalAllocatedMemoryLong()) + "M");
                }
            }
            else
            {
                s_isFreeMemory2 = false;
            }
        }
        else
        {
            s_isFreeMemory = false;
        }
        if (ByteToM(Profiler.GetMonoUsedSizeLong()) > (float)s_MaxHeapMemoryUse * 0.7f)
        {
            if (!s_isFreeHeapMemory)
            {
                s_isFreeHeapMemory = true;
            }
            if (ByteToM(Profiler.GetMonoUsedSizeLong()) > (float)s_MaxHeapMemoryUse)
            {
                if (!s_isFreeHeapMemory2)
                {
                    s_isFreeHeapMemory2 = true;
                    UnityEngine.Debug.LogError("Heap memory over-standard alarm ！Current heap memory usage： " + ByteToM(Profiler.GetMonoUsedSizeLong()) + "M");
                }
            }
            else
            {
                s_isFreeHeapMemory2 = false;
            }
        }
        else
        {
            s_isFreeHeapMemory = false;
        }
    }

    private static float ByteToM(long byteCount)
    {
        return (float)byteCount / 1048576f;
    }
}
