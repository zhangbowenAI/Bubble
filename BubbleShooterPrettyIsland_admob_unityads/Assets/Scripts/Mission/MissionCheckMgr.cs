using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 任务检测 
/// </summary>
public class MissionCheckMgr : MonoSingletonBase<MissionCheckMgr>
{
    public override void Init()
    {
        MissionInfoParam missionInfo = PlayerData.Instance.GetMissionInfo();
        int MissionCount = MissionData.Instance.GetDataRow();

        //初始化数据
        if (missionInfo.countList.Count < MissionCount)
        {
            Debug.Log("Init mission info");
            missionInfo.countList.Clear();
            missionInfo.finishStateList.Clear();
            for (int i = 0; i < MissionCount; ++i)
            {
                missionInfo.countList.Add(0);
                missionInfo.finishStateList.Add(false);
            }
        }

    }

    /// <summary>
    /// Checks the mission.
    /// </summary>
    /// <param name="mType">M type.</param>
    /// <param name="addCount">Add count.</param>
    public void CheckMission(MissionType mType, int addCount = 1)
    {
        MissionInfoParam missionInfo = PlayerData.Instance.GetMissionInfo();
        List<int> idList = MissionData.Instance.GetMissionIDListByType((int)mType);
        for (int i = 0; i < idList.Count; ++i)
        {
            int index = idList[i] - 1;
            missionInfo.countList[index] += addCount;
        }
        PlayerData.Instance.SaveData();
    }

    public void SetMission(MissionType mType, int Count)
    {
        MissionInfoParam missionInfo = PlayerData.Instance.GetMissionInfo();
        List<int> idList = MissionData.Instance.GetMissionIDListByType((int)mType);
        for (int i = 0; i < idList.Count; ++i)
        {
            int index = idList[i] - 1;
            if (Count > missionInfo.countList[index])
                missionInfo.countList[index] = Count;
        }
        PlayerData.Instance.SaveData();
    }
}


public enum MissionType
{
    GetCoin = 1,
    Level,
    ShootBubble,
    UseProp,
    CollectEnergy,
}
