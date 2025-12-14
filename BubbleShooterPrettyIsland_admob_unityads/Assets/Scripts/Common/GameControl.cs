
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
	public static GameControl Instance;

	public GameObject TipPoint;

	public GameObject HitPoint;

	public GameObject HitPoint2;

	public GameObject fx_qiufashe_lizi;

	public LineManager line1_1;

	public LineManager line1_2;

	public LineManager line1_3;

	public LineManager line2;

	public GameObject line1_fx_1;

	public GameObject line1_fx_2;

	public GameObject line1_fx_3;

	private GameObject mMoveBubble;

	private PoolObject ObjTmp;

	private BubbleObj bubbleObj;

	private Point endPoint;

	public bool isShooting;

	private List<Vector2> movePos = new List<Vector2>();

	private Vector2 endNormalized = Vector2.zero;

	private int timeIndex;

	private bool isReadyShoot;

	private bool cancel;

	private Vector2 hitPos;

	private BubbleObj hitBubble;

	private int ShootType;

	private BoxObj box;

	private BubbleObj pCurBubble;

	public void Awake()
	{
		Instance = this;
		line1_1.gameObject.SetActive(value: false);
		line1_2.gameObject.SetActive(value: false);
		line1_3.gameObject.SetActive(value: false);
		line2.gameObject.SetActive(value: false);
		fx_qiufashe_lizi.SetActive(value: false);
	}

	public void OnUpdate()
	{
		if (GameGuid.Instance != null && !GameGuid.Instance.GetShootBubble())
		{
			if (Input.GetMouseButtonUp(0))
			{
				GameGuid.Instance.TouchEnd();
			}
		}
		else
		{
			if (isShooting || ReadyBubbleController.Instance.ReadyBubble1 == null)
			{
				return;
			}
			if (Input.GetMouseButton(0))
			{
				hitBubble = null;
				Vector2 a = UnityEngine.Input.mousePosition;
				Vector2 b = Camera.main.WorldToScreenPoint(ReadyBubbleController.Instance.Ready1Parent.transform.position);
				Vector2 normalized = (a - b).normalized;
				if (normalized.y < 0.31f)
				{
					movePos.Clear();
					HideLine();
					CancelFireBubble(1);
					return;
				}
				movePos.Clear();
				Vector2 a2 = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
				Vector2 vector = ReadyBubbleController.Instance.Ready1Parent.transform.position;
				Vector2 normalized2 = (a2 - vector).normalized;
				Vector2 end = vector + normalized2 * 30f;
				movePos.Add(vector);
				if (ReadyBubbleController.Instance.ReadyBubble1.GetComponent<BubbleObj>().GetProp(2))
				{
					RaycastHitByProp2(vector, end, normalized2);
					TipPoint.gameObject.SetActive(value: false);
					HitPoint.gameObject.SetActive(value: false);
					HitPoint2.gameObject.SetActive(value: false);
				}
				else
				{
					bool flag = RaycastHit(vector, end, normalized2);
					int num = 0;
					while (flag)
					{
						if (num > 3)
						{
							movePos.Clear();
							break;
						}
						if (movePos.Count == 0)
						{
							break;
						}
						normalized2.x = 0f - normalized2.x;
						vector = movePos[movePos.Count - 1] + normalized2 * 0.01f;
						end = vector + normalized2 * 20f;
						flag = RaycastHit(vector, end, normalized2);
						num++;
					}
				}
				Line(movePos);
				PlayReadyAnim();
			}
			else if (Input.GetMouseButtonUp(0))
			{
				timeIndex = 0;
				HideLine();
				if (ShootBubble())
				{
					FireBubbleAnim();
				}
				else
				{
					CancelFireBubble(2);
				}
			}
		}
	}

	public void PlayReadyAnim()
	{
		if (!isReadyShoot)
		{
			isReadyShoot = true;
			if (GameScene.Instance.MoveNum <= 5)
			{
				GameScene.Instance.SwitchoverElfAni("ready2", bLoop: false);
			}
			else
			{
				GameScene.Instance.SwitchoverElfAni("ready", bLoop: false);
			}
		}
	}

	public void CancelFireBubble(int type)
	{
		if (!isReadyShoot)
		{
			return;
		}
		if (type == 1)
		{
			if (cancel)
			{
				return;
			}
			cancel = true;
		}
		else
		{
			cancel = false;
		}
		if (GameScene.Instance.MoveNum <= 5)
		{
			GameScene.Instance.SwitchoverElfAni("ready2_to_worry", bLoop: false);
		}
		else
		{
			GameScene.Instance.SwitchoverElfAni("ready_to_start", bLoop: false);
		}
		isReadyShoot = false;
	}

	public void FireBubbleAnim()
	{
		if (GameScene.Instance.MoveNum <= 5)
		{
			GameScene.Instance.SwitchoverElfAni("fire2", bLoop: false);
		}
		else
		{
			GameScene.Instance.SwitchoverElfAni("fire", bLoop: false);
		}
		isReadyShoot = false;
	}

	private void HideLine()
	{
		line1_1.gameObject.SetActive(value: false);
		line1_2.gameObject.SetActive(value: false);
		line1_3.gameObject.SetActive(value: false);
		line2.gameObject.SetActive(value: false);
		TipPoint.gameObject.SetActive(value: false);
		HitPoint.gameObject.SetActive(value: false);
		HitPoint2.gameObject.SetActive(value: false);
		line1_fx_1.SetActive(value: false);
		line1_fx_2.SetActive(value: false);
		line1_fx_3.SetActive(value: false);
		fx_qiufashe_lizi.SetActive(value: false);
	}

	public void Line(List<Vector2> movePos)
	{
		Vector2 normalized = (movePos[1] - movePos[0]).normalized;
		fx_qiufashe_lizi.SetActive(value: true);
		float angle = Mathf.Atan2(0f - normalized.x, normalized.y) * 57.29578f;
		fx_qiufashe_lizi.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		BubbleObj component = ReadyBubbleController.Instance.ReadyBubble1.GetComponent<BubbleObj>();
		if (!component.GetProp(2))
		{
			TipPoint.gameObject.SetActive(value: true);
		}
		int type = component.GetBubbleType();
		if (component.GetSkill(1) || component.GetSkill(2) || component.GetSkill(3) || component.GetSkill(4) || component.GetProp(1) || component.GetProp(2) || component.GetProp(3))
		{
			type = 6;
		}
		if (GameManager.Instance.useLongLine)
		{
			if (movePos.Count == 2)
			{
				line2.gameObject.SetActive(value: true);
				line2.SetPos(movePos[0], hitPos, type);
				line1_1.gameObject.SetActive(value: false);
				line1_2.gameObject.SetActive(value: false);
				line1_3.gameObject.SetActive(value: false);
				line1_fx_1.SetActive(value: false);
				line1_fx_2.SetActive(value: false);
				line1_fx_3.SetActive(value: false);
			}
			else if (movePos.Count == 3)
			{
				line1_1.gameObject.SetActive(value: true);
				line1_1.SetPos(movePos[0], movePos[1], type);
				line2.gameObject.SetActive(value: true);
				line2.SetPos(movePos[1], hitPos, type);
				line1_2.gameObject.SetActive(value: false);
				line1_3.gameObject.SetActive(value: false);
				line1_fx_1.SetActive(value: true);
				line1_fx_1.transform.position = movePos[1];
				line1_fx_2.SetActive(value: false);
				line1_fx_3.SetActive(value: false);
			}
			else if (movePos.Count == 4)
			{
				line1_1.gameObject.SetActive(value: true);
				line1_1.SetPos(movePos[0], movePos[1], type);
				line1_2.gameObject.SetActive(value: true);
				line1_2.SetPos(movePos[1], movePos[2], type);
				line2.gameObject.SetActive(value: true);
				line2.SetPos(movePos[2], hitPos, type);
				line1_3.gameObject.SetActive(value: false);
				line1_fx_1.SetActive(value: true);
				line1_fx_1.transform.position = movePos[1];
				line1_fx_2.SetActive(value: true);
				line1_fx_2.transform.position = movePos[2];
				line1_fx_3.SetActive(value: false);
			}
			else if (movePos.Count == 5)
			{
				line1_1.gameObject.SetActive(value: true);
				line1_1.SetPos(movePos[0], movePos[1], type);
				line1_2.gameObject.SetActive(value: true);
				line1_2.SetPos(movePos[1], movePos[2], type);
				line1_3.gameObject.SetActive(value: true);
				line1_3.SetPos(movePos[2], movePos[3], type);
				line2.gameObject.SetActive(value: true);
				line2.SetPos(movePos[3], hitPos, type);
				line1_fx_1.SetActive(value: true);
				line1_fx_1.transform.position = movePos[1];
				line1_fx_2.SetActive(value: true);
				line1_fx_2.transform.position = movePos[2];
				line1_fx_3.SetActive(value: true);
				line1_fx_3.transform.position = movePos[3];
			}
		}
		else if (movePos.Count == 2)
		{
			line2.gameObject.SetActive(value: true);
			line2.SetPos(movePos[0], hitPos, type);
			line1_1.gameObject.SetActive(value: false);
			line1_2.gameObject.SetActive(value: false);
			line1_3.gameObject.SetActive(value: false);
			line1_fx_1.SetActive(value: false);
			line1_fx_2.SetActive(value: false);
			line1_fx_3.SetActive(value: false);
		}
		else
		{
			line1_1.gameObject.SetActive(value: true);
			line1_1.SetPos(movePos[0], movePos[1], type);
			float num = Vector2.Distance(movePos[2], movePos[1]);
			Vector2 vector = movePos[2];
			if (num > 1f)
			{
				Vector2 normalized2 = (movePos[1] - movePos[0]).normalized;
				normalized2.x = 0f - normalized2.x;
				vector = movePos[1] + normalized2 * 0.8f;
				TipPoint.gameObject.SetActive(value: false);
			}
			else
			{
				Vector2 normalized3 = (movePos[1] - movePos[0]).normalized;
				normalized3.x = 0f - normalized3.x;
				vector = movePos[1] + normalized3 * num;
				TipPoint.gameObject.SetActive(value: true);
			}
			line2.gameObject.SetActive(value: true);
			line2.SetPos(movePos[1], vector, type);
			line1_2.gameObject.SetActive(value: false);
			line1_3.gameObject.SetActive(value: false);
			line1_fx_1.SetActive(value: true);
			line1_fx_1.transform.position = movePos[1];
			line1_fx_2.SetActive(value: false);
			line1_fx_3.SetActive(value: false);
		}
	}

	public bool RaycastHit(Vector2 start, Vector2 end, Vector2 normalized)
	{
		endNormalized = normalized;
		RaycastHit2D[] array = Physics2D.LinecastAll(start, end, (1 << LayerMask.NameToLayer("Bubble")) | (1 << LayerMask.NameToLayer("Border")) | (1 << LayerMask.NameToLayer("TopBorder")));
		RaycastHit2D[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			RaycastHit2D raycastHit2D = array2[i];
			UnityEngine.Debug.DrawLine(start, raycastHit2D.point, Color.red, 0.3f, depthTest: true);
			if (raycastHit2D.transform.gameObject.layer == LayerMask.NameToLayer("Bubble"))
			{
				Collision(raycastHit2D.transform.gameObject, raycastHit2D.point);
				hitPos = raycastHit2D.point;
				hitBubble = raycastHit2D.transform.parent.GetComponent<BubbleObj>();
				ShootType = 1;
				return false;
			}
			if (raycastHit2D.transform.gameObject.layer == LayerMask.NameToLayer("TopBorder"))
			{
				CollisionTop(raycastHit2D.point);
				hitPos = raycastHit2D.point;
				ShootType = 2;
				return false;
			}
			if (raycastHit2D.transform.gameObject.layer == LayerMask.NameToLayer("Border"))
			{
				movePos.Add(raycastHit2D.point);
				return true;
			}
		}
		return true;
	}

	public bool RaycastHitByProp2(Vector2 start, Vector2 end, Vector2 normalized)
	{
		endNormalized = normalized;
		RaycastHit2D[] array = Physics2D.LinecastAll(start, end, (1 << LayerMask.NameToLayer("Border")) | (1 << LayerMask.NameToLayer("TopBorder2")));
		RaycastHit2D[] array2 = array;
		int num = 0;
		
		if (num < array2.Length)
		{
			RaycastHit2D raycastHit2D = array2[num];
			UnityEngine.Debug.DrawLine(start, raycastHit2D.point, Color.red, 0.3f, depthTest: true);
			ShootType = 4;
			hitPos = raycastHit2D.point;
			movePos.Add(raycastHit2D.point);
			return false;
		}
		return true;
		
	}

	public void CollisionTop(Vector2 hitPoint)
	{
		float num = float.MaxValue;
		List<Point> list = new List<Point>();
		for (int i = 0; i < GameManager.cols; i++)
		{
			box = GameManager.Instance.GetBox(new Point(0, i));
			if (box != null && box.GetBubble() == null)
			{
				float num2 = Vector2.Distance(box.transform.position, hitPoint);
				if (num2 < num)
				{
					num = num2;
					endPoint = new Point(0, i);
				}
			}
			else if (box != null && (bool)box.GetBubble() && box.GetBubble().GetAttributes() == 3)
			{
				float num3 = Vector2.Distance(box.transform.position, hitPoint);
				if (num3 < num)
				{
					num = num3;
					endPoint = new Point(0, i);
				}
			}
		}
		Vector2 vector = GameManager.Instance.GetBox(endPoint).transform.position;
		movePos.Add(vector);
		TipPoint.transform.position = vector;
	}

	public void Collision(GameObject obj, Vector2 hitPoint)
	{
		Vector2 a = obj.transform.position;
		HitPoint.transform.position = obj.transform.position;
		HitPoint2.transform.position = hitPoint;
		Vector2 normalized = (a - hitPoint).normalized;
		float num = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
		BubbleObj component = obj.transform.parent.GetComponent<BubbleObj>();
		List<Point> around = GameManager.Instance.GetAround(component.point);
		float num2 = float.MaxValue;
		endPoint = new Point(-1, -1);
		for (int i = 0; i < around.Count; i++)
		{
			BoxObj boxObj = GameManager.Instance.GetBox(around[i]);
			if (boxObj != null && boxObj.GetBubble() == null)
			{
				float num3 = Vector2.Distance(boxObj.transform.position, hitPoint);
				if (num3 < num2)
				{
					num2 = num3;
					endPoint = around[i];
				}
			}
			else if (boxObj != null && (bool)boxObj.GetBubble() && boxObj.GetBubble().GetAttributes() == 3)
			{
				float num4 = Vector2.Distance(boxObj.transform.position, hitPoint);
				if (num4 < num2)
				{
					num2 = num4;
					endPoint = around[i];
				}
			}
		}
		Vector2 vector = GameManager.Instance.GetBox(endPoint).transform.position;
		movePos.Add(vector);
		TipPoint.transform.position = vector;
	}

	public bool ShootBubble()
	{
		if (movePos.Count == 0)
		{
			return false;
		}
		if (endPoint.X == -1)
		{
			movePos.Clear();
			return false;
		}
		if (GameManager.Instance.GetBox(endPoint) == null)
		{
			movePos.Clear();
			return false;
		}
		if (ShootType == 1 && hitBubble == null)
		{
			return false;
		}
		if (ShootType == 1 && hitBubble.GetBubbleState() != 0)
		{
			return false;
		}
		if (GameManager.Instance.GetBox(endPoint).GetBubble() != null && ShootType != 4 && GameManager.Instance.GetBox(endPoint).GetBubble().GetAttributes() != 3)
		{
			UnityEngine.Debug.Log("  There is a ball in this position " + endPoint.X + "      " + endPoint.Y + "     " + Time.time);
			movePos.Clear();
			return false;
		}
		if (ShootType == 4)
		{
			GameManager.Instance.iCombo++;
			AudioPlayManager.PlaySFX2D("skill_super_3");
		}
		if (isShooting)
		{
			movePos.Clear();
			return false;
		}
		AudioPlayManager.PlaySFX2D("b_shoot");
		isShooting = true;
		float num = 22f;
		float num2 = 0f;
		mMoveBubble = ReadyBubbleController.Instance.ReadyBubble1;
		ReadyBubbleController.Instance.ReadyBubble1 = null;
		Vector3[] array = new Vector3[movePos.Count];
		for (int i = 0; i < movePos.Count; i++)
		{
			num2 = ((i != 0) ? (num2 + Vector2.Distance(movePos[i - 1], movePos[i])) : Vector2.Distance(mMoveBubble.transform.position, movePos[0]));
			array[i] = movePos[i];
		}
		BubbleObj component = mMoveBubble.GetComponent<BubbleObj>();
		if (component.GetProp(2))
		{
			component.AddMofajian((movePos[1] - movePos[0]).normalized);
		}
		component.mCollider.gameObject.SetActive(value: true);
		float duration = num2 / num;
		Sequence s = DOTween.Sequence();
		Tweener t = mMoveBubble.transform.DOPath(array, duration, PathType.Linear, PathMode.Sidescroller2D, 5).SetEase(Ease.Linear);
		s.Append(t).OnComplete(ShootEnd);
		if (GameGuid.Instance != null)
		{
			GameGuid.Instance.ShootBubble();
		}
		return true;
	}

	private void ShootEnd()
	{
		HitPoint.transform.position = new Vector3(-100f, 0f, 0f);
		HitPoint2.transform.position = new Vector3(-100f, 0f, 0f);
		TipPoint.transform.position = new Vector3(-100f, 0f, 0f);
		if (ShootType == 4)
		{
			mMoveBubble.GetComponent<BubbleObj>().RemoveBubble(RemoveType.None);
			GameScene.Instance.ShootBubble(skill1: false, skill2: false, skill3: false, skill4: false, prop1: false, prop2: true, prop3: false);
			isShooting = false;
			Singleton<RemoveControl>.Instance.CheckFall(5);
			return;
		}
		bubbleObj = mMoveBubble.GetComponent<BubbleObj>();
		BubbleData data = default(BubbleData);
		data.key = bubbleObj.key;
		data.row = endPoint.X;
		data.col = endPoint.Y;
		data.i = 0;
		data.s = 0;
		data.isReadyBubble = false;
		bool skill = bubbleObj.GetSkill(1);
		bool skill2 = bubbleObj.GetSkill(2);
		bool skill3 = bubbleObj.GetSkill(3);
		bool skill4 = bubbleObj.GetSkill(4);
		bool prop = bubbleObj.GetProp(1);
		bool prop2 = bubbleObj.GetProp(2);
		bool prop3 = bubbleObj.GetProp(3);
		bubbleObj.DestroyReadyObject();
		bubbleObj = GameManager.Instance.BubbleInstantiate(data, shootEndCreate: true);
		bubbleObj.SetSkill(1, skill);
		bubbleObj.SetSkill(2, skill2);
		bubbleObj.SetSkill(3, skill3);
		bubbleObj.SetSkill(4, skill4);
		bubbleObj.ChangeToSkill(5);
		bubbleObj.SetProp(1, prop);
		bubbleObj.SetProp(2, prop2);
		bubbleObj.SetProp(3, prop3);
		bubbleObj.hitBubble = hitBubble;
		Singleton<RemoveControl>.Instance.CheckRemove(bubbleObj);
		HitAnim(new Point(endPoint.X, endPoint.Y));
		GameScene.Instance.ShootBubble(skill, skill2, skill3, skill4, prop, prop2, prop3);
		isShooting = false;
	}

	public void HitAnim(Point point)
	{
		Vector2 vector = GameManager.Instance.GetBox(point).transform.position;
		pCurBubble = GameManager.Instance.GetBox(point).GetBubble();
		if (pCurBubble != null && pCurBubble.GetBubbleState() == BubbleState.None)
		{
			Sequence s = DOTween.Sequence();
			s.Append(pCurBubble.transform.DOMove(vector + endNormalized * 0.1f, 0.05f)).Append(pCurBubble.transform.DOMove(vector, 0.15f));
		}
		List<Point> around = GameManager.Instance.GetAround(point);
		for (int i = 0; i < around.Count; i++)
		{
			box = GameManager.Instance.GetBox(around[i]);
			if (box != null && box.GetBubble() != null)
			{
				pCurBubble = box.GetBubble();
				PlayHitAnim(vector, box.GetBubble(), 1.6f, 0.1f, 0.35f);
			}
		}
		List<Point> list = new List<Point>();
		GameManager.Instance.GetAnimList(list, around, point);
		for (int j = 0; j < list.Count; j++)
		{
			box = GameManager.Instance.GetBox(list[j]);
			if (!(box != null) || !(box.GetBubble() != null))
			{
				continue;
			}
			Point point2 = list[j];
			if (point2.X == point.X)
			{
				Point point3 = list[j];
				if (point3.Y == point.Y)
				{
					continue;
				}
			}
			PlayHitAnim(vector, box.GetBubble(), 0.8f, 0.12f, 0.3f);
		}
	}

	public void PlayHitAnim(Vector3 postion, BubbleObj obj2, float elasticity, float time1, float time2)
	{
		Vector3 vector = postion - obj2.transform.position;
		double num = Math.Atan2(vector.y, vector.x);
		Vector3 position = GameManager.Instance.GetBox(obj2.point).transform.position;
		float x = position.x;
		Vector3 position2 = GameManager.Instance.GetBox(obj2.point).transform.position;
		float y = position2.y;
		float num2 = (float)Math.Cos(num) * 0.15f * elasticity;
		float num3 = (float)Math.Sin(num) * 0.15f * elasticity;
		float num4 = (float)Math.Cos(num) * 0.15f * elasticity;
		float num5 = (float)Math.Sin(num) * 0.15f * elasticity;
		float num6 = (float)Math.Cos(num) * 0.15f * elasticity;
		float num7 = (float)Math.Sin(num) * 0.15f * elasticity;
		if (obj2 != null && !obj2.isFall && !obj2.isRemove)
		{
			Sequence s = DOTween.Sequence();
			s.Append(obj2.transform.DOMove(new Vector3(x - num2, y - num3, 0f), time1)).Append(obj2.transform.DOMove(new Vector3(x, y, 0f), time2));
		}
	}
}
