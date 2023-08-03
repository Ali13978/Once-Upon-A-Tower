using UnityEngine;

namespace Flux
{
	[FEvent("Light/Spot Angle")]
	public class FLightSpotAngleEvent : FTweenEvent<FTweenFloat>
	{
		private Light _light;

		protected override void OnInit()
		{
			_light = Owner.GetComponent<Light>();
		}

		protected override void SetDefaultValues()
		{
			_tween = new FTweenFloat(30f, 45f);
		}

		protected override void ApplyProperty(float t)
		{
			_light.spotAngle = _tween.GetValue(t);
		}
	}
}
