using System.Collections;
using System.Collections.Generic;
// using TMPro;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// 本地化的文本替换
/// </summary>
[ExecuteInEditMode]
public class Localize : MonoBehaviour
{
    // public TextMeshProUGUI text;
    public Text uguiText;
    public string key = "";

    void Start ()
    {
        if (string.IsNullOrEmpty (key))
            return;
        
        SetUGUIText();
    }

//     private void SetTMText()
//     {
// if (text == null)
//         {
//             text = this.GetComponent<TextMeshProUGUI> ();
//         }

//         text.SetText (LocalizationManager.Instance.GetText (key));
//     }

    private void SetUGUIText()
    {
        if (uguiText == null)
        {
            uguiText = this.GetComponent<Text> ();
        }
        uguiText.text = LocalizationManager.Instance.GetText (key);
    }

#if UNITY_EDITOR
    private void OnEnable ()
    {
        // if (text == null)
        // {
        //     text = this.GetComponent<TextMeshProUGUI> ();
        // }

        if (uguiText == null)
        {
            uguiText = this.GetComponent<Text> ();
        }
    }
#endif

}