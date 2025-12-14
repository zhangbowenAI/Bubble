
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelBtnScriptMin : MonoBehaviour
{
    public Sprite PassLevelBtn;

    public Sprite NoPassLevelBtn;

    public GameObject ObjImage;

    public void SetPassLevel()
    {
        ObjImage.GetComponent<Image>().sprite = PassLevelBtn;
    }

    public void SetNoPassLevel()
    {
        ObjImage.GetComponent<Image>().sprite = NoPassLevelBtn;
    }

    public void WaitPass(float fTime)
    {
        StartCoroutine(IEWaitPass(fTime));
    }

    private IEnumerator IEWaitPass(float fTime)
    {
        yield return new WaitForSeconds(fTime);
        SetPassLevel();
    }

    public void Unlocked()
    {
    }

    public void UpdateBtnScale(float fScale)
    {
        base.transform.localScale = new Vector2(fScale, fScale);
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}
