using UnityEngine;

namespace Flux
{
	[FEvent("Light/Intensity")]
	public class FLightIntensityEvent : FTweenEvent<FTweenFloat>
	{
		private Light _light;

		private float _startIntensity;

		protected override void OnInit()
		{
			_light = Owner.GetComponent<Light>();
		}

		protected override void OnTrigger(float timeSinceTrigger)
		{
			_startIntensity = _light.intensity;
		}

		protected override void SetDefaultValues()
		{
			_tween = new FTweenFloat(1f, 2f);
		}

		protected override void ApplyProperty(float t)
		{
			_light.intensity = _tween.GetValue(t);
		}

		protected override void OnStop()
		{
			_light.intensity = _startIntensity;
		}

		protected override void PreEvent()
		{
			Owner.gameObject.hideFlags = HideFlags.DontSave;
		}

		protected override void PostEvent()
		{
			Owner.gameObject.hideFlags = HideFlags.None;
		}
	}
}
