using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
	public class FContainer : FObject
	{
		public static readonly Color DEFAULT_COLOR = new Color(0.14f, 0.14f, 0.14f, 0.7f);

		[SerializeField]
		private FSequence _sequence;

		[SerializeField]
		private Color _color;

		[SerializeField]
		private List<FTimeline> _timelines = new List<FTimeline>();

		public Color Color
		{
			get
			{
				return _color;
			}
			set
			{
				_color = value;
			}
		}

		public List<FTimeline> Timelines => _timelines;

		public override FSequence Sequence => _sequence;

		public override Transform Owner => null;

		public static FContainer Create(Color color)
		{
			GameObject gameObject = new GameObject("Default");
			FContainer fContainer = gameObject.AddComponent<FContainer>();
			fContainer.Color = color;
			return fContainer;
		}

		internal void SetSequence(FSequence sequence)
		{
			_sequence = sequence;
			if ((bool)_sequence)
			{
				base.transform.parent = _sequence.Content;
			}
			else
			{
				base.transform.parent = null;
			}
		}

		public override void Init()
		{
			foreach (FTimeline timeline in _timelines)
			{
				timeline.Init();
			}
		}

		public override void Stop()
		{
			foreach (FTimeline timeline in _timelines)
			{
				timeline.Stop();
			}
		}

		public void Resume()
		{
			foreach (FTimeline timeline in _timelines)
			{
				timeline.Resume();
			}
		}

		public void Pause()
		{
			foreach (FTimeline timeline in _timelines)
			{
				timeline.Pause();
			}
		}

		public bool IsEmpty()
		{
			foreach (FTimeline timeline in _timelines)
			{
				if (!timeline.IsEmpty())
				{
					return false;
				}
			}
			return true;
		}

		public void UpdateTimelines(int frame, float time)
		{
			for (int i = 0; i != _timelines.Count; i++)
			{
				if (_timelines[i].enabled)
				{
					_timelines[i].UpdateTracks(frame, time);
				}
			}
		}

		public void UpdateTimelinesEditor(int frame, float time)
		{
			for (int i = 0; i != _timelines.Count; i++)
			{
				if (_timelines[i].enabled)
				{
					_timelines[i].UpdateTracksEditor(frame, time);
				}
			}
		}

		public void Add(FTimeline timeline)
		{
			int count = _timelines.Count;
			_timelines.Add(timeline);
			timeline.SetId(count);
			timeline.SetContainer(this);
		}

		public void Remove(FTimeline timeline)
		{
			int num = 0;
			while (true)
			{
				if (num != _timelines.Count)
				{
					if (_timelines[num] == timeline)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			Remove(num);
		}

		public void Remove(int id)
		{
			FTimeline fTimeline = _timelines[id];
			_timelines.RemoveAt(id);
			fTimeline.SetContainer(null);
			UpdateTimelineIds();
		}

		public void Rebuild()
		{
			_timelines.Clear();
			Transform transform = base.transform;
			for (int i = 0; i != transform.childCount; i++)
			{
				FTimeline component = transform.GetChild(i).GetComponent<FTimeline>();
				if (component != null)
				{
					_timelines.Add(component);
					component.SetContainer(this);
					component.Rebuild();
				}
			}
			UpdateTimelineIds();
		}

		private void UpdateTimelineIds()
		{
			for (int i = 0; i != _timelines.Count; i++)
			{
				_timelines[i].SetId(i);
			}
		}
	}
}
