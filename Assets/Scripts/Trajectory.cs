using UnityEngine;

public struct Trajectory
{
	private struct DeltaVelocity
	{
		public float delta;

		public float velocity;
	}

	private float startTime;

	private Vector3 startPosition;

	private Vector3 startVelocity;

	private Vector3 targetPosition;

	public float acceleration;

	public float delta;

	public bool decelerating;

	public bool Active
	{
		get;
		private set;
	}

	public void Setup(Movable moveable, Vector3 target)
	{
		Active = true;
		startTime = Time.time;
		startPosition = moveable.transform.position;
		targetPosition = target;
		startVelocity = moveable.Velocity;
		delta = targetPosition.x - startPosition.x;
		acceleration = moveable.Acceleration * Mathf.Sign(delta);
		decelerating = false;
		if (moveable.DebugLog)
		{
			UnityEngine.Debug.Log("Setup trajectory " + startVelocity.x + " " + delta + " " + acceleration);
		}
	}

	public void Cancel(Movable moveable)
	{
		if (moveable.DebugLog)
		{
			UnityEngine.Debug.Log("Trajectory Cancel");
		}
		Active = false;
	}

	public bool Apply(Movable moveable)
	{
		if (!Active)
		{
			return true;
		}
		float num = Time.time - startTime;
		float num2 = Mathf.Sign(delta) * moveable.MoveSpeed;
		float num3 = (num2 - startVelocity.x) / acceleration;
		float num4 = 0f;
		float num5 = num2 / acceleration;
		if (moveable.DebugLog)
		{
			UnityEngine.Debug.Log("before  " + num3 + " " + num4 + " " + num5);
		}
		if (num3 < 0f)
		{
			num3 = 0f;
		}
		DeltaVelocity deltaVelocity = AccelerateCoastDecelerate(num3, num4, num5);
		if (Overshoot(delta, deltaVelocity.delta))
		{
			int num6 = 10;
			float num7 = 0f;
			float num8 = num3;
			for (int i = 0; i < num6; i++)
			{
				num3 = (num8 + num7) / 2f;
				float num9 = startVelocity.x + acceleration * num3;
				num5 = Mathf.Abs(num9 / acceleration);
				deltaVelocity = AccelerateCoastDecelerate(num3, num4, num5);
				if (Overshoot(delta, deltaVelocity.delta))
				{
					num8 = num3;
				}
				else
				{
					num7 = num3;
				}
			}
		}
		else
		{
			num4 = Mathf.Abs(delta - deltaVelocity.delta) / moveable.MoveSpeed;
		}
		if (moveable.DebugLog)
		{
			UnityEngine.Debug.Log("after  " + num3 + " " + num4 + " " + num5);
		}
		if (num >= num3 + num4 + num5)
		{
			return true;
		}
		if (num >= num3 + num4)
		{
			num5 = num - num3 - num4;
		}
		else if (num >= num3)
		{
			num5 = 0f;
			num4 = num - num3;
		}
		else
		{
			num5 = (num4 = 0f);
			num3 = num;
		}
		decelerating = (num5 > 0f);
		deltaVelocity = AccelerateCoastDecelerate(num3, num4, num5);
		moveable.Velocity = Vector3.right * deltaVelocity.velocity;
		Vector3 position = moveable.transform.position;
		position.x = startPosition.x + deltaVelocity.delta;
		moveable.transform.position = position;
		return false;
	}

	private bool Overshoot(float expectedDelta, float actualDelta)
	{
		return (expectedDelta < 0f && actualDelta <= expectedDelta) || (expectedDelta > 0f && actualDelta >= expectedDelta);
	}

	private DeltaVelocity AccelerateCoastDecelerate(float t0, float t1, float t2)
	{
		float num = startVelocity.x * t0 + acceleration * t0 * t0 / 2f;
		float num2 = startVelocity.x + acceleration * t0;
		float num3 = t1 * num2;
		float num4 = num2 * t2 - acceleration * t2 * t2 / 2f;
		DeltaVelocity result = default(DeltaVelocity);
		result.delta = num + num3 + num4;
		result.velocity = num2 - acceleration * t2;
		return result;
	}
}
