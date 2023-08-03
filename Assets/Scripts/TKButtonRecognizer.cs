using System;
using System.Collections.Generic;
using UnityEngine;

public class TKButtonRecognizer : TKAbstractGestureRecognizer
{
	private TKRect _defaultFrame;

	private TKRect _highlightedFrame;

	public event Action<TKButtonRecognizer> onSelectedEvent;

	public event Action<TKButtonRecognizer> onDeselectedEvent;

	public event Action<TKButtonRecognizer> onTouchUpInsideEvent;

	public TKButtonRecognizer(TKRect defaultFrame)
		: this(defaultFrame, 40f)
	{
	}

	public TKButtonRecognizer(TKRect defaultFrame, float highlightedExpansion)
		: this(defaultFrame, defaultFrame.copyWithExpansion(highlightedExpansion))
	{
	}

	public TKButtonRecognizer(TKRect defaultFrame, TKRect highlightedFrame)
	{
		_defaultFrame = defaultFrame;
		_highlightedFrame = highlightedFrame;
		boundaryFrame = _defaultFrame;
	}

	protected virtual void onSelected()
	{
		boundaryFrame = _highlightedFrame;
		if (this.onSelectedEvent != null)
		{
			this.onSelectedEvent(this);
		}
	}

	protected virtual void onDeselected()
	{
		if (this.onDeselectedEvent != null)
		{
			this.onDeselectedEvent(this);
		}
	}

	protected virtual void onTouchUpInside()
	{
		if (this.onTouchUpInsideEvent != null)
		{
			this.onTouchUpInsideEvent(this);
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
					onSelected();
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
			if (touches[i].phase == TouchPhase.Stationary)
			{
				bool flag = isTouchWithinBoundaryFrame(touches[i]);
				if (base.state == TKGestureRecognizerState.Began && flag)
				{
					base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
					onSelected();
				}
				else if (base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing && !flag)
				{
					base.state = TKGestureRecognizerState.FailedOrEnded;
					onDeselected();
				}
			}
		}
	}

	internal override void touchesEnded(List<TKTouch> touches)
	{
		if (base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing)
		{
			onTouchUpInside();
		}
		boundaryFrame = _defaultFrame;
		base.state = TKGestureRecognizerState.FailedOrEnded;
	}
}
