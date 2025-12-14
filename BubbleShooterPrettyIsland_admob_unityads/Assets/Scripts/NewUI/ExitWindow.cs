using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitWindow : UIWindowBase
{
    public GameObject exitPart, rewardPart;
    public override void OnOpen()
    {
        exitPart.SetActive(true);
        rewardPart.SetActive(false);
        ADInterface.Instance.SendADEvent(ADType.NativeStartAD, ADSpot.ExitPanel);
    }

    public void GetBtnClick()
    {
        ADInterface.Instance.SendADVideoEvent(ADType.RewardVideoAD, ADSpot.ExitReward, ADCallback);
    }

    private void ADCallback(bool result)
    {
        if (result)
        {
            exitPart.SetActive(false);
            rewardPart.SetActive(true);
            List<RewardType> list = new List<RewardType>();
            list.Add(new RewardType(2, 800));
            UIManager.Reward(list);
        }
    }

    public void CloseBtnClick()
    {
        ADInterface.Instance.SendADEvent(ADType.NativeEndAD, ADSpot.ExitPanel);
        UIManager.CloseUIWindow<ExitWindow>(isPlayAnim: true, null, new object[0]);
    }

    public void ExitBtnClick()
    {
        PlatformSetting.Instance.ExitGame();
    }
}
