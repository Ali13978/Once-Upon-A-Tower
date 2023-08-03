using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
	public class FTrack : FObject
	{
		[SerializeField]
		[HideInInspector]
		private FTimeline _timeline;

		[SerializeField]
		[HideInInspector]
		private string _evtTypeStr;

		private Type _evtType;

		[SerializeField]
		[HideInInspector]
		private List<FEvent> _events = new List<FEvent>();

		[SerializeField]
		[HideInInspector]
		private CacheMode _cacheMode;

		private int _currentEvent;

		private FTrackCache _cache;

		public List<FEvent> Events => _events;

		public bool RequiresNoCache => CacheMode == (CacheMode)0;

		public bool RequiresEditorCache => (CacheMode & CacheMode.Editor) != (CacheMode)0;

		public bool RequiresForwardCache => (CacheMode & CacheMode.RuntimeForward) != (CacheMode)0;

		public bool RequiresBackwardsCache => (CacheMode & CacheMode.RuntimeBackwards) != (CacheMode)0;

		public CacheMode CacheMode
		{
			get
			{
				return _cacheMode;
			}
			set
			{
				_cacheMode = value;
			}
		}

		public virtual CacheMode RequiredCacheMode => (CacheMode)0;

		public virtual CacheMode AllowedCacheMode => (CacheMode)0;

		public override FSequence Sequence => _timeline.Sequence;

		public override Transform Owner => _timeline.Owner;

		public FTimeline Timeline => _timeline;

		public FTrackCache Cache
		{
			get
			{
				return _cache;
			}
			set
			{
				_cache = value;
			}
		}

		public bool HasCache => _cache != null;

		public static FTrack Create<T>() where T : FEvent
		{
			Type typeFromHandle = typeof(T);
			string name = typeFromHandle.Name;
			GameObject gameObject = new GameObject(name);
			Type componentType = typeof(FTrack);
			object[] customAttributes = typeFromHandle.GetCustomAttributes(typeof(FEventAttribute), inherit: false);
			if (customAttributes.Length > 0)
			{
				componentType = ((FEventAttribute)customAttributes[0]).trackType;
			}
			FTrack fTrack = (FTrack)gameObject.AddComponent(componentType);
			fTrack.SetEventType(typeFromHandle);
			fTrack.CacheMode = fTrack.RequiredCacheMode;
			return fTrack;
		}

		internal void SetTimeline(FTimeline timeline)
		{
			_timeline = timeline;
			if ((bool)_timeline)
			{
				base.transform.parent = _timeline.transform;
			}
			else
			{
				base.transform.parent = null;
			}
		}

		public override void Init()
		{
			_currentEvent = ((!Sequence.IsPlayingForward) ? (_events.Count - 1) : 0);
			for (int i = 0; i != _events.Count; i++)
			{
				_events[i].Init();
			}
		}

		public virtual void Pause()
		{
			for (int i = 0; i != _events.Count; i++)
			{
				if (_events[i].HasTriggered && !_events[i].HasFinished)
				{
					_events[i].Pause();
				}
			}
		}

		public virtual void Resume()
		{
			for (int i = 0; i != _events.Count; i++)
			{
				if (_events[i].HasTriggered && !_events[i].HasFinished)
				{
					_events[i].Resume();
				}
			}
		}

		public override void Stop()
		{
			for (int num = _events.Count - 1; num >= 0; num--)
			{
				if (_events[num].HasTriggered)
				{
					_events[num].Stop();
				}
			}
			_currentEvent = ((!Sequence.IsPlayingForward) ? (_events.Count - 1) : 0);
		}

		public bool IsEmpty()
		{
			return _events.Count == 0;
		}

		public Type GetEventType()
		{
			if (_evtType == null)
			{
				_evtType = Type.GetType(_evtTypeStr);
			}
			return _evtType;
		}

		private void SetEventType(Type evtType)
		{
			if (!evtType.IsSubclassOf(typeof(FEvent)))
			{
				throw new ArgumentException(evtType.ToString() + " does not inherit from FEvent");
			}
			_evtType = evtType;
			_evtTypeStr = evtType.ToString();
		}

		public FEvent GetEvent(int index)
		{
			return _events[index];
		}

		public int GetEventsAt(int t, FEvent[] evtBuffer)
		{
			int num = 0;
			for (int i = 0; i != _events.Count; i++)
			{
				if (_events[i].Start <= t && _events[i].End >= t)
				{
					evtBuffer[num++] = _events[i];
				}
				else if (_events[i].Start > t)
				{
					break;
				}
			}
			return num;
		}

		public int GetEventsAt(int t, out FEvent first, out FEvent second)
		{
			int num = 0;
			first = null;
			second = null;
			for (int i = 0; i != _events.Count; i++)
			{
				if (_events[i].Start <= t && _events[i].End >= t)
				{
					if (num == 0)
					{
						first = _events[i];
					}
					else
					{
						second = _events[i];
					}
					num++;
				}
				else if (_events[i].Start > t)
				{
					break;
				}
			}
			return num;
		}

		public FEvent GetEventBefore(int t)
		{
			for (int i = 0; i != _events.Count; i++)
			{
				if (_events[i].Start >= t)
				{
					if (i > 0)
					{
						return _events[i - 1];
					}
					return null;
				}
			}
			return (_events.Count <= 0) ? null : _events[_events.Count - 1];
		}

		public FEvent GetEventAfter(int t)
		{
			for (int num = _events.Count - 1; num >= 0; num--)
			{
				if (_events[num].End <= t)
				{
					if (num < _events.Count - 1)
					{
						return _events[num + 1];
					}
					return null;
				}
			}
			return (_events.Count <= 0) ? null : _events[0];
		}

		public bool CanAdd(FEvent evt)
		{
			foreach (FEvent @event in _events)
			{
				if (@event.Start > evt.End)
				{
					break;
				}
				if (evt.Collides(@event))
				{
					return false;
				}
			}
			return true;
		}

		public bool CanAdd(FEvent evt, FrameRange newRange)
		{
			for (int i = 0; i != _events.Count && _events[i].Start <= newRange.End; i++)
			{
				if (!(_events[i] == evt) && _events[i].FrameRange.Collides(newRange))
				{
					return false;
				}
			}
			return true;
		}

		public bool CanAddAt(int t)
		{
			foreach (FEvent @event in _events)
			{
				if (@event.Start < t + 1 && @event.End > t)
				{
					return false;
				}
			}
			return true;
		}

		public bool CanAddAt(int t, out int maxLength)
		{
			maxLength = 0;
			if (t < 0)
			{
				return false;
			}
			for (int i = 0; i != _events.Count; i++)
			{
				if (_events[i].Start > t)
				{
					maxLength = _events[i].Start - t;
					return true;
				}
				if (_events[i].Start <= t && _events[i].End > t)
				{
					return false;
				}
			}
			if (t >= Sequence.Length - 1)
			{
				return false;
			}
			maxLength = Sequence.Length - t;
			return true;
		}

		public void Add(FEvent evt)
		{
			evt.SetTrack(this);
			int i = 0;
			for (int count = _events.Count; i != count; i++)
			{
				if (_events[i].Start > evt.End)
				{
					_events.Insert(i, evt);
					UpdateEventIds();
					return;
				}
			}
			evt.SetId(_events.Count);
			_events.Add(evt);
			if (!Sequence.IsStopped)
			{
				Init();
			}
		}

		public void Remove(FEvent evt)
		{
			_events.Remove(evt);
			evt.SetTrack(null);
			UpdateEventIds();
		}

		public virtual void UpdateEvents(int frame, float time)
		{
			int num = _events.Count;
			if (num == 0)
			{
				return;
			}
			int num2 = 1;
			if (!Sequence.IsPlayingForward)
			{
				num = -1;
				num2 = -1;
			}
			for (int i = _currentEvent; i != num; i += num2)
			{
				if (frame < _events[i].Start)
				{
					if (_events[i].HasTriggered)
					{
						_events[i].Stop();
					}
					if (Sequence.IsPlayingForward)
					{
						break;
					}
					_currentEvent = Mathf.Clamp(i - 1, 0, _events.Count - 1);
				}
				else if (frame >= _events[i].Start && frame <= _events[i].End)
				{
					if (_events[i].HasFinished && Sequence.FrameChanged)
					{
						_events[i].Stop();
					}
					if (!_events[i].HasFinished)
					{
						_events[i].UpdateEvent(frame - _events[i].Start, time - _events[i].StartTime);
					}
				}
				else
				{
					if (!_events[i].HasFinished && (_events[i].HasTriggered || _events[i].TriggerOnSkip))
					{
						_events[i].UpdateEvent(_events[i].Length, _events[i].LengthTime);
					}
					if (!Sequence.IsPlayingForward)
					{
						break;
					}
					_currentEvent = Mathf.Clamp(i + 1, 0, _events.Count - 1);
				}
			}
		}

		public virtual void UpdateEventsEditor(int frame, float time)
		{
			_currentEvent = ((!Sequence.IsPlayingForward) ? (_events.Count - 1) : 0);
			UpdateEvents(frame, time);
		}

		public virtual void CreateCache()
		{
		}

		public virtual void ClearCache()
		{
		}

		public virtual bool CanCreateCache()
		{
			return true;
		}

		public void Rebuild()
		{
			Transform transform = base.transform;
			_events.Clear();
			for (int i = 0; i != transform.childCount; i++)
			{
				FEvent component = transform.GetChild(i).GetComponent<FEvent>();
				if ((bool)component)
				{
					component.SetTrack(this);
					_events.Add(component);
				}
			}
			UpdateEventIds();
		}

		public void UpdateEventIds()
		{
			_events.Sort((FEvent c1, FEvent c2) => c1.FrameRange.Start.CompareTo(c2.FrameRange.Start));
			int i = 0;
			for (int count = _events.Count; i != count; i++)
			{
				_events[i].SetId(i);
			}
		}

		public FrameRange GetValidRange(FEvent evt)
		{
			int i;
			for (i = 0; i < _events.Count && !(_events[i] == evt); i++)
			{
			}
			FrameRange result = new FrameRange(0, Sequence.Length);
			if (i > 0)
			{
				result.Start = _events[i - 1].End;
			}
			if (i < _events.Count - 1)
			{
				result.End = _events[i + 1].Start;
			}
			if (result.Length > evt.GetMaxLength())
			{
				result.Length = evt.GetMaxLength();
			}
			return result;
		}
	}
}
