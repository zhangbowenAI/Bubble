
using System;
using System.Collections.Generic;

using UnityEngine;

public class AndroidManager : MonoBehaviour
{
    public static AndroidManager Instance;

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        GameObject gameObject = new GameObject("AndroidManager");
        AndroidManager androidManager = Instance = gameObject.AddComponent<AndroidManager>();
        DontDestroyOnLoad(gameObject);

    }


    public void Pay(string skuID)
    {
        Debug.Log("=====> Pay:" + skuID);

        // AGame.MyIAP.me.Buy(skuID, () =>
        // {
        //     PaySuccessCallBack(skuID);
        // }, (string failMsg) =>
        // {
        //     Debug.Log("=====> Pay failed:" + failMsg);
        //     PayFailureCallBack(skuID);
        // }

        // );

    }

    public void PayFailureCallBack(string skuID)
    {
        UnityEngine.Debug.Log("Failure  skuID: " + skuID);
    }

    public void PaySuccessCallBack(string skuID)
    {
        UnityEngine.Debug.Log("Success  skuID: " + skuID);
        string contentByKeyAndType = Singleton<DataPurchase>.Instance.GetContentByKeyAndType("Lives5", PurchaseType.GooglePay);

        string contentByKeyAndType2 = Singleton<DataPurchase>.Instance.GetContentByKeyAndType("Twohourslivs", PurchaseType.GooglePay);

        string contentByKeyAndType3 = Singleton<DataPurchase>.Instance.GetContentByKeyAndType("Piggybank", PurchaseType.GooglePay);

        if (skuID == contentByKeyAndType)
        {
            List<RewardType> list = new List<RewardType>();
            list.Add(new RewardType(1, 5f));
            UIManager.Reward(list);
        }
        else if (skuID == contentByKeyAndType2)
        {
            List<RewardType> list2 = new List<RewardType>();
            list2.Add(new RewardType(11, 1f));
            UIManager.Reward(list2);
        }
        else if (skuID == contentByKeyAndType3)
        {
            RecordManager.SaveRecord("UserData", "UserIsPay", 1);
            if (jinzuUIWindow.Instance != null && jinzuUIWindow.Instance.windowStatus == UIWindowBase.WindowStatus.Open)
            {
                // jinzuUIWindow.Instance.PayBack();
            }
            else
            {
                UIManager.OpenUIAsync<jinzuUIWindow>(delegate (UIWindowBase ui, object[] objs)
                {
                    // ui.GetComponent<jinzuUIWindow>().PayBack();
                }, new object[0]);
            }
        }
        else
        {
            PayReward(skuID);
        }
    }

    public void PayReward(string skuID)
    {
        for (int i = 0; i < 12; i++)
        {
            string contentByKeyAndType = Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), i), PayGoldType.GooglePay);

            if (!(contentByKeyAndType == skuID))
            {
                continue;
            }
            float num = 0f;
            List<RewardType> list = new List<RewardType>();
            int num2 = int.Parse(Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), i), PayGoldType.iGold));
            float num3 = float.Parse(Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), i), PayGoldType.ilove));
            string contentByKeyAndType2 = Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), i), PayGoldType.iMoney);
            string s = contentByKeyAndType2.Replace("$", string.Empty);
            num = float.Parse(s);
            string contentByKeyAndType3 = Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), i), PayGoldType.Package);
            if (num > 2f)
            {
                RecordManager.SaveRecord("UserData", "UserIsPay", 1);
            }
            if (num3 > 0f)
            {
                list.Add(new RewardType(9, num3));
            }
            if (num2 > 0)
            {
                list.Add(new RewardType(2, num2));
            }
            string[] array = contentByKeyAndType3.Split('|');
            for (int j = 0; j < array.Length; j++)
            {
                if (array[j].Split('_').Length > 1)
                {
                    string s2 = array[j].Split('_')[1];
                    string s3 = array[j].Split('_')[0];
                    list.Add(new RewardType(int.Parse(s3), int.Parse(s2)));
                }
            }
            UIManager.Reward(list);
        }
    }

    public void UpLevelData(string State)
    {
        int initBubbleCount = Singleton<LevelManager>.Instance.InitBubbleCount;
        string text = "LogType:LevelLog|";
        string text2 = text;
        text = text2 + "ThisLevelId:" + Singleton<LevelManager>.Instance.GetNowLevel() + "|";
        text = text + "LevelState:" + State + "|";
        text = text + "ResTime:" + RecordManager.GetStringRecord("UserData", "RegTime", "19900101") + "|";
        text2 = text;
        text = text2 + "NowMoveCount:" + GameScene.Instance.MoveNum + "|";
        text = text + "MoveCount:" + (Singleton<LevelManager>.Instance.AllBubbleCount - GameScene.Instance.MoveNum);
        UpdataALY(text);
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string text3 = "LogType:LevelLog|";
            text2 = text3;
            text3 = text2 + "ThisLevelId:" + Singleton<LevelManager>.Instance.GetNowLevel() + "|";
            text3 = text3 + "LevelState:" + State + "|";
            text2 = text3;
            text3 = text2 + "NowMoveCount:" + GameScene.Instance.MoveNum + "|";
            text3 = text3 + "MoveCount:" + (Singleton<LevelManager>.Instance.AllBubbleCount - GameScene.Instance.MoveNum);

        }
    }

    public string GetVersionCode()
    {
        try
        {
            return Application.version;
        }
        catch
        {
            return "1.0.0";
        }

    }

    public void ShowInterstitialAd()
    {
        Debug.Log("====ShowInterstitialAd==== iNowPassLevelID    " + Singleton<UserData>.Instance.GetPassLevel());

        // xxxxxxxxxxxxx for test, correct value is 5
        if (Singleton<UserData>.Instance.GetPassLevel() < 5)
        { // 10
            return;
        }
        // end

        if (RecordManager.GetIntRecord("UserData", "UserIsPay", 0) == 1)
        {
            return;
        }

        // AGame.MySdkManager.me.ShowFullscreenAds();
    }

    public void StartLevel(int level)
    {
        Debug.Log("=====> StartLevel:" + level);
    }

    public void FinishLevel(int level)
    {
        Debug.Log("=====> FinishLevel:" + level);
    }

    public void FailLevel(int level)
    {
        Debug.Log("=====> FailLevel:" + level);
    }

    public void AddAdjustEvent(string eventid)
    {

    }

    public void ShowVideoAd(string zoneId)
    {

        // Debug.Log("=====> ShowVideoAd:" + zoneId);
        // if (!AGame.MySdkManager.me.hasRewardAds)
        // {
        //     Debug.Log("there is no video ads right now");
        //     return;
        // }
        // RecordManager.SaveRecord("UserData", "InterstitialLastTime", Util.GetNowTime());

        // AGame.MySdkManager.me.ShowRewardAds(() =>
        // {
        //     Instance.VideoAdCallBack(zoneId);
        // });

    }

    public int GetCDK()
    {
        return 0;

    }

    public string GetIP()
    {
        Debug.Log("=====> GetIP");
        return "";

    }

    public void VideoAdCallBack(string zoneId)
    {

        UnityEngine.Debug.Log("=====>  VideoAdCallBack " + zoneId);
        if (zoneId == null)
        {
            return;
        }
        if (!(zoneId == "PlayVideo"))
        {
            if (!(zoneId == "BuyBubbleMove"))
            {
                if (!(zoneId == "WinFreeVideo"))
                {
                    if (!(zoneId == "LotterVideo"))
                    {
                        if (!(zoneId == "HalfHourFreeLivesVideo"))
                        {
                            if (zoneId == "WinChestVideo" && null != AdChestWindow.Instance)
                            {
                                // AdChestWindow.Instance.AdVideoCallback();
                            }
                            return;
                        }
                        string nowTime_Day = Util.GetNowTime_Day();
                        int intRecord = RecordManager.GetIntRecord("UserData", "HalfHourFreeLivesVideoCount" + nowTime_Day, 0);
                        intRecord++;
                        RecordManager.SaveRecord("UserData", "HalfHourFreeLivesVideoCount" + nowTime_Day, intRecord);
                        RecordManager.SaveRecord("UserData", "HalfHourFreeLivesVideoTick" + nowTime_Day, Util.GetNowTime());
                        List<RewardType> list = new List<RewardType>();
                        list.Add(new RewardType(10, 1f));
                        UIManager.Reward(list);

                    }
                    else
                    {
                        if ((bool)LotterUIWindow.Instance)
                        {
                            LotterUIWindow.Instance.ActivateTheLuckyDraw();
                        }

                    }
                }
                else
                {
                    // GameScene.Instance.WinPanel.GetComponent<WinPanelUI>().WatchVideoCallBack();

                }
            }
            else
            {
                // BuyBubbleMoveNumWindow.Instance.WatchVideoBack();

            }
        }
        else
        {

            if (RecordManager.GetIntRecord("UserData", "UserIsPay", 0) == 1)
            {
                string nowTime_Day2 = Util.GetNowTime_Day();
                int intRecord2 = RecordManager.GetIntRecord("UserData", "PlayCount" + nowTime_Day2, 0);
                intRecord2++;
                RecordManager.SaveRecord("UserData", "PlayCount" + nowTime_Day2, intRecord2);
            }
            UIManager.CloseUIWindow<PlayWindow>(isPlayAnim: false, null, new object[0]);
            GameEntry.ChangeScene(GameEntry.GameScene);
            Singleton<UserData>.Instance.PlaySkillUseViedo = true;

        }
    }

    public void UpdataALY(string str)
    {
        Debug.Log("=====> UpdataALY:" + str);

    }
}
