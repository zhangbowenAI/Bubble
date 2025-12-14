using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerData : IJsonData<PlayerData>
{
    public int guideStep = 0;

    public PlayerDataParam playerData;

    #region 初始化

    public PlayerData()
    {
        string jsonStr = base.InitData("PlayerData", true, false);
        playerData = JsonConvert.DeserializeObject<PlayerDataParam>(jsonStr);
    }

    public void SaveData()
    {
        base.SaveData(JsonConvert.SerializeObject(playerData));
    }

    public PlayerInfoParam GetPlayerInfo()
    {
        return playerData.playerInfo;
    }
    public MissionInfoParam GetMissionInfo()
    {
        return playerData.missionInfo;
    }

    #endregion

    public bool IsSignGuide
    {
        get { return this.playerData.playerInfo.isSignGuide; }
        set { this.playerData.playerInfo.isSignGuide = value; }
    }

    public bool IsLotteryGuide
    {
        get { return this.playerData.playerInfo.isLotteryGuide; }
        set { this.playerData.playerInfo.isLotteryGuide = value; }
    }

    public bool IsShopGuide
    {
        get { return this.playerData.playerInfo.isShopGuide; }
        set { this.playerData.playerInfo.isShopGuide = value; }
    }

    public bool IsMissionGuide
    {
        get { return this.playerData.playerInfo.isMissionGuide; }
        set { this.playerData.playerInfo.isMissionGuide = value; }
    }

    public List<DiscountShopItemInfo> DiscountShopList
    {
        get { return this.playerData.playerInfo.discountShopList; }
        set { this.playerData.playerInfo.discountShopList = value; }
    }

    public long LoginTS
    {
        get { return this.playerData.playerInfo.loginTS; }
        set { this.playerData.playerInfo.loginTS = value; }
    }

    public int CompleteDailyGoal
    {
        get { return this.playerData.playerInfo.completeDailyGoal; }
        set { this.playerData.playerInfo.completeDailyGoal = value; }
    }

    public int DailyGoalCount
    {
        get { return this.playerData.playerInfo.dailyGoalCount; }
        set { this.playerData.playerInfo.dailyGoalCount = value; }
    }

    public int DailyGoalDay
    {
        get { return this.playerData.playerInfo.dailyGoalDay; }
        set { this.playerData.playerInfo.dailyGoalDay = value; }
    }

    public bool HasGetShopCoin
    {
        get { return this.playerData.playerInfo.hasGetShopCoin; }
        set { this.playerData.playerInfo.hasGetShopCoin = value; }
    }

    public bool FirstInit
    {
        get { return this.playerData.playerInfo.firstInit; }
        set { this.playerData.playerInfo.firstInit = value; }
    }
}