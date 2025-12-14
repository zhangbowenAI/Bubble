
using UnityEngine;
using UnityEngine.UI;

public class ContinuousItemArranger : MonoBehaviour
{
    public static int gSequenceCount;

    [SerializeField]
    private Image mProgress;

    [SerializeField]
    private float[] mProgressAmount;

    [SerializeField]
    private GameObject mIndicator;

    [SerializeField]
    private GameObject mFxRect;

    [SerializeField]
    private float[] mProgressIndicatorPos;
    [SerializeField]
    private Transform[] psPox;

    public void Init()
    {
        mProgress.fillAmount = 0f;
        Vector3 localPosition = mIndicator.transform.localPosition;
        localPosition.x = mProgressIndicatorPos[gSequenceCount];
        mIndicator.transform.localPosition = localPosition;
        if (gSequenceCount == 0)
        {
            mFxRect.SetActive(value: false);
            return;
        }
        mFxRect.SetActive(value: false);
        // Vector3 localPosition2 = mFxRect.transform.localPosition;
        // localPosition2.x = localPosition.x;
        mFxRect.transform.localPosition = psPox[gSequenceCount].localPosition;
    }

    private void Update()
    {
        if (mProgress.fillAmount < mProgressAmount[gSequenceCount])
        {
            mProgress.fillAmount += Time.deltaTime * 0.5f;
        }
        else
        {
            mProgress.fillAmount = mProgressAmount[gSequenceCount];
        }
    }
}
