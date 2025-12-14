
using System;
using UnityEngine;
using UnityEngine.UI;

public class GoldsItem : MonoBehaviour
{
	public Image CenterImage;

	public Text GoldText;

	public Text PayText;

	private string Paykey = string.Empty;

	public void InitData(int index)
	{
		int num = int.Parse(Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.Icon));
		string contentByKeyAndType = Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.EnglishRemark);
		string contentByKeyAndType2 = Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.iMoney);
		string contentByKeyAndType3 = Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.RMB);
		int num2 = int.Parse(Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.iGold));
		float num3 = float.Parse(Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.ilove));
		string paykey = Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.GooglePay);
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			paykey = (Paykey = Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.IOS));
		}
		Paykey = paykey;
		GoldText.text = num2.ToString();
		if (GameEntry.buildType == GameEntry.BuildType.GoogleEN)
		{
			PayText.text = contentByKeyAndType2;
		}
		else
		{
			PayText.text = contentByKeyAndType3;
		}
		CenterImage.sprite = Util.GetResourcesSprite("ShopImage/ShopBG_" + num);
		CenterImage.SetNativeSize();
	}

	public void OnPayClick()
	{
		AudioPlayManager.PlaySFX2D("button");
		AndroidManager.Instance.Pay(Paykey);
	}
}
