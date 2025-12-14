
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyLoveUIWindow : UIWindowBase
{
	public Text LoveCountText;

	public Text FullText;

	public Text TimeText;

	public Text NextLoveText;

	public GameObject LoveObj;

	public List<GameObject> LoveList = new List<GameObject>();

	public Text Title;

	public override void OnOpen()
	{
		AddOnClickListener("CloseButton", CloseButton);
		InitUI();
		InitLanguage();
		ADInterface.Instance.SendADEvent(ADType.NativeStartAD,ADSpot.BuyLoveNaticeAd);
	}

	public void InitLanguage()
	{
		NextLoveText.text = Util.ReplaceText(GameEntry.Instance.GetString("BuyLives"));
		Title.text = Util.ReplaceText(GameEntry.Instance.GetString("BuyLivestitle"));
	}

	public void CloseButton(InputUIOnClickEvent e)
	{
		if (ApplicationManager.SceneName == GameEntry.GameScene && Singleton<UserData>.Instance.getLoveInfinite() == 0 && Singleton<UserData>.Instance.GetUserLoveCount() == 0)
		{
			Singleton<UserData>.Instance.PlaySkillUse1 = false;
			Singleton<UserData>.Instance.PlaySkillUse2 = false;
			Singleton<UserData>.Instance.PlaySkillUse3 = false;
			UIManager.CloseAllUI();
			GameEntry.ChangeScene(GameEntry.LevelScene);
		}
		else
		{
			UIManager.CloseUIWindow<BuyLoveUIWindow>(isPlayAnim: true, null, new object[0]);
		}
		ADInterface.Instance.SendADEvent(ADType.NativeEndAD,ADSpot.BuyLoveNaticeAd);
	}

	public void InitUI()
	{
		if (LoveList.Count <= 0)
		{
			for (int i = 0; i < 3; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(LoveObj);
				gameObject.transform.SetParent(LoveObj.gameObject.transform.parent, worldPositionStays: false);
				LoveItem component = gameObject.GetComponent<LoveItem>();
				component.InitData(i);
				LoveList.Add(gameObject);
			}
		}
		else
		{
			for (int j = 0; j < LoveList.Count; j++)
			{
				LoveList[j].GetComponent<LoveItem>().InitData(j);
			}
		}
		int userLoveCount = Singleton<UserData>.Instance.GetUserLoveCount();
		LoveCountText.text = userLoveCount.ToString();
		StartCoroutine(UpdateTime());
	}

	private IEnumerator UpdateTime()
	{
		bool b = true;
		while (b)
		{
			int LoveNum = Singleton<UserData>.Instance.GetUserLoveCount();
			LoveCountText.text = LoveNum.ToString();
			if (LoveNum >= Singleton<UserData>.Instance.iLoveMaxAll)
			{
				FullText.gameObject.SetActive(value: true);
				FullText.text = Util.ReplaceText(GameEntry.Instance.GetString("BuyLives4"));
				NextLoveText.gameObject.SetActive(value: false);
				TimeText.gameObject.SetActive(value: false);
			}
			else
			{
				FullText.gameObject.SetActive(value: false);
				NextLoveText.gameObject.SetActive(value: true);
				TimeText.gameObject.SetActive(value: true);
				int intRecord = RecordManager.GetIntRecord("UserData", "FullLoveTime", 0);
				int nowTime = Util.GetNowTime();
				if (nowTime > intRecord)
				{
					RecordManager.SaveRecord("UserData", "LoveCount", Singleton<UserData>.Instance.iLoveMaxAll);
					RecordManager.SaveRecord("UserData", "FullLoveTime", 0);
				}
				int num = intRecord - nowTime;
				int num2 = 0;
				while (num > Singleton<UserData>.Instance.ResLoveTime)
				{
					num2++;
					num -= Singleton<UserData>.Instance.ResLoveTime;
				}
				RecordManager.SaveRecord("UserData", "LoveCount", Singleton<UserData>.Instance.iLoveMaxAll - num2 - 1);
				TimeSpan timeSpan = new TimeSpan(0, 0, num);
				int minutes = timeSpan.Minutes;
				int seconds = timeSpan.Seconds;
				string text = minutes + string.Empty;
				string text2 = seconds + string.Empty;
				if (minutes < 10)
				{
					text = "0" + text;
				}
				if (seconds < 10)
				{
					text2 = "0" + text2;
				}
				TimeText.text = text + ":" + text2;
			}
			yield return new WaitForSeconds(1f);
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
