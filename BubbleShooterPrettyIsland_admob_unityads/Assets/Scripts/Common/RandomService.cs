
using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomService
{
	public class FixRandom
	{
		public int m_RandomSeed;

		private int m_randomA = 9301;

		private int m_randomB = 49297;

		private int m_randomC = 233280;

		public FixRandom(int seed)
		{
			SetFixRandomSeed(seed);
		}

		public void SetFixRandomSeed(int seed)
		{
			m_RandomSeed = seed;
		}

		public void SetFixRandomParm(int a, int b, int c)
		{
			m_randomA = a;
			m_randomB = b;
			m_randomC = c;
		}

		public int GetFixRandom()
		{
			m_RandomSeed = Math.Abs((m_RandomSeed * m_randomA + m_randomB) % m_randomC);
			return m_RandomSeed;
		}

		public int Range(int min, int max)
		{
			if (max <= min)
			{
				return min;
			}
			int fixRandom = GetFixRandom();
			int num = max - min;
			return fixRandom % num + min;
		}
	}

	private static RandomHandel s_onRandomCreat;

	private static bool s_isFixedRandom = false;

	private static List<int> s_randomList = new List<int>();

	public static RandomHandel OnRandomCreat
	{
		get
		{
			return s_onRandomCreat;
		}
		set
		{
			s_onRandomCreat = value;
		}
	}

	public static void SetRandomList(List<int> list)
	{
		s_isFixedRandom = true;
		s_randomList = list;
	}

	public static int GetRandomListCount()
	{
		return s_randomList.Count;
	}

	public static int GetRand(int min, int max)
	{
		return Range(min, max);
	}

	public static int GetRandReal(int min, int max)
	{
		return Range(min, max + 1);
	}

	public static int Range(int min, int max)
	{
		if (!s_isFixedRandom)
		{
			int num = UnityEngine.Random.Range(min, max);
			DispatchRandom(num);
			return num;
		}
		return GetFixedRandom();
	}

	public static float Range01()
	{
		int num = 0;
		if (s_isFixedRandom)
		{
			num = GetFixedRandom();
		}
		else
		{
			num = UnityEngine.Random.Range(0, 10001);
			DispatchRandom(num);
		}
		return (float)num / 10000f;
	}

	private static void DispatchRandom(int random)
	{
		if (s_onRandomCreat != null)
		{
			s_onRandomCreat(random);
		}
	}

	private static int GetFixedRandom()
	{
		if (s_randomList != null && s_randomList.Count > 0)
		{
			int result = s_randomList[0];
			s_randomList.RemoveAt(0);
			return result;
		}
		throw new Exception("RandomService Exception no RandomList!");
	}
}
