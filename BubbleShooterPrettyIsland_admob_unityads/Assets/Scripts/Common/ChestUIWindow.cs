
using DG.Tweening;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestUIWindow : UIWindowBase
{
    public GameObject ChestCloseObj;

    public GameObject ChestOpenObj;

    [SerializeField]
    private Button mClaimButton;

    public Text CenterText;

    public Text TitleText;

    public Text ClaimText;

    public GameObject baoxiang;

    public GameObject baoxiang_star;

    public GameObject[] baoxiang_light;

    public GameObject[] BoxObj;

    private bool isCanClick;

    private List<int> RewardNum = new List<int>();

    private List<int> RewardImg = new List<int>();

    public int count;

    public Transform box;
    public Sprite closeBox, openBox;

    private int BeforeIndex;

    private bool canOpen = false;

    public override void OnOpen()
    {
        InitUI();
        AddOnClickListener("CloseButton", CloseButton);
        AddOnClickListener("ContinueButton", AdBtnClick);
        AddOnClickListener("ClaimButton", ClaimButton);
    }

    public void InitUI()
    {
        count = 0;
        RewardNum.Clear();
        RewardImg.Clear();
        isCanClick = false;
        mClaimButton.gameObject.SetActive(!isCanClick);
        ClaimText.text = GameEntry.Instance.GetString("ChestClaim");
        for (int i = 1; i <= 9; i++)
        {
            RewardImg.Add(int.Parse(Singleton<DataChestBox>.Instance.GetContentByKeyAndType("A" + i.ToString(), ChestBoxType.reward)));
            RewardNum.Add(int.Parse(Singleton<DataChestBox>.Instance.GetContentByKeyAndType("A" + i.ToString(), ChestBoxType.rewardnum)));
        }
        //bool flag = false;
        if (((!(ApplicationManager.SceneName == GameEntry.GameScene)) ? LevelScene.Instance.BoxOpenObj.gameObject.activeSelf : GameScene.Instance.WinPanel.GetComponent<WinPanelUI>().BoxOpenObj.gameObject.activeSelf) || canOpen)
        {
            if (!canOpen)
            {
                CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.GetBoxReward);
            }
            canOpen = false;
            ChestOpenObj.SetActive(value: true);
            ChestCloseObj.SetActive(value: false);
            for (int j = 0; j < 6; j++)
            {
                BoxObj[j].SetActive(value: false);
                baoxiang_light[j].SetActive(value: false);
            }
            isCanClick = true;
            mClaimButton.gameObject.SetActive(!isCanClick);
            // baoxiang.GetComponent<SkeletonAnimation>().Initialize(overwrite: true);
            // baoxiang.GetComponent<SkeletonAnimation>().state.Event += State_Event;
            box.GetComponent<Image>().sprite = closeBox;
            box.GetComponent<Image>().SetNativeSize();
            Sequence seq = DOTween.Sequence();
            seq.Append(box.DOBlendableLocalRotateBy(new Vector3(0, 0, 10), 0.2f));
            seq.Append(box.DOBlendableLocalRotateBy(new Vector3(0, 0, -20), 0.2f));
            seq.Append(box.DOBlendableLocalRotateBy(new Vector3(0, 0, 10), 0.2f));
            seq.AppendInterval(0.3f);
            seq.SetLoops(3);
            seq.OnComplete(() =>
            {
                box.GetComponent<Image>().sprite = openBox;
                box.GetComponent<Image>().SetNativeSize();
                InvokeRepeating("State_Event", 0, 0.5f);
            });
            return;
        }
        ChestOpenObj.SetActive(value: false);
        ChestCloseObj.SetActive(value: true);
        string @string = GameEntry.Instance.GetString("ChestClose");
        TitleText.text = GameEntry.Instance.GetString("ChestTitle");
        ClaimText.text = GameEntry.Instance.GetString("ChestClaim");
        int intRecord = RecordManager.GetIntRecord("UserData", "ChestGrade", 0);
        int intRecord2 = RecordManager.GetIntRecord("UserData", "ChestCount", 0);
        switch (intRecord)
        {
            case 0:
                intRecord2 = 5 - intRecord2;
                break;
            case 1:
                intRecord2 = 10 - intRecord2;
                break;
            default:
                intRecord2 = 15 - intRecord2;
                break;
        }
        CenterText.text = Util.ReplaceText(@string.Replace("*", intRecord2.ToString()));
        ADInterface.Instance.SendADEvent(ADType.NativeStartAD, ADSpot.BoxPanelNaticeAd);
    }

    private void State_Event()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(box.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f));
        seq.Append(box.DOScale(Vector3.one, 0.1f));
        baoxiang_light[count].SetActive(value: true);
        CheckReward(count);
        count++;
        if (count == 6)
        {
            CancelInvoke("State_Event");
        }
    }

    public void SaveData()
    {
        isCanClick = false;
        mClaimButton.gameObject.SetActive(!isCanClick);
        int intRecord = RecordManager.GetIntRecord("UserData", "ChestGrade", 0);
        int intRecord2 = RecordManager.GetIntRecord("UserData", "ChestCount", 0);
        switch (intRecord)
        {
            case 0:
                intRecord2 -= 5;
                break;
            case 1:
                intRecord2 -= 10;
                break;
            default:
                intRecord2 -= 15;
                break;
        }
        if (intRecord2 < 0)
            intRecord2 = 0;
        RecordManager.SaveRecord("UserData", "ChestCount", intRecord2);
        intRecord++;
        RecordManager.SaveRecord("UserData", "ChestGrade", intRecord);
        GlobalEvent.DispatchEvent(GameEventEnum.UIFlushChane);
    }

    public void CheckReward(int index)
    {
        BoxObj[index].SetActive(value: true);
        AudioPlayManager.PlaySFX2D("ui_reward");
        RewardObj(BoxObj[index], index);
        BoxObj[index].transform.localScale = new Vector3(0f, 0f, 0f);
        BoxObj[index].transform.DOScale(new Vector2(1.2f, 1.2f), 0.15f).SetEase(Ease.OutSine);
        BoxObj[index].transform.DOScale(new Vector2(0.8f, 0.8f), 0.15f).SetEase(Ease.OutSine).SetDelay(0.15f);
        BoxObj[index].transform.DOScale(new Vector2(1.1f, 1.1f), 0.15f).SetEase(Ease.OutSine).SetDelay(0.3f);
        BoxObj[index].transform.DOScale(new Vector2(1f, 1f), 0.15f).SetEase(Ease.OutSine).SetDelay(0.45f);
        if (index == 5)
        {
            SaveData();
        }
    }

    public void RewardObj(GameObject obj, int index)
    {
        switch (index)
        {
            case 0:
                {
                    int index2 = Random.Range(0, 3);
                    obj.transform.Find("BoxImage").GetComponent<Image>().sprite = Util.GetResourcesSprite("ShopImage/ShopBG_8");
                    obj.transform.Find("BoxText").GetComponent<Text>().text = LocalizationManager.Instance.GetText("Coins");

                    obj.transform.Find("BoxNum").GetComponent<Text>().text = "x" + RewardNum[index2].ToString();
                    Singleton<UserData>.Instance.AddUserGold(RewardNum[index2]);
                    return;
                }
            case 1:
                obj.transform.Find("BoxImage").GetComponent<Image>().sprite = Util.GetResourcesSprite("Reward/Max_signin_icon_9");
                obj.transform.Find("BoxText").GetComponent<Text>().text = LocalizationManager.Instance.GetText("Lives");

                obj.transform.Find("BoxNum").GetComponent<Text>().text = "x1";
                obj.transform.Find("BoxImage").GetComponent<Image>().SetNativeSize();
                obj.transform.Find("BoxImage").gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                Singleton<UserData>.Instance.AddLoveInfinite(9, 1f);
                return;
        }
        int num = 0;
        switch (index)
        {
            case 2:
            case 3:
                num = Random.Range(3, 6);
                while (BeforeIndex == num)
                {
                    num = Random.Range(3, 6);
                }
                break;
            case 4:
            case 5:
                num = Random.Range(6, 9);
                while (BeforeIndex == num)
                {
                    num = Random.Range(6, 9);
                }
                break;
        }
        BeforeIndex = num;
        obj.transform.Find("BoxImage").GetComponent<Image>().sprite = Util.GetResourcesSprite("ShopImage/ShopIcon_" + RewardImg[num]);
        obj.transform.Find("BoxText").GetComponent<Text>().text = LocalizationManager.Instance.GetText("Props");

        obj.transform.Find("BoxNum").GetComponent<Text>().text = "x" + RewardNum[num].ToString();
        obj.transform.Find("BoxImage").GetComponent<Image>().SetNativeSize();
        obj.transform.Find("BoxImage").gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        Singleton<UserData>.Instance.AddProps(RewardImg[num] - 3, RewardNum[num]);
    }

    public void CloseButton(InputUIOnClickEvent e)
    {
        UIManager.CloseUIWindow<ChestUIWindow>(isPlayAnim: true, null, new object[0]);
        ADInterface.Instance.SendADEvent(ADType.NativeEndAD, ADSpot.BoxPanelNaticeAd);
    }

    public void AdBtnClick(InputUIOnClickEvent e)
    {
        ADInterface.Instance.SendADVideoEvent(ADType.RewardVideoAD, ADSpot.OpenBox, ADCallback);
    }

    private void ADCallback(bool result)
    {
        if (result)
        {
            CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.AD_OpenBox);
            ADInterface.Instance.SendADEvent(ADType.NativeEndAD, ADSpot.BoxPanelNaticeAd);
            canOpen = true;
            InitUI();
        }
    }

    public void ClaimButton(InputUIOnClickEvent e)
    {
        if (!isCanClick)
        {
            UIManager.CloseUIWindow<ChestUIWindow>(isPlayAnim: false, null, new object[0]);
            ADInterface.Instance.SendADEvent(ADType.CPAD, ADSpot.BoxPanel);
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
