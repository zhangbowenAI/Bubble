
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ExitUIWindow : UIWindowBase
{
	public Text SureText;

	public Text playOnText;

	public Text CenterText;

	[SerializeField]
	private GameObject[] mLoseRewardItem;

	[SerializeField]
	private GameObject mHintText;

	public override void OnOpen()
	{
		InitUI();
		AddOnClickListener("CloseButton", OnCloseBtn);
		AddOnClickListener("SureButton", OnSureBtn);
		AddOnClickListener("ContinueButton", OnContinueBtn);
		mHintText.SetActive(value: false);
		if (ContinuousItemArranger.gSequenceCount > 0 && Singleton<LevelManager>.Instance.GetNowLevel() > Singleton<UserData>.Instance.GetPassLevel())
		{
			mHintText.SetActive(value: true);
			mHintText.GetComponent<Text>().text = GameEntry.Instance.GetString("ContinuousWinLoseHint");
			for (int i = 0; i < mLoseRewardItem.Length; i++)
			{
				mLoseRewardItem[i].SetActive(i < ContinuousItemArranger.gSequenceCount);
			}
		}
	}

	public void InitUI()
	{
		InitLanguage();
	}

	public void InitLanguage()
	{
		if (Singleton<UserData>.Instance.IsQuit)
		{
			SureText.text = Util.ReplaceText(GameEntry.Instance.GetString("QuitGame2"));
		}
		else
		{
			SureText.text = Util.ReplaceText(GameEntry.Instance.GetString("QuitGame3"));
		}
		CenterText.text = Util.ReplaceText(GameEntry.Instance.GetString("QuitGame1"));
		playOnText.text = Util.ReplaceText(GameEntry.Instance.GetString("QuitGame4"));
	}

	public void OnCloseBtn(InputUIOnClickEvent e)
	{
		if (Singleton<UserData>.Instance.IsQuit)
		{
			AndroidManager.Instance.UpLevelData("pause_quit_play_on");
		}
		else
		{
			AndroidManager.Instance.UpLevelData("pause_retry_play_on");
		}
		UIManager.CloseUIWindow<ExitUIWindow>(isPlayAnim: true, null, new object[0]);
	}

	public void OnSureBtn(InputUIOnClickEvent e)
	{
		if (Singleton<LevelManager>.Instance.GetNowLevel() > Singleton<UserData>.Instance.GetPassLevel())
		{
			ContinuousItemArranger.gSequenceCount = 0;
			RecordManager.SaveRecord("LevelData", "SequenceWinCount", ContinuousItemArranger.gSequenceCount);
		}
		if (Singleton<UserData>.Instance.IsQuit)
		{
			AndroidManager.Instance.UpLevelData("pause_quit_quit");
			AndroidManager.Instance.ShowInterstitialAd();
			UIManager.CloseAllUI();
			GameEntry.ChangeScene(GameEntry.LevelScene);
		}
		else
		{
			AndroidManager.Instance.UpLevelData("pause_retry_retry");
			AndroidManager.Instance.ShowInterstitialAd();
			UIManager.OpenUIWindow<PlayWindow>();
		}
	}

	public void OnContinueBtn(InputUIOnClickEvent e)
	{
		UIManager.CloseAllUI();
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
