using UnityEngine;

namespace Flux
{
	public abstract class FObject : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private int _id = -1;

		public abstract FSequence Sequence
		{
			get;
		}

		public abstract Transform Owner
		{
			get;
		}

		public int GetId()
		{
			return _id;
		}

		internal void SetId(int id)
		{
			_id = id;
		}

		public abstract void Init();

		public abstract void Stop();
	}
}
