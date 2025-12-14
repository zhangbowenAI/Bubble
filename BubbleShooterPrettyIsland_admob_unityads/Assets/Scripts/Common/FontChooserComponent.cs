
using UnityEngine;
using UnityEngine.UI;

public class FontChooserComponent : MonoBehaviour
{
	public Font m_Traditional;

	public Font m_Simplified;

	private Text m_text;

	private void Start()
	{
		if (m_text == null)
		{
			m_text = GetComponent<Text>();
		}
		ResetLanguage();
		GlobalEvent.AddEvent(LanguageEventEnum.LanguageChange, ReceviceLanguageChange);
	}

	private void ResetLanguage()
	{
		if (m_text != null)
		{
			if (LanguageManager.s_currentLanguage == SystemLanguage.ChineseTraditional)
			{
				m_text.font = m_Traditional;
			}
			else
			{
				m_text.font = m_Simplified;
			}
		}
	}

	private void ReceviceLanguageChange(params object[] objs)
	{
		ResetLanguage();
	}
}
