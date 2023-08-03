using SupersonicJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AndroidAgent : SupersonicIAgent
{
	private static AndroidJavaObject _androidBridge;

	private static readonly string AndroidBridge = "com.supersonic.unity.androidbridge.AndroidBridge";

	private const string REWARD_AMOUNT = "reward_amount";

	private const string REWARD_NAME = "reward_name";

	private const string PLACEMENT_NAME = "placement_name";

	public AndroidAgent()
	{
		UnityEngine.Debug.Log("AndroidAgent ctr");
	}

	private AndroidJavaObject getBridge()
	{
		if (_androidBridge == null)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidBridge))
			{
				_androidBridge = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
			}
		}
		return _androidBridge;
	}

	public void start()
	{
		UnityEngine.Debug.Log("Android started");
		getBridge().Call("setPluginData", "Unity", Supersonic.pluginVersion(), Supersonic.unityVersion());
		UnityEngine.Debug.Log("Android started - ended");
	}

	public void reportAppStarted()
	{
		getBridge().Call("reportAppStarted");
	}

	public void onResume()
	{
		getBridge().Call("onResume");
	}

	public void onPause()
	{
		getBridge().Call("onPause");
	}

	public void setAge(int age)
	{
		getBridge().Call("setAge", age);
	}

	public void setGender(string gender)
	{
		getBridge().Call("setGender", gender);
	}

	public void setMediationSegment(string segment)
	{
		getBridge().Call("setMediationSegment", segment);
	}

	public bool setDynamicUserId(string dynamicUserId)
	{
		return getBridge().Call<bool>("setDynamicUserId", new object[1]
		{
			dynamicUserId
		});
	}

	public void initRewardedVideo(string appKey, string userId)
	{
		getBridge().Call("initRewardedVideo", appKey, userId);
	}

	public void showRewardedVideo()
	{
		getBridge().Call("showRewardedVideo");
	}

	public void showRewardedVideo(string placementName)
	{
		getBridge().Call("showRewardedVideo", placementName);
	}

	public bool isRewardedVideoAvailable()
	{
		return getBridge().Call<bool>("isRewardedVideoAvailable", new object[0]);
	}

	public bool isRewardedVideoPlacementCapped(string placementName)
	{
		return getBridge().Call<bool>("isRewardedVideoPlacementCapped", new object[1]
		{
			placementName
		});
	}

	public string getAdvertiserId()
	{
		return getBridge().Call<string>("getAdvertiserId", new object[0]);
	}

	public void shouldTrackNetworkState(bool track)
	{
		getBridge().Call("shouldTrackNetworkState", track);
	}

	public void validateIntegration()
	{
		getBridge().Call("validateIntegration");
	}

	public SupersonicPlacement getPlacementInfo(string placementName)
	{
		string text = getBridge().Call<string>("getPlacementInfo", new object[1]
		{
			placementName
		});
		SupersonicPlacement result = null;
		if (text != null)
		{
			Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
			string pName = dictionary["placement_name"].ToString();
			string rName = dictionary["reward_name"].ToString();
			int rAmount = Convert.ToInt32(dictionary["reward_amount"].ToString());
			result = new SupersonicPlacement(pName, rName, rAmount);
		}
		return result;
	}

	public void initInterstitial(string appKey, string userId)
	{
		getBridge().Call("initInterstitial", appKey, userId);
	}

	public void loadInterstitial()
	{
		getBridge().Call("loadInterstitial");
	}

	public void showInterstitial()
	{
		getBridge().Call("showInterstitial");
	}

	public void showInterstitial(string placementName)
	{
		getBridge().Call("showInterstitial", placementName);
	}

	public bool isInterstitialReady()
	{
		return getBridge().Call<bool>("isInterstitialReady", new object[0]);
	}

	public bool isInterstitialPlacementCapped(string placementName)
	{
		return getBridge().Call<bool>("isInterstitialPlacementCapped", new object[1]
		{
			placementName
		});
	}

	public void initOfferwall(string appKey, string userId)
	{
		getBridge().Call("initOfferwall", appKey, userId);
	}

	public void showOfferwall()
	{
		getBridge().Call("showOfferwall");
	}

	public void showOfferwall(string placementName)
	{
		getBridge().Call("showOfferwall", placementName);
	}

	public void getOfferwallCredits()
	{
		getBridge().Call("getOfferwallCredits");
	}

	public bool isOfferwallAvailable()
	{
		return getBridge().Call<bool>("isOfferwallAvailable", new object[0]);
	}
}
