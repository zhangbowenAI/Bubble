
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class ExpandMethod
{
	public static string ToSaveString(this Vector3 v3)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(v3.x.ToString());
		stringBuilder.Append("|");
		stringBuilder.Append(v3.y.ToString());
		stringBuilder.Append("|");
		stringBuilder.Append(v3.z.ToString());
		return stringBuilder.ToString();
	}

	public static string ToSaveString(this Vector2 v2)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(v2.x.ToString());
		stringBuilder.Append("|");
		stringBuilder.Append(v2.y.ToString());
		return stringBuilder.ToString();
	}

	public static string ToSaveString(this Color color)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(color.r.ToString());
		stringBuilder.Append("|");
		stringBuilder.Append(color.g.ToString());
		stringBuilder.Append("|");
		stringBuilder.Append(color.b.ToString());
		stringBuilder.Append("|");
		stringBuilder.Append(color.a.ToString());
		return stringBuilder.ToString();
	}

	public static string ToSaveString(this List<string> list)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < list.Count; i++)
		{
			stringBuilder.Append(list[i]);
			if (i != list.Count - 1)
			{
				stringBuilder.Append("|");
			}
		}
		return stringBuilder.ToString();
	}

	public static string ToSaveString(this string[] list)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < list.Length; i++)
		{
			stringBuilder.Append(list[i]);
			if (i != list.Length - 1)
			{
				stringBuilder.Append("|");
			}
		}
		return stringBuilder.ToString();
	}

	public static Vector3 Vector3RotateInXZ(this Vector3 dir, float angle)
	{
		angle *= (float)Math.PI / 180f;
		float x = dir.x * Mathf.Cos(angle) - dir.z * Mathf.Sin(angle);
		float z = dir.x * Mathf.Sin(angle) + dir.z * Mathf.Cos(angle);
		return new Vector3(x, dir.y, z);
	}

	public static Vector3 Vector3RotateInXZ2(this Vector3 dir, float angle)
	{
		angle *= (float)Math.PI / 180f;
		float x = dir.x * Mathf.Cos(angle) + dir.z * Mathf.Sin(angle);
		float z = (0f - dir.x) * Mathf.Sin(angle) + dir.z * Mathf.Cos(angle);
		return new Vector3(x, dir.y, z);
	}

	public static Vector3 PostionRotateInXZ(this Vector3 pos, Vector3 center, float angle)
	{
		angle *= -(float)Math.PI / 180f;
		float x = (pos.x - center.x) * Mathf.Cos(angle) - (pos.z - center.z) * Mathf.Sin(angle) + center.x;
		float z = (pos.x - center.x) * Mathf.Sin(angle) + (pos.z - center.z) * Mathf.Cos(angle) + center.z;
		return new Vector3(x, pos.y, z);
	}

	public static float GetRotationAngle(this Vector3 dir, Vector3 aimDir)
	{
		float num = (float)(Math.Acos(Vector3.Dot(dir, aimDir)) * 180.0 / Math.PI);
		if (num != 180f && num != 0f)
		{
			float num2 = dir.x * aimDir.z - aimDir.x * dir.z;
			if (num2 < 0f)
			{
				return num;
			}
			return 360f - num;
		}
		return num;
	}

	public static void SetScale(this Transform tr, Vector3 scale, bool recursion = true)
	{
		tr.localScale = scale;
		if (recursion)
		{
			IEnumerator enumerator = tr.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform tr2 = (Transform)enumerator.Current;
					tr2.SetScale(scale);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	public static void SetActiveOptimize(this GameObject go, bool isActive)
	{
		if (go.activeSelf != isActive)
		{
			go.SetActive(isActive);
		}
	}

	public static void CustomCrossFade(this Animator anim, string animName, int layer = 0, float time = 0.5f)
	{
		if (!anim.GetCurrentAnimatorStateInfo(layer).IsName(animName) && !anim.GetNextAnimatorStateInfo(layer).IsName(animName))
		{
			anim.CrossFade(animName, time, layer);
		}
	}

	public static float GetCrossFadeProgress(this Animator anim, int layer = 0)
	{
		if (anim.GetNextAnimatorStateInfo(layer).shortNameHash == 0)
		{
			return 1f;
		}
		return anim.GetCurrentAnimatorStateInfo(layer).normalizedTime % 1f;
	}

	public static void RecursionPlay(this GameObject ps)
	{
		ParticleSystem[] componentsInChildren = ps.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Play();
		}
	}

	public static void RecursionStop(this GameObject ps)
	{
		ParticleSystem[] componentsInChildren = ps.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Stop();
		}
	}

	public static void RecursionPause(this GameObject ps)
	{
		ParticleSystem[] componentsInChildren = ps.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Pause();
		}
	}
}
