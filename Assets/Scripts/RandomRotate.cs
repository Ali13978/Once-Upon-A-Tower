using UnityEngine;

public class RandomRotate : MonoBehaviour
{
	private void Start()
	{
		base.transform.localRotation = Quaternion.AngleAxis(180 * UnityEngine.Random.Range(0, 2), Vector3.back) * base.transform.localRotation;
	}
}
