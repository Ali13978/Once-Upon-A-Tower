using UnityEngine;

namespace Flux
{
	[FEvent("Pomelo/CameraFrame", typeof(FLegacyAnimationTrack))]
	public class FCameraFrameEvent : FEvent
	{
		public bool JumpToFrame = true;

		protected override void OnTrigger(float timeSinceTrigger)
		{
			OnUpdateEvent(timeSinceTrigger);
		}

		protected override void OnUpdateEvent(float timeSinceTrigger)
		{
			SingletonMonoBehaviour<Game>.Instance.GameCamera.TargetFrame = Owner.GetComponent<Collider>();
			if (JumpToFrame)
			{
				SingletonMonoBehaviour<Game>.Instance.GameCamera.Focus();
			}
		}

		protected override void OnStop()
		{
			SingletonMonoBehaviour<Game>.Instance.GameCamera.TargetFrame = null;
		}

		protected override void OnFinish()
		{
			SingletonMonoBehaviour<Game>.Instance.GameCamera.TargetFrame = null;
		}
	}
}
