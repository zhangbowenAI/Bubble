
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio3DPlayer : AudioPlayerBase
{
    public Dictionary<GameObject, Dictionary<int, AudioAsset>> bgMusicDic = new Dictionary<GameObject, Dictionary<int, AudioAsset>>();

    public Dictionary<GameObject, List<AudioAsset>> sfxDic = new Dictionary<GameObject, List<AudioAsset>>();

    private List<AudioAsset> tempClearList = new List<AudioAsset>();

    private List<GameObject> clearList = new List<GameObject>();

    public Audio3DPlayer(MonoBehaviour mono)
        : base(mono)
    {
    }

    public override void SetMusicVolume(float volume)
    {
        base.SetMusicVolume(volume);
        foreach (Dictionary<int, AudioAsset> value in bgMusicDic.Values)
        {
            foreach (AudioAsset value2 in value.Values)
            {
                value2.TotleVolume = volume;
            }
        }
    }

    public override void SetSFXVolume(float volume)
    {
        base.SetSFXVolume(volume);
        foreach (List<AudioAsset> value in sfxDic.Values)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i].TotleVolume = volume;
            }
        }
    }

    public void PlayMusic(GameObject owner, string audioName, int channel = 0, bool isLoop = true, float volumeScale = 1f, float delay = 0f, float fadeTime = 0.5f)
    {
        if (owner == null)
        {
            UnityEngine.Debug.LogError("can not play 3d player, owner is null");
            return;
        }
        Dictionary<int, AudioAsset> dictionary;
        if (bgMusicDic.ContainsKey(owner))
        {
            dictionary = bgMusicDic[owner];
        }
        else
        {
            dictionary = new Dictionary<int, AudioAsset>();
            bgMusicDic.Add(owner, dictionary);
        }
        AudioAsset audioAsset;
        if (dictionary.ContainsKey(channel))
        {
            audioAsset = dictionary[channel];
        }
        else
        {
            audioAsset = CreateAudioAsset(owner, is3D: true, isMusic: true);
            dictionary.Add(channel, audioAsset);
        }
        PlayMusicControl(audioAsset, audioName, isLoop, volumeScale, delay, fadeTime);
    }

    public void PauseMusic(GameObject owner, int channel, bool isPause, float fadeTime = 0.5f)
    {
        if (owner == null)
        {
            Debug.LogError("can not Pause , owner is null");
        }
        else if (bgMusicDic.ContainsKey(owner))
        {
            Dictionary<int, AudioAsset> dictionary = bgMusicDic[owner];
            if (dictionary.ContainsKey(channel))
            {
                AudioAsset au = dictionary[channel];
                PauseMusicControl(au, isPause, fadeTime);
            }
        }
    }

    public void PauseMusicAll(bool isPause, float fadeTime = 0.5f)
    {
        foreach (GameObject key in bgMusicDic.Keys)
        {
            foreach (int key2 in bgMusicDic[key].Keys)
            {
                PauseMusic(key, key2, isPause, fadeTime);
            }
        }
    }

    public void StopMusic(GameObject owner, int channel, float fadeTime = 0.5f)
    {
        if (bgMusicDic.ContainsKey(owner))
        {
            Dictionary<int, AudioAsset> dictionary = bgMusicDic[owner];
            if (dictionary.ContainsKey(channel))
            {
                StopMusicControl(dictionary[channel], fadeTime);
            }
        }
    }

    public void StopMusicOneAll(GameObject owner)
    {
        if (bgMusicDic.ContainsKey(owner))
        {
            List<int> list = new List<int>(bgMusicDic[owner].Keys);
            for (int i = 0; i < list.Count; i++)
            {
                StopMusic(owner, list[i]);
            }
        }
    }

    public void StopMusicAll()
    {
        List<GameObject> list = new List<GameObject>(bgMusicDic.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            StopMusicOneAll(list[i]);
        }
    }

    public void ReleaseMusic(GameObject owner)
    {
        if (bgMusicDic.ContainsKey(owner))
        {
            StopMusicOneAll(owner);
            List<AudioAsset> list = new List<AudioAsset>(bgMusicDic[owner].Values);
            for (int i = 0; i < list.Count; i++)
            {
                UnityEngine.Object.Destroy(list[i].audioSource);
            }
            list.Clear();
        }
        bgMusicDic.Remove(owner);
    }

    public void ReleaseMusicAll()
    {
        List<GameObject> list = new List<GameObject>(sfxDic.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            ReleaseMusic(list[i]);
        }
        bgMusicDic.Clear();
    }

    public void PlaySFX(GameObject owner, string name, float volumeScale = 1f, float delay = 0f)
    {
        AudioAsset emptyAudioAssetFromSFXList = GetEmptyAudioAssetFromSFXList(owner);
        PlayClip(emptyAudioAssetFromSFXList, name, isLoop: false, volumeScale, delay);
        ClearMoreAudioAsset(owner);
    }

    public void PlaySFX(Vector3 position, string name, float volumeScale = 1f, float delay = 0f)
    {
        AudioClip audioClip = GetAudioClip(name);
        if ((bool)audioClip)
        {
            mono.StartCoroutine(PlaySFXIEnumerator(position, audioClip, AudioPlayManager.TotleVolume * AudioPlayManager.SFXVolume * volumeScale, delay));
        }
    }

    private IEnumerator PlaySFXIEnumerator(Vector3 position, AudioClip ac, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioSource.PlayClipAtPoint(ac, position, volume);
    }

    public void PauseSFXAll(bool isPause)
    {
        List<GameObject> list = new List<GameObject>(sfxDic.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            List<AudioAsset> list2 = sfxDic[list[i]];
            for (int j = 0; j < list2.Count; j++)
            {
                if (isPause)
                {
                    if (list2[j].PlayState == AudioPlayState.Playing)
                    {
                        list2[j].Pause();
                    }
                }
                else if (list2[j].PlayState == AudioPlayState.Pause)
                {
                    list2[j].Play();
                }
            }
        }
    }

    public void ReleaseSFX(GameObject owner)
    {
        if ((bool)owner && sfxDic.ContainsKey(owner))
        {
            List<AudioAsset> list = sfxDic[owner];
            for (int i = 0; i < list.Count; i++)
            {
                UnityEngine.Object.Destroy(list[i].audioSource);
            }
            list.Clear();
            sfxDic.Remove(owner);
        }
    }

    public void ReleaseSFXAll()
    {
        List<GameObject> list = new List<GameObject>(sfxDic.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            ReleaseSFX(list[i]);
        }
        sfxDic.Clear();
    }

    private AudioAsset GetEmptyAudioAssetFromSFXList(GameObject owner)
    {
        AudioAsset audioAsset = null;
        List<AudioAsset> list = null;
        if (sfxDic.ContainsKey(owner))
        {
            list = sfxDic[owner];
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].CheckState();
                    if (list[i].PlayState == AudioPlayState.Stop)
                    {
                        audioAsset = list[i];
                        break;
                    }
                }
            }
        }
        else
        {
            list = new List<AudioAsset>();
            sfxDic.Add(owner, list);
        }
        if (audioAsset == null)
        {
            audioAsset = CreateAudioAsset(owner, is3D: true, isMusic: false);
            list.Add(audioAsset);
        }
        return audioAsset;
    }

    private void ClearMoreAudioAsset(GameObject owner)
    {
        if (!sfxDic.ContainsKey(owner))
        {
            return;
        }
        List<AudioAsset> list = sfxDic[owner];
        if (list.Count <= maxSFXAudioAssetNum)
        {
            return;
        }
        for (int i = 0; i < list.Count; i++)
        {
            list[i].CheckState();
            if (list[i].PlayState == AudioPlayState.Stop)
            {
                tempClearList.Add(list[i]);
            }
        }
        for (int j = 0; j < tempClearList.Count; j++)
        {
            if (list.Count <= maxSFXAudioAssetNum)
            {
                break;
            }
            UnityEngine.Object.Destroy(tempClearList[j].audioSource);
            list.Remove(tempClearList[j]);
        }
        tempClearList.Clear();
    }

    public void ClearDestroyObjectData()
    {
        if (bgMusicDic.Count > 0)
        {
            clearList.Clear();
            clearList.AddRange(bgMusicDic.Keys);
            for (int i = 0; i < clearList.Count; i++)
            {
                if (clearList[i] == null)
                {
                    bgMusicDic.Remove(clearList[i]);
                }
            }
        }
        if (sfxDic.Count <= 0)
        {
            return;
        }
        clearList.Clear();
        clearList.AddRange(sfxDic.Keys);
        for (int j = 0; j < clearList.Count; j++)
        {
            if (clearList[j] == null)
            {
                sfxDic.Remove(clearList[j]);
            }
        }
    }
}
