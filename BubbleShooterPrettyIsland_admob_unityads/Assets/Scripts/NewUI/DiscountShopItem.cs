using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscountShopItem : MonoBehaviour
{
    public Image icon;
    public Text discountText, priceText;
    public GameObject buyBtn, hasBuyIcon;

    private DiscountShopItemInfo itemInfo;
    private int id;
    private int price;

    public void Init(int index, DiscountShopItemInfo info)
    {
        id = index;
        itemInfo = info;
        Sprite iconSprite = Resources.Load<Sprite>("iconimage/" + DiscountShopData.Instance.GetImage(info.id));
        icon.sprite = iconSprite;
        discountText.text = string.Format("-{0}0%", info.discount);
        int normalPrice = DiscountShopData.Instance.GetPrice(info.id);
        price = (int)Mathf.Ceil((float)normalPrice * (1 - (float)info.discount / 10f));
        priceText.text = price.ToString();
        if (info.hasBuy)
        {
            buyBtn.SetActive(false);
            hasBuyIcon.SetActive(true);
        }
        else
        {
            buyBtn.SetActive(true);
            hasBuyIcon.SetActive(false);
        }
    }

    public void BuyBtnClick()
    {
        if (Singleton<UserData>.Instance.GetUserGold() >= price)
        {
            if (UIManager.GetWindowActive("RewardWindow"))
                return;
            Singleton<UserData>.Instance.DelUserGold(price);
            List<RewardType> list = new List<RewardType>();
            list.Add(new RewardType(DiscountShopData.Instance.GetRewardID(itemInfo.id), 2));
            UIManager.Reward(list);
            PlayerData.Instance.DiscountShopList[id].hasBuy = true;
            buyBtn.SetActive(false);
            hasBuyIcon.SetActive(true);
            LevelScene.Instance.SetShopBtnTip();
            ADInterface.Instance.SendADEvent(ADType.CPAD, ADSpot.BuyDiscounShop);
            CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.BuyItem);
        }
        else
        {
            UIManager.OpenUIWindow<BuyGoldUIWindow>();
        }

    }
}
