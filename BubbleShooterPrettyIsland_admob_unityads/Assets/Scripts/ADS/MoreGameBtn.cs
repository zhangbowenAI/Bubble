using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreGameBtn : MonoBehaviour
{
    public GameObject moreGameBtn;
    private void Awake()
    {
        moreGameBtn.SetActive(BuildSetting.Instance.showMoreGame);
    }

    public void BtnOnclick()
    {
        PlatformSetting.Instance.MoreGame();
    }
}
