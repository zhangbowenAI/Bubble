
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    public Text scoreText;

    public void InitScore(int score, int type)
    {
        scoreText.DOFade(1f, 0f);
        base.transform.localScale = new Vector3(1f, 1f, 1f);
        Transform transform = base.transform;
        Vector3 localPosition = base.transform.localPosition;
        transform.DOLocalMoveY(localPosition.y + 50f, 1f);
        scoreText.DOFade(0f, 1f).SetDelay(0.3f);
        switch (type)
        {
            case 1:
            case 11:
                scoreText.text = score.ToString();
                scoreText.fontSize = 30;
                base.transform.DOScale(1.3f, 0.1f).SetDelay(0.8f);
                break;
            case 2:
                scoreText.text = Util.ReplaceText(GameEntry.Instance.GetString("KillBubble1"));
                scoreText.fontSize = 60;
                break;
            case 3:
                scoreText.text = Util.ReplaceText(GameEntry.Instance.GetString("KillBubble2"));
                scoreText.fontSize = 60;
                break;
            case 4:
                scoreText.text = Util.ReplaceText(GameEntry.Instance.GetString("KillBubble3"));
                scoreText.fontSize = 60;
                break;
            case 5:
                scoreText.text = Util.ReplaceText(GameEntry.Instance.GetString("KillBubble4"));
                scoreText.fontSize = 60;
                break;
            case 6:
                scoreText.text = Util.ReplaceText(GameEntry.Instance.GetString("KillBubble5"));
                scoreText.fontSize = 60;
                break;
            case 7:
                scoreText.text = Util.ReplaceText(GameEntry.Instance.GetString("KillBubble6"));
                scoreText.fontSize = 60;
                break;
            case 8:
                scoreText.text = Util.ReplaceText(GameEntry.Instance.GetString("KillBubble7"));
                scoreText.fontSize = 60;
                break;
            case 9:
                scoreText.text = Util.ReplaceText(GameEntry.Instance.GetString("KillBubble8"));
                scoreText.fontSize = 60;
                break;
            case 10:
                scoreText.text = Util.ReplaceText(GameEntry.Instance.GetString("KillBubble9"));
                scoreText.fontSize = 60;
                break;
        }
    }
}
