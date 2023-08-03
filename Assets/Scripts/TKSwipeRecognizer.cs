using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TKSwipeRecognizer : TKAbstractGestureRecognizer
{
	public float timeToSwipe = 0.5f;

	public int minimumNumberOfTouches = 1;

	public int maximumNumberOfTouches = 2;

	public bool triggerWhenCriteriaMet = true;

	private float _minimumDistance = 2f;

	private List<Vector2> _points = new List<Vector2>();

	private float _startTime;

	public float swipeVelocity
	{
		get;
		private set;
	}

	public TKSwipeDirection completedSwipeDirection
	{
		get;
		private set;
	}

	public Vector2 startPoint => _points.FirstOrDefault();

	public Vector2 endPoint => _points.LastOrDefault();

	public event Action<TKSwipeRecognizer> gestureRecognizedEvent;

	public TKSwipeRecognizer()
		: this(2f)
	{
	}

	public TKSwipeRecognizer(float minimumDistanceCm)
	{
		_minimumDistance = minimumDistanceCm;
	}

	private bool checkForSwipeCompletion(TKTouch touch)
	{
		if (timeToSwipe > 0f && Time.time - _startTime > timeToSwipe)
		{
			return false;
		}
		if (_points.Count < 2)
		{
			return false;
		}
		float num = Vector2.Distance(startPoint, endPoint);
		float num2 = num / TouchKit.instance.ScreenPixelsPerCm;
		if (num2 < _minimumDistance)
		{
			return false;
		}
		float num3 = 0f;
		for (int i = 1; i < _points.Count; i++)
		{
			num3 += Vector2.Distance(_points[i], _points[i - 1]);
		}
		if (num3 > num * 1.1f)
		{
			return false;
		}
		swipeVelocity = num2 / (Time.time - _startTime);
		Vector2 normalized = (endPoint - startPoint).normalized;
		float num4 = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
		if (num4 < 0f)
		{
			num4 = 360f + num4;
		}
		num4 = 360f - num4;
		if (num4 >= 292.5f && num4 <= 337.5f)
		{
			completedSwipeDirection = TKSwipeDirection.UpRight;
		}
		else if (num4 >= 247.5f && num4 <= 292.5f)
		{
			completedSwipeDirection = TKSwipeDirection.Up;
		}
		else if (num4 >= 202.5f && num4 <= 247.5f)
		{
			completedSwipeDirection = TKSwipeDirection.UpLeft;
		}
		else if (num4 >= 157.5f && num4 <= 202.5f)
		{
			completedSwipeDirection = TKSwipeDirection.Left;
		}
		else if (num4 >= 112.5f && num4 <= 157.5f)
		{
			completedSwipeDirection = TKSwipeDirection.DownLeft;
		}
		else if (num4 >= 67.5f && num4 <= 112.5f)
		{
			completedSwipeDirection = TKSwipeDirection.Down;
		}
		else if (num4 >= 22.5f && num4 <= 67.5f)
		{
			completedSwipeDirection = TKSwipeDirection.DownRight;
		}
		else
		{
			completedSwipeDirection = TKSwipeDirection.Right;
		}
		return true;
	}

	internal override void fireRecognizedEvent()
	{
		if (this.gestureRecognizedEvent != null)
		{
			this.gestureRecognizedEvent(this);
		}
	}

	internal override bool touchesBegan(List<TKTouch> touches)
	{
		if (base.state == TKGestureRecognizerState.Possible)
		{
			for (int i = 0; i < touches.Count; i++)
			{
				_trackingTouches.Add(touches[i]);
			}
			if (_trackingTouches.Count >= minimumNumberOfTouches && _trackingTouches.Count <= maximumNumberOfTouches)
			{
				_points.Clear();
				_points.Add(touches[0].position);
				_startTime = Time.time;
				base.state = TKGestureRecognizerState.Began;
			}
		}
		return false;
	}

	internal override void touchesMoved(List<TKTouch> touches)
	{
		if (base.state == TKGestureRecognizerState.Began)
		{
			_points.Add(touches[0].position);
			if (triggerWhenCriteriaMet && checkForSwipeCompletion(touches[0]))
			{
				base.state = TKGestureRecognizerState.Recognized;
			}
		}
	}

	internal override void touchesEnded(List<TKTouch> touches)
	{
		if (base.state == TKGestureRecognizerState.Began)
		{
			_points.Add(touches[0].position);
			if (checkForSwipeCompletion(touches[0]))
			{
				base.state = TKGestureRecognizerState.Recognized;
			}
			else
			{
				base.state = TKGestureRecognizerState.FailedOrEnded;
			}
		}
	}

	public override string ToString()
	{
		return $"{base.ToString()}, swipe direction: {completedSwipeDirection}, swipe velocity: {swipeVelocity}, start point: {startPoint}, end point: {endPoint}";
	}
}
