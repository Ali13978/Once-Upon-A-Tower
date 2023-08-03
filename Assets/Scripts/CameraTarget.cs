using UnityEngine;

public class CameraTarget : MonoBehaviour
{
	public Collider Frame;

	private void OnTriggerEnter(Collider other)
	{
		if (other == SingletonMonoBehaviour<Game>.Instance.Digger.Collider)
		{
			SingletonMonoBehaviour<Game>.Instance.GameCamera.TargetFrame = Frame;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other == SingletonMonoBehaviour<Game>.Instance.Digger.Collider)
		{
			SingletonMonoBehaviour<Game>.Instance.GameCamera.TargetFrame = null;
		}
	}
}
