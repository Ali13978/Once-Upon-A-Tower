using System;
using System.Collections.Generic;
using UnityEngine;

public class TKPanRecognizer : TKAbstractGestureRecognizer
{
	public Vector2 deltaTranslation;

	public float deltaTranslationCm;

	public int minimumNumberOfTouches = 1;

	public int maximumNumberOfTouches = 2;

	private float totalDeltaMovementInCm;

	private Vector2 _previousLocation;

	private float _minDistanceToPanCm;

	private Vector2 _startPoint;

	private Vector2 _endPoint;

	public Vector2 startPoint => _startPoint;

	public Vector2 endPoint => _endPoint;

	public event Action<TKPanRecognizer> gestureRecognizedEvent;

	public event Action<TKPanRecognizer> gestureCompleteEvent;

	public TKPanRecognizer(float minPanDistanceCm = 0.5f)
	{
		_minDistanceToPanCm = minPanDistanceCm;
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
		if (_trackingTouches.Count + touches.Count > maximumNumberOfTouches)
		{
			base.state = TKGestureRecognizerState.FailedOrEnded;
			return false;
		}
		if (base.state == TKGestureRecognizerState.Possible || ((base.state == TKGestureRecognizerState.Began || base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing) && _trackingTouches.Count < maximumNumberOfTouches))
		{
			for (int i = 0; i < touches.Count; i++)
			{
				if (touches[i].phase == TouchPhase.Began)
				{
					_trackingTouches.Add(touches[i]);
					_startPoint = touches[0].position;
					if (_trackingTouches.Count == maximumNumberOfTouches)
					{
						break;
					}
				}
			}
			if (_trackingTouches.Count >= minimumNumberOfTouches && _trackingTouches.Count <= maximumNumberOfTouches)
			{
				_previousLocation = touchLocation();
				if (base.state != TKGestureRecognizerState.RecognizedAndStillRecognizing)
				{
					totalDeltaMovementInCm = 0f;
					base.state = TKGestureRecognizerState.Began;
				}
			}
		}
		return false;
	}

	internal override void touchesMoved(List<TKTouch> touches)
	{
		if (_trackingTouches.Count < minimumNumberOfTouches || _trackingTouches.Count > maximumNumberOfTouches)
		{
			return;
		}
		Vector2 vector = touchLocation();
		deltaTranslation = vector - _previousLocation;
		deltaTranslationCm = deltaTranslation.magnitude / TouchKit.instance.ScreenPixelsPerCm;
		_previousLocation = vector;
		if (base.state == TKGestureRecognizerState.Began)
		{
			totalDeltaMovementInCm += deltaTranslationCm;
			if (Math.Abs(totalDeltaMovementInCm) >= _minDistanceToPanCm)
			{
				base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
			}
		}
		else
		{
			base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
		}
	}

	internal override void touchesEnded(List<TKTouch> touches)
	{
		_endPoint = touchLocation();
		for (int i = 0; i < touches.Count; i++)
		{
			if (touches[i].phase == TouchPhase.Ended)
			{
				_trackingTouches.Remove(touches[i]);
			}
		}
		if (_trackingTouches.Count >= minimumNumberOfTouches)
		{
			_previousLocation = touchLocation();
			base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
			return;
		}
		if (base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing && this.gestureCompleteEvent != null)
		{
			this.gestureCompleteEvent(this);
		}
		base.state = TKGestureRecognizerState.FailedOrEnded;
	}

	public override string ToString()
	{
		return $"[{GetType()}] state: {base.state}, location: {touchLocation()}, deltaTranslation: {deltaTranslation}";
	}
}
