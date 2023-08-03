using UnityEngine;

namespace Flux
{
	[FEvent("Audio/Trigger Play", typeof(FLegacyAnimationTrack))]
	public class FPlayAudioSourceEvent : FEvent
	{
		public override string Text
		{
			get
			{
				AudioClip clip = Owner.GetComponent<AudioSource>().clip;
				return (!(clip == null)) ? clip.name : "!Missing!";
			}
			set
			{
			}
		}

		protected override void OnUpdateEvent(float timeSinceTrigger)
		{
			AudioSource component = Owner.GetComponent<AudioSource>();
			if (timeSinceTrigger < 0.1f && (!component.isPlaying || component.time > 0.1f))
			{
				component.Stop();
				component.Play();
			}
		}

		public override int GetMaxLength()
		{
			if (Owner.GetComponent<AudioSource>().clip != null)
			{
				return Mathf.RoundToInt(Owner.GetComponent<AudioSource>().clip.length * (float)Sequence.FrameRate);
			}
			return 0;
		}

		public override int GetMinLength()
		{
			return GetMaxLength();
		}
	}
}
