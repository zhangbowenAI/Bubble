
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class UIWindowBase : UIBase
{
    public enum WindowStatus
    {
        Create,
        Open,
        Close,
        OpenAnim,
        CloseAnim,
        Pause,
        Hide
    }

    [HideInInspector]
    public string cameraKey;

    public UIType m_UIType;

    public WindowStatus windowStatus;

    public GameObject m_bgMask;

    public GameObject m_uiRoot;

    public virtual void OnOpen()
    {
    }

    public virtual void OnClose()
    {
    }

    public virtual void OnHide()
    {
    }

    public virtual void OnShow()
    {
    }

    public virtual void OnRefresh()
    {
    }

    public virtual IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        m_bgMask.SetActive(value: true);
        Vector3 localPosition = base.transform.localPosition;
        localPosition.y = (float)Screen.height * 2f;
        m_uiRoot.transform.localPosition = localPosition;
        Tweener t = m_uiRoot.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 0f), 0.25f);
        t.OnComplete(delegate
        {
            l_animComplete(this, l_callBack, objs);
        });
        yield break;
    }

    public virtual void OnCompleteEnterAnim()
    {
    }

    public virtual IEnumerator PauserAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        m_bgMask.SetActive(value: false);
        Tweener t = m_uiRoot.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, (float)(-Screen.height) * 2f), 0.3f);
        t.OnComplete(delegate
        {
            l_animComplete(this, l_callBack, objs);
        });
        yield break;
    }

    public virtual void OnCompletePauserAnim()
    {
    }

    public virtual IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
    {
        Tweener t = m_uiRoot.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, (float)(-Screen.height) * 2f), 0.35f);
        t.OnComplete(delegate
        {
            m_bgMask.SetActive(value: false);
            l_animComplete(this, l_callBack, objs);
        });
        yield break;
    }

    public virtual void OnCompleteExitAnim()
    {
    }

    public void MoveInByUp(string name, float _time = 0.3f)
    {
        RectTransform rectTransform = GetRectTransform(name);
        Vector2 gameObjectPostion = GetGameObjectPostion(name);
        rectTransform.anchoredPosition = new Vector2(gameObjectPostion.x, gameObjectPostion.y + 1000f);
        Sequence s = DOTween.Sequence();
        Tweener t = rectTransform.DOAnchorPos(gameObjectPostion, _time);
        t.SetEase(Ease.InOutBack);
        Tweener t2 = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x, gameObjectPostion.y - 10f), 0.1f);
        t.SetEase(Ease.OutBack);
        Tweener t3 = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x, gameObjectPostion.y + 10f), 0.1f);
        t.SetEase(Ease.OutBack);
        Tweener t4 = rectTransform.DOAnchorPos(gameObjectPostion, 0.09f);
        t.SetEase(Ease.Linear);
        s.Append(t).Append(t2).Append(t3)
            .Append(t4);
    }

    public void MoveOutByUp(string name, float _time = 0.3f)
    {
        RectTransform rectTransform = GetRectTransform(name);
        Vector2 gameObjectPostion = GetGameObjectPostion(name);
        Sequence s = DOTween.Sequence();
        Tweener t = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x, gameObjectPostion.y - 10f), 0.1f);
        t.SetEase(Ease.OutBack);
        Tweener t2 = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x, gameObjectPostion.y + 1000f), _time);
        t2.SetEase(Ease.Linear);
        s.Append(t).Append(t2);
    }

    public void MoveInByDown(string name, float _time = 0.3f)
    {
        RectTransform rectTransform = GetRectTransform(name);
        Vector2 gameObjectPostion = GetGameObjectPostion(name);
        rectTransform.anchoredPosition = new Vector2(gameObjectPostion.x, gameObjectPostion.y - 1000f);
        Sequence s = DOTween.Sequence();
        Tweener t = rectTransform.DOAnchorPos(gameObjectPostion, _time);
        t.SetEase(Ease.InOutBack);
        Tweener t2 = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x, gameObjectPostion.y + 10f), 0.1f);
        t.SetEase(Ease.OutBack);
        Tweener t3 = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x, gameObjectPostion.y - 10f), 0.1f);
        t.SetEase(Ease.OutBack);
        Tweener t4 = rectTransform.DOAnchorPos(gameObjectPostion, 0.09f);
        t.SetEase(Ease.Linear);
        s.Append(t).Append(t2).Append(t3)
            .Append(t4);
    }

    public void MoveOutByDown(string name, float _time = 0.5f)
    {
        RectTransform rectTransform = GetRectTransform(name);
        Vector2 gameObjectPostion = GetGameObjectPostion(name);
        Sequence s = DOTween.Sequence();
        Tweener t = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x, gameObjectPostion.y + 10f), 0.1f);
        t.SetEase(Ease.OutBack);
        Tweener t2 = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x, gameObjectPostion.y - 1000f), _time);
        t2.SetEase(Ease.Linear);
        s.Append(t).Append(t2);
    }

    public void MoveInByLeft(string name, float _time = 0.25f)
    {
        RectTransform rectTransform = GetRectTransform(name);
        Vector2 gameObjectPostion = GetGameObjectPostion(name);
        rectTransform.anchoredPosition = new Vector2(gameObjectPostion.x - 1000f, gameObjectPostion.y);
        Sequence s = DOTween.Sequence();
        Tweener t = rectTransform.DOAnchorPos(gameObjectPostion, _time);
        t.SetEase(Ease.InOutBack);
        Tweener t2 = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x + 10f, gameObjectPostion.y), 0.1f);
        t.SetEase(Ease.OutBack);
        Tweener t3 = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x - 10f, gameObjectPostion.y), 0.1f);
        t.SetEase(Ease.OutBack);
        Tweener t4 = rectTransform.DOAnchorPos(gameObjectPostion, 0.1f);
        t.SetEase(Ease.Linear);
        s.Append(t).Append(t2).Append(t3)
            .Append(t4);
    }

    public void MoveOutByLeft(string name, float _time = 0.25f)
    {
        RectTransform rectTransform = GetRectTransform(name);
        Vector2 gameObjectPostion = GetGameObjectPostion(name);
        Sequence s = DOTween.Sequence();
        Tweener t = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x + 10f, gameObjectPostion.y), 0.1f);
        t.SetEase(Ease.OutBack);
        Tweener t2 = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x - 1000f, gameObjectPostion.y), _time);
        t2.SetEase(Ease.Linear);
        s.Append(t).Append(t2);
    }

    public void MoveInByRigth(string name, float _time = 0.25f)
    {
        RectTransform rectTransform = GetRectTransform(name);
        Vector2 gameObjectPostion = GetGameObjectPostion(name);
        rectTransform.anchoredPosition = new Vector2(gameObjectPostion.x + 1000f, gameObjectPostion.y);
        Sequence s = DOTween.Sequence();
        Tweener t = rectTransform.DOAnchorPos(gameObjectPostion, _time);
        t.SetEase(Ease.InOutBack);
        Tweener t2 = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x - 10f, gameObjectPostion.y), 0.1f);
        t.SetEase(Ease.OutBack);
        Tweener t3 = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x + 10f, gameObjectPostion.y), 0.1f);
        t.SetEase(Ease.OutBack);
        Tweener t4 = rectTransform.DOAnchorPos(gameObjectPostion, 0.1f);
        t.SetEase(Ease.Linear);
        s.Append(t).Append(t2).Append(t3)
            .Append(t4);
    }

    public void MoveOutByRigth(string name, float _time = 0.25f)
    {
        RectTransform rectTransform = GetRectTransform(name);
        Vector2 gameObjectPostion = GetGameObjectPostion(name);
        Sequence s = DOTween.Sequence();
        Tweener t = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x - 10f, gameObjectPostion.y), 0.1f);
        t.SetEase(Ease.OutBack);
        Tweener t2 = rectTransform.DOAnchorPos(new Vector2(gameObjectPostion.x + 1000f, gameObjectPostion.y), _time);
        t2.SetEase(Ease.Linear);
        s.Append(t).Append(t2);
    }

    public virtual void Show()
    {
        base.gameObject.SetActive(value: true);
    }

    public virtual void Hide()
    {
        base.gameObject.SetActive(value: false);
    }

    public void Refresh(params object[] args)
    {
        UISystemEvent.Dispatch(this, UIEvent.OnRefresh);
        OnRefresh();
    }

    public void AddEventListener(Enum l_Event)
    {
        if (!m_EventNames.Contains(l_Event))
        {
            m_EventNames.Add(l_Event);
            GlobalEvent.AddEvent(l_Event, Refresh);
        }
    }

    public override void RemoveAllListener()
    {
        base.RemoveAllListener();
        for (int i = 0; i < m_EventNames.Count; i++)
        {
            GlobalEvent.RemoveEvent(m_EventNames[i], Refresh);
        }
        m_EventNames.Clear();
    }
}
