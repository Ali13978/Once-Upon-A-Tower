using System;
using UnityEngine;

namespace Flux
{
	[Serializable]
	public class FTweenVector3 : FTween<Vector3>
	{
		public FTweenVector3(Vector3 from, Vector3 to)
		{
			_from = from;
			_to = to;
		}

		public override Vector3 GetValue(float t)
		{
			Vector3 result = default(Vector3);
			result.x = FEasing.Tween(_from.x, _to.x, t, _easingType);
			result.y = FEasing.Tween(_from.y, _to.y, t, _easingType);
			result.z = FEasing.Tween(_from.z, _to.z, t, _easingType);
			return result;
		}
	}
}
