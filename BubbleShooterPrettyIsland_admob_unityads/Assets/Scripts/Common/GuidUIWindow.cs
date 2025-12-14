
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GuidUIWindow : UIWindowBase
{
    public Text guidText;

    public Image guidImg;

    public Text NextText;

    public Text Titletext;

    public override void OnOpen()
    {
        AddOnClickListener("CloseButton", CloseButton);
        AddOnClickListener("NextButton", NextClick);
        try
        {
            Texture2D texture2D = ResourceManager.Load("gamescene/guid/guidimg/guide_bubble_level" + Singleton<LevelManager>.Instance.GetNowLevel()) as Texture2D;
            guidImg.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), Vector2.zero);
        }
        catch (Exception)
        {
        }
        InitLanguage();
    }

    public void InitLanguage()
    {
        // if (GameEntry.buildType == GameEntry.BuildType.GoogleEN)
        // {
        // 	guidText.text = Singleton<DataGuidEN>.Instance.GetContentByKeyAndType("level" + Singleton<LevelManager>.Instance.GetNowLevel(), GuidENType.text1);
        // }
        // else
        // {
        guidText.text = Singleton<DataGuidZH>.Instance.GetContentByKeyAndType("level" + Singleton<LevelManager>.Instance.GetNowLevel(), GuidZHType.text1);
        // }
        Titletext.text = Util.ReplaceText(GameEntry.Instance.GetString("GuideMaxTitle"));
        NextText.text = Util.ReplaceText(GameEntry.Instance.GetString("GuideMaxNext"));
    }

    public void CloseButton(InputUIOnClickEvent e)
    {
        UIManager.CloseUIWindow<GuidUIWindow>(isPlayAnim: true, null, new object[0]);
    }

    public void NextClick(InputUIOnClickEvent e)
    {
        UIManager.CloseUIWindow<GuidUIWindow>(isPlayAnim: true, null, new object[0]);
    }

    public override void OnRefresh()
    {
    }

    public override IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        StartCoroutine(base.EnterAnim(animComplete, callBack, objs));
        yield return new WaitForEndOfFrame();
    }

    public override IEnumerator ExitAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        if (GameGuid.Instance != null)
        {
            GameGuid.Instance.CloseWindow();
        }
        StartCoroutine(base.ExitAnim(animComplete, callBack, objs));
        yield return new WaitForEndOfFrame();
    }

    public override IEnumerator PauserAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        StartCoroutine(base.PauserAnim(animComplete, callBack, objs));
        yield return new WaitForEndOfFrame();
    }
}
