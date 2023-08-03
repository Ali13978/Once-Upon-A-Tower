using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
	public AudioSource Audio;

	public float FadeInTime = 0.3f;

	public float FadeOutTime = 0.3f;

	public float TargetVolume = 1f;

	private void Awake()
	{
		if (Audio == null)
		{
			Audio = GetComponent<AudioSource>();
		}
	}

	private void Update()
	{
		if (Audio.volume > TargetVolume)
		{
			Audio.volume -= Time.deltaTime / FadeOutTime;
			if (Audio.volume < TargetVolume)
			{
				Audio.volume = TargetVolume;
			}
		}
		else if (Audio.volume < TargetVolume)
		{
			Audio.volume += Time.deltaTime / FadeInTime;
			if (Audio.volume > TargetVolume)
			{
				Audio.volume = TargetVolume;
			}
		}
		if (Audio.volume == 0f && Audio.isPlaying)
		{
			Audio.Pause();
		}
		if (Audio.volume != 0f && !Audio.isPlaying)
		{
			Audio.Play();
		}
	}

	private void OnDisable()
	{
		Audio.volume = TargetVolume;
	}
}
