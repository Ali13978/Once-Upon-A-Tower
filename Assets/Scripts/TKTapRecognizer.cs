using System;
using System.Collections.Generic;
using UnityEngine;

public class TKTapRecognizer : TKAbstractGestureRecognizer
{
	public int numberOfTapsRequired = 1;

	public int numberOfTouchesRequired = 1;

	private float _maxDurationForTapConsideration = 0.5f;

	private float _maxDeltaMovementForTapConsideration = 1f;

	private float _touchBeganTime;

	private int _preformedTapsCount;

	public event Action<TKTapRecognizer> gestureRecognizedEvent;

	public TKTapRecognizer()
		: this(0.5f, 1f)
	{
	}

	public TKTapRecognizer(float maxDurationForTapConsideration, float maxDeltaMovementForTapConsiderationCm)
	{
		_maxDurationForTapConsideration = maxDurationForTapConsideration;
		_maxDeltaMovementForTapConsideration = maxDeltaMovementForTapConsiderationCm;
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
		if (Time.time > _touchBeganTime + _maxDurationForTapConsideration && _preformedTapsCount != 0 && _preformedTapsCount < numberOfTapsRequired)
		{
			base.state = TKGestureRecognizerState.FailedOrEnded;
		}
		if (base.state == TKGestureRecognizerState.Possible)
		{
			for (int i = 0; i < touches.Count; i++)
			{
				if (touches[i].phase == TouchPhase.Began)
				{
					_trackingTouches.Add(touches[i]);
					if (_trackingTouches.Count == numberOfTouchesRequired)
					{
						break;
					}
				}
			}
			if (_trackingTouches.Count == numberOfTouchesRequired)
			{
				_touchBeganTime = Time.time;
				_preformedTapsCount = 0;
				base.state = TKGestureRecognizerState.Began;
				return true;
			}
		}
		return false;
	}

	internal override void touchesMoved(List<TKTouch> touches)
	{
		if (base.state != TKGestureRecognizerState.Began)
		{
			return;
		}
		int num = 0;
		while (true)
		{
			if (num < touches.Count)
			{
				if (Math.Abs(touches[num].position.x - touches[num].startPosition.x) / TouchKit.instance.ScreenPixelsPerCm > _maxDeltaMovementForTapConsideration || Math.Abs(touches[num].position.y - touches[num].startPosition.y) / TouchKit.instance.ScreenPixelsPerCm > _maxDeltaMovementForTapConsideration)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		base.state = TKGestureRecognizerState.FailedOrEnded;
	}

	internal override void touchesEnded(List<TKTouch> touches)
	{
		if (base.state == TKGestureRecognizerState.Began && Time.time <= _touchBeganTime + _maxDurationForTapConsideration)
		{
			_preformedTapsCount++;
			if (_preformedTapsCount == numberOfTapsRequired)
			{
				base.state = TKGestureRecognizerState.Recognized;
			}
		}
		else
		{
			base.state = TKGestureRecognizerState.FailedOrEnded;
		}
	}
}
