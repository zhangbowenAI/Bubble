
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ContinuousPanelWindow : UIWindowBase
{
    [SerializeField]
    private Text mHint;

    [SerializeField]
    private Text mTitle;

    [SerializeField]
    private Text mConfirm;

    public override void OnOpen()
    {
        GetComponent<ContinuousItemArranger>().Init();
        GetComponent<Canvas>().overrideSorting = true;
        mHint.text = Util.ReplaceTextLine(GameEntry.Instance.GetString("ContinuousPanelHint"));
        mTitle.text = GameEntry.Instance.GetString("ContinuousPanelTitle");
        mConfirm.text = GameEntry.Instance.GetString("Confirm");
    }

    public override void OnRefresh()
    {
    }

    public void OnConfirm()
    {
        UIManager.CloseUIWindow(this, true, null);
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
