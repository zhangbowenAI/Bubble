using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Splash : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject splashText;

    private void Awake()
    {
        ADInterface.Instance.Init();
        PlatformSetting.Instance.Init();
        GarbageCodeInterface.Instance.Next();
        PlayerData.Instance.Init();
        MissionCheckMgr.Instance.Init();
        AtlasManager.Instance.Init();

        BuildSetting.Instance.IsShowCopyrightEvent += ShowCopyrightEvent;

#if UNITY_EDITOR
        splashText.SetActive(BuildSetting.Instance.showCompanyInfo);
#endif

        splashText.GetComponent<Text>().text = BuildSetting.Instance.isVivo ? "著作权人:广州皓棠互联网科技有限公司\n出版单位 : 广州隆达信息科技有限公司" : "著作权人:广州皓棠互联网科技有限公司\n出版单位 : 广州皓棠互联网科技有限公司";
        Invoke("StartGame", 2);

        DateTime lastLoginDateTime = WUtil.TimeStamp2DateTime(PlayerData.Instance.LoginTS);
        DateTime nowDateTime = DateTime.Now;
        if (lastLoginDateTime.Year != nowDateTime.Year || lastLoginDateTime.Month != nowDateTime.Month || lastLoginDateTime.Day != nowDateTime.Day)
        {
            PlayerData.Instance.LoginTS = WUtil.DateTime2TimeStamp(nowDateTime);
            DiscountShopData.Instance.RefreshItemList();
            PlayerData.Instance.DailyGoalDay++;
            PlayerData.Instance.HasGetShopCoin = false;
            if (PlayerData.Instance.DailyGoalDay == 1)
                PlayerData.Instance.DailyGoalCount = 12;
            else if (PlayerData.Instance.DailyGoalDay == 2)
                PlayerData.Instance.DailyGoalCount = PlayerData.Instance.CompleteDailyGoal + 8;
            else
                PlayerData.Instance.DailyGoalCount = PlayerData.Instance.CompleteDailyGoal + 6;
        }

        if (!PlayerData.Instance.FirstInit)
        {
            CollectInfoEvent.SendEvent(CollectInfoEvent.EventType.FirstStart);
        }
    }

    public void ShowCopyrightEvent(bool isShow)
    {
        splashText.SetActive(isShow);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        ADInterface.Instance.SendADEvent(ADType.ShowBanner, ADSpot.GameBanner);
    }
}
