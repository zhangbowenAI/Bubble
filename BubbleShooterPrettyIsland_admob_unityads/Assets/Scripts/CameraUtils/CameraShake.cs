
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Thinksquirrel.CShake
{
	[AddComponentMenu("Camera Shake/Camera Shake")]
	public class CameraShake : CameraShakeBase
	{
		public enum ShakeType
		{
			CameraMatrix,
			LocalPosition
		}

		internal class ShakeState
		{
			internal readonly ShakeType _shakeType;

			internal readonly Vector3 _startPosition;

			internal readonly Quaternion _startRotation;

			internal readonly Vector2 _uiStartPosition;

			internal Vector3 _shakePosition;

			internal Quaternion _shakeRotation;

			internal Vector2 _uiShakePosition;

			internal ShakeState(ShakeType shakeType, Vector3 position, Quaternion rotation, Vector2 uiPosition)
			{
				_shakeType = shakeType;
				_startPosition = position;
				_startRotation = rotation;
				_uiStartPosition = uiPosition;
				_shakePosition = position;
				_shakeRotation = rotation;
				_uiShakePosition = uiPosition;
			}
		}

		[SerializeField]
		private List<Camera> m_Cameras = new List<Camera>();

		[SerializeField]
		private ShakeType m_ShakeType;

		[SerializeField]
		private int m_NumberOfShakes = 2;

		[SerializeField]
		private Vector3 m_ShakeAmount = Vector3.one;

		[SerializeField]
		private Vector3 m_RotationAmount = Vector3.one;

		[SerializeField]
		private float m_Distance = 0.1f;

		[SerializeField]
		private float m_Speed = 50f;

		[SerializeField]
		private float m_Decay = 0.2f;

		[SerializeField]
		private float m_UiShakeModifier = 1f;

		[SerializeField]
		private bool m_MultiplyByTimeScale = true;

		private Rect m_ShakeRect;

		private bool m_Shaking;

		private bool m_Cancelling;

		private readonly List<Vector3> m_MatrixOffsetCache = new List<Vector3>(10);

		private readonly List<Quaternion> m_MatrixRotationCache = new List<Quaternion>(10);

		private readonly List<Vector3> m_OffsetCache = new List<Vector3>(10);

		private readonly List<Quaternion> m_RotationCache = new List<Quaternion>(10);

		private readonly List<bool> m_IgnoreMatrixCache = new List<bool>(10);

		private readonly List<bool> m_IgnorePositionCache = new List<bool>(10);

		private readonly Dictionary<Camera, List<ShakeState>> m_States = new Dictionary<Camera, List<ShakeState>>();

		private readonly Dictionary<Camera, int> m_ShakeCount = new Dictionary<Camera, int>();

		private const float minShakeValue = 0.001f;

		private const float minRotationValue = 0.001f;

		private static readonly List<CameraShake> m_Components = new List<CameraShake>();

		public List<Camera> cameras
		{
			get
			{
				return m_Cameras;
			}
			set
			{
				m_Cameras = value;
			}
		}

		public ShakeType shakeType
		{
			get
			{
				return m_ShakeType;
			}
			set
			{
				m_ShakeType = value;
			}
		}

		public int numberOfShakes
		{
			get
			{
				return m_NumberOfShakes;
			}
			set
			{
				m_NumberOfShakes = value;
			}
		}

		public Vector3 shakeAmount
		{
			get
			{
				return m_ShakeAmount;
			}
			set
			{
				m_ShakeAmount = value;
			}
		}

		public Vector3 rotationAmount
		{
			get
			{
				return m_RotationAmount;
			}
			set
			{
				m_RotationAmount = value;
			}
		}

		public float distance
		{
			get
			{
				return m_Distance;
			}
			set
			{
				m_Distance = value;
			}
		}

		public float speed
		{
			get
			{
				return m_Speed;
			}
			set
			{
				m_Speed = value;
			}
		}

		public float decay
		{
			get
			{
				return m_Decay;
			}
			set
			{
				m_Decay = value;
			}
		}

		[Obsolete("Use CameraShake.uiShakeModifier instead")]
		public float guiShakeModifier
		{
			get
			{
				return uiShakeModifier;
			}
			set
			{
				uiShakeModifier = value;
			}
		}

		public float uiShakeModifier
		{
			get
			{
				return m_UiShakeModifier;
			}
			set
			{
				m_UiShakeModifier = value;
			}
		}

		public bool multiplyByTimeScale
		{
			get
			{
				return m_MultiplyByTimeScale;
			}
			set
			{
				m_MultiplyByTimeScale = value;
			}
		}

		public Rect uiShakeRect
		{
			get
			{
				return m_ShakeRect;
			}
			set
			{
				m_ShakeRect = value;
			}
		}

		[Obsolete("Use CameraShake.GetComponents() to get all Camera Shake components")]
		public static CameraShake instance => (m_Components.Count <= 0) ? null : m_Components[0];

		[Obsolete("Use IsShaking method on individual Camera Shake components")]
		public static bool isShaking
		{
			get
			{
				int i = 0;
				for (int count = m_Components.Count; i < count; i++)
				{
					CameraShake cameraShake = m_Components[i];
					if (cameraShake.IsShaking())
					{
						return true;
					}
				}
				return false;
			}
		}

		[Obsolete("Use IsCancelling method on individual Camera Shake components")]
		public static bool isCancelling
		{
			get
			{
				CameraShake cameraShake = (m_Components.Count <= 0) ? null : m_Components[0];
				return cameraShake != null && cameraShake.IsCancelling();
			}
		}

		[Obsolete("Use CameraShake.onStartShaking instead")]
		public event Action cameraShakeStarted;

		[Obsolete("Use CameraShake.onEndShaking")]
		public event Action allCameraShakesCompleted;

		public event Action onStartShaking;

		public event Action onEndShaking;

		public event Action onPreShake;

		public event Action onPostShake;

		public event Action<Vector3, Quaternion> onShakeOffset;

		private void OnEnable()
		{
			if (cameras.Count < 1 && (bool)GetComponent<Camera>())
			{
				cameras.Add(GetComponent<Camera>());
			}
			if (cameras.Count < 1 && (bool)Camera.main)
			{
				cameras.Add(Camera.main);
			}
			if (cameras.Count < 1)
			{
				CameraShakeBase.LogError("No cameras assigned in the inspector!", "Camera Shake", "CameraShake", this);
			}
			m_Components.Add(this);
		}

		private void OnDisable()
		{
			m_Components.Remove(this);
		}

		public static CameraShake[] GetComponents()
		{
			return m_Components.ToArray();
		}

		public static void ShakeAll()
		{
			int i = 0;
			for (int count = m_Components.Count; i < count; i++)
			{
				CameraShake cameraShake = m_Components[i];
				cameraShake.Shake();
			}
		}

		public static void ShakeAll(ShakeType shakeType, int numberOfShakes, Vector3 shakeAmount, Vector3 rotationAmount, float distance, float speed, float decay, float uiShakeModifier, bool multiplyByTimeScale)
		{
			int i = 0;
			for (int count = m_Components.Count; i < count; i++)
			{
				CameraShake cameraShake = m_Components[i];
				cameraShake.Shake(shakeType, numberOfShakes, shakeAmount, rotationAmount, distance, speed, decay, uiShakeModifier, multiplyByTimeScale);
			}
		}

		public static void ShakeAll(Action callback)
		{
			int i = 0;
			for (int count = m_Components.Count; i < count; i++)
			{
				CameraShake cameraShake = m_Components[i];
				cameraShake.Shake(callback);
			}
		}

		public static void ShakeAll(ShakeType shakeType, int numberOfShakes, Vector3 shakeAmount, Vector3 rotationAmount, float distance, float speed, float decay, float uiShakeModifier, bool multiplyByTimeScale, Action callback)
		{
			int i = 0;
			for (int count = m_Components.Count; i < count; i++)
			{
				CameraShake cameraShake = m_Components[i];
				cameraShake.Shake(shakeType, numberOfShakes, shakeAmount, rotationAmount, distance, speed, decay, uiShakeModifier, multiplyByTimeScale, callback);
			}
		}

		public static void CancelAllShakes()
		{
			int i = 0;
			for (int count = m_Components.Count; i < count; i++)
			{
				CameraShake cameraShake = m_Components[i];
				cameraShake.CancelShake();
			}
		}

		public static void CancelAllShakes(float time)
		{
			int i = 0;
			for (int count = m_Components.Count; i < count; i++)
			{
				CameraShake cameraShake = m_Components[i];
				cameraShake.CancelShake(time);
			}
		}

		public bool IsShaking()
		{
			return m_Shaking;
		}

		public bool IsCancelling()
		{
			return m_Cancelling;
		}

		public void Shake()
		{
			Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
			int i = 0;
			for (int count = cameras.Count; i < count; i++)
			{
				Camera cam = cameras[i];
				StartCoroutine(DoShake_Internal(cam, insideUnitSphere, shakeType, numberOfShakes, shakeAmount, rotationAmount, distance, speed, decay, uiShakeModifier, multiplyByTimeScale, null));
			}
		}

		public void Shake(ShakeType shakeType, int numberOfShakes, Vector3 shakeAmount, Vector3 rotationAmount, float distance, float speed, float decay, float uiShakeModifier, bool multiplyByTimeScale)
		{
			Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
			int i = 0;
			for (int count = cameras.Count; i < count; i++)
			{
				Camera cam = cameras[i];
				StartCoroutine(DoShake_Internal(cam, insideUnitSphere, shakeType, numberOfShakes, shakeAmount, rotationAmount, distance, speed, decay, uiShakeModifier, multiplyByTimeScale, null));
			}
		}

		public void Shake(Action callback)
		{
			Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
			int i = 0;
			for (int count = cameras.Count; i < count; i++)
			{
				Camera cam = cameras[i];
				StartCoroutine(DoShake_Internal(cam, insideUnitSphere, shakeType, numberOfShakes, shakeAmount, rotationAmount, distance, speed, decay, uiShakeModifier, multiplyByTimeScale, callback));
			}
		}

		public void Shake(ShakeType shakeType, int numberOfShakes, Vector3 shakeAmount, Vector3 rotationAmount, float distance, float speed, float decay, float uiShakeModifier, bool multiplyByTimeScale, Action callback)
		{
			Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
			int i = 0;
			for (int count = cameras.Count; i < count; i++)
			{
				Camera cam = cameras[i];
				StartCoroutine(DoShake_Internal(cam, insideUnitSphere, shakeType, numberOfShakes, shakeAmount, rotationAmount, distance, speed, decay, uiShakeModifier, multiplyByTimeScale, callback));
			}
		}

		public void CancelShake()
		{
			if (!m_Shaking || m_Cancelling)
			{
				return;
			}
			m_Shaking = false;
			StopAllCoroutines();
			int i = 0;
			for (int count = cameras.Count; i < count; i++)
			{
				Camera camera = cameras[i];
				if (m_ShakeCount.ContainsKey(camera))
				{
					m_ShakeCount[camera] = 0;
				}
				ResetState(camera);
			}
		}

		public void CancelShake(float time)
		{
			if (m_Shaking && !m_Cancelling)
			{
				StopAllCoroutines();
				StartCoroutine(DoResetState(cameras, m_ShakeCount, time));
			}
		}

		public void BeginShakeGUI()
		{
			CheckShakeRect();
			GUI.BeginGroup(m_ShakeRect);
		}

		public void EndShakeGUI()
		{
			GUI.EndGroup();
		}

		public void BeginShakeGUILayout()
		{
			CheckShakeRect();
			GUILayout.BeginArea(m_ShakeRect);
		}

		public void EndShakeGUILayout()
		{
			GUILayout.EndArea();
		}

		private void OnDrawGizmosSelected()
		{
			int i = 0;
			for (int count = cameras.Count; i < count; i++)
			{
				Camera camera = cameras[i];
				if ((bool)camera)
				{
					if (!IsShaking())
					{
						Matrix4x4 matrix4x2 = Gizmos.matrix = Matrix4x4.TRS(camera.transform.position, camera.transform.rotation, camera.transform.lossyScale);
					}
					else
					{
						Vector4 v = camera.worldToCameraMatrix.GetColumn(3);
						v.z *= -1f;
						v = camera.transform.position + camera.transform.TransformPoint(v);
						Quaternion q = QuaternionFromMatrix(camera.worldToCameraMatrix.inverse * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, 1f, -1f)));
						Matrix4x4 matrix4x4 = Gizmos.matrix = Matrix4x4.TRS(v, q, camera.transform.lossyScale);
					}
					Gizmos.DrawWireCube(Vector3.zero, shakeAmount);
					Gizmos.color = Color.cyan;
					if (camera.orthographic)
					{
						Vector3 center = new Vector3(0f, 0f, (camera.nearClipPlane + camera.farClipPlane) / 2f);
						Vector3 size = new Vector3(camera.orthographicSize / camera.aspect, camera.orthographicSize * 2f, camera.farClipPlane - camera.nearClipPlane);
						Gizmos.DrawWireCube(center, size);
					}
					else
					{
						Gizmos.DrawFrustum(Vector3.zero, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, 0.7f / camera.aspect);
					}
				}
			}
		}

		private IEnumerator DoShake_Internal(Camera cam, Vector3 seed, ShakeType shakeType, int numberOfShakes, Vector3 shakeAmount, Vector3 rotationAmount, float distance, float speed, float decay, float uiShakeModifier, bool multiplyByTimeScale, Action callback)
		{
			if (m_Cancelling)
			{
				yield return null;
			}
			int mod = (seed.x > 0.5f) ? 1 : (-1);
			int mod2 = (seed.y > 0.5f) ? 1 : (-1);
			int mod3 = (seed.z > 0.5f) ? 1 : (-1);
			if (!m_Shaking)
			{
				m_Shaking = true;
				if (this.onStartShaking != null)
				{
					this.onStartShaking();
				}
			}
			Dictionary<Camera, int> shakeCount;
			if (m_ShakeCount.ContainsKey(cam))
			{
				Camera key;
				(shakeCount = m_ShakeCount)[key = cam] = shakeCount[key] + 1;
			}
			else
			{
				m_ShakeCount.Add(cam, 1);
			}
			if (this.onPreShake != null)
			{
				this.onPreShake();
			}
			float pixelWidth = GetPixelWidth(cameras[0].transform, cameras[0]);
			Transform cachedTransform = cam.transform;
			int currentShakes = numberOfShakes;
			float shakeDistance = distance;
			float rotationStrength = 1f;
			float startTime = Time.time;
			float scale = (!multiplyByTimeScale) ? 1f : Time.timeScale;
			float pixelScale = pixelWidth * uiShakeModifier * scale;
			Vector3 start3 = Vector2.zero;
			Quaternion startR = Quaternion.identity;
			Vector2 start2 = Vector2.zero;
			ShakeState state = new ShakeState(shakeType, cachedTransform.position, cachedTransform.rotation, new Vector2(m_ShakeRect.x, m_ShakeRect.y));
			if (m_States.TryGetValue(cam, out List<ShakeState> stateList))
			{
				stateList.Add(state);
			}
			else
			{
				stateList = new List<ShakeState>
				{
					state
				};
				m_States.Add(cam, stateList);
			}
			while (currentShakes > 0 && (!(Mathf.Abs(rotationAmount.sqrMagnitude) > 1E-05f) || !(rotationStrength <= 0.001f)) && (!(Mathf.Abs(shakeAmount.sqrMagnitude) > 1E-05f) || !(Mathf.Abs(distance) > 1E-05f) || !(shakeDistance <= 0.001f)))
			{
				float timer = (Time.time - startTime) * speed;
				state._shakePosition = start3 + new Vector3((float)mod * Mathf.Sin(timer) * (shakeAmount.x * shakeDistance * scale), (float)mod2 * Mathf.Cos(timer) * (shakeAmount.y * shakeDistance * scale), (float)mod3 * Mathf.Sin(timer) * (shakeAmount.z * shakeDistance * scale));
				state._shakeRotation = startR * Quaternion.Euler((float)mod * Mathf.Cos(timer) * (rotationAmount.x * rotationStrength * scale), (float)mod2 * Mathf.Sin(timer) * (rotationAmount.y * rotationStrength * scale), (float)mod3 * Mathf.Cos(timer) * (rotationAmount.z * rotationStrength * scale));
				state._uiShakePosition = new Vector2(start2.x - (float)mod * Mathf.Sin(timer) * (shakeAmount.x * shakeDistance * pixelScale), start2.y - (float)mod2 * Mathf.Cos(timer) * (shakeAmount.y * shakeDistance * pixelScale));
				Vector3 camOffset = GetGeometricAvg(stateList, position: true);
				Quaternion camRot = GetAvgRotation(stateList);
				NormalizeQuaternion(ref camRot);
				if (this.onShakeOffset != null)
				{
					this.onShakeOffset(camOffset, camRot);
				}
				switch (state._shakeType)
				{
				case ShakeType.CameraMatrix:
				{
					Matrix4x4 lhs = Matrix4x4.TRS(camOffset, camRot, new Vector3(1f, 1f, -1f));
					cam.worldToCameraMatrix = lhs * cachedTransform.worldToLocalMatrix;
					break;
				}
				case ShakeType.LocalPosition:
					cachedTransform.localPosition = camOffset;
					cachedTransform.localRotation = camRot;
					break;
				}
				Vector3 avg = GetGeometricAvg(stateList, position: false);
				m_ShakeRect.x = avg.x;
				m_ShakeRect.y = avg.y;
				if (timer > (float)Math.PI * 2f)
				{
					startTime = Time.time;
					shakeDistance *= 1f - Mathf.Clamp01(decay);
					rotationStrength *= 1f - Mathf.Clamp01(decay);
					currentShakes--;
				}
				yield return null;
			}
			Camera key2;
			(shakeCount = m_ShakeCount)[key2 = cam] = shakeCount[key2] - 1;
			if (this.onPostShake != null)
			{
				this.onPostShake();
			}
			if (m_ShakeCount[cam] == 0)
			{
				m_Shaking = false;
				ResetState(cam);
				if (this.onEndShaking != null)
				{
					this.onEndShaking();
				}
			}
			else
			{
				stateList.Remove(state);
			}
			callback?.Invoke();
		}

		private void CheckShakeRect()
		{
			if (Mathf.Abs((float)Screen.width - m_ShakeRect.width) > 1E-05f || Mathf.Abs((float)Screen.height - m_ShakeRect.height) > 1E-05f)
			{
				m_ShakeRect.width = Screen.width;
				m_ShakeRect.height = Screen.height;
			}
		}

		private void ResetState(Camera cam)
		{
			cam.ResetWorldToCameraMatrix();
			m_ShakeRect.x = 0f;
			m_ShakeRect.y = 0f;
			m_States[cam].Clear();
		}

		private IEnumerator DoResetState(IList<Camera> cameras, IDictionary<Camera, int> shakeCount, float time)
		{
			m_MatrixOffsetCache.Clear();
			m_MatrixRotationCache.Clear();
			m_OffsetCache.Clear();
			m_RotationCache.Clear();
			m_IgnoreMatrixCache.Clear();
			m_IgnorePositionCache.Clear();
			int j = 0;
			for (int count = cameras.Count; j < count; j++)
			{
				Camera camera = cameras[j];
				Transform transform = camera.transform;
				m_MatrixOffsetCache.Add((camera.worldToCameraMatrix * transform.worldToLocalMatrix.inverse).GetColumn(3));
				m_MatrixRotationCache.Add(QuaternionFromMatrix((camera.worldToCameraMatrix * transform.worldToLocalMatrix.inverse).inverse * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, 1f, -1f))));
				m_OffsetCache.Add(transform.localPosition);
				m_RotationCache.Add(transform.localRotation);
				if (shakeCount.ContainsKey(camera))
				{
					shakeCount[camera] = 0;
				}
				bool flag = true;
				bool flag2 = true;
				List<ShakeState> list = m_States[camera];
				int k = 0;
				for (int count2 = list.Count; k < count2; k++)
				{
					ShakeState shakeState = list[k];
					if (shakeState._shakeType == ShakeType.CameraMatrix)
					{
						flag = false;
					}
					else
					{
						flag2 = false;
					}
					if (!flag && !flag2)
					{
						break;
					}
				}
				m_IgnoreMatrixCache.Add(flag);
				m_IgnorePositionCache.Add(flag2);
				m_States[camera].Clear();
			}
			float t = 0f;
			float x = m_ShakeRect.x;
			float y = m_ShakeRect.y;
			m_Cancelling = true;
			while (t < time)
			{
				int i = 0;
				int l = 0;
				for (int count3 = cameras.Count; l < count3; l++)
				{
					Camera camera2 = cameras[l];
					Transform transform2 = camera2.transform;
					m_ShakeRect.x = Mathf.Lerp(x, 0f, t / time);
					m_ShakeRect.y = Mathf.Lerp(y, 0f, t / time);
					if (!m_IgnoreMatrixCache[l])
					{
						Vector3 pos = Vector3.Lerp(m_MatrixOffsetCache[i], Vector3.zero, t / time);
						Quaternion q = Quaternion.Slerp(m_MatrixRotationCache[i], transform2.rotation, t / time);
						Matrix4x4 lhs = Matrix4x4.TRS(pos, q, new Vector3(1f, 1f, -1f));
						camera2.worldToCameraMatrix = lhs * transform2.worldToLocalMatrix;
					}
					if (!m_IgnorePositionCache[l])
					{
						Vector3 localPosition = Vector3.Lerp(m_OffsetCache[i], Vector3.zero, t / time);
						Quaternion localRotation = Quaternion.Slerp(m_RotationCache[i], Quaternion.identity, t / time);
						transform2.localPosition = localPosition;
						transform2.localRotation = localRotation;
					}
					i++;
				}
				t += Time.deltaTime;
				yield return null;
			}
			int m = 0;
			for (int count4 = cameras.Count; m < count4; m++)
			{
				Camera camera3 = cameras[m];
				if (!m_IgnoreMatrixCache[m])
				{
					camera3.ResetWorldToCameraMatrix();
				}
				if (!m_IgnorePositionCache[m])
				{
					camera3.transform.localPosition = Vector3.zero;
					camera3.transform.localRotation = Quaternion.identity;
				}
				m_ShakeRect.x = 0f;
				m_ShakeRect.y = 0f;
			}
			m_Shaking = false;
			m_Cancelling = false;
		}

		private static Vector3 GetGeometricAvg(IList<ShakeState> states, bool position)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = states.Count;
			int i = 0;
			for (int count = states.Count; i < count; i++)
			{
				ShakeState shakeState = states[i];
				if (position)
				{
					num -= shakeState._shakePosition.x;
					num2 -= shakeState._shakePosition.y;
					num3 -= shakeState._shakePosition.z;
				}
				else
				{
					num += shakeState._uiShakePosition.x;
					num2 += shakeState._uiShakePosition.y;
				}
			}
			return new Vector3(num / num4, num2 / num4, num3 / num4);
		}

		private static Quaternion GetAvgRotation(IList<ShakeState> states)
		{
			Quaternion quaternion = new Quaternion(0f, 0f, 0f, 0f);
			int i = 0;
			for (int count = states.Count; i < count; i++)
			{
				ShakeState shakeState = states[i];
				if (Quaternion.Dot(shakeState._shakeRotation, quaternion) > 0f)
				{
					quaternion.x += shakeState._shakeRotation.x;
					quaternion.y += shakeState._shakeRotation.y;
					quaternion.z += shakeState._shakeRotation.z;
					quaternion.w += shakeState._shakeRotation.w;
				}
				else
				{
					quaternion.x += 0f - shakeState._shakeRotation.x;
					quaternion.y += 0f - shakeState._shakeRotation.y;
					quaternion.z += 0f - shakeState._shakeRotation.z;
					quaternion.w += 0f - shakeState._shakeRotation.w;
				}
			}
			float num = Mathf.Sqrt(quaternion.x * quaternion.x + quaternion.y * quaternion.y + quaternion.z * quaternion.z + quaternion.w * quaternion.w);
			if (num > 0.0001f)
			{
				quaternion.x /= num;
				quaternion.y /= num;
				quaternion.z /= num;
				quaternion.w /= num;
				return quaternion;
			}
			quaternion = states[0]._shakeRotation;
			return quaternion;
		}

		private static float GetPixelWidth(Transform cachedTransform, Camera cachedCamera)
		{
			Vector3 position = cachedTransform.position;
			Vector3 vector = cachedCamera.WorldToScreenPoint(position - cachedTransform.forward * 0.01f);
			Vector3 a = vector;
			a = ((!(vector.x > 0f)) ? (a + Vector3.right) : (a - Vector3.right));
			a = ((!(vector.y > 0f)) ? (a + Vector3.up) : (a - Vector3.up));
			a = cachedCamera.ScreenToWorldPoint(a);
			return 1f / (cachedTransform.InverseTransformPoint(position) - cachedTransform.InverseTransformPoint(a)).magnitude;
		}

		private static Quaternion QuaternionFromMatrix(Matrix4x4 m)
		{
			return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
		}

		private static void NormalizeQuaternion(ref Quaternion q)
		{
			float num = 0f;
			for (int i = 0; i < 4; i++)
			{
				num += q[i] * q[i];
			}
			float num2 = 1f / Mathf.Sqrt(num);
			for (int j = 0; j < 4; j++)
			{
				int index;
				q[index = j] = q[index] * num2;
			}
		}
	}
}
