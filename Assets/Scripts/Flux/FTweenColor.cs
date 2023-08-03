using System;
using UnityEngine;

namespace Flux
{
	[Serializable]
	public class FTweenColor : FTween<Color>
	{
		public FTweenColor(Color from, Color to)
		{
			_from = from;
			_to = to;
		}

		public override Color GetValue(float t)
		{
			Color result = default(Color);
			result.r = FEasing.Tween(_from.r, _to.r, t, _easingType);
			result.g = FEasing.Tween(_from.g, _to.g, t, _easingType);
			result.b = FEasing.Tween(_from.b, _to.b, t, _easingType);
			result.a = FEasing.Tween(_from.a, _to.a, t, _easingType);
			return result;
		}
	}
}
