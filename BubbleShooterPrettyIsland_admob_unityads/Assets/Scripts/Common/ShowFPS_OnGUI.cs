
using UnityEngine;

public class ShowFPS_OnGUI : MonoBehaviour
{
	public float fpsMeasuringDelta = 1f;

	private float timePassed;

	private int m_FrameCount;

	private float m_FPS;

	private GUIStyle bb;

	private void Start()
	{
		Application.targetFrameRate = 60;
		timePassed = 0f;
		bb = new GUIStyle();
		bb.normal.background = null;
		bb.normal.textColor = new Color(1f, 0.5f, 0f);
		bb.fontSize = 40;
	}

	private void Update()
	{
		m_FrameCount++;
		timePassed += Time.deltaTime;
		if (timePassed > fpsMeasuringDelta)
		{
			m_FPS = (float)m_FrameCount / timePassed;
			timePassed = 0f;
			m_FrameCount = 0;
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(Screen.width / 2 - 40, Screen.height - 100, 200f, 200f), "FPS: " + m_FPS, bb);
	}
}
