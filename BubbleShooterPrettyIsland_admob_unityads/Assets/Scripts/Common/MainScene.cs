
using UnityEngine;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    public bool clearAllSaveData = false;

    private void Start()
    {
        //Singleton<MapData>.Instance.iNowMapID = 2;
        //Singleton<UserData>.Instance.SetPassLevel(29);

        if (clearAllSaveData)
        {
            RecordManager.CleanAllRecord();
        }

        if (RecordManager.GetStringRecord("UserData", "RegTime", string.Empty) == string.Empty)
        {
            RecordManager.SaveRecord("UserData", "RegTime", Util.GetNowTime());
        }

        ApplicationManager.SceneName = GameEntry.MainScene;
        AudioPlayManager.PlayMusic2D("bg_music_main_1", 1);
        string str = "LogType:UserDataLog|";
        str = str + "ResTime:" + RecordManager.GetStringRecord("UserData", "RegTime", "19900101") + "|";
        str = str + "Login:" + Util.GetNowTime2() + "|";
        string text = str;
        str = text + "MaxLevel:" + Singleton<UserData>.Instance.GetPassLevel() + "|";
        str = str + "UserGold:" + Singleton<UserData>.Instance.GetUserGold();
        AndroidManager.Instance.UpdataALY(str);

        if (!PlayerData.Instance.FirstInit)
        {
            PlayerData.Instance.FirstInit = true;
            CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.FirstMainPage);
        }
    }

    public void ChangeScene()
    {
        AudioPlayManager.PlaySFX2D("button");
        if (RecordManager.GetIntRecord("UserData", "FirstInGame", 0) == 0)
        {
            Debug.Log("==== FirstInGame");
            RecordManager.SaveRecord("UserData", "LoveCount", 5);
            RecordManager.SaveRecord("UserData", "FirstInGame", 1);
            Singleton<LevelManager>.Instance.SetNowSelectLevel(1);
            GameEntry.ChangeScene(GameEntry.GameScene);
        }
        else
        {
            Debug.Log("==== Not FirstInGame");
            GameEntry.ChangeScene(GameEntry.LevelScene);
        }
    }
}
