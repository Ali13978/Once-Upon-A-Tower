using Flux;
using System;
using System.Collections;
using UnityEngine;

public class GuiButton : GuiView
{
	public Action Click;

	public Action Down;

	public Action Up;

	public bool ResetOnClick = true;

	public bool Focusable = true;

	public bool TVInertia;

	private float PressedScale = 1.1f;

	public Renderer HighlightRenderer;

	public Texture2D HighlightRamp;

	private MaterialPropertyBlock highlightPropertyBlock;

	private bool pressed;

	private float focusStartTime;

	private bool toggleActive;

	private Vector3 focusRotation = Vector3.zero;

	private IEnumerator focusRotationCoroutine;

	public AudioSource Sound;

	protected int touchIndex;

	private Vector3 touchStart;

	private Transform content;

	[NonSerialized]
	public GuiScroll Scroll;

	public bool Active
	{
		get
		{
			return toggleActive;
		}
		set
		{
			toggleActive = value;
			Reset();
		}
	}

	public Vector3 FocusRotation
	{
		get
		{
			return focusRotation;
		}
		set
		{
			if (!pressed)
			{
				if (focusRotationCoroutine != null)
				{
					StopCoroutine(focusRotationCoroutine);
					focusRotationCoroutine = null;
				}
				UpdateFocusRotation(value);
			}
		}
	}

	private void UpdateFocusRotation(Vector3 value)
	{
		CreateContentTransform();
		content.localRotation = Quaternion.Euler(new Vector3(value.x, 0f - value.y) * 5f);
		content.localPosition = content.InverseTransformVector(new Vector3(0f - value.x, value.y) * 0.06f);
		focusRotation = value;
	}

	protected override void Awake()
	{
		base.Awake();
		if ((bool)GetComponent<Renderer>())
		{
			UnityEngine.Debug.LogError("GuiButton " + base.name + " has renderer", base.gameObject);
		}
		CreateContentTransform();
	}

	private void CreateContentTransform()
	{
		if (content != null)
		{
			return;
		}
		content = new GameObject("Content").transform;
		content.parent = base.transform;
		content.localPosition = Vector3.zero;
		content.localRotation = Quaternion.identity;
		content.localScale = Vector3.one;
		int num = 0;
		while (num < base.transform.childCount)
		{
			Transform child = base.transform.GetChild(num);
			if (child != content && child.GetComponent<FSequence>() == null)
			{
				child.SetParent(content, worldPositionStays: true);
			}
			else
			{
				num++;
			}
		}
	}

	protected override void Start()
	{
		base.Start();
		OnTransformParentChanged();
	}

	private void OnTransformParentChanged()
	{
		Transform parent = base.transform.parent;
		while (parent != null)
		{
			Scroll = parent.GetComponent<GuiScroll>();
			if ((bool)Scroll)
			{
				break;
			}
			parent = parent.parent;
		}
	}

	private void OnEnable()
	{
		Reset();
	}

	public override bool OnTouchBegan(int index, Vector3 position)
	{
		if (!toggleActive && HasSequence("DownInactive"))
		{
			GoToLastFrame("DownInactive");
		}
		else if (HasSequence("Down"))
		{
			GoToLastFrame("Down");
		}
		if (Down != null)
		{
			Down();
		}
		SingletonMonoBehaviour<Gui>.Instance.GrabTouch(this, index);
		FocusRotation = Vector3.zero;
		pressed = true;
		ApplyHighlight();
		touchIndex = index;
		touchStart = position - SingletonMonoBehaviour<Gui>.Instance.GuiCamera.WorldToScreenPoint(base.transform.position);
		if (Scroll != null && SingletonMonoBehaviour<Gui>.Instance.Touchable)
		{
			Scroll.OnTouchBeganInternal(index, position, grab: false);
		}
		return true;
	}

	protected override void Update()
	{
		base.Update();
		CreateContentTransform();
		content.localScale = Vector3.one * (1f + FocusScale(Time.realtimeSinceStartup - focusStartTime)) * ((!pressed) ? 1f : PressedScale);
		if (!(Scroll == null) && pressed && SingletonMonoBehaviour<Gui>.Instance.Touchable)
		{
			Vector3 touchPosition = SingletonMonoBehaviour<Gui>.Instance.GetTouchPosition(touchIndex);
			if (Vector3.Distance(touchPosition - SingletonMonoBehaviour<Gui>.Instance.GuiCamera.WorldToScreenPoint(base.transform.position), touchStart) > Screen.dpi * 0.05f)
			{
				SingletonMonoBehaviour<Gui>.Instance.CancelGrabTouches();
				SingletonMonoBehaviour<Gui>.Instance.GrabTouch(Scroll, touchIndex);
			}
			else
			{
				Scroll.OnTouchMove();
			}
		}
	}

	private float FocusScale(float x)
	{
		if (SingletonMonoBehaviour<Gui>.Instance.Touchable || SingletonMonoBehaviour<Gui>.Instance.FocusView != this || HasSequence("GotFocus"))
		{
			return 0f;
		}
		float a = 0.02f * Mathf.Cos(x * 7f);
		float t = 1f - Mathf.Clamp(x, 0f, 0.3f) / 0.3f;
		float b = Tween.QuadEaseInOut(t, 0f, 1f, 1f) * 0.1f;
		return Mathf.Lerp(a, b, t);
	}

	public override bool OnTouchEnded(int index, Vector3 position, bool inside)
	{
		if (!pressed)
		{
			return false;
		}
		if (SingletonMonoBehaviour<Gui>.Instance.Touchable)
		{
			ResetHighlight();
		}
		pressed = false;
		if (inside)
		{
			if (ResetOnClick)
			{
				Reset();
			}
			if (Click != null)
			{
				Click();
			}
			if (Sound != null)
			{
				Sound.Play();
			}
		}
		else
		{
			if (!SingletonMonoBehaviour<Gui>.Instance.Touchable)
			{
				ResetHighlight();
			}
			Reset();
		}
		if (Up != null)
		{
			Up();
		}
		return true;
	}

	public override void OnTouchCancel(int index)
	{
		Reset();
		ResetHighlight();
		pressed = false;
	}

	private void SetHighlightRenderer()
	{
		if (HighlightRamp != null)
		{
			highlightPropertyBlock = new MaterialPropertyBlock();
			highlightPropertyBlock.SetTexture("_LightRamp", HighlightRamp);
			if (HighlightRenderer == null)
			{
				HighlightRenderer = GetComponentInChildren<MeshRenderer>();
			}
		}
	}

	private void ApplyHighlight()
	{
		SetHighlightRenderer();
		if (HighlightRenderer != null && highlightPropertyBlock != null)
		{
			HighlightRenderer.SetPropertyBlock(highlightPropertyBlock);
		}
	}

	private void ResetHighlight()
	{
		if (HighlightRenderer != null)
		{
			HighlightRenderer.SetPropertyBlock(null);
		}
	}

	public override void OnHoverBegan(Vector3 position)
	{
		if (!SingletonMonoBehaviour<Gui>.Instance.Touchable)
		{
			if (SingletonMonoBehaviour<Gui>.Instance.CanTouch(this) && HasSequence("GotFocus"))
			{
				StopSingle("LostFocus");
				PlaySingle("GotFocus");
			}
			focusStartTime = Time.realtimeSinceStartup;
			ApplyHighlight();
		}
	}

	public override void OnHoverEnded(Vector3 position)
	{
		if (!SingletonMonoBehaviour<Gui>.Instance.Touchable)
		{
			if (SingletonMonoBehaviour<Gui>.Instance.CanTouch(this) && base.TouchEnabledInHierarchy && HasSequence("LostFocus"))
			{
				StopSingle("GotFocus");
				PlaySingle("LostFocus");
			}
			else
			{
				CreateContentTransform();
				content.localScale = Vector3.one;
			}
			AnimateResetFocusRotation();
			ResetHighlight();
		}
	}

	public void Reset()
	{
		if (HasSequence("LostFocus"))
		{
			GoToLastFrameSingle("LostFocus");
		}
		if (!toggleActive && HasSequence("UpInactive"))
		{
			GoToLastFrame("UpInactive");
		}
		else if (!toggleActive && HasSequence("DownInactive"))
		{
			GoToFirstFrame("DownInactive");
		}
		else if (HasSequence("Up"))
		{
			GoToLastFrame("Up");
		}
		else if (HasSequence("Down"))
		{
			GoToFirstFrame("Down");
		}
	}

	public void AnimateResetFocusRotation()
	{
		if (!(FocusRotation == Vector3.zero) && focusRotationCoroutine == null)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				UpdateFocusRotation(Vector3.zero);
			}
			else
			{
				StartCoroutine(focusRotationCoroutine = ResetFocusRotationCoroutine());
			}
		}
	}

	private IEnumerator ResetFocusRotationCoroutine()
	{
		float startTime = Time.realtimeSinceStartup;
		float delta = 0.2f;
		Vector3 startRotation = focusRotation;
		Vector3 targetRotation = Vector3.zero;
		while (Time.realtimeSinceStartup - startTime < delta)
		{
			float a2 = (Time.realtimeSinceStartup - startTime) / delta;
			a2 = Tween.CubicEaseInOut(a2, 0f, 1f, 1f);
			UpdateFocusRotation(Vector3.Lerp(startRotation, targetRotation, a2));
			yield return null;
		}
		UpdateFocusRotation(Vector3.zero);
	}
}
