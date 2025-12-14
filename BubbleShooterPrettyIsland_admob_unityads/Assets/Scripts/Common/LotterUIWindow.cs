
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class LotterUIWindow : UIWindowBase
{
    public static LotterUIWindow Instance;

    public Text LotterText;

    public GameObject[] LotterIconObj;

    private string sTime = string.Empty;

    public GameObject WatchVideo;

    public Text TimeText;

    public Text WatchText;

    public Text TitleText;

    public GameObject WatchImage;

    public Sprite WatchSprite;

    public Sprite WatchSpriteHui;

    public GameObject YuanPan;

    private bool IsCanShow;

    private int iOverResult = -1;

    private bool bFloat = true;

    private bool IsWatchClick;

    private int Resutl;

    private int iRewardID;

    private int iRewardInumber;

    private int iOverResultiRewardID = 1;

    private int iOverResultiRewardCount = 1;

    private int _iNowZhuanGetTime;

    public bool isTimeOver;

    public override void OnOpen()
    {
        InitUI();
        AddOnClickListener("CloseButton", OnClickClose);
        AddOnClickListener("WatchVideo", OnWatchClick);
        ADInterface.Instance.SendADEvent(ADType.CPAD, ADSpot.LotteryPanel);
    }

    public void InitUI()
    {
        ResLotteryUI();
        Instance = this;
        IsCanShow = true;
        IsWatchClick = false;
        isTimeOver = false;
        bFloat = true;
        YuanPan.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        int intRecord = RecordManager.GetIntRecord("UserData", "UserIsPay", 0);
        sTime = Util.GetNowTime_Day();
        int intRecord2 = RecordManager.GetIntRecord("UserData", "LotterCount" + sTime, 0);
        _iNowZhuanGetTime = RecordManager.GetIntRecord("UserData", "LotterLastTime", 0);
        WatchVideo.SetActive(value: true);
        SetLotterTime();
        InitLanguage();
        if (intRecord == 1 && intRecord2 >= 3)
        {
            IsCanShow = false;
        }
        if (IsCanShow)
        {
            WatchVideo.SetActive(value: true);
        }
        else
        {
            WatchVideo.SetActive(value: false);
        }
    }

    public void InitLanguage()
    {
        WatchText.text = Util.ReplaceText(GameEntry.Instance.GetString("Turntablebutton"));
        LotterText.text = Util.ReplaceText(GameEntry.Instance.GetString("Turntableremark"));
        TitleText.text = Util.ReplaceText(GameEntry.Instance.GetString("Turntabletitle"));
    }

    public void OnClickClose(InputUIOnClickEvent e)
    {
        if (!IsWatchClick)
        {
            InitUI();
            UIManager.CloseUIWindow<LotterUIWindow>(isPlayAnim: true, null, new object[0]);

            if (!PlayerData.Instance.IsLotteryGuide && PlayerData.Instance.guideStep == 2)
            {
                PlayerData.Instance.IsLotteryGuide = true;
                Singleton<LevelManager>.Instance.SetNowSelectLevel(Singleton<UserData>.Instance.GetPassLevel() + 1);
                UIManager.OpenUIWindow<PlayWindow>();
                PlayerData.Instance.guideStep = 0;
            }
        }
    }

    public void OnWatchClick(InputUIOnClickEvent e)
    {
        if (!IsWatchClick)
        {
            // AndroidManager.Instance.ShowVideoAd("LotterVideo");
            ADInterface.Instance.SendADVideoEvent(ADType.RewardVideoAD, ADSpot.LotterVideo, ADCallback);
        }
    }

    private void ADCallback(bool result)
    {
        if (result)
        {
            CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.AD_Spin);
            ActivateTheLuckyDraw();
        }
    }

    public void ResLotteryUI()
    {
        if (GameEntry.buildType == GameEntry.BuildType.GoogleEN)
        {
            for (int i = 0; i < 10; i++)
            {
                int num = int.Parse(Singleton<Datazhuanpan>.Instance.GetContentByKeyAndType((zhuanpanKey)Enum.ToObject(typeof(zhuanpanKey), i), zhuanpanType.icon));
                int num2 = int.Parse(Singleton<Datazhuanpan>.Instance.GetContentByKeyAndType((zhuanpanKey)Enum.ToObject(typeof(zhuanpanKey), i), zhuanpanType.inumber));
                LotterIconObj[i].transform.Find("IconNum").GetComponent<Text>().text = "x" + num2.ToString();
                LotterIconObj[i].GetComponent<Image>().sprite = Util.GetResourcesSprite("IconImage/icon_" + num);
                LotterIconObj[i].GetComponent<Image>().SetNativeSize();
            }
        }
    }

    public void ActivateTheLuckyDraw()
    {
        WatchVideo.GetComponent<Button>().interactable = false;
        WatchVideo.GetComponent<Image>().sprite = WatchSpriteHui;
        AudioPlayManager.PlaySFX2D("ui_wheel_rot_start");
        startRotate();
    }

    private void startRotate()
    {
        bFloat = true;
        iOverResult = -1;
        IsWatchClick = true;
        ZPRotate();
        StartCoroutine(StopRotate());
    }

    private void Update()
    {
        SetLotterTime();
    }

    public void SetLotterTime()
    {
        if (isTimeOver)
        {
            return;
        }
        int nowTime = Util.GetNowTime();
        int num = nowTime - _iNowZhuanGetTime;
        int num2 = 1;
        if (num < num2)
        {
            int seconds = num2 - num;
            WatchVideo.GetComponent<Button>().interactable = false;
            WatchVideo.GetComponent<Image>().sprite = WatchSpriteHui;
            WatchImage.SetActive(value: false);
            TimeText.gameObject.SetActive(value: true);
            WatchVideo.SetActive(value: true);
            TimeSpan timeSpan = new TimeSpan(0, 0, seconds);
            int minutes = timeSpan.Minutes;
            int seconds2 = timeSpan.Seconds;
            string text = minutes + string.Empty;
            string text2 = seconds2 + string.Empty;
            if (minutes < 10)
            {
                text = "0" + text;
            }
            if (seconds2 < 10)
            {
                text2 = "0" + text2;
            }
            if (TimeText.gameObject.activeSelf)
            {
                TimeText.text = text + ":" + text2;
            }
        }
        else
        {
            isTimeOver = true;
            WatchVideo.GetComponent<Button>().interactable = true;
            WatchVideo.GetComponent<Image>().sprite = WatchSprite;
            WatchImage.SetActive(value: true);
            TimeText.gameObject.SetActive(value: false);
            WatchVideo.SetActive(value: true);
        }
    }

    private IEnumerator StopRotate()
    {
        yield return new WaitForSeconds(3f);
        int LotterCount2 = RecordManager.GetIntRecord("UserData", "LotterCount" + sTime, 0);
        for (Resutl = RandomQuanzhong(); Resutl == -1; Resutl = RandomQuanzhong())
        {
        }
        sTime = Util.GetNowTime_Day();
        if (LotterCount2 == 0 || Resutl == 0)
        {
            Resutl = 1;
        }
        if (Singleton<UserData>.Instance.GetUserLoveCount() <= 0 && Singleton<UserData>.Instance.getLoveInfinite() <= 0)
        {
            Resutl = 1;
        }
        iRewardID = Resutl;
        if (GameEntry.buildType == GameEntry.BuildType.GoogleEN)
        {
            iRewardInumber = int.Parse(Singleton<Datazhuanpan>.Instance.GetContentByKeyAndType((zhuanpanKey)Enum.ToObject(typeof(zhuanpanKey), iRewardID - 1), zhuanpanType.inumber));
            iRewardID = int.Parse(Singleton<Datazhuanpan>.Instance.GetContentByKeyAndType((zhuanpanKey)Enum.ToObject(typeof(zhuanpanKey), iRewardID - 1), zhuanpanType.icon));
        }
        iOverResult = Resutl;
        iOverResultiRewardID = iRewardID;
        iOverResultiRewardCount = iRewardInumber;
        bFloat = false;
        int iNowTime = Util.GetNowTime();
        LotterCount2++;
        RecordManager.SaveRecord("UserData", "LotterCount" + sTime, LotterCount2);
        RecordManager.SaveRecord("UserData", "LotterLastTime", iNowTime);
        _iNowZhuanGetTime = iNowTime;
    }

    private void ZPRotate()
    {
        if (bFloat)
        {
            YuanPan.transform.DOLocalRotate(new Vector3(0f, 0f, -360f), 1.6f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).OnComplete(delegate
            {
                ZPRotate();
            });
            return;
        }
        int num = iOverResult;
        float num2 = -36 * num;
        float num3 = (float)num * 1.6f / 360f * 36f;
        Sequence s = DOTween.Sequence();
        s.Append(YuanPan.transform.DOLocalRotate(new Vector3(0f, 0f, -360f + num2 + 36f - (float)UnityEngine.Random.Range(0, 15)), 2f + num3).SetEase(Ease.OutSine)).OnComplete(Reward);
        AudioPlayManager.PlaySFX2D("ui_wheel_rot_end_1");
    }

    private int RandomReward()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            int item = 0;
            if (GameEntry.buildType == GameEntry.BuildType.GoogleEN)
            {
                item = int.Parse(Singleton<Datazhuanpan>.Instance.GetContentByKeyAndType((zhuanpanKey)Enum.ToObject(typeof(zhuanpanKey), i), zhuanpanType.imax0));
            }
            list.Add(item);
        }
        int num = 0;
        for (int j = 0; j < list.Count; j++)
        {
            num += list[j] + 1;
        }
        System.Random random = new System.Random(GetRandomSeed());
        List<KeyValuePair<int, int>> list2 = new List<KeyValuePair<int, int>>();
        for (int k = 0; k < list.Count; k++)
        {
            int value = list[k] + 1 + random.Next(0, num);
            list2.Add(new KeyValuePair<int, int>(k, value));
        }
        list2.Sort((KeyValuePair<int, int> kvp1, KeyValuePair<int, int> kvp2) => kvp2.Value - kvp1.Value);
        return list2[0].Key;
    }

    private static int GetRandomSeed()
    {
        byte[] array = new byte[4];
        RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
        rNGCryptoServiceProvider.GetBytes(array);
        return BitConverter.ToInt32(array, 0);
    }

    public void Reward()
    {
        IsWatchClick = false;
        isTimeOver = false;
        if ((bool)LevelScene.Instance)
        {
            LevelScene.Instance.SetLotterTime();
        }
        List<RewardType> list = new List<RewardType>();
        list.Add(new RewardType(iOverResultiRewardID, iOverResultiRewardCount));
        UIManager.Reward(list);
        UnityEngine.Debug.Log("Lihai Lotter==== " + iOverResultiRewardID + "  " + Time.time);
    }

    private int RandomQuanzhong()
    {
        List<int> list = new List<int>();
        RecordManager.GetStringRecord("UserData", "ZhuanpanOldData" + sTime, string.Empty);
        string text = RecordManager.GetStringRecord("UserData", "ZhuanpanOldData" + sTime, string.Empty);
        if (text != string.Empty)
        {
            if (text.LastIndexOf(',') >= 0)
            {
                for (int num = 0; num < text.Split(',').Length; num++)
                {
                    if (text.Split(',')[num] != string.Empty)
                    {
                        list.Add(int.Parse(text.Split(',')[num]));
                    }
                }
            }
            else
            {
                list.Add(int.Parse(text));
            }
        }
        if (list.Count == 8)
        {
            list.Clear();
            RecordManager.SaveRecord("UserData", "ZhuanpanOldData" + sTime, string.Empty);
            text = string.Empty;
        }
        List<int> list2 = new List<int>();
        for (int i = 1; i <= 10; i++)
        {
            int item = 0;
            if (GameEntry.buildType == GameEntry.BuildType.GoogleEN)
            {
                item = int.Parse(Singleton<Datazhuanpan>.Instance.GetContentByKeyAndType("A" + i.ToString(), zhuanpanType.imax0));
            }
            for (int j = 0; j < list.Count; j++)
            {
                if (i == list[j] + 1)
                {
                    item = 1;
                    break;
                }
            }
            list2.Add(item);
        }
        int num2 = 0;
        for (int k = 0; k < list2.Count; k++)
        {
            num2 += list2[k] + 1;
        }
        int num3 = UnityEngine.Random.Range(0, num2);
        num2 = 0;
        for (int l = 0; l < list2.Count; l++)
        {
            num2 += list2[l] + 1;
            if (num3 > num2)
            {
                continue;
            }
            if (list.Count == 7)
            {
                for (int m = 0; m < 8; m++)
                {
                    bool flag = true;
                    for (int n = 0; n < list.Count; n++)
                    {
                        if (m == list[n])
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        l = m;
                    }
                }
            }
            else
            {
                for (int num4 = 0; num4 < list.Count; num4++)
                {
                    if (l == list[num4])
                    {
                        return -1;
                    }
                }
            }
            RecordManager.SaveRecord(value: (!(text == string.Empty)) ? (text + "," + l) : l.ToString(), RecordName: "UserData", key: "ZhuanpanOldData" + sTime);
            return l;
        }
        return -1;
    }

    public override void OnRefresh()
    {
    }

    public override IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        StartCoroutine(base.EnterAnim(animComplete, callBack, objs));
        yield return new WaitForEndOfFrame();
    }

    public override IEnumerator ExitAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        StartCoroutine(base.ExitAnim(animComplete, callBack, objs));
        yield return new WaitForEndOfFrame();
    }

    public override IEnumerator PauserAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        StartCoroutine(base.PauserAnim(animComplete, callBack, objs));
        yield return new WaitForEndOfFrame();
    }
}
