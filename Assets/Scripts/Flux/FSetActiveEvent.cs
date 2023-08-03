using UnityEngine;

namespace Flux
{
	[FEvent("Game Object/Set Active")]
	public class FSetActiveEvent : FEvent
	{
		[SerializeField]
		private bool _active = true;

		[SerializeField]
		[Tooltip("Does the event set the opposite on the last frame?")]
		private bool _setOppositeOnFinish = true;

		private bool _wasActive;

		private GameObject _ownerGO;

		protected override void OnTrigger(float timeSinceTrigger)
		{
			if (_ownerGO == null)
			{
				_ownerGO = Owner.gameObject;
				_wasActive = _ownerGO.activeSelf;
			}
		}

		protected override void OnUpdateEvent(float timeSinceTrigger)
		{
			if (_ownerGO.activeSelf != _active)
			{
				_ownerGO.SetActive(_active);
			}
		}

		protected override void OnFinish()
		{
			if (_setOppositeOnFinish)
			{
				_ownerGO.SetActive(!_active);
			}
		}

		protected override void OnStop()
		{
			Owner.gameObject.SetActive(_wasActive);
		}
	}
}
