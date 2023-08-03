using UnityEngine;

namespace Flux
{
	[FEvent("Light/Range")]
	public class FLightRangeEvent : FTweenEvent<FTweenFloat>
	{
		private Light _light;

		protected override void OnInit()
		{
			_light = Owner.GetComponent<Light>();
		}

		protected override void SetDefaultValues()
		{
			_tween = new FTweenFloat(10f, 20f);
		}

		protected override void ApplyProperty(float t)
		{
			_light.range = _tween.GetValue(t);
		}
	}
}
