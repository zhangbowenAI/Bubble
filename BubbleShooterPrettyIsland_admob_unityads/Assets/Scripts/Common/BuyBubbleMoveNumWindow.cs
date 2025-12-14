
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuyBubbleMoveNumWindow : UIWindowBase
{
    public static BuyBubbleMoveNumWindow Instance;

    public GameObject BuyMoveObj;

    public GameObject BuyMoveObj2;

    public Text GoldText;

    public Text Title1Text;

    public Text Title2Text;

    public Text CenterText;

    public Text CenterText2;

    public Text PlayOnText;

    public Text PlayOnText2;

    public Text FreeVideoText;

    public Text GoldText1;

    public Text GoldText2;

    [SerializeField]
    private GameObject[] mLoseRewardItem;

    [SerializeField]
    private GameObject mHintText;

    public override void OnOpen()
    {
        InitUI();
        AddOnClickListener("CloseButton", CloseButton);
        AddOnClickListener("CloseButton2", CloseButton);
        AddOnClickListener("PlayOnButton", payGoldBubble);
        AddOnClickListener("PlayOnButton2", payGoldBubble);
        AddOnClickListener("FreeVideoButton", payAdsBubble);
        AddEventListener(GameEventEnum.UIFlushChane, ReceviceGoldChange);
        InitLanguage();
        mHintText.SetActive(value: false);
        mHintText.GetComponent<Text>().text = GameEntry.Instance.GetString("ContinuousWinLoseHint");
        if (ContinuousItemArranger.gSequenceCount > 0 && Singleton<LevelManager>.Instance.GetNowLevel() > Singleton<UserData>.Instance.GetPassLevel())
        {
            mHintText.SetActive(value: true);
            for (int i = 0; i < mLoseRewardItem.Length; i++)
            {
                mLoseRewardItem[i].SetActive(i < ContinuousItemArranger.gSequenceCount);
            }
        }
    }

    private void ReceviceGoldChange(params object[] objs)
    {
        GoldText.text = Singleton<UserData>.Instance.GetUserGold().ToString();
    }

    public void InitLanguage()
    {
        Title1Text.text = Util.ReplaceText(GameEntry.Instance.GetString("BuyBubbleTtitle"));
        Title2Text.text = Util.ReplaceText(GameEntry.Instance.GetString("BuyBubbleTtitle"));
        CenterText.text = Util.ReplaceText(GameEntry.Instance.GetString("BuyBubbleRemark1"));
        CenterText2.text = Util.ReplaceText(GameEntry.Instance.GetString("BuyBubbleRemark1"));
        PlayOnText.text = Util.ReplaceText(GameEntry.Instance.GetString("BuyBubbleRemark2"));
        PlayOnText2.text = Util.ReplaceText(GameEntry.Instance.GetString("BuyBubbleRemark2"));
        FreeVideoText.text = Util.ReplaceText(GameEntry.Instance.GetString("BuyBubbleFree"));
    }

    public void InitUI()
    {
        Instance = this;
        GoldText.text = Singleton<UserData>.Instance.GetUserGold().ToString();
        string nowTime_Day = Util.GetNowTime_Day();
        int intRecord = RecordManager.GetIntRecord("UserData", "BuyMoveVideoCount" + nowTime_Day, 0);
        int intRecord2 = RecordManager.GetIntRecord("UserData", "UserIsPay", 0);
        bool flag = false;
        if (intRecord2 == 1)
        {
            if (intRecord < 3)
            {
                flag = true;
            }
        }
        else
        {
            flag = true;
        }
        int num = 600 + Singleton<UserData>.Instance.BuyMoveCount * 300;
        if (num > 1500)
        {
            num = 1500;
        }
        if (flag && GameScene.Instance.iWatchBuyMove < 2)
        {
            BuyMoveObj.SetActive(value: true);
            BuyMoveObj2.SetActive(value: false);
            GoldText1.text = num.ToString();
        }
        else
        {
            BuyMoveObj.SetActive(value: false);
            BuyMoveObj2.SetActive(value: true);
            GoldText2.text = num.ToString();
        }
    }

    public void CloseButton(InputUIOnClickEvent e)
    {
        GameScene.Instance.GameLoseUI();
        UIManager.CloseUIWindow<BuyBubbleMoveNumWindow>(isPlayAnim: true, null, new object[0]);
    }

    public void payGoldBubble(InputUIOnClickEvent e)
    {
        int userGold = Singleton<UserData>.Instance.GetUserGold();
        int num = 600 + Singleton<UserData>.Instance.BuyMoveCount * 300;
        if (num > 1500)
        {
            num = 1500;
        }
        if (userGold >= num)
        {
            Singleton<UserData>.Instance.DelUserGold(num);
            GameScene.Instance.BuyMoveCallBack(8);
            Singleton<UserData>.Instance.BuyMoveCount++;
            GameScene.Instance.AddFullFZ();
            UIManager.CloseUIWindow<BuyBubbleMoveNumWindow>(isPlayAnim: true, null, new object[0]);
        }
        else
        {
            UIManager.OpenUIWindow<BuyGoldUIWindow>();
        }
    }

    public void payAdsBubble(InputUIOnClickEvent e)
    {
        // AndroidManager.Instance.ShowVideoAd("BuyBubbleMove");
        ADInterface.Instance.SendADVideoEvent(ADType.RewardVideoAD, ADSpot.BuyBubbleMove, WatchVideoBack);
    }

    public void WatchVideoBack(bool result)
    {
        if (result)
        {
            string nowTime_Day = Util.GetNowTime_Day();
            int intRecord = RecordManager.GetIntRecord("UserData", "BuyMoveVideoCount" + nowTime_Day, 0);
            intRecord++;
            RecordManager.SaveRecord("UserData", "BuyMoveVideoCount" + nowTime_Day, intRecord);
            GameScene.Instance.BuyMoveCallBack(7);
            GameScene.Instance.iWatchBuyMove++;
            GameScene.Instance.AddFullFZ();
            UIManager.CloseUIWindow<BuyBubbleMoveNumWindow>(isPlayAnim: true, null, new object[0]);
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
