
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapData : Singleton<MapData>
{
    public int[] LMapStarBtnID;

    public int[] LMapBtnCount;

    public int[] LMapEndBtnID;

    public static int iMapCount
    {
        get
        {
            return 34;
        }
    }

    public static int iMaxLevelID = 1100;

    public int iNowMapID;

    public bool bFirstInMap = true;

    public Dictionary<int, int> dDataLevel_Map;

    public void InitMapData()
    {
        InitLMapBtnCount();
        InitLMapStarBtnID();
        InitLMapEndBtnID();
        InitLevel_Map();
        SetLevelMap();
        iNowMapID = RecordManager.GetIntRecord("LevelData", "iNowMapID", 0);
    }

    public void SetLevelMap()
    {
        if (Singleton<UserData>.Instance.GetPassLevel() >= iMaxLevelID)
        {
            int num = iMapCount;
            RecordManager.SaveRecord("LevelData", "iNowMapID", num - 1);
        }
    }

    public void InitLMapBtnCount()
    {
        LMapBtnCount = new int[iMapCount];
        LMapBtnCount[0] = 5;
        LMapBtnCount[1] = 10;
        LMapBtnCount[2] = 15;
        LMapBtnCount[3] = 20;
        for (int i = 4; i < iMapCount; i++)
        {
            LMapBtnCount[i] = 35;
        }

    }

    public void InitLMapEndBtnID()
    {
        LMapEndBtnID = new int[iMapCount];
        LMapEndBtnID[0] = 0;
        int num = 0;
        for (int i = 0; i < iMapCount; i++)
        {
            LMapEndBtnID[i] = num;
            num += LMapBtnCount[i];
        }
    }

    public void InitLMapStarBtnID()
    {
        LMapStarBtnID = new int[iMapCount];
        LMapStarBtnID[0] = 0;
        int num = 0;
        for (int i = 0; i < iMapCount; i++)
        {
            LMapStarBtnID[i] = num;
            num += LMapBtnCount[i];
        }
    }

    public int GetLevelStar(int iNumber)
    {
        if (iNumber > Singleton<UserData>.Instance.GetPassLevel())
        {
            return 0;
        }
        return RecordManager.GetIntRecord("LevelData", "LevelStar_" + iNumber, 0);
    }

    public int GetMapForLevelID(int indexLevel)
    {

        if (indexLevel > iMaxLevelID)
        {
            indexLevel = iMaxLevelID;
        }
        return dDataLevel_Map[indexLevel];
    }

    public void InitLevel_Map()
    {
        dDataLevel_Map = new Dictionary<int, int>();
        int num = 0;
        for (int i = 1; i <= iMapCount; i++)
        {
            for (int j = 1; j <= LMapBtnCount[i - 1]; j++)
            {
                num++;
                dDataLevel_Map.Add(num, i);
            }
        }
        try
        {
            int passLevel = Singleton<UserData>.Instance.GetPassLevel();
            if (passLevel != 0)
            {
                passLevel++;
                if (passLevel < iMaxLevelID)
                {
                    int mapForLevelID = GetMapForLevelID(passLevel);
                    RecordManager.SaveRecord("LevelData", "iNowMapID", mapForLevelID - 1);
                }
            }
        }
        catch (Exception arg)
        {
            UnityEngine.Debug.Log("InitLevel_Map error = " + arg);
        }
    }

    public void SetNowMapID(int iNowMapid)
    {
        if (iNowMapid < iMapCount)
        {
            iNowMapID = iNowMapid;
            RecordManager.SaveRecord("LevelData", "iNowMapID", iNowMapID);
        }
    }

    public void GoNextMap()
    {
        int mapForLevelID = GetMapForLevelID(Singleton<LevelManager>.Instance.GetNowLevel() + 1);
        if (mapForLevelID > iNowMapID)
        {
            SetNowMapID(mapForLevelID - 1);
        }
    }
}
