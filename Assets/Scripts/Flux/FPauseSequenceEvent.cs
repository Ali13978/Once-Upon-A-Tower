namespace Flux
{
	[FEvent("Sequence/Pause Sequence")]
	public class FPauseSequenceEvent : FEvent
	{
		private FSequence _sequence;

		protected override void OnInit()
		{
			_sequence = Owner.GetComponent<FSequence>();
		}

		protected override void OnTrigger(float timeSinceTrigger)
		{
			_sequence.Pause();
		}
	}
}
