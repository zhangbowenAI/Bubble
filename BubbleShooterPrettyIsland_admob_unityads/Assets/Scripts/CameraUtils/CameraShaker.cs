
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
	public Vector3 DefaultPosInfluence = new Vector3(0.15f, 0.15f, 0.15f);

	public Vector3 DefaultRotInfluence = new Vector3(1f, 1f, 1f);

	private Vector3 posAddShake;

	private Vector3 rotAddShake;

	private List<CameraShakeInstance> cameraShakeInstances = new List<CameraShakeInstance>();

	public string cameraTag = string.Empty;

	public bool isUICamera;

	private float UI_Z;

	public List<CameraShakeInstance> ShakeInstances => new List<CameraShakeInstance>(cameraShakeInstances);

	public void Awake()
	{
		if (string.IsNullOrEmpty(cameraTag))
		{
			cameraTag = base.gameObject.name;
		}
		CameraShakerManager.AddCameraShaker(cameraTag, this);
		Vector3 localPosition = base.transform.localPosition;
		UI_Z = localPosition.z;
	}

	public void Update()
	{
		posAddShake = Vector3.zero;
		rotAddShake = Vector3.zero;
		for (int i = 0; i < cameraShakeInstances.Count && i < cameraShakeInstances.Count; i++)
		{
			CameraShakeInstance cameraShakeInstance = cameraShakeInstances[i];
			if (cameraShakeInstance.CurrentState == CameraShakeState.Inactive && cameraShakeInstance.DeleteOnInactive)
			{
				cameraShakeInstances.RemoveAt(i);
				i--;
			}
			else if (cameraShakeInstance.CurrentState != CameraShakeState.Inactive)
			{
				posAddShake += CameraUtilities.MultiplyVectors(cameraShakeInstance.UpdateShake(), cameraShakeInstance.PositionInfluence);
				rotAddShake += CameraUtilities.MultiplyVectors(cameraShakeInstance.UpdateShake(), cameraShakeInstance.RotationInfluence);
			}
		}
		if (isUICamera)
		{
			posAddShake.z = UI_Z;
		}
		base.transform.localPosition = posAddShake;
		base.transform.localEulerAngles = rotAddShake;
	}

	public CameraShakeInstance Shake(CameraShakeInstance shake)
	{
		cameraShakeInstances.Add(shake);
		return shake;
	}

	public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
	{
		CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness, fadeInTime, fadeOutTime);
		cameraShakeInstance.PositionInfluence = DefaultPosInfluence;
		cameraShakeInstance.RotationInfluence = DefaultRotInfluence;
		cameraShakeInstances.Add(cameraShakeInstance);
		return cameraShakeInstance;
	}

	public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime, Vector3 posInfluence, Vector3 rotInfluence)
	{
		CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness, fadeInTime, fadeOutTime);
		cameraShakeInstance.PositionInfluence = posInfluence;
		cameraShakeInstance.RotationInfluence = rotInfluence;
		cameraShakeInstances.Add(cameraShakeInstance);
		return cameraShakeInstance;
	}

	public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime)
	{
		CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness);
		cameraShakeInstance.PositionInfluence = DefaultPosInfluence;
		cameraShakeInstance.RotationInfluence = DefaultRotInfluence;
		cameraShakeInstance.StartFadeIn(fadeInTime);
		cameraShakeInstances.Add(cameraShakeInstance);
		return cameraShakeInstance;
	}

	public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime, Vector3 posInfluence, Vector3 rotInfluence)
	{
		CameraShakeInstance cameraShakeInstance = new CameraShakeInstance(magnitude, roughness);
		cameraShakeInstance.PositionInfluence = posInfluence;
		cameraShakeInstance.RotationInfluence = rotInfluence;
		cameraShakeInstance.StartFadeIn(fadeInTime);
		cameraShakeInstances.Add(cameraShakeInstance);
		return cameraShakeInstance;
	}

	private void OnDestroy()
	{
		CameraShakerManager.RemoveCameraShaker(cameraTag);
	}
}
