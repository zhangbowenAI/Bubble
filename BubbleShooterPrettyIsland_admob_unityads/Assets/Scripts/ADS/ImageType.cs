using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ImageType : MonoBehaviour
{
    public Image image;
    public Sprite ChineseSprite;
    public Sprite EngLishSprite;
    public bool isNativeSize = false;

    void Start()
    {
        if (image == null)
        {
            image = this.GetComponent<Image>();
        }
        image.sprite = BuildSetting.Instance.language == LanguageType.CN ? ChineseSprite : EngLishSprite;
        if (isNativeSize)
            image.SetNativeSize();
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        if (image == null)
        {
            image = this.GetComponent<Image>();
            ChineseSprite = image.sprite;
            EngLishSprite = image.sprite;
        }
    }
#endif
}
