using UnityEngine;

public class ParticleSystemTrigger : MonoBehaviour
{
	public ParticleSystem ParticleSystem;

	public float MinTime = 5f;

	public float MaxTime = 10f;

	public AudioSource Audio;

	private float nextTime;

	private void Start()
	{
		nextTime = Time.time + UnityEngine.Random.Range(MinTime, MaxTime);
	}

	private void Update()
	{
		if (Time.time > nextTime)
		{
			if ((bool)ParticleSystem)
			{
				ParticleSystem.Play();
			}
			if ((bool)Audio)
			{
				Audio.Play();
			}
			nextTime = Time.time + UnityEngine.Random.Range(MinTime, MaxTime);
		}
	}
}
