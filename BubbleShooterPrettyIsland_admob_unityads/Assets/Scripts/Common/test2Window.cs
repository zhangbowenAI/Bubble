
using System.Collections;
using UnityEngine;

public class test2Window : UIWindowBase
{
	public override void OnOpen()
	{
		AddOnClickListener("CloseButton", CloseBtnUI);
	}

	public override void OnRefresh()
	{
	}

	public void CloseBtnUI(InputUIOnClickEvent e)
	{
		UIManager.CloseUIWindow<test2Window>(isPlayAnim: true, null, new object[0]);
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
