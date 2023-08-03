using System;
using UnityEngine;

namespace Flux
{
	[Serializable]
	public class FTweenVector2 : FTween<Vector2>
	{
		public FTweenVector2(Vector2 from, Vector2 to)
		{
			_from = from;
			_to = to;
		}

		public override Vector2 GetValue(float t)
		{
			Vector2 result = default(Vector2);
			result.x = FEasing.Tween(_from.x, _to.x, t, _easingType);
			result.y = FEasing.Tween(_from.y, _to.y, t, _easingType);
			return result;
		}
	}
}
