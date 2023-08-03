using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Flux
{
	public class FTimeline : FObject
	{
		[SerializeField]
		private FContainer _container;

		[SerializeField]
		private Transform _owner;

		[SerializeField]
		private string _ownerPath;

		[SerializeField]
		private List<FTrack> _tracks = new List<FTrack>();

		public FContainer Container => _container;

		public string OwnerPath => _ownerPath;

		public override FSequence Sequence => _container.Sequence;

		public override Transform Owner => _owner;

		public List<FTrack> Tracks => _tracks;

		public void Awake()
		{
			if (_owner == null && !string.IsNullOrEmpty(_ownerPath))
			{
				_owner = base.transform.Find(_ownerPath);
			}
		}

		public void SetOwner(Transform owner)
		{
			_owner = owner;
			if (_owner != null)
			{
				base.name = _owner.name;
			}
			OnValidate();
			if (Container != null && Sequence.IsInit)
			{
				Init();
			}
		}

		internal void SetContainer(FContainer container)
		{
			_container = container;
			if ((bool)_container)
			{
				base.transform.parent = container.transform;
			}
			else
			{
				base.transform.parent = null;
			}
			OnValidate();
		}

		public static FTimeline Create(Transform owner)
		{
			GameObject gameObject = new GameObject(owner.name);
			FTimeline fTimeline = gameObject.AddComponent<FTimeline>();
			fTimeline.SetOwner(owner);
			return fTimeline;
		}

		public override void Init()
		{
			if (_owner == null)
			{
				Awake();
			}
			base.enabled = (Owner != null);
			if (base.enabled)
			{
				for (int i = 0; i != _tracks.Count; i++)
				{
					_tracks[i].Init();
				}
			}
		}

		public void Pause()
		{
			for (int i = 0; i != _tracks.Count; i++)
			{
				_tracks[i].Pause();
			}
		}

		public void Resume()
		{
			for (int i = 0; i != _tracks.Count; i++)
			{
				_tracks[i].Resume();
			}
		}

		public override void Stop()
		{
			for (int i = 0; i != _tracks.Count; i++)
			{
				_tracks[i].Stop();
			}
		}

		public bool IsEmpty()
		{
			foreach (FTrack track in _tracks)
			{
				if (!track.IsEmpty())
				{
					return false;
				}
			}
			return true;
		}

		public FTrack Add<T>(FrameRange range) where T : FEvent
		{
			FTrack fTrack = FTrack.Create<T>();
			Add(fTrack);
			FEvent evt = FEvent.Create<T>(range);
			fTrack.Add(evt);
			return fTrack;
		}

		public void Add(FTrack track)
		{
			int count = _tracks.Count;
			_tracks.Add(track);
			track.SetTimeline(this);
			track.SetId(count);
			if (!Sequence.IsStopped)
			{
				track.Init();
			}
		}

		public void Remove(FTrack track)
		{
			if (_tracks.Remove(track))
			{
				track.SetTimeline(null);
				UpdateTrackIds();
			}
		}

		public void UpdateTracks(int frame, float time)
		{
			for (int i = 0; i != _tracks.Count; i++)
			{
				if (_tracks[i].enabled)
				{
					_tracks[i].UpdateEvents(frame, time);
				}
			}
		}

		public void UpdateTracksEditor(int frame, float time)
		{
			for (int i = 0; i != _tracks.Count; i++)
			{
				if (_tracks[i].enabled)
				{
					_tracks[i].UpdateEventsEditor(frame, time);
				}
			}
		}

		public void Rebuild()
		{
			Transform transform = base.transform;
			_tracks.Clear();
			for (int i = 0; i != transform.childCount; i++)
			{
				FTrack component = transform.GetChild(i).GetComponent<FTrack>();
				if ((bool)component)
				{
					_tracks.Add(component);
					component.SetTimeline(this);
					component.Rebuild();
				}
			}
			UpdateTrackIds();
		}

		private void UpdateTrackIds()
		{
			for (int i = 0; i != _tracks.Count; i++)
			{
				_tracks[i].SetId(i);
			}
		}

		protected virtual void OnValidate()
		{
			if (_owner != null)
			{
				_ownerPath = GetTransformPath(_owner);
			}
		}

		private string GetTransformPath(Transform t)
		{
			StringBuilder stringBuilder = new StringBuilder(t.name);
			if (Container == null || Sequence == null)
			{
				return string.Empty;
			}
			Transform transform = Sequence.transform;
			t = t.parent;
			while (t != null)
			{
				if (t == transform)
				{
					return stringBuilder.ToString();
				}
				stringBuilder.Insert(0, $"{t.name}/");
				t = t.parent;
			}
			stringBuilder.Insert(0, '/');
			return stringBuilder.ToString();
		}
	}
}
