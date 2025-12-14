
using System.Collections.Generic;
using UnityEngine;

public class Audio2DPlayer : AudioPlayerBase
{
    public Dictionary<int, AudioAsset> bgMusicDic = new Dictionary<int, AudioAsset>();

    public List<AudioAsset> sfxList = new List<AudioAsset>();

    private List<AudioAsset> clearList = new List<AudioAsset>();

    public Audio2DPlayer(MonoBehaviour mono)
        : base(mono)
    {
    }

    public override void SetMusicVolume(float volume)
    {
        base.SetMusicVolume(volume);
        foreach (AudioAsset value in bgMusicDic.Values)
        {
            value.TotleVolume = volume;
        }
    }

    public override void SetSFXVolume(float volume)
    {
        base.SetSFXVolume(volume);
        for (int i = 0; i < sfxList.Count; i++)
        {
            sfxList[i].TotleVolume = volume;
        }
    }

    public void PlayMusic(int channel, string audioName, bool isLoop = true, float volumeScale = 1f, float delay = 0f, float fadeTime = 0.5f)
    {
        AudioAsset audioAsset;
        if (bgMusicDic.ContainsKey(channel))
        {
            audioAsset = bgMusicDic[channel];
        }
        else
        {
            audioAsset = CreateAudioAsset(mono.gameObject, is3D: false, isMusic: true);
            bgMusicDic.Add(channel, audioAsset);
        }
        PlayMusicControl(audioAsset, audioName, isLoop, volumeScale, delay, fadeTime);
    }

    public void PauseMusic(int channel, bool isPause, float fadeTime = 0.5f)
    {
        if (bgMusicDic.ContainsKey(channel))
        {
            AudioAsset au = bgMusicDic[channel];
            PauseMusicControl(au, isPause, fadeTime);
        }
    }

    public void PauseMusicAll(bool isPause, float fadeTime = 0.5f)
    {
        foreach (int key in bgMusicDic.Keys)
        {
            PauseMusic(key, isPause, fadeTime);
        }
    }

    public void StopMusic(int channel, float fadeTime = 0.5f)
    {
        if (bgMusicDic.ContainsKey(channel))
        {
            StopMusicControl(bgMusicDic[channel], fadeTime);
        }
    }

    public void StopMusicAll()
    {
        foreach (int key in bgMusicDic.Keys)
        {
            StopMusic(key);
        }
    }

    public void PlaySFX(string name, bool nowPlay = false, float volumeScale = 1f, float delay = 0f)
    {
        AudioAsset emptyAudioAssetFromSFXList = GetEmptyAudioAssetFromSFXList();
        PlayClip(emptyAudioAssetFromSFXList, name, isLoop: false, volumeScale, delay, nowPlay);
    }

    public void PauseSFXAll(bool isPause)
    {
        for (int i = 0; i < sfxList.Count; i++)
        {
            if (isPause)
            {
                if (sfxList[i].PlayState == AudioPlayState.Playing)
                {
                    sfxList[i].Pause();
                }
            }
            else if (sfxList[i].PlayState == AudioPlayState.Pause)
            {
                sfxList[i].Play();
            }
        }
    }

    private AudioAsset GetEmptyAudioAssetFromSFXList()
    {
        AudioAsset audioAsset = null;
        if (sfxList.Count > 0)
        {
            for (int i = 0; i < sfxList.Count; i++)
            {
                sfxList[i].CheckState();
                if (sfxList[i].PlayState == AudioPlayState.Stop)
                {
                    audioAsset = sfxList[i];
                    break;
                }
            }
        }
        if (audioAsset == null)
        {
            audioAsset = CreateAudioAsset(mono.gameObject, is3D: false, isMusic: false);
            sfxList.Add(audioAsset);
        }
        return audioAsset;
    }

    public void ClearMoreAudioAsset()
    {
        if (sfxList.Count <= maxSFXAudioAssetNum)
        {
            return;
        }
        for (int i = 0; i < sfxList.Count; i++)
        {
            sfxList[i].CheckState();
            if (sfxList[i].PlayState == AudioPlayState.Stop)
            {
                clearList.Add(sfxList[i]);
            }
        }
        for (int j = 0; j < clearList.Count; j++)
        {
            if (sfxList.Count <= maxSFXAudioAssetNum)
            {
                break;
            }
            UnityEngine.Object.Destroy(clearList[j].audioSource);
            sfxList.Remove(clearList[j]);
        }
        clearList.Clear();
    }
}
