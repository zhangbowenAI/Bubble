using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using PathologicalGames;

public class DiscountShopWindow : UIWindowBase
{
    public Transform itemParent;
    public Text timerText;
    public Text GoldNum;
    private int refreshTimer;
    private SpawnPool itemPool;
    private List<DiscountShopItem> itemList = new List<DiscountShopItem>();

    public override void OnInit()
    {
        base.OnInit();
        itemPool = PoolManager.Pools["UIPool"];
        CreatItem();
    }

    public override void OnOpen()
    {
        DateTime nowTime = DateTime.Now;
        DateTime today1 = DateTime.Now.Date;
        int disTime = (int)(WUtil.DateTime2TimeStamp(DateTime.Now) - WUtil.DateTime2TimeStamp(DateTime.Now.Date));
        refreshTimer = 3600 * 24 - disTime;
        timerText.text = string.Format("{0}后刷新", WUtil.SecondsToTimeStr(refreshTimer));
        CancelInvoke("TimerChange");
        InvokeRepeating("TimerChange", 0, 1);
        RefeshItem();
        AddEventListener(GameEventEnum.UIFlushChane, ReceviceGoldChange);
        GoldNum.text = Singleton<UserData>.Instance.GetUserGold().ToString();
    }

    private void ReceviceGoldChange(params object[] objs)
	{
		GoldNum.text = Singleton<UserData>.Instance.GetUserGold().ToString();
	}

    public void CreatItem()
    {
        for (var i = 0; i < 6; i++)
        {
            Transform item = itemPool.Spawn("ShopItem", itemParent);
            itemList.Add(item.GetComponent<DiscountShopItem>());
        }
    }

    public void RefeshItem()
    {
        for (var i = 0; i < 6; i++)
        {
            itemList[i].Init(i, PlayerData.Instance.DiscountShopList[i]);
        }
    }

    public override void OnClose()
    {
        CancelInvoke("TimerChange");
    }

    private void TimerChange()
    {
        refreshTimer--;
        timerText.text = string.Format("{0}后刷新", WUtil.SecondsToTimeStr(refreshTimer));
    }

    public void OnCloseBtn()
    {
        UIManager.CloseUIWindow<DiscountShopWindow>(isPlayAnim: true, null, new object[0]);
        if (!PlayerData.Instance.IsShopGuide && PlayerData.Instance.guideStep == 3)
        {
            PlayerData.Instance.IsShopGuide = true;
            Singleton<LevelManager>.Instance.SetNowSelectLevel(Singleton<UserData>.Instance.GetPassLevel() + 1);
            UIManager.OpenUIWindow<PlayWindow>();
            PlayerData.Instance.guideStep = 0;
        }
    }

    public void RefreshBtnClick()
    {
        ADInterface.Instance.SendADVideoEvent(ADType.RewardVideoAD, ADSpot.RefreshShop, ADCallback);
    }

    private void ADCallback(bool result)
    {
        if (result)
        {
            DiscountShopData.Instance.RefreshItemList();
            RefeshItem();
            CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.AD_RefreshShop);
        }
    }
}
