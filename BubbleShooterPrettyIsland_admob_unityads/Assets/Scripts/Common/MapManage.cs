
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManage : MonoBehaviour
{
    public static MapManage Instance;

    public int iMapIndex = 0;

    #region v2
    private int levelFrom = 0;
    private int levelTo = 0;
    private int currentLevelPage = 1;
    private int totalPage = 1;

    public List<GameObject> lstBtnNode;
    public GameObject btnCurrentNode;
    public Button btnPrevPage;
    public Text txtPage;
    public GameObject goMapNodeGroup;

    public Button btnNextPage;
    public Sprite srBtnlocked;
    public Sprite srBtnUnlocked;
    public Sprite srBtnSelect;
    public Sprite srStarGray;
    public Sprite srStarYellow;
    public GameObject TipText;
    public Image MapImage;
    int pageSize
    {
        get
        {
            return lstBtnNode.Count;
        }
    }

    #endregion

    private void Awake()
    {
        Instance = this;
    }

    public void iStart()
    {

        Singleton<UserData>.Instance.LoadNowPassLevelNumber();

        iMapIndex = CalculateCurrentMapIndex();

        InitLevelInfo();

        GoMap(iMapIndex, binit: true);


    }

    #region v2

    int CalculateCurrentMapIndex()
    {
        int mapIdx;

        int iNextLevel = Singleton<UserData>.Instance.GetPassLevel() + 1;

        int mapForLevelID = Singleton<MapData>.Instance.GetMapForLevelID(iNextLevel);
        mapIdx = mapForLevelID - 1;

        return mapIdx;
    }

    void PlayLevel(int level)
    {
        AudioPlayManager.PlaySFX2D("button");
        Singleton<LevelManager>.Instance.SetNowSelectLevel(level);
        UIManager.OpenUIWindow<PlayWindow>();
    }

    void InitLevelInfo()
    {

        InitCurrentNode();

        btnPrevPage.onClick.RemoveAllListeners();
        btnPrevPage.onClick.AddListener(PreviousPage);

        btnNextPage.onClick.RemoveAllListeners();
        btnNextPage.onClick.AddListener(NextPage);
    }
    
    void InitMapNode()
    {
        Debug.Log("==== [iMapIndex:" + iMapIndex + "]");

        levelFrom = Singleton<MapData>.Instance.LMapEndBtnID[iMapIndex] + 1;
        levelTo = (Singleton<MapData>.Instance.LMapEndBtnID[iMapIndex] + Singleton<MapData>.Instance.LMapBtnCount[iMapIndex]);

        Debug.Log("levelFrom:" + levelFrom + "-levelTo:" + levelTo);

        totalPage = ((levelTo - levelFrom) + 1) / pageSize;

        GoToPage(1);
    }

    void GotoCurrentPlayingPage()
    {
        int iNextLevel = Singleton<UserData>.Instance.GetPassLevel() + 1;
        float fCurrentPage = (float)((iNextLevel - levelFrom) + 1) / (float)pageSize;

        int currentPage = Mathf.CeilToInt(fCurrentPage);
        Debug.Log("===> GotoCurrentPlayingPage [fCurrentPage:" + fCurrentPage + "] [currentPage:" + currentPage + "] [iNextLevel:" + iNextLevel + "] [levelFrom:" + levelFrom + "] [pageSize:" + pageSize + "]");

        GoToPage(currentPage);
    }

    void GoToPage(int page)
    {
        Debug.Log("==== GoToPage:" + page);
        currentLevelPage = page;

        for (int i = 0; i < lstBtnNode.Count; i++)
        {
            int iLevel = levelFrom + (currentLevelPage - 1) * pageSize + i;
            Button btn = lstBtnNode[i].transform.Find("node").GetComponent<Button>();

            btn.transform.Find("Text").GetComponent<Text>().text = iLevel.ToString();
            btn.onClick.RemoveAllListeners();

            int levelStar = Singleton<MapData>.Instance.GetLevelStar(iLevel);

            List<Image> lstStar = new List<Image>();
            lstStar.Add(lstBtnNode[i].transform.Find("star1").GetComponent<Image>());
            lstStar.Add(lstBtnNode[i].transform.Find("star2").GetComponent<Image>());
            lstStar.Add(lstBtnNode[i].transform.Find("star3").GetComponent<Image>());

            if (iLevel < Singleton<UserData>.Instance.GetPassLevel() + 1)
            {
                btn.GetComponent<Button>().enabled = true;
                btn.GetComponent<Image>().sprite = srBtnUnlocked;
                lstBtnNode[i].transform.localScale = Vector3.one;
                btn.transform.Find("Text").GetComponent<Text>().color = new Color32(217, 210, 148, 255);
                btn.onClick.AddListener(() =>
                {
                    PlayLevel(iLevel);
                });
            }
            else if (iLevel == Singleton<UserData>.Instance.GetPassLevel() + 1)
            {
                btn.GetComponent<Button>().enabled = true;
                btn.GetComponent<Image>().sprite = srBtnSelect;
                btn.onClick.AddListener(() =>
                {
                    PlayLevel(iLevel);
                });
                lstBtnNode[i].transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
                btn.transform.Find("Text").GetComponent<Text>().color = new Color32(249, 163, 252, 255);
            }
            else
            {
                btn.GetComponent<Image>().sprite = srBtnlocked;
                btn.GetComponent<Button>().enabled = false;
                lstBtnNode[i].transform.localScale = Vector3.one;
                btn.transform.Find("Text").GetComponent<Text>().color = new Color32(162, 226, 251, 255);
            }

            if (iLevel < Singleton<UserData>.Instance.GetPassLevel() + 1)
            {
                for (int iStar = 0; iStar < lstStar.Count; iStar++)
                {
                    lstStar[iStar].gameObject.SetActive(true);
                    if (iStar < levelStar)
                    {
                        lstStar[iStar].sprite = srStarYellow;
                    }
                    else
                    {
                        lstStar[iStar].sprite = srStarGray;
                    }
                }
            }
            else
            {
                for (int iStar = 0; iStar < lstStar.Count; iStar++)
                {
                    lstStar[iStar].gameObject.SetActive(false);
                }
            }

        }

        txtPage.text = currentLevelPage + "/" + totalPage;
    }

    void PreviousPage()
    {
        Debug.Log("==== PreviousPage");
        currentLevelPage--;
        if (currentLevelPage < 1) { currentLevelPage = 1; }
        GoToPage(currentLevelPage);
    }

    void NextPage()
    {
        Debug.Log("==== NextPage");
        currentLevelPage++;
        if (currentLevelPage > totalPage) { currentLevelPage = totalPage; }
        GoToPage(currentLevelPage);
    }

    void InitCurrentNode()
    {
        int iNextLevel = Singleton<UserData>.Instance.GetPassLevel() + 1;

        Debug.Log("====== InitCurrentNode => iNextLevel:" + iNextLevel);
        btnCurrentNode.transform.Find("Text").GetComponent<Text>().text = iNextLevel.ToString();
        btnCurrentNode.GetComponent<Button>().onClick.RemoveAllListeners();
        btnCurrentNode.GetComponent<Button>().onClick.AddListener(() =>
        {
            PlayLevel(iNextLevel);
        });
    }

    #endregion

    public void InitBG__Atlas()
    {
        string strAtlasName = "map_01";

        int mapId = iMapIndex + 1;
        strAtlasName = "map_0" + mapId;

        if (mapId > 9)
        {

            strAtlasName = "map_" + mapId;
        }

        MapImage.sprite = AtlasManager.Instance.GetSprite(AtlasType.MapAtlas, strAtlasName); // sprite;
    }

    public void GoMap(int iIndex, bool binit = false)
    {
        if (!binit)
        {
            LevelScene.Instance.ShowLodingUI();
        }
        iMapIndex = iIndex;
        InitBG__Atlas();

        int iNowMapID = Singleton<MapData>.Instance.iNowMapID;
        Debug.Log("iMapIndex:" + iMapIndex + "-iNowMapID:" + iNowMapID);
        if (iMapIndex > iNowMapID)
        {
            MapImage.color = new Color32(0, 0, 0, 255);
            btnNextPage.gameObject.SetActive(false);
            btnPrevPage.gameObject.SetActive(false);
            txtPage.gameObject.SetActive(false);
            goMapNodeGroup.SetActive(false);
            TipText.SetActive(true);
        }
        else
        {
            MapImage.color = new Color32(255, 255, 255, 255);
            btnNextPage.gameObject.SetActive(true);
            btnPrevPage.gameObject.SetActive(true);
            txtPage.gameObject.SetActive(true);
            goMapNodeGroup.SetActive(true);
            TipText.SetActive(false);
        }


        InitMapNode();

        if (iMapIndex == CalculateCurrentMapIndex())
        {
            GotoCurrentPlayingPage();
        }

        if (!binit)
        {
            LevelScene.Instance.HideLoadingUI();
        }
    }

    private IEnumerator IEOpenLevel()
    {
        yield return new WaitForSeconds(0.5f);
    }

}
