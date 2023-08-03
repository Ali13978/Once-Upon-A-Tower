using UnityEngine;

namespace Flux
{
	[FEvent("Time/Timescale")]
	public class FTimescaleEvent : FEvent
	{
		[SerializeField]
		private AnimationCurve _curve;

		[SerializeField]
		[Tooltip("Set Time.timescale back to 1 at the end?")]
		private bool _clearOnFinish = true;

		public AnimationCurve Curve
		{
			get
			{
				return _curve;
			}
			set
			{
				_curve = value;
			}
		}

		public bool ClearOnFinish
		{
			get
			{
				return _clearOnFinish;
			}
			set
			{
				_clearOnFinish = value;
			}
		}

		protected override void SetDefaultValues()
		{
			_curve = new AnimationCurve(new Keyframe(0f, 1f));
		}

		protected override void OnFrameRangeChanged(FrameRange oldFrameRange)
		{
			if (oldFrameRange.Length != base.FrameRange.Length)
			{
				FUtility.ResizeAnimationCurve(_curve, (float)base.FrameRange.Length * Sequence.InverseFrameRate);
			}
		}

		protected override void OnTrigger(float timeSinceTrigger)
		{
		}

		protected override void OnUpdateEvent(float timeSinceTrigger)
		{
			Time.timeScale = Mathf.Clamp(_curve.Evaluate(timeSinceTrigger), 0f, 100f);
		}

		protected override void OnStop()
		{
			Time.timeScale = 1f;
		}

		protected override void OnFinish()
		{
			if (ClearOnFinish)
			{
				Time.timeScale = 1f;
			}
		}
	}
}
