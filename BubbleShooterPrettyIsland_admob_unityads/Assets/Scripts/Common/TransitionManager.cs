
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : SingletonMonoBehaviour<TransitionManager>
{
	public const string TransitionShaderName = "LightGive/Unlit/TransitionShader";

	public const string ShaderParamTextureGradation = "_Gradation";

	public const string ShaderParamFloatInvert = "_Invert";

	public const string ShaderParamFloatCutoff = "_Cutoff";

	public const string ShaderParamColor = "_Color";

	[SerializeField]
	private float m_defaultFlashDuration = 0.1f;

	[SerializeField]
	private float m_defaultFlashWhiteDuration = 0.05f;

	[SerializeField]
	private Color m_defaultFlashColor = Color.white;

	[SerializeField]
	private float m_defaultTransDuration = 0.08f;

	[SerializeField]
	private Color m_defaultEffectColor = Color.white;

	[SerializeField]
	private AnimationCurve m_animCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	private bool m_isTransition;

	private Sprite m_transitionSprite;

	private Image m_transImage;

	private Image m_transImageFlash;

	private RawImage m_transRawImage;

	private CanvasScaler m_baseCanvasScaler;

	private Canvas m_baseCanvas;

	private bool startLoadingText;

	private int loadingTextIndex;

	public Color defaultFlashColor => m_defaultFlashColor;

	public float defaultFlashDuration => m_defaultFlashDuration;

	public float defaultFlashWhiteDuration => m_defaultFlashWhiteDuration;

	public Color defaultEffectColor => m_defaultEffectColor;

	public float defaultTransDuration => m_defaultTransDuration;

	protected override void Awake()
	{
		isDontDestroy = true;
		base.Awake();
		Init();
	}

	private void Init()
	{
		CreateCanvas();
		CreateRawImage();
		m_transImage = CreateImage();
		m_transImageFlash = CreateImage();
		Texture2D texture = CreateTexture2D();
		m_transitionSprite = Sprite.Create(texture, new Rect(0f, 0f, 32f, 32f), Vector2.zero);
		m_transitionSprite.name = "TransitionSpeite";
		m_transImage.sprite = m_transitionSprite;
		m_transImage.type = Image.Type.Filled;
		m_transImage.fillAmount = 1f;
		m_transImageFlash.gameObject.name = "FlashImage";
		m_transImageFlash.sprite = m_transitionSprite;
		m_transImageFlash.type = Image.Type.Simple;
	}

	private void CreateCanvas()
	{
		if (base.gameObject.GetComponent<Canvas>() != null)
		{
			m_baseCanvas = base.gameObject.GetComponent<Canvas>();
		}
		else
		{
			m_baseCanvas = base.gameObject.AddComponent<Canvas>();
		}
		m_baseCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		m_baseCanvas.sortingOrder = 999;
		m_baseCanvas.pixelPerfect = false;
		if (base.gameObject.GetComponent<CanvasScaler>() != null)
		{
			m_baseCanvasScaler = base.gameObject.GetComponent<CanvasScaler>();
		}
		else
		{
			m_baseCanvasScaler = base.gameObject.AddComponent<CanvasScaler>();
		}
		m_baseCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		m_baseCanvasScaler.referenceResolution = new Vector2(720f, 1280f);
		m_baseCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
		m_baseCanvasScaler.referencePixelsPerUnit = 100f;
	}

	private Image CreateImage()
	{
		GameObject gameObject = new GameObject("Transition Image");
		gameObject.transform.SetParent(base.gameObject.transform);
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.rotation = Quaternion.identity;
		Image image = gameObject.AddComponent<Image>();
		image.color = m_defaultEffectColor;
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.anchorMin = new Vector2(0.5f, 0.5f);
		component.anchorMax = new Vector2(0.5f, 0.5f);
		component.pivot = new Vector2(0.5f, 0.5f);
		component.localPosition = Vector3.zero;
		component.sizeDelta = new Vector2(1000f, 1540f);
		gameObject.SetActive(value: false);
		return image;
	}

	private void CreateRawImage()
	{
		GameObject gameObject = new GameObject("Transition Raw Image");
		gameObject.transform.SetParent(base.gameObject.transform);
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.rotation = Quaternion.identity;
		m_transRawImage = gameObject.AddComponent<RawImage>();
		m_transRawImage.color = m_defaultEffectColor;
		m_transRawImage.texture = null;
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.anchorMin = Vector2.zero;
		component.anchorMax = Vector2.one;
		component.pivot = new Vector2(0.5f, 0.5f);
		component.localPosition = Vector3.zero;
		component.sizeDelta = Vector3.zero;
		m_transRawImage.gameObject.SetActive(value: false);
	}

	private Texture2D CreateTexture2D()
	{
		Texture2D texture2D = new Texture2D(32, 32, TextureFormat.RGB24, false);
		for (int i = 0; i < texture2D.width; i++)
		{
			for (int j = 0; j < texture2D.height; j++)
			{
				texture2D.SetPixel(i, j, Color.white);
			}
		}
		return texture2D;
	}

	public void Update()
	{
		if (startLoadingText)
		{
			loadingTextIndex++;
		}
	}

	private IEnumerator TransitionAction(UnityAction _act, float _transtime, Color _effectColor)
	{
		if (!m_isTransition)
		{
			if (GameManager.Instance != null)
			{
				GameManager.Instance.isExitGame = true;
			}
			startLoadingText = true;
			loadingTextIndex = 1;
			m_isTransition = true;
			SceneTransitionInit(_effectColor);
			float t = Time.time;
			while (Time.time - t < _transtime)
			{
				float lp = m_animCurve.Evaluate((Time.time - t) / _transtime);
				SceneTransitionDirection(lp);
				yield return null;
			}
			if (GameManager.Instance != null)
			{
				GameManager.Instance.ExitGame();
			}
			_act();
			m_isTransition = false;
		}
	}

	private IEnumerator TransitionActionOut(UnityAction _act, float _transtime, Color _effectColor)
	{
		if (!m_isTransition)
		{
			m_transImage.fillAmount = 1f;
			m_isTransition = true;
			float time = Time.time;
			_act();
			float time2 = Time.time;
			yield return new WaitForSeconds(0.1f);
			float t = Time.time;
			m_animCurve = FlipCurve(m_animCurve);
			while (Time.time - t < _transtime)
			{
				float lp = m_animCurve.Evaluate((Time.time - t) / _transtime);
				SceneTransitionDirection(lp);
				yield return null;
			}
			m_animCurve = FlipCurve(m_animCurve);
			m_transImage.fillAmount = 0f;
			m_isTransition = false;
			startLoadingText = false;
			loadingTextIndex = 1;
		}
	}

	private IEnumerator TransitionActionInGame(float _transtime, Color _effectColor)
	{
		if (!m_isTransition)
		{
			m_transImage.gameObject.SetActive(value: true);
			m_transImage.fillAmount = 1f;
			m_isTransition = true;
			SceneTransitionInit(_effectColor);
			float time = Time.time;
			float t = Time.time;
			m_animCurve = FlipCurve(m_animCurve);
			while (Time.time - t < _transtime)
			{
				float lp = m_animCurve.Evaluate((Time.time - t) / _transtime);
				SceneTransitionDirection(lp);
				yield return null;
			}
			m_animCurve = FlipCurve(m_animCurve);
			m_transImage.fillAmount = 0f;
			m_isTransition = false;
		}
	}

	private AnimationCurve FlipCurve(AnimationCurve _curve)
	{
		AnimationCurve animationCurve = new AnimationCurve();
		for (int i = 0; i < _curve.length; i++)
		{
			Keyframe key = _curve[i];
			key.time = 1f - key.time;
			key.inTangent *= -1f;
			key.outTangent *= -1f;
			animationCurve.AddKey(key);
		}
		return animationCurve;
	}

	private void SceneTransitionDirection(float _lerp)
	{
		Image transImage = m_transImage;
		Color color = m_transImage.color;
		float r = color.r;
		Color color2 = m_transImage.color;
		float g = color2.g;
		Color color3 = m_transImage.color;
		transImage.color = new Color(r, g, color3.b, _lerp);
	}

	private void SceneTransitionInit(Color _effectColor)
	{
		m_transImage.fillAmount = 1f;
		m_transImage.color = new Color(_effectColor.r, _effectColor.g, _effectColor.b, 0f);
		m_transImage.gameObject.SetActive(value: true);
	}

	private void SceneTransitionEnd()
	{
		m_transImage.fillAmount = 1f;
	}

	public void LoadSceneIn(string _sceneName, Sprite sprite)
	{
		m_transImage.sprite = sprite;
		m_transImageFlash.sprite = sprite;
		StartCoroutine(TransitionAction(delegate
		{
			SceneManager.LoadScene(_sceneName);
		}, m_defaultTransDuration, defaultEffectColor));
	}

	public void LoadSceneOut(string _sceneName)
	{
		StartCoroutine(TransitionActionOut(delegate
		{
			SceneManager.LoadScene(_sceneName);
		}, m_defaultTransDuration, defaultEffectColor));
	}

	public void InScene(Sprite sprite)
	{
		SceneTransitionInit(Color.white);
		m_transImage.sprite = sprite;
		m_transImageFlash.sprite = sprite;
		StartCoroutine(TransitionActionInGame(0.5f, defaultEffectColor));
	}
}
