
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardWindow : UIWindowBase
{
	public Sprite[] rewardImg;

	public List<Vector2> list2Pos = new List<Vector2>();

	public List<Vector2> list3Pos = new List<Vector2>();

	public List<Vector2> list4Pos = new List<Vector2>();

	public List<Vector2> list5Pos = new List<Vector2>();

	public List<Vector2> list6Pos = new List<Vector2>();

	public List<Vector2> list7Pos = new List<Vector2>();

	public List<Vector2> list8Pos = new List<Vector2>();

	private float delayTime = 0.5f;

	public override void OnOpen()
	{
		GetComponent<Canvas>().overrideSorting = true;
		list2Pos.Add(new Vector2(-100f, 180f));
		list2Pos.Add(new Vector2(100f, 180f));
		list3Pos.Add(new Vector2(-180f, 180f));
		list3Pos.Add(new Vector2(0f, 180f));
		list3Pos.Add(new Vector2(180f, 180f));
		list4Pos.Add(new Vector2(-240f, 180f));
		list4Pos.Add(new Vector2(-80f, 180f));
		list4Pos.Add(new Vector2(80f, 180f));
		list4Pos.Add(new Vector2(240f, 180f));
		List<Vector2> list = list5Pos;
		Vector2 vector = list2Pos[0];
		float x = vector.x;
		Vector2 vector2 = list2Pos[0];
		list.Add(new Vector2(x, vector2.y));
		List<Vector2> list2 = list5Pos;
		Vector2 vector3 = list2Pos[1];
		float x2 = vector3.x;
		Vector2 vector4 = list2Pos[1];
		list2.Add(new Vector2(x2, vector4.y));
		List<Vector2> list3 = list5Pos;
		Vector2 vector5 = list3Pos[0];
		list3.Add(new Vector2(vector5.x, 0f));
		List<Vector2> list4 = list5Pos;
		Vector2 vector6 = list3Pos[1];
		list4.Add(new Vector2(vector6.x, 0f));
		List<Vector2> list5 = list5Pos;
		Vector2 vector7 = list3Pos[2];
		list5.Add(new Vector2(vector7.x, 0f));
		List<Vector2> list6 = list6Pos;
		Vector2 vector8 = list3Pos[0];
		float x3 = vector8.x;
		Vector2 vector9 = list3Pos[0];
		list6.Add(new Vector2(x3, vector9.y));
		List<Vector2> list7 = list6Pos;
		Vector2 vector10 = list3Pos[1];
		float x4 = vector10.x;
		Vector2 vector11 = list3Pos[1];
		list7.Add(new Vector2(x4, vector11.y));
		List<Vector2> list8 = list6Pos;
		Vector2 vector12 = list3Pos[2];
		float x5 = vector12.x;
		Vector2 vector13 = list3Pos[2];
		list8.Add(new Vector2(x5, vector13.y));
		List<Vector2> list9 = list6Pos;
		Vector2 vector14 = list3Pos[0];
		list9.Add(new Vector2(vector14.x, 0f));
		List<Vector2> list10 = list6Pos;
		Vector2 vector15 = list3Pos[1];
		list10.Add(new Vector2(vector15.x, 0f));
		List<Vector2> list11 = list6Pos;
		Vector2 vector16 = list3Pos[2];
		list11.Add(new Vector2(vector16.x, 0f));
		List<Vector2> list12 = list7Pos;
		Vector2 vector17 = list3Pos[0];
		float x6 = vector17.x;
		Vector2 vector18 = list3Pos[0];
		list12.Add(new Vector2(x6, vector18.y));
		List<Vector2> list13 = list7Pos;
		Vector2 vector19 = list3Pos[1];
		float x7 = vector19.x;
		Vector2 vector20 = list3Pos[1];
		list13.Add(new Vector2(x7, vector20.y));
		List<Vector2> list14 = list7Pos;
		Vector2 vector21 = list3Pos[2];
		float x8 = vector21.x;
		Vector2 vector22 = list3Pos[2];
		list14.Add(new Vector2(x8, vector22.y));
		List<Vector2> list15 = list7Pos;
		Vector2 vector23 = list4Pos[0];
		list15.Add(new Vector2(vector23.x, 0f));
		List<Vector2> list16 = list7Pos;
		Vector2 vector24 = list4Pos[1];
		list16.Add(new Vector2(vector24.x, 0f));
		List<Vector2> list17 = list7Pos;
		Vector2 vector25 = list4Pos[2];
		list17.Add(new Vector2(vector25.x, 0f));
		List<Vector2> list18 = list7Pos;
		Vector2 vector26 = list4Pos[3];
		list18.Add(new Vector2(vector26.x, 0f));
		List<Vector2> list19 = list8Pos;
		Vector2 vector27 = list4Pos[0];
		float x9 = vector27.x;
		Vector2 vector28 = list4Pos[0];
		list19.Add(new Vector2(x9, vector28.y));
		List<Vector2> list20 = list8Pos;
		Vector2 vector29 = list4Pos[1];
		float x10 = vector29.x;
		Vector2 vector30 = list4Pos[1];
		list20.Add(new Vector2(x10, vector30.y));
		List<Vector2> list21 = list8Pos;
		Vector2 vector31 = list4Pos[2];
		float x11 = vector31.x;
		Vector2 vector32 = list4Pos[2];
		list21.Add(new Vector2(x11, vector32.y));
		List<Vector2> list22 = list8Pos;
		Vector2 vector33 = list4Pos[0];
		list22.Add(new Vector2(vector33.x, 0f));
		List<Vector2> list23 = list8Pos;
		Vector2 vector34 = list4Pos[1];
		list23.Add(new Vector2(vector34.x, 0f));
		List<Vector2> list24 = list8Pos;
		Vector2 vector35 = list4Pos[2];
		list24.Add(new Vector2(vector35.x, 0f));
		List<Vector2> list25 = list8Pos;
		Vector2 vector36 = list4Pos[3];
		list25.Add(new Vector2(vector36.x, 0f));
	}

	public void InitReward(List<RewardType> rewardList)
	{
		if (rewardList.Count == 1)
		{
			Reward(rewardList[0], new Vector2(0f, 180f), 1, isover: true);
		}
		else if (rewardList.Count == 2)
		{
			Reward(rewardList[0], list2Pos[0], 1, isover: false);
			Reward(rewardList[1], list2Pos[1], 2, isover: true);
		}
		else if (rewardList.Count == 3)
		{
			Reward(rewardList[0], list3Pos[0], 1, isover: false);
			Reward(rewardList[1], list3Pos[1], 2, isover: false);
			Reward(rewardList[2], list3Pos[2], 3, isover: true);
		}
		else if (rewardList.Count == 4)
		{
			Reward(rewardList[0], list4Pos[0], 1, isover: false);
			Reward(rewardList[1], list4Pos[1], 2, isover: false);
			Reward(rewardList[2], list4Pos[2], 3, isover: false);
			Reward(rewardList[3], list4Pos[3], 4, isover: true);
		}
		else if (rewardList.Count == 5)
		{
			Reward(rewardList[0], list5Pos[0], 1, isover: false);
			Reward(rewardList[1], list5Pos[1], 2, isover: false);
			Reward(rewardList[2], list5Pos[2], 3, isover: false);
			Reward(rewardList[3], list5Pos[3], 4, isover: false);
			Reward(rewardList[4], list5Pos[4], 5, isover: true);
		}
		else if (rewardList.Count == 6)
		{
			Reward(rewardList[0], list6Pos[0], 1, isover: false);
			Reward(rewardList[1], list6Pos[1], 2, isover: false);
			Reward(rewardList[2], list6Pos[2], 3, isover: false);
			Reward(rewardList[3], list6Pos[3], 4, isover: false);
			Reward(rewardList[4], list6Pos[4], 5, isover: false);
			Reward(rewardList[5], list6Pos[5], 6, isover: true);
		}
		else if (rewardList.Count == 7)
		{
			Reward(rewardList[0], list7Pos[0], 1, isover: false);
			Reward(rewardList[1], list7Pos[1], 2, isover: false);
			Reward(rewardList[2], list7Pos[2], 3, isover: false);
			Reward(rewardList[3], list7Pos[3], 4, isover: false);
			Reward(rewardList[4], list7Pos[4], 5, isover: false);
			Reward(rewardList[5], list7Pos[5], 6, isover: false);
			Reward(rewardList[6], list7Pos[6], 7, isover: true);
		}
		else if (rewardList.Count == 8)
		{
			Reward(rewardList[0], list8Pos[0], 1, isover: false);
			Reward(rewardList[1], list8Pos[1], 2, isover: false);
			Reward(rewardList[2], list8Pos[2], 3, isover: false);
			Reward(rewardList[3], list8Pos[3], 4, isover: false);
			Reward(rewardList[4], list8Pos[4], 5, isover: false);
			Reward(rewardList[5], list8Pos[5], 6, isover: false);
			Reward(rewardList[6], list8Pos[6], 7, isover: false);
			Reward(rewardList[7], list8Pos[7], 8, isover: true);
		}
	}

	private void Reward(RewardType reward, Vector2 pos, int index, bool isover)
	{
		Timer.DelayCallBack(delayTime * (float)index, delegate
		{
			InitRewardObj(reward, pos, isover);
		});
	}

	private void InitRewardObj(RewardType reward, Vector2 pos, bool isover)
	{
		GameObject ObjTmp = GameObjectManager.CreateGameObject("Reward/RewardObj");
		ObjTmp.transform.SetParent(m_uiRoot.transform, worldPositionStays: false);
		ObjTmp.transform.localPosition = pos;
		ObjTmp.transform.Find("Text").GetComponent<Text>().text = "x" + reward.num;
		ObjTmp.transform.localScale = new Vector2(0.6f, 0.6f);
		ObjTmp.GetComponent<Image>().sprite = rewardImg[reward.type - 1];
		ObjTmp.GetComponent<Canvas>().overrideSorting = true;
		GameObject gameObject = GameObjectManager.CreateGameObject("Reward/jiangli_tongyong");
		gameObject.transform.SetParent(m_uiRoot.transform, worldPositionStays: false);
		gameObject.transform.localPosition = pos;
		UnityEngine.Object.Destroy(gameObject, 0.5f);
		AudioPlayManager.PlaySFX2D("ui_reward");
		Sequence s = DOTween.Sequence();
		s.Append(ObjTmp.transform.DOScale(new Vector3(0.900000036f, 0.900000036f, 1.5f), 0.3f).SetEase(Ease.OutSine)).Append(ObjTmp.transform.DOScale(new Vector3(0.480000019f, 0.480000019f, 0.8f), 0.3f).SetEase(Ease.InSine)).Append(ObjTmp.transform.DOScale(new Vector3(0.6f, 0.6f, 1f), 0.3f).SetEase(Ease.OutSine))
			.OnComplete(delegate
			{
				FlyEnd(reward, ObjTmp, isover);
			});
	}

	public void FlyEnd(RewardType reward, GameObject ObjTmp, bool isover)
	{
		if (reward.type > 2 && reward.type < 9)
		{
			Singleton<UserData>.Instance.AddProps(reward.type - 3, (int)reward.num);
			UnityEngine.Object.Destroy(ObjTmp, 1.3f);
			if (isover)
			{
				StartCoroutine(CloseReward());
			}
		}
		else if (reward.type <= 2)
		{
			if (UIManager.GetFixedUICount() > 0)
			{
				TopUIWindow.Instance.ShowSort();
			}
			else
			{
				UIManager.OpenUIWindow<TopUIWindow>();
				TopUIWindow.Instance.ShowSort();
				Singleton<UserData>.Instance.IsNewTopUI = true;
			}
			if (reward.type == 1)
			{
				ObjTmp.transform.DOMove(TopUIWindow.Instance.LovePos.transform.position, 0.5f).OnComplete(delegate
				{
					Singleton<UserData>.Instance.AddLoveCount((int)reward.num);
					UnityEngine.Object.Destroy(ObjTmp);
					if (isover)
					{
						TopUIWindow.Instance.HideSort();
						UIManager.CloseUIWindow<RewardWindow>(isPlayAnim: false, null, new object[0]);
						if (Singleton<UserData>.Instance.IsNewTopUI)
						{
							Singleton<UserData>.Instance.IsNewTopUI = false;
							if ((bool)TopUIWindow.Instance)
							{
								UIManager.CloseUIWindow<TopUIWindow>(isPlayAnim: false, null, new object[0]);
							}
						}
					}
				});
			}
			else if (reward.type == 2)
			{
				ObjTmp.transform.DOMove(TopUIWindow.Instance.GoldPos.transform.position, 0.5f).OnComplete(delegate
				{
					Singleton<UserData>.Instance.AddUserGold((int)reward.num);
					UnityEngine.Object.Destroy(ObjTmp);
					if (isover)
					{
						TopUIWindow.Instance.HideSort();
						UIManager.CloseUIWindow<RewardWindow>(isPlayAnim: false, null, new object[0]);
						if (Singleton<UserData>.Instance.IsNewTopUI)
						{
							if ((bool)TopUIWindow.Instance)
							{
								UIManager.CloseUIWindow<TopUIWindow>(isPlayAnim: false, null, new object[0]);
							}
							Singleton<UserData>.Instance.IsNewTopUI = false;
						}
					}
				});
			}
		}
		else
		{
			if (UIManager.GetFixedUICount() > 0)
			{
				TopUIWindow.Instance.ShowSort();
			}
			else
			{
				UIManager.OpenUIWindow<TopUIWindow>();
				TopUIWindow.Instance.ShowSort();
				Singleton<UserData>.Instance.IsNewTopUI = true;
			}
			Singleton<UserData>.Instance.AddLoveInfinite(reward.type, reward.num);
			ObjTmp.transform.DOMove(TopUIWindow.Instance.LovePos.transform.position, 0.5f).OnComplete(delegate
			{
				UnityEngine.Object.Destroy(ObjTmp);
				if (isover)
				{
					TopUIWindow.Instance.HideSort();
					UIManager.CloseUIWindow<RewardWindow>(isPlayAnim: false, null, new object[0]);
					if (Singleton<UserData>.Instance.IsNewTopUI)
					{
						Singleton<UserData>.Instance.IsNewTopUI = false;
						UIManager.CloseUIWindow<TopUIWindow>(isPlayAnim: false, null, new object[0]);
					}
				}
			});
		}
	}

	private IEnumerator CloseReward()
	{
		yield return new WaitForSeconds(0.3f);
		if ((bool)TopUIWindow.Instance)
		{
			TopUIWindow.Instance.HideSort();
		}
		if (Singleton<UserData>.Instance.IsNewTopUI)
		{
			Singleton<UserData>.Instance.IsNewTopUI = false;
			UIManager.CloseUIWindow<TopUIWindow>(isPlayAnim: false, null, new object[0]);
		}
		UIManager.CloseUIWindow<RewardWindow>(isPlayAnim: false, null, new object[0]);
	}

	public override IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
	{
		animComplete(this, callBack, objs);
		yield return new WaitForEndOfFrame();
	}

	public override IEnumerator ExitAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
	{
		animComplete(this, callBack, objs);
		yield return new WaitForEndOfFrame();
	}

	public override IEnumerator PauserAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
	{
		animComplete(this, callBack, objs);
		yield return new WaitForEndOfFrame();
	}
}
