using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TooSimpleFramework.UI;

public class MissionItem : MonoBehaviour
{
    public Text title, desc, rewardCountText;
    public GameObject completeIcon, canGetPart;
    public MissionState missionState = MissionState.UnFinish;
    public Sprite completeSprite, normalSprite;
    public Image iconBg;
    public int id;

    private int rewardCount;

    public Image progress;
    public Text progressText;

    public void Init(int id)
    {
        this.id = id;
        SetData();
    }

    public void SetData()
    {
        MissionInfoParam missionInfo = PlayerData.Instance.GetMissionInfo();
        title.text = MissionData.Instance.GetTitle(id);
        int needCount = MissionData.Instance.GetCount(id);
        int hasCount = missionInfo.countList[id - 1];
        desc.text = string.Format(MissionData.Instance.GetDesc(id), needCount);
        if (hasCount > needCount)
            hasCount = needCount;
        progressText.text = hasCount + "/" + needCount;
        progress.fillAmount = (float)hasCount / (float)needCount;
        if (missionInfo.finishStateList[id - 1] == true)//已领取
        {
            completeIcon.SetActive(true);
            canGetPart.SetActive(false);
            missionState = MissionState.HasGet;
            iconBg.sprite = normalSprite;
        }
        else//未领取
        {
            completeIcon.SetActive(false);
            if (hasCount >= needCount)//可领取
            {
                missionState = MissionState.CanGet;
                canGetPart.SetActive(true);
                iconBg.sprite = completeSprite;
            }
            else//不可领取
            {
                missionState = MissionState.UnFinish;
                canGetPart.SetActive(false);
                iconBg.sprite = normalSprite;
            }
            rewardCount = MissionData.Instance.GetRewardCount(id);
            rewardCountText.text = "X" + rewardCount.ToString();
        }
    }

    public void RewardBtnClick()
    {
        if (UIManager.GetWindowActive("RewardWindow"))
            return;
        //Todo展示领取界面
        completeIcon.SetActive(true);
        MissionInfoParam missionInfo = PlayerData.Instance.GetMissionInfo();
        missionInfo.finishStateList[id - 1] = true;
        missionState = MissionState.HasGet;

        List<RewardType> list = new List<RewardType>();
        list.Add(new RewardType(2, rewardCount));
        UIManager.Reward(list);

        int finishCount = 0;
        for (int i = 0; i < missionInfo.finishStateList.Count; i++)
        {
            if (missionInfo.finishStateList[i])
                finishCount++;
        }
        if (finishCount % 3 == 0)
        {
            if (BuildSetting.Instance.adChannel == AdChannelsType.OPPO)
                ADInterface.Instance.SendADEvent(ADType.SceneVideoAD, ADSpot.GetMission);
            else if (BuildSetting.Instance.adChannel == AdChannelsType.VIVO)
                ADInterface.Instance.SendADEvent(ADType.CPAD, ADSpot.GetMission);
        }
        MissionWindow.Instance.ResreshItem();
        LevelScene.Instance.SetMissionTip();
        CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.MissionClear);
    }
}

public enum MissionState
{
    CanGet = 0,
    UnFinish = 1,
    HasGet
}
