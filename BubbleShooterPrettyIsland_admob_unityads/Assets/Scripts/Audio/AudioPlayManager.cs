
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayManager : MonoBehaviour
{
    public static Audio2DPlayer a2DPlayer;

    public static Audio3DPlayer a3DPlayer;

    public static Dictionary<string, int> dPlayeTime = new Dictionary<string, int>();

    public static Dictionary<string, AudioClip> dClips = new Dictionary<string, AudioClip>();

    private static float totleVolume = 1f;

    private static float musicVolume = 1f;

    private static float sfxVolume = 1f;

    private float runTimeCount;

    public static float TotleVolume
    {
        get
        {
            return totleVolume;
        }
        set
        {
            totleVolume = Mathf.Clamp01(value);
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public static float MusicVolume
    {
        get
        {
            return musicVolume;
        }
        set
        {
            musicVolume = Mathf.Clamp01(value);
            SetMusicVolume();
        }
    }

    public static float SFXVolume
    {
        get
        {
            return sfxVolume;
        }
        set
        {
            sfxVolume = Mathf.Clamp01(value);
            SetSFXVolume();
        }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        GameObject gameObject = new GameObject("[AudioManager]");
        AudioPlayManager mono = gameObject.AddComponent<AudioPlayManager>();
        Object.DontDestroyOnLoad(gameObject);
        a2DPlayer = new Audio2DPlayer(mono);
        a3DPlayer = new Audio3DPlayer(mono);
        TotleVolume = RecordManager.GetFloatRecord("GameSettingData", "TotleVolume", 1f);
        MusicVolume = RecordManager.GetFloatRecord("GameSettingData", "MusicVolume", 1f);
        SFXVolume = RecordManager.GetFloatRecord("GameSettingData", "SFXVolume", 1f);
    }

    private static void SetMusicVolume()
    {
        a2DPlayer.SetMusicVolume(totleVolume * musicVolume);
        a3DPlayer.SetMusicVolume(totleVolume * musicVolume);
    }

    private static void SetSFXVolume()
    {
        a2DPlayer.SetSFXVolume(totleVolume * sfxVolume);
        a3DPlayer.SetSFXVolume(totleVolume * sfxVolume);
    }

    public static void SaveVolume()
    {
        RecordManager.SaveRecord("GameSettingData", "TotleVolume", TotleVolume);
        RecordManager.SaveRecord("GameSettingData", "MusicVolume", MusicVolume);
        RecordManager.SaveRecord("GameSettingData", "SFXVolume", SFXVolume);
    }

    public static void PlayMusic2D(string name, int channel, float volumeScale = 1f, bool isLoop = true, float fadeTime = 0.5f, float delay = 0f)
    {
        a2DPlayer.PlayMusic(channel, name, isLoop, volumeScale, delay, fadeTime);
    }

    public static void PauseMusic2D(int channel, bool isPause, float fadeTime = 0.5f)
    {
        a2DPlayer.PauseMusic(channel, isPause, fadeTime);
    }

    public static void PauseMusicAll2D(bool isPause, float fadeTime = 0.5f)
    {
        a2DPlayer.PauseMusicAll(isPause, fadeTime);
    }

    public static void StopMusic2D(int channel, float fadeTime = 0.5f)
    {
        a2DPlayer.StopMusic(channel, fadeTime);
    }

    public static void StopMusicAll2D()
    {
        a2DPlayer.StopMusicAll();
    }

    public static void PlaySFX2D(string name, bool nowPlay = false, float volumeScale = 1f)
    {
        a2DPlayer.PlaySFX(name, nowPlay, volumeScale);
    }

    public static void PauseSFXAll2D(bool isPause)
    {
        a2DPlayer.PauseSFXAll(isPause);
    }

    public static void LoadMusic(string name)
    {
        a2DPlayer.GetAudioClip(name);
    }

    public static void PlayMusic3D(GameObject owner, string audioName, int channel = 0, float volumeScale = 1f, bool isLoop = true, float fadeTime = 0.5f, float delay = 0f)
    {
        a3DPlayer.PlayMusic(owner, audioName, channel, isLoop, volumeScale, delay, fadeTime);
    }

    public static void PauseMusic3D(GameObject owner, int channel, bool isPause, float fadeTime = 0.5f)
    {
        a3DPlayer.PauseMusic(owner, channel, isPause, fadeTime);
    }

    public static void PauseMusicAll3D(bool isPause, float fadeTime = 0.5f)
    {
        a3DPlayer.PauseMusicAll(isPause, fadeTime);
    }

    public static void StopMusic3D(GameObject owner, int channel, float fadeTime = 0.5f)
    {
        a3DPlayer.StopMusic(owner, channel, fadeTime);
    }

    public static void StopMusicOneAll3D(GameObject owner)
    {
        a3DPlayer.StopMusicOneAll(owner);
    }

    public static void StopMusicAll3D()
    {
        a3DPlayer.StopMusicAll();
    }

    public static void ReleaseMusic3D(GameObject owner)
    {
        a3DPlayer.ReleaseMusic(owner);
    }

    public static void ReleaseMusicAll3D()
    {
        a3DPlayer.ReleaseMusicAll();
    }

    public static void PlaySFX3D(GameObject owner, string name, float delay = 0f, float volumeScale = 1f)
    {
        a3DPlayer.PlaySFX(owner, name, volumeScale, delay);
    }

    public static void PlaySFX3D(Vector3 position, string name, float delay = 0f, float volumeScale = 1f)
    {
        a3DPlayer.PlaySFX(position, name, volumeScale, delay);
    }

    public static void PauseSFXAll3D(bool isPause)
    {
        a3DPlayer.PauseSFXAll(isPause);
    }

    public static void ReleaseSFX3D(GameObject owner)
    {
        a3DPlayer.ReleaseSFX(owner);
    }

    public static void ReleaseSFXAll3D()
    {
        a3DPlayer.ReleaseSFXAll();
    }

    public void Update()
    {
        if (a2DPlayer == null)
        {
            AudioPlayManager component = GetComponent<AudioPlayManager>();
            a2DPlayer = new Audio2DPlayer(component);
        }
        if (a3DPlayer == null)
        {
            AudioPlayManager component2 = GetComponent<AudioPlayManager>();
            a3DPlayer = new Audio3DPlayer(component2);
        }
        if (runTimeCount <= 0f)
        {
            runTimeCount = 4f;
            a3DPlayer.ClearDestroyObjectData();
            a2DPlayer.ClearMoreAudioAsset();
        }
        else
        {
            runTimeCount -= Time.unscaledDeltaTime;
        }
        a2DPlayer.UpdateFade();
        a3DPlayer.UpdateFade();
    }
}
