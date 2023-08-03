using System;
using UnityEngine;

[Serializable]
public class GameObjectTranslation
{
	public float Forward;

	public float Backward;

	public float Period = 1f;

	public float StartTime;

	public float Attenuation;

	public float StopTime;

	public float PeriodTimePerDirection;

	public float StartTimePerDirection;

	private bool move = true;

	[HideInInspector]
	public bool Move
	{
		get
		{
			return move;
		}
		set
		{
			if (move && !value)
			{
				StopTime = Time.fixedTime;
			}
			if (!move && value && !MoveAfterStop(Time.fixedTime))
			{
				StartTime = Time.fixedTime;
			}
			move = value;
		}
	}

	public Vector3 AtTime(float time)
	{
		if (Forward == 0f && Backward == 0f)
		{
			return Vector3.zero;
		}
		if (!Move && !MoveAfterStop(time))
		{
			return Vector3.zero;
		}
		Vector3 a = new Vector3(0f, 1f, 0f);
		for (time -= StartTime; time < 0f; time += Period)
		{
		}
		float alpha = time % Period / Period;
		return a * QuadraticOffsetAtAlpha(alpha) * (1f - Attenuation);
	}

	private bool MoveAfterStop(float time)
	{
		time -= StartTime;
		float num = StopTime - StartTime;
		if (num % Period > Period / 2f)
		{
			return time < num + (Period - num % Period);
		}
		if (num % Period > 0f)
		{
			return time < num + (Period / 2f - num % (Period / 2f));
		}
		return false;
	}

	private float LinearOffsetAtAlpha(float alpha)
	{
		float num = (Forward + Backward) * 2f;
		if (num == 0f)
		{
			return 0f;
		}
		float num2 = Forward / num;
		if (alpha <= num2)
		{
			return num * alpha;
		}
		float num3 = (Forward * 2f + Backward) / num;
		if (alpha <= num3)
		{
			return Forward - num * (alpha - num2);
		}
		return 0f - Backward + num * (alpha - num3);
	}

	private float SinOffsetAtAlpha(float alpha)
	{
		return Mathf.Sin(alpha * (float)Math.PI * 2f) * (Forward + Backward) / 2f;
	}

	private float QuadraticOffsetAtAlpha(float alpha)
	{
		float num = (Forward + Backward) / 2f;
		if (num == 0f)
		{
			return 0f;
		}
		float num2 = 0.4f / Period;
		float num3 = -4f * num / (num2 - 1f);
		if (alpha < 0.25f - num2 / 2f)
		{
			return num3 * alpha;
		}
		float num4 = 4f * num / (num2 * num2 - num2);
		float num5 = -2f * num / (num2 * num2 - num2);
		float num6 = 0.25f * (4f * num2 * num2 - 4f * num2 + 1f) * num / (num2 * num2 - num2);
		if (alpha < 0.25f + num2 / 2f)
		{
			return num4 * alpha * alpha + num5 * alpha + num6;
		}
		if (alpha < 0.75f - num2 / 2f)
		{
			return (0f - num3) * alpha + num3 / 2f;
		}
		if (alpha < 0.75f + num2 / 2f)
		{
			alpha -= 0.5f;
			return (0f - num4) * alpha * alpha - num5 * alpha - num6;
		}
		return num3 * alpha - num3;
	}
}
