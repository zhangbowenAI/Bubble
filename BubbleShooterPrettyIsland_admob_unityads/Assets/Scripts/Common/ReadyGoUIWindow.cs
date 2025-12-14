
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ReadyGoUIWindow : UIWindowBase
{
	public GameObject ReadyGo1;

	public GameObject ReadyGo2;

	public override void OnOpen()
	{
		if (GameEntry.buildType == GameEntry.BuildType.GoogleEN)
		{
			ReadyGo1.SetActive(value: true);
			ReadyGo2.SetActive(value: false);
		}
		else
		{
			ReadyGo1.SetActive(value: false);
			ReadyGo2.SetActive(value: true);
		}
		StartCoroutine(CloseUI());
	}

	public IEnumerator CloseUI()
	{
		yield return new WaitForSeconds(1f);
		UIManager.CloseUIWindow<ReadyGoUIWindow>(isPlayAnim: true, null, new object[0]);
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
		Tweener tweener = GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, -Screen.height), 0.3f);
		tweener.OnComplete(delegate
		{
			animComplete(this, callBack, objs);
			GameScene.Instance.ReadyGoEnd();
		});
		yield return new WaitForEndOfFrame();
	}

	public override IEnumerator PauserAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
	{
		StartCoroutine(base.PauserAnim(animComplete, callBack, objs));
		yield return new WaitForEndOfFrame();
	}
}
