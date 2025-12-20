
using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelScene : MonoBehaviour
{
    public GameObject LodingUI;

    public static LevelScene Instance;

    public GameObject SignBtnObj;

    public GameObject LotterBtnObj;

    public GameObject SignHongdian;

    public GameObject LotterHongdian;

    public GameObject LeftUI;

    public GameObject RightUI;

    public GameObject jinzuObj;

    public GameObject jinzuHongdian;

    public Text LotterTime;

    public GameObject LotterImage;

    public Text jinzuGold;

    public Text MapName;

    public Text MapLevel;

    public Text SignText;

    public Text LotterText;

    public Text SettingText;
    public GameObject coinPackage, PropPackage;
    public PackagePanel packagePanel;

    [SerializeField]
    private Text ContinuousWinText;

    [SerializeField]
    private GameObject ContinuousWinButton;

    public GameObject SlideObj;

    public Image SliderImage;

    public Text SliderText;

    public GameObject BoxCloseObj;

    public GameObject BoxOpenObj;

    public Text OpenText;
    public GameObject nextMapBtn, preMapBtn;
    public GameObject guideFinger;
    public GameObject shopBtnTip;
    public GameObject winBtnTip;
    public GameObject taskBtn;
    public GameObject missionTip;

    private float stayTimer = 0;
    private bool showGuideFinger = false;

    private void Awake()
    {
        Instance = this;
        UIManager.OpenUIWindow<TopUIWindow>();
    }

    private void Start()
    {
        if (Singleton<UserData>.Instance.GetPassLevel() > 3)
            ADInterface.Instance.SendADEvent(ADType.CPAD, ADSpot.MainPanel);
        ApplicationManager.SceneName = GameEntry.LevelScene;
        coinPackage.SetActive(Singleton<UserData>.Instance.GetPassLevel() >= 6 && PlayerPrefs.GetInt("CoinsPackage", 0) == 0);
        PropPackage.SetActive(Singleton<UserData>.Instance.GetPassLevel() >= 20 && PlayerPrefs.GetInt("PropPackage", 0) == 0);

        SignBtnObj.SetActive(Singleton<UserData>.Instance.GetPassLevel() >= 1);
        LotterBtnObj.SetActive(Singleton<UserData>.Instance.GetPassLevel() >= 2);
        taskBtn.SetActive(Singleton<UserData>.Instance.GetPassLevel() >= 5);

        // try
        // {
        //     AGame.MySdkManager.me.LoadAddIfNotLoaded();
        // }
        // catch { }

        MapManage.Instance.iStart();
        InitMapName(MapManage.Instance.iMapIndex);
        // nextMapBtn.SetActive(MapManage.Instance.iMapIndex != MapData.iMapCount);
        // preMapBtn.SetActive(MapManage.Instance.iMapIndex != 0);
        string nowTime_Day = Util.GetNowTime_Day();
        AudioPlayManager.PlayMusic2D("bg_music_map", 1);
        SetLotterTime();
        InitBoxSlider();
        InitLanguage();
        jinzuGold.text = Singleton<UserData>.Instance.GetJinZhuGold().ToString();
        if (RecordManager.GetIntRecord("LevelData", "iSignOK" + nowTime_Day, 0) == 0)
        {
            SignHongdian.SetActive(value: true);
            // SignBtnObj.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "baidong", loop: true);
        }
        else
        {
            SignHongdian.SetActive(value: false);
            // SignBtnObj.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "stand", loop: true);
        }
        GlobalEvent.AddEvent(GameEventEnum.UIFlushChane, ReceviceJinzhuGoldChange);
        if (GameManager.GetFullScreen())
        {
            LeftUI.GetComponent<RectTransform>().DOAnchorPos(new Vector2(69f, -255f), 0.25f);
            RightUI.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-100f, -255f), 0.25f);
        }
        ContinuousWinButton.SetActive(Singleton<UserData>.Instance.GetPassLevel() >= 14);
        if (Singleton<UserData>.Instance.GetPassLevel() >= 21)
        {
            jinzuObj.SetActive(value: true);
            if (Singleton<UserData>.Instance.GetJinZhuGold() >= 4500)
            {
                StartCoroutine(OpenJinzu());
            }
            else
            {
                jinzuHongdian.SetActive(value: false);
            }
        }
        GlobalEvent.AddEvent(GameEventEnum.UIFlushChane, ReceviceGoldChange);

        if (PlayerData.Instance.guideStep == 1)
        {
            UIManager.OpenUIWindow<SignDayWindow>();
        }
        else if (PlayerData.Instance.guideStep == 2)
        {
            UIManager.OpenUIWindow<LotterUIWindow>();
        }
        else if (PlayerData.Instance.guideStep == 3)
        {
            UIManager.OpenUIWindow<DiscountShopWindow>();
        }
        else if (PlayerData.Instance.guideStep == 4)
        {
            UIManager.OpenUIWindow<MissionWindow>();
        }
        showGuideFinger = false;
        guideFinger.SetActive(false);

        SetShopBtnTip();
        SetMissionTip();
        winBtnTip.SetActive(RecordManager.GetIntRecord("LevelData", "SequenceWinCount", 0) > 0);
    }

    private IEnumerator OpenJinzu()
    {
        yield return new WaitForSeconds(1.5f);
        RecordManager.GetIntRecord("UserData", "JinZhuMan", 1);
        if (Singleton<UserData>.Instance.GetJinZhuGold() >= 6000 && RecordManager.GetIntRecord("UserData", "JinZhuMan", 0) == 0)
        {
            RecordManager.SaveRecord("UserData", "JinZhuMan", 1);
            UIManager.OpenUIWindow<jinzuUIWindow>();
        }
    }

    public void SetShopBtnTip()
    {
        bool showBtnTip = false;
        for (var i = 0; i < PlayerData.Instance.DiscountShopList.Count; i++)
        {
            if (PlayerData.Instance.DiscountShopList[i].hasBuy == false)
            {
                showBtnTip = true;
            }
        }
        shopBtnTip.SetActive(showBtnTip);
    }

    public void SetMissionTip()
    {
        if (missionTip.gameObject != null)
            missionTip.SetActive(CheckMissionTip());
    }

    private bool CheckMissionTip()
    {
        MissionInfoParam missionInfo = PlayerData.Instance.GetMissionInfo();
        for (var i = 0; i < missionInfo.countList.Count; i++)
        {
            if (missionInfo.finishStateList[i] == false)
            {
                if (missionInfo.countList[i] >= MissionData.Instance.GetCount(i + 1))
                    return true;
            }
        }
        return false;
    }

    public void InitBoxSlider()
    {
        int intRecord = RecordManager.GetIntRecord("UserData", "ChestGrade", 0);
        int intRecord2 = RecordManager.GetIntRecord("UserData", "ChestCount", 0);
        OpenText.text = Util.ReplaceText(GameEntry.Instance.GetString("ChestOpen"));
        switch (intRecord)
        {
            case 0:
                if (intRecord2 >= 5)
                {
                    BoxOpenObj.SetActive(value: true);
                    BoxCloseObj.SetActive(value: false);
                    break;
                }
                BoxOpenObj.SetActive(value: false);
                BoxCloseObj.SetActive(value: true);
                SlideObj.SetActive(value: true);
                SliderText.text = intRecord2 + "/5";
                SliderImage.fillAmount = (float)intRecord2 / 5f;
                break;
            case 1:
                if (intRecord2 >= 10)
                {
                    BoxOpenObj.SetActive(value: true);
                    BoxCloseObj.SetActive(value: false);
                    break;
                }
                BoxOpenObj.SetActive(value: false);
                BoxCloseObj.SetActive(value: true);
                SlideObj.SetActive(value: true);
                SliderText.text = intRecord2 + "/10";
                SliderImage.fillAmount = (float)intRecord2 / 10f;
                break;
            default:
                if (intRecord2 >= 15)
                {
                    BoxOpenObj.SetActive(value: true);
                    BoxCloseObj.SetActive(value: false);
                    break;
                }
                BoxOpenObj.SetActive(value: false);
                BoxCloseObj.SetActive(value: true);
                SlideObj.SetActive(value: true);
                SliderText.text = intRecord2 + "/15";
                SliderImage.fillAmount = (float)intRecord2 / 15f;
                break;
        }
    }

    private void ReceviceGoldChange(params object[] objs)
    {
        InitBoxSlider();
        if (Singleton<UserData>.Instance.GetJinZhuGold() < 4500)
        {
            jinzuHongdian.SetActive(value: false);
        }
    }

    public void InitLanguage()
    {
        SignText.text = GameEntry.Instance.GetString("Sign");
        LotterText.text = GameEntry.Instance.GetString("Lotter");
        SettingText.text = GameEntry.Instance.GetString("Setting");
        ContinuousWinText.text = Util.ReplaceTextLine(GameEntry.Instance.GetString("ContinuousButton"));
    }

    private void InitMapName(int mapindex)
    {
        string @string = GameEntry.Instance.GetString("MapNameRemark" + (mapindex + 1).ToString());
        if (@string.Split('_').Length > 1)
        {
            MapName.text = @string.Replace("_", " ");
        }
        else
        {
            MapName.text = @string;
        }
        string string2 = GameEntry.Instance.GetString("CurrerLevel");
        string newValue = (Singleton<MapData>.Instance.LMapEndBtnID[mapindex] + 1).ToString() + "-" + (Singleton<MapData>.Instance.LMapEndBtnID[mapindex] + Singleton<MapData>.Instance.LMapBtnCount[mapindex]).ToString();
        string2 = string2.Replace("_", " ").Replace("*", newValue);
        MapLevel.text = string2;
    }

    private void ReceviceJinzhuGoldChange(params object[] objs)
    {
        jinzuGold.text = Singleton<UserData>.Instance.GetJinZhuGold().ToString();
    }

    private void Update()
    {
        string nowTime_Day = Util.GetNowTime_Day();
        int intRecord = RecordManager.GetIntRecord("LevelData", "iSignOK" + nowTime_Day, 0);
        SetLotterTime();
        if (intRecord == 0)
        {
            if (!SignHongdian.activeSelf)
            {
                // SignBtnObj.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "baidong", loop: true);
                SignHongdian.SetActive(value: true);
            }
        }
        else if (SignHongdian.activeSelf)
        {
            SignHongdian.SetActive(value: false);
            // SignBtnObj.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "stand", loop: true);
        }

        if (!showGuideFinger)
        {
            stayTimer += Time.deltaTime;
            if (stayTimer >= 8)
            {
                showGuideFinger = true;
                guideFinger.SetActive(true);
                guideFinger.transform.DOLocalMoveY(55, 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }

    public void SetLotterTime()
    {
        int nowTime = Util.GetNowTime();
        int intRecord = RecordManager.GetIntRecord("UserData", "LotterLastTime", 0);
        int num = nowTime - intRecord;
        int num2 = 300;
        if (num < num2)
        {
            int seconds = num2 - num;
            LotterImage.SetActive(value: false);
            LotterBtnObj.SetActive(value: true);
            LotterHongdian.SetActive(true);
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
            if (LotterTime.gameObject.activeSelf)
            {
                LotterTime.text = text + ":" + text2;
            }
        }
        else if (LotterImage.gameObject.activeSelf)
        {
            LotterImage.SetActive(value: false);
            LotterBtnObj.SetActive(value: true);
            LotterHongdian.SetActive(true);
            // LotterBtnObj.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "zhuandong", loop: true);
        }
    }

    public void ChangeScene()
    {
        GameEntry.ChangeScene(GameEntry.GameScene);
    }

    public void ClickJinzu()
    {
        AudioPlayManager.PlaySFX2D("button");
        UIManager.OpenUIWindow<jinzuUIWindow>();
    }

    public void ClickSignDay()
    {
        AudioPlayManager.PlaySFX2D("button");
        UIManager.OpenUIWindow<SignDayWindow>();
    }

    public void ClickSettIng()
    {
        AudioPlayManager.PlaySFX2D("button");
        UIManager.OpenUIWindow<SettingUIWindow>();
    }

    public void ClickRateUs()
    {
        AudioPlayManager.PlaySFX2D("button");
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }

    public void ClickLotter()
    {
        AudioPlayManager.PlaySFX2D("button");
        UIManager.OpenUIWindow<LotterUIWindow>();
    }

    public void ClickContinuousWin()
    {
        AudioPlayManager.PlaySFX2D("button");
        UIManager.OpenUIWindow<ContinuousPanelWindow>();
    }

    public void ClickBuyGold()
    {
        AudioPlayManager.PlaySFX2D("button");
        UIManager.OpenUIWindow<BuyGoldUIWindow>();
    }

    public void ShowLodingUI()
    {
        LodingUI.SetActive(value: true);
        StartCoroutine("IEShowLodingUI");
    }

    IEnumerator IEShowLodingUI()
    {
        yield return new WaitForSeconds(1f);
        HideLoadingUI();
    }

    public void HideLoadingUI()
    {
        LodingUI.SetActive(value: false);
        StopCoroutine("IEShowLodingUI");
    }


    public void BackMapLayer()
    {
        AudioPlayManager.PlaySFX2D("button");
        int iMapIndex = MapManage.Instance.iMapIndex;

        Debug.Log("===> BackMapLayer [iMapIndex:" + iMapIndex + "]");

        if (iMapIndex != 0)
        {
            iMapIndex--;
            InitMapName(iMapIndex);
            MapManage.Instance.GoMap(iMapIndex);
            // nextMapBtn.SetActive(iMapIndex != MapData.iMapCount);
            // preMapBtn.SetActive(iMapIndex != 0);
        }
    }

    public void ComeNextMapLayer()
    {
        Debug.Log("===> ComeNextMapLayer [MapManage.Instance.iMapIndex:" + MapManage.Instance.iMapIndex + "]");

        AudioPlayManager.PlaySFX2D("button");
        if (MapManage.Instance.iMapIndex < MapData.iMapCount - 1)
        {
            int iMapIndex = MapManage.Instance.iMapIndex;
            iMapIndex++;
            InitMapName(iMapIndex);
            MapManage.Instance.GoMap(iMapIndex);
            // nextMapBtn.SetActive(iMapIndex != MapData.iMapCount);
            // preMapBtn.SetActive(iMapIndex != 0);
        }
    }

    public void CDkey()
    {
        UIManager.OpenUIWindow<CDkeyUIWindow>();
    }

    public void OnClickBox()
    {
        UIManager.OpenUIWindow<ChestUIWindow>();
    }

    public void CoinsPackageOnclick()
    {
        AudioPlayManager.PlaySFX2D("button");
        packagePanel.SetData(true);
    }

    public void PropPackageOnclick()
    {
        AudioPlayManager.PlaySFX2D("button");
        packagePanel.SetData(false);
    }

    public void ShopBtnClick()
    {
        UIManager.OpenUIWindow<DiscountShopWindow>();
    }

    public void MissionBtnClick()
    {
        UIManager.OpenUIWindow<MissionWindow>();
    }
}
