
using UnityEngine;
using UnityEngine.UI;

public class LanguageComponent : MonoBehaviour
{
    public string languageKey = string.Empty;

    public Text m_text;

    public void Start()
    {
        if (m_text == null)
        {
            m_text = GetComponent<Text>();
        }
        Init();
    }

    public void Init()
    {
        GlobalEvent.AddEvent(LanguageEventEnum.LanguageChange, ReceviceLanguageChange);
        ResetLanguage();
    }

    public void ResetLanguage()
    {
        if (!string.IsNullOrEmpty(languageKey))
        {
            m_text.text = LanguageManager.GetContentByKey(languageKey);
        }
    }

    private void ReceviceLanguageChange(params object[] objs)
    {
        ResetLanguage();
    }
}
