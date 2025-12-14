
using System;
using System.Collections;
using UnityEngine;

namespace HDJ.Framework.Tools
{
	public class CameraFade : MonoBehaviour
	{
		private static CameraFade instance;

		private float alpha;

		private Texture2D crossfadeTexture;

		public bool isFading;

		private Color tempColor;

		public static CameraFade Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new GameObject("[CameraFade]").AddComponent<CameraFade>();
				}
				return instance;
			}
			set
			{
				instance = value;
			}
		}

		public void OnAwake()
		{
			crossfadeTexture = new Texture2D(1, 1, TextureFormat.RGB24, false);
			crossfadeTexture.SetPixel(0, 0, Color.black);
			crossfadeTexture.Apply();
			UnityEngine.Object.DontDestroyOnLoad(this);
		}

		public void FadeIn(float _fadeInTime, CallBack _fun = null)
		{
			if (!isFading)
			{
				StartCoroutine(FadeAction(isFadeIn: true, _fadeInTime, _fun));
			}
		}

		public void FadeOut(float _fadeOutTime, CallBack _fun = null)
		{
			if (!isFading)
			{
				StartCoroutine(FadeAction(isFadeIn: false, _fadeOutTime, _fun));
			}
		}

		public void FadeInToOut(float _fadeInTime, float afterInDelayTime, float _fadeOutTime, CallBack afterFadeInCallback = null, CallBack afterFadeOutCallback = null)
		{
			if (!isFading)
			{
				StartCoroutine(FadeInToOutAction(_fadeInTime, afterInDelayTime, _fadeOutTime, afterFadeInCallback, afterFadeOutCallback));
			}
		}

		private IEnumerator FadeInToOutAction(float _fadeInTime, float afterInDelayTime, float _fadeOutTime, CallBack afterFadeInCallback = null, CallBack afterFadeOutCallback = null)
		{
			yield return StartCoroutine(FadeAction(isFadeIn: true, _fadeInTime, afterFadeInCallback));
			isFading = true;
			yield return new WaitForSeconds(afterInDelayTime);
			yield return StartCoroutine(FadeAction(isFadeIn: false, _fadeOutTime, afterFadeOutCallback));
		}

		private IEnumerator FadeAction(bool isFadeIn, float fadeTime, CallBack _fun)
		{
			isFading = true;
			tempColor = GUI.color;
			GUI.depth = 100;
			if (isFadeIn)
			{
				alpha = 0f;
			}
			else
			{
				alpha = 1f;
			}
			float tempTime = fadeTime + Time.unscaledTime;
			while (true)
			{
				if (!isFadeIn)
				{
					alpha = (tempTime - Time.unscaledTime) / fadeTime;
					if (alpha < 0.05f)
					{
						alpha = 0f;
						break;
					}
				}
				else
				{
					alpha = Mathf.Clamp(1f - (tempTime - Time.unscaledTime) / fadeTime, 0f, 1f);
					if (alpha >= 0.98f)
					{
						alpha = 1f;
						break;
					}
				}
				yield return new WaitForEndOfFrame();
			}
			isFading = false;
			try
			{
				_fun?.Invoke();
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("Camera Fade Call back Exception :" + arg);
			}
		}

		private void OnGUI()
		{
			if (!(alpha <= 0f))
			{
				tempColor.a = alpha;
				GUI.color = tempColor;
				if (crossfadeTexture != null)
				{
					GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), crossfadeTexture, ScaleMode.StretchToFill);
				}
			}
		}
	}
}
