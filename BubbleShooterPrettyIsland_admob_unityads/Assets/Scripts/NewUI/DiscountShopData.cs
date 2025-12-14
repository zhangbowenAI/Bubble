using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscountShopData : ICSVData<DiscountShopData>
{
    public DiscountShopData()
    {
        InitData("DiscountShopData");
    }

    public int GetRewardID(int id)
    {
        return GetPropertyToInt("RewardID", id);
    }

    public int GetPrice(int id)
    {
        return GetPropertyToInt("Price", id);
    }

    public string GetImage(int id)
    {
        return GetProperty("Image", id);
    }

    public void RefreshItemList()
    {
        List<DiscountShopItemInfo> list = new List<DiscountShopItemInfo>();
        for (var i = 0; i < 6; i++)
        {
            DiscountShopItemInfo item = new DiscountShopItemInfo();
            int id = Random.Range(1, 7);
            item.id = id;
            item.discount = Random.Range(2, 7);
            item.hasBuy = false;
            list.Add(item);
        }
        PlayerData.Instance.DiscountShopList = list;
    }
}
