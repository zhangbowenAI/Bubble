
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuyGuangUIWindow : UIWindowBase
{
	public Text TitleText;

	public Image LeftImage;

	public Image RightImage;

	public Text Miaoshu;

	public Text PayBtnNum;

	public Text GoldNum;

	public Sprite[] LeftSprite;

	public Sprite[] RightSprite;

	public override void OnOpen()
	{
		InitUI();
		AddOnClickListener("CloseButton", CloseButton);
		AddOnClickListener("PayButton", OnPayBtn);
		AddEventListener(GameEventEnum.UIFlushChane, ReceviceGoldChange);
	}

	private void ReceviceGoldChange(params object[] objs)
	{
		GoldNum.text = Singleton<UserData>.Instance.GetUserGold().ToString();
	}

	public void InitUI()
	{
		LeftImage.sprite = LeftSprite[Singleton<UserData>.Instance.BuyMagicType - 1];
		RightImage.sprite = RightSprite[Singleton<UserData>.Instance.BuyMagicType - 1];
		Miaoshu.text = Util.ReplaceText(GameEntry.Instance.GetString("BuyGangRemark" + Singleton<UserData>.Instance.BuyMagicType.ToString()));
		TitleText.text = Util.ReplaceText(GameEntry.Instance.GetString("BuyGangTypeRemark" + Singleton<UserData>.Instance.BuyMagicType.ToString()));
		GoldNum.text = Singleton<UserData>.Instance.GetUserGold().ToString();
		PayBtnNum.text = Util.ReplaceText(Singleton<DataSkillPrice>.Instance.GetContentByKeyAndType("A" + (Singleton<UserData>.Instance.BuyMagicType + 5).ToString(), SkillPriceType.numb1));
	}

	public void CloseButton(InputUIOnClickEvent e)
	{
		UIManager.CloseUIWindow<BuyGuangUIWindow>(isPlayAnim: true, null, new object[0]);
	}

	public void OnPayBtn(InputUIOnClickEvent e)
	{
		int num = int.Parse(PayBtnNum.text);
		if (Singleton<UserData>.Instance.GetUserGold() >= num)
		{
			Singleton<UserData>.Instance.DelUserGold(num);
			switch (Singleton<UserData>.Instance.BuyMagicType)
			{
			case 1:
				GameScene.Instance.FZ1.FullFZ();
				break;
			case 2:
				GameScene.Instance.FZ2.FullFZ();
				break;
			case 3:
				GameScene.Instance.FZ3.FullFZ();
				break;
			case 4:
				GameScene.Instance.FZ4.FullFZ();
				break;
			}
			UIManager.CloseUIWindow<BuyGuangUIWindow>(isPlayAnim: true, null, new object[0]);
		}
		else
		{
			UIManager.OpenUIWindow<BuyGoldUIWindow>();
		}
	}

	public override void OnRefresh()
	{
	}

	public override IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
	{
		StartCoroutine(base.EnterAnim(animComplete, callBack, objs));
		yield return new WaitForEndOfFrame();
	}

	public override IEnumerator ExitAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
	{
		StartCoroutine(base.ExitAnim(animComplete, callBack, objs));
		yield return new WaitForEndOfFrame();
	}

	public override IEnumerator PauserAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
	{
		StartCoroutine(base.PauserAnim(animComplete, callBack, objs));
		yield return new WaitForEndOfFrame();
	}
}
