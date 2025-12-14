
using DG.Tweening;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class jinzuUIWindow : UIWindowBase
{
    public static jinzuUIWindow Instance;

    public GameObject jinzuSpine;

    public GameObject jinzuPanel;

    public GameObject jinzuBacj;

    public GameObject GoumaiSpine;

    private GameObject _GoumaiSpineObj;

    public Text goldNum;

    public GameObject PayButton;

    public Text Paytext;

    public Text Miaosu;

    public Text TitleText;
    public GameObject closeBtn, close;

    public override void OnOpen()
    {
        InitUI();
        AddOnClickListener("CloseButton", OnClickClose);
        AddOnClickListener("BuyButton", OnPayJinzu);
        ADInterface.Instance.SendADEvent(ADType.CPAD, ADSpot.PiggyPanel);
    }

    public void InitUI()
    {
        Instance = this;
        jinzuPanel.SetActive(value: true);
        jinzuBacj.SetActive(value: false);
        goldNum.text = Singleton<UserData>.Instance.GetJinZhuGold() + "/6000";
        jinzuSpine.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "jinzhu_appear", loop: false);
        jinzuSpine.GetComponent<SkeletonGraphic>().AnimationState.Complete += delegate
        {
            jinzuSpine.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "jinzhu_stand", loop: true);
        };
        if (Singleton<UserData>.Instance.GetJinZhuGold() >= 4500)
        {
            PayButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            PayButton.GetComponent<Button>().interactable = false;
        }
        closeBtn.SetActive(!PayButton.GetComponent<Button>().interactable);
        close.SetActive(PayButton.GetComponent<Button>().interactable);
        InitLanguage();
    }

    public void InitLanguage()
    {
        string str = string.Empty;
        if (GameEntry.buildType == GameEntry.BuildType.GoogleEN)
        {
            str = Singleton<DataPurchase>.Instance.GetContentByKeyAndType("Piggybank", PurchaseType.iMoney);
        }

        // Paytext.text = Util.ReplaceText(GameEntry.Instance.GetString("Piggybutton")) + str;
        Paytext.text = Util.ReplaceText(GameEntry.Instance.GetString("Piggybutton"));
        if (Singleton<UserData>.Instance.GetJinZhuGold() >= 6000)
        {
            Miaosu.text = Util.ReplaceText(GameEntry.Instance.GetString("Piggyremark2"));
        }
        else
        {
            Miaosu.text = Util.ReplaceText(GameEntry.Instance.GetString("Piggyremark1"));
        }
        TitleText.text = Util.ReplaceText(GameEntry.Instance.GetString("Piggytitle"));
        Sequence s = DOTween.Sequence();
        s.Append(TitleText.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.3f).SetEase(Ease.OutSine)).Append(TitleText.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.3f).SetEase(Ease.InSine)).Append(TitleText.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).SetEase(Ease.OutSine));
    }

    public void OnClickClose(InputUIOnClickEvent e)
    {
        UIManager.CloseUIWindow<jinzuUIWindow>(isPlayAnim: true, null, new object[0]);
    }

    public void CloseBtnOnclick()
    {
        UIManager.CloseUIWindow<jinzuUIWindow>(isPlayAnim: true, null, new object[0]);
    }

    public void OnPayJinzu(InputUIOnClickEvent e)
    {
        // string contentByKeyAndType = Singleton<DataPurchase>.Instance.GetContentByKeyAndType("Piggybank", PurchaseType.GooglePay);
        // if (Application.platform == RuntimePlatform.IPhonePlayer)
        // {
        //     contentByKeyAndType = Singleton<DataPurchase>.Instance.GetContentByKeyAndType("Piggybank", PurchaseType.IOS);
        // }
        // AndroidManager.Instance.Pay(contentByKeyAndType);
        ADInterface.Instance.SendADVideoEvent(ADType.RewardVideoAD, ADSpot.Piggybank, PayBack);

    }

    public void PayBack(bool result)
    {
        if (result)
        {
            jinzuPanel.SetActive(value: false);
            jinzuBacj.SetActive(value: true);
            RecordManager.SaveRecord("UserData", "JinZhuMan", 0);
            _GoumaiSpineObj = Object.Instantiate(GoumaiSpine);
            _GoumaiSpineObj.transform.SetParent(jinzuBacj.gameObject.transform, worldPositionStays: false);
            _GoumaiSpineObj.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "jinzhu_goumai", loop: false);
            _GoumaiSpineObj.GetComponent<SkeletonGraphic>().AnimationState.Event += State_Event;
            _GoumaiSpineObj.transform.Find("fx_jinzhu_goumai").gameObject.SetActive(value: true);

            CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.AD_GetDepositMoney);
        }
    }

    private void State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
    {
        if (e.Data.Name == "boom")
        {
            _GoumaiSpineObj.transform.Find("fx_jinzhu_goumai_boom").gameObject.SetActive(value: true);
            UnityEngine.Debug.Log("Bomm =========");
            StartCoroutine(GetReward());
        }
    }

    private IEnumerator GetReward()
    {
        yield return new WaitForSeconds(0.3f);
        int num = Singleton<UserData>.Instance.GetJinZhuGold();
        UIManager.Reward(new List<RewardType>
        {
            new RewardType(2, num)
        });
        Singleton<UserData>.Instance.ClearJinZhuGold();
        yield return new WaitForSeconds(0.5f);
        if ((bool)_GoumaiSpineObj)
        {
            UnityEngine.Object.Destroy(_GoumaiSpineObj);
        }
        InitUI();
    }

    public override void OnRefresh()
    {
    }

    public override IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        animComplete(this, callBack, objs);
        yield return new WaitForEndOfFrame();
    }

    public override IEnumerator ExitAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        animComplete(this, callBack, objs);
        yield return new WaitForEndOfFrame();
    }

    public override IEnumerator PauserAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        animComplete(this, callBack, objs);
        yield return new WaitForEndOfFrame();
    }
}
