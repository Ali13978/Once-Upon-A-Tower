using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gui : SingletonMonoBehaviour<Gui>
{
	private class Touch
	{
		public int Id;

		public GuiView Grab;

		public Vector3 Position;
	}

	public enum Direction
	{
		Up,
		Left,
		Right,
		Down
	}

	private List<Touch> touches = new List<Touch>(10);

	private GuiView Hover;

	private RaycastHit2D[] hits = new RaycastHit2D[20];

	private Player rewiredPlayer;

	public GuiView FocusView;

	private Vector3 tvTouchReference;

	private Vector3 tvTouchStart;

	private Vector3 tvTouchPosition;

	private Vector3 tvTouchVelocity;

	public AudioSource TextLoop;

	public bool Ready;

	private List<GuiView> LaunchViews;

	private static bool cachedAndroidTV;

	private static bool androidTV;

	public GuiViews views;

	public Camera GuiCamera;

	public Camera SecondaryCamera;

	private GuiView onlyTouchView;

	private GuiView onlyVisibleView;

	private int guiLayer;

	private bool supportsTouch;

	private bool mouseButtonDown;

	private bool prevAllowExitToHome;

	private List<GuiButton> buttonList = new List<GuiButton>();

	public bool Touchable => !TV;

	public bool Active
	{
		get
		{
			if (Touchable)
			{
				return false;
			}
			if (LaunchViews == null)
			{
				return false;
			}
			for (int i = 0; i < Views.All.Count; i++)
			{
				GuiView guiView = Views.All[i];
				if (guiView.Visible && !LaunchViews.Contains(guiView))
				{
					return true;
				}
			}
			if (OnlyTouchView != null && OnlyTouchView.gameObject.activeInHierarchy)
			{
				return true;
			}
			return false;
		}
	}

	public static bool AndroidTV
	{
		get
		{
			if (!cachedAndroidTV)
			{
				cachedAndroidTV = true;
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.content.Context");
				AndroidJavaObject static2 = androidJavaClass2.GetStatic<AndroidJavaObject>("UI_MODE_SERVICE");
				AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getSystemService", new object[1]
				{
					static2
				});
				int num = androidJavaObject.Call<int>("getCurrentModeType", new object[0]);
				AndroidJavaClass androidJavaClass3 = new AndroidJavaClass("android.content.res.Configuration");
				int static3 = androidJavaClass3.GetStatic<int>("UI_MODE_TYPE_TELEVISION");
				if (static3 == num)
				{
					androidTV = true;
				}
				else
				{
					androidTV = false;
				}
			}
			return androidTV;
		}
	}

	public static bool AppleTV => false;

	public static bool iOS => false;

	public static bool TV => AndroidTV || AppleTV;

	public static bool Android => true;

	public static GuiViews Views => SingletonMonoBehaviour<Gui>.Instance.views;

	public bool TouchHandled
	{
		get;
		private set;
	}

	public bool Enabled
	{
		get;
		set;
	}

	public GuiView OnlyTouchView
	{
		get
		{
			return onlyTouchView;
		}
		set
		{
			onlyTouchView = value;
			SingletonMonoBehaviour<GameInput>.Instance.Enabled = !Active;
		}
	}

	public GuiView OnlyVisibleView
	{
		get
		{
			return onlyVisibleView;
		}
		set
		{
			if (onlyVisibleView != null)
			{
				ChangeLayer(onlyVisibleView.transform, LayerMask.NameToLayer("UISingle"), LayerMask.NameToLayer("UI"));
			}
			if (value != null)
			{
				ChangeLayer(value.transform, LayerMask.NameToLayer("UI"), LayerMask.NameToLayer("UISingle"));
				GuiCamera.cullingMask = 1 << LayerMask.NameToLayer("UISingle");
			}
			else
			{
				GuiCamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
			}
			onlyVisibleView = value;
		}
	}

	public bool IsGrabbingAny
	{
		get
		{
			for (int i = 0; i < touches.Count; i++)
			{
				if (touches[i] != null && touches[i].Grab != null)
				{
					return true;
				}
			}
			return false;
		}
	}

	public event Action<GuiView> FocusChanged;

	protected override void Awake()
	{
		base.Awake();
		StartCoroutine(LoadGuiScenes());
	}

	private IEnumerator LoadGuiScenes()
	{
		yield return null;
		GuiScenes guiScenes = Resources.Load<GuiScenes>("GuiViewList");
		List<AsyncOperation> asyncs = new List<AsyncOperation>();
		GuiViewScene[] scenes = guiScenes.Scenes;
		foreach (GuiViewScene guiViewScene in scenes)
		{
			if (!SceneManager.GetSceneByBuildIndex(guiViewScene.BuildIndex).isLoaded)
			{
				asyncs.Add(SceneManager.LoadSceneAsync(guiViewScene.BuildIndex, LoadSceneMode.Additive));
			}
		}
		while (!asyncs.All((AsyncOperation a) => a.isDone))
		{
			yield return null;
		}
		yield return null;
		Ready = true;
		LaunchViews = new List<GuiView>
		{
			Views.ComboView,
			Views.ItemsView,
			Views.InGameMissionCompleteView,
			Views.InGameStatsView,
			Views.InGameStoreItemView,
			Views.MissionsView,
			Views.MissionTextTemplate,
			Views.PauseButtonView,
			Views.TutorialView,
			Views.CharacterDealMessage
		};
	}

	public void RunOnReady(Action action)
	{
		StartCoroutine(RunOnReadyCoroutine(action));
	}

	private IEnumerator RunOnReadyCoroutine(Action action)
	{
		while (!Ready)
		{
			yield return null;
		}
		action();
	}

	private void Start()
	{
		Enabled = true;
		guiLayer = LayerMask.NameToLayer("Gui");
		Canvas[] componentsInChildren = GetComponentsInChildren<Canvas>(includeInactive: true);
		foreach (Canvas canvas in componentsInChildren)
		{
			canvas.worldCamera = GuiCamera;
		}
		rewiredPlayer = ReInput.players.GetSystemPlayer();
	}

	private GuiView FindMenuButtonHandler()
	{
		if (!Ready)
		{
			return null;
		}
		GuiView guiView = FocusView;
		while (guiView != null && !guiView.HandlesMenuButton())
		{
			guiView = guiView.Parent;
		}
		if (guiView == null && Views.PauseButtonView.HandlesMenuButton())
		{
			guiView = Views.PauseButtonView;
		}
		return guiView;
	}

	private void Update()
	{
		if (TV)
		{
			UpdateTV();
		}
		else
		{
			UpdateTouch();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || (AppleTV && UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0)) || rewiredPlayer.GetButtonDown(4))
		{
			GuiView guiView = FindMenuButtonHandler();
			if (guiView != null)
			{
				guiView.OnMenuButton();
			}
			else if (Android && !Views.ConfirmExit.Visible)
			{
				Views.ConfirmExit.ShowAnimated();
			}
		}
		if ((UnityEngine.Input.GetKeyDown(KeyCode.Space) || (AppleTV && UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton14)) || (AndroidTV && UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton0)) || rewiredPlayer.GetButtonDown(5)) && FocusView != null && FocusView.gameObject.activeInHierarchy)
		{
			FocusView.OnTouchBegan(0, FocusView.Bounds.center);
		}
		if (!Input.GetKeyUp(KeyCode.Space) && (!AppleTV || !Input.GetKeyUp(KeyCode.JoystickButton14)) && (!AndroidTV || !Input.GetKeyUp(KeyCode.JoystickButton0)) && !rewiredPlayer.GetButtonUp(5))
		{
			return;
		}
		if (FocusView != null && FocusView.gameObject.activeInHierarchy)
		{
			FocusView.OnTouchEnded(0, FocusView.Bounds.center, inside: true);
			return;
		}
		BombButton component = Views.ItemsView.BombsButton.GetComponent<BombButton>();
		if (component.gameObject.activeInHierarchy)
		{
			component.OnClick();
		}
	}

	private void UpdateTouch()
	{
		CheckJoystick();
		if (UnityEngine.Input.touchCount > 0)
		{
			supportsTouch = true;
			for (int i = 0; i < UnityEngine.Input.touchCount; i++)
			{
				UnityEngine.Touch touch = UnityEngine.Input.GetTouch(i);
				if (touch.phase == TouchPhase.Began)
				{
					TouchBegan(touch.fingerId, touch.position);
				}
				else if (touch.phase == TouchPhase.Ended)
				{
					TouchEnded(touch.fingerId, touch.position);
				}
				FindTouch(touch.fingerId).Position = touch.position;
			}
		}
		else if (!supportsTouch)
		{
			if (Input.GetMouseButtonDown(0))
			{
				TouchBegan(0, UnityEngine.Input.mousePosition);
				mouseButtonDown = true;
			}
			else if (Input.GetMouseButtonUp(0) || (mouseButtonDown && !Input.GetMouseButton(0)))
			{
				TouchEnded(0, UnityEngine.Input.mousePosition);
				mouseButtonDown = false;
			}
			FindTouch(0).Position = UnityEngine.Input.mousePosition;
			UpdateHover(UnityEngine.Input.mousePosition);
		}
	}

	private void CheckJoystick()
	{
		if (rewiredPlayer != null)
		{
			if (rewiredPlayer.GetButtonDown(0))
			{
				SelectLeft();
			}
			if (rewiredPlayer.GetButtonDown(1))
			{
				SelectRight();
			}
			if (rewiredPlayer.GetButtonDown(3))
			{
				SelectDown();
			}
			if (rewiredPlayer.GetButtonDown(2))
			{
				SelectUp();
			}
		}
	}

	private void UpdateTV()
	{
		if (!(SingletonMonoBehaviour<Gui>.Instance.FocusView == null))
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
			{
				SelectLeft();
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
			{
				SelectRight();
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
			{
				SelectUp();
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
			{
				SelectDown();
			}
			CheckJoystick();
		}
	}

	private void TVTouchMove(Vector3 touchPosition, Vector3 delta)
	{
		if (touchPosition.x - tvTouchReference.x > delta.x)
		{
			SelectRight();
			tvTouchReference = touchPosition;
		}
		else if (tvTouchReference.x - touchPosition.x > delta.x)
		{
			SelectLeft();
			tvTouchReference = touchPosition;
		}
		else if (tvTouchReference.y - touchPosition.y > delta.y)
		{
			SelectDown();
			tvTouchReference = touchPosition;
		}
		else if (touchPosition.y - tvTouchReference.y > delta.y)
		{
			SelectUp();
			tvTouchReference = touchPosition;
		}
	}

	public bool CanTouch(GuiView view)
	{
		if (!Enabled)
		{
			return false;
		}
		if (!view.TouchEnabledInHierarchy)
		{
			return false;
		}
		if (OnlyTouchView != null)
		{
			while (view != null)
			{
				if (view == OnlyTouchView)
				{
					return true;
				}
				view = view.Parent;
			}
			return false;
		}
		return true;
	}

	public GuiView FindViewInPosition(Vector3 position)
	{
		Vector3 position2 = new Vector3(position.x, position.y, 0f);
		Ray ray = GuiCamera.ScreenPointToRay(position2);
		int num = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, hits, 100f, guiLayer);
		for (int i = 0; i < num; i++)
		{
			RaycastHit2D raycastHit2D = hits[i];
			if ((bool)raycastHit2D.collider)
			{
				GuiView component = raycastHit2D.collider.GetComponent<GuiView>();
				if (component != null && CanTouch(component))
				{
					return component;
				}
			}
		}
		if (SecondaryCamera != null)
		{
			ray = SecondaryCamera.ScreenPointToRay(position2);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 100f))
			{
				// Code to handle the hitInfo
			}

			{
				return hitInfo.collider.GetComponent<GuiView>();
			}
		}
		return null;
	}

	private void TouchBegan(int index, Vector3 position)
	{
		for (int i = 0; i < touches.Count; i++)
		{
			if (touches[i] != null && touches[i].Grab != null)
			{
				return;
			}
		}
		GuiView guiView = FindViewInPosition(position);
		if (!(guiView != null))
		{
			return;
		}
		bool flag = false;
		while ((bool)guiView && !flag)
		{
			if (CanTouch(guiView))
			{
				flag = guiView.OnTouchBegan(index, position);
			}
			guiView = guiView.Parent;
		}
		TouchHandled = flag;
	}

	private void TouchEnded(int index, Vector3 position)
	{
		GuiView guiView = FindGrabTouch(index);
		GuiView guiView2 = FindViewInPosition(position);
		if (guiView == null)
		{
			guiView = guiView2;
		}
		if (guiView != null)
		{
			bool flag = false;
			while ((bool)guiView && !flag)
			{
				if (CanTouch(guiView))
				{
					flag = guiView.OnTouchEnded(index, position, guiView2 == guiView);
				}
				guiView = guiView.Parent;
			}
			TouchHandled = flag;
		}
		ReleaseGrabTouch(index);
		TouchHandled = false;
	}

	private void UpdateHover(Vector3 position)
	{
		if (FindGrabTouch(0) != null)
		{
			return;
		}
		GuiView guiView = FindViewInPosition(position);
		if (!(guiView == Hover))
		{
			if (guiView != null)
			{
				guiView.OnHoverBegan(position);
			}
			if (Hover != null)
			{
				Hover.OnHoverEnded(position);
			}
			Hover = guiView;
		}
	}

	private void UpdateFocus(Vector3 position)
	{
		if (!(FindGrabTouch(0) != null))
		{
			GuiView view = FindViewInPosition(position);
			UpdateFocus(view);
		}
	}

	public void UpdateFocus(GuiView view, bool scrollEnabled = true)
	{
		if (view == FocusView)
		{
			return;
		}
		if (view != null)
		{
			view.OnHoverBegan(Vector3.zero);
		}
		if (FocusView != null)
		{
			FocusView.OnHoverEnded(Vector3.zero);
		}
		FocusView = view;
		if (TV)
		{
			UpdateTVAllowExit();
		}
		if (scrollEnabled)
		{
			GuiView guiView = view;
			while (guiView != null && guiView.GetComponent<GuiScroll>() == null)
			{
				guiView = guiView.Parent;
			}
			if (guiView != null)
			{
				GuiScroll component = guiView.GetComponent<GuiScroll>();
				component.ScrollToView(view);
			}
		}
		if (this.FocusChanged != null)
		{
			this.FocusChanged(FocusView);
		}
	}

	public void UpdateTVAllowExit()
	{
	}

	private Touch FindTouch(int index)
	{
		for (int i = 0; i < touches.Count; i++)
		{
			if (touches[i] != null && touches[i].Id == index)
			{
				return touches[i];
			}
		}
		for (int j = 0; j < touches.Count; j++)
		{
			if (touches[j] == null || touches[j].Grab == null)
			{
				if (touches[j] == null)
				{
					touches[j] = new Touch();
				}
				touches[j].Id = index;
				touches[j].Grab = null;
				touches[j].Position = Vector3.zero;
				return touches[j];
			}
		}
		touches.Add(new Touch
		{
			Id = index,
			Grab = null,
			Position = Vector3.zero
		});
		if (touches.Count > 100)
		{
			UnityEngine.Debug.LogError("Touches array is too large: " + touches.Count + " items.");
		}
		return touches[touches.Count - 1];
	}

	private GuiView FindGrabTouch(int index)
	{
		return FindTouch(index).Grab;
	}

	private void ReleaseGrabTouch(int index)
	{
		FindTouch(index).Grab = null;
	}

	public void GrabTouch(GuiView view, int index)
	{
		FindTouch(index).Grab = view;
	}

	public void CancelGrabTouches()
	{
		for (int i = 0; i < touches.Count; i++)
		{
			if (touches[i] != null && touches[i].Grab != null)
			{
				touches[i].Grab.OnTouchCancel(i);
				touches[i].Grab = null;
			}
		}
	}

	public bool IsGrabbing(GuiView view)
	{
		for (int i = 0; i < touches.Count; i++)
		{
			if (touches[i] != null && touches[i].Grab == view)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsGrabbingType<T>()
	{
		for (int i = 0; i < touches.Count; i++)
		{
			if (touches[i] != null && touches[i].Grab is T)
			{
				return true;
			}
		}
		return false;
	}

	public Vector3 GetTouchPosition(int index)
	{
		return FindTouch(index).Position;
	}

	public GuiButton FindNextButton(GuiButton reference, Vector3 dir)
	{
		Vector3 vector = new Vector3(Screen.width, Screen.height, 0f);
		Bounds bounds = new Bounds(vector / 2f, vector);
		DebugDrawBounds(bounds);
		foreach (GuiView item in Views.All)
		{
			GuiButton[] componentsInChildren = item.GetComponentsInChildren<GuiButton>();
			foreach (GuiButton guiButton in componentsInChildren)
			{
				Collider2D component = guiButton.GetComponent<Collider2D>();
				if (!(component == null))
				{
					Bounds bounds2 = ProjectBounds(GuiCamera, component.bounds);
					if (bounds.Intersects(bounds2))
					{
						DebugDrawBounds(bounds2);
					}
				}
			}
		}
		return null;
	}

	private Bounds ProjectBounds(Camera camera, Bounds bounds)
	{
		Vector3 b = camera.WorldToScreenPoint(bounds.min);
		Vector3 a = camera.WorldToScreenPoint(bounds.max);
		bounds = new Bounds((a + b) / 2f, a - b);
		Vector3 center = bounds.center;
		float x = center.x;
		Vector3 center2 = bounds.center;
		bounds.center = new Vector3(x, center2.y, 0f);
		return bounds;
	}

	private void DebugDrawBounds(Bounds bounds)
	{
		Vector3 min = bounds.min;
		Vector3 min2 = bounds.min;
		Vector3 size = bounds.size;
		UnityEngine.Debug.DrawLine(min, min2 + new Vector3(size.x, 0f, 0f));
		Vector3 min3 = bounds.min;
		Vector3 min4 = bounds.min;
		Vector3 size2 = bounds.size;
		UnityEngine.Debug.DrawLine(min3, min4 + new Vector3(0f, size2.y, 0f));
		Vector3 max = bounds.max;
		Vector3 max2 = bounds.max;
		Vector3 size3 = bounds.size;
		UnityEngine.Debug.DrawLine(max, max2 - new Vector3(size3.x, 0f, 0f));
		Vector3 max3 = bounds.max;
		Vector3 max4 = bounds.max;
		Vector3 size4 = bounds.size;
		UnityEngine.Debug.DrawLine(max3, max4 - new Vector3(0f, size4.y, 0f));
	}

	private static void ChangeLayer(Transform transform, int source, int target)
	{
		if (transform.gameObject.layer == source)
		{
			transform.gameObject.layer = target;
		}
		foreach (GuiView item in Views.All)
		{
			ChangeLayer(item.transform, source, target);
		}
	}

	public void FocusNextButton(Direction dir)
	{
		Vector3 vector = new Vector3(Screen.width, Screen.height, 0f);
		Bounds bounds = new Bounds(vector / 2f, vector);
		buttonList.Clear();
		foreach (GuiView item in Views.All)
		{
			GuiButton[] componentsInChildren = item.GetComponentsInChildren<GuiButton>();
			foreach (GuiButton guiButton in componentsInChildren)
			{
				guiButton.UpdateBounds(GuiCamera);
				if ((bool)guiButton.Scroll)
				{
					guiButton.Scroll.UpdateBounds(GuiCamera);
				}
				if (bounds.Intersects(guiButton.Bounds) || ((bool)guiButton.Scroll && bounds.Intersects(guiButton.Scroll.Bounds)))
				{
					buttonList.Add(guiButton);
				}
			}
		}
		Bounds bounds2 = (FocusView != null) ? FocusView.Bounds : new Bounds(bounds.center, Vector3.zero);
		int num = 0;
		while (num < buttonList.Count)
		{
			bool flag = false;
			GuiButton guiButton2 = buttonList[num];
			foreach (GuiButton button in buttonList)
			{
				Vector3 position = button.transform.position;
				float z = position.z;
				Vector3 position2 = guiButton2.transform.position;
				if ((z < position2.z && button.Collider2D != null && guiButton2.Collider2D != null) || (button.Collider2D != null && guiButton2.Collider != null))
				{
					Vector3 max = button.Bounds.max;
					float x = max.x;
					Vector3 max2 = guiButton2.Bounds.max;
					if (x >= max2.x)
					{
						Vector3 max3 = button.Bounds.max;
						float y = max3.y;
						Vector3 max4 = guiButton2.Bounds.max;
						if (y >= max4.y)
						{
							Vector3 min = button.Bounds.min;
							float x2 = min.x;
							Vector3 min2 = guiButton2.Bounds.min;
							if (x2 <= min2.x)
							{
								Vector3 min3 = button.Bounds.min;
								float y2 = min3.y;
								Vector3 min4 = guiButton2.Bounds.min;
								if (y2 <= min4.y)
								{
									flag = true;
									break;
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				buttonList.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
		int num2 = 0;
		while (num2 < buttonList.Count)
		{
			GuiButton guiButton3 = buttonList[num2];
			Vector3 lhs = guiButton3.Bounds.center - bounds2.center;
			lhs.z = 0f;
			lhs.Normalize();
			if (Vector3.Dot(lhs, DirectionToVector(dir)) < Mathf.Cos((float)Math.PI / 3f))
			{
				buttonList.RemoveAt(num2);
			}
			else
			{
				num2++;
			}
		}
		int num3 = 0;
		while (num3 < buttonList.Count)
		{
			GuiButton guiButton4 = buttonList[num3];
			if (!guiButton4.Focusable || !CanTouch(guiButton4) || (guiButton4.Click == null && guiButton4.Down == null && guiButton4.Up == null && guiButton4.GetType() == typeof(GuiButton)))
			{
				buttonList.RemoveAt(num3);
			}
			else
			{
				num3++;
			}
		}
		GuiButton guiButton5 = null;
		float num4 = float.MaxValue;
		foreach (GuiButton button2 in buttonList)
		{
			Vector3 vector2 = button2.Bounds.center - bounds2.center;
			float num5 = 0f;
			num5 = ((dir != 0 && dir != Direction.Down) ? (Mathf.Abs(vector2.x) + Mathf.Abs(vector2.y) * 2f) : (Mathf.Abs(vector2.x) * 2f + Mathf.Abs(vector2.y)));
			if (button2 != FocusView && num5 < num4)
			{
				guiButton5 = button2;
				num4 = num5;
			}
		}
		if (guiButton5 != null)
		{
			UpdateFocus(guiButton5);
		}
	}

	private Vector3 DirectionToVector(Direction dir)
	{
		switch (dir)
		{
		case Direction.Up:
			return Vector3.up;
		case Direction.Down:
			return Vector3.down;
		case Direction.Left:
			return Vector3.left;
		case Direction.Right:
			return Vector3.right;
		default:
			return Vector3.up;
		}
	}

	private void SelectUp()
	{
		FocusNextButton(Direction.Up);
	}

	private void SelectDown()
	{
		FocusNextButton(Direction.Down);
	}

	private void SelectLeft()
	{
		FocusNextButton(Direction.Left);
	}

	private void SelectRight()
	{
		FocusNextButton(Direction.Right);
	}

	public void HideAll()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			GuiView component = base.transform.GetChild(i).GetComponent<GuiView>();
			if ((bool)component)
			{
				component.Hide();
			}
		}
	}
}
