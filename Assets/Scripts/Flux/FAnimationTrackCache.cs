using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
	public class FAnimationTrackCache : FTrackCache
	{
		private Animator _animator;

		private bool _inPlayback;

		private List<FAnimationTrack> _tracksCached = new List<FAnimationTrack>();

		private List<FrameRange> _validFrameRanges = new List<FrameRange>();

		public Animator Animator
		{
			get
			{
				if (_animator == null)
				{
					_animator = base.Track.Owner.GetComponent<Animator>();
				}
				return _animator;
			}
		}

		public bool InPlayback => _inPlayback;

		public int NumberTracksCached => _tracksCached.Count;

		public FAnimationTrackCache(FTrack track)
			: base(track)
		{
		}

		protected override bool BuildInternal()
		{
			List<FTrack> tracksToUpdate = GetTracksToUpdate(out _tracksCached);
			_validFrameRanges.Clear();
			if (_tracksCached.Count == 0 || Animator == null || Animator.runtimeAnimatorController == null)
			{
				return false;
			}
			TransformSnapshot transformSnapshot = new TransformSnapshot(base.Track.Owner);
			FSequence sequence = base.Track.Sequence;
			if (_tracksCached[0].Snapshot != null)
			{
				_tracksCached[0].Snapshot.Restore();
			}
			bool activeSelf = base.Track.Owner.gameObject.activeSelf;
			float speed = sequence.Speed;
			if (speed != 1f)
			{
				sequence.Speed = 1f;
			}
			Animator.speed = 1f;
			int currentFrame = sequence.CurrentFrame;
			bool isPlaying = sequence.IsPlaying;
			if (!sequence.IsStopped)
			{
				sequence.Stop();
			}
			if (!sequence.IsInit)
			{
				sequence.Init();
			}
			if (!activeSelf)
			{
				HideFlags hideFlags = base.Track.Owner.gameObject.hideFlags;
				base.Track.Owner.gameObject.hideFlags |= HideFlags.DontSave;
				base.Track.Owner.gameObject.SetActive(value: true);
				base.Track.Owner.gameObject.hideFlags = hideFlags;
			}
			FrameRange item = default(FrameRange);
			foreach (FAnimationTrack item2 in _tracksCached)
			{
				item2.Cache = null;
				item2.Stop();
			}
			AnimatorCullingMode cullingMode = Animator.cullingMode;
			Animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			Animator.enabled = true;
			for (int i = 0; i != Animator.layerCount; i++)
			{
				Animator.SetLayerWeight(i, 0f);
			}
			Animator.StartRecording(-1);
			bool flag = Animator.recorderMode == AnimatorRecorderMode.Record;
			if (flag)
			{
				Animator.enabled = false;
				float num = 1f / (float)sequence.FrameRate;
				for (int j = 0; j <= sequence.Length; j++)
				{
					bool enabled = Animator.enabled;
					foreach (FTrack item3 in tracksToUpdate)
					{
						item3.UpdateEvents(j, (float)j * num);
					}
					if (enabled)
					{
						item.End++;
						if (Animator.enabled)
						{
							Animator.Update(num);
							continue;
						}
						Animator.enabled = true;
						Animator.Update(num);
						Animator.enabled = false;
						_validFrameRanges.Add(item);
					}
					else if (Animator.enabled)
					{
						Animator.Update(0f);
						item = new FrameRange(j, j);
					}
				}
				foreach (FAnimationTrack item4 in _tracksCached)
				{
					item4.Cache = this;
				}
				base.Track = _tracksCached[0];
				Animator.StopRecording();
			}
			if (!activeSelf)
			{
				HideFlags hideFlags2 = base.Track.Owner.gameObject.hideFlags;
				base.Track.Owner.gameObject.hideFlags |= HideFlags.DontSave;
				base.Track.Owner.gameObject.SetActive(value: false);
				base.Track.Owner.gameObject.hideFlags = hideFlags2;
			}
			if (speed != 1f)
			{
				sequence.Speed = speed;
			}
			Animator.cullingMode = cullingMode;
			sequence.Stop(reset: true);
			if (currentFrame >= 0)
			{
				if (isPlaying)
				{
					sequence.Play(currentFrame);
				}
				else
				{
					sequence.SetCurrentFrame(currentFrame);
				}
				transformSnapshot.Restore();
			}
			return flag;
		}

		protected override bool ClearInternal()
		{
			StopPlayback();
			foreach (FAnimationTrack item in _tracksCached)
			{
				item.Cache = null;
			}
			_tracksCached.Clear();
			return true;
		}

		public void StartPlayback()
		{
			if (!_inPlayback)
			{
				Animator.enabled = true;
				Animator.StartPlayback();
				_inPlayback = true;
			}
		}

		public void StopPlayback()
		{
			if (_inPlayback)
			{
				Animator.StopPlayback();
				_inPlayback = false;
			}
		}

		public override void GetPlaybackAt(float sequenceTime)
		{
			if (!InPlayback)
			{
				StartPlayback();
			}
			if (base.Track.Owner.gameObject.activeInHierarchy)
			{
				Animator.playbackTime = ConvertSequenceTime(sequenceTime);
				Animator.Update(0f);
			}
		}

		private float ConvertSequenceTime(float sequenceTime)
		{
			float num = 0f;
			for (int i = 0; i != _validFrameRanges.Count; i++)
			{
				float num2 = (float)_validFrameRanges[i].Start * base.Track.Sequence.InverseFrameRate;
				if (num2 > sequenceTime)
				{
					num -= 0.0001f;
					break;
				}
				float num3 = (float)_validFrameRanges[i].End * base.Track.Sequence.InverseFrameRate;
				if (num3 > sequenceTime)
				{
					num += sequenceTime - num2 + 0.0001f;
					break;
				}
				num += num3 - num2;
			}
			return Mathf.Clamp(num, Animator.recorderStartTime, Animator.recorderStopTime - 0.0001f);
		}

		private List<FTrack> GetTracksToUpdate(out List<FAnimationTrack> animTracks)
		{
			List<FTrack> list = new List<FTrack>();
			animTracks = new List<FAnimationTrack>();
			Transform owner = base.Track.Owner;
			List<FContainer> containers = base.Track.Sequence.Containers;
			foreach (FContainer item in containers)
			{
				List<FTimeline> timelines = item.Timelines;
				foreach (FTimeline item2 in timelines)
				{
					if (!(item2.Owner != owner))
					{
						List<FTrack> tracks = item2.Tracks;
						foreach (FTrack item3 in tracks)
						{
							if (item3 != null && item3.enabled)
							{
								if (item3 is FAnimationTrack)
								{
									animTracks.Add((FAnimationTrack)item3);
									list.Add(item3);
								}
								else if (item3 is FTransformTrack)
								{
									list.Add(item3);
								}
							}
						}
					}
				}
			}
			return list;
		}
	}
}
