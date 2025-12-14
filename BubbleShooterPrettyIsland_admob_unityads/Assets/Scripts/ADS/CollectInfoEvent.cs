using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 自定义事件发送.
/// </summary>
public class CollectInfoEvent
{

    //自定义事件的发送配置
    static bool Is_Umeng_Event = true;       //友盟统计

    public enum EventType
    {
        FirstStart,//首次进入游戏
        FirstMainPage,//首次进入主界面
        GameClear,//通过关卡
        MissionClear,//完成任务
        SignUp,//普通签到
        BuyItem,//优惠商店购买道具
        GetBoxReward,//领取宝箱奖励
        GameOver,//关卡失败
        ClearLevel_3,//通过关卡3
        ClearLevel_12,//通过关卡12
        AD_GetItems,//开始关卡前获取免费道具
        AD_Spin,//转动转盘
        AD_OpenBox,//提前打开宝箱
        AD_SignUp,//三倍签到
        AD_GetMoneyPack,//领取金币礼包
        AD_GetItemPack,//领取道具礼包
        AD_GetMoney600,//商店获取600金币
        AD_GetMoney1200,//商店获取1200金币
        AD_RefreshShop,//刷新优惠商店
        AD_GetExitReward,//领取退出奖励
        AD_GetDepositMoney,//领取存钱罐奖励
    }


#if UNITY_ANDROID

    static AndroidJavaClass ajc;
    static AndroidJavaObject ajo;

#endif

    /// <summary>
    /// 此方法用于后面跟着计数值，比如Level_1这种事件就用此方法。.
    /// </summary>
    /// <param name="eventType">Event type.</param>
    /// <param name="event_params">Event_params.</param>
    public static void SendEventWithCount(EventType eventType, string count, params string[] event_params)
    {
        SendEvent(eventType.ToString() + count, event_params);
    }

    /// <summary>
    /// <para>此方法用于正常事件发送，比如SignIn.</para>
    /// <para>如果事件类型后缀为下划线，请用<see cref="SendEventWithCount"/></para>
    /// </summary>
    /// <param name="eventType">Event type.</param>
    /// <param name="event_params">Event_params.</param>
    public static void SendEvent(EventType eventType, params string[] event_params)
    {
        SendEvent(eventType.ToString(), event_params);
    }


    private static void SendEvent(string eventName, params string[] event_params)
    {
#if UNITY_EDITOR || UNITY_EDITOR_OSX
        //事件属性处理
        System.Text.StringBuilder key_value_str = new System.Text.StringBuilder();
        if (event_params != null && event_params.Length != 0)
        {
            for (int i = 0; i < event_params.Length; ++i)
            {
                if (i % 2 == 0)
                    key_value_str.Append(event_params[i]).Append("-").Append(event_params[i + 1]);
                else if (i != (event_params.Length - 1))
                    key_value_str.Append("|");
            }
        }
        else
        {
            key_value_str.Append("");
        }
        Debug.Log(eventName + " : " + key_value_str.ToString());
        return;
#endif

#if UNITY_IOS
			return;
#endif

#if UNITY_ANDROID
        try
        {
            if (ajc == null)
            {
                ajc = new AndroidJavaClass("com.qiyimao.UMEvent");
                ajo = ajc.CallStatic<AndroidJavaObject>("getInstance");
            }

            //事件属性处理
            System.Text.StringBuilder key_value = new System.Text.StringBuilder();
            if (event_params != null && event_params.Length != 0)
            {
                for (int i = 0; i < event_params.Length; ++i)
                {
                    if (i % 2 == 0)
                        key_value.Append(event_params[i]).Append("-").Append(event_params[i + 1]);
                    else if (i != (event_params.Length - 1))
                        key_value.Append("|");
                }
            }
            else
            {
                key_value.Append("");
            }

            //友盟统计
            if (Is_Umeng_Event)
            {
                if (ajo != null)
                {
                    ajo.Call("SendEvent", eventName, key_value.ToString());
                }
            }
        }
        catch
        {
            Debug.Log("SendEvent Fail");
        }
#endif
    }

    public static void StartLevel(int level)
    {
        Debug.Log("开始关卡：" + level);
#if UNITY_EDITOR || UNITY_EDITOR_OSX

        return;
#endif

#if UNITY_IOS
		return;
#endif

#if UNITY_ANDROID
        try
        {
            if (ajc == null)
            {
                ajc = new AndroidJavaClass("com.qiyimao.UMEvent");
                ajo = ajc.CallStatic<AndroidJavaObject>("getInstance");
            }

            if (ajo != null)
            {
                ajo.Call("StartLevel", level.ToString());
            }
        }
        catch
        {
            Debug.Log("Send Start Level Info Fail");
        }
#endif
    }

    public static void FailLevel(int level)
    {
        Debug.Log("失败关卡：" + level);
#if UNITY_EDITOR || UNITY_EDITOR_OSX

        return;
#endif

#if UNITY_IOS
		return;
#endif

#if UNITY_ANDROID
        try
        {
            if (ajc == null)
            {
                ajc = new AndroidJavaClass("com.qiyimao.UMEvent");
                ajo = ajc.CallStatic<AndroidJavaObject>("getInstance");
            }

            if (ajo != null)
            {
                ajo.Call("FailLevel", level.ToString());
            }
        }
        catch
        {
            //			Debug.Log("Send Fail Level Info Fail");
        }
#endif
    }

    public static void FinishLevel(int level)
    {
        Debug.Log("完成关卡：" + level);
#if UNITY_EDITOR || UNITY_EDITOR_OSX

        return;
#endif

#if UNITY_IOS
		return;
#endif

#if UNITY_ANDROID
        try
        {
            if (ajc == null)
            {
                ajc = new AndroidJavaClass("com.qiyimao.UMEvent");
                ajo = ajc.CallStatic<AndroidJavaObject>("getInstance");
            }

            if (ajo != null)
            {
                ajo.Call("FinishLevel", level.ToString());
            }
        }
        catch
        {
            //			Debug.Log("Send Finish Level Info Fail");
        }
#endif
    }

    public static void PayEvent(float price, float coin, int source)
    {
        Debug.Log("PayEvent：" + price);
#if UNITY_EDITOR || UNITY_EDITOR_OSX

        return;
#endif

#if UNITY_IOS
		return;
#endif

#if UNITY_ANDROID
        try
        {
            if (ajc == null)
            {
                ajc = new AndroidJavaClass("com.qiyimao.UMEvent");
                ajo = ajc.CallStatic<AndroidJavaObject>("getInstance");
            }

            if (ajo != null)
            {
                ajo.Call("PayEvent", price, coin, source);
            }
        }
        catch
        {
            //			Debug.Log("Send Finish Level Info Fail");
        }
#endif
    }

    public static void PayEvent(float money, string item, int number, float price, int source)
    {
        Debug.Log("PayEvent：" + money + " " + item + " " + number + " " + price + " " + source);
#if UNITY_EDITOR || UNITY_EDITOR_OSX

        return;
#endif

#if UNITY_IOS
        return;
#endif

#if UNITY_ANDROID
        try
        {
            if (ajc == null)
            {
                ajc = new AndroidJavaClass("com.qiyimao.UMEvent");
                ajo = ajc.CallStatic<AndroidJavaObject>("getInstance");
            }

            if (ajo != null)
            {
                ajo.Call("PayEvent", money, item, number, price, source);
            }
        }
        catch
        {
            //          Debug.Log("Send Finish Level Info Fail");
        }
#endif
    }

    public static void UseProp(string prop, int amount, double price, int level, int userLevel)
    {
        Debug.Log("使用道具：" + prop + ", 道具数量：" + amount + ", 道具单价：" + price + ", 关卡名称：" + level + ", 用户等级：" + userLevel);
#if UNITY_EDITOR || UNITY_EDITOR_OSX

        return;
#endif

#if UNITY_IOS
		return;
#endif

#if UNITY_ANDROID
        try
        {
            if (ajc == null)
            {
                ajc = new AndroidJavaClass("com.qiyimao.UMEvent");
                ajo = ajc.CallStatic<AndroidJavaObject>("getInstance");
            }

            if (ajo != null)
            {
                ajo.Call("UseProp", prop, amount, price, level, userLevel);
            }
        }
        catch
        {
            Debug.Log("Send UseProp Info Fail");
        }
#endif
    }

}
