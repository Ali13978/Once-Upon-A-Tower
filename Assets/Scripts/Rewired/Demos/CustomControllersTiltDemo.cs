using UnityEngine;

namespace Rewired.Demos
{
	[AddComponentMenu("")]
	public class CustomControllersTiltDemo : MonoBehaviour
	{
		public Transform target;

		public float speed = 10f;

		private CustomController controller;

		private Player player;

		private void Awake()
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			player = ReInput.players.GetPlayer(0);
			ReInput.InputSourceUpdateEvent += OnInputUpdate;
			controller = (CustomController)player.controllers.GetControllerWithTag(ControllerType.Custom, "TiltController");
		}

		private void Update()
		{
			if (!(target == null))
			{
				Vector3 a = Vector3.zero;
				a.y = player.GetAxis("Tilt Vertical");
				a.x = player.GetAxis("Tilt Horizontal");
				if (a.sqrMagnitude > 1f)
				{
					a.Normalize();
				}
				a *= Time.deltaTime;
				target.Translate(a * speed);
			}
		}

		private void OnInputUpdate()
		{
			Vector3 acceleration = Input.acceleration;
			controller.SetAxisValue(0, acceleration.x);
			controller.SetAxisValue(1, acceleration.y);
			controller.SetAxisValue(2, acceleration.z);
		}
	}
}
