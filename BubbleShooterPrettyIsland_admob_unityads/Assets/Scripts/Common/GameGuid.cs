
using UnityEngine;
using UnityEngine.UI;

public class GameGuid : MonoBehaviour
{
    public static GameGuid Instance;

    public GameObject DownObject;

    public GameObject guidGameObj;

    public GameObject duihuaObj;

    public GameObject zhezhaoyuan1;

    public GameObject zhezhaoyuan2;

    public GameObject zhezhaoyuan3;

    public GameObject zhezhaoyuan4;

    public GameObject zhezhao;

    public GameObject finger;

    public GameObject gameobjskill1;

    public GameObject gameobjskill2;

    public GameObject gameobjskill3;

    public GameObject gameobjskill4;

    public GameObject gameobjprop1;

    public GameObject gameobjprop2;

    public GameObject gameobjprop3;

    public GameObject jiantouskill1;

    public GameObject jiantouskill2;

    public GameObject jiantouskill3;

    public GameObject jiantouskill4;

    public GameObject jiantouprop1;

    public GameObject jiantouprop2;

    public GameObject jiantouprop3;

    public GameObject jiantouup;

    public GameObject level3;

    public Text duihuaText;

    private bool useSkill1 = true;

    private bool useSkill2 = true;

    private bool useSkill3 = true;

    private bool useSkill4 = true;

    private bool useProp1 = true;

    private bool useProp2 = true;

    private bool useProp3 = true;

    private bool usePause = true;

    private bool shootBubble = true;

    private bool changeBubble = true;

    private bool skill1;

    private bool skill2;

    private bool skill3;

    private bool skill4;

    private int steps;

    private int level;

    private int index;

    public GameObject Bombjiantou;

    public GameObject Arrowjiantou;

    public GameObject CaiQiujiantou;

    private GameObject shouzhi;

    private void Awake()
    {
        Instance = this;
        guidGameObj.SetActive(value: false);
    }

    public string GetKey()
    {
        level = Singleton<LevelManager>.Instance.GetNowLevel();
        string result = string.Empty;
        switch (level)
        {
            case 1:
                if (index == 0)
                {
                    result = "C";
                }
                break;
            case 2:
                if (index == 0)
                {
                    result = "C";
                }
                if (index == 1)
                {
                    result = "A";
                }
                break;
            case 3:
                if (index == 0)
                {
                    result = "A";
                }
                break;
            case 4:
                if (index == 0)
                {
                    result = "A";
                }
                break;
            case 5:
                if (index == 0)
                {
                    result = "A";
                }
                break;
            case 8:
                if (index == 0)
                {
                    result = "A";
                }
                break;
            case 10:
                if (index == 0)
                {
                    result = "A";
                }
                break;
            case 11:
                if (index == 0)
                {
                    result = "A";
                }
                if (index == 1)
                {
                    result = "A";
                }
                break;
            case 12:
                if (index == 0)
                {
                    result = "C";
                }
                break;
            case 13:
                if (index == 0)
                {
                    result = "A";
                }
                if (index == 1)
                {
                    result = "D";
                }
                break;
            case 15:
                if (index == 0)
                {
                    result = "C";
                }
                break;
            case 17:
                if (index == 0)
                {
                    result = "C";
                }
                break;
            case 24:
                if (index == 0)
                {
                    result = "C";
                }
                break;
            case 25:
                if (index == 0)
                {
                    result = "C";
                }
                if (index == 2)
                {
                    result = "A";
                }
                break;
            case 41:
                if (index == 0)
                {
                    result = "A";
                }
                break;
        }
        index++;
        return result;
    }

    public void InitGame()
    {
        SetUseFalse();
    }

    public void InitGuid()
    {
        steps = 0;
        level = Singleton<LevelManager>.Instance.GetNowLevel();
        HideGuiObj();
        switch (level)
        {
            case 1:
                InitMaxGuid();
                break;
            case 2:
                InitLevel2();
                break;
            case 3:
                InitLevel3();
                break;
            case 4:
            case 11:
            case 21:
            case 31:
            case 41:
            case 51:
            case 61:
            case 71:
            case 81:
            case 91:
            case 101:
            case 111:
                InitMaxGuid();
                break;
            case 5:
            case 6:
            case 8:
            case 9:
            case 10:
            case 15:
            case 16:
            case 17:
            case 24:
            case 25:
                InitMinGuid();
                break;
            case 18:
                InitMaxGuid();
                break;
            case 19:
                InitMaxGuid();
                break;
            case 20:
                InitMaxGuid();
                break;
            default:
                SetUseTrue();
                break;
        }
    }

    private string GetString()
    {
        // if (GameEntry.buildType == GameEntry.BuildType.GoogleEN)
        // {
        // 	return Singleton<DataGuidEN>.Instance.GetContentByKeyAndType("level" + level, GuidENType.text2);
        // }
        return Singleton<DataGuidZH>.Instance.GetContentByKeyAndType("level" + level, GuidZHType.text2);
    }

    public void InitMaxGuid()
    {
        steps = 1;
        UIManager.OpenUIWindow<GuidUIWindow>();
    }

    public void Level18_1()
    {
        steps = 2;
        SetUseFalse();
        ShowGuidObj();
        useProp1 = true;
        jiantouprop1.SetActive(value: true);
        duihuaObj.SetActive(value: true);
        duihuaObj.transform.localPosition = new Vector3(0f, 8.4f, -5);
        zhezhao.SetActive(value: true);
        zhezhaoyuan1.SetActive(value: true);
        zhezhaoyuan1.transform.position = gameobjprop1.transform.position;
        duihuaText.text = GetString().ToString();
    }

    public void Level18_2()
    {
        SetUseTrue();
        HideGuiObj();
        Bombjiantou.SetActive(value: true);
        steps++;
    }

    public void Level19_1()
    {
        steps = 2;
        SetUseFalse();
        ShowGuidObj();
        useProp2 = true;
        jiantouprop2.SetActive(value: true);
        duihuaObj.SetActive(value: true);
        duihuaObj.transform.localPosition = new Vector3(0f, 8.4f, -5);
        zhezhao.SetActive(value: true);
        zhezhaoyuan2.SetActive(value: true);
        zhezhaoyuan2.transform.position = gameobjprop2.transform.position;
        duihuaText.text = GetString().ToString();
    }

    public void Level19_2()
    {
        SetUseTrue();
        HideGuiObj();
        Arrowjiantou.SetActive(value: true);
        steps++;
    }

    public void Level20_1()
    {
        steps = 2;
        SetUseFalse();
        ShowGuidObj();
        useProp3 = true;
        jiantouprop3.SetActive(value: true);
        duihuaObj.SetActive(value: true);
        duihuaObj.transform.localPosition = new Vector3(0f, 8.4f, -5);
        zhezhao.SetActive(value: true);
        zhezhaoyuan3.SetActive(value: true);
        zhezhaoyuan3.transform.position = gameobjprop3.transform.position;
        duihuaText.text = GetString().ToString();
    }

    public void Level20_2()
    {
        SetUseTrue();
        HideGuiObj();
        CaiQiujiantou.SetActive(value: true);
        steps++;
    }

    public void InitLevel2()
    {
        SetUseFalse();
        ShowGuidObj();
        changeBubble = true;
        zhezhao.SetActive(value: true);
        duihuaObj.SetActive(value: true);
        duihuaObj.transform.localPosition = new Vector3(0f, 0f, -5);
        zhezhaoyuan1.SetActive(value: true);
        zhezhaoyuan1.transform.localPosition = new Vector3(0f, -11.15f);
        finger.SetActive(value: true);
        finger.transform.localPosition = new Vector3(0f, -12.25f);
        duihuaText.text = GetString().ToString();
    }

    public void Level2()
    {
        SetUseTrue();
        HideGuiObj();
        steps++;
    }

    public void InitLevel3()
    {
        SetUseFalse();
        ShowGuidObj();
        zhezhao.SetActive(value: true);
        level3.SetActive(value: true);
        duihuaObj.SetActive(value: true);
        duihuaObj.transform.localPosition = new Vector3(0f, 8.4f, -5);
        duihuaText.text = GetString().ToString();
    }

    public void Level3()
    {
        SetUseTrue();
        HideGuiObj();
        steps++;
    }

    public void InitMinGuid()
    {
        steps = 1;
        SetUseFalse();
        ShowGuidObj();
        shootBubble = true;
        jiantouup.SetActive(value: true);
        duihuaObj.SetActive(value: true);
        duihuaObj.transform.localPosition = new Vector3(0f, 8.4f, -5);
        duihuaText.text = GetString().ToString().Split('|')[0];
    }

    public void Level5_1()
    {
        steps = 2;
        HideGuiObj();
    }

    public void Level5_2()
    {
        steps = 3;
        SetUseFalse();
        ShowGuidObj();
        useSkill2 = true;
        jiantouskill2.SetActive(value: true);
        duihuaObj.SetActive(value: true);
        duihuaObj.transform.localPosition = new Vector3(0f, 8.4f, -5);
        zhezhao.SetActive(value: true);
        zhezhaoyuan2.SetActive(value: true);
        zhezhaoyuan2.transform.position = gameobjskill2.transform.position;
        duihuaText.text = GetString().ToString().Split('|')[1];
    }

    public void Level5_3()
    {
        skill2 = true;
        useSkill2 = false;
        zhezhaoyuan2.SetActive(value: false);
        jiantouskill2.SetActive(value: false);
        SkillOver();
    }

    public void Level6_1()
    {
        steps = 2;
        SetUseTrue();
        HideGuiObj();
    }

    public void Level9_1()
    {
        steps = 2;
        SetUseTrue();
        HideGuiObj();
    }

    public void Level16_1()
    {
        steps = 2;
        SetUseTrue();
        HideGuiObj();
    }

    public void Level8_1()
    {
        steps = 2;
        HideGuiObj();
    }

    public void Level8_2()
    {
        steps = 3;
        SetUseFalse();
        ShowGuidObj();
        useSkill3 = true;
        jiantouskill3.SetActive(value: true);
        duihuaObj.SetActive(value: true);
        duihuaObj.transform.localPosition = new Vector3(0f, 8.4f, -5);
        zhezhao.SetActive(value: true);
        zhezhaoyuan3.SetActive(value: true);
        zhezhaoyuan3.transform.position = gameobjskill3.transform.position;
        duihuaText.text = GetString().ToString().Split('|')[1];
    }

    public void Level8_3()
    {
        skill3 = true;
        useSkill3 = false;
        zhezhaoyuan3.SetActive(value: false);
        jiantouskill3.SetActive(value: false);
        SkillOver();
    }

    public void Level10_1()
    {
        steps = 2;
        HideGuiObj();
    }

    public void Level10_2()
    {
        steps = 3;
        SetUseFalse();
        ShowGuidObj();
        useSkill2 = true;
        jiantouskill2.SetActive(value: true);
        useSkill3 = true;
        jiantouskill3.SetActive(value: true);
        duihuaObj.SetActive(value: true);
        duihuaObj.transform.localPosition = new Vector3(0f, 8.4f, -5);
        zhezhao.SetActive(value: true);
        zhezhaoyuan3.SetActive(value: true);
        zhezhaoyuan3.transform.position = gameobjskill3.transform.position;
        zhezhaoyuan2.SetActive(value: true);
        zhezhaoyuan2.transform.position = gameobjskill2.transform.position;
        duihuaText.text = GetString().ToString().Split('|')[1];
    }

    public void Level10_3(int skillID)
    {
        if (skillID == 2)
        {
            skill2 = true;
            useSkill2 = false;
            zhezhaoyuan2.SetActive(value: false);
            jiantouskill2.SetActive(value: false);
        }
        if (skillID == 3)
        {
            skill3 = true;
            useSkill3 = false;
            zhezhaoyuan3.SetActive(value: false);
            jiantouskill3.SetActive(value: false);
        }
        if (skill2 && skill3)
        {
            SkillOver(showWindow: false);
        }
    }

    public void Level15_1()
    {
        steps = 2;
        HideGuiObj();
    }

    public void Level15_2()
    {
        steps = 3;
        SetUseFalse();
        ShowGuidObj();
        useSkill1 = true;
        jiantouskill1.SetActive(value: true);
        duihuaObj.SetActive(value: true);
        duihuaObj.transform.localPosition = new Vector3(0f, 8.4f, -5);
        zhezhao.SetActive(value: true);
        zhezhaoyuan1.SetActive(value: true);
        zhezhaoyuan1.transform.position = gameobjskill1.transform.position;
        duihuaText.text = GetString().ToString().Split('|')[1];
    }

    public void Level15_3()
    {
        skill1 = true;
        useSkill1 = false;
        zhezhaoyuan1.SetActive(value: false);
        jiantouskill1.SetActive(value: false);
        SkillOver();
    }

    public void Level15_4()
    {
        skill1 = true;
        useSkill1 = false;
        zhezhaoyuan1.SetActive(value: false);
        jiantouskill1.SetActive(value: false);
        SkillOver();
    }

    public void InitLevel15_5()
    {
        steps = 5;
        SetUseFalse();
        ShowGuidObj();
        shootBubble = true;
        jiantouup.SetActive(value: true);
    }

    public void Level15_6()
    {
        steps = 6;
        HideGuiObj();
        SetUseTrue();
    }

    public void Level17_1()
    {
        steps = 2;
        HideGuiObj();
    }

    public void Level17_2()
    {
        steps = 3;
        SetUseFalse();
        ShowGuidObj();
        useSkill1 = true;
        jiantouskill1.SetActive(value: true);
        useSkill2 = true;
        jiantouskill2.SetActive(value: true);
        useSkill3 = true;
        jiantouskill3.SetActive(value: true);
        duihuaObj.SetActive(value: true);
        duihuaObj.transform.localPosition = new Vector3(0f, 8.4f, -5);
        zhezhao.SetActive(value: true);
        zhezhaoyuan1.SetActive(value: true);
        zhezhaoyuan1.transform.position = gameobjskill1.transform.position;
        zhezhaoyuan2.SetActive(value: true);
        zhezhaoyuan2.transform.position = gameobjskill2.transform.position;
        zhezhaoyuan3.SetActive(value: true);
        zhezhaoyuan3.transform.position = gameobjskill3.transform.position;
        duihuaText.text = GetString().ToString().Split('|')[1];
    }

    public void Level17_3(int skillID)
    {
        if (skillID == 1)
        {
            skill1 = true;
            useSkill1 = false;
            zhezhaoyuan1.SetActive(value: false);
            jiantouskill1.SetActive(value: false);
        }
        if (skillID == 2)
        {
            skill2 = true;
            useSkill2 = false;
            zhezhaoyuan2.SetActive(value: false);
            jiantouskill2.SetActive(value: false);
        }
        if (skillID == 3)
        {
            skill3 = true;
            useSkill3 = false;
            zhezhaoyuan3.SetActive(value: false);
            jiantouskill3.SetActive(value: false);
        }
        if (skill1 && skill2 && skill3)
        {
            SkillOver();
        }
    }

    public void Level24_1()
    {
        steps = 2;
        HideGuiObj();
    }

    public void Level24_2()
    {
        steps = 3;
        SetUseFalse();
        ShowGuidObj();
        useSkill4 = true;
        jiantouskill4.SetActive(value: true);
        duihuaObj.SetActive(value: true);
        duihuaObj.transform.localPosition = new Vector3(0f, 8.4f, -5);
        zhezhao.SetActive(value: true);
        zhezhaoyuan4.SetActive(value: true);
        zhezhaoyuan4.transform.position = gameobjskill4.transform.position;
        duihuaText.text = GetString().ToString().Split('|')[1];
    }

    public void Level24_3()
    {
        skill4 = true;
        useSkill4 = false;
        zhezhaoyuan4.SetActive(value: false);
        jiantouskill4.SetActive(value: false);
        SkillOver();
    }

    public void Level25_1()
    {
        steps = 2;
        HideGuiObj();
    }

    public void Level25_2()
    {
        steps = 3;
        SetUseFalse();
        ShowGuidObj();
        useSkill1 = true;
        jiantouskill1.SetActive(value: true);
        useSkill2 = true;
        jiantouskill2.SetActive(value: true);
        useSkill3 = true;
        jiantouskill3.SetActive(value: true);
        useSkill4 = true;
        jiantouskill4.SetActive(value: true);
        duihuaObj.SetActive(value: true);
        duihuaObj.transform.localPosition = new Vector3(0f, 8.4f, -5);
        zhezhao.SetActive(value: true);
        zhezhaoyuan1.SetActive(value: true);
        zhezhaoyuan1.transform.position = gameobjskill1.transform.position;
        zhezhaoyuan2.SetActive(value: true);
        zhezhaoyuan2.transform.position = gameobjskill2.transform.position;
        zhezhaoyuan3.SetActive(value: true);
        zhezhaoyuan3.transform.position = gameobjskill3.transform.position;
        zhezhaoyuan4.SetActive(value: true);
        zhezhaoyuan4.transform.position = gameobjskill4.transform.position;
        duihuaText.text = GetString().ToString().Split('|')[1];
    }

    public void Level25_3(int skillID)
    {
        if (skillID == 1)
        {
            skill1 = true;
            useSkill1 = false;
            zhezhaoyuan1.SetActive(value: false);
            jiantouskill1.SetActive(value: false);
        }
        if (skillID == 2)
        {
            skill2 = true;
            useSkill2 = false;
            zhezhaoyuan2.SetActive(value: false);
            jiantouskill2.SetActive(value: false);
        }
        if (skillID == 3)
        {
            skill3 = true;
            useSkill3 = false;
            zhezhaoyuan3.SetActive(value: false);
            jiantouskill3.SetActive(value: false);
        }
        if (skillID == 4)
        {
            skill4 = true;
            useSkill4 = false;
            zhezhaoyuan4.SetActive(value: false);
            jiantouskill4.SetActive(value: false);
        }
        if (skill1 && skill2 && skill3 && skill4)
        {
            SkillOver();
        }
    }

    public void SkillOver(bool showWindow = true)
    {
        steps++;
        zhezhao.SetActive(value: false);
        duihuaObj.SetActive(value: false);
        Timer.DelayCallBack(1.3f, delegate
        {
            SetUseTrue();
            HideGuiObj();
            if (showWindow)
            {
                UIManager.OpenUIWindow<GuidUIWindow>();
            }
        });
    }

    public void SkillFull(int skillID)
    {
        if (level == 5 && steps == 2)
        {
            Level5_2();
        }
        else if (level == 8 && steps == 2)
        {
            Level8_2();
        }
        else if (level == 10 && steps == 2)
        {
            Level10_2();
        }
        else if (level == 15 && steps == 2)
        {
            Level15_2();
        }
        else if (level == 17 && steps == 2)
        {
            Level17_2();
        }
        else if (level == 24 && steps == 2)
        {
            Level24_2();
        }
        else if (level == 25 && steps == 2)
        {
            Level25_2();
        }
    }

    public void UseSkill(int skillID)
    {
        if (level == 5 && steps == 3)
        {
            Level5_3();
        }
        else if (level == 8 && steps == 3)
        {
            Level8_3();
        }
        else if (level == 10 && steps == 3)
        {
            Level10_3(skillID);
        }
        else if (level == 15 && steps == 3)
        {
            Level15_3();
        }
        else if (level == 17 && steps == 3)
        {
            Level17_3(skillID);
        }
        else if (level == 24 && steps == 3)
        {
            Level24_3();
        }
        else if (level == 25 && steps == 3)
        {
            Level25_3(skillID);
        }
    }

    public void UseProp()
    {
        if (level == 18 && steps == 2)
        {
            Level18_2();
        }
        else if (level == 19 && steps == 2)
        {
            Level19_2();
        }
        else if (level == 20 && steps == 2)
        {
            Level20_2();
        }
    }

    public void ShootBubble()
    {
        if (level == 1)
        {
            HideShouzhi();
        }
        else if (level == 5 && steps == 1)
        {
            Level5_1();
        }
        else if (level == 6 && steps == 1)
        {
            Level6_1();
        }
        else if (level == 8 && steps == 1)
        {
            Level8_1();
        }
        else if (level == 9 && steps == 1)
        {
            Level9_1();
        }
        else if (level == 10 && steps == 1)
        {
            Level10_1();
        }
        else if (level == 15 && steps == 1)
        {
            Level15_1();
        }
        else if (level == 16 && steps == 1)
        {
            Level16_1();
        }
        else if (level == 17 && steps == 1)
        {
            Level17_1();
        }
        else if (level == 24 && steps == 1)
        {
            Level24_1();
        }
        else if (level == 25 && steps == 1)
        {
            Level25_1();
        }
        else if (level == 15 && steps == 5)
        {
            Level15_6();
        }
    }

    public bool TouchEnd()
    {
        if (level == 3)
        {
            Level3();
        }
        return true;
    }

    public void ChangeBubble()
    {
        if (level == 2)
        {
            Level2();
        }
    }

    public void CloseWindow()
    {
        if (level == 1)
        {
            InitShouzhi();
        }
        else if (level == 18 && steps == 1)
        {
            Level18_1();
        }
        else if (level == 19 && steps == 1)
        {
            Level19_1();
        }
        else if (level == 20 && steps == 1)
        {
            Level20_1();
        }
        else if (level == 15 && steps == 4)
        {
            InitLevel15_5();
        }
        else
        {
            SetUseTrue();
        }
    }

    public void SetUseFalse()
    {
        useSkill1 = false;
        useSkill2 = false;
        useSkill3 = false;
        useSkill4 = false;
        useProp1 = false;
        useProp2 = false;
        useProp3 = false;
        usePause = false;
        shootBubble = false;
        changeBubble = false;
    }

    public void SetUseTrue()
    {
        useSkill1 = true;
        useSkill2 = true;
        useSkill3 = true;
        useSkill4 = true;
        useProp1 = true;
        useProp2 = true;
        useProp3 = true;
        usePause = true;
        shootBubble = true;
        changeBubble = true;
    }

    public void ShowGuidObj()
    {
        guidGameObj.SetActive(value: true);
    }

    public void HideGuiObj()
    {
        guidGameObj.SetActive(value: false);
        duihuaObj.SetActive(value: false);
        zhezhaoyuan1.SetActive(value: false);
        zhezhaoyuan2.SetActive(value: false);
        zhezhaoyuan3.SetActive(value: false);
        zhezhaoyuan4.SetActive(value: false);
        finger.SetActive(value: false);
        zhezhao.SetActive(value: false);
        jiantouskill1.SetActive(value: false);
        jiantouskill2.SetActive(value: false);
        jiantouskill3.SetActive(value: false);
        jiantouskill4.SetActive(value: false);
        level3.SetActive(value: false);
        jiantouprop1.SetActive(value: false);
        jiantouprop2.SetActive(value: false);
        jiantouprop3.SetActive(value: false);
        jiantouup.SetActive(value: false);
    }

    public void InitShouzhi()
    {
        shootBubble = true;
        shouzhi = GameObjectManager.CreateGameObject("GameScene/Guid/LevelGuidShouzhi");
        shouzhi.transform.SetParent(DownObject.transform, worldPositionStays: false);
        shouzhi.transform.localPosition = new Vector3(0f, 4.5f);
    }

    public void HideShouzhi()
    {
        if (shouzhi != null)
        {
            Object.Destroy(shouzhi);
            shouzhi = null;
        }
        SetUseTrue();
        HideGuiObj();
    }

    public bool GetUseSkill1()
    {
        return useSkill1;
    }

    public bool GetUseSkill2()
    {
        return useSkill2;
    }

    public bool GetUseSkill3()
    {
        return useSkill3;
    }

    public bool GetUseSkill4()
    {
        return useSkill4;
    }

    public bool GetUseProp1()
    {
        return useProp1;
    }

    public bool GetUseProp2()
    {
        return useProp2;
    }

    public bool GetUseProp3()
    {
        return useProp3;
    }

    public bool GetUsePause()
    {
        return usePause;
    }

    public bool GetShootBubble()
    {
        return shootBubble;
    }

    public bool GetChangeBubble()
    {
        return changeBubble;
    }
}
