using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

public class PlayerDataParam
{
    public PlayerInfoParam playerInfo = new PlayerInfoParam();
    public MissionInfoParam missionInfo = new MissionInfoParam();
}

#region 玩家数据
public class PlayerInfoParam
{
    public bool firstInit = false;
    public bool isSignGuide = false;
    public bool isLotteryGuide = false;
    public bool isShopGuide = false;
    public bool isMissionGuide = false;
    public List<DiscountShopItemInfo> discountShopList = new List<DiscountShopItemInfo>();
    public long loginTS = 0;
    public int completeDailyGoal = 0;
    public int dailyGoalCount = 0;
    public int dailyGoalDay = 0;
    public bool hasGetShopCoin = false;
}

#endregion


public class MissionInfoParam
{
    public List<int> countList = new List<int>(); //任务数量
    public List<bool> finishStateList = new List<bool>(); //任务完成状态
}

public class DiscountShopItemInfo
{
    public int id;
    public bool hasBuy;
    public int discount;
}
