using UnityEngine;

public class TKTouch
{
	public readonly int fingerId;

	public Vector2 position;

	public Vector2 startPosition;

	public Vector2 deltaPosition;

	public float deltaTime;

	public int tapCount;

	public TouchPhase phase = TouchPhase.Ended;

	public Vector2 previousPosition => position - deltaPosition;

	public TKTouch(int fingerId)
	{
		this.fingerId = fingerId;
	}

	public TKTouch populateWithTouch(Touch touch)
	{
		position = touch.position;
		deltaPosition = touch.deltaPosition;
		deltaTime = touch.deltaTime;
		tapCount = touch.tapCount;
		if (touch.phase == TouchPhase.Began)
		{
			startPosition = position;
		}
		if (touch.phase == TouchPhase.Canceled)
		{
			phase = TouchPhase.Ended;
		}
		else
		{
			phase = touch.phase;
		}
		return this;
	}

	public override string ToString()
	{
		return $"[TKTouch] fingerId: {fingerId}, phase: {phase}, position: {position}";
	}
}
