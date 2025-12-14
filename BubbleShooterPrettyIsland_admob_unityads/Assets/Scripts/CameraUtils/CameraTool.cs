
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraTool : MonoBehaviour
{
	private static float decay = 0.01f;

	private static float time;

	private static float dctime;

	private static float intensity;

	private static float nwintensity;

	private static Vector3 oldpos;

	private static Transform ts;

	private static TimerEvent te;

	private static bool _lock = true;

	public static void shake(Transform _ts, float _intensity = 0.2f, float _time = 1f)
	{
		_lock = false;
		ts = _ts;
		time = _time * _intensity;
		dctime = 0f;
		nwintensity = decay / _intensity / _time * 0.2f;
		intensity = _intensity;
		oldpos = ts.position;
		te = Timer.CallBackOfIntervalTimer(decay, doUpdate);
	}

	private static void doUpdate(object[] l_objs)
	{
		if (!(ts == null) && !_lock)
		{
			if (time >= dctime)
			{
				ts.position = oldpos + UnityEngine.Random.insideUnitSphere * intensity;
				intensity -= nwintensity;
				dctime += decay;
			}
			else
			{
				_lock = true;
				ts.position = oldpos;
				Timer.DestroyTimer(te);
			}
		}
	}
}
