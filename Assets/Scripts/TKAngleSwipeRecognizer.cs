using System;
using System.Collections.Generic;
using UnityEngine;

public class TKAngleSwipeRecognizer : TKAbstractGestureRecognizer
{
	private struct AngleListener
	{
		public float Varience;

		public Vector2 Direction;

		public Action<TKAngleSwipeRecognizer> Action;

		public AngleListener(Vector2 direction, float varience, Action<TKAngleSwipeRecognizer> action)
		{
			this = default(AngleListener);
			Varience = varience;
			Direction = direction;
			Action = action;
		}
	}

	private List<AngleListener> _angleRecognizedEvents = new List<AngleListener>();

	public float timeToSwipe = 0.5f;

	public float minimumDistance = 2f;

	private Vector2 _startPoint;

	private Vector2 _endPoint;

	private float _startTime;

	public float swipeVelocity
	{
		get;
		private set;
	}

	public float swipeAngle
	{
		get;
		private set;
	}

	public Vector2 swipeVelVector
	{
		get;
		private set;
	}

	public Vector2 startPoint => _startPoint;

	public Vector2 endPoint => _endPoint;

	public event Action<TKAngleSwipeRecognizer> gestureRecognizedEvent;

	public TKAngleSwipeRecognizer()
		: this(2f)
	{
	}

	public TKAngleSwipeRecognizer(float minimumDistanceCm)
	{
		minimumDistance = minimumDistanceCm;
	}

	public void addAngleRecogizedEvents(Action<TKAngleSwipeRecognizer> action, Vector2 direction, float angleVarience)
	{
		_angleRecognizedEvents.Add(new AngleListener(direction, angleVarience, action));
	}

	public void removeAngleRecognizedEvents(Action<TKAngleSwipeRecognizer> action)
	{
		_angleRecognizedEvents.RemoveAll((AngleListener listener) => listener.Action == action);
	}

	public void removeAllAngleRecongnizedEvents()
	{
		_angleRecognizedEvents.Clear();
	}

	public void fireAngleRecognizedEvents()
	{
		int i = 0;
		for (int count = _angleRecognizedEvents.Count; i < count; i++)
		{
			AngleListener angleListener = _angleRecognizedEvents[i];
			if (angleListener.Varience > Vector2.Angle(angleListener.Direction, swipeVelVector))
			{
				angleListener.Action(this);
			}
		}
	}

	private bool checkForSwipeCompletion(TKTouch touch)
	{
		if (timeToSwipe > 0f && Time.time - _startTime > timeToSwipe)
		{
			base.state = TKGestureRecognizerState.FailedOrEnded;
			return false;
		}
		float num = Mathf.Abs(_startPoint.x - touch.position.x) / TouchKit.instance.ScreenPixelsPerCm;
		float num2 = Mathf.Abs(_startPoint.y - touch.position.y) / TouchKit.instance.ScreenPixelsPerCm;
		_endPoint = touch.position;
		swipeVelocity = Mathf.Sqrt(num * num + num2 * num2);
		Vector2 vector = endPoint - startPoint;
		swipeAngle = 57.29578f * Mathf.Atan2(vector.y, vector.x);
		if (swipeAngle < 0f)
		{
			swipeAngle += 360f;
		}
		swipeVelVector = _endPoint - _startPoint;
		if (swipeVelocity > minimumDistance)
		{
			return true;
		}
		return false;
	}

	internal override void fireRecognizedEvent()
	{
		if (this.gestureRecognizedEvent != null)
		{
			this.gestureRecognizedEvent(this);
		}
		fireAngleRecognizedEvents();
	}

	internal override bool touchesBegan(List<TKTouch> touches)
	{
		if (base.state == TKGestureRecognizerState.Possible)
		{
			_startPoint = touches[0].position;
			_startTime = Time.time;
			_trackingTouches.Add(touches[0]);
			base.state = TKGestureRecognizerState.Began;
		}
		return false;
	}

	internal override void touchesMoved(List<TKTouch> touches)
	{
		if (base.state == TKGestureRecognizerState.Began && checkForSwipeCompletion(touches[0]))
		{
			base.state = TKGestureRecognizerState.Recognized;
		}
	}

	internal override void touchesEnded(List<TKTouch> touches)
	{
		base.state = TKGestureRecognizerState.FailedOrEnded;
	}

	public override string ToString()
	{
		return $"{base.ToString()}, velocity: {swipeVelocity}, angle: {swipeAngle}, start point: {startPoint}, end point: {endPoint}";
	}
}
