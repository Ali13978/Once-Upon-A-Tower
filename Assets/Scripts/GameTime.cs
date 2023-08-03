using System;
using UnityEngine;

public class GameTime
{
	private static bool paused;

	private static float[] slowMos;

	public static float originalFixedDelta;

	private static float fixedTimeOffset;

	public static float EditorTime;

	public static bool Paused => paused;

	public static bool IsSlow => Time.timeScale < 1f;

	public static float FixedTime => (!Application.isPlaying) ? EditorTime : (Time.fixedTime + fixedTimeOffset);

	public static float FixedDelta => Time.fixedDeltaTime;

	public static event Action OnPause;

	public static event Action OnResume;

	static GameTime()
	{
		originalFixedDelta = Time.fixedDeltaTime;
		slowMos = new float[2];
		for (int i = 0; i < slowMos.Length; i++)
		{
			slowMos[i] = 1f;
		}
	}

	public static void Pause()
	{
		if (!paused)
		{
			paused = true;
			UpdateTimeScale();
			if (GameTime.OnPause != null)
			{
				GameTime.OnPause();
			}
		}
	}

	public static void Resume()
	{
		if (paused)
		{
			paused = false;
			UpdateTimeScale();
			if (GameTime.OnResume != null)
			{
				GameTime.OnResume();
			}
		}
	}

	public static void Slow(float timeScale, SlowMoPriority priority = SlowMoPriority.Highest)
	{
		slowMos[(int)priority] = timeScale;
		UpdateTimeScale();
	}

	public static float GetTimeScale(SlowMoPriority priority = SlowMoPriority.Highest)
	{
		return slowMos[(int)priority];
	}

	public static void NormalSpeed(SlowMoPriority priority = SlowMoPriority.Highest)
	{
		slowMos[(int)priority] = 1f;
		UpdateTimeScale();
	}

	public static void NormalSpeedAll()
	{
		for (int i = 0; i < slowMos.Length; i++)
		{
			slowMos[i] = 1f;
		}
		UpdateTimeScale();
	}

	private static void UpdateTimeScale()
	{
		if (paused)
		{
			Time.timeScale = 0f;
			return;
		}
		float num = 1f;
		for (int i = 0; i < slowMos.Length; i++)
		{
			if (slowMos[i] != 1f)
			{
				num = slowMos[i];
				break;
			}
		}
		Time.fixedDeltaTime = originalFixedDelta * num;
		Time.timeScale = num;
	}

	public static void SetFixedTime(float time)
	{
		fixedTimeOffset = time - Time.fixedTime;
	}
}
