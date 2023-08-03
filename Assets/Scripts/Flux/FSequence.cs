using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
	public class FSequence : FObject
	{
		public const int DEFAULT_FRAMES_PER_SECOND = 60;

		public const int DEFAULT_LENGTH = 10;

		public const float DEFAULT_SPEED = 1f;

		[SerializeField]
		private Transform _content;

		[SerializeField]
		private List<FContainer> _containers = new List<FContainer>();

		[SerializeField]
		[HideInInspector]
		private int _version;

		[SerializeField]
		[Tooltip("What should be the default action when Start() gets called?\n\tNone\n\tInitialize\n\tPlay")]
		private ActionOnStart _actionOnStart;

		[SerializeField]
		private bool _loop;

		[SerializeField]
		private float _defaultSpeed = 1f;

		private float _speed = 1f;

		[SerializeField]
		private AnimatorUpdateMode _updateMode;

		[SerializeField]
		private int _length = 600;

		[SerializeField]
		[HideInInspector]
		private SequenceFinishedEvent _onFinishedCallback = new SequenceFinishedEvent();

		[SerializeField]
		[HideInInspector]
		private int _frameRate = 60;

		[SerializeField]
		[HideInInspector]
		private float _inverseFrameRate = 0.0166666675f;

		private bool _isInit;

		private bool _isPlaying;

		private bool _isPlayingForward = true;

		private float _lastUpdateTime;

		private int _currentFrame = -1;

		private int _lastFrame = -1;

		private float _currentTime = -1f;

		public Transform Content
		{
			get
			{
				return _content;
			}
			set
			{
				_content = value;
				_content.parent = base.transform;
			}
		}

		public List<FContainer> Containers => _containers;

		public int Version
		{
			get
			{
				return _version;
			}
			set
			{
				_version = value;
			}
		}

		public ActionOnStart ActionOnStart
		{
			get
			{
				return _actionOnStart;
			}
			set
			{
				_actionOnStart = value;
			}
		}

		public bool Loop
		{
			get
			{
				return _loop;
			}
			set
			{
				_loop = value;
			}
		}

		public float DefaultSpeed
		{
			get
			{
				return _defaultSpeed;
			}
			set
			{
				_defaultSpeed = value;
			}
		}

		public float Speed
		{
			get
			{
				return _speed;
			}
			set
			{
				bool isPlayingForward = _isPlayingForward;
				_speed = value;
				_isPlayingForward = (Speed * Time.timeScale >= 0f);
				if (isPlayingForward != _isPlayingForward && Application.isPlaying && IsInit)
				{
					CreateTrackCaches();
				}
			}
		}

		public AnimatorUpdateMode UpdateMode
		{
			get
			{
				return _updateMode;
			}
			set
			{
				_updateMode = value;
			}
		}

		public int Length
		{
			get
			{
				return _length;
			}
			set
			{
				_length = value;
			}
		}

		public float LengthTime => (float)_length * _inverseFrameRate;

		public SequenceFinishedEvent OnFinishedCallback => _onFinishedCallback;

		public int FrameRate
		{
			get
			{
				return _frameRate;
			}
			set
			{
				_frameRate = value;
				_inverseFrameRate = 1f / (float)_frameRate;
			}
		}

		public float InverseFrameRate => _inverseFrameRate;

		public bool IsInit => _isInit;

		public bool IsPlaying => _isPlaying;

		public bool IsPlayingForward => _isPlayingForward;

		public override Transform Owner => base.transform;

		public override FSequence Sequence => this;

		public int CurrentFrame
		{
			get
			{
				return _currentFrame;
			}
			private set
			{
				_lastFrame = ((value >= 0) ? _currentFrame : (-1));
				_currentFrame = value;
			}
		}

		public float CurrentTime
		{
			get
			{
				return _currentTime;
			}
			private set
			{
				_currentTime = value;
				if (_currentTime < 0f)
				{
					CurrentFrame = -1;
					return;
				}
				float num = 0.001f;
				CurrentFrame = ((!IsPlayingForward) ? Mathf.CeilToInt(_currentTime * (float)_frameRate - num) : Mathf.FloorToInt(_currentTime * (float)_frameRate + num));
			}
		}

		public bool FrameChanged => _lastFrame != _currentFrame;

		public bool IsPaused => !_isPlaying && _currentFrame >= 0;

		public bool IsStopped => _currentFrame < 0;

		public bool IsFinished => IsPaused && ((IsPlayingForward && _currentTime == LengthTime) || (!IsPlayingForward && _currentTime == 0f));

		public static FSequence CreateSequence()
		{
			return CreateSequence(new GameObject("FSequence"));
		}

		public static FSequence CreateSequence(GameObject gameObject)
		{
			FSequence fSequence = gameObject.AddComponent<FSequence>();
			fSequence._content = new GameObject("SequenceContent").transform;
			fSequence._content.hideFlags |= HideFlags.HideInHierarchy;
			fSequence._content.parent = fSequence.transform;
			fSequence.Add(FContainer.Create(FContainer.DEFAULT_COLOR));
			fSequence.Version = 210;
			return fSequence;
		}

		public void Add(FContainer container)
		{
			int count = _containers.Count;
			_containers.Add(container);
			container.SetId(count);
			container.SetSequence(this);
		}

		public void Remove(FContainer container)
		{
			if (_containers.Remove(container))
			{
				container.SetSequence(null);
				UpdateContainerIds();
			}
		}

		public void SetCurrentTimeEditor(float time)
		{
			if (time != _currentTime && _currentFrame != -1)
			{
				Speed = ((!(time > _currentTime)) ? (0f - Mathf.Abs(Speed)) : Mathf.Abs(Speed));
			}
			CurrentTime = Mathf.Clamp(time, 0f, LengthTime);
			for (int i = 0; i != _containers.Count; i++)
			{
				if (_containers[i].enabled)
				{
					_containers[i].UpdateTimelinesEditor(_currentFrame, _currentTime);
				}
			}
		}

		public void SetCurrentTime(float time)
		{
			if (!_isInit)
			{
				Init();
			}
			if (time != _currentTime && _currentFrame != -1 && (!IsPlayingForward || !(time > _currentTime)))
			{
				for (int i = 0; i != _containers.Count; i++)
				{
					_containers[i].Stop();
				}
			}
			SetCurrentTimeInternal(time);
		}

		public void SetCurrentFrame(int frame)
		{
			SetCurrentTime((float)frame * InverseFrameRate);
		}

		protected void SetCurrentTimeInternal(float time)
		{
			CurrentTime = Mathf.Clamp(time, 0f, LengthTime);
			for (int i = 0; i != _containers.Count; i++)
			{
				if (_containers[i].enabled)
				{
					_containers[i].UpdateTimelines(_currentFrame, _currentTime);
				}
			}
		}

		public override void Init()
		{
			_isInit = true;
			for (int i = 0; i != _containers.Count; i++)
			{
				_containers[i].Init();
			}
			CreateTrackCaches();
			_isInit = true;
		}

		public void CreateTrackCaches()
		{
			for (int i = 0; i != _containers.Count; i++)
			{
				List<FTimeline> timelines = _containers[i].Timelines;
				for (int j = 0; j != timelines.Count; j++)
				{
					List<FTrack> tracks = timelines[j].Tracks;
					foreach (FTrack item in tracks)
					{
						if (((IsPlayingForward && item.RequiresForwardCache) || (!IsPlayingForward && item.RequiresBackwardsCache)) && !item.HasCache)
						{
							item.CreateCache();
						}
					}
				}
			}
		}

		public void DestroyTrackCaches()
		{
			foreach (FContainer container in Containers)
			{
				foreach (FTimeline timeline in container.Timelines)
				{
					foreach (FTrack track in timeline.Tracks)
					{
						if (track.HasCache)
						{
							track.ClearCache();
						}
					}
				}
			}
		}

		public void Play(int startFrame)
		{
			Play((float)startFrame * _inverseFrameRate);
		}

		public void Play(float startTime)
		{
			if (!_isPlaying)
			{
				_isPlayingForward = (Speed * Time.timeScale >= 0f);
				if (!_isInit)
				{
					Init();
				}
				if (!IsStopped)
				{
					Resume();
				}
				_isPlaying = true;
				switch (_updateMode)
				{
				case AnimatorUpdateMode.Normal:
					_lastUpdateTime = Time.time;
					break;
				case AnimatorUpdateMode.AnimatePhysics:
					_lastUpdateTime = Time.fixedTime;
					break;
				case AnimatorUpdateMode.UnscaledTime:
					_lastUpdateTime = Time.unscaledTime;
					break;
				default:
					UnityEngine.Debug.LogError("Unsupported Update Mode");
					_lastUpdateTime = Time.time;
					break;
				}
				SetCurrentTimeInternal(startTime);
			}
		}

		public void Play()
		{
			if (IsPlayingForward)
			{
				Play(0f);
			}
			else
			{
				Play(LengthTime);
			}
		}

		public override void Stop()
		{
			Stop(reset: false);
		}

		public void Stop(bool reset)
		{
			if (reset)
			{
				_isInit = false;
			}
			if (!IsStopped || reset)
			{
				_isPlaying = false;
				_isPlayingForward = (Speed * Time.timeScale >= 0f);
				for (int i = 0; i != _containers.Count; i++)
				{
					_containers[i].Stop();
				}
				CurrentTime = -1f;
				_lastUpdateTime = 0f;
			}
		}

		public void Pause()
		{
			if (_isPlaying)
			{
				_isPlaying = false;
				for (int i = 0; i != _containers.Count; i++)
				{
					_containers[i].Pause();
				}
			}
		}

		public void Resume()
		{
			if (!_isPlaying)
			{
				_isPlaying = true;
				for (int i = 0; i != _containers.Count; i++)
				{
					_containers[i].Resume();
				}
				switch (_updateMode)
				{
				case AnimatorUpdateMode.Normal:
					_lastUpdateTime = Time.time;
					break;
				case AnimatorUpdateMode.AnimatePhysics:
					_lastUpdateTime = Time.fixedTime;
					break;
				case AnimatorUpdateMode.UnscaledTime:
					_lastUpdateTime = Time.unscaledTime;
					break;
				default:
					UnityEngine.Debug.LogError("Unsupported Update Mode");
					_lastUpdateTime = Time.time;
					break;
				}
			}
		}

		public bool IsEmpty()
		{
			foreach (FContainer container in _containers)
			{
				if (!container.IsEmpty())
				{
					return false;
				}
			}
			return true;
		}

		public bool HasTimelines()
		{
			foreach (FContainer container in _containers)
			{
				if (container.Timelines.Count > 0)
				{
					return true;
				}
			}
			return false;
		}

		protected virtual void Awake()
		{
			Speed = DefaultSpeed;
			_isPlayingForward = (Speed * Time.timeScale >= 0f);
		}

		protected virtual void Start()
		{
			switch (_actionOnStart)
			{
			case ActionOnStart.Initialize:
				Init();
				break;
			case ActionOnStart.Play:
				Play();
				break;
			}
		}

		protected virtual void Update()
		{
			if (_updateMode != AnimatorUpdateMode.AnimatePhysics && _isPlaying)
			{
				InternalUpdate((_updateMode != 0) ? Time.unscaledTime : Time.time);
			}
		}

		protected virtual void FixedUpdate()
		{
			if (_updateMode == AnimatorUpdateMode.AnimatePhysics && _isPlaying)
			{
				InternalUpdate(Time.fixedTime);
			}
		}

		protected virtual void InternalUpdate(float time)
		{
			float num = time - _lastUpdateTime;
			if (num == 0f)
			{
				return;
			}
			SetCurrentTimeInternal(_currentTime + num * Speed);
			if (_isPlayingForward)
			{
				if (_currentTime == LengthTime)
				{
					OnFinishedCallback.Invoke(this);
					if (_loop)
					{
						Stop();
						Play();
					}
					else
					{
						Pause();
					}
				}
			}
			else if (_currentTime == 0f)
			{
				OnFinishedCallback.Invoke(this);
				if (_loop)
				{
					Stop();
					Play();
				}
				else
				{
					Pause();
				}
			}
			_lastUpdateTime = time;
		}

		public void Rebuild()
		{
			_containers.Clear();
			Transform content = Content;
			for (int i = 0; i != content.childCount; i++)
			{
				FContainer component = content.GetChild(i).GetComponent<FContainer>();
				if (component != null)
				{
					_containers.Add(component);
					component.SetSequence(this);
					component.Rebuild();
				}
			}
			UpdateContainerIds();
		}

		private void UpdateContainerIds()
		{
			for (int i = 0; i != _containers.Count; i++)
			{
				_containers[i].SetId(i);
			}
		}

		public void ReplaceOwner(Transform oldOwner, Transform newOwner)
		{
			foreach (FContainer container in _containers)
			{
				foreach (FTimeline timeline in container.Timelines)
				{
					if (timeline.Owner == oldOwner)
					{
						timeline.SetOwner(newOwner);
					}
				}
			}
		}

		public void ReplaceOwner(string timelineName, Transform newOwner)
		{
			foreach (FContainer container in _containers)
			{
				foreach (FTimeline timeline in container.Timelines)
				{
					if (timeline.gameObject.name == timelineName)
					{
						timeline.SetOwner(newOwner);
					}
				}
			}
		}

		public void ReplaceOwnerByPath(string ownerPath, Transform newOwner)
		{
			foreach (FContainer container in _containers)
			{
				foreach (FTimeline timeline in container.Timelines)
				{
					if (timeline.OwnerPath == ownerPath)
					{
						timeline.SetOwner(newOwner);
					}
				}
			}
		}

		public void ReplaceOwner(params KeyValuePair<Transform, Transform>[] replacements)
		{
			foreach (FContainer container in _containers)
			{
				foreach (FTimeline timeline in container.Timelines)
				{
					for (int i = 0; i < replacements.Length; i++)
					{
						KeyValuePair<Transform, Transform> keyValuePair = replacements[i];
						if (timeline.Owner == keyValuePair.Key)
						{
							timeline.SetOwner(keyValuePair.Value);
							break;
						}
					}
				}
			}
		}
	}
}
