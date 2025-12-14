
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ReadyBubbleController : MonoBehaviour
{
	public static ReadyBubbleController Instance;

	public GameObject Ready1Parent;

	public GameObject Ready2Parent;

	public GameObject Ready3Parent;

	public GameObject ReadyBubble1;

	public GameObject ReadyBubble2;

	public GameObject ReadyBubble3;

	private bool isReadyBubble3;

	private PoolObject ObjTmp;

	private BubbleObj bubbleObj;

	public bool isReadyBubbleChange;

	private float movetime = 0.2f;

	public void Awake()
	{
		Instance = this;
	}

	public void Init()
	{
		Instance = this;
		Ready1Parent = GameObject.Find("-Ready1Parent");
		Ready2Parent = GameObject.Find("-Ready2Parent");
		Ready3Parent = GameObject.Find("-Ready3Parent");
	}

	public void InitReadyBubble()
	{
		CreateReadyBubble1();
		CreateReadyBubble2();
	}

	public void CreateNewReadyBubble()
	{
		isReadyBubbleChange = true;
		if (ReadyBubble1 == null && ReadyBubble2 == null && GameScene.Instance.MoveNum == 1)
		{
			CreateReadyBubble1();
			isReadyBubbleChange = false;
			return;
		}
		ReadyBubble1 = null;
		if (ReadyBubble2 != null)
		{
			ReadyBubble2.transform.DOScale(1f, movetime);
			ReadyBubble2.transform.DOLocalMove(Ready1Parent.transform.localPosition, movetime).OnComplete(ReadyMoveEnd1);
		}
		if (ReadyBubble3 != null)
		{
			ReadyBubble3.transform.DOScale(0.8f, movetime);
			ReadyBubble3.transform.DOLocalMove(Ready2Parent.transform.localPosition, movetime + 0.01f).OnComplete(ReadyMoveEnd2);
		}
		Timer.DelayCallBack(movetime + 0.1f, delegate
		{
			GameManager.Instance.ChangeReadyBubble();
			isReadyBubbleChange = false;
		});
	}

	public void ChangeReadyBubleColor(List<BubbleShow> gameShowBubble)
	{
		if (ReadyBubble1 != null)
		{
			CheckReadyBubble(ReadyBubble1.GetComponent<BubbleObj>(), gameShowBubble, 1);
		}
		if (ReadyBubble2 != null)
		{
			CheckReadyBubble(ReadyBubble2.GetComponent<BubbleObj>(), gameShowBubble, 2);
		}
		if (ReadyBubble3 != null)
		{
			CheckReadyBubble(ReadyBubble3.GetComponent<BubbleObj>(), gameShowBubble, 3);
		}
	}

	private void CheckReadyBubble(BubbleObj obj, List<BubbleShow> gameShowBubble, int index)
	{
		string key = obj.key;
		bool flag = true;
		for (int i = 0; i < gameShowBubble.Count; i++)
		{
			BubbleShow bubbleShow = gameShowBubble[i];
			if (bubbleShow.key == key)
			{
				BubbleShow bubbleShow2 = gameShowBubble[i];
				if (bubbleShow2.show)
				{
					flag = false;
					break;
				}
			}
		}
		if (flag && !obj.GetProp(1) && !obj.GetProp(2) && !obj.GetProp(3))
		{
			BubbleData bubbleData = obj.GetBubbleData();
			bubbleData.key = GameManager.Instance.GetRandomKey();
			bool skill = obj.GetSkill(1);
			bool skill2 = obj.GetSkill(2);
			bool skill3 = obj.GetSkill(3);
			bool skill4 = obj.GetSkill(4);
			obj.Init(bubbleData);
			obj.SetSkill(1, skill);
			obj.SetSkill(2, skill2);
			obj.SetSkill(3, skill3);
			obj.SetSkill(4, skill4);
			obj.RemoveFx();
			obj.ChangeToSkill(5);
			switch (index)
			{
			case 1:
				obj.transform.localScale = Vector3.one;
				break;
			case 2:
				obj.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
				break;
			case 3:
				obj.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
				break;
			}
		}
	}

	private void ReadyMoveEnd1()
	{
		ReadyBubble1 = ReadyBubble2;
		if (isReadyBubble3)
		{
			if (ReadyBubble3 == null)
			{
				ReadyBubble2 = null;
			}
		}
		else if (GameScene.Instance.MoveNum > 1)
		{
			CreateReadyBubble2();
		}
		else
		{
			ReadyBubble2 = null;
		}
	}

	public void BuyMove()
	{
		if (GameScene.Instance.MoveNum > 0 && ReadyBubble1 == null)
		{
			CreateReadyBubble1();
		}
		if (GameScene.Instance.MoveNum > 1 && ReadyBubble2 == null)
		{
			CreateReadyBubble2();
		}
		if (isReadyBubble3 && GameScene.Instance.MoveNum > 2 && ReadyBubble3 == null)
		{
			CreateReadyBubble3(string.Empty);
		}
	}

	private void ReadyMoveEnd2()
	{
		ReadyBubble2 = ReadyBubble3;
		ReadyBubble3 = null;
		if (GameScene.Instance.MoveNum > 2)
		{
			CreateReadyBubble3(string.Empty);
		}
		else
		{
			ReadyBubble3 = null;
		}
		isReadyBubbleChange = false;
	}

	private void CreateReadyBubble1()
	{
		ReadyBubble1 = GetReadyBubble(Ready1Parent.transform.position, string.Empty);
		ReadyBubble1.transform.localScale = new Vector3(1f, 1f, 0f);
	}

	private void CreateReadyBubble2()
	{
		ReadyBubble2 = GetReadyBubble(Ready2Parent.transform.position, string.Empty);
		ReadyBubble2.transform.localScale = new Vector3(0.8f, 0.8f, 0f);
	}

	private void CreateReadyBubble3(string key = "")
	{
		ReadyBubble3 = GetReadyBubble(Ready3Parent.transform.position, string.Empty);
		ReadyBubble3.transform.localScale = new Vector3(0.6f, 0.6f, 0f);
	}

	public void InitReadyBubble3(string key = "")
	{
		isReadyBubble3 = true;
		CreateReadyBubble3(key);
	}

	public void CreateNewReadyBubbleSkill(bool skill1, bool skill2, bool skill3, bool skill4)
	{
		isReadyBubbleChange = true;
		GameManager.Instance.iSkill4Count++;
		ReadyBubble1 = GetReadyBubble(Ready1Parent.transform.position, string.Empty);
		BubbleData data = default(BubbleData);
		data.key = GameManager.Instance.GetRandomKey();
		data.isReadyBubble = true;
		bubbleObj = ObjTmp.GetComponent<BubbleObj>();
		bubbleObj.Init(data);
		bubbleObj.SetSkill(1, skill1);
		bubbleObj.SetSkill(2, skill2);
		bubbleObj.SetSkill(3, skill3);
		bubbleObj.SetSkill(4, skill4);
		bubbleObj.ChangeToSkill(5);
		isReadyBubbleChange = false;
	}

	private GameObject GetReadyBubble(Vector3 pos, string _key = "")
	{
		ObjTmp = GameObjectManager.GetPoolObject("Obj/GameObj/Bubble");
		ObjTmp.transform.SetParent(Ready1Parent.transform.parent, worldPositionStays: false);
		ObjTmp.transform.position = pos;
		ObjTmp.transform.localScale = Vector3.zero;
		BubbleData data = default(BubbleData);
		string text = _key;
		if (GameGuid.Instance != null && _key == string.Empty)
		{
			text = GameGuid.Instance.GetKey();
		}
		if (text != string.Empty)
		{
			data.key = text;
		}
		else
		{
			data.key = GameManager.Instance.GetRandomKey();
		}
		data.isReadyBubble = true;
		bubbleObj = ObjTmp.GetComponent<BubbleObj>();
		bubbleObj.Init(data);
		return ObjTmp.gameObject;
	}

	public void ChangeBubble()
	{
		if (GameControl.Instance.isShooting || (GameGuid.Instance != null && !GameGuid.Instance.GetChangeBubble()))
		{
			return;
		}
		GameObject readyBubble = ReadyBubble1;
		if (!ReadyBubble1.GetComponent<BubbleObj>().GetProp(1) && !ReadyBubble1.GetComponent<BubbleObj>().GetProp(2) && !ReadyBubble1.GetComponent<BubbleObj>().GetProp(3))
		{
			if (GameGuid.Instance != null)
			{
				GameGuid.Instance.ChangeBubble();
			}
			isReadyBubbleChange = true;
			AudioPlayManager.PlaySFX2D("b_change");
			if (ReadyBubble3 != null)
			{
				bool skill = ReadyBubble1.GetComponent<BubbleObj>().GetSkill(1);
				bool skill2 = ReadyBubble1.GetComponent<BubbleObj>().GetSkill(2);
				bool skill3 = ReadyBubble1.GetComponent<BubbleObj>().GetSkill(3);
				bool skill4 = ReadyBubble1.GetComponent<BubbleObj>().GetSkill(4);
				bool skill5 = ReadyBubble2.GetComponent<BubbleObj>().GetSkill(1);
				bool skill6 = ReadyBubble2.GetComponent<BubbleObj>().GetSkill(2);
				bool skill7 = ReadyBubble2.GetComponent<BubbleObj>().GetSkill(3);
				bool skill8 = ReadyBubble2.GetComponent<BubbleObj>().GetSkill(4);
				bool skill9 = ReadyBubble3.GetComponent<BubbleObj>().GetSkill(1);
				bool skill10 = ReadyBubble3.GetComponent<BubbleObj>().GetSkill(2);
				bool skill11 = ReadyBubble3.GetComponent<BubbleObj>().GetSkill(3);
				bool skill12 = ReadyBubble3.GetComponent<BubbleObj>().GetSkill(4);
				ReadyBubble1 = ReadyBubble2;
				ReadyBubble1.transform.DOLocalMove(Ready1Parent.transform.localPosition, movetime);
				ReadyBubble1.transform.DOScale(1f, movetime);
				ReadyBubble1.GetComponent<BubbleObj>().SetSkill(1, skill);
				ReadyBubble1.GetComponent<BubbleObj>().SetSkill(2, skill2);
				ReadyBubble1.GetComponent<BubbleObj>().SetSkill(3, skill3);
				ReadyBubble1.GetComponent<BubbleObj>().SetSkill(4, skill4);
				ReadyBubble1.GetComponent<BubbleObj>().RemoveFx();
				ReadyBubble1.GetComponent<BubbleObj>().ChangeToSkill(5);
				ReadyBubble2 = ReadyBubble3;
				ReadyBubble2.transform.DOLocalMove(Ready2Parent.transform.localPosition, movetime);
				ReadyBubble2.transform.DOScale(0.8f, movetime);
				ReadyBubble2.GetComponent<BubbleObj>().SetSkill(1, skill5);
				ReadyBubble2.GetComponent<BubbleObj>().SetSkill(2, skill6);
				ReadyBubble2.GetComponent<BubbleObj>().SetSkill(3, skill7);
				ReadyBubble2.GetComponent<BubbleObj>().SetSkill(4, skill8);
				ReadyBubble2.GetComponent<BubbleObj>().RemoveFx();
				ReadyBubble2.GetComponent<BubbleObj>().ChangeToSkill(5);
				ReadyBubble3 = readyBubble;
				ReadyBubble3.transform.DOLocalMove(Ready3Parent.transform.localPosition, movetime);
				ReadyBubble3.transform.DOScale(0.6f, movetime);
				ReadyBubble3.GetComponent<BubbleObj>().SetSkill(1, isSkill: false);
				ReadyBubble3.GetComponent<BubbleObj>().SetSkill(2, isSkill: false);
				ReadyBubble3.GetComponent<BubbleObj>().SetSkill(3, isSkill: false);
				ReadyBubble3.GetComponent<BubbleObj>().SetSkill(4, isSkill: false);
				ReadyBubble3.GetComponent<BubbleObj>().RemoveFx();
				ReadyBubble3.GetComponent<BubbleObj>().ChangeToSkill(5);
				Timer.DelayCallBack(movetime, delegate
				{
					isReadyBubbleChange = false;
				});
			}
			else if (ReadyBubble2 != null)
			{
				bool skill13 = ReadyBubble1.GetComponent<BubbleObj>().GetSkill(1);
				bool skill14 = ReadyBubble1.GetComponent<BubbleObj>().GetSkill(2);
				bool skill15 = ReadyBubble1.GetComponent<BubbleObj>().GetSkill(3);
				bool skill16 = ReadyBubble1.GetComponent<BubbleObj>().GetSkill(4);
				bool skill17 = ReadyBubble2.GetComponent<BubbleObj>().GetSkill(1);
				bool skill18 = ReadyBubble2.GetComponent<BubbleObj>().GetSkill(2);
				bool skill19 = ReadyBubble2.GetComponent<BubbleObj>().GetSkill(3);
				bool skill20 = ReadyBubble2.GetComponent<BubbleObj>().GetSkill(4);
				ReadyBubble1 = ReadyBubble2;
				ReadyBubble1.transform.DOLocalMove(Ready1Parent.transform.localPosition, movetime);
				ReadyBubble1.transform.DOScale(1f, movetime);
				ReadyBubble1.GetComponent<BubbleObj>().SetSkill(1, skill13);
				ReadyBubble1.GetComponent<BubbleObj>().SetSkill(2, skill14);
				ReadyBubble1.GetComponent<BubbleObj>().SetSkill(3, skill15);
				ReadyBubble1.GetComponent<BubbleObj>().SetSkill(4, skill16);
				ReadyBubble1.GetComponent<BubbleObj>().RemoveFx();
				ReadyBubble1.GetComponent<BubbleObj>().ChangeToSkill(5);
				ReadyBubble2 = readyBubble;
				ReadyBubble2.transform.DOLocalMove(Ready2Parent.transform.localPosition, movetime);
				ReadyBubble2.transform.DOScale(0.8f, movetime);
				ReadyBubble2.GetComponent<BubbleObj>().SetSkill(1, skill17);
				ReadyBubble2.GetComponent<BubbleObj>().SetSkill(2, skill18);
				ReadyBubble2.GetComponent<BubbleObj>().SetSkill(3, skill19);
				ReadyBubble2.GetComponent<BubbleObj>().SetSkill(4, skill20);
				ReadyBubble2.GetComponent<BubbleObj>().RemoveFx();
				ReadyBubble2.GetComponent<BubbleObj>().ChangeToSkill(5);
				Timer.DelayCallBack(movetime, delegate
				{
					isReadyBubbleChange = false;
				});
			}
			else
			{
				isReadyBubbleChange = false;
			}
		}
	}

	public void ChangeToProp1()
	{
		AudioPlayManager.PlaySFX2D("item_use");
		BubbleData bubbleData = ReadyBubble1.GetComponent<BubbleObj>().GetBubbleData();
		bubbleData.key = "BB";
		ReadyBubble1.GetComponent<BubbleObj>().Init(bubbleData);
		ReadyBubble1.GetComponent<BubbleObj>().SetProp(1, isProp: true);
	}

	public void ChangeToProp2()
	{
		AudioPlayManager.PlaySFX2D("item_use");
		BubbleData bubbleData = ReadyBubble1.GetComponent<BubbleObj>().GetBubbleData();
		bubbleData.key = "JZ";
		ReadyBubble1.GetComponent<BubbleObj>().Init(bubbleData);
		ReadyBubble1.GetComponent<BubbleObj>().SetProp(2, isProp: true);
	}

	public void ChangeToProp3()
	{
		AudioPlayManager.PlaySFX2D("item_use");
		BubbleData bubbleData = ReadyBubble1.GetComponent<BubbleObj>().GetBubbleData();
		bubbleData.key = "JL";
		ReadyBubble1.GetComponent<BubbleObj>().Init(bubbleData);
		ReadyBubble1.GetComponent<BubbleObj>().SetProp(3, isProp: true);
	}

	public void RemoveReadyBubble()
	{
		if (ReadyBubble1 != null)
		{
			GameObjectManager.DestroyPoolObject(ReadyBubble1.GetComponent<BubblePool>());
		}
		if (ReadyBubble2 != null)
		{
			GameObjectManager.DestroyPoolObject(ReadyBubble2.GetComponent<BubblePool>());
		}
		if (ReadyBubble3 != null)
		{
			GameObjectManager.DestroyPoolObject(ReadyBubble3.GetComponent<BubblePool>());
		}
		ReadyBubble1 = null;
		ReadyBubble2 = null;
		ReadyBubble3 = null;
	}
}
