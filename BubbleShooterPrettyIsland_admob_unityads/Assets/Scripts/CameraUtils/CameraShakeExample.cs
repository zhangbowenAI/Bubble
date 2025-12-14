
using System.Collections;
using Thinksquirrel.CShake;
using UnityEngine;

namespace Thinksquirrel.CShakeExample
{
	[AddComponentMenu("Camera Shake Example/Camera Shake Example")]
	[RequireComponent(typeof(CameraShake))]
	public class CameraShakeExample : MonoBehaviour
	{
		public bool is2DExample;

		public float rigidbodyForceModifier = 2f;

		public float rigidbodyTorqueModifier = 0.2f;

		private CanvasGroup m_Canvas;

		private CameraShake m_Shake;

		private bool m_ShakeEverything;

		private Rigidbody[] m_Rigidbodies;

		private Rigidbody2D[] m_Rigidbodies2D;

		private bool m_MultiShake;

		private void Start()
		{
			m_Canvas = UnityEngine.Object.FindObjectOfType<CanvasGroup>();
			m_Shake = GetComponent<CameraShake>();
			m_Shake.onShakeOffset += OnShakeOffset;
			m_Rigidbodies = UnityEngine.Object.FindObjectsOfType<Rigidbody>();
			m_Rigidbodies2D = UnityEngine.Object.FindObjectsOfType<Rigidbody2D>();
		}

		private void Update()
		{
			if ((bool)m_Canvas)
			{
				m_Canvas.interactable = (!m_Shake.IsShaking() && !m_MultiShake);
			}
			if (m_Shake.shakeType == CameraShake.ShakeType.CameraMatrix)
			{
				Transform parent = base.transform.parent;
				Vector3 position = base.transform.parent.position;
				float x = position.x;
				float y = Mathf.Cos(Time.time) * 0.35f;
				Vector3 position2 = base.transform.parent.position;
				parent.position = new Vector3(x, y, position2.z);
				Transform transform = base.transform;
				float x2 = Mathf.Sin(Time.time) * 0.5f;
				Vector3 localPosition = base.transform.localPosition;
				float y2 = localPosition.y;
				Vector3 localPosition2 = base.transform.localPosition;
				transform.localPosition = new Vector3(x2, y2, localPosition2.z);
			}
			else
			{
				Transform parent2 = base.transform.parent;
				float x3 = Mathf.Sin(Time.time) * 0.5f;
				float y3 = Mathf.Cos(Time.time) * 0.35f;
				Vector3 position3 = base.transform.parent.position;
				parent2.position = new Vector3(x3, y3, position3.z);
			}
		}

		private void OnShakeOffset(Vector3 translation, Quaternion rotation)
		{
			if (!m_ShakeEverything)
			{
				return;
			}
			if (is2DExample)
			{
				Rigidbody2D[] rigidbodies2D = m_Rigidbodies2D;
				foreach (Rigidbody2D rigidbody2D in rigidbodies2D)
				{
					if ((bool)rigidbody2D)
					{
						rigidbody2D.AddForce(translation * rigidbodyForceModifier, ForceMode2D.Impulse);
						Vector3 eulerAngles = base.transform.eulerAngles;
						Quaternion a = Quaternion.Euler(0f, 0f, eulerAngles.z);
						Vector3 eulerAngles2 = rotation.eulerAngles;
						Quaternion b = Quaternion.Euler(0f, 0f, eulerAngles2.z);
						rigidbody2D.AddTorque(Quaternion.Angle(a, b) * rigidbodyTorqueModifier, ForceMode2D.Impulse);
					}
				}
				return;
			}
			Rigidbody[] rigidbodies = m_Rigidbodies;
			Vector3 a3 = default(Vector3);
			foreach (Rigidbody rigidbody in rigidbodies)
			{
				if ((bool)rigidbody)
				{
					rigidbody.AddForce(translation * rigidbodyForceModifier, ForceMode.Impulse);
					Vector3 eulerAngles3 = base.transform.eulerAngles;
					Quaternion a2 = Quaternion.Euler(eulerAngles3.x, 0f, 0f);
					Vector3 eulerAngles4 = rotation.eulerAngles;
					Quaternion b2 = Quaternion.Euler(eulerAngles4.x, 0f, 0f);
					a3.x = Quaternion.Angle(a2, b2);
					Vector3 eulerAngles5 = base.transform.eulerAngles;
					a2 = Quaternion.Euler(0f, eulerAngles5.y, 0f);
					Vector3 eulerAngles6 = base.transform.eulerAngles;
					b2 = Quaternion.Euler(0f, eulerAngles6.y, 0f);
					a3.y = Quaternion.Angle(a2, b2);
					Vector3 eulerAngles7 = base.transform.eulerAngles;
					a2 = Quaternion.Euler(0f, 0f, eulerAngles7.z);
					Vector3 eulerAngles8 = rotation.eulerAngles;
					b2 = Quaternion.Euler(0f, 0f, eulerAngles8.z);
					a3.z = Quaternion.Angle(a2, b2);
					rigidbody.AddTorque(a3 * rigidbodyTorqueModifier, ForceMode.Impulse);
				}
			}
		}

		private void Explosion()
		{
			m_MultiShake = true;
			m_ShakeEverything = true;
			Vector3 one = Vector3.one;
			Vector3 rotationAmount = new Vector3(0.2f, 0.05f, 1f);
			if (is2DExample)
			{
				one.z = 0f;
				rotationAmount.x = 0f;
				rotationAmount.y = 0f;
			}
			m_Shake.Shake(m_Shake.shakeType, 5, one, rotationAmount, 0.25f, 50f, 0.2f, 1f, multiplyByTimeScale: true, delegate
			{
				m_MultiShake = false;
			});
		}

		private void Footsteps()
		{
			m_ShakeEverything = true;
			m_MultiShake = true;
			StartCoroutine(DoFootsteps());
		}

		private IEnumerator DoFootsteps()
		{
			Vector3 shake = Vector3.one;
			Vector3 rot = new Vector3(0.2f, 0.05f, 0.1f);
			if (is2DExample)
			{
				shake.z = 0f;
				rot.x = 0f;
				rot.y = 0f;
			}
			m_Shake.Shake(m_Shake.shakeType, 3, shake, rot, 0.02f, 50f, 0.5f, 1f, multiplyByTimeScale: true, null);
			yield return new WaitForSeconds(1f);
			m_Shake.Shake(m_Shake.shakeType, 3, shake, rot, 0.03f, 50f, 0.5f, 1f, multiplyByTimeScale: true, null);
			yield return new WaitForSeconds(1f);
			m_Shake.Shake(m_Shake.shakeType, 3, shake, rot * 1.5f, 0.04f, 50f, 0.5f, 1f, multiplyByTimeScale: true, null);
			yield return new WaitForSeconds(1f);
			m_Shake.Shake(m_Shake.shakeType, 3, shake, rot * 2f, 0.05f, 50f, 0.5f, 1f, multiplyByTimeScale: true, null);
			yield return new WaitForSeconds(1f);
			m_Shake.Shake(m_Shake.shakeType, 3, shake, rot * 2.5f, 0.06f, 50f, 0.5f, 1f, multiplyByTimeScale: true, delegate
			{
				m_MultiShake = false;
			});
		}

		private void Earthquake()
		{
			m_ShakeEverything = true;
			m_MultiShake = true;
			StartCoroutine(DoEarthquake());
			StartCoroutine(DoEarthquake2());
		}

		private IEnumerator DoEarthquake()
		{
			Vector3 shake = Vector3.one;
			Vector3 rot = new Vector3(0.2f, 0.2f, 0.2f);
			if (is2DExample)
			{
				shake.z = 0f;
				rot.x = 0f;
				rot.y = 0f;
			}
			for (int i = 0; i < 50; i++)
			{
				m_Shake.Shake(m_Shake.shakeType, 3, shake, rot, 0.02f, 50f, 0f, 1f, multiplyByTimeScale: true, null);
				yield return new WaitForSeconds(0.1f);
			}
			m_Shake.Shake(m_Shake.shakeType, 3, shake, rot, 0.02f, 50f, 0f, 1f, multiplyByTimeScale: true, delegate
			{
				m_MultiShake = false;
			});
		}

		private IEnumerator DoEarthquake2()
		{
			Vector3 rot = new Vector3(0.8f, 0.1f, 0.4f);
			if (is2DExample)
			{
				rot.x = 0f;
				rot.y = 0f;
			}
			for (int i = 0; i < 5; i++)
			{
				yield return new WaitForSeconds(1f);
				m_Shake.Shake(m_Shake.shakeType, 10, Vector3.up, rot, 0.5f, 50f, 0.2f, 1f, multiplyByTimeScale: true, null);
			}
		}

		public void OnShakeCamera()
		{
			m_ShakeEverything = false;
			m_Shake.Shake();
		}

		public void OnShakeEverything()
		{
			m_ShakeEverything = true;
			m_Shake.Shake();
		}

		public void OnNumberOfShakesSlider(float val)
		{
			m_Shake.numberOfShakes = (int)val;
		}

		public void OnShakeAmountXSlider(float val)
		{
			Vector3 shakeAmount = m_Shake.shakeAmount;
			shakeAmount.x = val;
			m_Shake.shakeAmount = shakeAmount;
		}

		public void OnShakeAmountYSlider(float val)
		{
			Vector3 shakeAmount = m_Shake.shakeAmount;
			shakeAmount.y = val;
			m_Shake.shakeAmount = shakeAmount;
		}

		public void OnShakeAmountZSlider(float val)
		{
			Vector3 shakeAmount = m_Shake.shakeAmount;
			shakeAmount.z = val;
			m_Shake.shakeAmount = shakeAmount;
		}

		public void OnRotationAmountXSlider(float val)
		{
			Vector3 rotationAmount = m_Shake.rotationAmount;
			rotationAmount.x = val;
			m_Shake.rotationAmount = rotationAmount;
		}

		public void OnRotationAmountYSlider(float val)
		{
			Vector3 rotationAmount = m_Shake.rotationAmount;
			rotationAmount.y = val;
			m_Shake.rotationAmount = rotationAmount;
		}

		public void OnRotationAmountZSlider(float val)
		{
			Vector3 rotationAmount = m_Shake.rotationAmount;
			rotationAmount.z = val;
			m_Shake.rotationAmount = rotationAmount;
		}

		public void OnDistanceSlider(float val)
		{
			m_Shake.distance = val;
		}

		public void OnSpeedSlider(float val)
		{
			m_Shake.distance = val;
		}

		public void OnDecaySlider(float val)
		{
			m_Shake.decay = val;
		}

		public void OnPresetExplosion()
		{
			Explosion();
		}

		public void OnPresetFootsteps()
		{
			Footsteps();
		}

		public void OnPresetEarthquake()
		{
			Earthquake();
		}
	}
}
