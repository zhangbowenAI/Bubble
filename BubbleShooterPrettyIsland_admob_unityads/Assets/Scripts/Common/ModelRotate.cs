
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelRotate : MonoBehaviour
{
    public float rotateSpeed = 1f;

    private Vector2 mousePos = Vector2.zero;

    private void Rotate(Vector2 delta)
    {
        base.transform.localRotation = Quaternion.Euler(base.transform.localRotation.eulerAngles + new Vector3(0f, (0f - delta.x) * rotateSpeed, 0f));
    }

    public void OnDrag(BaseEventData arg0)
    {
        if (mousePos == Vector2.zero)
        {
            mousePos = arg0.currentInputModule.input.mousePosition;
            return;
        }
        Vector2 vector = arg0.currentInputModule.input.mousePosition - mousePos;
        if (vector != Vector2.zero)
        {
            mousePos = arg0.currentInputModule.input.mousePosition;
        }
        Rotate(vector);
    }
}
