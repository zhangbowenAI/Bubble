
using UnityEngine;

public class ShakeOnKeyPress : MonoBehaviour
{
	public float Magnitude = 2f;

	public float Roughness = 10f;

	public float FadeOutTime = 5f;

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
		{
			CameraShakerManager.GetCameraShaker("Main Camera").ShakeOnce(Magnitude, Roughness, 0f, FadeOutTime);
		}
	}
}
