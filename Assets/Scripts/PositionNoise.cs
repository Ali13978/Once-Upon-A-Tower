using UnityEngine;

public class PositionNoise : MonoBehaviour
{
	private void Start()
	{
		Transform transform = base.transform;
		Vector3 localPosition = transform.localPosition;
		Vector3 right = Vector3.right;
		Vector3 position = base.transform.position;
		transform.localPosition = localPosition + right * (Mathf.PerlinNoise(0f, position.y / 2f) - 0.5f) / 8f;
	}
}
