using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PathologicalGames;

public class MissionWindow : UIWindowBase
{
    public Transform container;
    public ScrollRect itemScrollRect;

    private SpawnPool itemPool;

    private List<MissionItem> itemList = new List<MissionItem>();
    private int itemHeight = 170;

    public static MissionWindow Instance;

    public override void OnInit()
    {
        Instance = this;
        itemPool = PoolManager.Pools["UIPool"];
        base.OnInit();

        MissionInfoParam missionInfo = PlayerData.Instance.GetMissionInfo();
        for (int i = 0; i < missionInfo.countList.Count; ++i)
        {
            Transform itemTran = itemPool.Spawn("MissionItem");
            itemTran.SetParent(container);
            itemTran.localScale = Vector3.one;
            itemTran.gameObject.SetActive(true);

            MissionItem itemScript = itemTran.GetComponent<MissionItem>();
            itemList.Add(itemScript);
        }
    }

    public override void OnOpen()
    {
        ResreshItem();
        float percent = 1f;
        float curPos = itemScrollRect.horizontalNormalizedPosition;
        DOTween.To(() => curPos, x => curPos = x, percent, 0.2f).OnUpdate(() =>
        {
            itemScrollRect.verticalNormalizedPosition = curPos;
        });
    }

    public void ResreshItem()
    {
        MissionInfoParam missionInfo = PlayerData.Instance.GetMissionInfo();
        int missionCount = missionInfo.countList.Count;
        for (int i = 0; i < missionCount; i++)
        {
            itemList[i].Init(i + 1);
        }

        RectTransform rectTransform = container.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, itemHeight * missionCount + 20);

        itemList.Sort((a, b) =>
        {
            int stateRes = a.missionState.CompareTo(b.missionState);
            if (stateRes != 0)
            {
                return stateRes;
            }
            else
            {
                return a.id.CompareTo(b.id);
            }
        });

        for (int k = 0; k < itemList.Count; ++k)
        {
            itemList[k].transform.SetSiblingIndex(k);
        }
    }

    public void OnCloseBtn()
    {
        UIManager.CloseUIWindow<MissionWindow>(isPlayAnim: true, null, new object[0]);
        if (!PlayerData.Instance.IsMissionGuide && PlayerData.Instance.guideStep == 4)
        {
            PlayerData.Instance.IsMissionGuide = true;
            Singleton<LevelManager>.Instance.SetNowSelectLevel(Singleton<UserData>.Instance.GetPassLevel() + 1);
            UIManager.OpenUIWindow<PlayWindow>();
            PlayerData.Instance.guideStep = 0;
        }
    }
}
