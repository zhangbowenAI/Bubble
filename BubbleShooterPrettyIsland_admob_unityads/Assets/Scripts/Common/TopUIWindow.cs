
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TopUIWindow : UIWindowBase
{
    public Text GoldObj;

    public GameObject GoldPos;

    public GameObject LovePos;

    public static TopUIWindow Instance;

    public GameObject LoveInfiniteObj;

    public GameObject LoveObj;

    private int LoveFullTime;

    public override void OnOpen()
    {
        AddOnClickListener("GoldAdd", ClickGoldAdd);
        AddOnClickListener("LoveAdd", ClickLoveAdd);
        AddEventListener(GameEventEnum.UIFlushChane, ReceviceGoldChange);
        InitUI();
    }

    public void InitUI()
    {
        Instance = this;
        GoldObj.text = Singleton<UserData>.Instance.GetUserGold().ToString();
        GetComponent<Canvas>().overrideSorting = false;
        InitLoveTime();
    }

    public void ShowSort()
    {
        GetComponent<Canvas>().overrideSorting = true;
        GetComponent<Canvas>().sortingOrder = 300;
    }

    public void HideSort()
    {
        GetComponent<Canvas>().overrideSorting = false;
    }

    private void ReceviceGoldChange(params object[] objs)
    {
        GoldObj.text = Singleton<UserData>.Instance.GetUserGold().ToString();
        InitLoveTime();
    }

    public void InitLoveTime()
    {
        if (Singleton<UserData>.Instance.getLoveInfinite() <= 0)
        {
            LoveInfiniteObj.SetActive(value: false);
            LoveObj.SetActive(value: true);
            LoadLove();
            CalcTime();
            LoadTime();
            StartCoroutine(UpdateViewLove());
        }
        else
        {
            LoveInfiniteObj.SetActive(value: true);
            LoveObj.SetActive(value: false);
            StartCoroutine(UpdateLoveInfinite());
        }
    }

    private IEnumerator UpdateViewLove()
    {
        bool b = true;
        while (b)
        {
            yield return new WaitForSeconds(1f);
            CalcTime();
            LoadTime();
            LoadLove();
            LoveInfiniteObj.SetActive(value: false);
            LoveObj.SetActive(value: true);
            int _iLoveTime = Singleton<UserData>.Instance.getLoveInfinite();
            if (_iLoveTime > 0)
            {
                b = false;
                StartCoroutine(UpdateLoveInfinite());
            }
        }
    }

    public void LoadTime()
    {
        if (LoveFullTime <= 0)
        {
            LoveObj.transform.Find("LoveFull").gameObject.SetActive(value: true);
            LoveObj.transform.Find("LoveTime").gameObject.SetActive(value: false);
            return;
        }
        LoveObj.transform.Find("LoveFull").gameObject.SetActive(value: false);
        LoveObj.transform.Find("LoveTime").gameObject.SetActive(value: true);
        int loveFullTime = LoveFullTime;
        int seconds = loveFullTime;
        TimeSpan timeSpan = new TimeSpan(0, 0, seconds);
        int minutes = timeSpan.Minutes;
        int num = timeSpan.Hours;
        int seconds2 = timeSpan.Seconds;
        int days = timeSpan.Days;
        if (days > 0)
        {
            num = days * 24 + num;
        }
        string text = minutes + string.Empty;
        string str = num + string.Empty;
        string text2 = seconds2 + string.Empty;
        if (minutes < 10)
        {
            text = "0" + text;
        }
        if (num < 10)
        {
            str = "0" + str;
        }
        if (seconds2 < 10)
        {
            text2 = "0" + text2;
        }
        LoveObj.transform.Find("LoveTime").GetComponent<Text>().text = text + ":" + text2;
    }

    public void LoadLove()
    {
        int userLoveCount = Singleton<UserData>.Instance.GetUserLoveCount();
        LoveObj.transform.Find("LoveText").GetComponent<Text>().text = userLoveCount.ToString();
    }

    private void CalcTime()
    {
        int userLoveCount = Singleton<UserData>.Instance.GetUserLoveCount();
        if (userLoveCount >= Singleton<UserData>.Instance.iLoveMaxAll)
        {
            LoveFullTime = 0;
            RecordManager.SaveRecord("UserData", "FullLoveTime", 0);
        }
        else
        {
            int intRecord = RecordManager.GetIntRecord("UserData", "FullLoveTime", 0);
            int nowTime = Util.GetNowTime();
            if (nowTime > intRecord)
            {
                RecordManager.SaveRecord("UserData", "LoveCount", Singleton<UserData>.Instance.iLoveMaxAll);
                RecordManager.SaveRecord("UserData", "FullLoveTime", 0);
                return;
            }
            LoveFullTime = intRecord - nowTime;
            int num = 0;
            while (LoveFullTime > Singleton<UserData>.Instance.ResLoveTime)
            {
                num++;
                LoveFullTime -= Singleton<UserData>.Instance.ResLoveTime;
            }
            RecordManager.SaveRecord("UserData", "LoveCount", Singleton<UserData>.Instance.iLoveMaxAll - num - 1);
        }
        LoveObj.transform.Find("LoveText").GetComponent<Text>().text = Singleton<UserData>.Instance.GetUserLoveCount().ToString();
    }

    private IEnumerator UpdateLoveInfinite()
    {
        bool b = true;
        while (b)
        {
            LoadLoveInfiniteTime();
            yield return new WaitForSeconds(1f);
            LoveInfiniteObj.SetActive(value: true);
            LoveObj.SetActive(value: false);
            int _iLoveTime = Singleton<UserData>.Instance.getLoveInfinite();
            if (_iLoveTime <= 0)
            {
                b = false;
                StartCoroutine(UpdateViewLove());
            }
        }
    }

    public void LoadLoveInfiniteTime()
    {
        int loveInfinite = Singleton<UserData>.Instance.getLoveInfinite();
        TimeSpan timeSpan = new TimeSpan(0, 0, loveInfinite);
        int minutes = timeSpan.Minutes;
        int num = timeSpan.Hours;
        int seconds = timeSpan.Seconds;
        int days = timeSpan.Days;
        if (days > 0)
        {
            num = days * 24 + num;
        }
        string text = minutes + string.Empty;
        string text2 = num + string.Empty;
        string text3 = seconds + string.Empty;
        if (minutes < 10)
        {
            text = "0" + text;
        }
        if (num < 10)
        {
            text2 = "0" + text2;
        }
        if (seconds < 10)
        {
            text3 = "0" + text3;
        }
        LoveInfiniteObj.transform.Find("LoveInfiniteTime").GetComponent<Text>().text = text2 + ":" + text + ":" + text3;
    }

    public void ClickGoldAdd(InputUIOnClickEvent e)
    {
        UIManager.OpenUIWindow<BuyGoldUIWindow>();
    }

    public void ClickLoveAdd(InputUIOnClickEvent e)
    {
        UIManager.OpenUIWindow<BuyLoveUIWindow>();
    }

    public override void OnRefresh()
    {
    }

    public override IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        m_bgMask.SetActive(value: true);
        Vector3 temp = base.transform.localPosition;
        temp.y = Screen.height;
        m_uiRoot.transform.localPosition = temp;
        if (GameManager.GetFullScreen())
        {
            Tweener t = m_uiRoot.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 0f), 0.25f);
            t.OnComplete(delegate
            {
                animComplete(this, callBack, objs);
            });
        }
        else
        {
            Tweener t2 = m_uiRoot.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 0f), 0.25f);
            t2.OnComplete(delegate
            {
                animComplete(this, callBack, objs);
            });
        }
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

    public void SettingBtnClick()
    {
        AudioPlayManager.PlaySFX2D("button");
        UIManager.OpenUIWindow<SettingUIWindow>();
    }
}
