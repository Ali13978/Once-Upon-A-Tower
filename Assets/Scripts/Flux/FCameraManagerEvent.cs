using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
	[FEvent("Camera/Camera Manager")]
	public class FCameraManagerEvent : FEvent
	{
		[Tooltip("Camera that should become active")]
		[SerializeField]
		private Camera _camera;

		public Camera Camera
		{
			get
			{
				return _camera;
			}
			set
			{
				_camera = value;
			}
		}

		public override string Text
		{
			get
			{
				return (!(_camera == null)) ? _camera.name : "!Missing!";
			}
			set
			{
			}
		}

		protected override void OnInit()
		{
			if (GetId() == 0)
			{
				List<FEvent> events = base.Track.Events;
				foreach (FCameraManagerEvent item in events)
				{
					if (item.Camera != null)
					{
						item.Camera.gameObject.SetActive(value: false);
					}
				}
			}
		}

		protected override void OnTrigger(float timeSinceTrigger)
		{
			_camera.gameObject.SetActive(value: true);
		}

		protected override void OnFinish()
		{
			_camera.gameObject.SetActive(value: false);
		}
	}
}
