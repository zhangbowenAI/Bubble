
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
	private void Start()
	{
		List<RewardType> list = new List<RewardType>();
		list.Add(new RewardType(1, 10f));
		list.Add(new RewardType(2, 10f));
		list.Add(new RewardType(3, 10f));
		list.Add(new RewardType(4, 10f));
		list.Add(new RewardType(5, 10f));
		list.Add(new RewardType(6, 10f));
		UIManager.Reward(list);
	}

	private void Update()
	{
	}
}
