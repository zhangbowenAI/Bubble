using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSetting : MonoSingletonBase<PlatformSetting>
{

    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "PlatformSetting";

        Debug.Log("Platform - init");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.OpenUIWindow<ExitWindow>();
            // ExitGame();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PlayerData.Instance.SaveData();
        }
        else
        {
        }
    }

    private void OnApplicationQuit()
    {
        PlayerData.Instance.SaveData();
    }



    #region 接口

    /// <summary>
    /// 设置所有广告开关
    /// </summary>
    /// <param name="result">Result.</param>
    public void SetAllAdsEnable(string result)
    {
        bool state = bool.Parse(result);
        print("SetAllAdsEnable: " + state.ToString());
        BuildSetting.Instance.adEnable = state;
    }

    public void SetAdsDefaultEnable(string result)
    {
        bool state = bool.Parse(result);
        print("SetAdsDefaultEnable: " + state.ToString());
        BuildSetting.Instance.adDefaultEnable = state;
    }


    /// <summary>
    /// 超休闲游戏oppo按钮
    /// OPPO专属的超休闲游戏“更多精彩”按钮显示/隐藏
    /// </summary>
    public void SetIsShowMoreGame(string result)
    {
        print("SetIsShowMoreGame:" + result);
        bool flag = bool.Parse(result);
        BuildSetting.Instance.showMoreGame = flag;
    }

    /// <summary>
    /// 设置是否显示出版单位、著作权人
    /// </summary>
    public void IsShowCompanyInfo(string result)
    {
        print("IsShowCompanyInfo:" + result);
        bool flag = bool.Parse(result);
        BuildSetting.Instance.showCompanyInfo = flag;
        BuildSetting.Instance.EventChange();
    }

    /// <summary>
    /// 设置是否显示版号、审批文号等
    /// </summary>
    public void IsShowCompanyOtherInfo(string result)
    {
        print("IsShowCompanyOtherInfo:" + result);
        bool flag = bool.Parse(result);
        BuildSetting.Instance.showCompanyOtherInfo = flag;
        BuildSetting.Instance.EventChange();
    }

    /// <summary>
    /// 设置是否显示公司相关信息
    /// </summary>
    public void IsShowAllCompanyInfo(string result)
    {
        print("IsShowAllCompanyInfo:" + result);
        bool flag = bool.Parse(result);
        BuildSetting.Instance.showAllCompanyInfo = flag;
        BuildSetting.Instance.EventChange();
    }

    /// <summary>
    /// 设置是否调用Toast
    /// </summary>
    public void IsShowUnityToast(string result)
    {
        print("isUnityShowToast:" + result);
        bool flag = bool.Parse(result);
        BuildSetting.Instance.isUnityShowToast = flag;
    }

    /// <summary>
    /// 设置渠道广告方案
    /// 1-VIVO   2-OPPO
    /// </summary>
    public void SetAdChannel(string result)
    {
        print("SetAdCannel:" + result);
        int flag = int.Parse(result);
        BuildSetting.Instance.adChannel = (AdChannelsType)flag;
    }

    #endregion



    #region  Android SDK 接口

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("ExitGame");

#if UNITY_EDITOR
        Application.Quit();
        return;
#endif

#if UNITY_ANDROID
        AndroidJavaClass androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject androidJO = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
        androidJO.Call("OnExitGame");
#endif
    }


    /// <summary>
    /// 超休闲游戏oppo按钮
    /// </summary>
    public void MoreGame()
    {
        Debug.Log("MoreGame");

#if UNITY_EDITOR
        return;
#endif

#if UNITY_ANDROID
        AndroidJavaClass androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject androidJO = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
        androidJO.Call("MoreGame");
#endif
    }

    #endregion

}