
using DG.Tweening;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoseUI : MonoBehaviour
{
    public GameObject GoldObj;

    public GameObject shibaijiesuan;

    public GameObject textwenzi;

    private bool IsLose;

    private void Update()
    {
        // if (IsLose && Input.GetMouseButtonDown(0))
        // {
        // 	AudioPlayManager.PlaySFX2D("button");

        //     UIManager.OpenUIWindow<PlayWindow>();
        //     IsLose = false;
        // }
    }

    public void ContinuneBtnClick()
    {
        AudioPlayManager.PlaySFX2D("button");
        UIManager.OpenUIWindow<PlayWindow>();
        IsLose = false;
    }

    public void HomeBtnClick()
    {
        AudioPlayManager.PlaySFX2D("button");
        IsLose = false;
        GameEntry.ChangeScene(GameEntry.LevelScene);
    }

    public void InitUI()
    {
        if (Singleton<UserData>.Instance.GetPassLevel() > 2)
        {
            if (BuildSetting.Instance.adChannel == AdChannelsType.OPPO)
                ADInterface.Instance.SendADEvent(ADType.SceneVideoAD, ADSpot.GameEnd);
            else if (BuildSetting.Instance.adChannel == AdChannelsType.VIVO)
                ADInterface.Instance.SendADEvent(ADType.CPAD, ADSpot.GameEnd);
        }
        ADInterface.Instance.SendADEvent(ADType.ShowBanner, ADSpot.GameBanner);
        CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.GameOver, "Level", Singleton<LevelManager>.Instance.GetNowLevel().ToString());
        GoldObj.SetActive(value: false);
        textwenzi.SetActive(value: false);
        // shibaijiesuan.SetActive(value: true);
        // shibaijiesuan.GetComponent<SkeletonGraphic>().AnimationState.Complete += delegate
        // {
        GoldObj.SetActive(value: true);
        GoldObj.transform.Find("GoldNum").GetComponent<Text>().text = Singleton<UserData>.Instance.GetUserGold().ToString();
        // shibaijiesuan.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "loop", loop: true);
        // };
        StartCoroutine(RenwuMove());
        InitLanguage();
        AudioPlayManager.PlaySFX2D("ui_lose");
        AndroidManager.Instance.FailLevel(Singleton<LevelManager>.Instance.GetNowLevel());
        UnityEngine.Debug.Log("Failure checkpoint level: " + Singleton<LevelManager>.Instance.GetNowLevel());
    }

    public void InitLanguage()
    {
        textwenzi.GetComponent<Text>().text = Util.ReplaceText(GameEntry.Instance.GetString("LoseUILevelRestartText"));
    }

    private IEnumerator RenwuMove()
    {
        yield return new WaitForSeconds(0.1f);
        // GameScene.Instance.jlxzObj.GetComponent<MeshRenderer>().sortingOrder = 31;
        // GameScene.Instance.GameOverMove3.transform.DOLocalRotate(new Vector3(0f, 180f, 0f), 0.1f);
        GameScene.Instance.SwitchoverElfAni("cry", bLoop: false);
        // Transform transform = GameScene.Instance.GameOverMove3.transform;
        // Vector3 localPosition = GameScene.Instance.GameOverMove3.transform.localPosition;
        // transform.DOLocalMoveX(localPosition.x + 4f, 0.7f).OnComplete(delegate
        // {
        //     // textwenzi.SetActive(value: true);
        // });
        yield return new WaitForSeconds(2f);
        IsLose = true;
    }
}
