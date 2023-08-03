using System;
using System.Collections;
using UnityEngine;

public class GuiScroll : GuiView
{
	public Transform Content;

	private Vector3 offset;

	public Vector3 TargetOffset;

	public Vector3 ContentSize;

	public float MaxOffset = 50f;

	public float BounceTime = 0.6f;

	public float Deacceleration = 50f;

	public float DragDelay = 0.07f;

	public float OverScrollAccelMultiplier = 2.5f;

	public float InertiaStopSpeed = 0.01f;

	public bool Horizontal = true;

	public bool Vertical = true;

	private bool started;

	protected Vector3 ContentOrigin;

	private Vector3 OffsetVelocity;

	private BoxCollider2D boxCollider;

	private Vector3 previousPosition;

	private int touchIndex;

	private float touchBegan;

	private VelocityTracker velocityTracker = new VelocityTracker();

	private Vector3 vel;

	private IEnumerator inertiaCoroutine;

	private const float margin = 2f;

	public Vector3 Offset
	{
		get
		{
			return offset;
		}
		set
		{
			TargetOffset = (offset = value);
		}
	}

	public Bounds Window
	{
		get
		{
			if (boxCollider == null)
			{
				boxCollider = GetComponent<BoxCollider2D>();
			}
			Vector3 localPosition = base.transform.localPosition;
			Vector2 vector = boxCollider.offset;
			float x = vector.x;
			Vector2 vector2 = boxCollider.offset;
			return new Bounds(localPosition + new Vector3(x, vector2.y, 0f), boxCollider.size);
		}
	}

	private float ZScale => SingletonMonoBehaviour<Gui>.Instance.GuiCamera.orthographicSize * 2f / (float)SingletonMonoBehaviour<Gui>.Instance.GuiCamera.pixelHeight;

	private float TrackerScale
	{
		get
		{
			float zScale = ZScale;
			Vector3 lossyScale = base.transform.lossyScale;
			return zScale / lossyScale.x * 1.1f;
		}
	}

	private Vector3 ClampedOffset => ClampOffset(Offset);

	protected override void Start()
	{
		base.Start();
		if (Content == null)
		{
			Content = base.transform.GetChild(0);
		}
		ResetContentOrigin();
		started = true;
		UpdateContentPosition();
	}

	public void ResetContentOrigin()
	{
		ContentOrigin = Content.localPosition;
		Offset = Vector3.zero;
	}

	public override bool OnTouchBegan(int index, Vector3 position)
	{
		return OnTouchBeganInternal(index, position, grab: true);
	}

	public bool OnTouchBeganInternal(int index, Vector3 position, bool grab)
	{
		previousPosition = position;
		touchIndex = index;
		touchBegan = Time.time;
		if (grab)
		{
			SingletonMonoBehaviour<Gui>.Instance.GrabTouch(this, index);
		}
		velocityTracker.Reset();
		velocityTracker.AddPoint(position * TrackerScale, Time.time);
		return true;
	}

	public override bool OnTouchEnded(int index, Vector3 position, bool inside)
	{
		vel = velocityTracker.ComputeCurrentVelocity(1);
		StartCoroutine(inertiaCoroutine = Inertia());
		return true;
	}

	public void OnTouchMove()
	{
		Vector3 touchPosition = SingletonMonoBehaviour<Gui>.Instance.GetTouchPosition(touchIndex);
		velocityTracker.AddPoint(touchPosition * TrackerScale, Time.time);
		if (Time.time - touchBegan > DragDelay || inertiaCoroutine == null)
		{
			if (inertiaCoroutine != null)
			{
				StopCoroutine(inertiaCoroutine);
				inertiaCoroutine = null;
			}
			Vector3 a = (touchPosition - previousPosition) * ZScale;
			Vector3 lossyScale = base.transform.lossyScale;
			Vector3 diff = a / lossyScale.x;
			if (!Horizontal)
			{
				diff.x = 0f;
			}
			if (!Vertical)
			{
				diff.y = 0f;
			}
			diff.z = 0f;
			Offset = Elastic(Offset, ClampedOffset, diff);
			UpdateContentPosition();
		}
		previousPosition = touchPosition;
	}

	protected override void Update()
	{
		base.Update();
		if (SingletonMonoBehaviour<Gui>.Instance.IsGrabbing(this) && SingletonMonoBehaviour<Gui>.Instance.Touchable)
		{
			OnTouchMove();
		}
		if (TargetOffset != Offset)
		{
			if (Vector3.Distance(TargetOffset, Offset) < 0.05f)
			{
				offset = TargetOffset;
			}
			else
			{
				offset = Vector3.SmoothDamp(Offset, TargetOffset, ref OffsetVelocity, 0.2f);
			}
			UpdateContentPosition();
		}
	}

	public void UpdateContentPosition()
	{
		if (started)
		{
			Content.localPosition = ContentOrigin + Offset;
		}
	}

	public void Stop()
	{
		if (inertiaCoroutine != null)
		{
			StopCoroutine(inertiaCoroutine);
			inertiaCoroutine = null;
		}
	}

	private static float Elastic(float value, float target, float max, float diff)
	{
		if (value == target || Mathf.Sign(diff) != Mathf.Sign(value - target))
		{
			return value + diff;
		}
		float num = Mathf.Min(Mathf.Abs(value - target) / max, 1f);
		float num2 = Mathf.Max((Mathf.Cos(num * (float)Math.PI) + 1f) / 2f, 0f);
		return value + diff * num2;
	}

	private Vector3 Elastic(Vector3 value, Vector3 target, Vector3 diff)
	{
		return new Vector3(Elastic(value.x, target.x, MaxOffset, diff.x), Elastic(value.y, target.y, MaxOffset, diff.y), value.z);
	}

	public Vector3 ClampOffset(Vector3 target)
	{
		if (Horizontal)
		{
			float x = target.x;
			float x2 = ContentSize.x;
			Vector3 size = Window.size;
			target.x = Mathf.Clamp(x, 0f - Mathf.Max(0f, x2 - size.x), 0f);
		}
		if (Vertical)
		{
			float y = target.y;
			float y2 = ContentSize.y;
			Vector3 size2 = Window.size;
			target.y = Mathf.Clamp(y, 0f, Mathf.Max(0f, y2 - size2.y));
		}
		return target;
	}

	private IEnumerator Inertia()
	{
		IEnumerator xcoroutine = null;
		IEnumerator ycoroutine = null;
		if (Horizontal)
		{
			xcoroutine = AxisInertia((Vector3 v) => v.x, delegate(Vector3 v, float x)
			{
				v.x = x;
				return v;
			});
		}
		if (Vertical)
		{
			ycoroutine = AxisInertia((Vector3 v) => v.y, delegate(Vector3 v, float y)
			{
				v.y = y;
				return v;
			});
		}
		bool xNext = Horizontal;
		bool yNext = Vertical;
		while (true)
		{
			if (xNext)
			{
				xNext = xcoroutine.MoveNext();
			}
			if (yNext)
			{
				yNext = ycoroutine.MoveNext();
			}
			if (!xNext && !yNext)
			{
				break;
			}
			yield return null;
		}
	}

	private IEnumerator AxisInertia(Func<Vector3, float> gett, Func<Vector3, float, Vector3> sett)
	{
		float sign = Mathf.Sign(gett(vel));
		if (gett(Offset) == gett(ClampedOffset) || gett(vel) * sign > InertiaStopSpeed)
		{
			float accel = (0f - sign) * Deacceleration;
			while (gett(vel) * sign > InertiaStopSpeed)
			{
				if (gett(Offset) != gett(ClampedOffset))
				{
					accel *= OverScrollAccelMultiplier;
				}
				if (gett(vel) * sign < 0f)
				{
					vel = sett(vel, 0f);
				}
				Offset = sett(Offset, gett(Offset) + gett(vel) * Time.fixedDeltaTime);
				UpdateContentPosition();
				vel = sett(vel, gett(vel) + accel * Time.deltaTime);
				yield return null;
			}
			vel = sett(vel, 0f);
		}
		if (gett(Offset) != gett(ClampedOffset))
		{
			vel = sett(vel, 0f);
			float startTime = Time.time;
			float startPos = gett(Offset);
			float endPos = gett(ClampedOffset);
			while (Time.time - startTime < BounceTime)
			{
				float a = Mathf.Min(1f, (Time.time - startTime) / BounceTime);
				Offset = sett(arg2: Mathf.Lerp(startPos, endPos, Tween.CircEaseInOut(a, 0f, 1f, 1f)), arg1: Offset);
				UpdateContentPosition();
				yield return null;
			}
			Offset = sett(Offset, gett(ClampedOffset));
			UpdateContentPosition();
		}
	}

	public void ScrollToView(GuiView view)
	{
		Bounds colliderBounds = view.ColliderBounds;
		Bounds colliderBounds2 = base.ColliderBounds;
		Vector3 zero = Vector3.zero;
		Vector3 max = colliderBounds.max;
		float y = max.y;
		Vector3 max2 = colliderBounds2.max;
		if (y > max2.y)
		{
			Vector3 max3 = colliderBounds2.max;
			float y2 = max3.y;
			Vector3 max4 = colliderBounds.max;
			zero.y = y2 - max4.y - 2f;
		}
		else
		{
			Vector3 min = colliderBounds2.min;
			float y3 = min.y;
			Vector3 min2 = colliderBounds.min;
			if (y3 > min2.y)
			{
				Vector3 min3 = colliderBounds2.min;
				float y4 = min3.y;
				Vector3 min4 = colliderBounds.min;
				zero.y = y4 - min4.y + 2f;
			}
		}
		float y5 = zero.y;
		Vector3 lossyScale = base.transform.lossyScale;
		zero.y = y5 / lossyScale.y;
		TargetOffset = Offset + zero;
	}
}
