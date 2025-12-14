
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayWindow : UIWindowBase
{
    public static PlayWindow Instance;

    public Text LevelNumberText;

    public GameObject Star1;

    public GameObject Star2;

    public GameObject Star3;

    public GameObject SkillSelectItem;

    public List<GameObject> PlaySkillList = new List<GameObject>();

    public Text PlayBtnText;

    public Text FreeBtnText;

    public Text RemarkText;

    public bool IsCanClick;

    public GameObject GuidBG;

    public GameObject GuidText;

    public GameObject FreeBtn;

    public GameObject PlayBtn;

    public Text dailyGoalText;

    public override void OnOpen()
    {
        Instance = this;
        AddOnClickListener("CloseButton", CloseButton);
        AddOnClickListener("StartBtn", StarButton);
        AddOnClickListener("FreePower", OnClickFreePower);
        AddEventListener(GameEventEnum.UIFlushChane, ReceviceDaojuChange);
        AddEventListener(GameEventEnum.GuidChange, ReceviceGuidChange);
        initUI();
        InitGuid();
        InitLanguage();
        if (PlayerData.Instance.DailyGoalDay <= 7)
        {

            if (PlayerData.Instance.CompleteDailyGoal < PlayerData.Instance.DailyGoalCount)
                dailyGoalText.text = string.Format("今日目标({0}/{1})", PlayerData.Instance.CompleteDailyGoal, PlayerData.Instance.DailyGoalCount);
            else
                dailyGoalText.text = "赢取更多奖励!";
        }
        else
        {
            dailyGoalText.gameObject.SetActive(false);
        }
    }

    private void ReceviceGuidChange(params object[] objs)
    {
        GuidBG.SetActive(value: false);
        IsCanClick = false;
    }

    private void ReceviceDaojuChange(params object[] objs)
    {
        Singleton<UserData>.Instance.PlaySkillUse1 = false;
        Singleton<UserData>.Instance.PlaySkillUse2 = false;
        Singleton<UserData>.Instance.PlaySkillUse3 = false;
        LoadSelectSkill();
    }

    public void InitLanguage()
    {
        string @string = GameEntry.Instance.GetString("PlayUILevelTitle");
        LevelNumberText.text = Util.ReplaceText(@string.Replace("*", Singleton<LevelManager>.Instance.GetNowSelectLevel().ToString()));
        PlayBtnText.text = Util.ReplaceText(GameEntry.Instance.GetString("PlayUIAdGift"));
        FreeBtnText.text = Util.ReplaceTextLine(Util.ReplaceText(GameEntry.Instance.GetString("PlayUIAdLive")));
        string contentByKeyAndType = Singleton<DatalevelTypeAndRemark>.Instance.GetContentByKeyAndType("a" + Singleton<LevelManager>.Instance.GetNowSelectLevel().ToString(), levelTypeAndRemarkType.sRemark);
        RemarkText.text = Util.ReplaceText(GameEntry.Instance.GetString("PlayUIRemark").Replace("*", contentByKeyAndType));
    }

    public void InitGuid()
    {
        if (Singleton<LevelManager>.Instance.GetNowSelectLevel() == 12)
        {
            GuidBG.SetActive(value: true);
            ChangeText();
            IsCanClick = true;
        }
        else if (Singleton<LevelManager>.Instance.GetNowSelectLevel() == 13)
        {
            GuidBG.SetActive(value: true);
            ChangeText();
            IsCanClick = true;
        }
        else if (Singleton<LevelManager>.Instance.GetNowSelectLevel() == 14)
        {
            GuidBG.SetActive(value: true);
            ChangeText();
            IsCanClick = true;
        }
    }

    public void ChangeText()
    {
        if (GameEntry.buildType == GameEntry.BuildType.GoogleEN)
        {
            GuidText.GetComponent<Text>().text = Singleton<DataGuidZH>.Instance.GetContentByKeyAndType("level" + Singleton<LevelManager>.Instance.GetNowSelectLevel(), GuidZHType.text1);
        }
    }

    public void OnClickFreePower(InputUIOnClickEvent e)
    {
        if (!IsCanClick)
        {
            // AndroidManager.Instance.ShowVideoAd("PlayVideo");
            ADInterface.Instance.SendADVideoEvent(ADType.RewardVideoAD, ADSpot.PlayVideo, ADCallback);
        }
    }

    public void ADCallback(bool result)
    {
        if (result)
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
            CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.AD_GetItems);
        }
    }

    public void CloseButton(InputUIOnClickEvent e)
    {
        if (!IsCanClick)
        {
            Singleton<UserData>.Instance.PlaySkillUse1 = false;
            Singleton<UserData>.Instance.PlaySkillUse2 = false;
            Singleton<UserData>.Instance.PlaySkillUse3 = false;
            if (ApplicationManager.SceneName == GameEntry.GameScene)
            {
                UIManager.CloseUIWindow<PlayWindow>(isPlayAnim: false, null, new object[0]);
                GameEntry.ChangeScene(GameEntry.LevelScene);
            }
            else
            {
                UIManager.CloseUIWindow<PlayWindow>(isPlayAnim: true, null, new object[0]);
            }
        }
    }

    public void StarButton(InputUIOnClickEvent e)
    {
        if (!IsCanClick)
        {
            if (Singleton<UserData>.Instance.GetUserLoveCount() > 0)
            {
                UIManager.CloseUIWindow<PlayWindow>(isPlayAnim: false, null, new object[0]);
                GameEntry.ChangeScene(GameEntry.GameScene);
            }
            else
            {
                UIManager.OpenUIWindow<BuyLoveUIWindow>();
            }
        }
    }

    public void initUI()
    {
        if (RecordManager.GetIntRecord("UserData", "UserIsPay", 0) == 1)
        {
            string nowTime_Day = Util.GetNowTime_Day();
            int intRecord = RecordManager.GetIntRecord("UserData", "PlayCount" + nowTime_Day, 0);
            if (intRecord > 2)
            {
                Transform transform = PlayBtn.transform;
                Vector3 localPosition = PlayBtn.transform.localPosition;
                transform.localPosition = new Vector3(0f, localPosition.y, 0f);
                FreeBtn.SetActive(value: false);
            }
        }
        int intRecord2 = RecordManager.GetIntRecord("LevelData", "LevelStar_" + Singleton<LevelManager>.Instance.GetNowSelectLevel(), 0);
        if (intRecord2 >= 1)
        {
            Star1.SetActive(value: true);
        }
        else
        {
            Star1.SetActive(value: false);
        }
        if (intRecord2 >= 2)
        {
            Star2.SetActive(value: true);
        }
        else
        {
            Star2.SetActive(value: false);
        }
        if (intRecord2 >= 3)
        {
            Star3.SetActive(value: true);
        }
        else
        {
            Star3.SetActive(value: false);
        }
        IsCanClick = false;
        LoadSelectSkill();
    }

    private void LoadSelectSkill()
    {
        if (PlaySkillList.Count > 0)
        {
            for (int i = 0; i < PlaySkillList.Count; i++)
            {
                PlaySkillList[i].GetComponent<PlaySkillItem>().InitSkillItem(i);
            }
            return;
        }
        for (int j = 0; j < 3; j++)
        {
            GameObject gameObject = Object.Instantiate(SkillSelectItem);
            gameObject.transform.SetParent(SkillSelectItem.transform.parent, worldPositionStays: false);
            gameObject.SetActive(value: true);
            PlaySkillItem component = gameObject.GetComponent<PlaySkillItem>();
            component.InitSkillItem(j);
            PlaySkillList.Add(gameObject);
        }
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
