using System.Collections;
using UnityEngine;

public class DelayedAnimation : MonoBehaviour
{
	public Animation Animation;

	public GameObject FlashSkewer;

	public WorldObject WO;

	public AudioSource Clip;

	private IEnumerator coroutine;

	private void Start()
	{
		FlashSkewer.SetActive(value: false);
	}

	private void FixedUpdate()
	{
		if (SingletonMonoBehaviour<Game>.Instance.Digger.Coord.Section == WO.Coord.Section && coroutine == null)
		{
			StartCoroutine(coroutine = DelayedAnimationCoroutine());
		}
	}

	private IEnumerator DelayedAnimationCoroutine()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1.5f));
		FlashSkewer.SetActive(value: true);
		Animation.Play();
		if (Clip != null)
		{
			Clip.Play();
		}
		while (Animation.isPlaying)
		{
			yield return null;
		}
	}
}
