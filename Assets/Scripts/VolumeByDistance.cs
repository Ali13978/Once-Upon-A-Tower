using UnityEngine;

public class VolumeByDistance : MonoBehaviour
{
	public float MinDistance = 0.5f;

	public float MaxDistanceUp = 10f;

	public float MaxDistanceDown = 5f;

	public AudioSource AudioSource;

	private void Awake()
	{
		if (AudioSource == null)
		{
			AudioSource = GetComponent<AudioSource>();
		}
	}

	private void Update()
	{
		if (SingletonMonoBehaviour<Game>.HasInstance() && SingletonMonoBehaviour<Game>.Instance.Ready && (bool)SingletonMonoBehaviour<Game>.Instance.Digger)
		{
			Vector3 position = base.transform.position;
			float y = position.y;
			Vector3 position2 = SingletonMonoBehaviour<Game>.Instance.Digger.transform.position;
			float num = Mathf.Abs(y - position2.y);
			Vector3 position3 = SingletonMonoBehaviour<Game>.Instance.Digger.transform.position;
			float y2 = position3.y;
			Vector3 position4 = base.transform.position;
			float num2 = (!(y2 > position4.y)) ? MaxDistanceDown : MaxDistanceUp;
			float t = 1f - Mathf.Clamp01((num - MinDistance) / (num2 - MinDistance));
			t = Tween.QuadEaseInOut(t, 0f, 1f, 1f);
			AudioSource.volume = t;
		}
	}
}
