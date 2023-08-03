using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TKLongPressRecognizer : TKAbstractGestureRecognizer
{
	public float minimumPressDuration = 0.5f;

	public int requiredTouchesCount = -1;

	public float allowableMovementCm = 1f;

	private Vector2 _beginLocation;

	private bool _waiting;

	public event Action<TKLongPressRecognizer> gestureRecognizedEvent;

	public event Action<TKLongPressRecognizer> gestureCompleteEvent;

	public TKLongPressRecognizer()
	{
	}

	public TKLongPressRecognizer(float minimumPressDuration, float allowableMovement, int requiredTouchesCount)
	{
		this.minimumPressDuration = minimumPressDuration;
		allowableMovementCm = allowableMovement;
		this.requiredTouchesCount = requiredTouchesCount;
	}

	private IEnumerator beginGesture()
	{
		float endTime = Time.time + minimumPressDuration;
		while (_waiting && Time.time < endTime)
		{
			yield return null;
		}
		if (Time.time >= endTime && base.state == TKGestureRecognizerState.Began)
		{
			base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
		}
		_waiting = false;
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
		if (!_waiting && base.state == TKGestureRecognizerState.Possible && (requiredTouchesCount == -1 || touches.Count == requiredTouchesCount))
		{
			_beginLocation = touches[0].position;
			_waiting = true;
			TouchKit.instance.StartCoroutine(beginGesture());
			_trackingTouches.Add(touches[0]);
			base.state = TKGestureRecognizerState.Began;
		}
		else if (requiredTouchesCount != -1)
		{
			_waiting = false;
		}
		return false;
	}

	internal override void touchesMoved(List<TKTouch> touches)
	{
		if (base.state != TKGestureRecognizerState.Began && base.state != TKGestureRecognizerState.RecognizedAndStillRecognizing)
		{
			return;
		}
		float num = Vector2.Distance(touches[0].position, _beginLocation) / TouchKit.instance.ScreenPixelsPerCm;
		if (num > allowableMovementCm)
		{
			if (base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing && this.gestureCompleteEvent != null)
			{
				this.gestureCompleteEvent(this);
			}
			base.state = TKGestureRecognizerState.FailedOrEnded;
			_waiting = false;
		}
	}

	internal override void touchesEnded(List<TKTouch> touches)
	{
		if (base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing && this.gestureCompleteEvent != null)
		{
			this.gestureCompleteEvent(this);
		}
		base.state = TKGestureRecognizerState.FailedOrEnded;
		_waiting = false;
	}
}
