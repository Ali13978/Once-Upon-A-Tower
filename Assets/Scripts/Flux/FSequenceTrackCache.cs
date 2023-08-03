using UnityEngine;

namespace Flux
{
	public class FSequenceTrackCache : FTrackCache
	{
		public FSequenceTrackCache(FSequenceTrack track)
			: base(track)
		{
		}

		protected override bool BuildInternal()
		{
			FSequence component = base.Track.Owner.GetComponent<FSequence>();
			foreach (FContainer container in component.Containers)
			{
				foreach (FTimeline timeline in container.Timelines)
				{
					foreach (FTrack track in timeline.Tracks)
					{
						if ((track.RequiresForwardCache && base.Track.Sequence.IsPlayingForward) || (track.RequiresBackwardsCache && !base.Track.Sequence.IsPlayingForward) || (track.RequiresEditorCache && !Application.isPlaying))
						{
							track.CreateCache();
						}
					}
				}
			}
			return true;
		}

		protected override bool ClearInternal()
		{
			FSequence component = base.Track.Owner.GetComponent<FSequence>();
			foreach (FContainer container in component.Containers)
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
			FAnimationTrack.DeleteAnimationPreviews(component);
			return true;
		}

		public override void GetPlaybackAt(float sequenceTime)
		{
			base.Track.UpdateEventsEditor((int)(sequenceTime * (float)base.Track.Sequence.FrameRate), sequenceTime);
		}
	}
}
