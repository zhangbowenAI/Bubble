
using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BubbleObj : MonoBehaviour
{
    public BubbleKey objKey;

    public BubbleState state;

    public string key;

    public Point point;

    public bool isFall;

    public bool isFind;

    public bool isRemove;

    private int bubbleType = 1;

    private int bubbleImg = 1;

    private int iRemoveIndex;

    private int iAttributes;

    private bool isReadyBubble;

    private bool isSkill1;

    private bool isSkill2;

    private bool isSkill3;

    private bool isSkill4;

    public bool isSkillCount3;

    public bool isSkillCount4;

    private bool removeBySkillCount4;

    private bool isProp1;

    private bool isProp2;

    private bool isProp3;

    private bool bMenOpen = true;

    private int zhizhuIndex;

    public int overIndex;

    public BubbleObj hitBubble;

    private BubbleData mBubbleData;

    private BubbleData mBubbleTopData;

    private bool isShootRemove;

    private GameObject ObjFX;

    private GameObject ObjFX2;

    private GameObject ObjFX3;

    private GameObject ObjFX4;

    private PoolObject ObjPoolFX;

    public SpriteRenderer render;

    public SpriteRenderer topRender;

    public GameObject FXParent;

    private GameObject zhizhuObj;

    private PoolObject elfObj;

    public string TopKey;

    public CircleCollider2D mCollider;

    private void InitData(BubbleData data)
    {
        mBubbleTopData = default(BubbleData);
        mBubbleTopData.key = string.Empty;
        mBubbleTopData.row = -1;
        mBubbleTopData.col = -1;
        mBubbleTopData.s = -1;
        mBubbleTopData.i = -1;
        mBubbleTopData.isReadyBubble = false;
        mBubbleTopData.isTopData = false;
        bMenOpen = true;
        iRemoveIndex = 0;
        overIndex = 0;
        isShootRemove = false;
        isSkill1 = false;
        isSkill2 = false;
        isSkill3 = false;
        isSkill4 = false;
        isSkillCount3 = false;
        isSkillCount4 = false;
        removeBySkillCount4 = false;
        isProp1 = false;
        isProp2 = false;
        isProp3 = false;
        hitBubble = null;
        zhizhuIndex = 0;
        mBubbleData = data;
        isFall = false;
        isFind = false;
        key = data.key;
        state = BubbleState.None;
        point.X = data.row;
        point.Y = data.col;
        objKey = (BubbleKey)Enum.Parse(typeof(BubbleKey), data.key);
        bubbleType = int.Parse(Singleton<DataBubble>.Instance.GetContentByKeyAndType(objKey, BubbleType.type));
        bubbleImg = int.Parse(Singleton<DataBubble>.Instance.GetContentByKeyAndType(objKey, BubbleType.img)) - 1;
        iAttributes = int.Parse(Singleton<DataBubble>.Instance.GetContentByKeyAndType(objKey, BubbleType.attributes));
        if (!data.isReadyBubble)
        {
            isReadyBubble = false;
            if (GameManager.Instance.GetBox(point) != null)
            {
                GameManager.Instance.GetBox(point).SetBubble(this);
            }
        }
        else
        {
            isReadyBubble = true;
        }
        if (isReadyBubble)
        {
            mCollider.radius = 0.18f;
        }
        else
        {
            mCollider.radius = 0.4f;
        }
        if (isReadyBubble)
        {
            mCollider.gameObject.layer = LayerMask.NameToLayer("ReadyBubble");
            mCollider.isTrigger = true;
            if (base.gameObject.GetComponent<Rigidbody2D>() == null)
            {
                Rigidbody2D rigidbody2D = base.gameObject.AddComponent<Rigidbody2D>();
                rigidbody2D.isKinematic = true;
            }
        }
        else if (GetAttributes() == 3)
        {
            mCollider.gameObject.layer = LayerMask.NameToLayer("AirBubble");
            mCollider.isTrigger = true;
        }
        else
        {
            mCollider.gameObject.layer = LayerMask.NameToLayer("Bubble");
            mCollider.isTrigger = false;
        }
        mCollider.gameObject.SetActive(value: true);
    }

    private void InitTopData(BubbleData data)
    {
        mBubbleTopData = data;
    }

    public bool GetSkill(int skillId)
    {
        switch (skillId)
        {
            case 1:
                return isSkill1;
            case 2:
                return isSkill2;
            case 3:
                return isSkill3;
            case 4:
                return isSkill4;
            default:
                return false;
        }
    }

    public bool GetUseProp()
    {
        if (isProp1 || isProp2 || isProp3)
        {
            return true;
        }
        return false;
    }

    public bool GetUseSkill()
    {
        if (isSkill1 || isSkill2 || isSkill3 || isSkill4)
        {
            return true;
        }
        return false;
    }

    public bool GetProp(int propId)
    {
        switch (propId)
        {
            case 1:
                return isProp1;
            case 2:
                return isProp2;
            case 3:
                return isProp3;
            default:
                return false;
        }
    }

    public void SetProp(int propId, bool isProp)
    {
        switch (propId)
        {
            case 1:
                isProp1 = isProp;
                break;
            case 2:
                if (isProp)
                {
                    mCollider.radius = 0.4f;
                }
                isProp2 = isProp;
                break;
            case 3:
                isProp3 = isProp;
                break;
        }
    }

    public void SetSkill(int skillId, bool isSkill)
    {
        switch (skillId)
        {
            case 1:
                isSkill1 = isSkill;
                break;
            case 2:
                isSkill2 = isSkill;
                break;
            case 3:
                isSkill3 = isSkill;
                break;
            case 4:
                isSkill4 = isSkill;
                break;
        }
    }

    public void RemoveBubbleData()
    {
        if (GameManager.Instance.GetBox(point) != null)
        {
            GameManager.Instance.GetBox(point).RemoveBubble(this);
        }
    }

    public bool GetCanCheckMatch3Remove()
    {
        if (mBubbleTopData.key == "L")
        {
            return false;
        }
        if (mBubbleTopData.key == "Wang")
        {
            return false;
        }
        if (!bMenOpen)
        {
            return false;
        }
        return true;
    }

    public bool GetIsSkill()
    {
        if (isSkill1 || isSkill2 || isSkill3 || isSkill4)
        {
            return true;
        }
        return false;
    }

    public bool GetCanChecRemove()
    {
        if (mBubbleData.key == "II")
        {
            return false;
        }
        return true;
    }

    public BubbleState GetBubbleState()
    {
        return state;
    }

    public int GetAttributes()
    {
        return iAttributes;
    }

    public BubbleData GetBubbleData()
    {
        return mBubbleData;
    }

    public BubbleData GetTopBubbleData()
    {
        return mBubbleTopData;
    }

    public int GetBubbleType()
    {
        return bubbleType;
    }

    public void SetRemoveIndex(int index)
    {
        iRemoveIndex = index;
    }

    public void PlayRemoveEff()
    {
        if (removeBySkillCount4)
        {
            Singleton<EffManager>.Instance.PlayEff4(this, "FX/Skill/fx_4skill_ball");
        }
        else if (bubbleType <= 5)
        {
            if (isShootRemove)
            {
                Singleton<EffManager>.Instance.PlayEff4(this, "FX/Remove/fx_end_pop_boom");
            }
            else if (mBubbleData.row == -1)
            {
                Singleton<EffManager>.Instance.PlayRemoveEff(this, "FX/Remove/fx_pop_remove");
            }
            else
            {
                Singleton<EffManager>.Instance.PlayRemoveEff(point, "FX/Remove/fx_pop_remove");
            }
            AudioPlayManager.PlaySFX2D("b_boom");
        }
        else if (bubbleType == 16)
        {
            Singleton<EffManager>.Instance.PlayRemoveEffBySpine(point, "FX/Remove/bigElf_remove", string.Empty);
        }
        else if (bubbleType == 11)
        {
            AudioPlayManager.PlaySFX2D("b_ice");
            PlayRemoveEff("fx_bing_remove");
        }
        else if (bubbleType == 13)
        {
            AudioPlayManager.PlaySFX2D("b_stone");
            PlayRemoveEff("fx_shitou_remove");
        }
        else if (bubbleType == 9)
        {
            AudioPlayManager.PlaySFX2D("b_spikey");
            PlayRemoveEff("fx_tieci_remove");
        }
        else if (bubbleType == 10)
        {
            AudioPlayManager.PlaySFX2D("b_guadian");
            PlayRemoveEff("fx_guadian_remove");
        }
        else if (bubbleType == 7)
        {
            AudioPlayManager.PlaySFX2D("b_air");
            PlayRemoveEff("fx_kongqi_remove");
        }
        else if (bubbleType == 17)
        {
            PlayRemoveEff("FX/Prop/fx_boom", isAutoPath: false);
        }
        else if (bubbleType == 19)
        {
            PlayRemoveEff("FX/Prop/fx_mofa_elf_remove", isAutoPath: false, 0.8f);
        }
        else if (bubbleType == 21)
        {
            Singleton<EffManager>.Instance.PlayRemoveEffBySpine(point, "FX/Skill/fx_skill_zhizhu", "remove");
        }
        else
        {
            Singleton<EffManager>.Instance.PlayRemoveEff(point, "FX/Remove/fx_pop_remove");
        }
    }

    private void PlayRemoveEff(string path, bool isAutoPath = true, float removetime = 2f)
    {
        if (isAutoPath)
        {
            path = "FX/Remove/" + path;
        }
        Singleton<EffManager>.Instance.PlayEff2(point, path, 1, removetime);
    }

    private void PlayEff(string path, bool isAutoPath = true)
    {
        if (isAutoPath)
        {
            path = "FX/Function/" + path;
        }
        Singleton<EffManager>.Instance.PlayEff(this, path);
    }

    private void PlaySkillEff(string path, int type = 1, bool isCenter = false)
    {
        path = "FX/Skill/" + path;
        Singleton<EffManager>.Instance.PlayEff2(point, path, type, 2f, isCenter);
    }

    public void AddMofajian(Vector2 normalized)
    {
        GameObject gameObject = AddFx("FX/Prop/fx_mofajian");
        float angle = Mathf.Atan2(0f - normalized.x, normalized.y) * 57.29578f;
        base.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private GameObject AddFx(string path, bool isScale = false)
    {
        if (ObjFX == null)
        {
            ObjFX = GameObjectManager.CreateGameObject(path, FXParent);
            ObjFX.transform.localPosition = Vector3.zero;
            ObjFX.transform.localScale = Vector3.one;
            if (isScale)
            {
                ObjFX.transform.localScale = new Vector3(0.64f, 0.64f, 0.64f);
            }
            return ObjFX;
        }
        if (ObjFX2 == null)
        {
            ObjFX2 = GameObjectManager.CreateGameObject(path, FXParent);
            ObjFX2.transform.localPosition = Vector3.zero;
            ObjFX2.transform.localScale = Vector3.one;
            if (isScale)
            {
                ObjFX2.transform.localScale = new Vector3(0.64f, 0.64f, 0.64f);
            }
            return ObjFX2;
        }
        if (ObjFX3 == null)
        {
            ObjFX3 = GameObjectManager.CreateGameObject(path, FXParent);
            ObjFX3.transform.localPosition = Vector3.zero;
            ObjFX3.transform.localScale = Vector3.one;
            if (isScale)
            {
                ObjFX3.transform.localScale = new Vector3(0.64f, 0.64f, 0.64f);
            }
            return ObjFX3;
        }
        if (ObjFX4 == null)
        {
            ObjFX4 = GameObjectManager.CreateGameObject(path, FXParent);
            ObjFX4.transform.localPosition = Vector3.zero;
            ObjFX4.transform.localScale = Vector3.one;
            if (isScale)
            {
                ObjFX4.transform.localScale = new Vector3(0.64f, 0.64f, 0.64f);
            }
            return ObjFX4;
        }
        return ObjFX;
    }

    private PoolObject AddPoolFx(string path)
    {
        ObjPoolFX = GameObjectManager.GetPoolObject(path, FXParent);
        ObjPoolFX.transform.localPosition = Vector3.zero;
        return ObjPoolFX;
    }

    public void RemoveFx()
    {
        if (ObjFX != null)
        {
            UnityEngine.Object.Destroy(ObjFX);
            ObjFX = null;
        }
        if (ObjFX2 != null)
        {
            UnityEngine.Object.Destroy(ObjFX2);
            ObjFX2 = null;
        }
        if (ObjFX3 != null)
        {
            UnityEngine.Object.Destroy(ObjFX3);
            ObjFX3 = null;
        }
        if (ObjFX4 != null)
        {
            UnityEngine.Object.Destroy(ObjFX4);
            ObjFX4 = null;
        }
        if (ObjPoolFX != null)
        {
            GameObjectManager.DestroyPoolObject(ObjPoolFX);
            ObjPoolFX = null;
        }
    }

    public void SwitchZhiZhu(GameObject obj, string AniName)
    {
        if (obj.GetComponent<SkeletonAnimation>() == null)
            return;
        SkeletonAnimation component = obj.GetComponent<SkeletonAnimation>();
        component.Initialize(overwrite: true);
        component.loop = false;
        component.state.SetAnimation(0, AniName, loop: false);
        component.state.End += delegate
        {
            int num = UnityEngine.Random.Range(0, 100);
            if (num < 30)
            {
                SwitchZhiZhu(obj, "static");
            }
            else if (num < 60)
            {
                SwitchZhiZhu(obj, "pose1");
            }
            else
            {
                SwitchZhiZhu(obj, "pose2");
            }
        };
    }

    public void SwitchAniMax(GameObject obj, string AniName)
    {
        if (!(obj == null))
        {
            if (obj.GetComponent<SkeletonAnimation>() == null)
                return;
            SkeletonAnimation component = obj.GetComponent<SkeletonAnimation>();
            component.Initialize(overwrite: true);
            component.loop = false;
            component.state.SetAnimation(0, AniName, loop: false);
            component.state.End += delegate
            {
                int num = UnityEngine.Random.Range(0, 100);
                if (num < 50)
                {
                    SwitchAniMax(obj, "elf_static1");
                }
                else if (num < 90)
                {
                    SwitchAniMax(obj, "elf_static2");
                }
                else
                {
                    SwitchAniMax(obj, "elf_static3");
                }
            };
        }
    }

    public void SwitchAni(GameObject obj, string AniName)
    {
        if (obj.GetComponent<SkeletonAnimation>() == null)
            return;
        SkeletonAnimation skelet = obj.GetComponent<SkeletonAnimation>();
        skelet.Initialize(overwrite: true);
        skelet.loop = false;
        skelet.state.SetAnimation(0, AniName, loop: false);
        skelet.state.End += delegate
        {
            if (skelet.AnimationName == "elf_S1toS2")
            {
                SwitchAni(obj, "elf_static2");
            }
            else if (skelet.AnimationName == "elf_S2toS1")
            {
                SwitchAni(obj, "elf_static1");
            }
            else if (skelet.AnimationName == "elf_S2toS3")
            {
                SwitchAni(obj, "elf_static3");
            }
            else if (skelet.AnimationName == "elf_S3toS2")
            {
                SwitchAni(obj, "elf_static2");
            }
            else
            {
                int num = UnityEngine.Random.Range(0, 100);
                if (num < 50)
                {
                    if (skelet.AnimationName == "elf_static2")
                    {
                        SwitchAni(obj, "elf_S2toS1");
                    }
                    else
                    {
                        SwitchAni(obj, "elf_static1");
                    }
                }
                else if (num < 90)
                {
                    if (skelet.AnimationName == "elf_static1")
                    {
                        SwitchAni(obj, "elf_S1toS2");
                    }
                    else if (skelet.AnimationName == "elf_static3")
                    {
                        SwitchAni(obj, "elf_S3toS2");
                    }
                    else
                    {
                        SwitchAni(obj, "elf_static2");
                    }
                }
                else if (skelet.AnimationName == "elf_static2")
                {
                    SwitchAni(obj, "elf_S2toS3");
                }
                else
                {
                    SwitchAni(obj, "elf_static3");
                }
            }
        };
    }

    public void FallBubble()
    {
        if (GetBubbleState() == BubbleState.Remove)
        {
            return;
        }
        if (bubbleType == 16)
        {
            if (mBubbleData.i != 0)
            {
                RemoveBubbleData();
                FallDestroyObject();
            }
            else
            {
                RemoveBubble(RemoveType.BeRmove);
            }
            return;
        }
        if (GetAttributes() == 100)
        {
            RemoveBubble(RemoveType.BeRmove);
            return;
        }
        if (mBubbleTopData.key == "YunDian")
        {
            RemoveBubble(RemoveType.BeRmove);
            return;
        }
        TopRemoveAndFall();
        SetState(BubbleState.Fall);
        mCollider.gameObject.layer = LayerMask.NameToLayer("FallBubble");
        mCollider.isTrigger = false;
        Rigidbody2D rigidbody2D = null;
        rigidbody2D = ((!(base.gameObject.GetComponent<Rigidbody2D>() == null)) ? base.gameObject.GetComponent<Rigidbody2D>() : base.gameObject.AddComponent<Rigidbody2D>());
        rigidbody2D.isKinematic = false;
        rigidbody2D.gravityScale = 1f;
        rigidbody2D.mass = 0.9f;
        rigidbody2D.drag = 0.3f;
        rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rigidbody2D.velocity = base.gameObject.GetComponent<Rigidbody2D>().velocity + new Vector2((float)UnityEngine.Random.Range(0, 200) * 0.01f, (float)(-UnityEngine.Random.Range(0, 100)) * 0.01f) - new Vector2((float)UnityEngine.Random.Range(0, 200) * 0.01f, 0f);
        RemoveBubbleData();
    }

    public void GameOverShootBubble(int ivelocity, int _index)
    {
        isShootRemove = true;
        overIndex = _index;
        while (ivelocity > 5)
        {
            ivelocity -= 5;
        }
        isFall = true;
        SetState(BubbleState.Fall);
        mCollider.gameObject.layer = LayerMask.NameToLayer("FallBubble");
        mCollider.isTrigger = false;
        Rigidbody2D rigidbody2D = null;
        if (base.gameObject.GetComponent<Rigidbody2D>() == null)
        {
            rigidbody2D = base.gameObject.AddComponent<Rigidbody2D>();
        }
        rigidbody2D = base.gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.isKinematic = false;
        rigidbody2D.gravityScale = 1f;
        rigidbody2D.mass = 0.5f;
        rigidbody2D.drag = 0.3f;
        rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rigidbody2D.velocity = base.gameObject.GetComponent<Rigidbody2D>().velocity + new Vector2(ivelocity, 14f);
        SetRemoveIndex(UnityEngine.Random.Range(8, 18));
        RemoveBubble(RemoveType.BeRmove);
    }

    public void FallDestroyObject()
    {
        if (GetBubbleState() != BubbleState.Destroy && GetBubbleState() != BubbleState.Remove)
        {
            SetState(BubbleState.Destroy);
            base.gameObject.name = "Bubble";
            GameObjectManager.DestroyPoolObject(GetComponent<BubblePool>());
        }
    }

    public void UpFunction()
    {
        if (GameManager.Instance.iSkill4Count == 0)
        {
            if (GetAttributes() == 1)
            {
                RandomBubble();
            }
            ChangeMen();
            CheckL();
            Checkzhizhu();
        }
    }

    public void CheckRemove()
    {
        if (GetAttributes() == 11)
        {
            RemoveShanDian();
        }
        else if (GetAttributes() == 10)
        {
            RemoveHuoQiu();
        }
        else if (GetAttributes() == 12)
        {
            RemoveBing();
        }
        else if (GetAttributes() == 13)
        {
            RemoveMu();
        }
        else if (GetAttributes() == 7)
        {
            RemoveAddMove();
        }
        else if (GetAttributes() == 8)
        {
            RemoveCutMove();
        }
        else if (GetAttributes() == 32)
        {
            RemoveBBB();
        }
        if (isSkillCount4)
        {
            RemoveSkill4Count();
        }
        else if (isSkillCount3)
        {
            RemoveSkill3Count();
        }
        else
        {
            if (isSkill1)
            {
                RemoveShanDian();
            }
            if (isSkill2)
            {
                RemoveHuoQiu();
            }
            if (isSkill3)
            {
                RemoveMu();
            }
            if (isSkill4)
            {
                RemoveBing();
            }
        }
        if (isProp1)
        {
            RemoveProp1();
        }
        if (isProp2)
        {
            RemoveProp2();
        }
        if (isProp3)
        {
            RemoveProp3();
        }
        if (mBubbleTopData.key == "YunDian")
        {
            GameScene.Instance.RemoveGuandian(this);
        }
    }

    public void CheckGorundByRemove()
    {
        CheckTopGorundByRemove();
        if (GetBubbleType() <= 5)
        {
            CheckJingXiang();
        }
    }

    public void CheckGorundByHit(BubbleData _data, bool isRemoveSlef = false)
    {
        if (GameManager.Instance.iSkill4Count == 0)
        {
            CheckTopGorundByHit();
            bool flag = false;
            if (CheckBoli(isRemoveSlef))
            {
                flag = true;
            }
            if (CheckTieCi(isRemoveSlef))
            {
                AudioPlayManager.PlaySFX2D("b_spikey");
                flag = true;
            }
            CheckGuaDian();
            CheckShanDian();
            CheckHuoQiu();
            CheckBing();
            if (CheckMu(isRemoveSlef))
            {
                flag = true;
            }
            if (CheckRanSe(_data))
            {
            }
            CheckBBB();
            if (flag && GameManager.Instance.iSkill4Count == 0)
            {
                SetRemoveIndex(0);
                RemoveBubble(RemoveType.BeRmove);
            }
        }
    }

    private void Checkzhizhu()
    {
        int maxRow = Singleton<CameraMove>.Instance.GetMaxRow();
        if (point.X < maxRow - 11 || bubbleType != 21)
        {
            return;
        }
        if (GameManager.Instance.iSkill4Count != 0)
        {
            if (GameManager.Instance.iSkill4Count == 3)
            {
                zhizhuIndex++;
            }
        }
        else
        {
            zhizhuIndex++;
        }
        if (zhizhuIndex < 3)
        {
            return;
        }
        zhizhuIndex = 0;
        SwitchZhiZhu(zhizhuObj, "attack");
        List<BubbleObj> RemoveBubbleArray = new List<BubbleObj>();
        List<BubbleObj> bubbleArray = GameManager.Instance.GetBubbleArray();
        for (int i = 0; i < bubbleArray.Count; i++)
        {
            if (bubbleArray[i].point.X > maxRow - 11 && bubbleArray[i].GetBubbleType() <= 5 && bubbleArray[i].GetBubbleState() == BubbleState.None)
            {
                BubbleData topBubbleData = bubbleArray[i].GetTopBubbleData();
                if (topBubbleData.key == string.Empty && bubbleArray[i].GetAttributes() != 100 && bubbleArray[i].GetAttributes() != 101)
                {
                    RemoveBubbleArray.Add(bubbleArray[i]);
                }
            }
        }
        int num = 3;
        if (RemoveBubbleArray.Count < 3)
        {
            num = RemoveBubbleArray.Count;
        }
        for (int num2 = num; num2 > 0; num2--)
        {
            int random = UnityEngine.Random.Range(0, RemoveBubbleArray.Count - 1);
            if (random > RemoveBubbleArray.Count - 1)
            {
                random = 0;
            }
            GameObject obj2 = Singleton<EffManager>.Instance.PlayEff3(point, "FX/Function/bubble_infect");
            obj2.transform.localScale = Vector3.zero;
            obj2.transform.DOScale(1f, 15f * GameManager.removeTime);
            obj2.transform.DOLocalMove(RemoveBubbleArray[random].transform.localPosition, 15f * GameManager.removeTime);
            Timer.DelayCallBack(15f * GameManager.removeTime + 0.1f, delegate
            {
                BubbleData topBubbleData2 = RemoveBubbleArray[random].GetTopBubbleData();
                topBubbleData2.key = "Wang";
                RemoveBubbleArray[random].InitTop(topBubbleData2);
                UnityEngine.Object.Destroy(obj2);
            });
        }
    }

    private void CheckTopGorundByRemove()
    {
        List<Point> around = GameManager.Instance.GetAround(point);
        for (int i = 0; i < around.Count; i++)
        {
            if (CheckBox(around[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(around[i]);
                box.GetBubble().TopGorundByRemove();
            }
        }
    }

    public void CheckTopGorundByHit()
    {
        List<Point> around = GameManager.Instance.GetAround(point);
        for (int i = 0; i < around.Count; i++)
        {
            if (CheckBox(around[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(around[i]);
                box.GetBubble().TopGorundByHit();
            }
        }
    }

    private bool CheckBoli(bool isRemoveSlef)
    {
        int num = 0;
        List<Point> around = GameManager.Instance.GetAround(point);
        for (int i = 0; i < around.Count; i++)
        {
            if (CheckBox(around[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(around[i]);
                BubbleObj bubble = box.GetBubble();
                if (bubble.GetAttributes() == 6)
                {
                    num++;
                    bubble.RemoveBubble(RemoveType.BeRmove);
                }
            }
        }
        if (isRemoveSlef && num > 0)
        {
            return true;
        }
        return false;
    }

    private bool CheckTieCi(bool isRemoveSlef)
    {
        if (isRemoveSlef)
        {
            List<Point> around = GameManager.Instance.GetAround(point);
            for (int i = 0; i < around.Count; i++)
            {
                if (CheckBox(around[i]))
                {
                    BoxObj box = GameManager.Instance.GetBox(around[i]);
                    BubbleObj bubble = box.GetBubble();
                    if (bubble.GetAttributes() == 4)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void CheckGuaDian()
    {
        List<Point> around = GameManager.Instance.GetAround(point);
        for (int i = 0; i < around.Count; i++)
        {
            if (CheckBox(around[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(around[i]);
                BubbleObj bubble = box.GetBubble();
                if (bubble.GetAttributes() == 5)
                {
                    bubble.RemoveBubble(RemoveType.BeRmove);
                    Singleton<RemoveControl>.Instance.isCombo = true;
                }
            }
        }
    }

    private void CheckShanDian()
    {
        List<Point> around = GameManager.Instance.GetAround(point);
        for (int i = 0; i < around.Count; i++)
        {
            if (CheckBox(around[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(around[i]);
                BubbleObj bubble = box.GetBubble();
                if (bubble.GetAttributes() == 11)
                {
                    bubble.RemoveBubble(RemoveType.None);
                    Singleton<RemoveControl>.Instance.isCombo = true;
                }
            }
        }
    }

    private void CheckHuoQiu()
    {
        List<Point> around = GameManager.Instance.GetAround(point);
        for (int i = 0; i < around.Count; i++)
        {
            if (CheckBox(around[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(around[i]);
                BubbleObj bubble = box.GetBubble();
                if (bubble.GetAttributes() == 10)
                {
                    bubble.RemoveBubble(RemoveType.None);
                    Singleton<RemoveControl>.Instance.isCombo = true;
                }
            }
        }
    }

    private void CheckBing()
    {
        List<Point> around = GameManager.Instance.GetAround(point);
        for (int i = 0; i < around.Count; i++)
        {
            if (CheckBox(around[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(around[i]);
                BubbleObj bubble = box.GetBubble();
                if (bubble.GetAttributes() == 12)
                {
                    bubble.RemoveBubble(RemoveType.None);
                    Singleton<RemoveControl>.Instance.isCombo = true;
                }
            }
        }
    }

    private bool CheckMu(bool isRemoveSlef)
    {
        int num = 0;
        List<Point> around = GameManager.Instance.GetAround(point);
        for (int i = 0; i < around.Count; i++)
        {
            if (CheckBox(around[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(around[i]);
                BubbleObj bubble = box.GetBubble();
                if (bubble.GetAttributes() == 13)
                {
                    bubble.RemoveBubble(RemoveType.None);
                    num++;
                    Singleton<RemoveControl>.Instance.isCombo = true;
                }
            }
        }
        if (isRemoveSlef && num > 0)
        {
            return true;
        }
        return false;
    }

    private bool CheckRanSe(BubbleData _data)
    {
        int num = 0;
        List<Point> around = GameManager.Instance.GetAround(point);
        for (int i = 0; i < around.Count; i++)
        {
            if (CheckBox(around[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(around[i]);
                BubbleObj bubble = box.GetBubble();
                if (bubble.GetAttributes() == 9 && bubble.GetChangeColor())
                {
                    bubble.BubbleRanse(_data);
                    num++;
                }
            }
        }
        if (num > 0)
        {
            return true;
        }
        return false;
    }

    private void CheckBBB()
    {
        List<Point> around = GameManager.Instance.GetAround(point);
        for (int i = 0; i < around.Count; i++)
        {
            if (CheckBox(around[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(around[i]);
                BubbleObj bubble = box.GetBubble();
                if (bubble.GetAttributes() == 32)
                {
                    bubble.RemoveBubble(RemoveType.None);
                    Singleton<RemoveControl>.Instance.isCombo = true;
                }
            }
        }
    }

    private void RemoveBBB()
    {
        AudioPlayManager.PlaySFX2D("sfx_laser");
        GameScene.Instance.CameraShake();

        List<Point> around = GameManager.Instance.GetAround(point);
        for (int i = 0; i < around.Count; i++)
        {
            if (CheckBox(around[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(around[i]);
                BubbleObj bubble = box.GetBubble();
                bubble.SetRemoveIndex(3);
                bubble.RemoveBubble(RemoveType.None);
            }
        }
        Singleton<RemoveControl>.Instance.CheckFall(8);
    }

    private void BubbleRanse(BubbleData _data)
    {
        if (_data.key != "A" && _data.key != "B" && _data.key != "C" && _data.key != "D" && _data.key != "E")
        {
            _data.key = GameManager.Instance.GetRandomKey();
        }
        if (_data.key == "A")
        {
            PlaySkillEff("fx_skill_ranse_remove_1", 2);
        }
        else if (_data.key == "B")
        {
            PlaySkillEff("fx_skill_ranse_remove_2", 2);
        }
        else if (_data.key == "C")
        {
            PlaySkillEff("fx_skill_ranse_remove_3", 2);
        }
        else if (_data.key == "D")
        {
            PlaySkillEff("fx_skill_ranse_remove_4", 2);
        }
        else if (_data.key == "E")
        {
            PlaySkillEff("fx_skill_ranse_remove_5", 2);
        }
        List<Point> around = GameManager.Instance.GetAround(this.point);
        List<Point> list = new List<Point>();
        GameManager.Instance.GetAnimList2(list, around, this.point);
        for (int i = 0; i < around.Count; i++)
        {
            if (CheckBox(around[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(around[i]);
                BubbleObj bubble = box.GetBubble();
                if (bubble.GetBubbleType() <= 5 && bubble.GetAttributes() != 100 && bubble.GetAttributes() != 101 && !IsSkill(bubble))
                {
                    bubble.mBubbleData.key = _data.key;
                    bubble.Init(bubble.mBubbleData);
                }
            }
        }
        for (int j = 0; j < list.Count; j++)
        {
            if (!CheckBox(list[j]))
            {
                continue;
            }
            if (mBubbleData.row % 2 != 0)
            {
                Point point = list[j];
                if (point.Y == mBubbleData.col)
                {
                    Point point2 = list[j];
                    if (point2.X == mBubbleData.row + 2)
                    {
                        continue;
                    }
                }
                Point point3 = list[j];
                if (point3.Y == mBubbleData.col)
                {
                    Point point4 = list[j];
                    if (point4.X == mBubbleData.row - 2)
                    {
                        continue;
                    }
                }
                Point point5 = list[j];
                if (point5.X == mBubbleData.row + 1)
                {
                    Point point6 = list[j];
                    if (point6.Y == mBubbleData.col - 1)
                    {
                        continue;
                    }
                }
                Point point7 = list[j];
                if (point7.X == mBubbleData.row + 1)
                {
                    Point point8 = list[j];
                    if (point8.Y == mBubbleData.col + 2)
                    {
                        continue;
                    }
                }
                Point point9 = list[j];
                if (point9.X == mBubbleData.row - 1)
                {
                    Point point10 = list[j];
                    if (point10.Y == mBubbleData.col - 1)
                    {
                        continue;
                    }
                }
                Point point11 = list[j];
                if (point11.X == mBubbleData.row - 1)
                {
                    Point point12 = list[j];
                    if (point12.Y == mBubbleData.col + 2)
                    {
                        continue;
                    }
                }
            }
            else
            {
                Point point13 = list[j];
                if (point13.Y == mBubbleData.col)
                {
                    Point point14 = list[j];
                    if (point14.X == mBubbleData.row + 2)
                    {
                        continue;
                    }
                }
                Point point15 = list[j];
                if (point15.Y == mBubbleData.col)
                {
                    Point point16 = list[j];
                    if (point16.X == mBubbleData.row - 2)
                    {
                        continue;
                    }
                }
                Point point17 = list[j];
                if (point17.Y == mBubbleData.row + 1)
                {
                    Point point18 = list[j];
                    if (point18.X == mBubbleData.col - 2)
                    {
                        continue;
                    }
                }
                Point point19 = list[j];
                if (point19.Y == mBubbleData.row + 1)
                {
                    Point point20 = list[j];
                    if (point20.X == mBubbleData.col + 1)
                    {
                        continue;
                    }
                }
                Point point21 = list[j];
                if (point21.X == mBubbleData.row - 1)
                {
                    Point point22 = list[j];
                    if (point22.Y == mBubbleData.col - 2)
                    {
                        continue;
                    }
                }
                Point point23 = list[j];
                if (point23.X == mBubbleData.row - 1)
                {
                    Point point24 = list[j];
                    if (point24.Y == mBubbleData.col + 1)
                    {
                        continue;
                    }
                }
            }
            BoxObj box2 = GameManager.Instance.GetBox(list[j]);
            BubbleObj bubble2 = box2.GetBubble();
            if (bubble2.GetBubbleType() <= 5 && bubble2.GetAttributes() != 100 && bubble2.GetAttributes() != 101 && !IsSkill(bubble2))
            {
                bubble2.mBubbleData.key = _data.key;
                bubble2.Init(bubble2.mBubbleData);
            }
        }
        mBubbleData.key = _data.key;
        Init(mBubbleData);
    }

    private bool IsSkill(BubbleObj bubble)
    {
        if (bubble.GetSkill(1) || bubble.GetSkill(2) || bubble.GetSkill(3) || bubble.GetSkill(4))
        {
            return true;
        }
        if (bubble.GetProp(1) || bubble.GetProp(2) || bubble.GetProp(3))
        {
            return true;
        }
        return false;
    }

    private void RemoveProp1()
    {
        AudioPlayManager.PlaySFX2D("item_boom");
        GameScene.Instance.CameraShake();
        List<Point> around = GameManager.Instance.GetAround(point);
        for (int i = 0; i < around.Count; i++)
        {
            if (CheckBox(around[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(around[i]);
                BubbleObj bubble = box.GetBubble();
                bubble.RemoveBubble(RemoveType.BeRmove);
            }
        }
        List<Point> list = new List<Point>();
        GameManager.Instance.GetAnimList2(list, around, point);
        for (int j = 0; j < list.Count; j++)
        {
            if (CheckBox(list[j]))
            {
                BoxObj box2 = GameManager.Instance.GetBox(list[j]);
                BubbleObj bubble2 = box2.GetBubble();
                bubble2.RemoveBubble(RemoveType.BeRmove);
            }
        }
        Singleton<RemoveControl>.Instance.CheckFall(8);
    }

    private void RemoveProp2()
    {
    }

    private void RemoveProp3()
    {
        AudioPlayManager.PlaySFX2D("skill_super_4");
        string a = string.Empty;
        if (hitBubble != null)
        {
            int num = int.Parse(Singleton<DataBubble>.Instance.GetContentByKeyAndType(hitBubble.key, BubbleType.type));
            if (num <= 5)
            {
                a = hitBubble.key;
            }
        }
        if (a == string.Empty)
        {
            List<Point> around = GameManager.Instance.GetAround(point);
            for (int i = 0; i < around.Count; i++)
            {
                if (CheckBox(around[i]))
                {
                    BoxObj box = GameManager.Instance.GetBox(around[i]);
                    BubbleObj bubble = box.GetBubble();
                    if (bubble.GetBubbleType() <= 5)
                    {
                        a = bubble.key;
                        break;
                    }
                }
            }
        }
        if (a == string.Empty)
        {
            a = GameManager.Instance.GetRandomKey();
        }
        if (a == string.Empty)
        {
            Singleton<RemoveControl>.Instance.CheckFall(20);
            return;
        }
        int num2 = int.Parse(Singleton<DataBubble>.Instance.GetContentByKeyAndType(a, BubbleType.type));
        int maxRow = Singleton<CameraMove>.Instance.GetMaxRow();
        List<BubbleObj> list = new List<BubbleObj>();
        List<BubbleObj> bubbleArray = GameManager.Instance.GetBubbleArray();
        for (int j = 0; j < bubbleArray.Count; j++)
        {
            if (bubbleArray[j].point.X > maxRow - 11 && CheckBox(bubbleArray[j].point) && bubbleArray[j].GetBubbleType() == num2)
            {
                list.Add(bubbleArray[j]);
            }
        }
        for (int k = 0; k < list.Count; k++)
        {
            BubbleObj bubbleObj = list[k];
            bubbleObj.SetRemoveIndex(10);
            bubbleObj.PlayRemoveEff("FX/Prop/fx_mofa_elf_select", isAutoPath: false, 0.8f);
            bubbleObj.RemoveBubble(RemoveType.BeRmove);
        }
        Singleton<RemoveControl>.Instance.CheckFall(25);
    }

    private void RemoveBing()
    {
        AudioPlayManager.PlaySFX2D("b_bingshuang");
        GameScene.Instance.CameraShake();
        PlaySkillEff("fx_skill_ice_remove", 2);
        List<Point> around = GameManager.Instance.GetAround(point);
        for (int i = 0; i < around.Count; i++)
        {
            if (CheckBox(around[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(around[i]);
                BubbleObj bubble = box.GetBubble();
                bubble.RemoveBubble(RemoveType.BeRmove);
            }
        }
    }

    private void RemoveHuoQiu()
    {
        AudioPlayManager.PlaySFX2D("b_fire");
        PlaySkillEff("fx_skill_fire_remove", 2);
        List<Point> around = GameManager.Instance.GetAround(point);
        List<Point> list = new List<Point>();
        GameManager.Instance.GetAnimList2(list, around, point);
        for (int i = 0; i < around.Count; i++)
        {
            if (CheckBox(around[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(around[i]);
                BubbleObj bubble = box.GetBubble();
                bubble.SetRemoveIndex(10);
                bubble.RemoveBubble(RemoveType.BeRmove);
            }
        }
        for (int j = 0; j < list.Count; j++)
        {
            if (CheckBox(list[j]))
            {
                BoxObj box2 = GameManager.Instance.GetBox(list[j]);
                BubbleObj bubble2 = box2.GetBubble();
                bubble2.SetRemoveIndex(15);
                bubble2.RemoveBubble(RemoveType.BeRmove);
            }
        }
        Singleton<RemoveControl>.Instance.CheckFall(20);
    }

    private void RemoveSkill3Count()
    {
        GameScene.Instance.CameraShake();
        PlaySkillEff("fx_3skill_remove2", 2, isCenter: true);
        PlaySkillEff("fx_3skill_bg", 3);
        for (int i = point.X; i <= point.X + 3; i++)
        {
            if (i >= GameManager.rows)
            {
                continue;
            }
            for (int j = 0; j < GameManager.cols - mBubbleData.row % 2; j++)
            {
                if (CheckBox(new Point(i, j)))
                {
                    BoxObj box = GameManager.Instance.GetBox(new Point(i, j));
                    BubbleObj bubble = box.GetBubble();
                    bubble.SetRemoveIndex(3);
                    bubble.RemoveBubble(RemoveType.BeRmove);
                }
            }
        }
        for (int num = point.X - 1; num >= point.X - 3; num--)
        {
            if (num >= 0)
            {
                for (int k = 0; k < GameManager.cols - num % 2; k++)
                {
                    if (CheckBox(new Point(num, k)))
                    {
                        BoxObj box2 = GameManager.Instance.GetBox(new Point(num, k));
                        BubbleObj bubble2 = box2.GetBubble();
                        bubble2.SetRemoveIndex(3);
                        bubble2.RemoveBubble(RemoveType.BeRmove);
                    }
                }
            }
        }
        Singleton<RemoveControl>.Instance.CheckFall(8);
    }

    private void RemoveSkill4Count()
    {
        AudioPlayManager.PlaySFX2D("skill_super_2");
        PlaySkillEff("fx_4skill_light", 3);
        int maxRow = Singleton<CameraMove>.Instance.GetMaxRow();
        for (int i = maxRow - 11; i <= maxRow; i++)
        {
            if (i < 0)
            {
                continue;
            }
            for (int j = 0; j < GameManager.cols - i % 2; j++)
            {
                if (CheckBox(new Point(i, j)))
                {
                    BoxObj box = GameManager.Instance.GetBox(new Point(i, j));
                    BubbleObj bubble = box.GetBubble();
                    bubble.SetRemoveIndex(20);
                    bubble.removeBySkillCount4 = true;
                    bubble.RemoveBubble(RemoveType.BeRmove);
                }
            }
        }
        Singleton<RemoveControl>.Instance.CheckFall(25);
    }

    private void RemoveMu()
    {
        AudioPlayManager.PlaySFX2D("b_wood");
        PlaySkillEff("fx_skill_wood_remove", 2);
        int num = 0;
        List<Point> list = new List<Point>();
        List<int> list2 = new List<int>();
        for (int num2 = mBubbleData.row - 1; num2 >= mBubbleData.row - 5; num2--)
        {
            if (num2 >= 0)
            {
                num++;
                if (mBubbleData.row % 2 == 1)
                {
                    if (CheckBox(new Point(num2, mBubbleData.col)))
                    {
                        list.Add(new Point(num2, mBubbleData.col));
                        list2.Add(num);
                    }
                    if (num2 % 2 == 0 && mBubbleData.col + 1 <= GameManager.cols && CheckBox(new Point(num2, mBubbleData.col + 1)))
                    {
                        list2.Add(num);
                        list.Add(new Point(num2, mBubbleData.col + 1));
                    }
                }
                else
                {
                    if (CheckBox(new Point(num2, mBubbleData.col)))
                    {
                        list.Add(new Point(num2, mBubbleData.col));
                        list2.Add(num);
                    }
                    if (num2 % 2 == 1 && mBubbleData.col - 1 >= 0 && CheckBox(new Point(num2, mBubbleData.col - 1)))
                    {
                        list2.Add(num);
                        list.Add(new Point(num2, mBubbleData.col - 1));
                    }
                }
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            if (CheckBox(list[i]))
            {
                BoxObj box = GameManager.Instance.GetBox(list[i]);
                box.GetBubble().SetRemoveIndex(list2[i] * 5);
                box.GetBubble().RemoveBubble(RemoveType.BeRmove);
            }
        }
        if (list2.Count > 0)
        {
            Singleton<RemoveControl>.Instance.CheckFall(list2[list2.Count - 1] * 5 + 5);
        }
        else
        {
            Singleton<RemoveControl>.Instance.CheckFall(10);
        }
    }

    private void RemoveAddMove()
    {
        if (!GameScene.Instance.GetGameWin())
        {
            AudioPlayManager.PlaySFX2D("b_fettle_ball");
            UnityEngine.Debug.Log(" 加3步");
            PlayRemoveEff("FX/GameScene/fx_+-3qiu_remve", isAutoPath: false, 0.8f);
            GameScene.Instance.Operation(this, isAddMove: true);
        }
    }

    private void RemoveCutMove()
    {
        if (!GameScene.Instance.GetGameWin())
        {
            AudioPlayManager.PlaySFX2D("b_fettle_ball");
            UnityEngine.Debug.Log(" 减3步");
            PlayRemoveEff("FX/GameScene/fx_+-3qiu_remve", isAutoPath: false, 0.8f);
            GameScene.Instance.Operation(this, isAddMove: false);
        }
    }

    private void RemoveShanDian()
    {
        AudioPlayManager.PlaySFX2D("b_lightning");
        GameScene.Instance.CameraShake();
        PlaySkillEff("fx_skill_zap_remove", 2, isCenter: true);
        for (int i = 0; i < GameManager.cols - point.X % 2; i++)
        {
            if (CheckBox(new Point(point.X, i)))
            {
                BoxObj box = GameManager.Instance.GetBox(new Point(point.X, i));
                box.GetBubble().RemoveBubble(RemoveType.BeRmove);
            }
        }
        Singleton<RemoveControl>.Instance.CheckFall(5);
    }

    private void RandomBubble()
    {
        PlayEff("fx_bianse");
        mBubbleData.key = GameManager.Instance.GetRandomKey() + "1";
        BubbleData data = mBubbleTopData;
        bool flag = bMenOpen;
        Init(mBubbleData);
        InitTop(data);
        mBubbleTopData = data;
        bMenOpen = flag;
    }

    private void CheckJingXiang()
    {
        List<Point> around = GameManager.Instance.GetAround(point);
        for (int i = 0; i < around.Count; i++)
        {
            if (!CheckBox(around[i]))
            {
                continue;
            }
            BoxObj box = GameManager.Instance.GetBox(around[i]);
            BubbleObj bubble = box.GetBubble();
            if (bubble.GetAttributes() == 2 && bubble.GetChangeColor())
            {
                bubble.mBubbleData.key = mBubbleData.key;
                BubbleKey bubbleKey = (BubbleKey)Enum.Parse(typeof(BubbleKey), mBubbleData.key);
                int num = int.Parse(Singleton<DataBubble>.Instance.GetContentByKeyAndType(bubbleKey, BubbleType.type));
                if (num == 1)
                {
                    bubble.mBubbleData.key = "A";
                }
                if (num == 2)
                {
                    bubble.mBubbleData.key = "B";
                }
                if (num == 3)
                {
                    bubble.mBubbleData.key = "C";
                }
                if (num == 4)
                {
                    bubble.mBubbleData.key = "D";
                }
                if (num == 5)
                {
                    bubble.mBubbleData.key = "E";
                }
                bubble.Init(bubble.mBubbleData);
                bubble.PlayEff("fx_jingxiang");
                AudioPlayManager.PlaySFX2D("b_jingxiang");
            }
        }
    }

    private bool CheckBox(Point _point)
    {
        BoxObj box = GameManager.Instance.GetBox(_point);
        if (box != null && box.GetBubble() != null && box.GetBubble().GetBubbleState() == BubbleState.None && box.GetBubble().GetCanChecRemove())
        {
            return true;
        }
        return false;
    }

    public void RemoveBubble(RemoveType removeType)
    {
        if (GetBubbleState() != BubbleState.Destroy && GetBubbleState() != BubbleState.Remove)
        {
            SetState(BubbleState.Remove);
            if (bubbleType == 16)
            {
                RemoveBigEff();
                return;
            }
            mCollider.gameObject.layer = LayerMask.NameToLayer("FallBubble");
            RemoveBubbleData();
            Timer.DelayCallBack((float)iRemoveIndex * GameManager.removeTime, delegate
            {
                CheckGorundByRemove();
                if (isShootRemove)
                {
                    GameScene.Instance.AddScore(1000 * overIndex, null, 11, base.transform);
                }
                else
                {
                    GameScene.Instance.AddScore(10 * GameManager.Instance.iCombo, this);
                }
                CheckRemove();
                DestroyObject();
            });
        }
    }

    private void RemoveBigEff()
    {
        if (mBubbleData.i != 0)
        {
            List<Point> around = GameManager.Instance.GetAround(point);
            int num = 0;
            BubbleObj bubble;
            while (true)
            {
                if (num >= around.Count)
                {
                    return;
                }
                BoxObj box = GameManager.Instance.GetBox(around[num]);
                if (box != null && box.GetBubble() != null)
                {
                    bubble = box.GetBubble();
                    BubbleData bubbleData = bubble.GetBubbleData();
                    if (bubbleData.i == 0 && bubble.GetBubbleType() == 16 && bubble.GetBubbleState() == BubbleState.None)
                    {
                        break;
                    }
                }
                num++;
            }
            bubble.RemoveBubble(RemoveType.BeRmove);
            return;
        }
        List<Point> around2 = GameManager.Instance.GetAround(point);
        for (int i = 0; i < around2.Count; i++)
        {
            BoxObj box2 = GameManager.Instance.GetBox(around2[i]);
            if (box2 != null && box2.GetBubble() != null)
            {
                BubbleObj bubble2 = box2.GetBubble();
                if (bubble2.GetBubbleType() == 16)
                {
                    bubble2.RemoveBubbleData();
                    bubble2.DestroyObject(isShowRemoveEff: false);
                }
            }
        }
        RemoveBubbleData();
        DestroyObject();
    }

    public void DestroyObject(bool isShowRemoveEff = true)
    {
        if (isShowRemoveEff)
        {
            PlayRemoveEff();
        }
        TopRemoveAndFall();
        if (mBubbleData.i == 0)
        {
            GameScene.Instance.RemoveTarget(this);
        }
        base.gameObject.name = "Bubble";
        SetState(BubbleState.Remove);
        GameObjectManager.DestroyPoolObject(GetComponent<BubblePool>());
    }

    public void DestroyReadyObject()
    {
        base.gameObject.name = "Bubble";
        GameObjectManager.DestroyPoolObject(GetComponent<BubblePool>());
    }

    private void InitRes(bool shootEndCreate = false)
    {
        base.transform.localScale = Vector3.one;
        base.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        render.GetComponent<Renderer>().enabled = true;
        render.sprite = ResManage.Instance.bubbles[bubbleImg];
        topRender.sprite = null;
        if (isReadyBubble)
        {
            render.GetComponent<Renderer>().sortingOrder = 4;
        }
        if (bubbleType == 16)
        {
            render.GetComponent<Renderer>().sortingOrder = 3;
        }
        else
        {
            render.GetComponent<Renderer>().sortingOrder = 5;
        }
        if (bubbleType == 16)
        {
            if (mBubbleData.i != 0)
            {
                render.GetComponent<Renderer>().enabled = false;
                return;
            }
            elfObj = AddPoolFx("FX/Elf/bigElf");
            SwitchAniMax(elfObj.transform.Find("bigElf").gameObject, "elf_static1");
        }
        else if (GetAttributes() == 100)
        {
            elfObj = AddPoolFx("FX/Elf/elfin");
            SwitchAni(elfObj.gameObject, "elf_static1");
        }
        else if (bubbleType == 12)
        {
            AddFx("FX/Skill/fx_skill_fire");
        }
        else if (bubbleType == 13)
        {
            AddFx("FX/Skill/fx_skill_zap");
        }
        else if (bubbleType == 14)
        {
            AddFx("FX/Skill/fx_skill_ice");
        }
        else if (bubbleType == 15)
        {
            AddFx("FX/Skill/fx_skill_wood");
        }
        else if (bubbleType == 20)
        {
            AddFx("FX/Skill/fx_skill_ranse");
        }
        else if (bubbleType == 21)
        {
            zhizhuObj = AddFx("FX/Skill/fx_skill_zhizhu");
            SwitchZhiZhu(zhizhuObj, "static");
        }
        else if (bubbleType == 23)
        {
            AddFx("FX/Skill/fx_skill_jiguang");
        }
        else if (bubbleType == 17)
        {
            AddFx("FX/Prop/fx_mofa_bomb_64", isScale: true);
            AddFx("FX/Prop/fx_mofa_light", isScale: true);
            if (!shootEndCreate)
            {
                PlayEff("FX/Prop/fx_prop4", isAutoPath: false);
            }
        }
        else if (bubbleType == 18)
        {
            AddFx("FX/Prop/fx_mofa_jian_64", isScale: true);
            AddFx("FX/Prop/fx_mofa_light", isScale: true);
            if (!shootEndCreate)
            {
                PlayEff("FX/Prop/fx_prop5", isAutoPath: false);
            }
        }
        else if (bubbleType == 19)
        {
            AddFx("FX/Prop/fx_mofa_elf_64", isScale: true);
            AddFx("FX/Prop/fx_mofa_light", isScale: true);
            if (!shootEndCreate)
            {
                PlayEff("FX/Prop/fx_prop6", isAutoPath: false);
            }
        }
    }

    private void InitTopRes()
    {
        if (mBubbleTopData.key == "S")
        {
            topRender.gameObject.SetActive(value: true);
            topRender.sprite = ResManage.Instance.bubbleTops[0];
        }
        else if (mBubbleTopData.key == "L")
        {
            topRender.gameObject.SetActive(value: true);
            topRender.sprite = ResManage.Instance.bubbleTops[1];
        }
        else if (mBubbleTopData.key == "Suo")
        {
            topRender.gameObject.SetActive(value: true);
            topRender.sprite = ResManage.Instance.bubbleTops[2];
        }
        else if (mBubbleTopData.key == "YunDian")
        {
            topRender.gameObject.SetActive(value: true);
            AddFx("FX/Skill/fx_skill_dots");
            if (ObjFX.GetComponent<SkeletonAnimation>() != null)
            {
                SkeletonAnimation component = ObjFX.GetComponent<SkeletonAnimation>();
                component.Initialize(overwrite: true);
                int num = UnityEngine.Random.Range(1, 4);
                component.state.SetAnimation(0, "anim" + num, loop: true);
            }

        }
        else if (mBubbleTopData.key == "Men" || mBubbleTopData.key == "Men1")
        {
            AddFx("FX/Skill/fx_skill_door");
            if (mBubbleTopData.key == "Men1")
            {
                SubDoor(bopen: false, binit: true);
            }
            else
            {
                SubDoor(bopen: true, binit: true);
            }
        }
        else if (mBubbleTopData.key == "Wang")
        {
            topRender.gameObject.SetActive(value: true);
            topRender.sprite = ResManage.Instance.bubbleTops[3];
        }
    }

    public void SubDoor(bool bopen = false, bool binit = false)
    {
        if (ObjFX.GetComponent<SkeletonAnimation>() == null)
            return;
        SkeletonAnimation component = ObjFX.GetComponent<SkeletonAnimation>();
        if (component.name != "fx_skill_door" && ObjFX2 != null)
        {
            component = ObjFX2.GetComponent<SkeletonAnimation>();
        }
        if (component.name != "fx_skill_door" && ObjFX3 != null)
        {
            component = ObjFX3.GetComponent<SkeletonAnimation>();
        }
        if (component.name != "fx_skill_door" && ObjFX4 != null)
        {
            component = ObjFX4.GetComponent<SkeletonAnimation>();
        }
        if (component.name != "fx_skill_door")
        {
            return;
        }
        if (binit)
        {
            if (bopen)
            {
                component.state.AddAnimation(0, "open_static", loop: false, 0f);
                bMenOpen = false;
            }
            else
            {
                component.state.AddAnimation(0, "close_static", loop: false, 0f);
                bMenOpen = true;
            }
        }
        else if (bopen)
        {
            component.state.AddAnimation(0, "open", loop: false, 0f);
            bMenOpen = true;
        }
        else if (bMenOpen)
        {
            component.state.AddAnimation(0, "close", loop: false, 0f);
            bMenOpen = false;
        }
        else
        {
            component.state.AddAnimation(0, "open", loop: false, 0f);
            bMenOpen = true;
        }
    }

    public void InitTop(BubbleData data)
    {
        InitTopData(data);
        topRender.gameObject.SetActive(value: false);
        InitTopRes();
        TopKey = mBubbleTopData.key;
        if (mBubbleTopData.key == "S")
        {
            topRender.sprite = ResManage.Instance.bubbleTops[0];
        }
        else if (!(mBubbleTopData.key == "L") && !(mBubbleTopData.key == "Suo") && !(mBubbleTopData.key == "YunDian") && !(mBubbleTopData.key == "Men") && !(mBubbleTopData.key == "Men1") && !(mBubbleTopData.key == "Wang"))
        {
        }
    }

    public bool GetChangeColor()
    {
        if (mBubbleTopData.key == "Men" || mBubbleTopData.key == "Men1")
        {
            return false;
        }
        return true;
    }

    public void ChangeMen()
    {
        if (mBubbleTopData.key == "Men" || mBubbleTopData.key == "Men1")
        {
            SubDoor();
        }
    }

    public void TopRemoveAndFall()
    {
        RemoveTop();
        if (mBubbleTopData.key == "S")
        {
            PlayRemoveEff("fx_cloud_remove");
        }
        else if (mBubbleTopData.key == "Suo")
        {
            PlayRemoveEff("fx_shuo1_remove");
            RemoveSuo();
            AudioPlayManager.PlaySFX2D("b_lock_destroy");
        }
        else if (mBubbleTopData.key == "L")
        {
            PlayRemoveEff("fx_shuo2_remove");
            AudioPlayManager.PlaySFX2D("b_lock_destroy");
        }
        mBubbleTopData.key = string.Empty;
    }

    public void TopGorundByRemove()
    {
        if (mBubbleTopData.key == "S")
        {
            RemoveTop();
            PlayRemoveEff("fx_cloud_remove");
            mBubbleTopData.key = string.Empty;
        }
        else if (mBubbleTopData.key == "Wang")
        {
            RemoveTop();
            PlayRemoveEff("FX/Function/fx_zhizhusi_remove", isAutoPath: false);
            mBubbleTopData.key = string.Empty;
            AudioPlayManager.PlaySFX2D("b_spider_web_destroy");
        }
    }

    public void TopGorundByHit(bool isRemoveSlef = false)
    {
        if (mBubbleTopData.key == "S")
        {
            RemoveTop();
            PlayRemoveEff("fx_cloud_remove");
            mBubbleTopData.key = string.Empty;
        }
        else if (mBubbleTopData.key == "Wang")
        {
            RemoveTop();
            PlayRemoveEff("FX/Function/fx_zhizhusi_remove", isAutoPath: false);
            mBubbleTopData.key = string.Empty;
            AudioPlayManager.PlaySFX2D("b_spider_web_destroy");
        }
    }

    public void RemoveTop()
    {
        topRender.gameObject.SetActive(value: false);
    }

    private void CheckL()
    {
        BubbleData topBubbleData = GetTopBubbleData();
        if (topBubbleData.key != "L")
        {
            return;
        }
        List<Point> list = new List<Point>();
        int num = point.Y - 1;
        while (num >= 0 && CheckBox(new Point(point.X, num)))
        {
            BoxObj box = GameManager.Instance.GetBox(new Point(point.X, num));
            BubbleObj bubble = box.GetBubble();
            BubbleData topBubbleData2 = bubble.GetTopBubbleData();
            if (!(topBubbleData2.key == "L"))
            {
                BubbleData topBubbleData3 = bubble.GetTopBubbleData();
                if (topBubbleData3.key == "Suo")
                {
                    return;
                }
                break;
            }
            num--;
        }
        for (int i = point.Y + 1; i < GameManager.cols && CheckBox(new Point(point.X, i)); i++)
        {
            BoxObj box2 = GameManager.Instance.GetBox(new Point(point.X, i));
            BubbleObj bubble2 = box2.GetBubble();
            BubbleData topBubbleData4 = bubble2.GetTopBubbleData();
            if (!(topBubbleData4.key == "L"))
            {
                BubbleData topBubbleData5 = bubble2.GetTopBubbleData();
                if (topBubbleData5.key == "Suo")
                {
                    return;
                }
                break;
            }
        }
        TopRemoveAndFall();
    }

    private void RemoveSuo()
    {
        List<Point> list = new List<Point>();
        int num = point.Y - 1;
        while (num >= 0 && CheckBox(new Point(point.X, num)))
        {
            BoxObj box = GameManager.Instance.GetBox(new Point(point.X, num));
            BubbleObj bubble = box.GetBubble();
            BubbleData topBubbleData = bubble.GetTopBubbleData();
            if (topBubbleData.key == "L")
            {
                bubble.TopRemoveAndFall();
            }
            else
            {
                BubbleData topBubbleData2 = bubble.GetTopBubbleData();
                if (topBubbleData2.key == "Suo")
                {
                    break;
                }
            }
            num--;
        }
        for (int i = point.Y + 1; i < GameManager.cols && CheckBox(new Point(point.X, i)); i++)
        {
            BoxObj box2 = GameManager.Instance.GetBox(new Point(point.X, i));
            BubbleObj bubble2 = box2.GetBubble();
            BubbleData topBubbleData3 = bubble2.GetTopBubbleData();
            if (topBubbleData3.key == "L")
            {
                bubble2.TopRemoveAndFall();
                continue;
            }
            BubbleData topBubbleData4 = bubble2.GetTopBubbleData();
            if (topBubbleData4.key == "Suo")
            {
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("AirBubble") && mCollider.gameObject.layer == LayerMask.NameToLayer("ReadyBubble"))
        {
            coll.transform.parent.GetComponent<BubbleObj>().RemoveBubble(RemoveType.BeRmove);
        }
        if (isProp2 && (coll.gameObject.layer == LayerMask.NameToLayer("AirBubble") || coll.gameObject.layer == LayerMask.NameToLayer("Bubble")) && mCollider.gameObject.layer == LayerMask.NameToLayer("ReadyBubble"))
        {
            coll.transform.parent.GetComponent<BubbleObj>().RemoveBubble(RemoveType.BeRmove);
        }
    }

    public void Init(BubbleData data, bool shootEndCreate = false)
    {
        mCollider = base.transform.Find("Collider").GetComponent<CircleCollider2D>();
        if (data.isReadyBubble)
        {
            data.row = -1;
            data.col = -1;
            mCollider.gameObject.SetActive(value: false);
        }
        else
        {
            mCollider.gameObject.SetActive(value: true);
            base.gameObject.name = "Bubble" + data.row + "_" + data.col;
        }
        InitData(data);
        InitRes(shootEndCreate);
    }

    public void ChangeToSkill(int index)
    {
        if (index < 5)
        {
            if (isSkill1 && index == 1)
            {
                AddFx("FX/Skill/fx_skill_zap");
                PlayEff("FX/Skill/fx_skill_juneng", isAutoPath: false);
            }
            if (isSkill2 && index == 2)
            {
                AddFx("FX/Skill/fx_skill_fire");
                PlayEff("FX/Skill/fx_skill_juneng", isAutoPath: false);
            }
            if (isSkill3 && index == 3)
            {
                AddFx("FX/Skill/fx_skill_wood");
                PlayEff("FX/Skill/fx_skill_juneng", isAutoPath: false);
            }
            if (isSkill4 && index == 4)
            {
                AddFx("FX/Skill/fx_skill_ice");
                PlayEff("FX/Skill/fx_skill_juneng", isAutoPath: false);
            }
        }
        else
        {
            if (isSkill1)
            {
                AddFx("FX/Skill/fx_skill_zap");
            }
            if (isSkill2)
            {
                AddFx("FX/Skill/fx_skill_fire");
            }
            if (isSkill3)
            {
                AddFx("FX/Skill/fx_skill_wood");
            }
            if (isSkill4)
            {
                AddFx("FX/Skill/fx_skill_ice");
            }
        }
    }

    private void SetState(BubbleState _state)
    {
        if (state != BubbleState.Remove)
        {
            state = _state;
            switch (_state)
            {
                case BubbleState.Remove:
                    base.transform.parent = GameManager.Instance.RemoveParent.transform;
                    break;
                case BubbleState.Fall:
                    base.transform.parent = GameManager.Instance.FallParent.transform;
                    break;
            }
        }
    }
}
