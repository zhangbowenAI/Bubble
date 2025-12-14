
using UnityEngine;

public class CheckShowCDK : MonoBehaviour
{
	private void Start()
	{
		if (AndroidManager.Instance.GetCDK() == 0)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
