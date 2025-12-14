
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FZDownBorder : MonoBehaviour
{
    public Image skillImg;

    public Image skillImg2;

    public GameObject FXParent;

    public GameObject jinengLight, jinengLight2;

    public GameObject parLizi;

    public GameObject skill;

    public bool isUse;

    private int index = 1;

    private float time;

    private float fzScore;

    private float fzMaxScore = 800f;

    private bool isMaxScore;

    private void Start()
    {
        skillImg.fillAmount = 0f;
        skillImg2.fillAmount = 1f;
    }

    public void InitFZ()
    {
        isMaxScore = false;
        if (base.transform.tag == "FZ1")
        {
            index = 1;
        }
        else if (base.transform.tag == "FZ2")
        {
            index = 2;
        }
        else if (base.transform.tag == "FZ3")
        {
            index = 3;
        }
        else if (base.transform.tag == "FZ4")
        {
            index = 4;
        }
        if (!Singleton<LevelManager>.Instance.GetGangSkill(index))
        {
            skill.SetActive(value: false);
            isUse = false;
        }
        else
        {
            skill.SetActive(value: true);
            isUse = true;
        }
    }

    public void UseSkill()
    {
        if (GameManager.Instance.iSkill4Count > 1)
        {
            return;
        }
        GameObject readyBubble = ReadyBubbleController.Instance.ReadyBubble1;
        if (readyBubble.GetComponent<BubbleObj>().GetUseProp())
        {
            return;
        }
        if (base.transform.tag == "FZ1")
        {
            if (GameGuid.Instance != null && !GameGuid.Instance.GetUseSkill1())
            {
                return;
            }
            if (fzScore < fzMaxScore)
            {
                Singleton<UserData>.Instance.BuyMagicType = 1;
                UIManager.OpenUIWindow<BuyGuangUIWindow>();
                return;
            }
            if (GameGuid.Instance != null)
            {
                GameGuid.Instance.UseSkill(1);
            }
            readyBubble.GetComponent<BubbleObj>().SetSkill(1, isSkill: true);
            readyBubble.GetComponent<BubbleObj>().ChangeToSkill(1);
            AudioPlayManager.PlaySFX2D("skill_use");
        }
        else if (base.transform.tag == "FZ2")
        {
            if (GameGuid.Instance != null && !GameGuid.Instance.GetUseSkill2())
            {
                return;
            }
            if (fzScore < fzMaxScore)
            {
                Singleton<UserData>.Instance.BuyMagicType = 2;
                UIManager.OpenUIWindow<BuyGuangUIWindow>();
                return;
            }
            if (GameGuid.Instance != null)
            {
                GameGuid.Instance.UseSkill(2);
            }
            readyBubble.GetComponent<BubbleObj>().SetSkill(2, isSkill: true);
            readyBubble.GetComponent<BubbleObj>().ChangeToSkill(2);
            AudioPlayManager.PlaySFX2D("skill_use");
        }
        else if (base.transform.tag == "FZ3")
        {
            if (GameGuid.Instance != null && !GameGuid.Instance.GetUseSkill3())
            {
                return;
            }
            if (fzScore < fzMaxScore)
            {
                Singleton<UserData>.Instance.BuyMagicType = 3;
                UIManager.OpenUIWindow<BuyGuangUIWindow>();
                return;
            }
            if (GameGuid.Instance != null)
            {
                GameGuid.Instance.UseSkill(3);
            }
            readyBubble.GetComponent<BubbleObj>().SetSkill(3, isSkill: true);
            readyBubble.GetComponent<BubbleObj>().ChangeToSkill(3);
            AudioPlayManager.PlaySFX2D("skill_use");
        }
        else if (base.transform.tag == "FZ4")
        {
            if (GameGuid.Instance != null && !GameGuid.Instance.GetUseSkill4())
            {
                return;
            }
            if (fzScore < fzMaxScore)
            {
                Singleton<UserData>.Instance.BuyMagicType = 4;
                UIManager.OpenUIWindow<BuyGuangUIWindow>();
                return;
            }
            if (GameGuid.Instance != null)
            {
                GameGuid.Instance.UseSkill(4);
            }
            readyBubble.GetComponent<BubbleObj>().SetSkill(4, isSkill: true);
            readyBubble.GetComponent<BubbleObj>().ChangeToSkill(4);
            AudioPlayManager.PlaySFX2D("skill_use");
            GameManager.Instance.iSkill4Count++;
        }
        fzScore = 0f;
        if (fzScore >= fzMaxScore)
        {
            jinengLight.SetActive(value: true);
            jinengLight2.SetActive(true);
            parLizi.SetActive(value: true);
        }
        else
        {
            jinengLight.SetActive(value: false);
            jinengLight2.SetActive(false);
            parLizi.SetActive(value: false);
        }
        StartCoroutine(UpFillAmount());
        isMaxScore = false;
    }

    private IEnumerator UpFillAmount()
    {
        int i = 100;
        while (i > 0)
        {
            i -= 2;
            skillImg.fillAmount = (float)i * 0.01f;
            // skillImg2.fillAmount = (float)i * 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("FallBubble"))
        {
            RemoveObj(coll.transform.parent.GetComponent<BubbleObj>());
        }
    }

    private void RemoveObj(BubbleObj bubble)
    {
        if (!(bubble == null) && bubble.GetBubbleState() == BubbleState.Fall)
        {
            bubble.FallDestroyObject();
            AddScore();
        }
    }

    public void AddScore()
    {
        if (Time.time - time > 0.2f)
        {
            time = Time.time;
            if (isUse && fzScore < fzMaxScore)
            {
                string name = "FX/FZ/fx_mofazheng_juneng/fx_mofazheng_light";
                if (index == 1)
                {
                    name = "FX/FZ/fx_mofazheng_juneng/fx_mofazheng_huang";
                }
                else if (index == 2)
                {
                    name = "FX/FZ/fx_mofazheng_juneng/fx_mofazheng_hong";
                }
                else if (index == 3)
                {
                    name = "FX/FZ/fx_mofazheng_juneng/fx_mofazheng_lv";
                }
                else if (index == 4)
                {
                    name = "FX/FZ/fx_mofazheng_juneng/fx_mofazheng_lan";
                }
                PoolObject poolObject = GameObjectManager.GetPoolObject(name, FXParent);
                poolObject.transform.localPosition = Vector3.zero;
                GameObjectManager.DestroyPoolObject(poolObject, 1f);
            }
            else
            {
                PoolObject poolObject2 = GameObjectManager.GetPoolObject("FX/FZ/rudai_fazhen", FXParent);
                poolObject2.transform.localPosition = Vector3.zero;
                GameObjectManager.DestroyPoolObject(poolObject2, 1f);
            }
        }
        int num = 1;
        if (GameManager.Instance.iCombo >= 5 && GameManager.Instance.iCombo < 10)
        {
            num = 2;
        }
        else if (GameManager.Instance.iCombo >= 10)
        {
            num = 3;
        }
        int num2 = 100;
        if (Singleton<LevelManager>.Instance.GetNowLevel() == 5 || Singleton<LevelManager>.Instance.GetNowLevel() == 8 || Singleton<LevelManager>.Instance.GetNowLevel() == 10 || Singleton<LevelManager>.Instance.GetNowLevel() == 15 || Singleton<LevelManager>.Instance.GetNowLevel() == 17 || Singleton<LevelManager>.Instance.GetNowLevel() == 24 || Singleton<LevelManager>.Instance.GetNowLevel() == 25 || Singleton<LevelManager>.Instance.GetNowLevel() == 7 || Singleton<LevelManager>.Instance.GetNowLevel() == 12 || Singleton<LevelManager>.Instance.GetNowLevel() == 13 || Singleton<LevelManager>.Instance.GetNowLevel() == 14 || Singleton<LevelManager>.Instance.GetNowLevel() == 18 || Singleton<LevelManager>.Instance.GetNowLevel() == 19 || Singleton<LevelManager>.Instance.GetNowLevel() == 20)
        {
            num2 = 800;
        }
        GameScene.Instance.AddScore(num2 * num, null, 11, base.transform);
        fzScore += num2 * num;
        if (fzScore > fzMaxScore)
        {
            fzScore = fzMaxScore;
        }
        if (isUse)
        {
            // skillImg2.fillAmount = fzScore / fzMaxScore == 1 ? 1 : 0;
            skillImg.fillAmount = fzScore / fzMaxScore == 1 ? 1 : 0;
        }
        if (fzScore >= fzMaxScore)
        {
            if (GameGuid.Instance != null)
            {
                GameGuid.Instance.SkillFull(index);
            }
            if (isUse)
            {
                AudioPlayManager.PlaySFX2D("b_enter_buchet_1");
            }
            else
            {
                AudioPlayManager.PlaySFX2D("b_enter_buchet");
            }
            if (isUse && !isMaxScore)
            {
                isMaxScore = true;
                string name2 = "FX/FZ/fx_mofazheng_juneng/fx_mofazheng_light";
                if (index == 1)
                {
                    name2 = "FX/FZ/fx_mofazheng_juneng/fx_mofazheng_light_huang";
                }
                else if (index == 2)
                {
                    name2 = "FX/FZ/fx_mofazheng_juneng/fx_mofazheng_light_hong";
                }
                else if (index == 3)
                {
                    name2 = "FX/FZ/fx_mofazheng_juneng/fx_mofazheng_light_lv";
                }
                else if (index == 4)
                {
                    name2 = "FX/FZ/fx_mofazheng_juneng/fx_mofazheng_light_lan";
                }
                PoolObject poolObject3 = GameObjectManager.GetPoolObject(name2, skill);
                poolObject3.transform.localPosition = Vector3.zero;
                GameObjectManager.DestroyPoolObject(poolObject3, 1f);
                jinengLight.SetActive(value: true);
                jinengLight2.SetActive(true);
                parLizi.SetActive(value: true);
            }
        }
        else
        {
            AudioPlayManager.PlaySFX2D("b_enter_buchet");
            jinengLight.SetActive(value: false);
            jinengLight2.SetActive(false);
            parLizi.SetActive(value: false);
        }
    }

    public void FullFZ()
    {
        if (isUse)
        {
            fzScore = fzMaxScore;
            jinengLight.SetActive(value: true);
            jinengLight2.SetActive(true);
            parLizi.SetActive(value: true);
            StartCoroutine(UpFillAmount2());
        }
    }

    public bool IsFull()
    {
        return fzScore >= fzMaxScore;
    }

    private IEnumerator UpFillAmount2()
    {
        int i = (int)(skillImg.fillAmount * 100f);
        while (i < 100)
        {
            i += 2;
            skillImg.fillAmount = (float)i * 0.01f;
            // skillImg2.fillAmount = (float)i * 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
