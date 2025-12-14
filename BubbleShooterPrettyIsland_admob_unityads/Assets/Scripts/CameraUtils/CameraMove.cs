
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : Singleton<CameraMove>
{
    private bool mapMoving;

    private float height = -10000f;

    private float speed = 5f;

    private float moveY;

    public bool isQuick;

    private BubbleObj bubbleObj;

    public void InitData()
    {
        height = -10000f;
        mapMoving = false;
        speed = 5f;
        isQuick = false;
    }

    public void UpMove()
    {
        int maxRow = GetMaxRow();
        // if (height == -10000f)
        // {
        //     Vector3 position = GameManager.Instance.GetBox(new Point(6, 0)).transform.position;
        //     height = position.y;
        // }
        Vector3 position2 = GameManager.Instance.GetBox(new Point(maxRow, 0)).transform.position;
        float y = position2.y;
        // if (y < height)
        // {
        //     moveY = 0f - (height - y);
        //     mapMoving = true;
        // }
        // else
        // {
        //     moveY = 0f;
        //     mapMoving = true;
        // }
        moveY = y - 0.8f;
        // float maxMove = height - 2;
        // Debug.LogError(height + "   " + moveY);
        if (moveY > 1.95f)
        {
            moveY = 1.95f;
        }
        if (GameScene.Instance.initEnd)
        {
            speed = 1.2f;
        }
        Vector2 a = Camera.main.transform.position;
        Vector3 position3 = Camera.main.transform.position;
        float x = position3.x;
        // float y2 = moveY;
        // Vector3 position4 = Camera.main.transform.position;
        float num = Vector2.Distance(a, new Vector3(x, moveY, position3.z));
        // Transform transform = Camera.main.transform;
        // Vector3 position5 = Camera.main.transform.position;
        // float x2 = position5.x;
        // float y3 = moveY;
        // Vector3 position6 = Camera.main.transform.position;
        Camera.main.transform.DOMove(new Vector3(x, moveY, position3.z), num / speed).OnComplete(MoveEnd);
    }

    public void QuickMove()
    {
        if (!isQuick)
        {
            isQuick = true;
            mapMoving = true;
            Camera.main.transform.DOKill();
            Vector2 a = Camera.main.transform.position;
            Vector3 position = Camera.main.transform.position;
            float x = position.x;
            float y = moveY;
            // Vector3 position2 = Camera.main.transform.position;
            float num = Vector2.Distance(a, new Vector3(x, y, position.z));
            // Transform transform = Camera.main.transform;
            // Vector3 position3 = Camera.main.transform.position;
            // float x2 = position3.x;
            // float y2 = moveY;
            // Vector3 position4 = Camera.main.transform.position;
            Camera.main.transform.DOMove(new Vector3(x, moveY, position.z), num / (speed * 2f)).OnComplete(MoveEnd);
        }
    }

    public bool GetMapMoving()
    {
        return mapMoving;
    }

    public void MoveEnd()
    {
        isQuick = false;
        mapMoving = false;
        GameScene.Instance.MoveEnd();
    }

    public int GetMaxRow()
    {
        int num = 0;
        List<BubbleObj> bubbleArray = GameManager.Instance.GetBubbleArray();
        for (int i = 0; i < bubbleArray.Count; i++)
        {
            bubbleObj = bubbleArray[i];
            if (bubbleObj.GetBubbleState() == BubbleState.None && bubbleObj.point.X > num)
            {
                num = bubbleObj.point.X;
            }
        }
        return num;
    }
}
