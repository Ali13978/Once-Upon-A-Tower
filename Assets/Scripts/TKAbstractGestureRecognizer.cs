using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TKAbstractGestureRecognizer : IComparable<TKAbstractGestureRecognizer>
{
	public bool enabled = true;

	public TKRect? boundaryFrame;

	public uint zIndex;

	private TKGestureRecognizerState _state;

	protected bool alwaysSendTouchesMoved;

	protected List<TKTouch> _trackingTouches = new List<TKTouch>();

	private List<TKTouch> _subsetOfTouchesBeingTrackedApplicableToCurrentRecognizer = new List<TKTouch>();

	private bool _sentTouchesBegan;

	private bool _sentTouchesMoved;

	private bool _sentTouchesEnded;

	public TKGestureRecognizerState state
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
			if (_state == TKGestureRecognizerState.Recognized || _state == TKGestureRecognizerState.RecognizedAndStillRecognizing)
			{
				fireRecognizedEvent();
			}
			if (_state == TKGestureRecognizerState.Recognized || _state == TKGestureRecognizerState.FailedOrEnded)
			{
				reset();
			}
		}
	}

	private bool shouldAttemptToRecognize => enabled && state != TKGestureRecognizerState.FailedOrEnded && state != TKGestureRecognizerState.Recognized;

	protected bool isTrackingTouch(TKTouch t)
	{
		return _trackingTouches.Contains(t);
	}

	protected bool isTrackingAnyTouch(List<TKTouch> touches)
	{
		for (int i = 0; i < touches.Count; i++)
		{
			if (_trackingTouches.Contains(touches[i]))
			{
				return true;
			}
		}
		return false;
	}

	private bool populateSubsetOfTouchesBeingTracked(List<TKTouch> touches)
	{
		_subsetOfTouchesBeingTrackedApplicableToCurrentRecognizer.Clear();
		for (int i = 0; i < touches.Count; i++)
		{
			if (alwaysSendTouchesMoved || isTrackingTouch(touches[i]))
			{
				_subsetOfTouchesBeingTrackedApplicableToCurrentRecognizer.Add(touches[i]);
			}
		}
		return _subsetOfTouchesBeingTrackedApplicableToCurrentRecognizer.Count > 0;
	}

	internal void recognizeTouches(List<TKTouch> touches)
	{
		if (!shouldAttemptToRecognize)
		{
			return;
		}
		_sentTouchesBegan = (_sentTouchesMoved = (_sentTouchesEnded = false));
		for (int num = touches.Count - 1; num >= 0; num--)
		{
			TKTouch tKTouch = touches[num];
			switch (tKTouch.phase)
			{
			case TouchPhase.Began:
				if (_sentTouchesBegan || !isTouchWithinBoundaryFrame(touches[num]))
				{
					break;
				}
				if (touchesBegan(touches) && zIndex != 0)
				{
					int num2 = 0;
					for (int num3 = touches.Count - 1; num3 >= 0; num3--)
					{
						if (touches[num3].phase == TouchPhase.Began)
						{
							touches.RemoveAt(num3);
							num2++;
						}
					}
					if (num2 > 0)
					{
						num -= num2 - 1;
					}
				}
				_sentTouchesBegan = true;
				break;
			case TouchPhase.Moved:
				if (!_sentTouchesMoved && populateSubsetOfTouchesBeingTracked(touches))
				{
					touchesMoved(_subsetOfTouchesBeingTrackedApplicableToCurrentRecognizer);
					_sentTouchesMoved = true;
				}
				break;
			case TouchPhase.Ended:
			case TouchPhase.Canceled:
				if (!_sentTouchesEnded && populateSubsetOfTouchesBeingTracked(touches))
				{
					touchesEnded(_subsetOfTouchesBeingTrackedApplicableToCurrentRecognizer);
					_sentTouchesEnded = true;
				}
				break;
			}
		}
	}

	internal void reset()
	{
		_state = TKGestureRecognizerState.Possible;
		_trackingTouches.Clear();
	}

	internal bool isTouchWithinBoundaryFrame(TKTouch touch)
	{
		return !boundaryFrame.HasValue || boundaryFrame.Value.contains(touch.position);
	}

	public Vector2 touchLocation()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		for (int i = 0; i < _trackingTouches.Count; i++)
		{
			num += _trackingTouches[i].position.x;
			num2 += _trackingTouches[i].position.y;
			num3 += 1f;
		}
		if (num3 > 0f)
		{
			return new Vector2(num / num3, num2 / num3);
		}
		return Vector2.zero;
	}

	public Vector2 startTouchLocation()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		for (int i = 0; i < _trackingTouches.Count; i++)
		{
			num += _trackingTouches[i].startPosition.x;
			num2 += _trackingTouches[i].startPosition.y;
			num3 += 1f;
		}
		if (num3 > 0f)
		{
			return new Vector2(num / num3, num2 / num3);
		}
		return Vector2.zero;
	}

	internal virtual bool touchesBegan(List<TKTouch> touches)
	{
		return false;
	}

	internal virtual void touchesMoved(List<TKTouch> touches)
	{
	}

	internal virtual void touchesEnded(List<TKTouch> touches)
	{
	}

	internal abstract void fireRecognizedEvent();

	public int CompareTo(TKAbstractGestureRecognizer other)
	{
		return zIndex.CompareTo(other.zIndex);
	}

	public override string ToString()
	{
		return $"[{GetType()}] state: {state}, location: {touchLocation()}, zIndex: {zIndex}";
	}
}
