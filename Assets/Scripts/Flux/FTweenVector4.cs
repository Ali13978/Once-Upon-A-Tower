using System;
using UnityEngine;

namespace Flux
{
	[Serializable]
	public class FTweenVector4 : FTween<Vector4>
	{
		public FTweenVector4(Vector4 from, Vector4 to)
		{
			_from = from;
			_to = to;
		}

		public override Vector4 GetValue(float t)
		{
			Vector4 result = default(Vector4);
			result.x = FEasing.Tween(_from.x, _to.x, t, _easingType);
			result.y = FEasing.Tween(_from.y, _to.y, t, _easingType);
			result.z = FEasing.Tween(_from.z, _to.z, t, _easingType);
			result.w = FEasing.Tween(_from.w, _to.w, t, _easingType);
			return result;
		}
	}
}
