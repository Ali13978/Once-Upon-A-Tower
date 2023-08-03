using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
	public class FAnimationTrack : FTransformTrack
	{
		private static Dictionary<int, Dictionary<int, FAnimationTrackCache>> _animPreviews = new Dictionary<int, Dictionary<int, FAnimationTrackCache>>();

		[SerializeField]
		[HideInInspector]
		private RuntimeAnimatorController _animatorController;

		[SerializeField]
		[HideInInspector]
		private string _layerName;

		[SerializeField]
		[HideInInspector]
		private int _layerId = -1;

		public RuntimeAnimatorController AnimatorController => _animatorController;

		public string LayerName => _layerName;

		public int LayerId => _layerId;

		public override CacheMode RequiredCacheMode => CacheMode.Editor | CacheMode.RuntimeBackwards;

		public override CacheMode AllowedCacheMode => RequiredCacheMode | CacheMode.RuntimeForward;

		public static FAnimationTrackCache GetAnimationPreview(FSequence sequence, Transform owner)
		{
			Dictionary<int, FAnimationTrackCache> value = null;
			FAnimationTrackCache value2 = null;
			if (_animPreviews.TryGetValue(sequence.GetInstanceID(), out value))
			{
				value.TryGetValue(owner.GetInstanceID(), out value2);
			}
			return value2;
		}

		private static FAnimationTrackCache GetAnimationPreview(FAnimationTrack animTrack)
		{
			return GetAnimationPreview(animTrack, createIfDoesntExist: true);
		}

		private static FAnimationTrackCache GetAnimationPreview(FAnimationTrack animTrack, bool createIfDoesntExist)
		{
			Dictionary<int, FAnimationTrackCache> value = null;
			if (!_animPreviews.TryGetValue(animTrack.Sequence.GetInstanceID(), out value))
			{
				if (!createIfDoesntExist)
				{
					return null;
				}
				value = new Dictionary<int, FAnimationTrackCache>();
				_animPreviews.Add(animTrack.Sequence.GetInstanceID(), value);
			}
			FAnimationTrackCache value2 = null;
			if (!value.TryGetValue(animTrack.Owner.GetInstanceID(), out value2))
			{
				if (!createIfDoesntExist)
				{
					return null;
				}
				value2 = new FAnimationTrackCache(animTrack);
				value.Add(animTrack.Owner.GetInstanceID(), value2);
			}
			return value2;
		}

		private static void DeleteAnimationPreview(FAnimationTrack animTrack)
		{
			Dictionary<int, FAnimationTrackCache> value = null;
			if (_animPreviews.TryGetValue(animTrack.Sequence.GetInstanceID(), out value))
			{
				value[animTrack.Owner.GetInstanceID()].Clear();
				value.Remove(animTrack.Owner.GetInstanceID());
			}
		}

		public static void DeleteAnimationPreviews(FSequence sequence)
		{
			Dictionary<int, FAnimationTrackCache> value = null;
			if (_animPreviews.TryGetValue(sequence.GetInstanceID(), out value))
			{
				Dictionary<int, FAnimationTrackCache>.Enumerator enumerator = value.GetEnumerator();
				while (enumerator.MoveNext())
				{
					enumerator.Current.Value.Clear();
				}
				value.Clear();
				_animPreviews.Remove(sequence.GetInstanceID());
			}
		}

		public override void Init()
		{
			if (Owner.GetComponent<Animator>() == null)
			{
				Owner.gameObject.AddComponent<Animator>();
			}
			base.Init();
			_snapshot.TakeChildSnapshots();
		}

		public override void Stop()
		{
			if (base.HasCache && base.Cache.Track == this)
			{
				((FAnimationTrackCache)base.Cache).StopPlayback();
				Owner.GetComponent<Animator>().enabled = false;
			}
			base.Stop();
		}

		public override void UpdateEventsEditor(int frame, float time)
		{
			if (base.HasCache && base.Cache.Track == this)
			{
				GetPreviewAt(time);
			}
		}

		public override void UpdateEvents(int frame, float time)
		{
			if (Sequence.Speed != 1f && !base.HasCache)
			{
				Owner.GetComponent<Animator>().speed = Sequence.Speed;
			}
			if (base.HasCache)
			{
				if (base.Cache.Track == this)
				{
					GetPreviewAt(time);
				}
			}
			else
			{
				base.UpdateEvents(frame, time);
			}
		}

		public override void CreateCache()
		{
			FAnimationTrackCache animationPreview = GetAnimationPreview(this);
			animationPreview.Build(rebuild: true);
		}

		public override void ClearCache()
		{
			FAnimationTrackCache fAnimationTrackCache = (FAnimationTrackCache)base.Cache;
			if (fAnimationTrackCache != null)
			{
				if (fAnimationTrackCache.NumberTracksCached <= 1)
				{
					DeleteAnimationPreview(this);
				}
				else
				{
					fAnimationTrackCache.Build(rebuild: true);
				}
			}
			base.Cache = null;
		}

		public override bool CanCreateCache()
		{
			if (_animatorController == null)
			{
				return false;
			}
			List<FEvent> events = base.Events;
			for (int i = 0; i != events.Count; i++)
			{
				if (((FPlayAnimationEvent)events[i])._animationClip == null)
				{
					return false;
				}
			}
			return true;
		}

		private void GetPreviewAt(float time)
		{
			base.Cache.GetPlaybackAt(time);
		}

		private bool HasAnimationOnFrame(int frame)
		{
			FEvent[] array = new FEvent[2];
			int eventsAt = GetEventsAt(frame, array);
			if (eventsAt == 0)
			{
				return false;
			}
			return ((FPlayAnimationEvent)array[0])._animationClip != null || (eventsAt == 2 && ((FPlayAnimationEvent)array[1])._animationClip != null);
		}
	}
}
