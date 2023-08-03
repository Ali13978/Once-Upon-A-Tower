using System.Collections;
using UnityEngine;

public class Birds : MonoBehaviour
{
	public Animation[] Animations;

	private IEnumerator Start()
	{
		WaitForSeconds wait = new WaitForSeconds(1.5f);
		while (true)
		{
			Animation animation = Animations[Random.Range(0, Animations.Length)];
			WorldCustomizer currentCustomizer = SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer;
			if (!animation.isPlaying && currentCustomizer.IsDay)
			{
				Vector3 position = animation.transform.position;
				Vector3 position2 = SingletonMonoBehaviour<Game>.Instance.GameCamera.transform.position;
				position.y = position2.y + UnityEngine.Random.Range(-10f, 10f);
				animation.transform.position = position;
				animation.gameObject.SetActive(value: true);
				animation.Play();
			}
			for (int i = 0; i < Animations.Length; i++)
			{
				if (!Animations[i].isPlaying && Animations[i].gameObject.activeSelf)
				{
					Animations[i].gameObject.SetActive(value: false);
				}
			}
			yield return wait;
		}
	}
}
