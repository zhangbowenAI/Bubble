
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SettingUIWindow : UIWindowBase
{
    public GameObject MusicClose;

    public GameObject SoundClose;

    public Text TitleText;

    public Text SoundText;

    public Text MusicText;

    public override void OnOpen()
    {
        AddOnClickListener("SoundButton", OnSoundBtn);
        AddOnClickListener("CloseButton", OnCloseBtn);
        AddOnClickListener("MusicButton", OnMusicBtn);
        InitLanguage();
        ADInterface.Instance.SendADEvent(ADType.NativeStartAD, ADSpot.SettingNaticeAd);
    }

    public void InitLanguage()
    {
        TitleText.text = Util.ReplaceText(GameEntry.Instance.GetString("SetUITitle"));
        SoundText.text = Util.ReplaceText(GameEntry.Instance.GetString("SetUI1"));
        MusicText.text = Util.ReplaceText(GameEntry.Instance.GetString("SetUI2"));
    }

    public void OnMusicBtn(InputUIOnClickEvent e)
    {
        if (AudioPlayManager.MusicVolume > 0f)
        {
            AudioPlayManager.MusicVolume = 0f;
            AudioPlayManager.SaveVolume();
            MusicClose.SetActive(value: true);
        }
        else
        {
            AudioPlayManager.MusicVolume = 1f;
            AudioPlayManager.SaveVolume();
            MusicClose.SetActive(value: false);
        }
    }

    public void OnSoundBtn(InputUIOnClickEvent e)
    {
        if (AudioPlayManager.SFXVolume > 0f)
        {
            AudioPlayManager.SFXVolume = 0f;
            AudioPlayManager.SaveVolume();
            SoundClose.SetActive(value: true);
        }
        else
        {
            AudioPlayManager.SFXVolume = 1f;
            AudioPlayManager.SaveVolume();
            SoundClose.SetActive(value: false);
        }
    }

    public void OnCloseBtn(InputUIOnClickEvent e)
    {
        UIManager.CloseUIWindow<SettingUIWindow>(isPlayAnim: true, null, new object[0]);
        ADInterface.Instance.SendADEvent(ADType.NativeEndAD, ADSpot.SettingNaticeAd);
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
