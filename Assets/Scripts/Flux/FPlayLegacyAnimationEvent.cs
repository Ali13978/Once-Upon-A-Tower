using UnityEngine;

namespace Flux
{
	[FEvent("Legacy Animation/Play Animation", typeof(FLegacyAnimationTrack))]
	public class FPlayLegacyAnimationEvent : FEvent
	{
		public AnimationClip Clip;

		public int ClipStart;

		public float Speed = 1f;

		public override string Text
		{
			get
			{
				return (!(Clip == null)) ? Clip.name : "!Missing!";
			}
			set
			{
			}
		}

		protected override void OnUpdateEvent(float timeSinceTrigger)
		{
			if (!(Clip == null))
			{
				Clip.SampleAnimation(Owner.gameObject, timeSinceTrigger * Speed + (float)ClipStart * 1f / (float)Sequence.FrameRate);
			}
		}

		public override int GetMaxLength()
		{
			if (Clip != null)
			{
				return Mathf.RoundToInt(Clip.length / Speed * (float)Sequence.FrameRate) - ClipStart;
			}
			return 0;
		}

		public override int GetMinLength()
		{
			return 1;
		}
	}
}
