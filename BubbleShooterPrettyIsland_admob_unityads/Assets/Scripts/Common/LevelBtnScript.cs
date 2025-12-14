
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelBtnScript : MonoBehaviour
{
    public GameObject BtnObj;

    public Text LevelBtnNumber;

    public GameObject StarBg1;

    public GameObject StarBg2;

    public GameObject StarBg3;

    public Sprite StarSprite;

    public GameObject levle_starObj;

    public GameObject level_btnObj;

    public GameObject fx_ui_mapPinObj;

    public GameObject LevelRewardObj;

    public GameObject TipObj;

    public Text TipText;

    public Text MapUnBtnText;

    public Sprite PassLevelBtn;

    private int iNowBtnSelectLevelIndex = 1;

    private int iLevelIndexID;

    private int istarNb;

    public void ClearRemark()
    {
        TipObj.SetActive(value: false);
    }

    public void SetPassLevel()
    {
        BtnObj.GetComponent<Image>().sprite = PassLevelBtn;
        BtnObj.GetComponent<Button>().enabled = true;
    }

    public void Unlocked(bool b = true)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(level_btnObj, base.transform.position, base.transform.rotation);
        gameObject.transform.parent = base.transform.transform;
        if (b)
        {
            Createfx_ui_mapPin();
        }
        UnityEngine.Object.Destroy(gameObject, 3f);
    }

    public void Createfx_ui_mapPin()
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(fx_ui_mapPinObj);
        gameObject.transform.parent = base.transform.transform;
        gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        gameObject.transform.localScale = new Vector3(100f, 100f, 0f);
    }

    public void HideNumber()
    {
        LevelBtnNumber.gameObject.SetActive(value: false);
        BtnObj.GetComponent<Button>().enabled = false;
    }

    public void ShowNumber()
    {
        LevelBtnNumber.gameObject.SetActive(value: true);
        BtnObj.GetComponent<Button>().enabled = true;
    }

    public void SetLevelBtnNumber(int index)
    {
        if (index >= 100)
        {
            LevelBtnNumber.fontSize = 26;
        }
        iLevelIndexID = index;
        LevelBtnNumber.text = index.ToString();
        SetRewardState(index);
    }

    public void HideStar()
    {
        StarBg1.SetActive(value: false);
        StarBg2.SetActive(value: false);
        StarBg3.SetActive(value: false);
        StartCoroutine(IEStarAni());
    }

    private void OpenFx_StarAni(GameObject obj)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(levle_starObj, base.transform.position, base.transform.rotation);
        gameObject.transform.parent = obj.transform.transform;
        gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        UnityEngine.Object.Destroy(gameObject, 3f);
    }

    private IEnumerator IEStarAni()
    {
        yield return new WaitForSeconds(0.1f);
        if (istarNb >= 1)
        {
            StarBg1.GetComponent<Image>().sprite = StarSprite;
            StarBg1.transform.localPosition = new Vector3(0f, 0f, 0f);
            StarBg1.transform.localScale = new Vector2(0f, 0f);
            StarBg1.SetActive(value: true);
            Vector3 endValue = new Vector3(-50f, 66f, 0f);
            Sequence s = DOTween.Sequence();
            s.Append(StarBg1.transform.DOScale(new Vector2(1f, 1.2f), 0.35f)).Append(StarBg1.transform.DOScale(new Vector2(0.8f, 0.8f), 0.15f));
            StarBg1.transform.DOLocalMove(endValue, 0.35f).OnComplete(delegate
            {
                OpenFx_StarAni(StarBg1);
            });
        }
        yield return new WaitForSeconds(0.2f);
        if (istarNb >= 2)
        {
            StarBg2.GetComponent<Image>().sprite = StarSprite;
            StarBg2.transform.localPosition = new Vector3(0f, 0f, 0f);
            StarBg2.transform.localScale = new Vector2(0f, 0f);
            StarBg2.SetActive(value: true);
            Sequence s2 = DOTween.Sequence();
            s2.Append(StarBg2.transform.DOScale(new Vector2(1f, 1.2f), 0.35f)).Append(StarBg2.transform.DOScale(new Vector2(0.8f, 0.8f), 0.15f));
            ShortcutExtensions.DOLocalMove(endValue: new Vector3(0f, 79.3f, 0f), target: StarBg2.transform, duration: 0.35f).OnComplete(delegate
            {
                OpenFx_StarAni(StarBg2);
            });
        }
        yield return new WaitForSeconds(0.2f);
        if (istarNb >= 3)
        {
            StarBg3.GetComponent<Image>().sprite = StarSprite;
            StarBg3.transform.localPosition = new Vector3(0f, 0f, 0f);
            StarBg3.transform.localScale = new Vector2(0f, 0f);
            StarBg3.SetActive(value: true);
            Vector3 endValue3 = new Vector3(50f, 66f, 0f);
            Sequence s3 = DOTween.Sequence();
            s3.Append(StarBg3.transform.DOScale(new Vector2(1f, 1.2f), 0.35f)).Append(StarBg3.transform.DOScale(new Vector2(0.8f, 0.8f), 0.15f));
            StarBg3.transform.DOLocalMove(endValue3, 0.35f).OnComplete(delegate
            {
                OpenFx_StarAni(StarBg3);
            });
        }
    }

    public void SetBtnState(int iStarNumber)
    {
        istarNb = iStarNumber;
        if (iStarNumber == 0)
        {
            StarBg1.SetActive(value: false);
            StarBg2.SetActive(value: false);
            StarBg3.SetActive(value: false);
            return;
        }
        if (iStarNumber >= 1)
        {
            StarBg1.GetComponent<Image>().sprite = StarSprite;
            StarBg1.GetComponent<Canvas>().overrideSorting = true;
            StarBg1.GetComponent<Canvas>().sortingOrder = 10;
        }
        if (iStarNumber >= 2)
        {
            StarBg2.GetComponent<Image>().sprite = StarSprite;
            StarBg2.GetComponent<Canvas>().overrideSorting = true;
            StarBg2.GetComponent<Canvas>().sortingOrder = 10;
        }
        if (iStarNumber >= 3)
        {
            StarBg3.GetComponent<Image>().sprite = StarSprite;
            StarBg3.GetComponent<Canvas>().overrideSorting = true;
            StarBg3.GetComponent<Canvas>().sortingOrder = 10;
        }
    }

    public void SelectLevelIndex(int index)
    {
        iNowBtnSelectLevelIndex = index;
        SetLevelBtnNumber(index);
    }

    public GameObject TouchChecker(Vector3 mouseposition)
    {
        if ((bool)Physics2D.OverlapPoint(mouseposition))
        {
            return Physics2D.OverlapPoint(mouseposition).gameObject;
        }
        return null;
    }

    private IEnumerator IETipRemark()
    {
        yield return new WaitForSeconds(0.2f);
        TipObj.SetActive(value: true);
    }

    public void ClickLevelReward()
    {
    }

    public void Reslanguage()
    {
    }

    public void SetRewardState(int indexLevel)
    {
    }

    public void OpenPlayUI()
    {
        throw new System.Exception("xxxxxxxxxxxxxxxxx No use");

    }
}
