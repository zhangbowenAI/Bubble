

public class UserData : Singleton<UserData>
{
    public static int iFirstLoginGameLoadCloud;

    public bool bGrilMoveing;

    public int iLoveMaxAll = 5;

    public int ResLoveTime = 1800;

    public bool PlaySkillUse1;

    public bool PlaySkillUse2;

    public bool PlaySkillUse3;

    public bool PlaySkillUseViedo;

    public bool PlaySkillUseMagicPower;

    public bool PlaySkillUse1ByContinuousWin;

    public bool PlaySkillUse2ByContinuousWin;

    public int buySkillType;

    public int BuyMagicType;

    public bool IsQuit;

    public int BuyMoveCount;

    public bool IsNewTopUI;

    public string city = "no";

    public string ip = "no";

    public void InitUserData()
    {
        LoadNowPassLevelNumber();
    }

    public void LoadNowPassLevelNumber()
    {
        ContinuousItemArranger.gSequenceCount = RecordManager.GetIntRecord("LevelData", "SequenceWinCount", 0);
        iFirstLoginGameLoadCloud = RecordManager.GetIntRecord("LevelData", "iFirstLoginGameLoadCloud", 0);
        OpenProps(GetPassLevel());
    }

    public bool GetWinByLevel(int level)
    {
        return RecordManager.GetBoolRecord("WinData", "iWinByLevel" + level, defaultValue: false);
    }

    public void SetWinByLevel(int level)
    {
        RecordManager.SaveRecord("WinData", "iWinByLevel" + level, value: true);
    }

    public int GetPassLevel()
    {
        return RecordManager.GetIntRecord("LevelData", "iNowPassLevelID", 0);
    }

    public void SetPassLevel(int level)
    {
        RecordManager.SaveRecord("LevelData", "iNowPassLevelID", level);
    }

    public void AddPassLevel()
    {
        // try
        // {
        //     AGame.MySdkManager.me.myLog.LogEvent("pass_level_" + GetPassLevel() + 1, "", "");
        // }
        // catch { }


        RecordManager.SaveRecord("LevelData", "iNowPassLevelID", GetPassLevel() + 1);
    }

    public int getLoveInfinite()
    {
        int intRecord = RecordManager.GetIntRecord("UserData", "LoveInfinite", 0);
        if (intRecord <= 0)
        {
            RecordManager.SaveRecord("UserData", "LoveInfinite", 0);
            return 0;
        }
        int nowTime = Util.GetNowTime();
        intRecord -= nowTime;
        if (intRecord > 0)
        {
            return intRecord;
        }
        RecordManager.SaveRecord("UserData", "LoveInfinite", 0);
        return 0;
    }

    public int GetUserGold()
    {
        return RecordManager.GetIntRecord("UserData", "GoldNumBer", 0);
    }

    public void AddUserGold(int Num)
    {
        int intRecord = RecordManager.GetIntRecord("UserData", "GoldNumBer", 0);
        intRecord += Num;
        RecordManager.SaveRecord("UserData", "GoldNumBer", intRecord);
        GlobalEvent.DispatchEvent(GameEventEnum.UIFlushChane);
    }

    public void DelUserGold(int Num)
    {
        int intRecord = RecordManager.GetIntRecord("UserData", "GoldNumBer", 0);
        intRecord -= Num;
        RecordManager.SaveRecord("UserData", "GoldNumBer", intRecord);
        GlobalEvent.DispatchEvent(GameEventEnum.UIFlushChane);
    }

    public void AddLoveCount(int num)
    {
        if (num >= 1)
        {
            int userLoveCount = Singleton<UserData>.Instance.GetUserLoveCount();
            if (userLoveCount < iLoveMaxAll)
            {
                int intRecord = RecordManager.GetIntRecord("UserData", "FullLoveTime", 0);
                intRecord -= ResLoveTime * num;
                RecordManager.SaveRecord("UserData", "FullLoveTime", intRecord);
            }
            userLoveCount += num;
            RecordManager.SaveRecord("UserData", "LoveCount", userLoveCount);
            GlobalEvent.DispatchEvent(GameEventEnum.UIFlushChane);
        }
    }

    public void DelLoveCount(int num)
    {
        if (getLoveInfinite() > 0)
        {
            return;
        }
        int userLoveCount = Singleton<UserData>.Instance.GetUserLoveCount();
        if (userLoveCount <= 0)
        {
            UnityEngine.Debug.Log("Physical strength is 0");
            return;
        }
        userLoveCount -= num;
        RecordManager.SaveRecord("UserData", "LoveCount", userLoveCount);
        for (int i = 0; i < num; i++)
        {
            if (userLoveCount < iLoveMaxAll)
            {
                int num2 = 0;
                int intRecord = RecordManager.GetIntRecord("UserData", "FullLoveTime", 0);
                num2 = ((Util.GetNowTime() >= intRecord) ? (Util.GetNowTime() + ResLoveTime) : (intRecord + ResLoveTime));
                RecordManager.SaveRecord("UserData", "FullLoveTime", num2);
            }
        }
        GlobalEvent.DispatchEvent(GameEventEnum.UIFlushChane);
    }

    public void AddLoveInfinite(int type, float time)
    {
        int intRecord = RecordManager.GetIntRecord("UserData", "LoveInfinite", 0);
        int num = 60;
        switch (type)
        {
            case 10:
                num = 30;
                break;
            case 9:
                num = 60;
                break;
            case 11:
                num = 120;
                break;
        }
        if (intRecord <= 0)
        {
            int nowTime = Util.GetNowTime();
            nowTime += (int)(time * (float)num * 60f);
            RecordManager.SaveRecord("UserData", "LoveInfinite", nowTime);
        }
        else
        {
            int nowTime2 = Util.GetNowTime();
            intRecord -= nowTime2;
            if (intRecord > 0)
            {
                nowTime2 += intRecord + (int)(time * (float)num * 60f);
                RecordManager.SaveRecord("UserData", "LoveInfinite", nowTime2);
            }
            else
            {
                nowTime2 += (int)(time * (float)num * 60f);
                RecordManager.SaveRecord("UserData", "LoveInfinite", nowTime2);
            }
        }
        GlobalEvent.DispatchEvent(GameEventEnum.UIFlushChane);
    }

    public int GetUserLoveCount()
    {
        if (RecordManager.GetIntRecord("UserData", "LoveCount", 0) < 0)
        {
            RecordManager.SaveRecord("UserData", "LoveCount", 0);
        }
        if (RecordManager.GetIntRecord("UserData", "LoveInfinite", 0) > 0)
        {
            return iLoveMaxAll;
        }
        return RecordManager.GetIntRecord("UserData", "LoveCount", 0);
    }

    public void DelProps(int index)
    {
        if ((index != 0 || Singleton<LevelManager>.Instance.GetNowLevel() != 12) && (index != 1 || Singleton<LevelManager>.Instance.GetNowLevel() != 13) && (index != 2 || Singleton<LevelManager>.Instance.GetNowLevel() != 14))
        {
            int intRecord = RecordManager.GetIntRecord("LevelData", "SkillOpenNum_" + index, 0);
            intRecord--;
            RecordManager.SaveRecord("LevelData", "SkillOpenNum_" + index, intRecord);
            GlobalEvent.DispatchEvent(GameEventEnum.UIFlushChane);
        }
    }

    public void AddProps(int index, int Num)
    {
        int intRecord = RecordManager.GetIntRecord("LevelData", "SkillOpenNum_" + index, 0);
        intRecord += Num;
        RecordManager.SaveRecord("LevelData", "SkillOpenNum_" + index, intRecord);
        GlobalEvent.DispatchEvent(GameEventEnum.UIFlushChane);
    }

    public void OpenProps(int level)
    {
        if (level >= 11 && RecordManager.GetIntRecord("LevelData", "SkillOpen_0", 0) == 0)
        {
            RecordManager.SaveRecord("LevelData", "SkillOpen_0", 1);
            AddProps(0, 3);
        }
        if (level >= 12 && RecordManager.GetIntRecord("LevelData", "SkillOpen_1", 0) == 0)
        {
            RecordManager.SaveRecord("LevelData", "SkillOpen_1", 1);
            AddProps(1, 3);
        }
        if (level >= 13 && RecordManager.GetIntRecord("LevelData", "SkillOpen_2", 0) == 0)
        {
            RecordManager.SaveRecord("LevelData", "SkillOpen_2", 1);
            AddProps(2, 3);
        }
        if (level >= 17 && RecordManager.GetIntRecord("LevelData", "SkillOpen_3", 0) == 0)
        {
            RecordManager.SaveRecord("LevelData", "SkillOpen_3", 1);
            AddProps(3, 3);
        }
        if (level >= 18 && RecordManager.GetIntRecord("LevelData", "SkillOpen_4", 0) == 0)
        {
            RecordManager.SaveRecord("LevelData", "SkillOpen_4", 1);
            AddProps(4, 3);
        }
        if (level >= 19 && RecordManager.GetIntRecord("LevelData", "SkillOpen_5", 0) == 0)
        {
            RecordManager.SaveRecord("LevelData", "SkillOpen_5", 1);
            AddProps(5, 3);
        }
    }

    public void ClearJinZhuGold()
    {
        RecordManager.SaveRecord("UserData", "JinZhuGold", 0);
        GlobalEvent.DispatchEvent(GameEventEnum.UIFlushChane);
    }

    public int GetJinZhuGold()
    {
        int num = RecordManager.GetIntRecord("UserData", "JinZhuGold", 0);
        if (num > 6000)
        {
            num = 6000;
            RecordManager.SaveRecord("UserData", "JinZhuGold", num);
        }
        return num;
    }

    public void AddJinZhuGold(int Goldnum)
    {
        int intRecord = RecordManager.GetIntRecord("UserData", "JinZhuGold", 0);
        intRecord += Goldnum;
        if (intRecord > 6000)
        {
            intRecord = 6000;
            RecordManager.SaveRecord("UserData", "JinZhuGold", intRecord);
        }
        else
        {
            RecordManager.SaveRecord("UserData", "JinZhuGold", intRecord);
        }
    }
}
