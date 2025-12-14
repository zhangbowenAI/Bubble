
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerBase
{
    protected MonoBehaviour mono;

    protected int maxSFXAudioAssetNum = 10;

    private float musicVolume = 1f;

    private float sfxVolume = 1f;

    protected Dictionary<AudioAsset, VolumeFadeData> fadeData = new Dictionary<AudioAsset, VolumeFadeData>();

    private List<AudioAsset> deleteAssets = new List<AudioAsset>();

    private Queue<VolumeFadeData> catcheData = new Queue<VolumeFadeData>();

    public float MusicVolume => musicVolume;

    public float SFXVolume => sfxVolume;

    public AudioPlayerBase(MonoBehaviour mono)
    {
        this.mono = mono;
    }

    public void SetMaxSFXAudioAssetNum(int max)
    {
        maxSFXAudioAssetNum = Mathf.Clamp(max, 5, 100);
    }

    public virtual void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    public virtual void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }

    public AudioClip GetAudioClip(string name)
    {
        if (!AudioPlayManager.dClips.ContainsKey(name))
        {
            string str = "Audio/";
            AudioClip audioClip = ResourceManager.Load<AudioClip>(str + name);
            if (audioClip != null)
            {
            }
            if (audioClip == null)
            {
                if (name != "b_jingxiang")
                {
                    UnityEngine.Debug.Log("Can not find AudioClip:" + name);
                }
                return null;
            }
            AudioPlayManager.dClips[name] = audioClip;
        }
        else if (AudioPlayManager.dClips[name] == null)
        {
            string str2 = "Audio/";
            AudioClip audioClip2 = ResourceManager.Load<AudioClip>(str2 + name);
            if (audioClip2 != null)
            {
            }
            if (audioClip2 == null)
            {
                UnityEngine.Debug.LogError("Can not find AudioClip:" + name);
            }
            AudioPlayManager.dClips[name] = audioClip2;
        }
        return AudioPlayManager.dClips[name];
    }

    public AudioAsset CreateAudioAsset(GameObject gameObject, bool is3D, bool isMusic)
    {
        AudioAsset audioAsset = new AudioAsset();
        audioAsset.audioSource = gameObject.AddComponent<AudioSource>();
        audioAsset.audioSource.spatialBlend = (is3D ? 1 : 0);
        if (isMusic)
        {
            audioAsset.TotleVolume = musicVolume;
        }
        else
        {
            audioAsset.TotleVolume = sfxVolume;
        }
        return audioAsset;
    }

    protected void PlayClip(AudioAsset au, string audioName, bool isLoop = true, float volumeScale = 1f, float delay = 0f, bool nowPlay = false)
    {
        if (!nowPlay)
        {
            if (!AudioPlayManager.dPlayeTime.ContainsKey(audioName))
            {
                AudioPlayManager.dPlayeTime[audioName] = GetAudioNowTime();
            }
            else
            {
                if (GetAudioNowTime() - AudioPlayManager.dPlayeTime[audioName] <= 50)
                {
                    return;
                }
                AudioPlayManager.dPlayeTime[audioName] = GetAudioNowTime();
            }
        }
        au.assetName = audioName;
        AudioClip audioClip = GetAudioClip(au.assetName);
        au.audioSource.clip = audioClip;
        au.audioSource.loop = isLoop;
        au.Play(delay);
        au.VolumeScale = volumeScale;
    }

    public static int GetAudioNowTime()
    {
        string s = DateTime.Now.ToString("hhmmssfff");
        return int.Parse(s);
    }

    protected void PlayMusicControl(AudioAsset au, string audioName, bool isLoop = true, float volumeScale = 1f, float delay = 0f, float fadeTime = 0.5f)
    {
        if (au.assetName == audioName)
        {
            au.SetPlayState(AudioPlayState.Playing);
            AddFade(au, VolumeFadeType.FadeIn, fadeTime, delay, null, null);
            au.Play();
            return;
        }
        AudioPlayState playState = au.PlayState;
        au.SetPlayState(AudioPlayState.Playing);
        if (playState == AudioPlayState.Playing)
        {
            AddFade(au, VolumeFadeType.FadeOut2In, fadeTime, delay, null, delegate (AudioAsset value)
            {
                PlayClip(value, audioName, isLoop, volumeScale, delay);
            });
            return;
        }
        PlayClip(au, audioName, isLoop, volumeScale, delay);
        AddFade(au, VolumeFadeType.FadeIn, fadeTime, delay, null, null);
    }

    protected void PauseMusicControl(AudioAsset au, bool isPause, float fadeTime = 0.5f)
    {
        if (isPause)
        {
            if (au.PlayState == AudioPlayState.Playing)
            {
                au.SetPlayState(AudioPlayState.Pause);
                AddFade(au, VolumeFadeType.FadeOut, fadeTime, 0f, delegate (AudioAsset value)
                {
                    value.Pause();
                }, null);
            }
        }
        else if (au.PlayState == AudioPlayState.Pause)
        {
            au.SetPlayState(AudioPlayState.Playing);
            AddFade(au, VolumeFadeType.FadeIn, fadeTime, 0f, null, null);
            au.Play();
        }
    }

    protected void StopMusicControl(AudioAsset au, float fadeTime = 0.5f)
    {
        if (au.PlayState != AudioPlayState.Stop)
        {
            au.SetPlayState(AudioPlayState.Stop);
            UnityEngine.Debug.Log("StopMusicControl Stop");
            AddFade(au, VolumeFadeType.FadeOut, fadeTime, 0f, delegate (AudioAsset value)
            {
                UnityEngine.Debug.LogWarning("StopMusicControl Stop fade CallBack");
                value.Stop();
            }, null);
        }
    }

    public void UpdateFade()
    {
        if (fadeData.Count <= 0)
        {
            return;
        }
        foreach (VolumeFadeData value in fadeData.Values)
        {
            bool flag = false;
            switch (value.fadeType)
            {
                case VolumeFadeType.FadeIn:
                    flag = FadeIn(value);
                    break;
                case VolumeFadeType.FadeOut:
                    flag = FadeOut(value);
                    break;
                case VolumeFadeType.FadeOut2In:
                    flag = FadeOut2In(value);
                    break;
            }
            if (flag)
            {
                deleteAssets.Add(value.au);
            }
        }
        if (deleteAssets.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < deleteAssets.Count; i++)
        {
            AudioAsset audioAsset = deleteAssets[i];
            VolumeFadeData volumeFadeData = fadeData[audioAsset];
            if (volumeFadeData.fadeCompleteCallBack != null)
            {
                volumeFadeData.fadeCompleteCallBack(audioAsset);
            }
            volumeFadeData.fadeCompleteCallBack = null;
            volumeFadeData.fadeOutCompleteCallBack = null;
            catcheData.Enqueue(volumeFadeData);
            fadeData.Remove(audioAsset);
        }
        deleteAssets.Clear();
    }

    public void AddFade(AudioAsset au, VolumeFadeType fadeType, float fadeTime, float delay, CallBack<AudioAsset> fadeCompleteCallBack, CallBack<AudioAsset> fadeOutCompleteCallBack)
    {
        VolumeFadeData volumeFadeData = null;
        if (fadeData.ContainsKey(au))
        {
            volumeFadeData = fadeData[au];
        }
        else
        {
            volumeFadeData = ((catcheData.Count <= 0) ? new VolumeFadeData() : catcheData.Dequeue());
            fadeData.Add(au, volumeFadeData);
        }
        if (volumeFadeData.fadeOutCompleteCallBack != null)
        {
            volumeFadeData.fadeOutCompleteCallBack(volumeFadeData.au);
        }
        if (volumeFadeData.fadeCompleteCallBack != null)
        {
            volumeFadeData.fadeCompleteCallBack(volumeFadeData.au);
        }
        volumeFadeData.fadeCompleteCallBack = null;
        volumeFadeData.fadeOutCompleteCallBack = null;
        volumeFadeData.au = au;
        volumeFadeData.fadeType = fadeType;
        volumeFadeData.fadeTime = fadeTime;
        if (volumeFadeData.fadeTime <= 0f)
        {
            volumeFadeData.fadeTime = 1E-06f;
        }
        volumeFadeData.fadeCompleteCallBack = fadeCompleteCallBack;
        volumeFadeData.fadeOutCompleteCallBack = fadeOutCompleteCallBack;
        switch (volumeFadeData.fadeType)
        {
            case VolumeFadeType.FadeIn:
                volumeFadeData.fadeState = VolumeFadeStateType.FadeIn;
                break;
            case VolumeFadeType.FadeOut:
                volumeFadeData.fadeState = VolumeFadeStateType.FadeOut;
                break;
            case VolumeFadeType.FadeOut2In:
                volumeFadeData.fadeState = VolumeFadeStateType.FadeOut;
                break;
        }
        volumeFadeData.tempVolume = volumeFadeData.au.Volume;
        volumeFadeData.delayTime = delay;
    }

    private bool FadeIn(VolumeFadeData data)
    {
        if (string.IsNullOrEmpty(data.au.assetName))
        {
            data.au.ResetVolume();
            return true;
        }
        float tempVolume = data.tempVolume;
        float num = data.au.GetMaxRealVolume() / data.fadeTime * 2f;
        tempVolume += num * Time.deltaTime;
        data.au.Volume = tempVolume;
        data.tempVolume = tempVolume;
        if (tempVolume < data.au.GetMaxRealVolume())
        {
            return false;
        }
        data.au.ResetVolume();
        return true;
    }

    public bool FadeOut(VolumeFadeData data)
    {
        if (string.IsNullOrEmpty(data.au.assetName))
        {
            data.au.Volume = 0f;
            return true;
        }
        float tempVolume = data.tempVolume;
        float num = data.au.GetMaxRealVolume() / data.fadeTime;
        tempVolume -= num * Time.deltaTime;
        data.au.Volume = tempVolume;
        data.tempVolume = tempVolume;
        if (tempVolume > 0f)
        {
            return false;
        }
        data.au.Volume = 0f;
        return true;
    }

    public bool FadeOut2In(VolumeFadeData data)
    {
        if (data.fadeState == VolumeFadeStateType.FadeOut)
        {
            if (FadeOut(data))
            {
                data.fadeState = VolumeFadeStateType.Delay;
                if (data.fadeOutCompleteCallBack != null)
                {
                    data.fadeOutCompleteCallBack(data.au);
                }
                return false;
            }
        }
        else if (data.fadeState == VolumeFadeStateType.Delay)
        {
            data.delayTime -= Time.deltaTime;
            if (data.delayTime <= 0f)
            {
                data.fadeState = VolumeFadeStateType.FadeIn;
                return false;
            }
        }
        else if (data.fadeState == VolumeFadeStateType.FadeIn && FadeIn(data))
        {
            data.fadeState = VolumeFadeStateType.Complete;
            return true;
        }
        return false;
    }
}
