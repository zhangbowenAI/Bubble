using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonExtension : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);
    public Vector3 normalScale = Vector3.one;
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(targetScale, 0.1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(normalScale, 0.1f);
    }
}
