
using Lean;
using UnityEngine;

public class SimpleRotateScaleRelative : MonoBehaviour
{
    protected virtual void LateUpdate()
    {
        Vector2 centerOfFingers = LeanTouch.GetCenterOfFingers();
        LeanTouch.ScaleObjectRelative(base.transform, LeanTouch.PinchScale, centerOfFingers);
        Vector3 localScale = base.transform.localScale;
        if (localScale.x < 0.2f)
        {
            Transform transform = base.transform;
            Vector3 localScale2 = base.transform.localScale;
            transform.localScale = new Vector3(0.2f, 0.2f, localScale2.z);
        }
        Vector3 localScale3 = base.transform.localScale;
        if (localScale3.x >= 1f)
        {
            Transform transform2 = base.transform;
            Vector3 localScale4 = base.transform.localScale;
            transform2.localScale = new Vector3(1f, 1f, localScale4.z);
        }
    }
}
