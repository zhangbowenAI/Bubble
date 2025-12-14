
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyGoldUIWindow : UIWindowBase
{
    public GameObject PackAgeItem;

    public GameObject GoldItem;

    public GameObject BuyGoldShow;

    private List<GameObject> GoldItemList = new List<GameObject>();

    public GameObject ContentH;

    public Sprite ShowLessSprite;

    public Sprite ShowMoreSprite;

    public bool IsShowAll;

    public Text ShowText;

    public override void OnOpen()
    {
        IsShowAll = false;
        // BuyGoldShow.transform.Find("ShowButton").GetComponent<Image>().sprite = ShowMoreSprite;
        // InitUI();
        // AddOnClickListener("CloseButton", CloseButton);
        // AddOnClickListener("ShowButton", ShowButton);
        // AddOnClickListener("BuyGoldShow", ShowButton);
        AddEventListener(GameEventEnum.UIFlushChane, ReceviceGoldChange);
        InitLanguage();
        BuyGoldShow.SetActive(false);
        ADInterface.Instance.SendADEvent(ADType.NativeStartAD, ADSpot.BuyGoldNaticeAd);
    }

    private void ReceviceGoldChange(params object[] objs)
    {
    }

    public void InitLanguage()
    {
        ShowText.text = Util.ReplaceText(GameEntry.Instance.GetString("ShopRemark1"));
    }

    public void ShowButton(InputUIOnClickEvent e)
    {
        if (IsShowAll)
        {
            // InitUI();
            IsShowAll = false;
            ShowText.text = Util.ReplaceText(GameEntry.Instance.GetString("ShopRemark1"));
            BuyGoldShow.transform.Find("ShowButton").GetComponent<Image>().sprite = ShowMoreSprite;
        }
        else
        {
            // InitAllUI();
            ShowText.text = Util.ReplaceText(GameEntry.Instance.GetString("ShopRemark2"));
            BuyGoldShow.transform.Find("ShowButton").GetComponent<Image>().sprite = ShowLessSprite;
            IsShowAll = true;
        }
    }

    public void CloseButton()
    {
        // CloseAllLizi();
        UIManager.CloseUIWindow<BuyGoldUIWindow>(isPlayAnim: true, null, new object[0]);
        ADInterface.Instance.SendADEvent(ADType.NativeEndAD, ADSpot.BuyGoldNaticeAd);
    }

    public void CloseAllLizi()
    {
        if (GoldItemList.Count > 4)
        {
            for (int i = 0; i < GoldItemList.Count; i++)
            {
                GoldItemList[i].transform.Find("CenterImage/fx_shangdian_star").gameObject.SetActive(value: false);
            }
        }
    }

    public void InitAllUI()
    {
        if (GoldItemList.Count > 0)
        {
            for (int i = 0; i < GoldItemList.Count; i++)
            {
                UnityEngine.Object.Destroy(GoldItemList[i]);
            }
        }
        GoldItemList.Clear();
        Transform transform = ContentH.transform;
        Vector3 localPosition = ContentH.transform.localPosition;
        transform.localPosition = new Vector3(localPosition.x, 0f);
        float num = 0f;
        for (int j = 0; j < 12; j++)
        {
            if (j > 5)
            {
                GameObject gameObject = Object.Instantiate(GoldItem);
                gameObject.transform.SetParent(GoldItem.gameObject.transform.parent, worldPositionStays: false);
                GoldsItem component = gameObject.GetComponent<GoldsItem>();
                component.InitData(j);
                GoldItemList.Add(gameObject);
                Vector2 sizeDelta = gameObject.transform.GetComponent<RectTransform>().sizeDelta;
                float num2 = sizeDelta.x / 2f;
                float num3 = num;
                Vector2 sizeDelta2 = gameObject.transform.GetComponent<RectTransform>().sizeDelta;
                float y = num3 - sizeDelta2.y / 2f;
                gameObject.transform.localPosition = new Vector3(0f, y);
                float num4 = num;
                Vector2 sizeDelta3 = gameObject.transform.GetComponent<RectTransform>().sizeDelta;
                num = num4 - sizeDelta3.y - 8f;
                gameObject.SetActive(value: true);
            }
            else
            {
                GameObject gameObject2 = Object.Instantiate(PackAgeItem);
                gameObject2.transform.SetParent(PackAgeItem.gameObject.transform.parent, worldPositionStays: false);
                GoldPackItem component2 = gameObject2.GetComponent<GoldPackItem>();
                component2.InitData(j);
                GoldItemList.Add(gameObject2);
                Vector2 sizeDelta4 = gameObject2.transform.GetComponent<RectTransform>().sizeDelta;
                float num5 = sizeDelta4.x / 2f;
                float num6 = num;
                Vector2 sizeDelta5 = gameObject2.transform.GetComponent<RectTransform>().sizeDelta;
                float y2 = num6 - sizeDelta5.y / 2f;
                gameObject2.transform.localPosition = new Vector3(0f, y2);
                float num7 = num;
                Vector2 sizeDelta6 = gameObject2.transform.GetComponent<RectTransform>().sizeDelta;
                num = num7 - sizeDelta6.y - 8f;
                gameObject2.SetActive(value: true);
            }
        }
        BuyGoldShow.SetActive(value: true);
        Vector2 sizeDelta7 = BuyGoldShow.transform.GetComponent<RectTransform>().sizeDelta;
        float num8 = sizeDelta7.x / 2f;
        float num9 = num;
        Vector2 sizeDelta8 = BuyGoldShow.transform.GetComponent<RectTransform>().sizeDelta;
        float y3 = num9 - sizeDelta8.y / 2f;
        BuyGoldShow.transform.localPosition = new Vector3(0f, y3);
        float num10 = num;
        Vector2 sizeDelta9 = BuyGoldShow.transform.GetComponent<RectTransform>().sizeDelta;
        num = num10 - sizeDelta9.y - 8f - 60f;
        ContentH.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f - num);
    }

    public void InitUI()
    {
        if (GoldItemList.Count > 0)
        {
            for (int i = 0; i < GoldItemList.Count; i++)
            {
                UnityEngine.Object.Destroy(GoldItemList[i]);
            }
        }
        GoldItemList.Clear();
        Transform transform = ContentH.transform;
        Vector3 localPosition = ContentH.transform.localPosition;
        transform.localPosition = new Vector3(localPosition.x, 0f);
        float num = 0f;
        // for (int j = 0; j < 4; j++)
        // {
        // 	if (j > 1)
        // 	{
        // 		GameObject gameObject = Object.Instantiate(GoldItem);
        // 		gameObject.transform.SetParent(GoldItem.gameObject.transform.parent, worldPositionStays: false);
        // 		GoldsItem component = gameObject.GetComponent<GoldsItem>();
        // 		component.InitData(j + 8);
        // 		GoldItemList.Add(gameObject);
        // 		Vector2 sizeDelta = gameObject.transform.GetComponent<RectTransform>().sizeDelta;
        // 		float num2 = sizeDelta.x / 2f;
        // 		float num3 = num;
        // 		Vector2 sizeDelta2 = gameObject.transform.GetComponent<RectTransform>().sizeDelta;
        // 		float y = num3 - sizeDelta2.y / 2f;
        // 		gameObject.transform.localPosition = new Vector3(0f, y);
        // 		float num4 = num;
        // 		Vector2 sizeDelta3 = gameObject.transform.GetComponent<RectTransform>().sizeDelta;
        // 		num = num4 - sizeDelta3.y - 8f;
        // 		gameObject.SetActive(value: true);
        // 	}
        // 	else
        // 	{
        // 		GameObject gameObject2 = Object.Instantiate(PackAgeItem);
        // 		gameObject2.transform.SetParent(PackAgeItem.gameObject.transform.parent, worldPositionStays: false);
        // 		GoldPackItem component2 = gameObject2.GetComponent<GoldPackItem>();
        // 		component2.InitData(j + 3);
        // 		GoldItemList.Add(gameObject2);
        // 		Vector2 sizeDelta4 = gameObject2.transform.GetComponent<RectTransform>().sizeDelta;
        // 		float num5 = sizeDelta4.x / 2f;
        // 		float num6 = num;
        // 		Vector2 sizeDelta5 = gameObject2.transform.GetComponent<RectTransform>().sizeDelta;
        // 		float y2 = num6 - sizeDelta5.y / 2f;
        // 		gameObject2.transform.localPosition = new Vector3(0f, y2);
        // 		float num7 = num;
        // 		Vector2 sizeDelta6 = gameObject2.transform.GetComponent<RectTransform>().sizeDelta;
        // 		num = num7 - sizeDelta6.y - 8f;
        // 		gameObject2.SetActive(value: true);
        // 	}
        // }
        BuyGoldShow.SetActive(value: true);
        Vector2 sizeDelta7 = BuyGoldShow.transform.GetComponent<RectTransform>().sizeDelta;
        float num8 = sizeDelta7.x / 2f;
        float num9 = num;
        Vector2 sizeDelta8 = BuyGoldShow.transform.GetComponent<RectTransform>().sizeDelta;
        float y3 = num9 - sizeDelta8.y / 2f;
        BuyGoldShow.transform.localPosition = new Vector3(0f, y3);
        float num10 = num;
        Vector2 sizeDelta9 = BuyGoldShow.transform.GetComponent<RectTransform>().sizeDelta;
        num = num10 - sizeDelta9.y - 8f;
        ContentH.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f - num);
    }

    public override void OnRefresh()
    {
    }

    public override IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        m_bgMask.SetActive(value: true);
        Vector3 temp = base.transform.localPosition;
        temp.y = Screen.height;
        m_uiRoot.transform.localPosition = temp;
        if (GameManager.GetFullScreen())
        {
            Tweener t = m_uiRoot.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, -60f), 0.25f);
            t.OnComplete(delegate
            {
                animComplete(this, callBack, objs);
            });
        }
        else
        {
            Tweener t2 = m_uiRoot.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 0f), 0.25f);
            t2.OnComplete(delegate
            {
                animComplete(this, callBack, objs);
            });
        }
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
