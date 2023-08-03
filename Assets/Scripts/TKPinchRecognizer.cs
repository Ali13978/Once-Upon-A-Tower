using System;
using System.Collections.Generic;
using UnityEngine;

public class TKPinchRecognizer : TKAbstractGestureRecognizer
{
	public float minimumScaleDistanceToRecognize;

	public float deltaScale;

	private float _intialDistance;

	private float _firstDistance;

	private float _previousDistance;

	public float accumulatedScale
	{
		get
		{
			float num = distanceBetweenTrackedTouches();
			return num / _intialDistance;
		}
	}

	public event Action<TKPinchRecognizer> gestureRecognizedEvent;

	public event Action<TKPinchRecognizer> gestureCompleteEvent;

	private float distanceBetweenTrackedTouches()
	{
		float b = Vector2.Distance(_trackingTouches[0].position, _trackingTouches[1].position);
		return Mathf.Max(0.0001f, b) / TouchKit.instance.ScreenPixelsPerCm;
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
				if (touches[i].phase == TouchPhase.Began)
				{
					_trackingTouches.Add(touches[i]);
					if (_trackingTouches.Count == 2)
					{
						break;
					}
				}
			}
			if (_trackingTouches.Count == 2)
			{
				_firstDistance = distanceBetweenTrackedTouches();
			}
		}
		return false;
	}

	internal override void touchesMoved(List<TKTouch> touches)
	{
		if (_trackingTouches.Count != 2)
		{
			return;
		}
		if (base.state == TKGestureRecognizerState.Possible)
		{
			if (Mathf.Abs(distanceBetweenTrackedTouches() - _firstDistance) >= minimumScaleDistanceToRecognize)
			{
				deltaScale = 0f;
				_intialDistance = distanceBetweenTrackedTouches();
				_previousDistance = _intialDistance;
				base.state = TKGestureRecognizerState.Began;
			}
		}
		else if (base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing || base.state == TKGestureRecognizerState.Began)
		{
			float num = distanceBetweenTrackedTouches();
			deltaScale = (num - _previousDistance) / _intialDistance;
			_previousDistance = num;
			base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
		}
	}

	internal override void touchesEnded(List<TKTouch> touches)
	{
		for (int i = 0; i < touches.Count; i++)
		{
			if (touches[i].phase == TouchPhase.Ended)
			{
				_trackingTouches.Remove(touches[i]);
			}
		}
		if (base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing && this.gestureCompleteEvent != null)
		{
			this.gestureCompleteEvent(this);
		}
		if (_trackingTouches.Count == 1)
		{
			base.state = TKGestureRecognizerState.Possible;
			deltaScale = 0f;
		}
		else
		{
			base.state = TKGestureRecognizerState.FailedOrEnded;
		}
	}

	public override string ToString()
	{
		return $"[{GetType()}] state: {base.state}, deltaScale: {deltaScale}";
	}
}
