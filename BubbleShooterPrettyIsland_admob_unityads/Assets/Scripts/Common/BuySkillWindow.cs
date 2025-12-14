
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySkillWindow : UIWindowBase
{
	public Sprite[] CenterImage;

	public Image CenterBG;

	public List<GameObject> BuySkillList = new List<GameObject>();

	public GameObject BuySkillItem;

	public Text GoldNum;

	public Text Title;

	public Text CenterText;

	public override void OnOpen()
	{
		InitUI();
		AddOnClickListener("CloseButton", CloseButton);
		AddEventListener(GameEventEnum.UIFlushChane, ReceviceGoldChange);
		InitLanguage();
	}

	public void InitLanguage()
	{
		Title.text = Util.ReplaceText(GameEntry.Instance.GetString("BuySkillTitle"));
		CenterText.text = Util.ReplaceText(GameEntry.Instance.GetString("BuySkillRemark" + (Singleton<UserData>.Instance.buySkillType + 1).ToString()));
	}

	private void ReceviceGoldChange(params object[] objs)
	{
		GoldNum.text = Singleton<UserData>.Instance.GetUserGold().ToString();
	}

	public void CloseButton(InputUIOnClickEvent e)
	{
		UIManager.CloseUIWindow<BuySkillWindow>(isPlayAnim: true, null, new object[0]);
	}

	public void InitUI()
	{
		CenterBG.sprite = CenterImage[Singleton<UserData>.Instance.buySkillType];
		GoldNum.text = Singleton<UserData>.Instance.GetUserGold().ToString();
		if (BuySkillList.Count > 0)
		{
			for (int i = 0; i < BuySkillList.Count; i++)
			{
				BuySkillList[i].GetComponent<BuySkillItem>().InitBuySkill(i);
			}
			return;
		}
		for (int j = 0; j < 3; j++)
		{
			GameObject gameObject = Object.Instantiate(BuySkillItem);
			gameObject.transform.SetParent(BuySkillItem.transform.parent, worldPositionStays: false);
			gameObject.SetActive(value: true);
			BuySkillItem component = gameObject.GetComponent<BuySkillItem>();
			component.InitBuySkill(j);
			BuySkillList.Add(gameObject);
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
