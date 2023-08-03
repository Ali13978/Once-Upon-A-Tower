using System;
using System.Collections.Generic;
using UnityEngine;

public class TKAnyTouchRecognizer : TKAbstractGestureRecognizer
{
	public event Action<TKAnyTouchRecognizer> onEnteredEvent;

	public event Action<TKAnyTouchRecognizer> onExitedEvent;

	public TKAnyTouchRecognizer(TKRect frame)
	{
		alwaysSendTouchesMoved = true;
		boundaryFrame = frame;
	}

	private void onTouchEntered()
	{
		if (_trackingTouches.Count == 1 && this.onEnteredEvent != null)
		{
			this.onEnteredEvent(this);
		}
	}

	private void onTouchExited()
	{
		if (_trackingTouches.Count == 0 && this.onExitedEvent != null)
		{
			this.onExitedEvent(this);
		}
	}

	internal override void fireRecognizedEvent()
	{
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
					base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
					onTouchEntered();
					return true;
				}
			}
		}
		return false;
	}

	internal override void touchesMoved(List<TKTouch> touches)
	{
		for (int i = 0; i < touches.Count; i++)
		{
			bool flag = isTouchWithinBoundaryFrame(touches[i]);
			bool flag2 = _trackingTouches.Contains(touches[i]);
			if (!flag2 || !flag)
			{
				if (!flag2 && flag)
				{
					_trackingTouches.Add(touches[i]);
					base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
					onTouchEntered();
				}
				else if (flag2 && !flag)
				{
					_trackingTouches.Remove(touches[i]);
					base.state = TKGestureRecognizerState.FailedOrEnded;
					onTouchExited();
				}
			}
		}
	}

	internal override void touchesEnded(List<TKTouch> touches)
	{
		for (int i = 0; i < touches.Count; i++)
		{
			if (touches[i].phase == TouchPhase.Ended && _trackingTouches.Contains(touches[i]))
			{
				_trackingTouches.Remove(touches[i]);
				base.state = TKGestureRecognizerState.FailedOrEnded;
				onTouchExited();
			}
		}
	}
}
