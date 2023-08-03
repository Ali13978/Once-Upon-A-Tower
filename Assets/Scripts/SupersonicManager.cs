using System;
using System.Collections;
using UnityEngine;

public class SupersonicManager : SingletonMonoBehaviour<SupersonicManager>
{
	public const int MaxVideosPerDay = 1000;

	public const int MaxISPerDay = 8;

	public AdViewsInDay currentVideoViews;

	public AdViewsInDay currentISViews;

	private AdManager manager = new SupersonicUninitializedManager();

	public bool VRAvailable
	{
		get
		{
			checkVideosLeft();
			return manager.VRAvailable && currentVideoViews.AdsLeft > 0;
		}
	}

	public bool GotReward => manager.GotReward;

	public bool ShowedIS => manager.ShowedIS;

	public bool ShowedVR => manager.ShowedVR;

	public bool ISAvailable
	{
		get
		{
			checkInterstitialsLeft();
			return manager.ISAvailable && currentISViews.AdsLeft > 0;
		}
	}

	private void Start()
	{
		manager = new SupersonicMobileManager();
		manager.Start();
		manager.OnEnable();
		checkVideosLeft();
	}

	private AdViewsInDay checkViews(AdViewsInDay adViews, int max)
	{
		if (DateTime.Today > adViews.Day)
		{
			adViews.Day = DateTime.Today;
			adViews.AdsLeft = max;
		}
		return adViews;
	}

	private void checkVideosLeft()
	{
		currentVideoViews = checkViews(currentVideoViews, 1000);
	}

	private void checkInterstitialsLeft()
	{
		currentISViews = checkViews(currentISViews, 8);
	}

	private void OnEnable()
	{
		manager.OnEnable();
	}

	private void OnDisable()
	{
		manager.OnDisable();
	}

	private void SetAudioActive(bool active)
	{
		AudioListener.pause = !active;
	}

	public IEnumerator ShowVideo()
	{
		GameTime.Pause();
		SingletonMonoBehaviour<GameInput>.Instance.Enabled = false;
		SingletonMonoBehaviour<Gui>.Instance.Enabled = false;
		SetAudioActive(active: false);
		IEnumerator subroutine = manager.ShowVideo();
		while (subroutine.MoveNext())
		{
			yield return subroutine.Current;
		}
		if (ShowedVR)
		{
			currentVideoViews.AdsLeft--;
		}
		SingletonMonoBehaviour<Gui>.Instance.Enabled = true;
		SingletonMonoBehaviour<GameInput>.Instance.Enabled = true;
		SetAudioActive(active: true);
		GameTime.Resume();
	}

	public IEnumerator ShowInterstitial()
	{
		SetAudioActive(active: false);
		IEnumerator subroutine = manager.ShowInterstitial();
		while (subroutine.MoveNext())
		{
			yield return subroutine.Current;
		}
		if (ShowedIS)
		{
			currentISViews.AdsLeft--;
		}
		SetAudioActive(active: true);
	}

	private void OnApplicationPause(bool isPaused)
	{
		if (isPaused)
		{
			manager.OnPause();
		}
		else
		{
			manager.OnResume();
		}
	}

	public virtual void KongregateAdsAvailable(string param)
	{
		UnityEngine.Debug.Log("KongregateAdsAvailable");
		manager.KongregateAdsAvailable(param);
	}

	public virtual void KongregateAdsUnavailable(string param)
	{
		UnityEngine.Debug.Log("KongregateAdsUnavailable");
		manager.KongregateAdsUnavailable(param);
	}

	public virtual void KongregateAdOpened(string param)
	{
		UnityEngine.Debug.Log("KongregateAdOpened");
		manager.KongregateAdOpened(param);
	}

	public virtual void KongregateAdCompleted(string param)
	{
		UnityEngine.Debug.Log("KongregateAdCompleted");
		manager.KongregateAdCompleted(param);
	}

	public virtual void KongregateAdAbandoned(string param)
	{
		UnityEngine.Debug.Log("KongregateAdAbandoned");
		manager.KongregateAdAbandoned(param);
	}
}
