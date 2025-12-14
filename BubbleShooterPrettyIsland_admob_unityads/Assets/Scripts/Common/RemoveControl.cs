
using System.Collections.Generic;

public class RemoveControl : Singleton<RemoveControl>
{
	private bool isCheckRemove;

	private bool isPlayMove;

	public bool isCombo;

	private List<BubbleObj> skillObj = new List<BubbleObj>();

	private BubbleObj bubbleObj;

	private BoxObj box;

	private BubbleObj pCurBubble;

	public bool GetCheckRemove()
	{
		return isCheckRemove;
	}

	public void CheckRemove(BubbleObj bubble)
	{
		isCheckRemove = true;
		isCombo = false;
		bool flag = false;
		List<Point> list = findClearBubble(bubble);
		if (bubble.GetIsSkill())
		{
			isCombo = true;
			int num = 0;
			if (bubble.GetSkill(1))
			{
				num++;
			}
			if (bubble.GetSkill(2))
			{
				num++;
			}
			if (bubble.GetSkill(3))
			{
				num++;
			}
			if (bubble.GetSkill(4))
			{
				num++;
			}
			flag = true;
			list.Clear();
			bubble.CheckGorundByHit(bubble.GetBubbleData());
			bubble.SetRemoveIndex(1);
			if (num >= 3)
			{
				if (num == 4)
				{
					GameScene.Instance.AddScore(0, null, 6);
					bubble.isSkillCount4 = true;
				}
				else
				{
					GameScene.Instance.AddScore(0, null, 5);
					bubble.isSkillCount3 = true;
				}
				GameManager.Instance.iSkill4Count = 0;
				skillObj.Clear();
				bubble.SetRemoveIndex(2);
				bubble.RemoveBubble(RemoveType.None);
			}
			else if (GameManager.Instance.iSkill4Count == 1)
			{
				skillObj.Add(bubble);
			}
			else if (GameManager.Instance.iSkill4Count == 2)
			{
				skillObj.Add(bubble);
			}
			else if (GameManager.Instance.iSkill4Count == 3)
			{
				skillObj.Add(bubble);
				for (int i = 0; i < skillObj.Count; i++)
				{
					if (skillObj[i] != null && skillObj[i].GetBubbleState() == BubbleState.None)
					{
						skillObj[i].RemoveBubble(RemoveType.None);
					}
				}
				skillObj.Clear();
				GameManager.Instance.iSkill4Count = 0;
			}
			else
			{
				skillObj.Clear();
				bubble.RemoveBubble(RemoveType.None);
			}
		}
		else if (bubble.GetProp(1))
		{
			isCombo = true;
			flag = true;
			list.Clear();
			bubble.CheckGorundByHit(bubble.GetBubbleData());
			bubble.SetRemoveIndex(1);
			bubble.RemoveBubble(RemoveType.None);
		}
		else if (bubble.GetProp(3))
		{
			isCombo = true;
			flag = true;
			list.Clear();
			bubble.CheckGorundByHit(bubble.GetBubbleData());
			bubble.SetRemoveIndex(1);
			bubble.RemoveBubble(RemoveType.None);
		}
		if (list.Count >= 3)
		{
			bubble.CheckGorundByHit(bubble.GetBubbleData());
			bubble.SetRemoveIndex(0);
			for (int j = 0; j < list.Count; j++)
			{
				if (GameManager.Instance.GetBox(list[j]) != null)
				{
					GameManager.Instance.GetBox(list[j]).GetBubble().RemoveBubble(RemoveType.None);
				}
			}
			GameManager.Instance.iCombo++;
			GameScene.Instance.ShowCombo(GameManager.Instance.iCombo);
			if (GameManager.Instance.iCombo == 5)
			{
				AudioPlayManager.PlaySFX2D("combo3");
			}
			if (GameManager.Instance.iCombo >= 5)
			{
				GameScene.Instance.gameCombo5.SetActive(value: true);
			}
		}
		else
		{
			AudioPlayManager.PlaySFX2D("b_hit_no_match");
			bubble.CheckGorundByHit(bubble.GetBubbleData(), isRemoveSlef: true);
			if (!isCombo)
			{
				GameManager.Instance.iCombo = 0;
				GameScene.Instance.gameCombo5.SetActive(value: false);
			}
			else if (GameManager.Instance.iSkill4Count != 0)
			{
				if (GameManager.Instance.iSkill4Count == 3)
				{
					GameManager.Instance.iCombo++;
					GameScene.Instance.ShowCombo(GameManager.Instance.iCombo);
				}
			}
			else
			{
				GameManager.Instance.iCombo++;
				GameScene.Instance.ShowCombo(GameManager.Instance.iCombo);
			}
		}
		if (GameManager.Instance.iCombo == 5 && GameManager.Instance.iSkill4Count == 0)
		{
			GameScene.Instance.AddScore(0, null, 2);
		}
		int num2 = 0;
		for (int k = 0; k < list.Count; k++)
		{
			if (GameManager.Instance.GetBox(list[k]).GetBubble() != null && (GameManager.Instance.GetBox(list[k]).GetBubble().GetAttributes() == 100 || GameManager.Instance.GetBox(list[k]).GetBubble().GetAttributes() == 101))
			{
				num2++;
			}
		}
		if (num2 >= 6)
		{
			GameScene.Instance.AddScore(0, null, 4);
		}
		else if (num2 >= 3)
		{
			GameScene.Instance.AddScore(0, null, 3);
		}
		if (list.Count >= 17)
		{
			GameScene.Instance.GirlAni_lively1();
		}
		else if (list.Count >= 3)
		{
			GameScene.Instance.GirlAni_fault(b: true);
		}
		else
		{
			GameScene.Instance.GirlAni_fault();
		}
		int num3 = list.Count;
		if (num3 > 10)
		{
			num3 -= 5;
		}
		if (flag)
		{
			num3 = 15;
		}
		CheckFall(num3);
	}

	public void CheckFall(int iCount)
	{
		GameManager.Instance.SetDelayTime((float)iCount * GameManager.removeTime);
		if (iCount == -1)
		{
			if (!GameManager.Instance.GetDelayGame2())
			{
				FallBubble();
				GameManager.Instance.ChangeReadyBubble();
				isPlayMove = true;
				Singleton<CameraMove>.Instance.UpMove();
			}
		}
		else
		{
			if (iCount < 2)
			{
				iCount = 2;
			}
			Timer.DelayCallBack((float)iCount * GameManager.removeTime, delegate
			{
				if (!GameManager.Instance.GetDelayGame2())
				{
					FallBubble();
					GameManager.Instance.ChangeReadyBubble();
					isPlayMove = true;
					Singleton<CameraMove>.Instance.UpMove();
				}
			});
		}
	}

	public void MoveEnd()
	{
		if (isPlayMove)
		{
			isPlayMove = false;
			UpBubbleFunction();
			isCheckRemove = false;
		}
	}

	public void UpBubbleFunction()
	{
		List<BubbleObj> bubbleArray = GameManager.Instance.GetBubbleArray();
		for (int i = 0; i < bubbleArray.Count; i++)
		{
			bubbleObj = bubbleArray[i];
			if (bubbleObj.GetBubbleState() == BubbleState.None)
			{
				bubbleObj.UpFunction();
			}
		}
	}

	public void FallBubble()
	{
		List<Point> list = checkFallBubble();
		if (list.Count <= 0)
		{
			return;
		}
		list = checkFallBubbleGuadian(list);
		if (list.Count > 16)
		{
			GameScene.Instance.AddScore(0, null, 10);
		}
		else if (list.Count > 6)
		{
			GameScene.Instance.AddScore(0, null, 9);
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (GameManager.Instance.GetBox(list[i]) != null && GameManager.Instance.GetBox(list[i]).GetBubble() != null)
			{
				GameManager.Instance.GetBox(list[i]).GetBubble().FallBubble();
			}
		}
	}

	private List<Point> findClearBubble(BubbleObj pReadyBubble)
	{
		List<Point> list = findSameBubble(pReadyBubble);
		if (list.Count < 3)
		{
			list.Clear();
		}
		return list;
	}

	private List<Point> findSameBubble(BubbleObj pReadyBubble)
	{
		List<Point> list = new List<Point>();
		list.Add(pReadyBubble.point);
		int num = 0;
		bool flag = false;
		while (!flag)
		{
			num++;
			List<Point> sameBubble = GetSameBubble(list, pReadyBubble.GetBubbleType());
			if (sameBubble.Count > 0)
			{
				for (int i = 0; i < sameBubble.Count; i++)
				{
					box = GameManager.Instance.GetBox(sameBubble[i]);
					if (box != null && box.GetBubble() != null && box.GetBubble().GetCanChecRemove() && box.GetBubble().GetCanCheckMatch3Remove())
					{
						box.GetBubble().SetRemoveIndex(num);
					}
					list.Add(sameBubble[i]);
				}
			}
			else
			{
				flag = true;
			}
		}
		return list;
	}

	public List<Point> GetSameBubble(List<Point> _list, int mType)
	{
		List<Point> list = new List<Point>();
		for (int i = 0; i < _list.Count; i++)
		{
			List<Point> around = GameManager.Instance.GetAround(_list[i]);
			for (int j = 0; j < around.Count; j++)
			{
				Point point = around[j];
				int x = point.X;
				Point point2 = around[j];
				if (_list.Contains(new Point(x, point2.Y)))
				{
					continue;
				}
				List<Point> list2 = list;
				Point point3 = around[j];
				int x2 = point3.X;
				Point point4 = around[j];
				if (list2.Contains(new Point(x2, point4.Y)))
				{
					continue;
				}
				box = GameManager.Instance.GetBox(around[j]);
				if (box != null && box.GetBubble() != null && box.GetBubble().GetCanCheckMatch3Remove())
				{
					pCurBubble = box.GetBubble();
					if (pCurBubble.GetBubbleType() == mType)
					{
						list.Add(pCurBubble.point);
					}
				}
			}
		}
		return list;
	}

	private void RmoveNullGuadian()
	{
		List<Point> list = new List<Point>();
		List<BubbleObj> bubbleArray = GameManager.Instance.GetBubbleArray();
		for (int i = 0; i < bubbleArray.Count; i++)
		{
			bubbleObj = bubbleArray[i];
			if (bubbleObj.GetBubbleState() == BubbleState.None && bubbleObj.GetAttributes() == 5)
			{
				list.Add(bubbleObj.point);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			box = GameManager.Instance.GetBox(list[j]);
			int num = 0;
			if (!(box != null) || !(box.GetBubble() != null))
			{
				continue;
			}
			List<Point> around = GameManager.Instance.GetAround(list[j]);
			for (int k = 0; k < around.Count; k++)
			{
				BoxObj boxObj = GameManager.Instance.GetBox(around[k]);
				if (boxObj != null && boxObj.GetBubble() != null)
				{
					num++;
					break;
				}
			}
			if (num == 0)
			{
				box.GetBubble().RemoveBubble(RemoveType.BeRmove);
			}
		}
	}

	private List<Point> checkFallBubble()
	{
		RmoveNullGuadian();
		for (int i = 0; i < GameManager.rows; i++)
		{
			for (int j = 0; j < GameManager.cols - i % 2; j++)
			{
				box = GameManager.Instance.GetBox(new Point(i, j));
				if (box != null && box.GetBubble() != null)
				{
					box.GetBubble().isFind = false;
				}
			}
		}
		Dictionary<Point, int> dictionary = new Dictionary<Point, int>();
		List<Point> list = new List<Point>();
		for (int k = 0; k < GameManager.cols; k++)
		{
			box = GameManager.Instance.GetBox(new Point(0, k));
			if (box != null && box.GetBubble() != null)
			{
				pCurBubble = box.GetBubble();
				if (pCurBubble.GetBubbleState() == BubbleState.None)
				{
					dictionary.Add(new Point(0, k), 1);
					list.Add(new Point(0, k));
				}
			}
		}
		int maxRow = Singleton<CameraMove>.Instance.GetMaxRow();
		List<Point> list2 = new List<Point>();
		if (list.Count == 0)
		{
			for (int l = 0; l <= maxRow; l++)
			{
				for (int m = 0; m < GameManager.cols - l % 2; m++)
				{
					box = GameManager.Instance.GetBox(new Point(l, m));
					if (box != null && box.GetBubble() != null)
					{
						list2.Add(new Point(l, m));
					}
				}
			}
			return list2;
		}
		for (int n = 0; n < list.Count; n++)
		{
			List<Point> around = GameManager.Instance.GetAround(list[n]);
			for (int num = 0; num < around.Count; num++)
			{
				box = GameManager.Instance.GetBox(around[num]);
				if (box != null && box.GetBubble() != null && !box.GetBubble().isFind)
				{
					pCurBubble = box.GetBubble();
					pCurBubble.isFind = true;
					if (pCurBubble.GetBubbleState() == BubbleState.None && !dictionary.ContainsKey(pCurBubble.point))
					{
						dictionary.Add(pCurBubble.point, 1);
						list.Add(pCurBubble.point);
					}
				}
			}
		}
		maxRow = Singleton<CameraMove>.Instance.GetMaxRow();
		for (int num2 = 0; num2 <= maxRow; num2++)
		{
			for (int num3 = 0; num3 < GameManager.cols - num2 % 2; num3++)
			{
				box = GameManager.Instance.GetBox(new Point(num2, num3));
				if (box != null && box.GetBubble() != null)
				{
					pCurBubble = box.GetBubble();
					if (pCurBubble.GetBubbleState() == BubbleState.None && !dictionary.ContainsKey(pCurBubble.point))
					{
						list2.Add(pCurBubble.point);
					}
				}
			}
		}
		dictionary = null;
		return list2;
	}

	private List<Point> checkFallBubbleGuadian(List<Point> NoLinkBubbleList)
	{
		for (int i = 0; i < NoLinkBubbleList.Count; i++)
		{
			box = GameManager.Instance.GetBox(NoLinkBubbleList[i]);
			if (box != null && box.GetBubble() != null)
			{
				box.GetBubble().isFind = false;
			}
		}
		List<Point> list = new List<Point>();
		List<Point> list2 = new List<Point>();
		for (int j = 0; j < NoLinkBubbleList.Count; j++)
		{
			box = GameManager.Instance.GetBox(NoLinkBubbleList[j]);
			if (box != null && box.GetBubble() != null && box.GetBubble().GetAttributes() == 5)
			{
				box.GetBubble().isFind = true;
				list2.Add(NoLinkBubbleList[j]);
			}
		}
		Dictionary<Point, int> dictionary = new Dictionary<Point, int>();
		for (int k = 0; k < list2.Count; k++)
		{
			List<Point> around = GameManager.Instance.GetAround(list2[k]);
			for (int l = 0; l < around.Count; l++)
			{
				box = GameManager.Instance.GetBox(around[l]);
				if (box != null && box.GetBubble() != null && !box.GetBubble().isFind)
				{
					pCurBubble = box.GetBubble();
					pCurBubble.isFind = true;
					if (pCurBubble.GetBubbleState() == BubbleState.None && !dictionary.ContainsKey(pCurBubble.point))
					{
						dictionary.Add(pCurBubble.point, 1);
						list2.Add(pCurBubble.point);
					}
				}
			}
		}
		for (int m = 0; m < NoLinkBubbleList.Count; m++)
		{
			box = GameManager.Instance.GetBox(NoLinkBubbleList[m]);
			if (box != null && box.GetBubble() != null && !box.GetBubble().isFind)
			{
				BubbleObj bubble = box.GetBubble();
				if (bubble.GetBubbleState() == BubbleState.None && !dictionary.ContainsKey(bubble.point))
				{
					dictionary.Add(bubble.point, 1);
					list.Add(bubble.point);
				}
			}
		}
		return list;
	}
}
