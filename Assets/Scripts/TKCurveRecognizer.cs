using System;
using System.Collections.Generic;
using UnityEngine;

public class TKCurveRecognizer : TKAbstractGestureRecognizer
{
	public float reportRotationStep = 20f;

	public float squareDistance = 10f;

	public float maxSharpnes = 50f;

	public int minimumNumberOfTouches = 1;

	public int maximumNumberOfTouches = 2;

	public float deltaRotation;

	private Vector2 _previousLocation;

	private Vector2 _deltaTranslation;

	private Vector2 _previousDeltaTranslation;

	public event Action<TKCurveRecognizer> gestureRecognizedEvent;

	public event Action<TKCurveRecognizer> gestureCompleteEvent;

	internal override void fireRecognizedEvent()
	{
		if (this.gestureRecognizedEvent != null)
		{
			this.gestureRecognizedEvent(this);
		}
	}

	internal override bool touchesBegan(List<TKTouch> touches)
	{
		if (base.state == TKGestureRecognizerState.Possible || ((base.state == TKGestureRecognizerState.Began || base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing) && _trackingTouches.Count < maximumNumberOfTouches))
		{
			for (int i = 0; i < touches.Count; i++)
			{
				if (touches[i].phase == TouchPhase.Began)
				{
					_trackingTouches.Add(touches[i]);
					if (_trackingTouches.Count == maximumNumberOfTouches)
					{
						break;
					}
				}
			}
			if (_trackingTouches.Count >= minimumNumberOfTouches)
			{
				_previousLocation = touchLocation();
				if (base.state != TKGestureRecognizerState.RecognizedAndStillRecognizing)
				{
					base.state = TKGestureRecognizerState.Possible;
					deltaRotation = 0f;
					_deltaTranslation = Vector2.zero;
					_previousDeltaTranslation = Vector2.zero;
				}
			}
		}
		return false;
	}

	internal override void touchesMoved(List<TKTouch> touches)
	{
		if (base.state == TKGestureRecognizerState.Possible)
		{
			Vector2 vector = touchLocation();
			Vector2 vector2 = _deltaTranslation = vector - _previousLocation;
			_previousLocation = vector;
			_previousDeltaTranslation = _deltaTranslation;
			base.state = TKGestureRecognizerState.Began;
		}
		else
		{
			if (base.state != TKGestureRecognizerState.RecognizedAndStillRecognizing && base.state != TKGestureRecognizerState.Began)
			{
				return;
			}
			Vector2 vector3 = touchLocation();
			Vector2 vector4 = vector3 - _previousLocation;
			if (!(vector4.sqrMagnitude >= 10f))
			{
				return;
			}
			float num = Vector2.Angle(_previousDeltaTranslation, vector4);
			if (num > maxSharpnes)
			{
				UnityEngine.Debug.Log("Curve is to sharp: " + num + "  max sharpnes set to:" + maxSharpnes);
				base.state = TKGestureRecognizerState.FailedOrEnded;
				return;
			}
			_deltaTranslation = vector4;
			Vector3 vector5 = Vector3.Cross(_previousDeltaTranslation, vector4);
			float z = vector5.z;
			if (z > 0f)
			{
				deltaRotation -= num;
			}
			else
			{
				deltaRotation += num;
			}
			if (Mathf.Abs(deltaRotation) >= reportRotationStep)
			{
				base.state = TKGestureRecognizerState.RecognizedAndStillRecognizing;
				deltaRotation = 0f;
			}
			_previousLocation = vector3;
			_previousDeltaTranslation = _deltaTranslation;
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
		return $"[{GetType()}] state: {base.state}, trans: {_deltaTranslation}, lastTrans: {_previousDeltaTranslation}, totalRot: {deltaRotation}";
	}
}
