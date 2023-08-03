using System;

namespace Flux
{
	[Serializable]
	public class FTweenFloat : FTween<float>
	{
		public FTweenFloat(float from, float to)
		{
			_from = from;
			_to = to;
		}

		public override float GetValue(float t)
		{
			return FEasing.Tween(_from, _to, t, _easingType);
		}
	}
}
