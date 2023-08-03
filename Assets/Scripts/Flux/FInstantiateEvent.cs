using UnityEngine;

namespace Flux
{
	[FEvent("Game Object/Instantiate")]
	public class FInstantiateEvent : FEvent
	{
		[SerializeField]
		private GameObject _prefab;

		private GameObject _instance;

		protected override void OnTrigger(float timeSinceTrigger)
		{
			_instance = Object.Instantiate(_prefab);
		}

		protected override void OnStop()
		{
			if (_instance != null)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(_instance);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(_instance);
				}
			}
		}
	}
}
