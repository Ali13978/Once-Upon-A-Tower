using UnityEngine;

public class ZFightFix : MonoBehaviour
{
	public float Range = 0.1f;

	private void Start()
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z += UnityEngine.Random.Range((0f - Range) / 2f, Range / 2f);
		base.transform.localPosition = localPosition;
	}
}
