
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public struct LevelObject
    {
        public string key;

        public int value;
    }

    public JsonData dJsonData;

    public List<LevelObject> gemSpawnChance = new List<LevelObject>();

    public List<BubbleData> LTbubble;

    public List<BubbleData> LTBubbleTop;

    public List<BubbleData> LTDown;

    public int iLevelType;

    public int iLevelCount;

    private int iNowSelectLevelIndex = -1;

    private int iNowLevel = -1;

    public int iNowPassLevel = -1;

    public int star1;

    public int star2;

    public int star3;

    public int G1;

    public int G2;

    public int G3;

    public int G4;

    public int G5;

    public int R1;

    public int R2;

    public int R3;

    public int R4;

    public int iNowStar;

    public int iNowScore;

    public int AllBubbleCount
    {
        get;
        set;
    }

    public int InitBubbleCount
    {
        get;
        private set;
    }

    public override void Init()
    {
    }

    public int GetNowLevel()
    {
        return iNowLevel;
    }

    private void SetNowLevel(int level)
    {
        iNowLevel = level;
    }

    public void SetNowSelectLevel(int level)
    {
        if (level <= MapData.iMaxLevelID)
        {
            iNowSelectLevelIndex = level;
        }
    }

    public int GetNowSelectLevel()
    {
        return iNowSelectLevelIndex;
    }

    public bool LoadLevelData(bool bwww = false, JsonData www_dJsonData = null)
    {
        UnityEngine.Debug.Log("==== Load level ====" + GetNowSelectLevel());
        string text = "Level_" + GetNowSelectLevel();
        iNowSelectLevelIndex = GetNowSelectLevel();
        SetNowLevel(GetNowSelectLevel());
        iNowPassLevel = Singleton<UserData>.Instance.GetPassLevel();
        if (GetNowSelectLevel() > 100)
        {
            text += "_1";
        }
        iNowStar = 0;
        star1 = 0;
        star2 = 0;
        star3 = 0;
        G1 = 0;
        G2 = 0;
        G3 = 0;
        G4 = 0;
        G5 = 0;
        R1 = 0;
        R2 = 0;
        R3 = 0;
        R4 = 0;
        InitBubbleCount = 0;
        iLevelType = 0;
        iLevelCount = 0;
        Singleton<MapData>.Instance.bFirstInMap = false;
        Singleton<UserData>.Instance.IsQuit = false;
        Singleton<UserData>.Instance.BuyMoveCount = 0;
        Singleton<UserData>.Instance.DelLoveCount(1);
        Singleton<UserData>.Instance.BuyMagicType = 0;
        string empty = string.Empty;

        TextAsset textAsset = (TextAsset)ResourceManager.Load("Data/level/" + text);
        if (textAsset == null)
        {
            UnityEngine.Debug.LogError("Invalid level !");
            return false;
        }
        empty = textAsset.ToString();

        if (empty == string.Empty)
        {
            UnityEngine.Debug.LogError("Invalid level !!");
            return false;
        }
        empty = empty.Replace("\r\n", string.Empty);
        dJsonData = JsonMapper.ToObject(empty);
        InitBubbleCount = (int)dJsonData["BallNumber"];
        AllBubbleCount = InitBubbleCount;
        star1 = (int)dJsonData["Star"]["star1"];
        star2 = (int)dJsonData["Star"]["star2"];
        star3 = (int)dJsonData["Star"]["star3"];
        try
        {
            R1 = (int)dJsonData["ReadyBubble"]["R1"];
            R2 = (int)dJsonData["ReadyBubble"]["R2"];
            R3 = (int)dJsonData["ReadyBubble"]["R3"];
            R4 = (int)dJsonData["ReadyBubble"]["R4"];
        }
        catch (Exception)
        {
        }
        try
        {
            G1 = (int)dJsonData["Gang"]["Gang1"];
            G2 = (int)dJsonData["Gang"]["Gang2"];
            G3 = (int)dJsonData["Gang"]["Gang3"];
            G4 = (int)dJsonData["Gang"]["Gang4"];
            G5 = (int)dJsonData["Gang"]["Gang5"];
        }
        catch (Exception)
        {
        }
        iLevelType = (int)dJsonData["LevelType"]["Type"];
        iLevelCount = (int)dJsonData["LevelType"]["Number"];
        gemSpawnChance = new List<LevelObject>();
        LevelObject item = default(LevelObject);
        for (int i = 0; i < dJsonData["ObjChance"].Count; i++)
        {
            item.key = dJsonData["ObjChance"][i]["Key"].ToString();
            item.value = int.Parse(dJsonData["ObjChance"][i]["P"].ToString());
            gemSpawnChance.Add(item);
        }
        LTbubble = new List<BubbleData>();
        LTBubbleTop = new List<BubbleData>();
        LTDown = new List<BubbleData>();
        BubbleData item2 = default(BubbleData);
        for (int j = 0; j < dJsonData["bubble"].Count; j++)
        {
            string key = dJsonData["bubble"][j]["key"].ToString();
            string s = dJsonData["bubble"][j]["x"].ToString();
            string s2 = dJsonData["bubble"][j]["y"].ToString();
            string s3 = dJsonData["bubble"][j]["s"].ToString();
            string s4 = dJsonData["bubble"][j]["i"].ToString();
            item2.key = key;
            item2.row = int.Parse(s);
            item2.col = int.Parse(s2);
            item2.s = int.Parse(s3);
            item2.i = int.Parse(s4);
            item2.isReadyBubble = false;
            item2.isTopData = false;
            LTbubble.Add(item2);
        }
        BubbleData item3 = default(BubbleData);
        for (int k = 0; k < dJsonData["sub"].Count; k++)
        {
            string key2 = dJsonData["sub"][k]["key"].ToString();
            string s5 = dJsonData["sub"][k]["x"].ToString();
            string s6 = dJsonData["sub"][k]["y"].ToString();
            string s7 = dJsonData["sub"][k]["s"].ToString();
            string s8 = dJsonData["sub"][k]["i"].ToString();
            item3.key = key2;
            item3.row = int.Parse(s5);
            item3.col = int.Parse(s6);
            item3.s = int.Parse(s7);
            item3.i = int.Parse(s8);
            item3.isReadyBubble = false;
            item3.isTopData = true;
            LTBubbleTop.Add(item3);
        }
        return true;
    }

    public bool GetGangSkill(int iGangID)
    {
        switch (iGangID)
        {
            case 1:
                if (G1 == 1)
                {
                    return true;
                }
                break;
            case 2:
                if (G2 == 1)
                {
                    return true;
                }
                break;
            case 3:
                if (G4 == 1)
                {
                    return true;
                }
                break;
            case 4:
                if (G5 == 1)
                {
                    return true;
                }
                break;
            case 5:
                if (G5 == 1)
                {
                    return true;
                }
                break;
        }
        return false;
    }
}
