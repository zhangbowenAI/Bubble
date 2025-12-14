using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackagePanel : MonoBehaviour
{
    public Text title;
    public GameObject coinsPart, propPart;
    public GameObject coinPackage, PropPackage;

    private bool isCoin;

    public void SetData(bool isCoin)
    {
        gameObject.SetActive(true);
        this.isCoin = isCoin;
        coinsPart.SetActive(isCoin);
        propPart.SetActive(!isCoin);
        title.text = isCoin ? "金币大礼包" : "道具大礼包";
    }

    public void CloseBtnOnclick()
    {
        AudioPlayManager.PlaySFX2D("button");
        gameObject.SetActive(false);
    }

    public void GetBtnOnclick()
    {
        AudioPlayManager.PlaySFX2D("button");
        ADInterface.Instance.SendADVideoEvent(ADType.RewardVideoAD, ADSpot.GetPackage, ADCallback);
    }

    private void ADCallback(bool result)
    {
        if (result)
        {
            if (isCoin)
            {
                PlayerPrefs.SetInt("CoinsPackage", 1);
                // UserData.Instance.AddUserGold(5000);
                List<RewardType> list = new List<RewardType>();
                list.Add(new RewardType(2, 5000));
                UIManager.Reward(list);
                if (coinPackage != null)
                    coinPackage.SetActive(false);

                CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.AD_GetMoneyPack);
            }
            else
            {
                PlayerPrefs.SetInt("PropPackage", 1);
                // UserData.Instance.AddProps(3, 3);
                // UserData.Instance.AddProps(4, 3);
                // UserData.Instance.AddProps(5, 3);
                List<RewardType> list = new List<RewardType>();
                list.Add(new RewardType(6, 3));
                list.Add(new RewardType(7, 3));
                list.Add(new RewardType(8, 3));
                UIManager.Reward(list);
                if (PropPackage != null)
                    PropPackage.SetActive(false);

                CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.AD_GetItemPack);
            }
        }
        gameObject.SetActive(false);
    }
}
