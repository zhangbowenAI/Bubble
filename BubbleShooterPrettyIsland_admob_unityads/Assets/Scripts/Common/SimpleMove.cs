
using Lean;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleMove : MonoBehaviour
{
    private bool bMoveState;

    private bool bDown;

    private Vector3 ClickPos = new Vector3(0f, 0f, 0f);

    protected virtual void LateUpdate()
    {
        if (((!Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0)) || !(EventSystem.current.currentSelectedGameObject != null)) && UIManager.GetNormalUICount() <= 0)
        {
            LeanTouch.MoveObject(base.transform, LeanTouch.DragDelta);
            base.transform.rotation = Quaternion.identity;
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bDown = true;
            ClickPos = UnityEngine.Input.mousePosition;
        }
        if (bDown && ClickPos != UnityEngine.Input.mousePosition)
        {
            bMoveState = true;
        }
        if (!bDown && bMoveState)
        {
            bMoveState = false;
        }
        RectTransform component = base.transform.GetComponent<RectTransform>();
        Vector3 localScale = component.localScale;
        float x = localScale.x;
        float num = 50f * x;
        Vector3 localPosition = component.localPosition;
        float x2 = localPosition.x;
        Vector3 localPosition2 = component.localPosition;
        float y = localPosition2.y;
        if (x2 >= num)
        {
            RectTransform rectTransform = component;
            float x3 = num;
            Vector3 localPosition3 = component.localPosition;
            float y2 = localPosition3.y;
            Vector3 localPosition4 = component.localPosition;
            rectTransform.localPosition = new Vector3(x3, y2, localPosition4.z);
        }
        else if (x2 * -1f >= num)
        {
            RectTransform rectTransform2 = component;
            float x4 = 0f - num;
            Vector3 localPosition5 = component.localPosition;
            float y3 = localPosition5.y;
            Vector3 localPosition6 = component.localPosition;
            rectTransform2.localPosition = new Vector3(x4, y3, localPosition6.z);
        }
        if (y >= num)
        {
            RectTransform rectTransform3 = component;
            Vector3 localPosition7 = component.localPosition;
            float x5 = localPosition7.x;
            float y4 = num;
            Vector3 localPosition8 = component.localPosition;
            rectTransform3.localPosition = new Vector3(x5, y4, localPosition8.z);
        }
        else if (y * -1f >= num)
        {
            RectTransform rectTransform4 = component;
            Vector3 localPosition9 = component.localPosition;
            float x6 = localPosition9.x;
            float y5 = 0f - num;
            Vector3 localPosition10 = component.localPosition;
            rectTransform4.localPosition = new Vector3(x6, y5, localPosition10.z);
        }
    }
}
