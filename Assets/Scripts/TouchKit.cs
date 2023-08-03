using System.Collections.Generic;
using UnityEngine;

public class TouchKit : MonoBehaviour
{
	[HideInInspector]
	public bool simulateTouches = true;

	[HideInInspector]
	public bool simulateMultitouch = true;

	[HideInInspector]
	public bool drawTouches;

	[HideInInspector]
	public bool drawDebugBoundaryFrames;

	public bool autoScaleRectsAndDistances = true;

	public bool shouldAutoUpdateTouches = true;

	private Vector2 _designTimeResolution = new Vector2(320f, 180f);

	public int maxTouchesToProcess = 2;

	private List<TKAbstractGestureRecognizer> _gestureRecognizers = new List<TKAbstractGestureRecognizer>(5);

	private TKTouch[] _touchCache;

	private List<TKTouch> _liveTouches = new List<TKTouch>(2);

	private bool _shouldCheckForLostTouches;

	private const float inchesToCentimeters = 2.54f;

	private static TouchKit _instance;

	public Vector2 designTimeResolution
	{
		get
		{
			return _designTimeResolution;
		}
		set
		{
			_designTimeResolution = value;
			setupRuntimeScale();
		}
	}

	public Vector2 runtimeScaleModifier
	{
		get;
		private set;
	}

	public float runtimeDistanceModifier
	{
		get;
		private set;
	}

	public Vector2 pixelsToUnityUnitsMultiplier
	{
		get;
		private set;
	}

	public float ScreenPixelsPerCm
	{
		get
		{
			float num = 72f;
			num = 160f;
			return (Screen.dpi != 0f) ? (Screen.dpi / 2.54f) : (num / 2.54f);
		}
	}

	public static TouchKit instance
	{
		get
		{
			if (!_instance)
			{
				_instance = (UnityEngine.Object.FindObjectOfType(typeof(TouchKit)) as TouchKit);
				if (!_instance)
				{
					GameObject gameObject = new GameObject("TouchKit");
					_instance = gameObject.AddComponent<TouchKit>();
					Object.DontDestroyOnLoad(gameObject);
				}
				Camera camera = Camera.main ?? Camera.allCameras[0];
				if (camera.orthographic)
				{
					setupPixelsToUnityUnitsMultiplierWithCamera(camera);
				}
				else
				{
					_instance.pixelsToUnityUnitsMultiplier = Vector2.one;
				}
				_instance.setupRuntimeScale();
			}
			return _instance;
		}
	}

	protected void setupRuntimeScale()
	{
		TouchKit instance = _instance;
		float num = Screen.width;
		Vector2 designTimeResolution = _instance.designTimeResolution;
		float x = num / designTimeResolution.x;
		float num2 = Screen.height;
		Vector2 designTimeResolution2 = _instance.designTimeResolution;
		instance.runtimeScaleModifier = new Vector2(x, num2 / designTimeResolution2.y);
		TouchKit instance2 = _instance;
		Vector2 runtimeScaleModifier = _instance.runtimeScaleModifier;
		float x2 = runtimeScaleModifier.x;
		Vector2 runtimeScaleModifier2 = _instance.runtimeScaleModifier;
		instance2.runtimeDistanceModifier = (x2 + runtimeScaleModifier2.y) / 2f;
		if (!_instance.autoScaleRectsAndDistances)
		{
			_instance.runtimeScaleModifier = Vector2.one;
			_instance.runtimeDistanceModifier = 1f;
		}
	}

	private void addTouchesUnityForgotToEndToLiveTouchesList()
	{
		for (int i = 0; i < _touchCache.Length; i++)
		{
			if (_touchCache[i].phase != TouchPhase.Ended)
			{
				UnityEngine.Debug.LogWarning("found touch Unity forgot to end with phase: " + _touchCache[i].phase);
				_touchCache[i].phase = TouchPhase.Ended;
				_liveTouches.Add(_touchCache[i]);
			}
		}
	}

	private void internalUpdateTouches()
	{
		if (UnityEngine.Input.touchCount > 0)
		{
			_shouldCheckForLostTouches = true;
			int num = Mathf.Min(Input.touches.Length, maxTouchesToProcess);
			for (int i = 0; i < num; i++)
			{
				Touch touch = Input.touches[i];
				if (touch.fingerId < maxTouchesToProcess)
				{
					_liveTouches.Add(_touchCache[touch.fingerId].populateWithTouch(touch));
				}
			}
		}
		else if (_shouldCheckForLostTouches)
		{
			addTouchesUnityForgotToEndToLiveTouchesList();
			_shouldCheckForLostTouches = false;
		}
		if (_liveTouches.Count > 0)
		{
			for (int j = 0; j < _gestureRecognizers.Count; j++)
			{
				_gestureRecognizers[j].recognizeTouches(_liveTouches);
			}
			_liveTouches.Clear();
		}
	}

	private void Awake()
	{
		_touchCache = new TKTouch[maxTouchesToProcess];
		for (int i = 0; i < maxTouchesToProcess; i++)
		{
			_touchCache[i] = new TKTouch(i);
		}
	}

	private void Update()
	{
		if (shouldAutoUpdateTouches)
		{
			internalUpdateTouches();
		}
	}

	private void OnApplicationQuit()
	{
		_instance = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public static void setupPixelsToUnityUnitsMultiplierWithCamera(Camera cam)
	{
		if (!cam.orthographic)
		{
			UnityEngine.Debug.LogError("Attempting to setup unity pixel-to-units modifier with a non-orthographic camera");
			return;
		}
		Vector2 vector = new Vector2(cam.aspect * cam.orthographicSize * 2f, cam.orthographicSize * 2f);
		_instance.pixelsToUnityUnitsMultiplier = new Vector2(vector.x / (float)Screen.width, vector.y / (float)Screen.height);
	}

	public static void updateTouches()
	{
		if (!(_instance == null))
		{
			_instance.internalUpdateTouches();
		}
	}

	public static void addGestureRecognizer(TKAbstractGestureRecognizer recognizer)
	{
		instance._gestureRecognizers.Add(recognizer);
		if (recognizer.zIndex != 0)
		{
			_instance._gestureRecognizers.Sort();
			_instance._gestureRecognizers.Reverse();
		}
	}

	public static void removeGestureRecognizer(TKAbstractGestureRecognizer recognizer)
	{
		if (!(_instance == null))
		{
			if (!_instance._gestureRecognizers.Contains(recognizer))
			{
				UnityEngine.Debug.LogError("Trying to remove gesture recognizer that has not been added: " + recognizer);
				return;
			}
			recognizer.reset();
			instance._gestureRecognizers.Remove(recognizer);
		}
	}

	public static void removeAllGestureRecognizers()
	{
		if (!(_instance == null))
		{
			instance._gestureRecognizers.Clear();
		}
	}
}
