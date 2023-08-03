using System.Collections;

public class AdManager
{
	public bool ShowedIS
	{
		get;
		protected set;
	}

	public bool ShowedVR
	{
		get;
		protected set;
	}

	public virtual bool VRAvailable => false;

	public virtual bool ISAvailable => false;

	public virtual bool GotReward
	{
		get;
		protected set;
	}

	public virtual void Start()
	{
	}

	public virtual IEnumerator ShowInterstitial()
	{
		yield break;
	}

	public virtual void RunGui()
	{
	}

	public virtual IEnumerator ShowVideo()
	{
		yield break;
	}

	public virtual void OnEnable()
	{
	}

	public virtual void OnDisable()
	{
	}

	public virtual void OnPause()
	{
	}

	public virtual void OnResume()
	{
	}

	public virtual void KongregateAdsAvailable(string param)
	{
	}

	public virtual void KongregateAdsUnavailable(string param)
	{
	}

	public virtual void KongregateAdOpened(string param)
	{
	}

	public virtual void KongregateAdCompleted(string param)
	{
	}

	public virtual void KongregateAdAbandoned(string param)
	{
	}
}
