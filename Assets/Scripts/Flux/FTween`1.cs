using System;
using UnityEngine;

namespace Flux
{
	[Serializable]
	public abstract class FTween<T> : FTweenBase
	{
		[SerializeField]
		protected T _from;

		[SerializeField]
		protected T _to;

		public T From
		{
			get
			{
				return _from;
			}
			set
			{
				_from = value;
			}
		}

		public T To
		{
			get
			{
				return _to;
			}
			set
			{
				_to = value;
			}
		}

		public abstract T GetValue(float t);
	}
}
