
using System.Collections;
using UnityEngine;

public class InitScene : MonoBehaviour {

    public bool clearAllSaveData = false;

    private void Start() {

        if (clearAllSaveData)
        {
            RecordManager.CleanAllRecord();
        }

        // try
        // {
        //     AGame.MySdkManager.me.myLog.LogEvent("InitScene", "", "");
        // }
        // catch { }
        if (RecordManager.GetStringRecord("UserData", "RegTime", string.Empty) == string.Empty)
        {
            RecordManager.SaveRecord("UserData", "RegTime", Util.GetNowTime());
        }

        GameEntry.ChangeScene(GameEntry.MainScene);

        // try
        // {
        //     AGame.MySdkManager.me.myLog.LogEvent("MainScene_1", "", "");
        // }
        // catch { }
   
    }

}
