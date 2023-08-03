using System;
using UnityEngine;

namespace Flux
{
	public class FEvent : FObject
	{
		[SerializeField]
		[HideInInspector]
		protected FTrack _track;

		[SerializeField]
		[HideInInspector]
		private bool _triggerOnSkip = true;

		[SerializeField]
		[HideInInspector]
		private FrameRange _frameRange;

		private bool _hasTriggered;

		private bool _hasFinished;

		public override Transform Owner => _track.Owner;

		public override FSequence Sequence => _track.Sequence;

		public FTrack Track => _track;

		public bool TriggerOnSkip
		{
			get
			{
				return _triggerOnSkip;
			}
			set
			{
				_triggerOnSkip = value;
			}
		}

		public FrameRange FrameRange
		{
			get
			{
				return _frameRange;
			}
			set
			{
				FrameRange frameRange = _frameRange;
				_frameRange = value;
				OnFrameRangeChanged(frameRange);
			}
		}

		public bool HasTriggered => _hasTriggered;

		public bool HasFinished => _hasFinished;

		public virtual string Text
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public int TriggerFrame => _frameRange.Start;

		public float TriggerTime => (float)_frameRange.Start * Sequence.InverseFrameRate;

		public bool IsFirstEvent => GetId() == 0;

		public bool IsLastEvent => GetId() == _track.Events.Count - 1;

		public int Start
		{
			get
			{
				return _frameRange.Start;
			}
			set
			{
				_frameRange.Start = value;
			}
		}

		public int End
		{
			get
			{
				return _frameRange.End;
			}
			set
			{
				_frameRange.End = value;
			}
		}

		public int Length
		{
			get
			{
				return _frameRange.Length;
			}
			set
			{
				_frameRange.Length = value;
			}
		}

		public float StartTime => (float)_frameRange.Start * Sequence.InverseFrameRate;

		public float EndTime => (float)_frameRange.End * Sequence.InverseFrameRate;

		public float LengthTime => (float)_frameRange.Length * Sequence.InverseFrameRate;

		public static T Create<T>(FrameRange range) where T : FEvent
		{
			GameObject gameObject = new GameObject(typeof(T).ToString());
			T val = gameObject.AddComponent<T>();
			val._frameRange = new FrameRange(range.Start, range.End);
			val.SetDefaultValues();
			return val;
		}

		public static FEvent Create(Type evtType, FrameRange range)
		{
			GameObject gameObject = new GameObject(evtType.ToString());
			FEvent fEvent = (FEvent)gameObject.AddComponent(evtType);
			fEvent._frameRange = new FrameRange(range.Start, range.End);
			fEvent.SetDefaultValues();
			return fEvent;
		}

		internal void SetTrack(FTrack track)
		{
			_track = track;
			if ((bool)_track)
			{
				base.transform.parent = _track.transform;
			}
			else
			{
				base.transform.parent = null;
			}
		}

		protected virtual void SetDefaultValues()
		{
		}

		protected virtual void OnFrameRangeChanged(FrameRange oldFrameRange)
		{
		}

		public void Trigger(float timeSinceTrigger)
		{
			_hasTriggered = true;
			OnTrigger(timeSinceTrigger);
		}

		protected virtual void OnTrigger(float timeSinceTrigger)
		{
		}

		public void Finish()
		{
			_hasFinished = true;
			if (Sequence.IsPlayingForward)
			{
				OnFinish();
			}
		}

		protected virtual void OnFinish()
		{
		}

		public sealed override void Init()
		{
			_hasTriggered = false;
			_hasFinished = false;
			OnInit();
		}

		protected virtual void OnInit()
		{
		}

		public void Pause()
		{
			OnPause();
		}

		protected virtual void OnPause()
		{
		}

		public void Resume()
		{
			OnResume();
		}

		protected virtual void OnResume()
		{
		}

		public sealed override void Stop()
		{
			_hasTriggered = false;
			_hasFinished = false;
			OnStop();
		}

		protected virtual void OnStop()
		{
		}

		public void UpdateEvent(int framesSinceTrigger, float timeSinceTrigger)
		{
			if (!_hasTriggered)
			{
				Trigger(timeSinceTrigger);
			}
			OnUpdateEvent(timeSinceTrigger);
			if (framesSinceTrigger == Length)
			{
				Finish();
			}
		}

		protected virtual void OnUpdateEvent(float timeSinceTrigger)
		{
		}

		protected virtual void PreEvent()
		{
		}

		protected virtual void PostEvent()
		{
		}

		public virtual int GetMinLength()
		{
			return 1;
		}

		public virtual int GetMaxLength()
		{
			return int.MaxValue;
		}

		public bool Collides(FEvent e)
		{
			return _frameRange.Collides(e.FrameRange);
		}

		public FrameRange GetMaxFrameRange()
		{
			FrameRange result = new FrameRange(0, 0);
			int id = GetId();
			if (id == 0)
			{
				result.Start = 0;
			}
			else
			{
				result.Start = _track.Events[id - 1].End;
			}
			if (id == _track.Events.Count - 1)
			{
				result.End = _track.Timeline.Sequence.Length;
			}
			else
			{
				result.End = _track.Events[id + 1].Start;
			}
			return result;
		}

		public static int Compare(FEvent e1, FEvent e2)
		{
			return e1.Start.CompareTo(e2.Start);
		}
	}
}
