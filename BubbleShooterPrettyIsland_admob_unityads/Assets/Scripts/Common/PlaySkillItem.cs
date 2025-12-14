
using UnityEngine;
using UnityEngine.UI;

public class PlaySkillItem : MonoBehaviour
{
    public Sprite[] SkillImage;

    public Image SkillIcon;

    public GameObject SkillNumBG;

    public GameObject UnSkillRemark;

    public Text SkillNumText;

    public GameObject Select;

    public GameObject shouzhi;

    private int _index;

    public void InitSkillItem(int index)
    {
        _index = index;
        int nowSelectLevel = Singleton<LevelManager>.Instance.GetNowSelectLevel();
        if (nowSelectLevel == 12 || nowSelectLevel == 13 || nowSelectLevel == 14)
        {
            PlayWindow.Instance.IsCanClick = true;
        }
        int intRecord = RecordManager.GetIntRecord("LevelData", "SkillOpen_" + index, 0);
        int intRecord2 = RecordManager.GetIntRecord("LevelData", "SkillOpenNum_" + index, 0);
        SkillNumText.gameObject.SetActive(value: true);
        Select.gameObject.SetActive(value: false);
        shouzhi.SetActive(value: false);
        if (intRecord == 1)
        {
            SkillIcon.sprite = SkillImage[index];
            SkillNumText.text = intRecord2.ToString();
            if (intRecord2 <= 0)
            {
                SkillNumText.text = "+";
            }
            SkillNumBG.SetActive(value: true);
            UnSkillRemark.SetActive(value: false);
        }
        else
        {
            SkillIcon.sprite = SkillImage[3];
            SkillNumBG.SetActive(value: false);
            UnSkillRemark.SetActive(value: true);
            UnSkillRemark.GetComponent<Text>().text = string.Format("第{0}关解锁", index + 12);
        }
        SkillIcon.SetNativeSize();
        GuidUI();
    }

    public void GuidUI()
    {
        if (Singleton<LevelManager>.Instance.GetNowSelectLevel() == 12)
        {
            if (_index == 0)
            {
                shouzhi.SetActive(value: true);
            }
        }
        else if (Singleton<LevelManager>.Instance.GetNowSelectLevel() == 13)
        {
            if (_index == 1)
            {
                shouzhi.SetActive(value: true);
            }
        }
        else if (Singleton<LevelManager>.Instance.GetNowSelectLevel() == 14 && _index == 2)
        {
            shouzhi.SetActive(value: true);
        }
    }

    public void SelectSkill()
    {
        AudioPlayManager.PlaySFX2D("button");
        int nowSelectLevel = Singleton<LevelManager>.Instance.GetNowSelectLevel();
        if (nowSelectLevel == 12 && _index == 0)
        {
            if (PlayWindow.Instance.IsCanClick)
            {
                ShowSelect();
                GlobalEvent.DispatchEvent(GameEventEnum.GuidChange);
                shouzhi.SetActive(value: false);
            }
        }
        else if (nowSelectLevel == 13 && _index == 1)
        {
            if (PlayWindow.Instance.IsCanClick)
            {
                ShowSelect();
                GlobalEvent.DispatchEvent(GameEventEnum.GuidChange);
                shouzhi.SetActive(value: false);
            }
        }
        else if (nowSelectLevel == 14 && _index == 2)
        {
            if (PlayWindow.Instance.IsCanClick)
            {
                ShowSelect();
                GlobalEvent.DispatchEvent(GameEventEnum.GuidChange);
                shouzhi.SetActive(value: false);
            }
        }
        else
        {
            if (PlayWindow.Instance.IsCanClick)
            {
                return;
            }
            int intRecord = RecordManager.GetIntRecord("LevelData", "SkillOpen_" + _index, 0);
            int intRecord2 = RecordManager.GetIntRecord("LevelData", "SkillOpenNum_" + _index, 0);
            if (intRecord == 1)
            {
                if (intRecord2 > 0)
                {
                    ShowSelect();
                    return;
                }
                Singleton<UserData>.Instance.buySkillType = _index;
                UIManager.OpenUIWindow<BuySkillWindow>();
            }
        }
    }

    public void ShowSelect()
    {
        if (_index == 0)
        {
            if (Singleton<UserData>.Instance.PlaySkillUse1)
            {
                Singleton<UserData>.Instance.PlaySkillUse1 = false;
                SkillNumText.gameObject.SetActive(value: true);
                Select.gameObject.SetActive(value: false);
            }
            else
            {
                Singleton<UserData>.Instance.PlaySkillUse1 = true;
                SkillNumText.gameObject.SetActive(value: false);
                Select.gameObject.SetActive(value: true);
            }
        }
        else if (_index == 1)
        {
            if (Singleton<UserData>.Instance.PlaySkillUse2)
            {
                Singleton<UserData>.Instance.PlaySkillUse2 = false;
                SkillNumText.gameObject.SetActive(value: true);
                Select.gameObject.SetActive(value: false);
            }
            else
            {
                Singleton<UserData>.Instance.PlaySkillUse2 = true;
                SkillNumText.gameObject.SetActive(value: false);
                Select.gameObject.SetActive(value: true);
            }
        }
        else if (_index == 2)
        {
            if (Singleton<UserData>.Instance.PlaySkillUse3)
            {
                Singleton<UserData>.Instance.PlaySkillUse3 = false;
                SkillNumText.gameObject.SetActive(value: true);
                Select.gameObject.SetActive(value: false);
            }
            else
            {
                Singleton<UserData>.Instance.PlaySkillUse3 = true;
                SkillNumText.gameObject.SetActive(value: false);
                Select.gameObject.SetActive(value: true);
            }
        }
    }
}
