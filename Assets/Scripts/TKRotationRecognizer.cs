using System;
using System.Collections.Generic;
using UnityEngine;

public class TKRotationRecognizer : TKAbstractGestureRecognizer
{
	public float deltaRotation;

	public float minimumRotationToRecognize;

	protected float _previousRotation;

	protected float _firstRotation;

	protected float _initialRotation;

	public float accumulatedRotation
	{
		get
		{
			if (_trackingTouches.Count == 2)
			{
				return Mathf.DeltaAngle(angleBetweenPoints(_trackingTouches[0].position, _trackingTouches[1].position), _initialRotation);
			}
			return 0f;
		}
	}

	public event Action<TKRotationRecognizer> gestureRecognizedEvent;

	public event Action<TKRotationRecognizer> gestureCompleteEvent;

	public static float angleBetweenPoints(Vector2 position1, Vector2 position2)
	{
		Vector2 vector = position2 - position1;
		Vector2 vector2 = new Vector2(1f, 0f);
		float num = Vector2.Angle(vector, vector2);
		Vector3 vector3 = Vector3.Cross(vector, vector2);
		if (vector3.z > 0f)
		{
			num = 360f - num;
		}
		return num;
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
				if (minimumRotationToRecognize == 0f)
				{
					deltaRotation = 0f;
					_previousRotation = angleBetweenPoints(_trackingTouches[0].position, _trackingTouches[1].position);
					base.state = TKGestureRecognizerState.Began;
				}
				else
				{
					_firstRotation = angleBetweenPoints(_trackingTouches[0].position, _trackingTouches[1].position);
				}
			}
		}
		return false;
	}

	internal override void touchesMoved(List<TKTouch> touches)
	{
		if (base.state == TKGestureRecognizerState.Possible && _trackingTouches.Count == 2)
		{
			float num = angleBetweenPoints(_trackingTouches[0].position, _trackingTouches[1].position);
			float num2 = Mathf.Abs(Mathf.DeltaAngle(num, _firstRotation));
			if (num2 > minimumRotationToRecognize)
			{
				_initialRotation = num;
				deltaRotation = 0f;
				_previousRotation = angleBetweenPoints(_trackingTouches[0].position, _trackingTouches[1].position);
				base.state = TKGestureRecognizerState.Began;
			}
		}
		if (base.state == TKGestureRecognizerState.RecognizedAndStillRecognizing || base.state == TKGestureRecognizerState.Began)
		{
			float num3 = angleBetweenPoints(_trackingTouches[0].position, _trackingTouches[1].position);
			deltaRotation = Mathf.DeltaAngle(num3, _previousRotation);
			_previousRotation = num3;
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
			deltaRotation = 0f;
		}
		else
		{
			base.state = TKGestureRecognizerState.FailedOrEnded;
			_initialRotation = 0f;
		}
	}

	public override string ToString()
	{
		return $"[{GetType()}] state: {base.state}, location: {touchLocation()}, rotation: {deltaRotation}";
	}
}
