using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionData : ICSVData<MissionData>
{
    public MissionData()
    {
        InitData("MissionData");
    }

    public int GetType(int id)
    {
        return GetPropertyToInt("Type", id);
    }

    public string GetTitle(int id)
    {
        return GetProperty("Title", id);
    }

    public string GetDesc(int id)
    {
        return GetProperty("Desc", id);
    }

    public int GetCount(int id)
    {
        return GetPropertyToInt("Count", id);
    }

    public int GetRewardType(int id)
    {
        return GetPropertyToInt("RewardType", id);
    }

    public int GetRewardCount(int id)
    {
        return GetPropertyToInt("RewardCount", id);
    }

    public List<int> GetMissionIDListByType(int type)
    {
        List<int> list = new List<int>();
        for (int i = 1; i <= GetDataRow(); i++)
        {
            if (GetType(i) == type)
                list.Add(i);
        }
        return list;
    }
}
