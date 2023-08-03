using UnityEngine;

namespace Flux
{
	[FEvent("Renderer/Tween Float", typeof(FRendererTrack))]
	public class FRendererFloatEvent : FRendererEvent
	{
		[SerializeField]
		private FTweenFloat _tween;

		protected override void SetDefaultValues()
		{
			if (base.PropertyName == null)
			{
				base.PropertyName = "_Alpha";
			}
			if (_tween == null)
			{
				_tween = new FTweenFloat(0f, 1f);
			}
		}

		protected override void ApplyProperty(float t)
		{
			_matPropertyBlock.SetFloat(base.PropertyName, _tween.GetValue(t));
		}
	}
}
