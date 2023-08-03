using UnityEngine;

namespace Flux
{
	[FEvent("Sequence/Play Sequence", typeof(FSequenceTrack))]
	public class FPlaySequenceEvent : FEvent
	{
		private FSequence _sequence;

		[SerializeField]
		private int _startOffset;

		public int StartOffset => _startOffset;

		protected override void OnInit()
		{
			_sequence = Owner.GetComponent<FSequence>();
		}

		protected override void OnTrigger(float timeSinceTrigger)
		{
			if (Sequence.IsPlaying && Application.isPlaying)
			{
				_sequence.Play((float)_startOffset * _sequence.InverseFrameRate + timeSinceTrigger);
			}
		}

		protected override void OnUpdateEvent(float timeSinceTrigger)
		{
			_sequence.Speed = Mathf.Sign(Sequence.Speed) * Mathf.Abs(_sequence.Speed);
		}

		protected override void OnStop()
		{
			_sequence.Stop(reset: true);
		}

		protected override void OnFinish()
		{
			_sequence.Pause();
		}

		protected override void OnPause()
		{
			_sequence.Pause();
		}

		protected override void OnResume()
		{
			_sequence.Play(_sequence.CurrentTime);
		}
	}
}
