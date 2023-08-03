using System;
using System.Collections.Generic;
using UnityEngine;

public class TKTouchPadRecognizer : TKAbstractGestureRecognizer
{
	public AnimationCurve inputCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	public Vector2 value;

	public event Action<TKTouchPadRecognizer> gestureRecognizedEvent;

	public event Action<TKTouchPadRecognizer> gestureCompleteEvent;

	public TKTouchPadRecognizer(TKRect frame)
	{
		boundaryFrame = frame;
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
				}
			}
			if (_trackingTouches.Count > 0)
			{
				base.state = TKGestureRecognizerState.Began;
				touchesMoved(touches);
			}
		}
		return false;
	}

	internal override void touchesMoved(List<TKTouch> touches)
	{
		if (base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing || base.state == TKGestureRecognizerState.Began)
		{
			Vector2 a = touchLocation();
			value = a - boundaryFrame.Value.center;
			ref Vector2 reference = ref value;
			float x = value.x;
			TKRect tKRect = boundaryFrame.Value;
			reference.x = Mathf.Clamp(x / (tKRect.width * 0.5f), -1f, 1f);
			ref Vector2 reference2 = ref value;
			float y = value.y;
			TKRect tKRect2 = boundaryFrame.Value;
			reference2.y = Mathf.Clamp(y / (tKRect2.height * 0.5f), -1f, 1f);
			value.x = inputCurve.Evaluate(Mathf.Abs(value.x)) * Mathf.Sign(value.x);
			value.y = inputCurve.Evaluate(Mathf.Abs(value.y)) * Mathf.Sign(value.y);
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
		value = Vector2.zero;
		base.state = TKGestureRecognizerState.FailedOrEnded;
	}

	public override string ToString()
	{
		return $"[{GetType()}] state: {base.state}, value: {value}";
	}
}
