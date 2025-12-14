
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public GameObject LineParent;

	public GameObject BoxParent;

	public GameObject BallPrefab;

	public GameObject FallParent;

	public GameObject RemoveParent;

	public GameObject FlyParent;

	public GameObject FXParent;

	public GameObject FXSkill4;

	public bool isExitGame;

	public int iCombo;

	public int iGameScore;

	public int iSkill4Count;

	public static float removeTime = 0.05f;

	public static int cols = 11;

	public static int rows = 180;

	public int iTestLevel = 1;

	private BoxObj[,] BoxArray = new BoxObj[rows, cols];

	private List<BubbleObj> BubbleArray = new List<BubbleObj>();

	private PoolObject ObjTmp;

	private BubbleObj bubbleObj;

	private float delayTime;

	private float devWidth = 7.2f;

	private float orthographicSize = 6.4f;

	public bool useLongLine;

	private float posX;

	private float posY;

	private float radius = 32f;

	private float topHeight = 60f;

	private List<BubbleShow> gameShowBubble = new List<BubbleShow>();

	public string readykey = string.Empty;

	public int readyindex;

	private int randomindex = 1;

	public void Awake()
	{
		Instance = this;
	}

	public void InitCamera()
	{
		orthographicSize = Camera.main.orthographicSize;
		float num = (float)Screen.width * 1f / (float)Screen.height;
		float num2 = orthographicSize * 2f * num;
		if (num2 < devWidth)
		{
			orthographicSize = devWidth / (2f * num);
			Camera.main.orthographicSize = orthographicSize;
		}
		Transform transform = Camera.main.transform;
		Vector3 position = Camera.main.transform.position;
		float x = position.x;
		Vector3 position2 = Camera.main.transform.position;
		transform.position = new Vector3(x, 15f, position2.z);
		delayTime = 0f;
		isExitGame = false;
		iCombo = 0;
		iSkill4Count = 0;
		useLongLine = false;
	}

	public void SetDelayTime(float _delayTime)
	{
		if (delayTime < Time.time + _delayTime)
		{
			delayTime = Time.time + _delayTime - 0.01f;
		}
	}

	public bool GetDelayGame()
	{
		if (Time.time > delayTime)
		{
			return false;
		}
		return true;
	}

	public bool GetDelayGame2()
	{
		if (Time.time > delayTime - 0.1f)
		{
			return false;
		}
		return true;
	}

	public bool InitData()
	{
		if (Singleton<LevelManager>.Instance.GetNowSelectLevel() == -1)
		{
			Singleton<LevelManager>.Instance.SetNowSelectLevel(iTestLevel);
		}
		if (!Singleton<LevelManager>.Instance.LoadLevelData())
		{
			return false;
		}
		return true;
	}

	public List<BubbleObj> GetBubbleArray()
	{
		return BubbleArray;
	}

	public void AddBubble(BubbleObj bubble)
	{
		BubbleArray.Add(bubble);
	}

	public void RemoveBubble(BubbleObj bubble)
	{
		BubbleArray.Remove(bubble);
	}

	public BoxObj[,] GetBoxArray()
	{
		return BoxArray;
	}

	public BoxObj GetBox(Point point)
	{
		if (!IsValidPos(point.X, point.Y))
		{
			return null;
		}
		return BoxArray[point.X, point.Y];
	}

	public void InitParent()
	{
		LineParent = GameObject.Find("-Line");
		BoxParent = GameObject.Find("-Box");
		BallPrefab = GameObject.Find("-Ball");
		FallParent = GameObject.Find("-Fall");
		RemoveParent = GameObject.Find("-Remove");
		FlyParent = GameObject.Find("-Fly");
		FXParent = GameObject.Find("-FX");
	}

	public bool InitBubble()
	{
		CreateBox();
		CreateBubble();
		CreateBubbleTop();
		return true;
	}

	private void CreateBox()
	{
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols - i % 2; j++)
			{
				BoxArray[i, j] = BoxInstantiate(i, j);
				BoxArray[i, j].InitBox(new Point(i, j));
			}
		}
	}

	public BoxObj BoxInstantiate(int row, int col)
	{
		ObjTmp = GameObjectManager.GetPoolObject("Obj/GameObj/Box");
		ObjTmp.transform.SetParent(BoxParent.transform, worldPositionStays: false);
		ObjTmp.transform.localPosition = GetPosByRowAndCol(row, col);
		ObjTmp.name = "Box" + row + "_" + col;
		return ObjTmp.GetComponent<BoxObj>();
	}

	private void CreateBubble()
	{
		for (int i = 0; i < Singleton<LevelManager>.Instance.LTbubble.Count; i++)
		{
			BubbleData data = Singleton<LevelManager>.Instance.LTbubble[i];
			if (data.key == "@")
			{
				data.key = Instance.GetRandomKey();
			}
			BubbleInstantiate(data);
		}
	}

	private void CreateBubbleTop()
	{
		for (int i = 0; i < Singleton<LevelManager>.Instance.LTBubbleTop.Count; i++)
		{
			BubbleData data = Singleton<LevelManager>.Instance.LTBubbleTop[i];
			BoxObj box = Instance.GetBox(new Point(data.row, data.col));
			if (box != null && box.GetBubble() != null)
			{
				box.GetBubble().InitTop(data);
			}
		}
	}

	public BubbleObj BubbleInstantiate(BubbleData data, bool shootEndCreate = false)
	{
		ObjTmp = GameObjectManager.GetPoolObject("Obj/GameObj/Bubble");
		ObjTmp.transform.SetParent(BallPrefab.transform, worldPositionStays: false);
		ObjTmp.transform.localPosition = GetPosByRowAndCol(data.row, data.col);
		bubbleObj = ObjTmp.GetComponent<BubbleObj>();
		bubbleObj.Init(data, shootEndCreate);
		return bubbleObj;
	}

	public void ExitGame()
	{
		isExitGame = true;
		for (int i = 0; i < BubbleArray.Count; i++)
		{
			BubbleArray[i].gameObject.name = "Bubble";
			GameObjectManager.DestroyPoolObject(BubbleArray[i].GetComponent<BubblePool>());
		}
		BubbleArray.Clear();
		for (int j = 0; j < rows; j++)
		{
			for (int k = 0; k < cols - j % 2; k++)
			{
				BoxArray[j, k].gameObject.name = "Box";
				GameObjectManager.DestroyPoolObject(BoxArray[j, k].GetComponent<BoxPool>());
				BoxArray[j, k] = null;
			}
		}
	}

	public static bool GetFullScreen()
	{
		if ((float)(Screen.height / Screen.width) > 1.86f)
		{
			return true;
		}
		return false;
	}

	public Vector2 GetPosByRowAndCol(int row, int col)
	{
		if (GetFullScreen())
		{
			topHeight = 120f;
		}
		float num = orthographicSize * 100f - topHeight;
		posX = (float)(col * 2) * radius + radius + (float)(row % 2) * radius - 352f;
		posY = (float)(-row * 2) * radius * Mathf.Sin(1.04666674f) + num - radius;
		return new Vector2(posX / 100f, posY / 100f);
	}

	public bool IsValidPos(int nRow, int nCol)
	{
		if (nRow < 0 || nCol < 0)
		{
			return false;
		}
		if (nRow >= rows || nCol >= cols - nRow % 2)
		{
			return false;
		}
		return true;
	}

	public List<Point> GetAround(Point point)
	{
		List<Point> list = new List<Point>();
		if (!IsValidPos(point.X, point.Y))
		{
			return list;
		}
		if (IsValidPos(point.X, point.Y - 1))
		{
			list.Add(new Point(point.X, point.Y - 1));
		}
		if (IsValidPos(point.X, point.Y + 1))
		{
			list.Add(new Point(point.X, point.Y + 1));
		}
		if (IsValidPos(point.X - 1, point.Y))
		{
			list.Add(new Point(point.X - 1, point.Y));
		}
		if (IsValidPos(point.X + 1, point.Y))
		{
			list.Add(new Point(point.X + 1, point.Y));
		}
		int num = (point.X % 2 != 0) ? (point.Y + 1) : (point.Y - 1);
		if (IsValidPos(point.X - 1, num))
		{
			list.Add(new Point(point.X - 1, num));
		}
		if (IsValidPos(point.X + 1, num))
		{
			list.Add(new Point(point.X + 1, num));
		}
		return list;
	}

	public void GetAnimList(List<Point> vecPos, List<Point> _list, Point point)
	{
		for (int i = 0; i < _list.Count; i++)
		{
			BoxObj box = Instance.GetBox(_list[i]);
			if (!(box != null) || !(box.GetBubble() != null))
			{
				continue;
			}
			List<Point> around = Instance.GetAround(box.GetBubble().point);
			for (int j = 0; j < around.Count; j++)
			{
				Point point2 = around[j];
				if (point2.X == point.X)
				{
					Point point3 = around[j];
					if (point3.Y == point.Y)
					{
						continue;
					}
				}
				box = Instance.GetBox(around[j]);
				if (box != null && box.GetBubble() != null && !_list.Contains(around[j]))
				{
					vecPos.Add(around[j]);
				}
			}
		}
	}

	public void GetAnimList2(List<Point> vecPos, List<Point> _list, Point point)
	{
		for (int i = 0; i < _list.Count; i++)
		{
			BoxObj box = Instance.GetBox(_list[i]);
			if (!(box != null))
			{
				continue;
			}
			List<Point> around = Instance.GetAround(_list[i]);
			for (int j = 0; j < around.Count; j++)
			{
				Point point2 = around[j];
				if (point2.X == point.X)
				{
					Point point3 = around[j];
					if (point3.Y == point.Y)
					{
						continue;
					}
				}
				box = Instance.GetBox(around[j]);
				if (box != null && !_list.Contains(around[j]))
				{
					vecPos.Add(around[j]);
				}
			}
		}
	}

	public void GetBubbleShow()
	{
		gameShowBubble.Clear();
		List<BubbleShow> list = new List<BubbleShow>();
		for (int i = 0; i < Singleton<LevelManager>.Instance.gemSpawnChance.Count; i++)
		{
			BubbleShow item = default(BubbleShow);
			LevelManager.LevelObject levelObject = Singleton<LevelManager>.Instance.gemSpawnChance[i];
			item.key = levelObject.key;
			item.show = false;
			LevelManager.LevelObject levelObject2 = Singleton<LevelManager>.Instance.gemSpawnChance[i];
			item.P = levelObject2.value;
			list.Add(item);
		}
		for (int j = 0; j < list.Count; j++)
		{
			BubbleShow item2 = list[j];
			int num = int.Parse(Singleton<DataBubble>.Instance.GetContentByKeyAndType(item2.key, BubbleType.type));
			for (int k = 0; k < BubbleArray.Count; k++)
			{
				BubbleObj bubbleObj = BubbleArray[k];
				if (num == bubbleObj.GetBubbleType() && !gameShowBubble.Contains(item2))
				{
					item2.show = true;
					gameShowBubble.Add(item2);
					break;
				}
			}
		}
	}

	public void ChangeReadyBubble()
	{
		GetBubbleShow();
		if (gameShowBubble.Count == 0)
		{
			int index = UnityEngine.Random.Range(0, Singleton<LevelManager>.Instance.gemSpawnChance.Count - 1);
			BubbleShow item = default(BubbleShow);
			LevelManager.LevelObject levelObject = Singleton<LevelManager>.Instance.gemSpawnChance[index];
			item.key = levelObject.key;
			item.show = true;
			LevelManager.LevelObject levelObject2 = Singleton<LevelManager>.Instance.gemSpawnChance[index];
			item.P = levelObject2.value;
			gameShowBubble.Add(item);
		}
		ReadyBubbleController.Instance.ChangeReadyBubleColor(gameShowBubble);
	}

	public string GetRandomKey()
	{
		GetBubbleShow();
		if (gameShowBubble.Count == 0)
		{
			int index = UnityEngine.Random.Range(0, Singleton<LevelManager>.Instance.gemSpawnChance.Count - 1);
			LevelManager.LevelObject levelObject = Singleton<LevelManager>.Instance.gemSpawnChance[index];
			return levelObject.key;
		}
		int num = UnityEngine.Random.Range(0, gameShowBubble.Count);
		if (num == gameShowBubble.Count)
		{
			num = gameShowBubble.Count - 1;
		}
		BubbleShow bubbleShow = gameShowBubble[num];
		string text = bubbleShow.key;
		if (text == string.Empty)
		{
			LevelManager.LevelObject levelObject2 = Singleton<LevelManager>.Instance.gemSpawnChance[0];
			text = levelObject2.key;
		}
		if (text == readykey)
		{
			readyindex++;
		}
		else
		{
			readyindex = 1;
		}
		int num2 = 0;
		int num3 = 0;
		if (readyindex >= 2 && gameShowBubble.Count > 1)
		{
			List<BubbleShow> list = new List<BubbleShow>();
			for (int i = 0; i < gameShowBubble.Count; i++)
			{
				BubbleShow bubbleShow2 = gameShowBubble[i];
				if (bubbleShow2.key != readykey)
				{
					list.Add(gameShowBubble[i]);
				}
			}
			num2 = 0;
			for (int j = 0; j < list.Count; j++)
			{
				BubbleShow bubbleShow3 = list[j];
				num2 += bubbleShow3.P;
			}
			text = string.Empty;
			num3 = UnityEngine.Random.Range(0, num2);
			if (num3 == num2)
			{
				num3--;
			}
			for (int k = 0; k < list.Count; k++)
			{
				BubbleShow bubbleShow4 = list[k];
				num2 += bubbleShow4.P;
				if (num2 >= num3)
				{
					text = bubbleShow4.key;
					break;
				}
			}
			if (text == string.Empty)
			{
				BubbleShow bubbleShow5 = list[0];
				text = bubbleShow5.key;
			}
			if (list.Count == 1)
			{
				BubbleShow bubbleShow6 = list[0];
				text = bubbleShow6.key;
			}
		}
		readykey = text;
		return text;
	}

	public Vector3 GetCenterPost(Vector3 Start, Vector3 End, float r = 0f, bool _bOrientation = false)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = (Start.x + End.x) / 2f;
		float num4 = (Start.y + End.y) / 2f;
		num = num3 - Start.x;
		num2 = num4 - Start.y;
		float num5 = 0f;
		num5 = num;
		if (_bOrientation)
		{
			num = num2 / r;
			num2 = (0f - num5) / r;
		}
		else
		{
			num = (0f - num2) / r;
			num2 = num5 / r;
		}
		num = num3 - num;
		num2 = num4 - num2;
		return new Vector3(num, num2, Start.z);
	}
}
