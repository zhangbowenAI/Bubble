
public class VolumeFadeData
{
	public AudioAsset au;

	public float fadeTime;

	public float tempVolume;

	public float delayTime;

	public VolumeFadeType fadeType;

	public VolumeFadeStateType fadeState;

	public CallBack<AudioAsset> fadeCompleteCallBack;

	public CallBack<AudioAsset> fadeOutCompleteCallBack;
}
