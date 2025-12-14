
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TooSimpleFramework.UI;

public class DayObjInit : MonoBehaviour
{
    public Text IconNumText;

    public Text DayNumText;

    public Image IconImage;

    public GameObject OKobj;

    public GameObject HuanObj;
    public Image bg;
    public Sprite normal, select;

    public Image titleImage;
    public Sprite normalTitle, slectTitle;

    public OutlineEx titleOutLine;

    private int rewardType;

    private int rewardNum;
    private int index;

    public void InitSignDay(int index)
    {
        this.index = index;
        base.gameObject.SetActive(value: true);
        HuanObj.SetActive(value: false);
        rewardType = int.Parse(Singleton<DataSignmap7>.Instance.GetContentByKeyAndType((Signmap7Key)Enum.ToObject(typeof(Signmap7Key), index), Signmap7Type.reward));
        rewardNum = int.Parse(Singleton<DataSignmap7>.Instance.GetContentByKeyAndType((Signmap7Key)Enum.ToObject(typeof(Signmap7Key), index), Signmap7Type.rewardnum));
        IconNumText.text = "x" + rewardNum.ToString();
        IconImage.sprite = Util.GetResourcesSprite("IconImage/icon_" + rewardType);
        IconImage.SetNativeSize();
        DayNumText.text = Util.ReplaceText(GameEntry.Instance.GetString("SigninUI" + (index + 1).ToString()));
        int intRecord = RecordManager.GetIntRecord("LevelData", "SignCount", 0);
        string nowTime_Day = Util.GetNowTime_Day();
        int intRecord2 = RecordManager.GetIntRecord("LevelData", "iSignOK" + nowTime_Day, 0);
        if (intRecord >= index + 1)
        {
            OKobj.SetActive(value: true);
            if (intRecord == 7 && intRecord2 == 0)
            {
                OKobj.SetActive(value: false);
            }
        }
        else
        {
            OKobj.SetActive(value: false);
        }

        // bg.sprite = select;
    }

    public void CheckIsSign()
    {
        OKobj.SetActive(value: true);
        HuanObj.SetActive(value: false);
        GameObject gameObject = OKobj.transform.Find("Image").gameObject;
        gameObject.transform.DOScale(new Vector2(1.4f, 1.4f), 0.2f).SetEase(Ease.OutSine);
        gameObject.transform.DOScale(new Vector2(1f, 1f), 0.2f).SetEase(Ease.OutSine).SetDelay(0.2f);
        gameObject.transform.DOScale(new Vector2(1.2f, 1.2f), 0.2f).SetEase(Ease.OutSine).SetDelay(0.4f);
        gameObject.transform.DOScale(new Vector2(1f, 1f), 0.2f).SetEase(Ease.OutSine).SetDelay(0.6f);
    }

    public void ShowHuanObj()
    {
        HuanObj.SetActive(value: true);
        bg.sprite = select;
        titleImage.sprite = slectTitle;
        titleOutLine.OutlineColor = new Color32(236, 119, 23, 255);
        titleOutLine.gameObject.SetActive(false);
        Invoke("ShowText", 0.02f);
    }

    public void Reward()
    {
        List<RewardType> list = new List<RewardType>();
        list.Add(new RewardType(rewardType, rewardNum));
        UIManager.Reward(list);
        bg.sprite = normal;
        titleImage.sprite = normalTitle;
        titleOutLine.OutlineColor = new Color32(62, 68, 111, 255);
        titleOutLine.gameObject.SetActive(false);
        Invoke("ShowText", 0.02f);
        CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.SignUp);
    }

    public void ADReward()
    {
        List<RewardType> list = new List<RewardType>();
        list.Add(new RewardType(rewardType, rewardNum * 3));
        list.Add(new RewardType(2, (index + 1) * 200));
        UIManager.Reward(list);
        bg.sprite = normal;
        titleImage.sprite = normalTitle;
        titleOutLine.OutlineColor = new Color32(62, 68, 111, 255);
        titleOutLine.gameObject.SetActive(false);
        Invoke("ShowText", 0.02f);
    }

    private void ShowText()
    {
        titleOutLine.gameObject.SetActive(true);
        titleOutLine._Refresh();
    }
}
