using System;
using UnityEngine;

namespace Flux
{
	[Serializable]
	public abstract class FTweenBase
	{
		[SerializeField]
		protected FEasingType _easingType = FEasingType.EaseInOutQuad;

		public FEasingType EasingType
		{
			get
			{
				return _easingType;
			}
			set
			{
				_easingType = value;
			}
		}
	}
}
