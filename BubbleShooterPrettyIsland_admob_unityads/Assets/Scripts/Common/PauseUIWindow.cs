
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIWindow : UIWindowBase
{
    public GameObject SoundClose;

    public GameObject MusicClose;

    public Text SoundText;

    public Text MusicText;

    public Text PauseText;

    public Text ResumeText;

    public Text RetryText;

    public Text QuitText;

    public override void OnOpen()
    {
        AddOnClickListener("CloseButton", OnCloseBtn);
        AddOnClickListener("SoundButton", OnSoundBtn);
        AddOnClickListener("MusicButton", OnMusicBtn);
        AddOnClickListener("ResumeButton", OnResumeBtn);
        AddOnClickListener("RetryButton", OnRetryBtn);
        AddOnClickListener("QuitButton", OnQuitBtn);
        InitLanguage();
        ADInterface.Instance.SendADEvent(ADType.CPAD, ADSpot.PauseGame);
    }

    public void InitLanguage()
    {
        PauseText.text = Util.ReplaceText(GameEntry.Instance.GetString("PauseUITitle"));
        ResumeText.text = Util.ReplaceText(GameEntry.Instance.GetString("PauseUI1"));
        RetryText.text = Util.ReplaceText(GameEntry.Instance.GetString("PauseUI2"));
        QuitText.text = Util.ReplaceText(GameEntry.Instance.GetString("PauseUI3"));
        SoundText.text = Util.ReplaceText(GameEntry.Instance.GetString("SetUI1"));
        MusicText.text = Util.ReplaceText(GameEntry.Instance.GetString("SetUI2"));
    }

    public void OnCloseBtn(InputUIOnClickEvent e)
    {
        UIManager.CloseUIWindow<PauseUIWindow>(isPlayAnim: true, null, new object[0]);
    }

    public void OnMusicBtn(InputUIOnClickEvent e)
    {
        if (AudioPlayManager.MusicVolume > 0f)
        {
            AudioPlayManager.MusicVolume = 0f;
            MusicClose.SetActive(value: true);
        }
        else
        {
            AudioPlayManager.MusicVolume = 1f;
            MusicClose.SetActive(value: false);
        }
    }

    public void OnSoundBtn(InputUIOnClickEvent e)
    {
        if (AudioPlayManager.SFXVolume > 0f)
        {
            AudioPlayManager.SFXVolume = 0f;
            SoundClose.SetActive(value: true);
        }
        else
        {
            AudioPlayManager.SFXVolume = 1f;
            SoundClose.SetActive(value: false);
        }
    }

    public void OnResumeBtn(InputUIOnClickEvent e)
    {
        AndroidManager.Instance.UpLevelData("pause_resume");
        UIManager.CloseUIWindow<PauseUIWindow>(isPlayAnim: true, null, new object[0]);
    }

    public void OnRetryBtn(InputUIOnClickEvent e)
    {
        AndroidManager.Instance.UpLevelData("pause_retry");
        Singleton<UserData>.Instance.IsQuit = false;
        UIManager.OpenUIWindow<ExitUIWindow>();
    }

    public void OnQuitBtn(InputUIOnClickEvent e)
    {
        AndroidManager.Instance.UpLevelData("pause_quit");
        Singleton<UserData>.Instance.IsQuit = true;
        UIManager.OpenUIWindow<ExitUIWindow>();
    }

    public override void OnRefresh()
    {
    }

    public override IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        StartCoroutine(base.EnterAnim(animComplete, callBack, objs));
        yield return new WaitForEndOfFrame();
    }

    public override IEnumerator ExitAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        StartCoroutine(base.ExitAnim(animComplete, callBack, objs));
        yield return new WaitForEndOfFrame();
    }

    public override IEnumerator PauserAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        StartCoroutine(base.PauserAnim(animComplete, callBack, objs));
        yield return new WaitForEndOfFrame();
    }
}
