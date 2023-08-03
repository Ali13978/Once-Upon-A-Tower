using UnityEngine;

namespace Flux
{
	[FEvent("Pomelo/World Textures", typeof(FTextMeshTrack))]
	public class FWorldTexturesEvent : FEvent
	{
		public WorldKeyframe Keyframe;

		public AnimationCurve Curve;

		protected override void OnUpdateEvent(float timeSinceTrigger)
		{
			SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer.CustomKeyframe = Keyframe;
			SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer.CustomKeyframeAlpha = Curve.Evaluate(timeSinceTrigger / base.LengthTime);
			if (!Application.isPlaying)
			{
				WorldCustomizer[] array = Object.FindObjectsOfType<WorldCustomizer>();
				foreach (WorldCustomizer worldCustomizer in array)
				{
					worldCustomizer.Update();
				}
			}
		}

		protected override void OnStop()
		{
			SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer.CustomKeyframe = new WorldKeyframe();
		}

		protected override void OnFinish()
		{
			SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer.CustomKeyframe = new WorldKeyframe();
		}
	}
}
