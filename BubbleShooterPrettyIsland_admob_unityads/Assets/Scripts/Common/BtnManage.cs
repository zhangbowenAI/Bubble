
using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnManage : MonoBehaviour
{
    public GameObject LevelBtn;

    public GameObject LevelBtnMin;

    #region v2
    private int levelFrom = 0;
    private int levelTo = 0;
    private int currentLevel = 0;

    public List<GameObject> lstBtnNode;
    public GameObject btnCurrentNode;
    public Button btnPrevSection;
    public Button btnNextSection;
    #endregion



    private Vector2[] BtnPos;

    public GameObject[] LBtnObj;

    public GameObject[,] LBtnObjMin;

    private int iMapIndex;

    public static bool bClickTimeDown;

    public static BtnManage Instance;

    public GameObject Girl;

    public GameObject GirlImage;

    private int iGirlNextID;

    private void Start()
    {
        Debug.Log("======= GetNowLevel():" + Singleton<LevelManager>.Instance.GetNowLevel());
        Instance = this;
        Singleton<UserData>.Instance.bGrilMoveing = false;
        Singleton<UserData>.Instance.LoadNowPassLevelNumber();
        if (Singleton<MapData>.Instance.bFirstInMap)
        {
            iMapIndex = Singleton<MapData>.Instance.iNowMapID;
        }
        else
        {
            int nowLevel = Singleton<LevelManager>.Instance.GetNowLevel();
            nowLevel++;
            if (nowLevel >= MapData.iMaxLevelID)
            {
                nowLevel = MapData.iMaxLevelID;
            }
            int mapForLevelID = Singleton<MapData>.Instance.GetMapForLevelID(nowLevel);
            iMapIndex = mapForLevelID - 1;
        }
        InitLevelInfo();
        
    }

    void PlayLevel(int level)
    {
        AudioPlayManager.PlaySFX2D("button");
        Singleton<LevelManager>.Instance.SetNowSelectLevel(level);
        UIManager.OpenUIWindow<PlayWindow>();
    }

    void InitLevelInfo()
    {
        int iPassLevel = Singleton<UserData>.Instance.GetPassLevel();
        int iNextLevel = iPassLevel + 1;

        Debug.Log("====== iNextLevel:" + iNextLevel);
        btnCurrentNode.transform.Find("Text").GetComponent<Text>().text = iNextLevel.ToString();
        btnCurrentNode.GetComponent<Button>().onClick.RemoveAllListeners();
        btnCurrentNode.GetComponent<Button>().onClick.AddListener(() => {
            PlayLevel(iNextLevel);
        });
    }

    private void Update()
    {
       
        if (Girl != null)
        {
            SkeletonAnimation component = Girl.GetComponent<SkeletonAnimation>();
            if (GirlImage != null)
            {
                GirlImage.transform.localPosition = new Vector3(component.skeleton.FindBone("play_icon").worldX, component.skeleton.FindBone("play_icon").worldY, 0f);
            }
        }
    }
}
