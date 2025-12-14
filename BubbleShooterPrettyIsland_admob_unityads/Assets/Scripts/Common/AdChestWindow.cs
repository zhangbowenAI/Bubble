
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdChestWindow : UIWindowBase
{
    private struct RewardItem
    {
        public int iType;

        public int iCount;
    }

    public static AdChestWindow Instance;

    private bool bWait;

    [SerializeField]
    private Image[] mItemImage;

    [SerializeField]
    private Text[] mItemText;

    [SerializeField]
    private Text[] mItemHintText;

    [SerializeField]
    private Text mOpenButtonText;

    private List<List<RewardItem>> mAllItemList = new List<List<RewardItem>>();

    private List<RewardType> mRewardList = new List<RewardType>();

    public override void OnOpen()
    {
        if (null == Instance)
        {
            Instance = this;
        }
        mOpenButtonText.text = GameEntry.Instance.GetString("OpenForFree");
        mItemHintText[0].text = GameEntry.Instance.GetString("GoldText");
        mItemHintText[1].text = GameEntry.Instance.GetString("PropText");
        mItemHintText[2].text = GameEntry.Instance.GetString("PropText");
        if (mAllItemList.Count == 0)
        {
            mAllItemList.Add(new List<RewardItem>());
            mAllItemList.Add(new List<RewardItem>());
            mAllItemList.Add(new List<RewardItem>());
            int count = Singleton<DataWinVideoChest>.Instance.dDataObj.Count;
            foreach (KeyValuePair<WinVideoChestKey, Dictionary<WinVideoChestType, string>> item2 in Singleton<DataWinVideoChest>.Instance.dDataObj)
            {
                int num = 0;
                RewardItem item = default(RewardItem);
                foreach (KeyValuePair<WinVideoChestType, string> item3 in item2.Value)
                {
                    switch (item3.Key)
                    {
                        case WinVideoChestType.Pos:
                            num = int.Parse(item3.Value);
                            break;
                        case WinVideoChestType.reward:
                            item.iType = int.Parse(item3.Value);
                            break;
                        case WinVideoChestType.rewardnum:
                            item.iCount = int.Parse(item3.Value);
                            break;
                    }
                }
                mAllItemList[num - 1].Add(item);
            }
        }
        mRewardList.Clear();
        int index = Random.Range(0, mAllItemList[0].Count);
        RewardItem rewardItem = mAllItemList[0][index];
        int iType = rewardItem.iType;
        RewardItem rewardItem2 = mAllItemList[0][index];
        int iCount = rewardItem2.iCount;
        mRewardList.Add(new RewardType(iType, iCount));
        mItemImage[0].gameObject.SetActive(value: true);
        mItemImage[0].sprite = Util.GetResourcesSprite("IconImage/icon_" + iType);
        mItemImage[0].SetNativeSize();
        mItemText[0].text = "x" + iCount.ToString();
        int index2 = Random.Range(0, mAllItemList[1].Count);
        RewardItem rewardItem3 = mAllItemList[1][index2];
        int iType2 = rewardItem3.iType;
        RewardItem rewardItem4 = mAllItemList[1][index2];
        int iCount2 = rewardItem4.iCount;
        mRewardList.Add(new RewardType(iType2, iCount2));
        mItemImage[1].gameObject.SetActive(value: true);
        mItemImage[1].sprite = Util.GetResourcesSprite("IconImage/icon_" + iType2);
        mItemImage[1].SetNativeSize();
        mItemText[1].text = "x" + iCount2.ToString();
        int index3 = Random.Range(0, mAllItemList[2].Count);
        RewardItem rewardItem5 = mAllItemList[2][index3];
        int iType3 = rewardItem5.iType;
        RewardItem rewardItem6 = mAllItemList[2][index3];
        int iCount3 = rewardItem6.iCount;
        mRewardList.Add(new RewardType(iType3, iCount3));
        mItemImage[2].gameObject.SetActive(value: true);
        mItemImage[2].sprite = Util.GetResourcesSprite("IconImage/icon_" + iType3);
        mItemImage[2].SetNativeSize();
        mItemText[2].text = "x" + iCount3.ToString();
    }

    public void OnOpenChest()
    {
        if (!bWait)
        {
            bWait = true;
            AudioPlayManager.PlaySFX2D("button");
            // AndroidManager.Instance.ShowVideoAd("WinChestVideo");
            ADInterface.Instance.SendADVideoEvent(ADType.RewardVideoAD, ADSpot.WinChestVideo, AdVideoCallback);
        }
    }


    public void AdVideoCallback(bool result)
    {
        if (result)
        {
            bWait = false;
            mItemImage[0].gameObject.SetActive(value: false);
            mItemImage[1].gameObject.SetActive(value: false);
            mItemImage[2].gameObject.SetActive(value: false);
            UIManager.Reward(mRewardList);
            UIManager.CloseUIWindow(this, false, null);
            Timer.DelayCallBack(3f, OpenPlayWindow);
        }
    }

    private void OpenPlayWindow(params object[] l_objs)
    {
        UIManager.OpenUIWindow<PlayWindow>();
    }

    public void ShowPlayWindow()
    {
        if (!bWait)
        {
            UIManager.CloseUIWindow(this, false, null);
            UIManager.OpenUIWindow<PlayWindow>();
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
