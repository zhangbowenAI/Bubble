using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldADItem : MonoBehaviour
{
    public GameObject getBtn, hasGetIcon;

    private void OnEnable()
    {
        if (getBtn != null)
        {
            getBtn.SetActive(!PlayerData.Instance.HasGetShopCoin);
            hasGetIcon.SetActive(PlayerData.Instance.HasGetShopCoin);
        }
    }

    public void OnPayClick()
    {
        AudioPlayManager.PlaySFX2D("button");
        // AndroidManager.Instance.Pay(Paykey);
        ADInterface.Instance.SendADVideoEvent(ADType.RewardVideoAD, ADSpot.GetCoins, ADCallback);
    }

    public void ADCallback(bool result)
    {
        if (result)
        {
            List<RewardType> list = new List<RewardType>();
            list.Add(new RewardType(2, 900));
            UIManager.Reward(list);

            CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.AD_GetMoney600);
        }
    }

    public void OnPayClick2()
    {
        AudioPlayManager.PlaySFX2D("button");
        // AndroidManager.Instance.Pay(Paykey);
        ADInterface.Instance.SendADVideoEvent(ADType.RewardVideoAD, ADSpot.GetCoins, ADCallback2);
    }

    public void ADCallback2(bool result)
    {
        if (result)
        {
            List<RewardType> list = new List<RewardType>();
            list.Add(new RewardType(2, 1800));
            UIManager.Reward(list);
            PlayerData.Instance.HasGetShopCoin = true;
            getBtn.SetActive(false);
            hasGetIcon.SetActive(true);

            CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.AD_GetMoney1200);
        }
    }
}
