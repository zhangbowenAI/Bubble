
using System;
using UnityEngine;
using UnityEngine.UI;

public class GoldPackItem : MonoBehaviour
{
	public Text GoldNum;

	public Text PackName;

	public Image CenterImage;

	public GameObject[] IconObj;

	private string Paykey = string.Empty;

	public Text PayBtnText;

	public void InitData(int index)
	{
		int num = int.Parse(Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.Icon));
		string contentByKeyAndType = Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.EnglishRemark);
		
		string contentByKeyAndType2 = Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.RMB);
		string contentByKeyAndType3 = Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.iMoney);
		int num2 = int.Parse(Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.iGold));
		float ilove = float.Parse(Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.ilove));
		string paykey = Paykey = Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.GooglePay);
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			paykey = (Paykey = Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.IOS));
		}
		Paykey = paykey;
		string contentByKeyAndType4 = Singleton<DataPayGold>.Instance.GetContentByKeyAndType((PayGoldKey)Enum.ToObject(typeof(PayGoldKey), index), PayGoldType.Package);
		InitIconImage(ilove, contentByKeyAndType4);
		PackName.text = contentByKeyAndType;
		CenterImage.sprite = Util.GetResourcesSprite("ShopImage/ShopBG_" + num);
		CenterImage.SetNativeSize();
        
        if (GameEntry.buildType == GameEntry.BuildType.GoogleEN) {
            PayBtnText.text = contentByKeyAndType3;
        }

        GoldNum.text = num2.ToString();
	}

	public void InitIconImage(float ilove, string Package)
	{
		string[] array = Package.Split('|');
		if (!(ilove > 0f))
		{
			return;
		}
		IconObj[0].transform.Find("NumText").GetComponent<Text>().text = ilove + "h";
		IconObj[0].transform.Find("Icon").GetComponent<Image>().sprite = Util.GetResourcesSprite("ShopImage/ShopIcon_1");
		IconObj[0].transform.Find("Icon").GetComponent<Image>().SetNativeSize();
		for (int i = 1; i < 6; i++)
		{
			if (i <= array.Length)
			{
				IconObj[i].SetActive(value: true);
				string text = array[i - 1].Split('_')[1];
				string str = array[i - 1].Split('_')[0];
				IconObj[i].transform.Find("NumText" + i).GetComponent<Text>().text = text;
				IconObj[i].transform.Find("Icon" + i).GetComponent<Image>().sprite = Util.GetResourcesSprite("ShopImage/ShopIcon_" + str);
				IconObj[i].transform.Find("Icon" + i).GetComponent<Image>().SetNativeSize();
			}
			else
			{
				IconObj[i].SetActive(value: false);
			}
		}
	}

	public void OnPayButton()
	{
		AudioPlayManager.PlaySFX2D("button");
		AndroidManager.Instance.Pay(Paykey);
	}
}
