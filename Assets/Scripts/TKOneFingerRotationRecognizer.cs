using System;
using System.Collections.Generic;
using UnityEngine;

public class TKOneFingerRotationRecognizer : TKRotationRecognizer
{
	public Vector2 targetPosition;

	public new event Action<TKOneFingerRotationRecognizer> gestureRecognizedEvent;

	public new event Action<TKOneFingerRotationRecognizer> gestureCompleteEvent;

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
			_trackingTouches.Add(touches[0]);
			deltaRotation = 0f;
			_previousRotation = TKRotationRecognizer.angleBetweenPoints(targetPosition, _trackingTouches[0].position);
			base.state = TKGestureRecognizerState.Began;
		}
		return false;
	}

	internal override void touchesMoved(List<TKTouch> touches)
	{
		if (base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing || base.state == TKGestureRecognizerState.Began)
		{
			float num = TKRotationRecognizer.angleBetweenPoints(targetPosition, _trackingTouches[0].position);
			deltaRotation = Mathf.DeltaAngle(num, _previousRotation);
			_previousRotation = num;
			base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
		}
	}

	internal override void touchesEnded(List<TKTouch> touches)
	{
		if (base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing && this.gestureCompleteEvent != null)
		{
			this.gestureCompleteEvent(this);
		}
		base.state = TKGestureRecognizerState.FailedOrEnded;
	}
}
