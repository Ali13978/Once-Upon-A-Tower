using SupersonicJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SupersonicEvents : MonoBehaviour
{
	private const string ERROR_CODE = "error_code";

	private const string ERROR_DESCRIPTION = "error_description";

	private static event Action _onRewardedVideoInitSuccessEvent;

	public static event Action onRewardedVideoInitSuccessEvent;

	private static event Action<SupersonicError> _onRewardedVideoInitFailEvent;

	public static event Action<SupersonicError> onRewardedVideoInitFailEvent;

	private static event Action<SupersonicError> _onRewardedVideoShowFailEvent;

	public static event Action<SupersonicError> onRewardedVideoShowFailEvent;

	private static event Action _onRewardedVideoAdOpenedEvent;

	public static event Action onRewardedVideoAdOpenedEvent;

	private static event Action _onRewardedVideoAdClosedEvent;

	public static event Action onRewardedVideoAdClosedEvent;

	private static event Action _onVideoStartEvent;

	public static event Action onVideoStartEvent;

	private static event Action _onVideoEndEvent;

	public static event Action onVideoEndEvent;

	private static event Action<SupersonicPlacement> _onRewardedVideoAdRewardedEvent;

	public static event Action<SupersonicPlacement> onRewardedVideoAdRewardedEvent;

	private static event Action<bool> _onVideoAvailabilityChangedEvent;

	public static event Action<bool> onVideoAvailabilityChangedEvent;

	private static event Action _onInterstitialInitSuccessEvent;

	public static event Action onInterstitialInitSuccessEvent;

	private static event Action<SupersonicError> _onInterstitialInitFailedEvent;

	public static event Action<SupersonicError> onInterstitialInitFailedEvent;

	private static event Action _onInterstitialReadyEvent;

	public static event Action onInterstitialReadyEvent;

	private static event Action<SupersonicError> _onInterstitialLoadFailedEvent;

	public static event Action<SupersonicError> onInterstitialLoadFailedEvent;

	private static event Action _onInterstitialOpenEvent;

	public static event Action onInterstitialOpenEvent;

	private static event Action _onInterstitialCloseEvent;

	public static event Action onInterstitialCloseEvent;

	private static event Action _onInterstitialShowSuccessEvent;

	public static event Action onInterstitialShowSuccessEvent;

	private static event Action<SupersonicError> _onInterstitialShowFailedEvent;

	public static event Action<SupersonicError> onInterstitialShowFailedEvent;

	private static event Action _onInterstitialClickEvent;

	public static event Action onInterstitialClickEvent;

	private static event Action _onOfferwallInitSuccessEvent;

	public static event Action onOfferwallInitSuccessEvent;

	private static event Action<SupersonicError> _onOfferwallInitFailEvent;

	public static event Action<SupersonicError> onOfferwallInitFailEvent;

	private static event Action _onOfferwallOpenedEvent;

	public static event Action onOfferwallOpenedEvent;

	private static event Action<SupersonicError> _onOfferwallShowFailEvent;

	public static event Action<SupersonicError> onOfferwallShowFailEvent;

	private static event Action _onOfferwallClosedEvent;

	public static event Action onOfferwallClosedEvent;

	private static event Action<SupersonicError> _onGetOfferwallCreditsFailEvent;

	public static event Action<SupersonicError> onGetOfferwallCreditsFailEvent;

	private static event Action<Dictionary<string, object>> _onOfferwallAdCreditedEvent;

	public static event Action<Dictionary<string, object>> onOfferwallAdCreditedEvent;

	private void Awake()
	{
		base.gameObject.name = "SupersonicEvents";
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void onRewardedVideoInitSuccess(string empty)
	{
		if (SupersonicEvents._onRewardedVideoInitSuccessEvent != null)
		{
			SupersonicEvents._onRewardedVideoInitSuccessEvent();
		}
	}

	public void onRewardedVideoInitFail(string description)
	{
		if (SupersonicEvents._onRewardedVideoInitFailEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onRewardedVideoInitFailEvent(errorFromErrorString);
		}
	}

	public void onRewardedVideoShowFail(string description)
	{
		if (SupersonicEvents._onRewardedVideoShowFailEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onRewardedVideoShowFailEvent(errorFromErrorString);
		}
	}

	public void onRewardedVideoAdOpened(string empty)
	{
		if (SupersonicEvents._onRewardedVideoAdOpenedEvent != null)
		{
			SupersonicEvents._onRewardedVideoAdOpenedEvent();
		}
	}

	public void onRewardedVideoAdClosed(string empty)
	{
		if (SupersonicEvents._onRewardedVideoAdClosedEvent != null)
		{
			SupersonicEvents._onRewardedVideoAdClosedEvent();
		}
	}

	public void onVideoStart(string empty)
	{
		if (SupersonicEvents._onVideoStartEvent != null)
		{
			SupersonicEvents._onVideoStartEvent();
		}
	}

	public void onVideoEnd(string empty)
	{
		if (SupersonicEvents._onVideoEndEvent != null)
		{
			SupersonicEvents._onVideoEndEvent();
		}
	}

	public void onRewardedVideoAdRewarded(string description)
	{
		if (SupersonicEvents._onRewardedVideoAdRewardedEvent != null)
		{
			SupersonicPlacement placementFromString = getPlacementFromString(description);
			SupersonicEvents._onRewardedVideoAdRewardedEvent(placementFromString);
		}
	}

	public void onVideoAvailabilityChanged(string stringAvailable)
	{
		bool obj = (stringAvailable == "true") ? true : false;
		if (SupersonicEvents._onVideoAvailabilityChangedEvent != null)
		{
			SupersonicEvents._onVideoAvailabilityChangedEvent(obj);
		}
	}

	public void onInterstitialInitSuccess(string empty)
	{
		if (SupersonicEvents._onInterstitialInitSuccessEvent != null)
		{
			SupersonicEvents._onInterstitialInitSuccessEvent();
		}
	}

	public void onInterstitialInitFailed(string description)
	{
		if (SupersonicEvents._onInterstitialInitFailedEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onInterstitialInitFailedEvent(errorFromErrorString);
		}
	}

	public void onInterstitialReady()
	{
		if (SupersonicEvents._onInterstitialReadyEvent != null)
		{
			SupersonicEvents._onInterstitialReadyEvent();
		}
	}

	public void onInterstitialLoadFailed(string description)
	{
		if (SupersonicEvents._onInterstitialLoadFailedEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onInterstitialLoadFailedEvent(errorFromErrorString);
		}
	}

	public void onInterstitialOpen(string empty)
	{
		if (SupersonicEvents._onInterstitialOpenEvent != null)
		{
			SupersonicEvents._onInterstitialOpenEvent();
		}
	}

	public void onInterstitialClose(string empty)
	{
		if (SupersonicEvents._onInterstitialCloseEvent != null)
		{
			SupersonicEvents._onInterstitialCloseEvent();
		}
	}

	public void onInterstitialShowSuccess(string empty)
	{
		if (SupersonicEvents._onInterstitialShowSuccessEvent != null)
		{
			SupersonicEvents._onInterstitialShowSuccessEvent();
		}
	}

	public void onInterstitialShowFailed(string description)
	{
		if (SupersonicEvents._onInterstitialShowFailedEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onInterstitialShowFailedEvent(errorFromErrorString);
		}
	}

	public void onInterstitialClick(string empty)
	{
		if (SupersonicEvents._onInterstitialClickEvent != null)
		{
			SupersonicEvents._onInterstitialClickEvent();
		}
	}

	public void onOfferwallInitSuccess(string empty)
	{
		if (SupersonicEvents._onOfferwallInitSuccessEvent != null)
		{
			SupersonicEvents._onOfferwallInitSuccessEvent();
		}
	}

	public void onOfferwallInitFail(string description)
	{
		if (SupersonicEvents._onOfferwallInitFailEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onOfferwallInitFailEvent(errorFromErrorString);
		}
	}

	public void onOfferwallOpened(string empty)
	{
		if (SupersonicEvents._onOfferwallOpenedEvent != null)
		{
			SupersonicEvents._onOfferwallOpenedEvent();
		}
	}

	public void onOfferwallShowFail(string description)
	{
		if (SupersonicEvents._onOfferwallShowFailEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onOfferwallShowFailEvent(errorFromErrorString);
		}
	}

	public void onOfferwallClosed(string empty)
	{
		if (SupersonicEvents._onOfferwallClosedEvent != null)
		{
			SupersonicEvents._onOfferwallClosedEvent();
		}
	}

	public void onGetOfferwallCreditsFail(string description)
	{
		if (SupersonicEvents._onGetOfferwallCreditsFailEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onGetOfferwallCreditsFailEvent(errorFromErrorString);
		}
	}

	public void onOfferwallAdCredited(string json)
	{
		if (SupersonicEvents._onOfferwallAdCreditedEvent != null)
		{
			SupersonicEvents._onOfferwallAdCreditedEvent(Json.Deserialize(json) as Dictionary<string, object>);
		}
	}

	public SupersonicError getErrorFromErrorString(string description)
	{
		if (!string.IsNullOrEmpty(description))
		{
			Dictionary<string, object> dictionary = Json.Deserialize(description) as Dictionary<string, object>;
			if (dictionary != null)
			{
				int errCode = Convert.ToInt32(dictionary["error_code"].ToString());
				string errDescription = dictionary["error_description"].ToString();
				return new SupersonicError(errCode, errDescription);
			}
			return new SupersonicError(-1, string.Empty);
		}
		return new SupersonicError(-1, string.Empty);
	}

	public SupersonicPlacement getPlacementFromString(string jsonPlacement)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(jsonPlacement) as Dictionary<string, object>;
		int rAmount = Convert.ToInt32(dictionary["placement_reward_amount"].ToString());
		string rName = dictionary["placement_reward_name"].ToString();
		string pName = dictionary["placement_name"].ToString();
		return new SupersonicPlacement(pName, rName, rAmount);
	}
}
