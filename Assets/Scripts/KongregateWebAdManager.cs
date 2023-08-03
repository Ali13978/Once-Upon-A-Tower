using System.Collections;
using UnityEngine;

public class KongregateWebAdManager : AdManager
{
	private bool showingVR;

	private static bool adsAvailable;

	public override bool VRAvailable => adsAvailable;

	public override void Start()
	{
		UnityEngine.Debug.Log("Initializing Kongregate Ads");
		Application.ExternalEval("\nkongregate.mtx.addEventListener('adsAvailable', function(e){\n  kongregateUnitySupport.getUnityObject().SendMessage('Global', 'KongregateAdsAvailable', '');\n});\n\nkongregate.mtx.addEventListener('adsUnavailable', function(e){\n  kongregateUnitySupport.getUnityObject().SendMessage('Global', 'KongregateAdsUnavailable', '');\n});\n\nkongregate.mtx.addEventListener('adOpened', function(e){\n  kongregateUnitySupport.getUnityObject().SendMessage('Global', 'KongregateAdOpened', '');\n});\n\nkongregate.mtx.addEventListener('adCompleted', function(e){\n  kongregateUnitySupport.getUnityObject().SendMessage('Global', 'KongregateAdCompleted', '');\n});\n\nkongregate.mtx.addEventListener('adAbandoned', function(e){\n  kongregateUnitySupport.getUnityObject().SendMessage('Global', 'KongregateAdAbandoned', '');\n});\nkongregate.mtx.initializeIncentivizedAds();\n");
	}

	public override void KongregateAdsAvailable(string param)
	{
		adsAvailable = true;
	}

	public override void KongregateAdsUnavailable(string param)
	{
		adsAvailable = false;
	}

	public override void KongregateAdOpened(string param)
	{
		base.ShowedVR = true;
	}

	public override void KongregateAdCompleted(string param)
	{
		showingVR = false;
		GotReward = true;
	}

	public override void KongregateAdAbandoned(string param)
	{
		showingVR = false;
	}

	public override IEnumerator ShowVideo()
	{
		Application.ExternalEval("kongregate.mtx.showIncentivizedAd();");
		showingVR = true;
		base.ShowedVR = false;
		GotReward = false;
		while (showingVR)
		{
			yield return null;
		}
	}
}
