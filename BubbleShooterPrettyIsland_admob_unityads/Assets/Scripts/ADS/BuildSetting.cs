using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class BuildSetting : ScriptableObject
{
#if UNITY_EDITOR
    [MenuItem("自定义菜单/编译前设置/测试包")]
    public static void TestBuildSetting()
    {
        BuildSetting.Instance.adEnable = true;
        BuildSetting.Instance.adDefaultEnable = true;
        BuildSetting.Instance.showCompanyInfo = true;
        BuildSetting.Instance.showCompanyOtherInfo = true;
        BuildSetting.Instance.showAllCompanyInfo = true;
        BuildSetting.Instance.showMoreGame = true;
        BuildSetting.Instance.isUnityShowToast = true;
        Debug.Log("测试包编译完成！");
    }

    [MenuItem("自定义菜单/编译前设置/OV测评包")]
    public static void OVBuildSetting()
    {
        BuildSetting.Instance.adEnable = true;
        BuildSetting.Instance.adDefaultEnable = true;
        BuildSetting.Instance.showCompanyInfo = true;
        BuildSetting.Instance.showCompanyOtherInfo = true;
        BuildSetting.Instance.showAllCompanyInfo = true;
        BuildSetting.Instance.showMoreGame = false;
        BuildSetting.Instance.isUnityShowToast = false;
        Debug.Log("OV测评包设置编译完成！");

    }

    [MenuItem("自定义菜单/编译前设置/工程包")]
    public static void AndroidBuildSetting()
    {
        BuildSetting.Instance.adEnable = true;
        BuildSetting.Instance.adDefaultEnable = false;
        BuildSetting.Instance.showCompanyInfo = false;
        BuildSetting.Instance.showCompanyOtherInfo = false;
        BuildSetting.Instance.showAllCompanyInfo = false;
        BuildSetting.Instance.showMoreGame = false;
        BuildSetting.Instance.isUnityShowToast = false;
        Debug.Log("工程包设置编译完成！");
    }
    [MenuItem("自定义菜单/清除数据")]
    public static void ClearData()
    {
        PlayerPrefs.DeleteAll();
        RecordManager.CleanAllRecord();
        NewFileTool.DelectFile("PlayerData");
        Debug.Log("清除本地数据完成！");
    }
#endif

    private static BuildSetting _instance = null;

    public static BuildSetting Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<BuildSetting>("BuildSetting");
            }
            return _instance;
        }
    }

    public Action<bool> IsShowCopyrightEvent;
    public Action<bool> IsShowCompanyOtherInfoEvent;
    public Action<bool> IsShowAllCompanyInfoEvent;
    public void EventChange()
    {
        if (IsShowCopyrightEvent != null)
            IsShowCopyrightEvent(this.showCompanyInfo);
        if (IsShowCompanyOtherInfoEvent != null)
            IsShowCompanyOtherInfoEvent(this.showCompanyOtherInfo);
        if (IsShowAllCompanyInfoEvent != null)
            IsShowAllCompanyInfoEvent(this.showAllCompanyInfo);
    }

    [Header("广告开关")]
    public bool adEnable = true;

    [Header("广告默认通过开关，评测用")]
    public bool adDefaultEnable = false;

    [Header("*显示出版单位、著作权人")]
    public bool showCompanyInfo = false;

    [Header("显示版号、审批文号等")]
    public bool showCompanyOtherInfo = false;

    [Header("显示公司相关信息")]
    public bool showAllCompanyInfo = false;

    [Header("*语种")]
    public LanguageType language = LanguageType.CN;

    [Header("*超休闲游戏oppo按钮")]
    public bool showMoreGame = false;

    [Header("显示Toast")]
    public bool isUnityShowToast = true;

    [Header("是否VIVO渠道")]
    public bool isVivo = false;

    [Header("渠道广告方案")]
    public AdChannelsType adChannel = AdChannelsType.VIVO;
}

public enum AdChannelsType
{
    VIVO = 1,
    OPPO = 2,
}