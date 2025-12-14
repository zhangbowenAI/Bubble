
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinousWinWindow : UIWindowBase
{
    [SerializeField]
    private GameObject mRewardHint0;

    [SerializeField]
    private GameObject mRewardHint1;

    [SerializeField]
    private Text mTapHint;

    [SerializeField]
    private GameObject[] mNextLevelItems;

    private List<GameObject> mNextRewardGoList = new List<GameObject>();

    private float fTimer;

    private bool bClose;

    public override void OnOpen()
    {
        GetComponent<ContinuousItemArranger>().Init();
        fTimer = 0f;
        bClose = false;
        mTapHint.text = Util.ReplaceText(GameEntry.Instance.GetString("LoseUILevelRestartText"));
        if (ContinuousItemArranger.gSequenceCount > 0)
        {
            mRewardHint0.SetActive(value: true);
            mRewardHint1.SetActive(value: true);
            mRewardHint0.GetComponent<Text>().text = Util.ReplaceTextLine(GameEntry.Instance.GetString("ContinuousWinHint1"));
            mRewardHint1.GetComponent<Text>().text = Util.ReplaceTextLine(GameEntry.Instance.GetString("ContinuousWinHint2"));
            for (int i = 0; i < mNextLevelItems.Length; i++)
            {
                mNextLevelItems[i].SetActive(i < ContinuousItemArranger.gSequenceCount);
            }
            switch (ContinuousItemArranger.gSequenceCount)
            {
                case 1:
                    mNextLevelItems[0].transform.localPosition = new Vector3(0f, -313f, 0f);
                    break;
                case 2:
                    mNextLevelItems[0].transform.localPosition = new Vector3(-50f, -313f, 0f);
                    mNextLevelItems[1].transform.localPosition = new Vector3(50f, -313f, 0f);
                    break;
                case 3:
                    mNextLevelItems[0].transform.localPosition = new Vector3(-100f, -313f, 0f);
                    mNextLevelItems[1].transform.localPosition = new Vector3(0f, -313f, 0f);
                    mNextLevelItems[2].transform.localPosition = new Vector3(100f, -313f, 0f);
                    break;
            }
        }
        else
        {
            mRewardHint0.SetActive(value: false);
            mRewardHint1.SetActive(value: false);
        }
    }

    private void Update()
    {
        fTimer += Time.deltaTime;
        if (!bClose && Input.GetMouseButtonDown(0) && fTimer > (float)ContinuousItemArranger.gSequenceCount)
        {
            bClose = true;
            WinPanelUI winPanel = Object.FindObjectOfType<WinPanelUI>();
            winPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 0f), 0.8f).OnComplete(delegate
            {
                winPanel.InitUI();
            });
            foreach (GameObject mNextRewardGo in mNextRewardGoList)
            {
                UnityEngine.Object.Destroy(mNextRewardGo);
            }
            mNextRewardGoList.Clear();
            UIManager.CloseUIWindow(this, true, null);
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
