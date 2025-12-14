
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySkillItem : MonoBehaviour
{
	public Image SkillIcon;

	public Text SkillCount;

	public Text GoldNum;

	public Sprite[] Icon;

	private int _index;

	public void InitBuySkill(int index)
	{
		SkillIcon.sprite = Icon[Singleton<UserData>.Instance.buySkillType];
		int goldnum = int.Parse(Singleton<DataSkillPrice>.Instance.GetContentByKeyAndType((SkillPriceKey)Enum.ToObject(typeof(SkillPriceKey), Singleton<UserData>.Instance.buySkillType), SkillPriceType.numb1));
		int goldnum2 = int.Parse(Singleton<DataSkillPrice>.Instance.GetContentByKeyAndType((SkillPriceKey)Enum.ToObject(typeof(SkillPriceKey), Singleton<UserData>.Instance.buySkillType), SkillPriceType.numb3));
		int goldnum3 = int.Parse(Singleton<DataSkillPrice>.Instance.GetContentByKeyAndType((SkillPriceKey)Enum.ToObject(typeof(SkillPriceKey), Singleton<UserData>.Instance.buySkillType), SkillPriceType.numb9));
		switch (index)
		{
		case 2:
			SetSkillNum(9, goldnum3);
			_index = 9;
			break;
		case 1:
			SetSkillNum(3, goldnum2);
			_index = 3;
			break;
		case 0:
			SetSkillNum(1, goldnum);
			_index = 1;
			break;
		}
	}

	public void SetSkillNum(int index, int goldnum)
	{
		GoldNum.text = goldnum.ToString();
		SkillCount.text = "x" + index.ToString();
	}

	public void ClickPay()
	{
		AudioPlayManager.PlaySFX2D("button");
		int userGold = Singleton<UserData>.Instance.GetUserGold();
		if (userGold >= int.Parse(GoldNum.text))
		{
			Singleton<UserData>.Instance.DelUserGold(int.Parse(GoldNum.text));
			List<RewardType> list = new List<RewardType>();
			list.Add(new RewardType(Singleton<UserData>.Instance.buySkillType + 3, _index));
			UIManager.Reward(list);
			UIManager.CloseUIWindow<BuySkillWindow>(isPlayAnim: true, null, new object[0]);
		}
		else
		{
			UIManager.OpenUIWindow<BuyGoldUIWindow>();
		}
	}
}
