using System.Collections;
using UnityEngine;

public class Splash : GuiView
{
	public GameObject FirstSplash;

	public GameObject SecondSplash;

	protected override void Start()
    {
        base.Start();
		StartCoroutine(SplashCoroutine());
	}

	private IEnumerator SplashCoroutine()
	{
        SecondSplash.SetActive(value: false);
        while (true)
        {
            Show();
            PlaySingle("Intro");
            yield return null;
            while (!SingletonMonoBehaviour<Game>.HasInstance() || !SingletonMonoBehaviour<Game>.Instance.Ready)
            {
                yield return null;
            }
            yield return PlayInCoroutine("Outro");
            Hide();
            while (SingletonMonoBehaviour<Game>.HasInstance() && SingletonMonoBehaviour<Game>.Instance.Ready)
            {
                yield return null;
            }
            FirstSplash.SetActive(value: false);
            SecondSplash.SetActive(value: true);
        }
        //Hide();
	}
}
