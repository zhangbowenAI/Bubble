
using UnityEngine;

public class AudioAsset
{
    public AudioSource audioSource;

    public string assetName = string.Empty;

    private float totleVolume = 1f;

    private float volumeScale = 1f;

    private AudioPlayState playState = AudioPlayState.Stop;

    public float TotleVolume
    {
        get
        {
            return totleVolume;
        }
        set
        {
            totleVolume = value;
            Volume = TotleVolume * volumeScale;
        }
    }

    public float Volume
    {
        get
        {
            return audioSource.volume;
        }
        set
        {
            audioSource.volume = value;
        }
    }

    public float VolumeScale
    {
        get
        {
            return volumeScale;
        }
        set
        {
            volumeScale = Mathf.Clamp01(value);
            ResetVolume();
        }
    }

    public bool IsPlay => audioSource.isPlaying;

    public AudioPlayState PlayState => playState;

    public void ResetVolume()
    {
        Volume = TotleVolume * volumeScale;
    }

    public float GetMaxRealVolume()
    {
        return TotleVolume * volumeScale;
    }

    public void SetPlayState(AudioPlayState state)
    {
        playState = state;
    }

    public void CheckState()
    {
        if (audioSource == null || (!audioSource.isPlaying && playState != AudioPlayState.Pause))
        {
            playState = AudioPlayState.Stop;
        }
    }

    public void Play(float delay = 0f)
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayDelayed(delay);
            playState = AudioPlayState.Playing;
        }
    }

    public void Pause()
    {
        if (audioSource != null && audioSource.clip != null && audioSource.isPlaying)
        {
            audioSource.Pause();
            playState = AudioPlayState.Pause;
        }
    }

    public void Stop()
    {
        if ((bool)audioSource)
        {
            audioSource.Stop();
        }
        playState = AudioPlayState.Stop;
    }
}
