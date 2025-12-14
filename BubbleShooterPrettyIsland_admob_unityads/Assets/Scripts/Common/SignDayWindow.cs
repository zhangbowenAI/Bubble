
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SignDayWindow : UIWindowBase
{
    public GameObject DayFourObj;

    public GameObject DayThreeObj;

    private List<GameObject> SignList = new List<GameObject>();

    public GameObject SignButton;
    public GameObject ADSignButton;

    public Sprite SignSpriteHui;

    public Sprite SignSprite;

    public Text SignText;

    public Text SignBtnText;

    public GameObject tip1;
    private Sequence seq;

    public override void OnOpen()
    {
        AddOnClickListener("SignButton", ClickSignBtn);
        AddOnClickListener("CloseButton", CloseButton);
        InitLanguage();
        InitUI();
        InitSignBtn();
        ADInterface.Instance.SendADEvent(ADType.CPAD, ADSpot.SignPanel);
    }

    public void InitLanguage()
    {
        SignBtnText.text = Util.ReplaceText(GameEntry.Instance.GetString("SigninButton"));
        SignText.text = Util.ReplaceText(GameEntry.Instance.GetString("SigninUIremark"));
    }

    public override void OnRefresh()
    {
    }

    public void InitUI()
    {
        if (SignList.Count <= 0)
        {
            for (int i = 0; i < 7; i++)
            {
                if (i > 3)
                {
                    GameObject gameObject = Object.Instantiate(DayThreeObj);
                    gameObject.transform.SetParent(DayThreeObj.gameObject.transform.parent, worldPositionStays: false);
                    DayObjInit component = gameObject.GetComponent<DayObjInit>();
                    component.InitSignDay(i);
                    SignList.Add(gameObject);
                }
                else
                {
                    GameObject gameObject2 = Object.Instantiate(DayFourObj);
                    gameObject2.transform.SetParent(DayFourObj.gameObject.transform.parent, worldPositionStays: false);
                    DayObjInit component2 = gameObject2.GetComponent<DayObjInit>();
                    component2.InitSignDay(i);
                    SignList.Add(gameObject2);
                }
            }
        }
        else
        {
            for (int j = 0; j < 7; j++)
            {
                SignList[j].GetComponent<DayObjInit>().InitSignDay(j);
            }
        }

        seq.Kill();
        seq = DOTween.Sequence();
        seq.Append(tip1.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 10), 0.2f));
        seq.Append(tip1.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, -20), 0.2f));
        seq.Append(tip1.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 10), 0.2f));
        seq.AppendInterval(3f);
        seq.SetLoops(-1);
    }

    public void ClickSignBtn(InputUIOnClickEvent e)
    {
        string nowTime_Day = Util.GetNowTime_Day();
        RecordManager.SaveRecord("LevelData", "iSignOK" + nowTime_Day, 1);
        int num = RecordManager.GetIntRecord("LevelData", "SignCount", 0);
        if (num == 7)
        {
            num = 0;
        }
        SignList[num].GetComponent<DayObjInit>().Reward();
        SignList[num].GetComponent<DayObjInit>().CheckIsSign();
        num++;
        RecordManager.SaveRecord("LevelData", "SignCount", num);
        if (num != 7)
        {
            SignList[num].GetComponent<DayObjInit>().ShowHuanObj();
        }
        InitSignBtn();
        if (!PlayerPrefs.HasKey("FirstSign"))
        {
            PlayerPrefs.SetInt("FirstSign", 1);
        }
        else
        {
            if (BuildSetting.Instance.adChannel == AdChannelsType.OPPO)
                ADInterface.Instance.SendADEvent(ADType.SceneVideoAD, ADSpot.NormalSign);
            else if (BuildSetting.Instance.adChannel == AdChannelsType.VIVO)
                ADInterface.Instance.SendADEvent(ADType.CPAD, ADSpot.NormalSign);
        }
    }

    public void ADBtnOnclick()
    {
        ADInterface.Instance.SendADVideoEvent(ADType.RewardVideoAD, ADSpot.SignDay, ADCallback);
    }

    private void ADCallback(bool result)
    {
        if (result)
        {
            string nowTime_Day = Util.GetNowTime_Day();
            RecordManager.SaveRecord("LevelData", "iSignOK" + nowTime_Day, 1);
            int num = RecordManager.GetIntRecord("LevelData", "SignCount", 0);
            if (num == 7)
            {
                num = 0;
            }
            SignList[num].GetComponent<DayObjInit>().ADReward();
            SignList[num].GetComponent<DayObjInit>().CheckIsSign();
            num++;
            RecordManager.SaveRecord("LevelData", "SignCount", num);
            if (num != 7)
            {
                SignList[num].GetComponent<DayObjInit>().ShowHuanObj();
            }
            InitSignBtn();
            if (!PlayerPrefs.HasKey("FirstSign"))
            {
                PlayerPrefs.SetInt("FirstSign", 1);
            }
            CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.AD_SignUp);
        }
    }

    public void InitSignBtn()
    {
        string nowTime_Day = Util.GetNowTime_Day();
        SignButton.GetComponent<Button>().interactable = false;
        SignButton.GetComponent<Image>().sprite = SignSpriteHui;
        ADSignButton.SetActive(false);
        if (RecordManager.GetIntRecord("LevelData", "iSignOK" + nowTime_Day, 0) == 0)
        {
            StartCoroutine(ShowHuanObj());
            SignButton.GetComponent<Button>().interactable = true;
            SignButton.transform.Find("Text").GetComponent<Text>().text = Util.ReplaceText(GameEntry.Instance.GetString("SigninButton"));
            SignButton.GetComponent<Image>().sprite = SignSprite;
            SignButton.transform.localPosition = new Vector3(-120, -255, 0);
            ADSignButton.SetActive(true);
        }
        else
        {
            StartCoroutine(UpdateTime());
            StartCoroutine(ShowHuanObj());
            SignButton.transform.localPosition = new Vector3(0, -255, 0);
        }
    }

    private IEnumerator UpdateTime()
    {
        bool b = true;
        while (b)
        {
            string sTime = Util.GetNowTime_Day();
            string NowTime = Util.GetNowTime_Hours();
            int iH_ = int.Parse(NowTime.Split('_')[0]);
            int iF_2 = int.Parse(NowTime.Split('_')[1]);
            int iM_2 = int.Parse(NowTime.Split('_')[2]);
            iH_ = 24 - iH_ - 1;
            iM_2 = 60 - iM_2;
            iF_2 = 60 - iF_2;
            string sF = iF_2 + string.Empty;
            string sH = iH_ + string.Empty;
            string sM = iM_2 + string.Empty;
            if (iF_2 < 10)
            {
                sF = "0" + sF;
            }
            if (iH_ < 10)
            {
                sH = "0" + sH;
            }
            if (iM_2 < 10)
            {
                sM = "0" + sM;
            }
            SignButton.transform.Find("Text").GetComponent<Text>().text = sH + ":" + sF + ":" + sM;
            if (RecordManager.GetIntRecord("LevelData", "iSignOK" + sTime, 0) == 0)
            {
                InitSignBtn();
                b = false;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void CloseButton(InputUIOnClickEvent e)
    {
        HideHuanObj();
        UIManager.CloseUIWindow<SignDayWindow>(isPlayAnim: true, null, new object[0]);
        if (!PlayerData.Instance.IsSignGuide && PlayerData.Instance.guideStep == 1)
        {
            PlayerData.Instance.IsSignGuide = true;
            Singleton<LevelManager>.Instance.SetNowSelectLevel(Singleton<UserData>.Instance.GetPassLevel() + 1);
            UIManager.OpenUIWindow<PlayWindow>();
            PlayerData.Instance.guideStep = 0;
        }
    }

    private IEnumerator ShowHuanObj()
    {
        yield return new WaitForSeconds(0.2f);
        string sTime = Util.GetNowTime_Day();
        int IsSign = RecordManager.GetIntRecord("LevelData", "iSignOK" + sTime, 0);
        int SignDay = RecordManager.GetIntRecord("LevelData", "SignCount", 0);
        if (SignDay != 7 || IsSign != 1)
        {
            if (SignDay == 7 && IsSign == 0)
            {
                SignList[0].GetComponent<DayObjInit>().ShowHuanObj();
            }
            else
            {
                SignList[SignDay].GetComponent<DayObjInit>().ShowHuanObj();
            }
        }
    }

    public void HideHuanObj()
    {
        string nowTime_Day = Util.GetNowTime_Day();
        int intRecord = RecordManager.GetIntRecord("LevelData", "iSignOK" + nowTime_Day, 0);
        int intRecord2 = RecordManager.GetIntRecord("LevelData", "SignCount", 0);
        if (intRecord2 != 7 || intRecord != 1)
        {
            if (intRecord2 == 7 && intRecord == 0)
            {
                SignList[0].GetComponent<DayObjInit>().HuanObj.SetActive(value: false);
            }
            else
            {
                SignList[intRecord2].GetComponent<DayObjInit>().HuanObj.SetActive(value: false);
            }
        }
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
