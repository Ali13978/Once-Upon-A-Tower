using System.Collections;
using UnityEngine;

public class SupersonicMobileManager : AdManager
{
	public enum SubsystemState
	{
		Uninitialized,
		Initializing,
		Initialized
	}

	private const string keyAndroid = "60538e8d";

	private const string keyiOS = "60535505";

	private static bool started;

	private static bool eventISAvailable;

	private static bool eventVRAvailable;

	public static SubsystemState ISState;

	public static SubsystemState VRState;

	private bool showingIs;

	private bool showingVideo;

	public override bool ISAvailable
	{
		get
		{
			if (ISState != SubsystemState.Initialized)
			{
				return false;
			}
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				return false;
			}
			return eventISAvailable;
		}
	}

	public override bool VRAvailable
	{
		get
		{
			if (VRState != SubsystemState.Initialized)
			{
				return false;
			}
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				return false;
			}
			return eventVRAvailable;
		}
	}

	public override void Start()
	{
		if (!started)
		{
			GameObject gameObject = new GameObject("Supersonic Events");
			gameObject.AddComponent<SupersonicEvents>();
			started = true;
			Supersonic.Agent.start();
		}
		Supersonic.Agent.validateIntegration();
		string userId = SaveGame.Instance.UserId;
		if (VRState == SubsystemState.Uninitialized)
		{
			VRState = SubsystemState.Initializing;
			Supersonic.Agent.initRewardedVideo("60538e8d", userId);
		}
	}

	private void ISInitSuccess()
	{
		ISState = SubsystemState.Initialized;
	}

	private void ISInitFail(SupersonicError error)
	{
		ISState = SubsystemState.Uninitialized;
	}

	private void ISShowSuccess()
	{
		base.ShowedIS = true;
	}

	private void ISShowFail(SupersonicError error)
	{
		showingIs = false;
	}

	private void ISAvailability(bool available)
	{
		UnityEngine.Debug.Log("Supersonic interstitial availability changed. Available: " + available);
		eventISAvailable = available;
	}

	private void ISAdClicked()
	{
		showingIs = false;
	}

	private void ISDidClose()
	{
		showingIs = false;
	}

	public override IEnumerator ShowInterstitial()
	{
		base.ShowedIS = false;
		if (ISAvailable)
		{
			showingIs = true;
			Supersonic.Agent.showInterstitial();
			while (showingIs)
			{
				yield return null;
			}
		}
	}

	public override IEnumerator ShowVideo()
	{
		base.ShowedVR = false;
		GotReward = false;
		if (!VRAvailable)
		{
			yield break;
		}
		showingVideo = true;
		Supersonic.Agent.showRewardedVideo();
		float time = Time.realtimeSinceStartup;
		while (showingVideo)
		{
			if (Time.realtimeSinceStartup - time > 2f && !base.ShowedVR)
			{
				GotReward = true;
				break;
			}
			yield return null;
		}
		yield return null;
	}

	private void RewardedVideoInitSuccessEvent()
	{
		VRState = SubsystemState.Initialized;
	}

	private void RewardedVideoInitFailEvent(SupersonicError error)
	{
		VRState = SubsystemState.Uninitialized;
	}

	private void RewardedVideoAdOpenedEvent()
	{
		UnityEngine.Debug.Log("Supersonic Video Ad Opened");
		base.ShowedVR = true;
	}

	private void RewardedVideoAdRewardedEvent(SupersonicPlacement placement)
	{
		UnityEngine.Debug.Log("Supersonic Video Ad Rewarded: " + Time.frameCount);
		GotReward = true;
	}

	private void RewardedVideoAdClosedEvent()
	{
		UnityEngine.Debug.Log("Supersonic Video Ad Closed");
		showingVideo = false;
	}

	private void RewardedVideoShowFailEvent(SupersonicError error)
	{
		UnityEngine.Debug.Log("Supersonic Video Show Fail: " + error);
		if (showingVideo)
		{
			showingVideo = false;
			GotReward = true;
		}
		if (error.getCode() == 520)
		{
			eventVRAvailable = false;
		}
	}

	private void VideoAvailabilityChangedEvent(bool available)
	{
		UnityEngine.Debug.Log("Supersonic Video availability changed. Available: " + available);
		eventVRAvailable = available;
	}

	private void VideoStartEvent()
	{
		UnityEngine.Debug.Log("Supersonic Video Start");
	}

	private void VideoEndEvent()
	{
		UnityEngine.Debug.Log("Supersonic Video End: " + Time.frameCount);
	}

	public override void OnPause()
	{
		Supersonic.Agent.onPause();
	}

	public override void OnResume()
	{
		Supersonic.Agent.onResume();
	}

	public override void OnEnable()
	{
		SupersonicEvents.onRewardedVideoInitSuccessEvent += RewardedVideoInitSuccessEvent;
		SupersonicEvents.onRewardedVideoInitFailEvent += RewardedVideoInitFailEvent;
		SupersonicEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
		SupersonicEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
		SupersonicEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
		SupersonicEvents.onRewardedVideoShowFailEvent += RewardedVideoShowFailEvent;
		SupersonicEvents.onVideoAvailabilityChangedEvent += VideoAvailabilityChangedEvent;
		SupersonicEvents.onVideoStartEvent += VideoStartEvent;
		SupersonicEvents.onVideoEndEvent += VideoEndEvent;
	}

	public override void OnDisable()
	{
		SupersonicEvents.onRewardedVideoInitSuccessEvent -= RewardedVideoInitSuccessEvent;
		SupersonicEvents.onRewardedVideoInitFailEvent -= RewardedVideoInitFailEvent;
		SupersonicEvents.onRewardedVideoAdOpenedEvent -= RewardedVideoAdOpenedEvent;
		SupersonicEvents.onRewardedVideoAdRewardedEvent -= RewardedVideoAdRewardedEvent;
		SupersonicEvents.onRewardedVideoAdClosedEvent -= RewardedVideoAdClosedEvent;
		SupersonicEvents.onRewardedVideoShowFailEvent -= RewardedVideoShowFailEvent;
		SupersonicEvents.onVideoAvailabilityChangedEvent -= VideoAvailabilityChangedEvent;
		SupersonicEvents.onVideoStartEvent -= VideoStartEvent;
		SupersonicEvents.onVideoEndEvent -= VideoEndEvent;
	}
}
