
using UnityEngine;

public class ResManage : MonoBehaviour
{
	public static ResManage Instance;

	public Sprite[] bubbles;

	public Sprite[] bubbleTops;

	public void Awake()
	{
		if (Instance == null)
		{
			Object.DontDestroyOnLoad(base.gameObject);
			Instance = this;
		}
		else if (Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
