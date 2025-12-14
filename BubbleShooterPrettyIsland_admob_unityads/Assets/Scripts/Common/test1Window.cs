
using System.Collections;
using UnityEngine;

public class test1Window : UIWindowBase
{
	public override void OnOpen()
	{
		AddOnClickListener("CloseButton", OnClickClose);
	}

	public override void OnRefresh()
	{
	}

	public void OnClickClose(InputUIOnClickEvent e)
	{
		UIManager.OpenUIWindow<test2Window>();
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
