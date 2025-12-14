
using System;
using UnityEngine;

namespace Thinksquirrel.CShake
{
	[AddComponentMenu("")]
	public abstract class CameraShakeBase : MonoBehaviour
	{
		public static void Log(object message, string prefix, string type)
		{
			UnityEngine.Debug.Log($"[{prefix}] ({type}): {message}");
		}

		public static void LogWarning(object message, string prefix, string type)
		{
			UnityEngine.Debug.LogWarning($"[{prefix}] ({type}): {message}");
		}

		public static void LogError(object message, string prefix, string type)
		{
			UnityEngine.Debug.LogError($"[{prefix}] ({type}): {message}");
		}

		public static void LogException(Exception ex)
		{
			UnityEngine.Debug.LogException(ex);
		}

		public static void Log(object message, string prefix, string type, UnityEngine.Object context)
		{
			UnityEngine.Debug.Log($"[{prefix}] ({type}): {message}", context);
		}

		public static void LogWarning(object message, string prefix, string type, UnityEngine.Object context)
		{
			UnityEngine.Debug.LogWarning($"[{prefix}] ({type}): {message}", context);
		}

		public static void LogError(object message, string prefix, string type, UnityEngine.Object context)
		{
			UnityEngine.Debug.LogError($"[{prefix}] ({type}): {message}", context);
		}

		public static void LogException(Exception ex, UnityEngine.Object context)
		{
			UnityEngine.Debug.LogException(ex, context);
		}
	}
}
