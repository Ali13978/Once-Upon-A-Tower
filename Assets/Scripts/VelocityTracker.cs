using System;
using UnityEngine;

public class VelocityTracker
{
	private const int NUM_PAST = 10;

	private const float LONGEST_PAST_TIME = 0.2f;

	private float[] pastX = new float[10];

	private float[] pastY = new float[10];

	private float[] pastTime = new float[10];

	public void Reset()
	{
		pastTime[0] = 0f;
	}

	public void AddPoint(Vector2 point, float time)
	{
		AddPoint(point.x, point.y, time);
	}

	private void AddPoint(float x, float y, float time)
	{
		int num = -1;
		int i;
		for (i = 0; i < 10 && pastTime[i] != 0f; i++)
		{
			if (pastTime[i] < time - 0.2f)
			{
				num = i;
			}
		}
		if (i == 10 && num < 0)
		{
			num = 0;
		}
		if (num == i)
		{
			num--;
		}
		if (num >= 0)
		{
			int sourceIndex = num + 1;
			int length = 10 - num - 1;
			Array.Copy(pastX, sourceIndex, pastX, 0, length);
			Array.Copy(pastY, sourceIndex, pastY, 0, length);
			Array.Copy(pastTime, sourceIndex, pastTime, 0, length);
			i -= num + 1;
		}
		pastX[i] = x;
		pastY[i] = y;
		pastTime[i] = time;
		i++;
		if (i < 10)
		{
			pastTime[i] = 0f;
		}
	}

	public Vector2 ComputeCurrentVelocity(int units)
	{
		return ComputeCurrentVelocity(units, float.MaxValue);
	}

	public Vector2 ComputeCurrentVelocity(int units, float maxVelocity)
	{
		float num = pastX[0];
		float num2 = pastY[0];
		float num3 = pastTime[0];
		float num4 = 0f;
		float num5 = 0f;
		int i;
		for (i = 0; i < 10 && pastTime[i] != 0f; i++)
		{
		}
		if (i > 3)
		{
			i--;
		}
		for (int j = 1; j < i; j++)
		{
			float num6 = pastTime[j] - num3;
			if (num6 != 0f)
			{
				float num7 = pastX[j] - num;
				float num8 = num7 / num6 * (float)units;
				num4 = ((num4 != 0f) ? ((num4 + num8) * 0.5f) : num8);
				num7 = pastY[j] - num2;
				num8 = num7 / num6 * (float)units;
				num5 = ((num5 != 0f) ? ((num5 + num8) * 0.5f) : num8);
			}
		}
		float x = (!(num4 < 0f)) ? Mathf.Min(num4, maxVelocity) : Mathf.Max(num4, 0f - maxVelocity);
		float y = (!(num5 < 0f)) ? Mathf.Min(num5, maxVelocity) : Mathf.Max(num5, 0f - maxVelocity);
		return new Vector2(x, y);
	}
}
