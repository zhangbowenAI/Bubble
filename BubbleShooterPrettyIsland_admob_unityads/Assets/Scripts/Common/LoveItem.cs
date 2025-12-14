
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoveItem : MonoBehaviour
{
    public GameObject IconObj;

    public Sprite[] LIconObj;

    [SerializeField]
    private Sprite FreeVideoIcon;

    [SerializeField]
    private Sprite FreeVideoBtnImg;

    [SerializeField]
    private Sprite FreeVideoBtnImgGrey;

    public GameObject BuyLoveBtn;

    private int _myindex;

    public GameObject PayNumText;

    public GameObject GoldNum;

    public Text Miaosu;

    private GameObject goFreeVideoIcon;

    private string iDays = string.Empty;

    private int HalfHourFreeLivesVideoCount;

    private int HalfHourFreeLivesVideoTick;

    private Text ButtonTimerText;

    private Text ButtonHintText;

    public void InitData(int index)
    {
        base.gameObject.SetActive(value: true);
        _myindex = index;
        IconObj.GetComponent<Image>().sprite = LIconObj[index];
        GlobalEvent.AddEvent(GameEventEnum.UIFlushChane, ReceviceLoveChange);
        Miaosu.text = Util.ReplaceText(GameEntry.Instance.GetString("BuyLives" + (index + 1).ToString()));
        switch (index)
        {
            case 0:
                BuyLoveBtn.transform.Find("Image").gameObject.SetActive(value: true);
                BuyLoveBtn.GetComponent<Button>().interactable = true;
                PayNumText.SetActive(value: false);
                GoldNum.GetComponent<Text>().text = "1000";
                if (Singleton<UserData>.Instance.getLoveInfinite() > 0 || Singleton<UserData>.Instance.GetUserLoveCount() >= Singleton<UserData>.Instance.iLoveMaxAll)
                {
                    BuyLoveBtn.GetComponent<Button>().interactable = false;
                }
                break;
            case 1:
                BuyLoveBtn.GetComponent<Image>().sprite = FreeVideoBtnImg;
                goFreeVideoIcon = BuyLoveBtn.transform.Find("Image").gameObject;
                goFreeVideoIcon.SetActive(value: true);
                goFreeVideoIcon.GetComponent<Image>().sprite = FreeVideoIcon;
                goFreeVideoIcon.GetComponent<RectTransform>().setSize(Vector2.one * 50);
                PayNumText.SetActive(value: false);
                ButtonTimerText = PayNumText.GetComponent<Text>();
                ButtonTimerText.fontSize = 27;
                ButtonHintText = GoldNum.GetComponent<Text>();
                if (iDays != Util.GetNowTime_Day())
                {
                    iDays = Util.GetNowTime_Day();
                    HalfHourFreeLivesVideoCount = RecordManager.GetIntRecord("UserData", "HalfHourFreeLivesVideoCount" + iDays, 0);
                    HalfHourFreeLivesVideoTick = RecordManager.GetIntRecord("UserData", "HalfHourFreeLivesVideoTick" + iDays, 0);
                }
                UpdateItem1();
                break;
            default:
                PayNumText.SetActive(value: true);
                BuyLoveBtn.transform.Find("Image").gameObject.SetActive(value: false);
                if (GameEntry.buildType == GameEntry.BuildType.GoogleEN)
                {
                    PayNumText.GetComponent<Text>().text = Singleton<DataPurchase>.Instance.GetContentByKeyAndType("Twohourslivs", PurchaseType.iMoney);
                }
                break;
        }
        if(_myindex == 2){
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_myindex != 1)
        {
            return;
        }
        if (iDays != Util.GetNowTime_Day())
        {
            iDays = Util.GetNowTime_Day();
            HalfHourFreeLivesVideoCount = RecordManager.GetIntRecord("UserData", "HalfHourFreeLivesVideoCount" + iDays, 0);
            HalfHourFreeLivesVideoTick = RecordManager.GetIntRecord("UserData", "HalfHourFreeLivesVideoTick" + iDays, 0);
        }
        if (HalfHourFreeLivesVideoCount == 1)
        {
            long num = 3600 - (Util.GetNowTime() - HalfHourFreeLivesVideoTick);
            if (num > 0)
            {
                goFreeVideoIcon.SetActive(value: false);
                ButtonTimerText.gameObject.SetActive(value: true);
                int num2 = (int)num / 60;
                string str = (num2 < 10) ? ("0" + num2.ToString()) : num2.ToString();
                int num3 = (int)num % 60;
                string str2 = (num3 < 10) ? ("0" + num3.ToString()) : num3.ToString();
                ButtonTimerText.text = str + ":" + str2;
                ButtonHintText.gameObject.SetActive(value: false);
                BuyLoveBtn.GetComponent<Button>().interactable = false;
                BuyLoveBtn.GetComponent<Image>().sprite = FreeVideoBtnImgGrey;
            }
            else
            {
                goFreeVideoIcon.SetActive(value: true);
                ButtonHintText.gameObject.SetActive(value: true);
                ButtonHintText.text = GameEntry.Instance.GetString("HalfHourUnlimitedLivesFree");
                ButtonTimerText.gameObject.SetActive(value: false);
                BuyLoveBtn.GetComponent<Button>().interactable = true;
                BuyLoveBtn.GetComponent<Image>().sprite = FreeVideoBtnImg;
            }
        }
    }

    private void UpdateItem1()
    {
        if (iDays != Util.GetNowTime_Day())
        {
            iDays = Util.GetNowTime_Day();
            HalfHourFreeLivesVideoCount = RecordManager.GetIntRecord("UserData", "HalfHourFreeLivesVideoCount" + iDays, 0);
            HalfHourFreeLivesVideoTick = RecordManager.GetIntRecord("UserData", "HalfHourFreeLivesVideoTick" + iDays, 0);
        }
        if (HalfHourFreeLivesVideoCount >= 2)
        {
            goFreeVideoIcon.SetActive(value: false);
            ButtonTimerText.gameObject.SetActive(value: true);
            ButtonTimerText.text = GameEntry.Instance.GetString("HalfHourUnlimitedLivesTmr");
            ButtonHintText.gameObject.SetActive(value: false);
            BuyLoveBtn.GetComponent<Button>().interactable = false;
            BuyLoveBtn.GetComponent<Image>().sprite = FreeVideoBtnImgGrey;
        }
        else if (HalfHourFreeLivesVideoCount == 1)
        {
            long num = 3600 - (Util.GetNowTime() - HalfHourFreeLivesVideoTick);
            if (num < 0)
            {
                goFreeVideoIcon.SetActive(value: true);
                ButtonHintText.gameObject.SetActive(value: true);
                ButtonHintText.text = GameEntry.Instance.GetString("HalfHourUnlimitedLivesFree");
                ButtonTimerText.gameObject.SetActive(value: false);
                BuyLoveBtn.GetComponent<Button>().interactable = true;
                BuyLoveBtn.GetComponent<Image>().sprite = FreeVideoBtnImg;
            }
            else
            {
                goFreeVideoIcon.SetActive(value: false);
                ButtonTimerText.gameObject.SetActive(value: true);
                int num2 = (int)num / 60;
                string str = (num2 < 10) ? ("0" + num2.ToString()) : num2.ToString();
                int num3 = (int)num % 60;
                string str2 = (num3 < 10) ? ("0" + num3.ToString()) : num3.ToString();
                ButtonTimerText.text = str + ":" + str2;
                ButtonHintText.gameObject.SetActive(value: false);
                BuyLoveBtn.GetComponent<Button>().interactable = false;
                BuyLoveBtn.GetComponent<Image>().sprite = FreeVideoBtnImgGrey;
            }
        }
        else
        {
            goFreeVideoIcon.SetActive(value: true);
            ButtonHintText.gameObject.SetActive(value: true);
            ButtonHintText.text = GameEntry.Instance.GetString("HalfHourUnlimitedLivesFree");
            BuyLoveBtn.GetComponent<Button>().interactable = true;
            BuyLoveBtn.GetComponent<Image>().sprite = FreeVideoBtnImg;
            ButtonTimerText.gameObject.SetActive(value: false);
        }
    }

    private void ReceviceLoveChange(params object[] objs)
    {
        switch (_myindex)
        {
            case 0:
                if (Singleton<UserData>.Instance.getLoveInfinite() <= 0)
                {
                    if (Singleton<UserData>.Instance.GetUserLoveCount() < Singleton<UserData>.Instance.iLoveMaxAll)
                    {
                        BuyLoveBtn.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        BuyLoveBtn.GetComponent<Button>().interactable = false;
                    }
                }
                else
                {
                    BuyLoveBtn.GetComponent<Button>().interactable = false;
                }
                break;
            case 1:
                HalfHourFreeLivesVideoCount = RecordManager.GetIntRecord("UserData", "HalfHourFreeLivesVideoCount" + iDays, 0);
                HalfHourFreeLivesVideoTick = RecordManager.GetIntRecord("UserData", "HalfHourFreeLivesVideoTick" + iDays, 0);
                UpdateItem1();
                break;
        }
    }

    public void PayLove()
    {
        Debug.Log("=====> PayLove:" + _myindex);
        AudioPlayManager.PlaySFX2D("button");
        if (_myindex == 0)
        {
            int num = int.Parse(BuyLoveBtn.transform.Find("Image/Text").GetComponent<Text>().text);
            if (Singleton<UserData>.Instance.GetUserGold() >= num)
            {
                Singleton<UserData>.Instance.DelUserGold(num);
                int num2 = Singleton<UserData>.Instance.iLoveMaxAll - Singleton<UserData>.Instance.GetUserLoveCount();
                List<RewardType> list = new List<RewardType>();
                list.Add(new RewardType(1, num2));
                UIManager.Reward(list);
            }
            else
            {
                UIManager.OpenUIWindow<BuyGoldUIWindow>();
            }
        }
        else if (_myindex == 1)
        {
            // AndroidManager.Instance.ShowVideoAd("HalfHourFreeLivesVideo");
			ADInterface.Instance.SendADVideoEvent(ADType.RewardVideoAD,ADSpot.HalfHourFreeLivesVideo,ADCallback);
        }
        else if (_myindex == 2)
        {
            string contentByKeyAndType = Singleton<DataPurchase>.Instance.GetContentByKeyAndType("Twohourslivs", PurchaseType.GooglePay);
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                contentByKeyAndType = Singleton<DataPurchase>.Instance.GetContentByKeyAndType("Twohourslivs", PurchaseType.IOS);
            }
            AndroidManager.Instance.Pay(contentByKeyAndType);
        }
    }

    private void ADCallback(bool result)
    {
        if (result)
        {
            string nowTime_Day = Util.GetNowTime_Day();
            int intRecord = RecordManager.GetIntRecord("UserData", "HalfHourFreeLivesVideoCount" + nowTime_Day, 0);
            intRecord++;
            RecordManager.SaveRecord("UserData", "HalfHourFreeLivesVideoCount" + nowTime_Day, intRecord);
            RecordManager.SaveRecord("UserData", "HalfHourFreeLivesVideoTick" + nowTime_Day, Util.GetNowTime());
            List<RewardType> list = new List<RewardType>();
            list.Add(new RewardType(9, 1f));
            UIManager.Reward(list);
        }
    }
}
