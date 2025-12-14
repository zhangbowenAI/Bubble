
using DG.Tweening;
using UnityEngine;

public class CoinFly : MonoBehaviour
{
    public GameObject EndObj;

    public void Init(GameObject endObj, float time, bool up)
    {
        Vector3 centerPost = GameManager.Instance.GetCenterPost(base.transform.localPosition, endObj.transform.localPosition, 3f);
        Vector3[] path = new Vector3[3]
        {
            base.transform.localPosition,
            centerPost,
            endObj.transform.localPosition + new Vector3(-50f, 0f)
        };
        base.transform.DOLocalPath(path, 0.8f, PathType.CatmullRom, PathMode.TopDown2D, 20).SetEase(Ease.InQuad).OnComplete(delegate
        {
            AudioPlayManager.PlaySFX2D("ui_winstar1");
            if (up)
            {
                Singleton<UserData>.Instance.AddUserGold(25);
            }
            else
            {
                Singleton<UserData>.Instance.AddUserGold(5);
            }
            UnityEngine.Object.Destroy(base.gameObject);
        })
            .SetDelay(time);
    }
}
