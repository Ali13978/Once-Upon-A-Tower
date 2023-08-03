using Flux;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuiView : MonoBehaviour
{
	public int ZIndex;

	private bool touchEnabled = true;

	private List<Transform> children;

	protected FSequence lastSequence;

	private Action onSequenceEnd;

	private bool visible;

	public GuiView Parent
	{
		get;
		private set;
	}

	public Dictionary<string, FSequence> Sequences
	{
		get;
		private set;
	}

	public Bounds Bounds
	{
		get;
		private set;
	}

	public Collider Collider
	{
		get;
		private set;
	}

	public Collider2D Collider2D
	{
		get;
		private set;
	}

	public Bounds ColliderBounds => (!(Collider2D != null)) ? Collider.bounds : Collider2D.bounds;

	public bool TouchEnabled
	{
		get
		{
			return touchEnabled;
		}
		set
		{
			touchEnabled = value;
			if (Collider2D != null)
			{
				Collider2D.enabled = touchEnabled;
			}
		}
	}

	public bool Hiding
	{
		get;
		protected set;
	}

	public bool TouchEnabledInHierarchy
	{
		get
		{
			GuiView guiView = this;
			while (guiView != null)
			{
				if (!guiView.TouchEnabled)
				{
					return false;
				}
				guiView = guiView.Parent;
			}
			return true;
		}
	}

	public bool IsPlaying => lastSequence != null && lastSequence.IsPlaying;

	public bool Visible
	{
		get
		{
			return visible;
		}
		private set
		{
			visible = value;
			SingletonMonoBehaviour<GameInput>.Instance.Enabled = !SingletonMonoBehaviour<Gui>.Instance.Active;
		}
	}

	public bool IsPlayingSequence(string sequence)
	{
		return IsPlaying && Sequences.ContainsKey(sequence) && lastSequence == Sequences[sequence];
	}

	public bool IsPlayingSingle(string sequence)
	{
		return Sequences.ContainsKey(sequence) && Sequences[sequence].IsPlaying;
	}

	protected virtual void Awake()
	{
		Collider = GetComponent<Collider>();
		Collider2D = GetComponent<Collider2D>();
		Transform parent = base.transform.parent;
		while (parent != null && Parent == null)
		{
			Parent = parent.GetComponent<GuiView>();
			parent = parent.parent;
		}
		Sequences = new Dictionary<string, FSequence>();
		children = new List<Transform>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			FSequence component = child.GetComponent<FSequence>();
			if ((bool)component)
			{
				string text = component.name;
				if (text.EndsWith("Sequence"))
				{
					text = text.Substring(0, text.Length - "Sequence".Length);
				}
				Sequences[text] = component;
				component.Init();
				component.gameObject.SetActive(value: false);
			}
			else
			{
				children.Add(child);
			}
		}
		ApplyBoundingBox();
		if (Application.isPlaying && base.transform.parent == null)
		{
			if (SceneManager.sceneCount == 1 && !SingletonMonoBehaviour<Gui>.HasInstance())
			{
				UnityEngine.Debug.Log("Create test Gui");
				Gui gui = new GameObject("Gui").AddComponent<Gui>();
				Camera camera = new GameObject("GuiCamera").AddComponent<Camera>();
				gui.gameObject.AddComponent<AudioListener>();
				camera.orthographic = true;
				camera.transform.position = base.transform.position + new Vector3(0f, 0f, -15f);
				camera.clearFlags = CameraClearFlags.Color;
				camera.cullingMask = 1 << LayerMask.NameToLayer("UI");
				camera.transform.parent = gui.transform;
				gui.GuiCamera = camera;
				base.transform.parent = gui.transform;
				StartCoroutine(ShowAnimatedInNextFrame());
			}
			if (SingletonMonoBehaviour<Gui>.HasInstance())
			{
				Gui.Views.GetType().GetField(base.gameObject.scene.name)?.SetValue(Gui.Views, this);
				Gui.Views.All.Add(this);
				Hide();
			}
		}
	}

	protected void ApplyBoundingBox()
	{
		BoundingBox[] componentsInChildren = GetComponentsInChildren<BoundingBox>(includeInactive: true);
		foreach (BoundingBox boundingBox in componentsInChildren)
		{
			if (boundingBox.enabled)
			{
				boundingBox.Apply();
			}
		}
	}

	protected virtual void Start()
	{
	}

	private void OnTransformParentChanged()
	{
		Transform parent = base.transform.parent;
		while (parent != null && Parent == null)
		{
			Parent = parent.GetComponent<GuiView>();
			parent = parent.parent;
		}
	}

	public virtual void Show()
	{
		TouchEnabled = true;
		Hiding = false;
		Visible = true;
		for (int i = 0; i < children.Count; i++)
		{
			children[i].gameObject.SetActive(value: true);
		}
		if (Gui.TV)
		{
			SingletonMonoBehaviour<Gui>.Instance.UpdateTVAllowExit();
		}
	}

	public virtual void Hide()
	{
		Hiding = false;
		Visible = false;
		for (int i = 0; i < children.Count; i++)
		{
			children[i].gameObject.SetActive(value: false);
		}
		if (Gui.TV)
		{
			SingletonMonoBehaviour<Gui>.Instance.UpdateTVAllowExit();
		}
	}

	public IEnumerator ShowAnimatedInNextFrame()
	{
		yield return null;
		ShowAnimated();
	}

	public virtual void ShowAnimated()
	{
		if (Hiding)
		{
			StopCurrentSequence();
		}
		Show();
		if (Sequences.ContainsKey("Intro"))
		{
			Play("Intro");
		}
	}

	public virtual void HideAnimated()
	{
		if (Visible && !Hiding)
		{
			Hiding = true;
			TouchEnabled = false;
			if (Sequences.ContainsKey("Outro"))
			{
				Play("Outro", Hide);
			}
			else
			{
				Hide();
			}
		}
	}

	public void StopCurrentSequence()
	{
		if (lastSequence != null)
		{
			lastSequence.Stop();
			lastSequence.gameObject.SetActive(value: false);
			lastSequence = null;
			if (onSequenceEnd != null)
			{
				onSequenceEnd();
			}
			onSequenceEnd = null;
		}
	}

	public void Play(string sequence, Action onSequenceEnd = null)
	{
		StopCurrentSequence();
		if (Sequences == null || !Sequences.TryGetValue(sequence, out lastSequence))
		{
			if (Application.isPlaying)
			{
				UnityEngine.Debug.LogError("Trying to play sequence " + sequence + " in " + base.name);
			}
		}
		else
		{
			this.onSequenceEnd = onSequenceEnd;
			lastSequence.gameObject.SetActive(value: true);
			lastSequence.Stop();
			lastSequence.Play();
		}
	}

	public IEnumerator PlayInCoroutine(string sequence)
	{
		bool finished = false;
		Play(sequence, delegate
		{
			finished = true;
		});
		while (!finished)
		{
			yield return null;
		}
	}

	public void PlaySingle(string seqname)
	{
		FSequence value;
		if (Sequences == null || !Sequences.TryGetValue(seqname, out value))
		{
			if (Application.isPlaying)
			{
				UnityEngine.Debug.LogError("Trying to play sequence " + seqname + " in " + base.name);
			}
		}
		else
		{
			value.gameObject.SetActive(value: true);
			value.Play();
		}
	}

	public void StopSingle(string seqname)
	{
		FSequence value;
		if (Sequences == null || !Sequences.TryGetValue(seqname, out value))
		{
			if (Application.isPlaying)
			{
				UnityEngine.Debug.LogError("Trying to play sequence " + seqname + " in " + base.name);
			}
		}
		else
		{
			value.Stop();
			value.gameObject.SetActive(value: false);
		}
	}

	public void GoToLastFrameSingle(string seqname)
	{
		FSequence value;
		if (Sequences == null || !Sequences.TryGetValue(seqname, out value))
		{
			UnityEngine.Debug.LogError("Trying to play sequence " + seqname + " in " + base.name);
			return;
		}
		value.gameObject.SetActive(value: true);
		value.Stop();
		value.SetCurrentFrame(value.Length);
	}

	public void PauseCurrentSequence()
	{
		if (lastSequence != null)
		{
			lastSequence.Pause();
		}
	}

	public void ResumeCurrentSequence()
	{
		if (lastSequence != null)
		{
			lastSequence.Resume();
		}
	}

	public void GoToFirstFrame(string sequence)
	{
		StopCurrentSequence();
		if (Sequences == null || !Sequences.TryGetValue(sequence, out lastSequence))
		{
			UnityEngine.Debug.LogError("Trying to play sequence " + sequence + " in " + base.name);
			return;
		}
		lastSequence.gameObject.SetActive(value: true);
		lastSequence.Stop();
		lastSequence.SetCurrentFrame(0);
	}

	public void GoToFrameProportional(string sequence, float a)
	{
		StopCurrentSequence();
		if (Sequences == null || !Sequences.TryGetValue(sequence, out lastSequence))
		{
			UnityEngine.Debug.LogError("Trying to play sequence " + sequence + " in " + base.name);
			return;
		}
		lastSequence.gameObject.SetActive(value: true);
		lastSequence.Stop();
		lastSequence.SetCurrentFrame(Mathf.RoundToInt(a * (float)lastSequence.Length));
	}

	public void GoToLastFrame(string sequence)
	{
		StopCurrentSequence();
		if (Sequences == null || !Sequences.TryGetValue(sequence, out lastSequence))
		{
			UnityEngine.Debug.LogError("Trying to play sequence " + sequence + " in " + base.name);
			return;
		}
		lastSequence.gameObject.SetActive(value: true);
		lastSequence.Stop();
		lastSequence.SetCurrentFrame(lastSequence.Length);
	}

	protected virtual void Update()
	{
		if (lastSequence != null && !lastSequence.IsPlaying && lastSequence.CurrentFrame == lastSequence.Length && onSequenceEnd != null)
		{
			onSequenceEnd();
			onSequenceEnd = null;
		}
	}

	public virtual void UpdateText()
	{
	}

	public virtual void Message(string message)
	{
	}

	public bool HasSequence(string sequence)
	{
		return Sequences != null && Sequences.ContainsKey(sequence);
	}

	public virtual bool OnTouchBegan(int index, Vector3 position)
	{
		return false;
	}

	public virtual bool OnTouchEnded(int index, Vector3 position, bool inside)
	{
		return false;
	}

	public virtual void OnTouchCancel(int index)
	{
	}

	public virtual void OnHoverBegan(Vector3 position)
	{
	}

	public virtual void OnHoverEnded(Vector3 position)
	{
	}

	public virtual bool OnMenuButton()
	{
		return false;
	}

	public virtual bool HandlesMenuButton()
	{
		return false;
	}

	public void UpdateBounds(Camera camera)
	{
		if ((Collider == null && Collider2D == null) || (Collider != null && !Collider.enabled) || (Collider2D != null && !Collider2D.enabled))
		{
			Bounds = new Bounds(new Vector3(-10000f, -10000f, -10000f), Vector3.zero);
			return;
		}
		Bounds colliderBounds = ColliderBounds;
		Vector3 b = camera.WorldToScreenPoint(colliderBounds.min);
		Vector3 a = camera.WorldToScreenPoint(colliderBounds.max);
		if (b.z < camera.nearClipPlane || a.z < camera.nearClipPlane)
		{
			Bounds = new Bounds(new Vector3(-10000f, -10000f, -10000f), Vector3.zero);
			return;
		}
		colliderBounds = new Bounds((a + b) / 2f, new Vector3(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z)));
		Vector3 center = colliderBounds.center;
		float x = center.x;
		Vector3 center2 = colliderBounds.center;
		colliderBounds.center = new Vector3(x, center2.y, 0f);
		Bounds = colliderBounds;
	}
}
