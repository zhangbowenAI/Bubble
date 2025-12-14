
using DG.Tweening;
using UnityEngine;

public class GameEntry : MonoBehaviour
{
    public enum BuildType
    {
        GoogleEN = 1

    }

    public static BuildType buildType = BuildType.GoogleEN;

    public static GameEntry Instance;

    public static string MainScene = "MainScene";

    public static string GameScene = "GameScene";

    public static string LevelScene = "LevelScene";

    public static string LoadingScene = "LoadingScene";

    public static Sprite loadingBG;

    private void InitAudio()
    {
        AudioPlayManager.LoadMusic("bg_music_main_1");
        AudioPlayManager.LoadMusic("bg_music_main");
        AudioPlayManager.LoadMusic("bg_music_map");
        RecordManager.SaveRecord("GameSettingData", "MusicVolume", 1);
        RecordManager.SaveRecord("GameSettingData", "SFXVolume", 1);
    }

    private void InitData()
    {
        Singleton<UserData>.Instance.InitUserData();
        Singleton<MapData>.Instance.InitMapData();
    }

    public string GetString(string key)
    {
        return Singleton<DataLanguageZH>.Instance.GetContentByKeyAndType(key, LanguageZHType.Text);
    }

    private void InitSDK()
    {
    }

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        GameObject gameObject = new GameObject("[GameEntry]");
        gameObject.AddComponent<GameEntry>();
        Instance = gameObject.GetComponent<GameEntry>();
        UnityEngine.Object.DontDestroyOnLoad(gameObject);
        string path = "LoadingScene/splash";
        Application.targetFrameRate = 60;
        Texture2D texture2D = ResourceManager.Load(path) as Texture2D;
        loadingBG = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), Vector2.zero);
    }

    private void Awake()
    {
        ApplicationManager.Init();
        InitData();
        InitSDK();
        base.transform.DOMove(Vector3.zero, 0.1f);
    }

    public void Start()
    {
        InitAudio();
        UIManager.CreateUIWindow<PlayWindow>();

        string stringRecord = RecordManager.GetStringRecord("UserData", "ip", "no");
        if (stringRecord == "no")
        {
            Singleton<UserData>.Instance.city = "unkcity";
            Singleton<UserData>.Instance.ip = "192.120.1.70";
            RecordManager.SaveRecord("UserData", "ip", Singleton<UserData>.Instance.ip);
            RecordManager.SaveRecord("UserData", "city", Singleton<UserData>.Instance.city);
            return;
        }
        Singleton<UserData>.Instance.city = RecordManager.GetStringRecord("UserData", "city", "no");
        Singleton<UserData>.Instance.ip = RecordManager.GetStringRecord("UserData", "ip", "no");
    }

    public static void ChangeScene(string sceneName)
    {

        // try
        // {
        //     AGame.MySdkManager.me.myLog.LogEvent("change_scene_" + sceneName, "", "");
        // }
        // catch { }
        if (sceneName == GameScene)
        {
            ADInterface.Instance.SendADEvent(ADType.HideBanner, ADSpot.GameBanner);
            if (Singleton<UserData>.Instance.GetPassLevel() > 5)
                ADInterface.Instance.SendADEvent(ADType.CPAD, ADSpot.StartGame);
        }
        ApplicationManager.SceneName = sceneName;
        SingletonMonoBehaviour<TransitionManager>.Instance.LoadSceneIn(LoadingScene, loadingBG);
    }
}
