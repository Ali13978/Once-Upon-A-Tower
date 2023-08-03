using UnityEngine;

public class GuiTrack : MonoBehaviour
{
	public Vector3 Point;

	public Transform TrackTransform;

	public Camera TrackCamera;

	private void Update()
	{
		if (!(Point == Vector3.zero) || !(TrackTransform == null))
		{
			base.transform.position = Target();
		}
	}

	private Vector3 Target()
	{
		Camera camera = (!(TrackCamera == null)) ? TrackCamera : SingletonMonoBehaviour<Game>.Instance.GameCamera.Camera;
		if (TrackTransform != null)
		{
			Point = TrackTransform.position;
		}
		Vector3 position = camera.WorldToViewportPoint(Point);
		Vector3 result = SingletonMonoBehaviour<Gui>.Instance.GuiCamera.ViewportToWorldPoint(position);
		Vector3 position2 = base.transform.position;
		result.z = position2.z;
		return result;
	}

	public void JumpToTarget()
	{
		base.transform.position = Target();
	}
}
