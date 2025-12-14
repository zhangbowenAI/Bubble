
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CDkeyUIWindow : UIWindowBase
{
	public InputField levelText;

	public InputField tiliText;

	public override void OnOpen()
	{
		AddOnClickListener("Close", CloseButton);
		AddOnClickListener("CoinSure", CoinSureButton);
		AddOnClickListener("LevelSure", LevelSureButton);
		AddOnClickListener("tiliSure", TiLiSureButton);
		AddOnClickListener("DataSure", DataButton);
	}

	public void CloseButton(InputUIOnClickEvent e)
	{
		UIManager.CloseUIWindow<CDkeyUIWindow>(isPlayAnim: false, null, new object[0]);
	}

	public void DataButton(InputUIOnClickEvent e)
	{
		RecordManager.CleanAllRecord();
		GameEntry.ChangeScene(GameEntry.MainScene);
	}

	public void TiLiSureButton(InputUIOnClickEvent e)
	{
		int num = int.Parse(tiliText.text.ToString());
		if (num < 0)
		{
			Singleton<UserData>.Instance.DelLoveCount(-num);
		}
		else
		{
			Singleton<UserData>.Instance.AddLoveCount(num);
		}
	}

	public void CoinSureButton(InputUIOnClickEvent e)
	{
		Singleton<UserData>.Instance.AddUserGold(10000);
	}

	public void LevelSureButton(InputUIOnClickEvent e)
	{
		Singleton<UserData>.Instance.SetPassLevel(int.Parse(levelText.text.ToString()));
		Singleton<MapData>.Instance.InitMapData();
		for (int i = 1; i <= int.Parse(levelText.text.ToString()); i++)
		{
			RecordManager.SaveRecord("LevelData", "LevelStar_" + i, 3);
		}
		int mapForLevelID = Singleton<MapData>.Instance.GetMapForLevelID(int.Parse(levelText.text.ToString()));
		Debug.LogError(mapForLevelID);
		Singleton<MapData>.Instance.iNowMapID = mapForLevelID - 1;
		RecordManager.SaveRecord("LevelData", "iNowMapID", mapForLevelID - 1);
		GameEntry.ChangeScene(GameEntry.LevelScene);
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
