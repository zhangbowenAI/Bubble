
using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using Thinksquirrel.CShake;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    public static GameScene Instance;

    public GameObject DownObject;

    public Transform TopBorder;

    public Transform TopBorder2;

    public Transform topUI;

    public Text scoreText;

    public Image scoreProgress;

    public Image startImg1;

    public Image startImg2;

    public Image startImg3;

    public Image targetImg;

    public Text targetText;

    public Text moveText;

    public Text levelText;

    public SpriteRenderer mapBG;

    public GameObject HSBJ;

    private int gameScore;

    private int gameStar;

    public GameObject GameOverMove1;

    public GameObject GameOverMove2;

    public RectTransform GameOverMoveUITop;

    public RectTransform GameOverMoveUIDown;

    public GameObject WinPanel;

    public GameObject LosePanel;

    private SkeletonAnimation skelet;

    public Image BombImage;

    public Image ArrowImage;

    public Image CaiQiuImage;

    public Text BombText;

    public Text ArrowText;

    public Text CaiQiuText;

    public Sprite SkillAdd;

    public Sprite SkillNOAdd;

    public Sprite SkillLockImage;

    public GameObject gameCombo5;

    public GameObject scoreParent;

    public GameObject playProp1;

    public GameObject playProp2;

    public GameObject playProp3;

    public GameObject playProp4;

    public GameObject playPropMagicPower;

    public FZDownBorder FZ1;

    public FZDownBorder FZ2;

    public FZDownBorder FZ3;

    public FZDownBorder FZ4;

    public GameObject comboTrs;

    public GameObject comboText;

    public GameObject comboText2;

    public GameObject only10Trs;

    public bool initEnd;

    public int iWatchBuyMove;

    private float orthographicSize;

    public GameObject fx_ui_goalStarObj;

    public GameObject fx_goalstar_lightObj;

    public GameObject testwinlBtn;

    public GameObject testNextLevelBtn;

    private int iTargetNum;

    private int iHarvestNum;

    private int iNowMoveTargetNum;

    private int iNowMoveTextNum;

    private PoolObject ObjPoolFX;

    private bool isPlayGameWin;

    private BubbleObj bubbleObj;

    private static int iiOverShootBubbleCountIndex = -5;

    private bool biiOverShootBubbleCountIndexstate = true;

    private PoolObject ObjTmp;

    public int BeforeStar;

    private int CurrerStar;

    private bool isCheckGameLose;

    private int iFaultCombo;

    private int ivo_role_shoot;

    public SpriteRenderer srCurrentMapPlay;
    public SpriteRenderer srCurrentMapEndLevel;

    public GameObject gameEndBg;

    public int MoveNum
    {
        get;
        set;
    }

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ApplicationManager.SceneName = GameEntry.GameScene;

        // try
        // {
        //     AGame.MySdkManager.me.LoadAddIfNotLoaded();
        // }
        // catch { }

        if (AndroidManager.Instance.GetCDK() == 0)
        {
            testNextLevelBtn.SetActive(value: false);
            testwinlBtn.SetActive(value: false);
        }
        if (GameManager.GetFullScreen())
        {
            GameOverMoveUITop.DOAnchorPos(new Vector2(0f, -120), 0f);
        }
        initEnd = false;
        iWatchBuyMove = 0;
        AudioPlayManager.PlayMusic2D("bg_music_main", 1);
        Singleton<CameraMove>.Instance.InitData();
        GameManager.Instance.InitCamera();
        if (!GameManager.Instance.InitData())
        {
            UnityEngine.Debug.LogError("Data initialization failed");
        }
        AndroidManager.Instance.UpLevelData("enter");
        orthographicSize = Camera.main.orthographicSize;
        DownObject.transform.localPosition = new Vector3(0f, 0f - orthographicSize - 5f, 0f);
        TopBorder.position = new Vector3(0f, orthographicSize - 0.4f, 0f);
        TopBorder2.localPosition = new Vector3(0f, orthographicSize - 0.4f, 0f);
        if (GameManager.GetFullScreen())
        {
            TopBorder.position = new Vector3(0f, orthographicSize - 0.4f - 0.6f, 0f);
            TopBorder2.localPosition = new Vector3(0f, orthographicSize - 0.4f - 0.6f, 0f);
        }
        ReadyBubbleController.Instance.Init();
        GameManager.Instance.InitParent();
        GameManager.Instance.InitBubble();
        InitTarget();
        InitDaoju();
        Timer.DelayCallBack(0.5f, delegate
        {
            Singleton<CameraMove>.Instance.UpMove();
        });
        // skelet = jlxzObj.GetComponent<SkeletonAnimation>();
        GlobalEvent.AddEvent(GameEventEnum.UIFlushChane, ReceviceDaojuChange);
        FZ1.InitFZ();
        FZ2.InitFZ();
        FZ3.InitFZ();
        FZ4.InitFZ();
        if (GameGuid.Instance != null)
        {
            GameGuid.Instance.InitGame();
        }
        AndroidManager.Instance.StartLevel(Singleton<LevelManager>.Instance.GetNowLevel());
        gameEndBg.SetActive(false);
        InitCurrentMapImage();
    }

    void InitCurrentMapImage()
    {
        try
        {
            int nowLevel = Singleton<LevelManager>.Instance.GetNowLevel();
            int mapId = Singleton<MapData>.Instance.GetMapForLevelID(nowLevel);

            string strAtlasName = "map_01";

            strAtlasName = "map_0" + mapId;

            if (mapId > 9)
            {
                strAtlasName = "map_" + mapId;
            }

            // srCurrentMapEndLevel.sprite = AGame.AtlasManager.me.GetSprite(strAtlasName, AGame.AtlasManager.AtlasType.mapImage);
            // srCurrentMapPlay.sprite = AGame.AtlasManager.me.GetSprite(strAtlasName, AGame.AtlasManager.AtlasType.mapImage);
        }
        catch (Exception exp)
        {
            Debug.LogError(exp);
        }
    }

    private void ReceviceDaojuChange(params object[] objs)
    {
        InitDaoju();
    }

    public void InitDaoju()
    {
        int intRecord = RecordManager.GetIntRecord("LevelData", "SkillOpenNum_3", 0);
        int intRecord2 = RecordManager.GetIntRecord("LevelData", "SkillOpenNum_4", 0);
        int intRecord3 = RecordManager.GetIntRecord("LevelData", "SkillOpenNum_5", 0);
        BombImage.sprite = SkillAdd;
        ArrowImage.sprite = SkillAdd;
        CaiQiuImage.sprite = SkillAdd;
        BombText.gameObject.SetActive(value: false);
        ArrowText.gameObject.SetActive(value: false);
        CaiQiuText.gameObject.SetActive(value: false);
        if (intRecord > 0)
        {
            BombText.gameObject.SetActive(value: true);
            BombText.text = intRecord.ToString();
            BombImage.sprite = SkillNOAdd;
        }
        if (intRecord2 > 0)
        {
            ArrowText.gameObject.SetActive(value: true);
            ArrowText.text = intRecord2.ToString();
            ArrowImage.sprite = SkillNOAdd;
        }
        if (intRecord3 > 0)
        {
            CaiQiuText.gameObject.SetActive(value: true);
            CaiQiuText.text = intRecord3.ToString();
            CaiQiuImage.sprite = SkillNOAdd;
        }
        int intRecord4 = RecordManager.GetIntRecord("LevelData", "SkillOpen_3", 0);
        int intRecord5 = RecordManager.GetIntRecord("LevelData", "SkillOpen_4", 0);
        int intRecord6 = RecordManager.GetIntRecord("LevelData", "SkillOpen_5", 0);
        if (intRecord4 == 0)
        {
            BombImage.gameObject.SetActive(value: false);
            BombImage.gameObject.transform.parent.GetComponent<Image>().sprite = SkillLockImage;
        }
        if (intRecord5 == 0)
        {
            ArrowImage.gameObject.SetActive(value: false);
            ArrowImage.gameObject.transform.parent.GetComponent<Image>().sprite = SkillLockImage;
        }
        if (intRecord6 == 0)
        {
            CaiQiuImage.gameObject.SetActive(value: false);
            CaiQiuImage.gameObject.transform.parent.GetComponent<Image>().sprite = SkillLockImage;
        }
        switch (Singleton<LevelManager>.Instance.GetNowLevel())
        {
            case 18:
                BombText.gameObject.SetActive(value: true);
                BombText.text = "∞";
                BombImage.sprite = SkillNOAdd;
                break;
            case 19:
                ArrowText.gameObject.SetActive(value: true);
                ArrowText.text = "∞";
                ArrowImage.sprite = SkillNOAdd;
                break;
            case 20:
                CaiQiuText.gameObject.SetActive(value: true);
                CaiQiuText.text = "∞";
                CaiQiuImage.sprite = SkillNOAdd;
                break;
        }
    }

    private void InitTarget()
    {
        gameScore = 0;
        gameStar = 0;
        CurrerStar = 0;
        scoreText.text = "0";
        scoreProgress.fillAmount = 0f;
        startImg1.enabled = false;
        startImg2.enabled = false;
        startImg3.enabled = false;
        targetText.text = "0/" + Singleton<LevelManager>.Instance.iLevelCount;
        iTargetNum = Singleton<LevelManager>.Instance.iLevelCount;
        iHarvestNum = 0;
        iNowMoveTargetNum = 0;
        iNowMoveTextNum = 0;
        MoveNum = Singleton<LevelManager>.Instance.InitBubbleCount;
        moveText.text = MoveNum.ToString();
        levelText.text = "关卡" + Singleton<LevelManager>.Instance.GetNowLevel().ToString();
    }

    public void RemoveTarget(BubbleObj obj)
    {
        if (obj.GetAttributes() == 100)
        {
            ObjPoolFX = GameObjectManager.GetPoolObject("FX/Elf/elfin", topUI.gameObject);
            ObjPoolFX.transform.position = obj.transform.position;
            MeshRenderer component = ObjPoolFX.GetComponent<MeshRenderer>();
            component.sortingOrder = 110;
            MoveToTarget(ObjPoolFX, ObjPoolFX.transform.localPosition, targetImg.transform.localPosition);
            ObjPoolFX.GetComponentInChildren<SpriteRenderer>().sortingOrder = 100;
        }
        else if (obj.GetAttributes() == 101)
        {
            ObjPoolFX = GameObjectManager.GetPoolObject("FX/Elf/bigElf", topUI.gameObject);
            ObjPoolFX.transform.position = obj.transform.position;
            // MeshRenderer component2 = ObjPoolFX.transform.Find("bigElf").GetComponent<MeshRenderer>();
            // component2.sortingOrder = 110;
            MoveToTarget(ObjPoolFX, ObjPoolFX.transform.localPosition, targetImg.transform.localPosition, isBigElf: true);
            ObjPoolFX.GetComponentInChildren<SpriteRenderer>().sortingOrder = 100;
        }
    }

    public void MoveToTarget(PoolObject flyObj, Vector3 startPos, Vector3 endPos, bool isBigElf = false)
    {
        AudioPlayManager.PlaySFX2D("ui_save");
        iHarvestNum++;
        iNowMoveTargetNum++;
        float num = Vector3.Distance(startPos, endPos);
        if (num < 250f)
        {
            num = 250f;
        }
        num /= 400f;
        Vector3 vector = new Vector3(startPos.x - 0.2f, startPos.y + 0.1f, 0f);
        Vector3 centerPost = GameManager.Instance.GetCenterPost(startPos, endPos, 3f);
        Vector3[] path = new Vector3[3]
        {
            startPos,
            centerPost,
            endPos
        };
        if (MoveNum <= 5)
        {
            SwitchoverElfAni("succeed2", bLoop: false);
        }
        else
        {
            SwitchoverElfAni("succeed", bLoop: false);
        }
        if (isBigElf)
        {
            // SkeletonAnimation component = flyObj.transform.Find("bigElf").GetComponent<SkeletonAnimation>();
            // component.Initialize(overwrite: true);
            // component.loop = true;
            // component.state.SetAnimation(0, "elf_shake", loop: false);
        }
        else
        {
            SkeletonAnimation component2 = flyObj.GetComponent<SkeletonAnimation>();
            component2.Initialize(overwrite: true);
            component2.loop = true;
            component2.state.SetAnimation(0, "elf_shake", loop: false);
        }
        flyObj.transform.DOScale(new Vector2(300f, 300f), 0.3f).SetEase(Ease.OutSine);
        flyObj.transform.DOScale(new Vector2(120f, 120f), 0.3f).SetEase(Ease.OutSine).SetDelay(0.3f);
        flyObj.transform.DOScale(new Vector2(130f, 130f), 0.2f).SetEase(Ease.OutSine).SetDelay(0.6f);
        flyObj.transform.DOScale(new Vector2(100f, 100f), 0.1f).SetEase(Ease.OutSine).SetDelay(0.8f);
        flyObj.transform.DOLocalPath(path, num, PathType.CatmullRom, PathMode.TopDown2D, 20).SetEase(Ease.InQuad).OnComplete(delegate
        {
            TargetMoveEnd(flyObj);
        })
            .SetDelay(0.2f);
    }

    public void ReadyGoEnd()
    {
        if (Singleton<LevelManager>.Instance.GetNowLevel() > Singleton<UserData>.Instance.GetPassLevel())
        {
            Singleton<UserData>.Instance.PlaySkillUseMagicPower = (ContinuousItemArranger.gSequenceCount >= 1);
            Singleton<UserData>.Instance.PlaySkillUse1ByContinuousWin = (ContinuousItemArranger.gSequenceCount >= 2);
            Singleton<UserData>.Instance.PlaySkillUse2ByContinuousWin = (ContinuousItemArranger.gSequenceCount >= 3);
        }
        if (Singleton<UserData>.Instance.PlaySkillUseMagicPower || Singleton<UserData>.Instance.PlaySkillUse1 || Singleton<UserData>.Instance.PlaySkillUse2 || Singleton<UserData>.Instance.PlaySkillUse3 || Singleton<UserData>.Instance.PlaySkillUseViedo)
        {
            StartCoroutine(InitPropSkillAni());
        }
        else if (GameGuid.Instance != null)
        {
            GameGuid.Instance.InitGuid();
        }
    }

    public IEnumerator InitPropSkillAni()
    {
        playProp1.SetActive(value: false);
        playProp2.SetActive(value: false);
        playProp3.SetActive(value: false);
        playProp4.SetActive(value: false);
        playPropMagicPower.SetActive(value: false);
        int index2 = 0;

        if (Singleton<UserData>.Instance.PlaySkillUseMagicPower)
        {
            Singleton<UserData>.Instance.PlaySkillUseMagicPower = false;
            InitFlyPlayPropMagicPower();
            yield return new WaitForSeconds(1.1f);
        }
        if (Singleton<UserData>.Instance.PlaySkillUse1 || Singleton<UserData>.Instance.PlaySkillUse1ByContinuousWin)
        {
            MissionCheckMgr.Instance.CheckMission(MissionType.UseProp, 1);
            Singleton<UserData>.Instance.PlaySkillUse1 = false;
            if (!Singleton<UserData>.Instance.PlaySkillUse1ByContinuousWin)
            {
                Singleton<UserData>.Instance.DelProps(0);
            }
            Singleton<UserData>.Instance.PlaySkillUse1ByContinuousWin = false;
            index2++;
            InitFlyPlayProp1();
            yield return new WaitForSeconds(1.1f);
        }
        if (Singleton<UserData>.Instance.PlaySkillUse2 || Singleton<UserData>.Instance.PlaySkillUse2ByContinuousWin)
        {
            MissionCheckMgr.Instance.CheckMission(MissionType.UseProp, 1);
            Singleton<UserData>.Instance.PlaySkillUse2 = false;
            if (!Singleton<UserData>.Instance.PlaySkillUse2ByContinuousWin)
            {
                Singleton<UserData>.Instance.DelProps(1);
            }
            Singleton<UserData>.Instance.PlaySkillUse2ByContinuousWin = false;
            index2++;
            InitFlyPlayProp2(index2);
            yield return new WaitForSeconds(1.5f);
        }
        if (Singleton<UserData>.Instance.PlaySkillUse3)
        {
            MissionCheckMgr.Instance.CheckMission(MissionType.UseProp, 1);
            Singleton<UserData>.Instance.PlaySkillUse3 = false;
            Singleton<UserData>.Instance.DelProps(2);
            index2++;
            InitFlyPlayProp3(index2);
            yield return new WaitForSeconds(1.1f);
        }
        if (Singleton<UserData>.Instance.PlaySkillUseViedo)
        {
            Singleton<UserData>.Instance.PlaySkillUseViedo = false;
            index2++;
            InitFlyPlayProp4(index2);
            yield return new WaitForSeconds(1.1f);
        }
        if (GameGuid.Instance != null)
        {
            GameGuid.Instance.InitGuid();
        }
    }

    public void InitFlyPlayPropMagicPower()
    {
        playPropMagicPower.SetActive(value: true);
        playPropMagicPower.GetComponent<Image>().DOFade(0f, 0.5f).SetEase(Ease.OutSine)
            .SetDelay(1f);
        PlayPropAnim(playPropMagicPower.transform);
        GameObject gameObject = GameObjectManager.CreateGameObject("FX/Prop/fx_prop1");
        gameObject.transform.SetParent(playProp1.transform.parent, worldPositionStays: false);
        gameObject.transform.position = playProp1.transform.position;
        UnityEngine.Object.Destroy(gameObject, 1.5f);
        Timer.DelayCallBack(1f, delegate
        {
            AddFullFZ();
            playPropMagicPower.SetActive(value: false);
        });
    }

    public void InitFlyPlayProp1()
    {
        playProp1.SetActive(value: true);
        playProp1.GetComponent<Image>().DOFade(0f, 0.5f).SetEase(Ease.OutSine)
            .SetDelay(1f);
        PlayPropAnim(playProp1.transform);
        GameObject gameObject = GameObjectManager.CreateGameObject("FX/Prop/fx_prop1");
        gameObject.transform.SetParent(playProp1.transform.parent, worldPositionStays: false);
        gameObject.transform.position = playProp1.transform.position;
        UnityEngine.Object.Destroy(gameObject, 1.5f);
        Timer.DelayCallBack(1f, delegate
        {
            playProp1.SetActive(value: false);
        });
        GameManager.Instance.useLongLine = true;
    }

    public void InitFlyPlayProp2(int index)
    {
        playProp2.SetActive(value: true);
        playProp2.GetComponent<Image>().DOFade(0f, 0.5f).SetEase(Ease.OutSine)
            .SetDelay(1f);
        PlayPropAnim(playProp2.transform);
        GameObject gameObject = GameObjectManager.CreateGameObject("FX/Prop/fx_prop2");
        gameObject.transform.SetParent(playProp1.transform.parent, worldPositionStays: false);
        gameObject.transform.position = playProp1.transform.position;
        UnityEngine.Object.Destroy(gameObject, 2f);
        gameObject.transform.DOMove(ReadyBubbleController.Instance.Ready3Parent.transform.position, 0.6f).SetDelay(0.8f).OnComplete(delegate
        {
            ReadyBubbleController.Instance.InitReadyBubble3(string.Empty);
        });
        Timer.DelayCallBack(1f, delegate
        {
            playProp2.SetActive(value: false);
        });
    }

    public void InitFlyPlayProp3(int index)
    {
        playProp3.SetActive(value: true);
        playProp3.GetComponent<Image>().DOFade(0f, 0.5f).SetEase(Ease.OutSine)
            .SetDelay(1f);
        PlayPropAnim(playProp3.transform);
        GameObject gameObject = GameObjectManager.CreateGameObject("FX/Prop/fx_prop1");
        gameObject.transform.SetParent(playProp1.transform.parent, worldPositionStays: false);
        gameObject.transform.position = playProp1.transform.position;
        UnityEngine.Object.Destroy(gameObject, 3f);
        gameObject.transform.DOMove(moveText.transform.position, 1f).SetDelay(1.17f).OnComplete(delegate
        {
            OperationMove(7, isAdd: true, 2);
        });
        Timer.DelayCallBack(1f, delegate
        {
            playProp3.SetActive(value: false);
        });
    }

    public void InitFlyPlayProp4(int index)
    {
        playProp4.SetActive(value: true);
        playProp4.GetComponent<Image>().DOFade(0f, 0.5f).SetEase(Ease.OutSine)
            .SetDelay(1f);
        PlayPropAnim(playProp4.transform);
        GameObject gameObject = GameObjectManager.CreateGameObject("FX/Prop/fx_prop1");
        gameObject.transform.SetParent(playProp1.transform.parent, worldPositionStays: false);
        gameObject.transform.position = playProp1.transform.position;
        UnityEngine.Object.Destroy(gameObject, 3f);
        Timer.DelayCallBack(1f, delegate
        {
            OperationMove(7, isAdd: true, 2);
            AddFullFZ();
            playProp4.SetActive(value: false);
        });
    }

    private void PlayPropAnim(Transform trs)
    {
        Sequence s = DOTween.Sequence();
        s.Append(trs.DOScale(new Vector3(0f, 0f, 0f), 0f)).Append(trs.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.12f).SetEase(Ease.OutSine)).Append(trs.DOScale(new Vector3(1f, 1f, 1f), 0.12f).SetEase(Ease.InSine))
            .Append(trs.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.12f).SetEase(Ease.OutSine).SetDelay(0.5f))
            .Append(trs.DOScale(new Vector3(2f, 2f, 2f), 0.12f).SetEase(Ease.InSine));
    }

    public void AddFullFZ()
    {
        List<FZDownBorder> list = new List<FZDownBorder>();
        if (FZ1.isUse && !FZ1.IsFull())
        {
            list.Add(FZ1);
        }
        if (FZ2.isUse && !FZ2.IsFull())
        {
            list.Add(FZ2);
        }
        if (FZ3.isUse && !FZ3.IsFull())
        {
            list.Add(FZ3);
        }
        if (FZ4.isUse && !FZ4.IsFull())
        {
            list.Add(FZ4);
        }
        if (list.Count != 0)
        {
            int num = UnityEngine.Random.Range(0, list.Count);
            if (num == list.Count)
            {
                num = 0;
            }
            list[num].FullFZ();
        }
    }

    private void TargetMoveEnd(PoolObject obj)
    {
        MissionCheckMgr.Instance.CheckMission(MissionType.CollectEnergy, 1);
        iNowMoveTargetNum--;
        targetText.text = iHarvestNum - iNowMoveTargetNum + "/" + Singleton<LevelManager>.Instance.iLevelCount;
        MubiaoIconAni(targetImg.gameObject);
        UnityEngine.Object.Destroy(obj.gameObject);
        Singleton<EffManager>.Instance.PlayEffByUI(targetImg.gameObject, "FX/GameScene/elf_star");
        AudioPlayManager.PlaySFX2D("ui_collect");
    }

    public void MubiaoIconAni(GameObject obj)
    {
        obj.transform.DOScale(new Vector2(0.640000045f, 0.640000045f), 0f).SetEase(Ease.InOutSine);
        Sequence s = DOTween.Sequence();
        s.Append(obj.transform.DOScale(new Vector2(0.8f, 0.8f), 0.13f).SetEase(Ease.InOutSine)).Append(obj.transform.DOScale(new Vector2(0.2f, 0.2f), 0.18f).SetEase(Ease.OutSine)).Append(obj.transform.DOScale(new Vector2(0.8f, 0.8f), 0.18f).SetEase(Ease.OutSine));
    }

    public void ShootBubble(bool skill1, bool skill2, bool skill3, bool skill4, bool prop1, bool prop2, bool prop3)
    {
        if (GameManager.Instance.iSkill4Count == 1 || GameManager.Instance.iSkill4Count == 2)
        {
            ReadyBubbleController.Instance.CreateNewReadyBubbleSkill(skill1, skill2, skill3, skill4);
            return;
        }
        if (!prop1 && !prop2 && !prop3)
        {
            OperationMove(1, isAdd: false);
        }
        if (prop1 || prop2 || prop3)
        {
            MissionCheckMgr.Instance.CheckMission(MissionType.UseProp, 1);
        }
        MissionCheckMgr.Instance.CheckMission(MissionType.ShootBubble, 1);
        moveText.text = MoveNum.ToString();
        ReadyBubbleController.Instance.CreateNewReadyBubble();
    }

    private bool CheckUseProp()
    {
        if (ReadyBubbleController.Instance.ReadyBubble1 == null)
        {
            return false;
        }
        if (ReadyBubbleController.Instance.ReadyBubble1.GetComponent<BubbleObj>().GetUseSkill())
        {
            return false;
        }
        if (ReadyBubbleController.Instance.ReadyBubble1.GetComponent<BubbleObj>().GetUseProp())
        {
            return false;
        }
        return true;
    }

    public bool CanUseProp(int index)
    {
        int intRecord = RecordManager.GetIntRecord("LevelData", "SkillOpen_" + index, 0);
        int intRecord2 = RecordManager.GetIntRecord("LevelData", "SkillOpenNum_" + index, 0);
        if (intRecord == 0)
        {
            return false;
        }
        if (intRecord2 <= 0)
        {
            Singleton<UserData>.Instance.buySkillType = index;
            UIManager.OpenUIWindow<BuySkillWindow>();
            return false;
        }
        RecordManager.SaveRecord("LevelData", "SkillOpenNum_" + index, intRecord2 - 1);
        return true;
    }

    public void UseProps1()
    {
        if (!(GameGuid.Instance != null) || GameGuid.Instance.GetUseProp1())
        {
            if (GameGuid.Instance != null)
            {
                GameGuid.Instance.UseProp();
            }
            if (Singleton<LevelManager>.Instance.GetNowLevel() == 18)
            {
                ReadyBubbleController.Instance.ChangeToProp1();
            }
            else if (CheckUseProp() && CanUseProp(3))
            {
                GlobalEvent.DispatchEvent(GameEventEnum.UIFlushChane);
                ReadyBubbleController.Instance.ChangeToProp1();
            }
        }
    }

    public void TestGameWin()
    {
        iHarvestNum = Singleton<LevelManager>.Instance.iLevelCount;
    }

    public void TestNext()
    {
        Singleton<LevelManager>.Instance.SetNowSelectLevel(Singleton<LevelManager>.Instance.GetNowSelectLevel() + 1);
        GameEntry.ChangeScene(GameEntry.GameScene);
    }

    public void UseProps2()
    {
        if (!(GameGuid.Instance != null) || GameGuid.Instance.GetUseProp2())
        {
            if (GameGuid.Instance != null)
            {
                GameGuid.Instance.UseProp();
            }
            if (Singleton<LevelManager>.Instance.GetNowLevel() == 19)
            {
                ReadyBubbleController.Instance.ChangeToProp2();
            }
            else if (CheckUseProp() && CanUseProp(4))
            {
                GlobalEvent.DispatchEvent(GameEventEnum.UIFlushChane);
                ReadyBubbleController.Instance.ChangeToProp2();
            }
        }
    }

    public void UseProps3()
    {
        if (!(GameGuid.Instance != null) || GameGuid.Instance.GetUseProp3())
        {
            if (GameGuid.Instance != null)
            {
                GameGuid.Instance.UseProp();
            }
            if (Singleton<LevelManager>.Instance.GetNowLevel() == 20)
            {
                ReadyBubbleController.Instance.ChangeToProp3();
            }
            else if (CheckUseProp() && CanUseProp(5))
            {
                GlobalEvent.DispatchEvent(GameEventEnum.UIFlushChane);
                ReadyBubbleController.Instance.ChangeToProp3();
            }
        }
    }

    public bool GetGameWin()
    {
        if (iHarvestNum == Singleton<LevelManager>.Instance.iLevelCount)
        {
            return true;
        }
        return false;
    }

    public void PlayGameLose()
    {
        UnityEngine.Debug.Log("   Failure requires purchase steps   ");
        UIManager.OpenUIWindow<BuyBubbleMoveNumWindow>();
    }

    public void BuyMoveCallBack(int MoveNum)
    {
        OperationMove(MoveNum, isAdd: true);
        BuyBubbleAnim();
        ReadyBubbleController.Instance.BuyMove();
    }

    private void BuyBubbleAnim()
    {
        SwitchoverElfAni("worry_to_static", bLoop: false);
    }

    public void PlayGameWin()
    {
        if (iHarvestNum - iNowMoveTargetNum >= Singleton<LevelManager>.Instance.iLevelCount && !isPlayGameWin)
        {
            isPlayGameWin = true;
            GameWinFallBall();
        }
    }

    public void GameWinFallBall()
    {
        UnityEngine.Debug.Log("    Victory drop   ");
        AudioPlayManager.PlaySFX2D("FireworkBurst");
        SwitchoverElfAni("happy", bLoop: true);
        List<BubbleObj> bubbleArray = GameManager.Instance.GetBubbleArray();
        for (int num = bubbleArray.Count - 1; num >= 0; num--)
        {
            bubbleObj = bubbleArray[num];
            if (bubbleObj.GetBubbleState() == BubbleState.None)
            {
                bubbleObj.RemoveBubble(RemoveType.BeRmove);
            }
        }
        StartCoroutine(GameWinShoot());
    }

    private IEnumerator GameWinShoot()
    {
        ReadyBubbleController.Instance.RemoveReadyBubble();
        int index = 0;
        AndroidManager.Instance.UpLevelData("win");
        while (MoveNum > 0)
        {
            MoveNum--;
            moveText.text = MoveNum.ToString();
            BubbleData _data = new BubbleData
            {
                key = GameManager.Instance.GetRandomKey(),
                isReadyBubble = true
            };
            ObjTmp = GameObjectManager.GetPoolObject("Obj/GameObj/Bubble");
            ObjTmp.transform.SetParent(ReadyBubbleController.Instance.Ready1Parent.transform.parent, worldPositionStays: false);
            ObjTmp.transform.localPosition = ReadyBubbleController.Instance.Ready1Parent.transform.localPosition;
            bubbleObj = ObjTmp.GetComponent<BubbleObj>();
            bubbleObj.Init(_data);
            if (iiOverShootBubbleCountIndex >= 5)
            {
                biiOverShootBubbleCountIndexstate = false;
            }
            if (iiOverShootBubbleCountIndex <= -5)
            {
                biiOverShootBubbleCountIndexstate = true;
            }
            index++;
            if (biiOverShootBubbleCountIndexstate)
            {
                bubbleObj.GameOverShootBubble(iiOverShootBubbleCountIndex++, index);
                iiOverShootBubbleCountIndex++;
            }
            else
            {
                bubbleObj.GameOverShootBubble(iiOverShootBubbleCountIndex--, index);
                iiOverShootBubbleCountIndex--;
            }
            yield return new WaitForSeconds(0.1f);
        }
        while (GameManager.Instance.FallParent.transform.childCount > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        while (GameManager.Instance.RemoveParent.transform.childCount > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.8f);
        GameOverMove1.transform.DOLocalMoveY(-16f, 0.8f);
        GameOverMove2.transform.DOLocalMoveY(-16f, 0.8f);
        // Transform transform = GameOverMove3.transform;
        // Vector3 localPosition = GameOverMove3.transform.localPosition;
        // transform.DOLocalMoveX(localPosition.x - 8f, 0.0001f);
        // GameOverMove3.transform.DOLocalRotate(new Vector3(0f, 180f, 0f), 1E-05f);
        // Transform transform2 = GameOverMove3.transform;
        // Vector3 localPosition2 = GameOverMove3.transform.localPosition;
        // transform2.DOLocalMoveY(localPosition2.y + 15f, 0.1f);
        GameOverMoveUITop.DOAnchorPos(new Vector2(0f, 100f), 0.8f);
        GameOverMoveUIDown.DOAnchorPos(new Vector2(0f, -60f), 0.8f);
        LosePanel.SetActive(value: false);
        WinPanel.SetActive(value: true);
        gameEndBg.SetActive(true);
        AndroidManager.Instance.ShowInterstitialAd();
        WinPanel.GetComponent<WinPanelUI>().OpenYanhua();
        GameGuid.Instance.Bombjiantou.SetActive(value: false);
        GameGuid.Instance.Arrowjiantou.SetActive(value: false);
        GameGuid.Instance.CaiQiujiantou.SetActive(value: false);
        SaveUserData();
        if (Singleton<LevelManager>.Instance.GetNowLevel() >= 14 && ContinuousItemArranger.gSequenceCount != 3 && !Singleton<UserData>.Instance.GetWinByLevel(Singleton<LevelManager>.Instance.GetNowLevel()))
        {
            ContinuousItemArranger.gSequenceCount++;
            UIManager.OpenUIWindow<ContinousWinWindow>();
            RecordManager.SaveRecord("LevelData", "SequenceWinCount", ContinuousItemArranger.gSequenceCount);
        }
        else
        {
            WinPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 0f), 0.8f).OnComplete(delegate
            {
                WinPanel.GetComponent<WinPanelUI>().InitUI();
            });
        }
    }

    public void SaveUserData()
    {
        Singleton<UserData>.Instance.AddLoveCount(1);
        BeforeStar = 0;
        int intRecord = RecordManager.GetIntRecord("LevelData", "LevelStar_" + Singleton<LevelManager>.Instance.GetNowLevel(), 0);
        RecordManager.SaveRecord("LevelData", "LevelScore_" + Singleton<LevelManager>.Instance.GetNowLevel(), gameScore);
        BeforeStar = intRecord;
        if (gameStar >= intRecord)
        {
            RecordManager.SaveRecord("LevelData", "LevelStar_" + Singleton<LevelManager>.Instance.GetNowLevel(), gameStar);
        }
        if (Singleton<LevelManager>.Instance.GetNowLevel() <= MapData.iMaxLevelID)
        {
            if (Singleton<LevelManager>.Instance.GetNowLevel() > Singleton<UserData>.Instance.GetPassLevel())
            {
                Singleton<UserData>.Instance.AddPassLevel();
                int intRecord2 = RecordManager.GetIntRecord("UserData", "ChestCount", 0);
                intRecord2++;
                RecordManager.SaveRecord("UserData", "ChestCount", intRecord2);
            }
            Singleton<LevelManager>.Instance.SetNowSelectLevel(Singleton<LevelManager>.Instance.GetNowSelectLevel() + 1);
            Singleton<UserData>.Instance.OpenProps(Singleton<UserData>.Instance.GetPassLevel());
            Singleton<MapData>.Instance.GoNextMap();
        }
    }

    public void GameLoseUI()
    {
        // try
        // {
        //     AGame.MySdkManager.me.myLog.LogEvent("lose_level_" + Singleton<LevelManager>.Instance.GetNowLevel(), "", "");
        // }
        // catch { }

        GameOverMove1.transform.DOLocalMoveY(-16f, 0.8f);
        GameOverMove2.transform.DOLocalMoveY(-16f, 0.8f);
        GameOverMove2.SetActive(false);
        // Transform transform = GameOverMove3.transform;
        // Vector3 localPosition = GameOverMove3.transform.localPosition;
        // transform.DOLocalMoveX(localPosition.x - 8f, 0.0001f);
        // GameOverMove3.transform.DOLocalRotate(new Vector3(0f, 180f, 0f), 1E-05f);
        // Transform transform2 = GameOverMove3.transform;
        // Vector3 localPosition2 = GameOverMove3.transform.localPosition;
        // transform2.DOLocalMoveY(localPosition2.y + 15f, 0.1f);
        GameOverMoveUITop.DOAnchorPos(new Vector2(0f, 100f), 0.8f);
        GameOverMoveUIDown.DOAnchorPos(new Vector2(0f, -60f), 0.8f);
        AndroidManager.Instance.ShowInterstitialAd();
        GameGuid.Instance.Bombjiantou.SetActive(value: false);
        GameGuid.Instance.Arrowjiantou.SetActive(value: false);
        GameGuid.Instance.CaiQiujiantou.SetActive(value: false);
        AndroidManager.Instance.UpLevelData("fail");
        WinPanel.SetActive(value: false);
        gameEndBg.SetActive(true);
        LosePanel.SetActive(value: true);
        if (Singleton<LevelManager>.Instance.GetNowLevel() > Singleton<UserData>.Instance.GetPassLevel())
        {
            ContinuousItemArranger.gSequenceCount = 0;
            RecordManager.SaveRecord("LevelData", "SequenceWinCount", ContinuousItemArranger.gSequenceCount);
        }
        LosePanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 0f), 0.8f).OnComplete(delegate
        {
            LosePanel.GetComponent<LoseUI>().InitUI();
        });
    }

    public void MoveEnd()
    {
        if (!initEnd)
        {
            MoveDown();
        }
        else
        {
            Singleton<RemoveControl>.Instance.MoveEnd();
        }
    }

    private void DownEnd()
    {
        ReadyBubbleController.Instance.InitReadyBubble();
        UIManager.OpenUIWindow<ReadyGoUIWindow>();
        initEnd = true;
    }

    public void MoveDown()
    {
        Vector2 a = DownObject.transform.position;
        Vector3 position = DownObject.transform.position;
        float x = position.x;
        float y = 0f - orthographicSize;
        Vector3 position2 = DownObject.transform.position;
        float num = Vector2.Distance(a, new Vector3(x, y, position2.z));
        DownObject.transform.DOLocalMove(new Vector3(0f, 0f - orthographicSize, 0f), num / 60f).OnComplete(DownEnd);
    }

    public void CameraShake()
    {
        CameraShake component = Camera.main.GetComponent<CameraShake>();
        component.numberOfShakes = 2;
        component.shakeAmount = new Vector3(0.3f, 0.6f, 0f);
        component.rotationAmount = new Vector3(0f, 0f, 0f);
        component.distance = 0.2f;
        component.speed = 55f;
        component.decay = 0.6f;
        component.Shake();
    }

    public void Operation(BubbleObj obj, bool isAddMove)
    {
        ShowHSBJ();
        CameraShake();
        GameObject ObjPoolFX;
        if (isAddMove)
        {
            ObjPoolFX = GameObjectManager.CreateGameObject("FX/GameScene/+3qiu_tuowei");
        }
        else
        {
            ObjPoolFX = GameObjectManager.CreateGameObject("FX/GameScene/-3qiu_tuowei");
        }
        ObjPoolFX.transform.position = obj.transform.position;
        ObjPoolFX.transform.parent = moveText.transform.parent;
        Vector3 localPosition = ObjPoolFX.transform.localPosition;
        localPosition.z = 0f;
        Vector3 localPosition2 = moveText.transform.localPosition;
        localPosition2.z = 0f;
        iNowMoveTextNum++;
        float duration = Vector2.Distance(localPosition, localPosition2) / 800f;
        Vector3 centerPost = GameManager.Instance.GetCenterPost(localPosition, localPosition2, 3f);
        Vector3[] path = new Vector3[3]
        {
            localPosition,
            centerPost,
            localPosition2
        };
        ObjPoolFX.transform.DOLocalPath(path, duration, PathType.CatmullRom, PathMode.TopDown2D).SetEase(Ease.Unset).OnComplete(delegate
        {
            iNowMoveTextNum--;
            UnityEngine.Object.Destroy(ObjPoolFX);
            OperationMove(3, isAddMove);
            AudioPlayManager.PlaySFX2D("ui_life_fly");
        });
    }

    private void OperationMove(int move, bool isAdd, int type = 1)
    {
        if (isAdd)
        {
            if (MoveNum < 5)
            {
                SwitchoverElfAni("worry_to_static", bLoop: false);
            }
            if (MoveNum < 3 && move == 3)
            {
                MoveNum += move;
                Singleton<LevelManager>.Instance.AllBubbleCount += move;
                ReadyBubbleController.Instance.BuyMove();
            }
            else
            {
                MoveNum += move;
                Singleton<LevelManager>.Instance.AllBubbleCount += move;
            }
        }
        else
        {
            MoveNum -= move;
        }
        if (move > 1)
        {
            Singleton<EffManager>.Instance.PlayEffByUI(moveText.gameObject, "FX/GameScene/elf_star");
            MoveAni(moveText.gameObject);
        }
        if (MoveNum < 0)
        {
            MoveNum = 0;
        }
        if (MoveNum == 10)
        {
            AudioPlayManager.PlaySFX2D("b_remain_10_bubbles");
            ShowOnly10();
        }
        if (MoveNum == 0)
        {
            StartCoroutine(CheckGameLose());
        }
        if (type == 1)
        {
            moveText.text = MoveNum.ToString();
            return;
        }
        moveText.text = "+" + move;
        Timer.DelayCallBack(0.8f, delegate
        {
            moveText.text = MoveNum.ToString();
            MoveAni(moveText.gameObject);
        });
    }

    public int GetScoreStar()
    {
        return gameStar;
    }

    public void AddScore(int score, BubbleObj bubble, int type = 1, Transform trs = null)
    {
        gameScore += score;
        scoreText.text = gameScore.ToString();
        if (gameScore < Singleton<LevelManager>.Instance.star1)
        {
            gameStar = 0;
            scoreProgress.fillAmount = (float)gameScore / (float)Singleton<LevelManager>.Instance.star1 * 0.25f;
        }
        else if (gameScore < Singleton<LevelManager>.Instance.star2)
        {
            gameStar = 1;
            scoreProgress.fillAmount = 0.25f + (float)(gameScore - Singleton<LevelManager>.Instance.star1) / (float)(Singleton<LevelManager>.Instance.star2 - Singleton<LevelManager>.Instance.star1) * 0.4f;
            FlyStar(1, startImg1.gameObject);
        }
        else if (gameScore < Singleton<LevelManager>.Instance.star3)
        {
            gameStar = 2;
            scoreProgress.fillAmount = 0.65f + (float)(gameScore - Singleton<LevelManager>.Instance.star2) / (float)(Singleton<LevelManager>.Instance.star3 - Singleton<LevelManager>.Instance.star2) * 0.35f;
            FlyStar(2, startImg2.gameObject);
        }
        else
        {
            gameStar = 3;
            scoreProgress.fillAmount = 1f;
            FlyStar(3, startImg3.gameObject);
        }
        PoolObject poolObject = GameObjectManager.GetPoolObject("GameScene/ScoreText");
        poolObject.transform.SetParent(scoreParent.transform, worldPositionStays: false);
        if (type == 1)
        {
            if (!(bubble != null) || !(GameManager.Instance.GetBox(bubble.point) != null))
            {
                GameObjectManager.DestroyPoolObject(poolObject);
                return;
            }
            poolObject.transform.position = GameManager.Instance.GetBox(bubble.point).transform.position;
        }
        else if (trs != null)
        {
            poolObject.transform.position = trs.position;
        }
        else
        {
            poolObject.transform.localPosition = Vector3.zero;
        }
        GameObjectManager.DestroyPoolObject(poolObject, 2f);
        poolObject.GetComponent<ScoreText>().InitScore(score, type);
    }

    public void FlyStar(int iStar, GameObject _StarObj)
    {
        if (CurrerStar < iStar)
        {
            CurrerStar = gameStar;
            GameObject _fx_ui_goalStarObj = UnityEngine.Object.Instantiate(fx_ui_goalStarObj);
            _fx_ui_goalStarObj.transform.SetParent(_StarObj.transform.parent.transform, worldPositionStays: false);
            _fx_ui_goalStarObj.transform.localPosition = new Vector3(0f, -500f, 0f);
            _fx_ui_goalStarObj.transform.localScale = new Vector2(150f, 150f);
            _fx_ui_goalStarObj.transform.DOScale(new Vector2(200f, 200f), 0.4f).SetEase(Ease.OutSine);
            _fx_ui_goalStarObj.transform.DOScale(new Vector2(150f, 150f), 0.3f).SetEase(Ease.OutSine).SetDelay(0.4f);
            _fx_ui_goalStarObj.transform.DOScale(new Vector2(200f, 200f), 0.2f).SetEase(Ease.OutSine).SetDelay(0.3f);
            Vector3 vector = _StarObj.transform.localPosition;
            float x = vector.x;
            float y = vector.y;
            Vector3 localPosition = _StarObj.transform.localPosition;
            vector = new Vector3(x, y, localPosition.z);
            Vector3 localPosition2 = _fx_ui_goalStarObj.transform.localPosition;
            float num = localPosition2.y - 20f;
            Vector3 localPosition3 = _fx_ui_goalStarObj.transform.localPosition;
            float num2 = localPosition3.x + 20f;
            float duration = 0.8f;
            float x2 = num2;
            float y2 = num;
            Vector3 localPosition4 = _fx_ui_goalStarObj.transform.localPosition;
            Vector3 vector2 = new Vector3(x2, y2, localPosition4.z);
            Vector3 centerPost = GameManager.Instance.GetCenterPost(vector2, vector, 9f);
            Vector3[] path = new Vector3[1]
            {
                vector2
            };
            Vector3[] waypoints = new Vector3[2]
            {
                centerPost,
                vector
            };
            _fx_ui_goalStarObj.transform.DOLocalPath(path, duration, PathType.CatmullRom, PathMode.TopDown2D).SetEase(Ease.OutSine).OnComplete(delegate
            {
                Fly2(waypoints, _StarObj, _fx_ui_goalStarObj);
            });
        }
    }

    public void Fly2(Vector3[] waypoints1, GameObject obj1, GameObject obj2)
    {
        AudioPlayManager.PlaySFX2D("ui_winstar1");
        obj2.transform.DOLocalPath(waypoints1, 0.7f, PathType.CatmullRom, PathMode.TopDown2D).SetEase(Ease.InSine).OnComplete(delegate
        {
            OpenStar(obj1, obj2);
        });
        obj2.transform.DORotate(new Vector3(0f, 0f, 360f), 0.6f);
    }

    private void OpenStar(GameObject _StarObj, GameObject fx_ui_goalStarObj)
    {
        AudioPlayManager.PlaySFX2D("ui_star");
        UnityEngine.Object.Destroy(fx_ui_goalStarObj);
        GameObject gameObject = UnityEngine.Object.Instantiate(fx_goalstar_lightObj);
        gameObject.transform.SetParent(_StarObj.transform.parent.transform, worldPositionStays: false);
        gameObject.transform.localPosition = _StarObj.transform.localPosition;
        _StarObj.GetComponent<Image>().enabled = true;
    }

    public IEnumerator CheckGameLose()
    {
        if (isCheckGameLose)
        {
            yield break;
        }
        isCheckGameLose = true;
        if (iHarvestNum == Singleton<LevelManager>.Instance.iLevelCount)
        {
            yield break;
        }
        while (GameManager.Instance.FallParent.transform.childCount > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        while (GameManager.Instance.RemoveParent.transform.childCount > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (MoveNum == 0)
        {
            while (iNowMoveTargetNum != 0)
            {
                yield return new WaitForSeconds(0.02f);
            }
            while (iNowMoveTextNum != 0)
            {
                yield return new WaitForSeconds(0.02f);
            }
            if (MoveNum != 0)
            {
                if (iHarvestNum + iNowMoveTargetNum == Singleton<LevelManager>.Instance.iLevelCount)
                {
                    yield break;
                }
                ReadyBubbleController.Instance.BuyMove();
            }
        }
        while (iNowMoveTargetNum != 0)
        {
            yield return new WaitForSeconds(0.02f);
        }
        while (iNowMoveTextNum != 0)
        {
            yield return new WaitForSeconds(0.02f);
        }
        bool isLock = true;
        while (isLock)
        {
            bool isFind = false;
            for (int i = 0; i < GameManager.Instance.GetBubbleArray().Count; i++)
            {
                if (GameManager.Instance.GetBubbleArray()[i].GetBubbleState() != 0)
                {
                    isFind = true;
                }
            }
            yield return new WaitForSeconds(0.02f);
            isLock = (isFind ? true : false);
        }
        Singleton<RemoveControl>.Instance.CheckFall(-1);
        if (MoveNum == 0)
        {
            if (iHarvestNum + iNowMoveTargetNum == Singleton<LevelManager>.Instance.iLevelCount)
            {
                yield break;
            }
            ReadyBubbleController.Instance.RemoveReadyBubble();
            PlayGameLose();
        }
        isCheckGameLose = false;
    }

    public void MoveAni(GameObject obj)
    {
        obj.transform.DOScale(new Vector2(1.6f, 1.6f), 0f).SetEase(Ease.InOutSine);
        Sequence s = DOTween.Sequence();
        s.Append(obj.transform.DOScale(new Vector2(2f, 2f), 0.13f).SetEase(Ease.InOutSine)).Append(obj.transform.DOScale(new Vector2(0.5f, 0.5f), 0.18f).SetEase(Ease.OutSine)).Append(obj.transform.DOScale(new Vector2(1f, 1f), 0.18f).SetEase(Ease.OutSine));
    }

    public void ShowHSBJ()
    {
        HSBJ.transform.GetComponent<SpriteRenderer>().DOFade(0.5f, 0f);
        HSBJ.SetActive(value: true);
        HSBJ.transform.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f).SetDelay(0.3f);
        Timer.DelayCallBack(0.8f, delegate
        {
            HSBJ.SetActive(value: false);
        });
    }

    public void Test1()
    {
        GameEntry.ChangeScene(GameEntry.LevelScene);
    }

    public void OpenPauseUI()
    {
        if (!(GameGuid.Instance != null) || GameGuid.Instance.GetUsePause())
        {
            UIManager.OpenUIWindow<PauseUIWindow>();
            AndroidManager.Instance.UpLevelData("pause");
        }
    }

    public void RemoveGuandian(BubbleObj bubble)
    {
        int maxRow = Singleton<CameraMove>.Instance.GetMaxRow();
        List<BubbleObj> list = new List<BubbleObj>();
        List<BubbleObj> bubbleArray = GameManager.Instance.GetBubbleArray();
        for (int i = 0; i < bubbleArray.Count; i++)
        {
            if (bubbleArray[i].point.X > maxRow - 11 && bubbleArray[i].GetBubbleType() <= 5 && bubbleArray[i].GetBubbleState() == BubbleState.None)
            {
                BubbleData topBubbleData = bubbleArray[i].GetTopBubbleData();
                if (topBubbleData.key == string.Empty)
                {
                    list.Add(bubbleArray[i]);
                }
            }
        }
        if (list.Count >= 2)
        {
            int num = UnityEngine.Random.Range(0, list.Count - 1);
            if (num > list.Count - 1)
            {
                num = 0;
            }
            GameObject obj = Singleton<EffManager>.Instance.PlayEff3(bubble.point, "FX/Skill/fx_dost_fly");
            obj.transform.DOLocalMove(list[num].transform.localPosition, 20f * GameManager.removeTime).OnComplete(delegate
            {
                UnityEngine.Object.Destroy(obj);
            });
            list[num].SetRemoveIndex(20);
            list[num].RemoveBubble(RemoveType.BeRmove);
            Singleton<RemoveControl>.Instance.CheckFall(25);
        }
    }

    private void Update()
    {
        if ((bool)GameManager.Instance && GameManager.Instance.isExitGame)
        {
            return;
        }
        if (GetGameWin())
        {
            PlayGameWin();
        }
        else
        {
            if (iNowMoveTextNum != 0 || MoveNum == 0)
            {
                return;
            }
            if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
            {
                if (EventSystem.current.currentSelectedGameObject != null)
                {
                    return;
                }
                IEnumerator enumerator = GameManager.Instance.RemoveParent.transform.GetEnumerator();
                try
                {
                    if (enumerator.MoveNext())
                    {
                        Transform transform = (Transform)enumerator.Current;
                        return;
                    }
                }
                finally
                {
                    IDisposable disposable;
                    if ((disposable = (enumerator as IDisposable)) != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
            if (!initEnd)
            {
                if (Input.GetMouseButton(0) && Singleton<CameraMove>.Instance.GetMapMoving())
                {
                    Singleton<CameraMove>.Instance.QuickMove();
                }
            }
            else
            {
                if (UIManager.GetNormalUICount() > 0)
                {
                    return;
                }
                if (Singleton<CameraMove>.Instance.GetMapMoving())
                {
                    Singleton<CameraMove>.Instance.QuickMove();
                }
                else
                {
                    if (GameManager.Instance.GetDelayGame() || Singleton<RemoveControl>.Instance.GetCheckRemove() || ReadyBubbleController.Instance.isReadyBubbleChange)
                    {
                        return;
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        GameObject gameObject = TouchChecker(UnityEngine.Input.mousePosition);
                        if ((bool)gameObject && gameObject.layer == LayerMask.NameToLayer("ChangeBubble"))
                        {
                            gameObject.GetComponent<ReadyBubbleController>().ChangeBubble();
                            GameControl.Instance.CancelFireBubble(2);
                            return;
                        }
                        gameObject = TouchChecker(UnityEngine.Input.mousePosition);
                        if ((bool)gameObject && gameObject.layer == LayerMask.NameToLayer("UseSkill"))
                        {
                            gameObject.transform.parent.GetComponent<FZDownBorder>().UseSkill();
                            return;
                        }
                    }
                    if (GameControl.Instance != null)
                    {
                        GameControl.Instance.OnUpdate();
                    }
                }
            }
        }
    }

    public GameObject TouchChecker(Vector3 mouseposition)
    {
        Vector3 vector = Camera.main.ScreenToWorldPoint(mouseposition);
        Vector2 point = new Vector2(vector.x, vector.y);
        if ((bool)Physics2D.OverlapPoint(point))
        {
            return Physics2D.OverlapPoint(point).gameObject;
        }
        return null;
    }

    public void ShowCombo(int combo)
    {
        if (combo >= 5)
        {
            comboText.GetComponent<Text>().text = combo.ToString();

            comboTrs.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-120f, 320f), 0.4f);
            comboTrs.GetComponent<RectTransform>().DOAnchorPos(new Vector2(300f, 320f), 0.4f).SetDelay(0.6f);
        }
    }

    public void ShowOnly10()
    {
        only10Trs.GetComponent<RectTransform>().DOAnchorPos(new Vector2(120f, -248f), 0.4f);
        only10Trs.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-300f, -248f), 0.4f).SetDelay(0.7f);
        only10Trs.transform.Find("Text").GetComponent<Text>().text = Util.ReplaceText(GameEntry.Instance.GetString("BubbleBuyTipText"));
    }

    public void GirlAni_lively1()
    {
        if (MoveNum <= 5)
        {
            SwitchoverElfAni("lively1_2", bLoop: false);
        }
        else
        {
            SwitchoverElfAni("lively1", bLoop: false);
        }
    }

    public void GirlAni_fault(bool b = false)
    {
        if (b)
        {
            if (MoveNum <= 5)
            {
                SwitchoverElfAni("fire2_to_worry", bLoop: false);
            }
            else
            {
                SwitchoverElfAni("fire_to_start", bLoop: false);
            }
            return;
        }
        iFaultCombo++;
        bool flag = false;
        int num = UnityEngine.Random.Range(1, 100);
        if (iFaultCombo == 1)
        {
            if (num < 10)
            {
                flag = true;
            }
        }
        else if (iFaultCombo == 2)
        {
            if (num < 30)
            {
                flag = true;
            }
        }
        else if (num < 50)
        {
            flag = true;
        }
        if (!flag)
        {
            if (MoveNum <= 5)
            {
                SwitchoverElfAni("fire2_to_worry", bLoop: false);
            }
            else
            {
                SwitchoverElfAni("fire_to_start", bLoop: false);
            }
        }
        else if (MoveNum <= 5)
        {
            StartCoroutine(IESwitchoverElfAni("fault2"));
        }
        else
        {
            StartCoroutine(IESwitchoverElfAni("fault"));
        }
    }

    private IEnumerator IESwitchoverElfAni(string SAniName)
    {
        yield return new WaitForSeconds(0.5f);
        SwitchoverElfAni(SAniName, bLoop: false);
    }

    public void SwitchoverElfAni(string sAniName, bool bLoop)
    {
        // if (sAniName == "to_static")
        // {
        //     if (GameManager.Instance.iCombo < 5 && Singleton<LevelManager>.Instance.InitBubbleCount >= 5)
        //     {
        //         skelet.state.AddAnimation(0, "start_to_static", loop: false, 0f);
        //         skelet.state.AddAnimation(0, "static", loop: true, 0f);
        //     }
        // }
        // else if (sAniName == "to_start")
        // {
        //     sAniName = ((Singleton<LevelManager>.Instance.InitBubbleCount >= 5) ? "fire_to_start" : "fire2_to_worry");
        // }
        // else
        // {
        //     if (skelet.AnimationName == "happy" || skelet.AnimationName == "cry" || (sAniName == "start_to_static" && (skelet.AnimationName == "worry" || skelet.AnimationName == "fire2" || skelet.AnimationName == "fire")))
        //     {
        //         return;
        //     }
        //     if (sAniName == "fire_to_start" && skelet.AnimationName == "fire")
        //     {
        //         skelet.state.AddAnimation(0, "fire_to_start", loop: false, 0f);
        //         if (GameManager.Instance.iCombo >= 5)
        //         {
        //             skelet.state.AddAnimation(0, "start", loop: false, 0f);
        //             skelet.state.AddAnimation(0, "combo", loop: true, 0f);
        //         }
        //         else
        //         {
        //             skelet.state.AddAnimation(0, "start", loop: true, 0f);
        //         }
        //         return;
        //     }
        //     if (sAniName == "fire2_to_worry" && skelet.AnimationName == "fire2")
        //     {
        //         skelet.state.AddAnimation(0, "fire2_to_worry", loop: false, 0f);
        //         skelet.state.AddAnimation(0, "worry", loop: true, 0f);
        //         return;
        //     }
        //     PlayGirlMp3(sAniName);
        //     skelet.loop = bLoop;
        //     skelet.state.SetAnimation(0, sAniName, bLoop);
        //     switch (sAniName)
        //     {
        //         case "fire_to_start":
        //         case "succeed":
        //         case "fault":
        //         case "lively1":
        //         case "lively2":
        //         case "change":
        //         case "ready_to_start":
        //             if (GameManager.Instance.iCombo >= 5)
        //             {
        //                 skelet.state.AddAnimation(0, "combo", loop: true, 0f);
        //             }
        //             else
        //             {
        //                 skelet.state.AddAnimation(0, "start", loop: true, 0f);
        //             }
        //             break;
        //     }
        //     if (sAniName == "fire2_to_worry" || sAniName == "succeed2" || sAniName == "fault2" || sAniName == "lively1_2" || sAniName == "lively2_2" || sAniName == "change2" || sAniName == "ready2_to_worry")
        //     {
        //         skelet.state.AddAnimation(0, "worry", loop: true, 0f);
        //     }
        //     if (sAniName == "start_to_static")
        //     {
        //         skelet.state.AddAnimation(0, "static", loop: true, 0f);
        //     }
        // }
    }

    public void PlayGirlMp3(string sAniname)
    {
        if (sAniname == "happy")
        {
            AudioPlayManager.PlaySFX2D("role_girl_win", nowPlay: true);
        }
        else if (sAniname == "wait")
        {
            AudioPlayManager.PlaySFX2D("role_girl_wait");
        }
        else if (sAniname == "cry")
        {
            AudioPlayManager.PlaySFX2D("role_girl_cry");
        }
        else if (!(sAniname == "worry"))
        {
            if (sAniname == "win")
            {
                AudioPlayManager.PlaySFX2D("role_girl_win", nowPlay: true);
            }
            else if ("fault2" == sAniname || "fault" == sAniname)
            {
                AudioPlayManager.PlaySFX2D("vo_role_despond");
            }
            else if ("fire" == sAniname || "fire2" == sAniname)
            {
                vo_role_shoot();
            }
        }
    }

    public void vo_role_shoot()
    {
        ivo_role_shoot++;
        if (ivo_role_shoot == 1)
        {
            int num = UnityEngine.Random.Range(1, 100);
            if (num < 80)
            {
                num = UnityEngine.Random.Range(1, 5);
                AudioPlayManager.PlaySFX2D("vo_role_shoot" + num);
            }
        }
        else if (ivo_role_shoot == 2)
        {
            int num2 = UnityEngine.Random.Range(1, 100);
            if (num2 < 30)
            {
                num2 = UnityEngine.Random.Range(1, 5);
                AudioPlayManager.PlaySFX2D("vo_role_shoot" + num2);
            }
        }
        else
        {
            if (ivo_role_shoot == 3)
            {
                return;
            }
            if (ivo_role_shoot == 4)
            {
                int num3 = UnityEngine.Random.Range(1, 100);
                if (num3 < 50)
                {
                    num3 = UnityEngine.Random.Range(1, 5);
                    AudioPlayManager.PlaySFX2D("vo_role_shoot" + num3);
                }
            }
            else if (ivo_role_shoot == 5)
            {
                int num4 = UnityEngine.Random.Range(1, 100);
                if (num4 < 50)
                {
                    num4 = UnityEngine.Random.Range(1, 5);
                    AudioPlayManager.PlaySFX2D("vo_role_shoot" + num4);
                }
            }
            else if (ivo_role_shoot == 6)
            {
                ivo_role_shoot = 0;
            }
        }
    }
}
