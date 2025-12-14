using System;
using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;


//广告类型
public enum ADType
{
    RewardVideoAD,          //激励视频广告点
    SceneVideoAD,           //场景视频广告点
    CPAD,                   //插屏广告点
    RedGift,                //红包弹出点
    NativeStartAD,          //嵌入式原生广告点展示
    NativeEndAD,             //嵌入式原生广告点关闭
    ShowBanner,  //横幅广告点展示
    HideBanner     //横幅广告点关闭
}

//广告位置
//梦幻泡泡龙（竖屏）
public enum ADSpot
{
    WinFreeVideo,//胜利界面
    PlayVideo,//开始界面
    HalfHourFreeLivesVideo,//能量
    LotterVideo,//转盘
    BuyBubbleMove,//复活
    WinChestVideo,//礼盒
    Piggybank,//存钱罐
    GetCoins,//商店领取金币
    SignDay,//签到
    GetPackage,//新增礼包
    OpenBox,
    RefreshShop,
    ExitReward,


    //插屏
    MainPanel,
    SignPanel,
    LotteryPanel,
    PiggyPanel,
    BoxPanel,
    StartGame,
    PauseGame,
    BuyDiscounShop,
    //场景点
    GameEnd,
    NormalSign,
    GetMission,

    GameBanner,

    BuyLoveNaticeAd,
    BuyGoldNaticeAd,
    BoxPanelNaticeAd,
    SettingNaticeAd,
    ExitPanel,
}

public class ADInterface : MonoSingletonBase<ADInterface>
{
    private float invokeTime = 0;
    private bool isTick = false;


    static AndroidJavaClass androidJC;
    static AndroidJavaObject andoridJO;
    public override void Init()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "ADInterface";
        Debug.Log("ADinterface - init");
    }

#if UNITY_IOS
    /// <summary>
    /// 发送广告点
    /// </summary>
    /// <param name="adType">广告类型</param>
    /// <param name="adSpot">广告位置</param>
    [DllImport("__Internal")]
    private static extern void SendEvent(string adType, string adSpot);

    /// <summary>
    /// 调用IOS提示框
    /// </summary>
    /// <param name="title">提示框标题</param>
    /// <param name="message">提示框内容</param>
    [DllImport("__Internal")]
    private static extern void ShowAlertView(string title, string message);
#endif

    /// <summary>
    /// 无回调广告
    /// </summary>
    /// <param name="adType">广告类型</param>
    /// <param name="adSpot">广告位置</param>
    public void SendADEvent(ADType adType, ADSpot adSpot)
    {
        string info = "SendADEvent -- 广告类型：" + adType.ToString() + "    广告位置：" + adSpot.ToString();
        //Debug.Log(info);
        ShowToast(info);

#if UNITY_EDITOR_OSX || UNITY_EDITOR
        return;
#elif UNITY_ANDROID
        try
        {
            if (androidJC == null)
                androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            if (andoridJO == null)
                andoridJO = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
            andoridJO.Call("ADEvent", adType.ToString(), adSpot.ToString());
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
#elif UNITY_IOS
    SendEvent(adType.ToString(), adSpot.ToString());
#endif
    }


    public Action<bool> adFinishCallBack = null;

    /// <summary>
    /// 播放广告 
    /// </summary>
    /// <param name="adType">广告类型</param>
    /// <param name="adSpot">广告位置</param>
    /// <param name="action">Action</param>
    public void SendADVideoEvent(ADType adType, ADSpot adSpot, Action<bool> action = null)
    {
        Debug.Log(adType.ToString() + "  " + adSpot.ToString());
        adFinishCallBack = null; /*清除之前的回调函数*/

        if (action != null)
        {
            adFinishCallBack = action;
        }

        if (BuildSetting.Instance.adDefaultEnable)
        {
            FinishAdvertisement();
            return;
        }
#if UNITY_EDITOR
        invokeTime = 1;
        isTick = true;
#elif UNITY_ANDROID || UNITY_IOS
        SendADEvent(adType, adSpot);
#endif
    }


    //调用Toast
    static AndroidJavaClass Toast;
    public static void ShowToast(string info)
    {
        if (!BuildSetting.Instance.isUnityShowToast)
            return;

#if UNITY_EDITOR_OSX || UNITY_EDITOR
        Debug.Log(info);
        return;
#elif UNITY_IOS
    ShowAlertView("提示", info);
#elif UNITY_ANDROID
        if (androidJC == null)
            androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        if (andoridJO == null)
            andoridJO = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
        if (Toast == null)
            Toast = new AndroidJavaClass("android.widget.Toast");

        andoridJO.Call("runOnUiThread", new AndroidJavaRunnable(() =>
      {
          Toast.CallStatic<AndroidJavaObject>("makeText", andoridJO, info, Toast.GetStatic<int>("LENGTH_LONG")).Call("show");
      }));
#endif
    }

    /// <summary>
    /// 广告失败回调函数
    /// </summary>
    public void FailAdvertisement()
    {

        Debug.Log("FailAdvertisement");
        if (adFinishCallBack != null)
        {
            adFinishCallBack(false);
            adFinishCallBack = null;
        }
    }

    /// <summary>
    /// Finishs the advertisement.
    /// 播放完广告的回调函数
    /// </summary>
    public void FinishAdvertisement()
    {
        Debug.Log("FinishAdvertisement");
        if (adFinishCallBack != null)
        {
            adFinishCallBack(true);
            adFinishCallBack = null;
        }
    }

#if UNITY_EDITOR
    void Update()
    {
        if (isTick)
        {
            invokeTime -= Time.unscaledDeltaTime;
            if (invokeTime < 0)
            {
                FinishAdvertisement();
                isTick = false;
            }
        }
    }
#endif

}