using UnityEngine;

public class CopyCameraAspect : MonoBehaviour
{
	public Camera Source;

	public Camera Target;

	private void Update()
	{
		if (Target.aspect != Source.aspect)
		{
			Target.aspect = Source.aspect;
		}
		if (Target.orthographicSize != Source.orthographicSize)
		{
			Target.orthographicSize = Source.orthographicSize;
		}
	}
}
