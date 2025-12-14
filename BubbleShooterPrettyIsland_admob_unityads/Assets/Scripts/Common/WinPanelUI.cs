
using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanelUI : MonoBehaviour
{
    public GameObject lihua1;

    public GameObject lihua2;

    public GameObject lihua3;

    public GameObject StartBG1;

    public GameObject StartBG2;

    public GameObject StartBG3;

    public GameObject Start1;

    public GameObject Start2;

    public GameObject Start3;

    public GameObject fx_end_star;

    public GameObject GoldObj;

    public GameObject shengliwenzi;

    public GameObject FreeVideo;

    public GameObject textwenzi;
    public GameObject continuneBtn;
    public GameObject homeBtn;

    public GameObject shopBtn;
    public GameObject shopBtnTip;
    public GameObject missionBtn;
    public GameObject missionBtnTip;

    public GameObject GoldIcon;

    public int index;

    public Text GoldNumText;

    private bool IsWin;

    public GameObject JinzuSpine;

    public Text FreeVideoText;

    public GameObject BoxCloseObj;

    public GameObject BoxOpenObj;

    public Text OpenText;

    public Image SliderImage;

    public Text SliderText;

    public GameObject BaoixngBG;

    public GameObject CoinsPos;

    public GameObject CoinsObj;

    public List<GameObject> gridBox = new List<GameObject>();
    public GameObject winLogo;
    public PackagePanel packagePanel;

    public GameObject tip1, tip2, tip3, tip4;

    private void Update()
    {
        if (!IsWin || !Input.GetMouseButtonDown(0))
        {
            return;
        }
        AudioPlayManager.PlaySFX2D("button");
        if (UIManager.GetNormalUICount() > 0)
        {
            return;
        }
        // GameObject gameObject = CandyTouchChecker(UnityEngine.Input.mousePosition);
        // if ((bool)gameObject)
        // {
        //     if (gameObject.name == "FreeVideo")
        //     {
        //         WatchWinVideo(); 
        //     }
        // }
        // else
        // {
        //     AudioPlayManager.PlaySFX2D("ButtonClick");
        //     FlyCoins();
        //     IsWin = false;
        // }
    }

    public void ContinuneBtnClick()
    {
        Debug.Log("通过关卡：" + Singleton<UserData>.Instance.GetPassLevel());
        if (Singleton<UserData>.Instance.GetPassLevel() == 1 && !PlayerData.Instance.IsSignGuide)
        {
            AudioPlayManager.PlaySFX2D("ButtonClick");
            FlyCoins(true);
            IsWin = false;
            PlayerData.Instance.guideStep = 1;
        }
        else if (Singleton<UserData>.Instance.GetPassLevel() == 2 && !PlayerData.Instance.IsLotteryGuide)
        {
            AudioPlayManager.PlaySFX2D("ButtonClick");
            FlyCoins(true);
            IsWin = false;
            PlayerData.Instance.guideStep = 2;
        }
        else if (Singleton<UserData>.Instance.GetPassLevel() == 5 && !PlayerData.Instance.IsMissionGuide)
        {
            AudioPlayManager.PlaySFX2D("ButtonClick");
            FlyCoins(true);
            IsWin = false;
            PlayerData.Instance.guideStep = 4;
        }
        else if (Singleton<UserData>.Instance.GetPassLevel() == 12 && !PlayerData.Instance.IsShopGuide)
        {
            AudioPlayManager.PlaySFX2D("ButtonClick");
            FlyCoins(true);
            IsWin = false;
            PlayerData.Instance.guideStep = 3;
        }
        else
        {
            AudioPlayManager.PlaySFX2D("ButtonClick");
            FlyCoins();
            IsWin = false;
        }

    }

    public void HomeBtnClick()
    {
        AudioPlayManager.PlaySFX2D("ButtonClick");
        FlyCoins(true);
        IsWin = false;
    }


    public void OpenBaoxiang()
    {
        AudioPlayManager.PlaySFX2D("button");
        UIManager.OpenUIWindow<ChestUIWindow>();
    }

    public void OpenYanhua()
    {
        lihua1.SetActive(value: true);
        lihua3.SetActive(value: true);
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
        CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.GameClear, "Level", Singleton<LevelManager>.Instance.GetNowLevel().ToString());
        IsWin = false;
        tip1.SetActive(false);
        tip2.SetActive(false);
        tip3.SetActive(false);
        tip4.SetActive(false);
        GoldIcon.SetActive(value: false);
        textwenzi.SetActive(value: false);
        continuneBtn.SetActive(value: false);
        shopBtn.SetActive(value: false);
        missionBtn.SetActive(value: false);
        homeBtn.SetActive(value: false);
        StartBG1.SetActive(value: true);
        StartBG2.SetActive(value: true);
        StartBG3.SetActive(value: true);
        GoldObj.SetActive(value: false);
        FreeVideo.SetActive(value: false);
        JinzuSpine.SetActive(value: false);
        // shengliwenzi.SetActive(value: true);
        BaoixngBG.SetActive(value: false);
        winLogo.SetActive(value: true);
        InitLanguage();
        index = 1;
        // shengliwenzi.GetComponent<SkeletonGraphic>().AnimationState.Complete += delegate
        // {
        Startani();
        // GameScene.Instance.GameOverMove3.transform.DOLocalRotate(new Vector3(0f, 180f, 0f), 0.1f);
        // // GameScene.Instance.jlxzObj.GetComponent<MeshRenderer>().sortingOrder = 31;
        // Transform transform = GameScene.Instance.GameOverMove3.transform;
        // Vector3 localPosition = GameScene.Instance.GameOverMove3.transform.localPosition;
        // transform.DOLocalMoveX(localPosition.x + 4f, 0.7f);
        GoldObj.SetActive(value: true);
        GoldObj.transform.Find("GoldNum").GetComponent<Text>().text = Singleton<UserData>.Instance.GetUserGold().ToString();
        BaoixngBG.SetActive(value: true);
        BaoixngBG.GetComponentInChildren<Button>().enabled = true;
        InitBaoXiang();
        // };
        AudioPlayManager.PlaySFX2D("ui_win");
        GlobalEvent.AddEvent(GameEventEnum.UIFlushChane, ReceviceGoldChange);
        UnityEngine.Debug.Log(" ==win=== " + (Singleton<UserData>.Instance.GetPassLevel() + 1));
        UnityEngine.Debug.Log(" passlevel:" + Singleton<UserData>.Instance.GetPassLevel() + "  == now level " + Singleton<LevelManager>.Instance.GetNowLevel());

        MissionCheckMgr.Instance.SetMission(MissionType.Level, Singleton<LevelManager>.Instance.GetNowLevel());

        if (Singleton<UserData>.Instance.GetPassLevel() == 10 && Singleton<LevelManager>.Instance.GetNowLevel() == 10)
        {
            AndroidManager.Instance.AddAdjustEvent("e6n1f8");
        }
        else if (Singleton<UserData>.Instance.GetPassLevel() == 30 && Singleton<LevelManager>.Instance.GetNowLevel() == 30)
        {
            AndroidManager.Instance.AddAdjustEvent("qyahlg");
        }
        else if (Singleton<UserData>.Instance.GetPassLevel() == 100 && Singleton<LevelManager>.Instance.GetNowLevel() == 100)
        {
            AndroidManager.Instance.AddAdjustEvent("1s3q2p");
        }



        AndroidManager.Instance.FinishLevel(Singleton<LevelManager>.Instance.GetNowLevel());
        UnityEngine.Debug.Log("Victory checkpoint level: " + Singleton<UserData>.Instance.GetPassLevel());

        if (Singleton<UserData>.Instance.GetPassLevel() == 6 && Singleton<LevelManager>.Instance.GetNowLevel() == 6)
        {
            packagePanel.SetData(true);
        }
        else if (Singleton<UserData>.Instance.GetPassLevel() == 20 && Singleton<LevelManager>.Instance.GetNowLevel() == 20)
        {
            packagePanel.SetData(false);
        }

        if (Singleton<LevelManager>.Instance.GetNowLevel() == 3)
        {
            CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.ClearLevel_3);
        }
        else if (Singleton<LevelManager>.Instance.GetNowLevel() == 12)
        {
            CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.ClearLevel_12);
        }

        PlayerData.Instance.CompleteDailyGoal++;
    }

    public void InitBaoXiang()
    {
        int intRecord = RecordManager.GetIntRecord("UserData", "ChestGrade", 0);
        int intRecord2 = RecordManager.GetIntRecord("UserData", "ChestCount", 0);
        OpenText.text = Util.ReplaceText(GameEntry.Instance.GetString("ChestOpen"));
        switch (intRecord)
        {
            case 0:
                if (intRecord2 >= 5)
                {
                    BoxOpenObj.SetActive(value: true);
                    BoxCloseObj.SetActive(value: false);
                    break;
                }
                BoxOpenObj.SetActive(value: false);
                BoxCloseObj.SetActive(value: true);
                SliderText.text = intRecord2 + "/5";
                SliderImage.fillAmount = (float)intRecord2 / 5f;
                break;
            case 1:
                if (intRecord2 >= 10)
                {
                    BoxOpenObj.SetActive(value: true);
                    BoxCloseObj.SetActive(value: false);
                    break;
                }
                BoxOpenObj.SetActive(value: false);
                BoxCloseObj.SetActive(value: true);
                SliderText.text = intRecord2 + "/10";
                SliderImage.fillAmount = (float)intRecord2 / 10f;
                break;
            default:
                if (intRecord2 >= 15)
                {
                    BoxOpenObj.SetActive(value: true);
                    BoxCloseObj.SetActive(value: false);
                    break;
                }
                BoxOpenObj.SetActive(value: false);
                BoxCloseObj.SetActive(value: true);
                SliderText.text = intRecord2 + "/15";
                SliderImage.fillAmount = (float)intRecord2 / 15f;
                break;
        }
    }

    public void WatchVideoCallBack(bool result)
    {
        if (result)
        {
            IsWin = false;
            FreeVideo.SetActive(value: false);
            GoldIcon.transform.DOScale(new Vector2(1.2f, 1.2f), 0.2f).SetEase(Ease.OutSine);
            GoldIcon.transform.DOScale(new Vector2(1f, 1f), 0.2f).SetEase(Ease.OutSine).SetDelay(0.2f);
            GoldIcon.transform.DOScale(new Vector2(1.1f, 1.1f), 0.2f).SetEase(Ease.OutSine).SetDelay(0.4f);
            GoldIcon.transform.DOScale(new Vector2(1f, 1f), 0.2f).SetEase(Ease.OutSine).SetDelay(0.6f);
            int num = int.Parse(GoldNumText.text);
            GoldNumText.text = (num * 5).ToString();
            IsWin = true;
        }
    }

    public void InitLanguage()
    {
        textwenzi.GetComponent<Text>().text = Util.ReplaceText(GameEntry.Instance.GetString("LoseUILevelRestartText"));
        FreeVideoText.text = Util.ReplaceText(GameEntry.Instance.GetString("WinReward"));
    }

    private void ReceviceGoldChange(params object[] objs)
    {
        InitBaoXiang();
        GoldObj.transform.Find("GoldNum").GetComponent<Text>().text = Singleton<UserData>.Instance.GetUserGold().ToString();
    }

    public void Startani()
    {
        StartCoroutine(IEStartani());
    }

    public IEnumerator IEStartani()
    {
        int istart = GameScene.Instance.GetScoreStar();
        yield return new WaitForSeconds(0.5f);
        float delay = 0f;
        for (int i = 0; i < istart; i++)
        {
            GameObject gameObject = Start1;
            switch (i)
            {
                case 1:
                    gameObject = Start2;
                    break;
                case 2:
                    gameObject = Start3;
                    break;
            }
            gameObject.SetActive(value: true);
            gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
            if (i == 0)
            {
                gameObject.transform.DOScale(new Vector3(7f, 7f), 0f).SetEase(Ease.InOutSine).SetDelay(delay);
            }
            if (i == 1)
            {
                gameObject.transform.DOScale(new Vector3(6f, 6f), 0f).SetEase(Ease.InOutSine).SetDelay(delay);
            }
            if (i == 2)
            {
                gameObject.transform.DOScale(new Vector3(5f, 5f), 0f).SetEase(Ease.InOutSine).SetDelay(delay);
            }
            gameObject.transform.DOScale(new Vector3(0.7f, 0.7f), 0.3f).SetEase(Ease.InOutSine).SetDelay(delay);
            gameObject.transform.DOScale(new Vector2(1.3f, 1.3f), 0.1f).SetEase(Ease.OutSine).SetDelay(delay + 0.3f);
            Color color = gameObject.GetComponent<Image>().color;
            gameObject.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.4f);
            gameObject.GetComponent<Image>().DOColor(new Color(color.r, color.g, color.b, 1f), 0.2f).SetDelay(delay);
            gameObject.transform.DOScale(new Vector2(1f, 1f), 0.12f).SetEase(Ease.OutSine).SetDelay(delay + 0.4f);
            StartCoroutine(Playfx_end_star(i, delay, 0.4f));
            delay += 0.4f;
        }
    }

    public void WatchWinVideo()
    {
        AudioPlayManager.PlaySFX2D("button");
        // AndroidManager.Instance.ShowVideoAd("WinFreeVideo");
        ADInterface.Instance.SendADVideoEvent(ADType.RewardVideoAD, ADSpot.WinFreeVideo, WatchVideoCallBack);
    }

    private IEnumerator Playfx_end_star(int j, float stime, float stime2)
    {
        yield return new WaitForSeconds(stime);
        switch (j)
        {
            case 1:
                AudioPlayManager.PlaySFX2D("ui_winstar1");
                break;
            case 2:
                AudioPlayManager.PlaySFX2D("ui_winstar2");
                break;
            case 0:
                AudioPlayManager.PlaySFX2D("ui_winstar3");
                break;
        }
        yield return new WaitForSeconds(stime2);
        GameObject s = Start1;
        switch (j)
        {
            case 1:
                s = Start2;
                break;
            case 2:
                s = Start3;
                break;
        }
        GameObject obj = UnityEngine.Object.Instantiate(fx_end_star, base.transform.position, base.transform.rotation);
        obj.transform.parent = s.gameObject.transform;
        obj.transform.localPosition = new Vector3(0f, 0f, 0f);
        if (j == GameScene.Instance.GetScoreStar() - 1)
        {
            if (Singleton<UserData>.Instance.GetPassLevel() >= 21 && GameScene.Instance.BeforeStar < 3 && GameScene.Instance.GetScoreStar() > GameScene.Instance.BeforeStar)
            {
                PlayJinzuSpine();
            }
            else
            {
                JinZuEnd(playFX: false);
            }
        }
    }

    private void PlayJinzuSpine()
    {
        JinzuSpine.SetActive(value: true);
        bool isNext = true;
        JinzuSpine.GetComponent<SkeletonGraphic>().timeScale = 1.4f;
        JinzuSpine.GetComponent<SkeletonGraphic>().AnimationState.Event += State_Event;
        JinzuSpine.GetComponent<SkeletonGraphic>().AnimationState.Complete += delegate
        {
            if (isNext)
            {
                PlayNextJinzu();
                isNext = false;
            }
        };
    }

    private void PlayNextJinzu()
    {
        if (index >= 1 && index <= 3)
        {
            if (GameScene.Instance.BeforeStar == 1 && GameScene.Instance.GetScoreStar() > 1 && index == 1)
            {
                index = 2;
            }
            else if (GameScene.Instance.BeforeStar == 2 && GameScene.Instance.GetScoreStar() > 2)
            {
                index = 3;
            }
            int num = index;
            if (index == 2)
            {
                num = 3;
            }
            if (index == 3)
            {
                num = 2;
            }
            if (Singleton<UserData>.Instance.GetJinZhuGold() < 6000)
            {
                JinzuSpine.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "fx_jiesuan_zuanshi_ruguan0" + num, loop: false);
            }
            else
            {
                JinzuSpine.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "fx_feizhu_zuanshi_hit0" + num, loop: false);
            }
        }
    }

    private void State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
    {
        if (e.Data.Name == "end")
        {
            StateEnd();
        }
    }

    public void StateEnd()
    {
        index++;
        PlayNextJinzu();
        if (index == 2)
        {
            if (GameScene.Instance.BeforeStar == 0)
            {
                Singleton<UserData>.Instance.AddJinZhuGold(200);
            }
        }
        else if (index == 3)
        {
            if (GameScene.Instance.BeforeStar <= 1)
            {
                Singleton<UserData>.Instance.AddJinZhuGold(200);
            }
        }
        else if (index == 4 && GameScene.Instance.BeforeStar <= 2)
        {
            Singleton<UserData>.Instance.AddJinZhuGold(200);
        }
        if (GameScene.Instance.GetScoreStar() + 1 == index)
        {
            JinZuEnd();
        }
    }

    public void JinZuEnd(bool playFX = true)
    {
        if (playFX)
        {
            JinzuSpine.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "fx_jinzhu_end", loop: false);
        }
        //任务按钮第6关开始显示
        if (Singleton<UserData>.Instance.GetPassLevel() >= 6)
        {
            missionBtn.SetActive(value: true);
            missionBtnTip.SetActive(CheckMissionTip());
        }
        //商城按钮第13关开始显示
        if (Singleton<UserData>.Instance.GetPassLevel() >= 13)
        {
            shopBtn.SetActive(value: true);
            shopBtnTip.SetActive(CheckShopBtnTip());
        }
        Timer.DelayCallBack(1f, delegate
        {
            JinzuSpine.SetActive(value: false);
            UnityEngine.Debug.Log(" =====   " + Singleton<LevelManager>.Instance.GetNowLevel());
            if (!Singleton<UserData>.Instance.GetWinByLevel(Singleton<LevelManager>.Instance.GetNowLevel()))
            {
                Singleton<UserData>.Instance.SetWinByLevel(Singleton<LevelManager>.Instance.GetNowLevel());
                GoldNumText.text = "30";
                bool flag = true;
                FreeVideo.SetActive(flag);
                FreeVideo.GetComponentInChildren<Button>().enabled = flag;
            }
            else
            {
                GoldNumText.text = "0";
                FreeVideo.SetActive(value: false);
            }
            // textwenzi.SetActive(value: true);

            continuneBtn.SetActive(value: true);
            homeBtn.SetActive(value: true);
            if (GoldNumText.text == "0")
            {
                GoldIcon.SetActive(value: false);
            }
            else
            {
                GoldIcon.SetActive(value: true);
            }
            IsWin = true;

            if (Singleton<UserData>.Instance.GetPassLevel() == 1 && !PlayerData.Instance.IsSignGuide)
            {
                tip1.SetActive(true);
                homeBtn.SetActive(false);
                Sequence seq = DOTween.Sequence();
                seq.Append(tip1.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 10), 0.2f));
                seq.Append(tip1.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, -20), 0.2f));
                seq.Append(tip1.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 10), 0.2f));
                seq.AppendInterval(3f);
                seq.SetLoops(-1);
            }
            else if (Singleton<UserData>.Instance.GetPassLevel() == 2 && !PlayerData.Instance.IsLotteryGuide)
            {
                tip2.SetActive(true);
                homeBtn.SetActive(false);
                Sequence seq = DOTween.Sequence();
                seq.Append(tip2.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 10), 0.2f));
                seq.Append(tip2.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, -20), 0.2f));
                seq.Append(tip2.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 10), 0.2f));
                seq.AppendInterval(3f);
                seq.SetLoops(-1);
            }
            else if (Singleton<UserData>.Instance.GetPassLevel() == 5 && !PlayerData.Instance.IsMissionGuide)
            {
                tip4.SetActive(true);
                homeBtn.SetActive(false);
                Sequence seq = DOTween.Sequence();
                seq.Append(tip4.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 10), 0.2f));
                seq.Append(tip4.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, -20), 0.2f));
                seq.Append(tip4.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 10), 0.2f));
                seq.AppendInterval(3f);
                seq.SetLoops(-1);
            }
            else if (Singleton<UserData>.Instance.GetPassLevel() == 12 && !PlayerData.Instance.IsShopGuide)
            {
                tip3.SetActive(true);
                homeBtn.SetActive(false);
                Sequence seq = DOTween.Sequence();
                seq.Append(tip3.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 10), 0.2f));
                seq.Append(tip3.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, -20), 0.2f));
                seq.Append(tip3.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 10), 0.2f));
                seq.AppendInterval(3f);
                seq.SetLoops(-1);
            }
        });
    }

    public GameObject CandyTouchChecker(Vector3 mouseposition)
    {
        Vector3 vector = Camera.main.ScreenToWorldPoint(mouseposition);
        Vector2 point = new Vector2(vector.x, vector.y);
        if ((bool)Physics2D.OverlapPoint(point))
        {
            return Physics2D.OverlapPoint(point).gameObject;
        }
        vector = Camera.main.WorldToScreenPoint(mouseposition);
        point = new Vector2(vector.x, vector.y);
        if ((bool)Physics2D.OverlapPoint(point))
        {
            return Physics2D.OverlapPoint(point).gameObject;
        }
        vector = Camera.main.WorldToViewportPoint(mouseposition);
        point = new Vector2(vector.x, vector.y);
        if ((bool)Physics2D.OverlapPoint(point))
        {
            return Physics2D.OverlapPoint(point).gameObject;
        }
        vector = Camera.main.ViewportToWorldPoint(mouseposition);
        point = new Vector2(vector.x, vector.y);
        if ((bool)Physics2D.OverlapPoint(point))
        {
            return Physics2D.OverlapPoint(point).gameObject;
        }
        vector = Camera.main.ScreenToViewportPoint(mouseposition);
        point = new Vector2(vector.x, vector.y);
        if ((bool)Physics2D.OverlapPoint(point))
        {
            return Physics2D.OverlapPoint(point).gameObject;
        }
        return null;
    }

    public void FlyCoins(bool home = false)
    {
        BaoixngBG.GetComponentInChildren<Button>().enabled = false;
        FreeVideo.GetComponentInChildren<Button>().enabled = false;
        int num = int.Parse(GoldNumText.text);
        MissionCheckMgr.Instance.CheckMission(MissionType.GetCoin, num);
        float num2 = 0f;
        gridBox.Clear();
        // textwenzi.SetActive(value: false);
        shopBtn.SetActive(value: false);
        continuneBtn.SetActive(value: false);
        missionBtn.SetActive(value: false);
        homeBtn.SetActive(value: false);
        Vector3 localPosition = GoldIcon.transform.localPosition;

        if (home)
        {
            //Invoke("HomeScene", (float)(num / 5) * 0.2f + 1f);
            Invoke("HomeScene", 6f * 0.2f + 1f);
        }
        else
        {
            //StartCoroutine(ShowPlay((float)(num / 5) * 0.2f + 1f));
            StartCoroutine(ShowPlay(6f * 0.2f + 1f));
        }
        if (num != 150)
        {
            for (int i = 0; i < num / 5; i++)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate(CoinsObj, CoinsPos.transform.parent.parent);
                GoldNumText.text = string.Empty + (num - i * 5 - 5);
                gameObject.transform.localPosition = localPosition;
                gridBox.Add(gameObject);
                CoinFly component = gridBox[i].GetComponent<CoinFly>();
                component.Init(GoldObj.transform.Find("GoldImage").transform.parent.gameObject, num2, false);
                num2 += 0.2f;
            }
        }
        else
        {
            for (int i = 0; i < num / 25; i++)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate(CoinsObj, CoinsPos.transform.parent.parent);
                GoldNumText.text = string.Empty + (num - i * 25 - 25);
                gameObject.transform.localPosition = localPosition;
                gridBox.Add(gameObject);
                CoinFly component = gridBox[i].GetComponent<CoinFly>();
                component.Init(GoldObj.transform.Find("GoldImage").transform.parent.gameObject, num2, true);
                num2 += 0.2f;
            }
        }
        GoldIcon.SetActive(value: false);
    }

    public void HomeScene()
    {
        GameEntry.ChangeScene(GameEntry.LevelScene);
    }

    public IEnumerator ShowPlay(float time)
    {
        yield return new WaitForSeconds(time);
        UIManager.OpenUIWindow<PlayWindow>();
    }

    public IEnumerator ShowAdChest(float time)
    {
        yield return new WaitForSeconds(time);
        UIManager.OpenUIWindow<AdChestWindow>();
    }
    public void ShopBtnClick()
    {
        UIManager.OpenUIWindow<DiscountShopWindow>();
    }

    public void MissionBtnClick()
    {
        UIManager.OpenUIWindow<MissionWindow>();
    }

    public void BuyGoldBtnClick()
    {
        UIManager.OpenUIWindow<BuyGoldUIWindow>();
    }
    private bool CheckMissionTip()
    {
        MissionInfoParam missionInfo = PlayerData.Instance.GetMissionInfo();
        for (var i = 0; i < missionInfo.countList.Count; i++)
        {
            if (missionInfo.finishStateList[i] == false)
            {
                if (missionInfo.countList[i] >= MissionData.Instance.GetCount(i + 1))
                    return true;
            }
        }
        return false;
    }
    private bool CheckShopBtnTip()
    {
        for (var i = 0; i < PlayerData.Instance.DiscountShopList.Count; i++)
        {
            if (PlayerData.Instance.DiscountShopList[i].hasBuy == false)
            {
                return true;
            }
        }
        return false;
    }
}
