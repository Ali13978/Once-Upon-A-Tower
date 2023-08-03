using System;
using UnityEngine;

namespace Flux
{
	[Serializable]
	public class FTweenQuaternion : FTween<Quaternion>
	{
		public FTweenQuaternion(Quaternion from, Quaternion to)
		{
			_from = from;
			_to = to;
		}

		public override Quaternion GetValue(float t)
		{
			Quaternion result = default(Quaternion);
			result.x = FEasing.Tween(_from.x, _to.x, t, _easingType);
			result.y = FEasing.Tween(_from.y, _to.y, t, _easingType);
			result.z = FEasing.Tween(_from.z, _to.z, t, _easingType);
			result.w = FEasing.Tween(_from.w, _to.w, t, _easingType);
			return result;
		}
	}
}
