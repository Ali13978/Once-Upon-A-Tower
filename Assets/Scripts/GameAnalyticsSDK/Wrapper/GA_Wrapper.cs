using GameAnalyticsSDK.State;
using GameAnalyticsSDK.Utilities;
using System.Collections;
using UnityEngine;

namespace GameAnalyticsSDK.Wrapper
{
	public class GA_Wrapper
	{
		private static readonly AndroidJavaClass GA = new AndroidJavaClass("com.gameanalytics.sdk.GameAnalytics");

		private static void configureAvailableCustomDimensions01(string list)
		{
			ArrayList arrayList = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GA.CallStatic("configureAvailableCustomDimensions01", arrayList.ToArray(typeof(string)));
		}

		private static void configureAvailableCustomDimensions02(string list)
		{
			ArrayList arrayList = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GA.CallStatic("configureAvailableCustomDimensions02", arrayList.ToArray(typeof(string)));
		}

		private static void configureAvailableCustomDimensions03(string list)
		{
			ArrayList arrayList = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GA.CallStatic("configureAvailableCustomDimensions03", arrayList.ToArray(typeof(string)));
		}

		private static void configureAvailableResourceCurrencies(string list)
		{
			ArrayList arrayList = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GA.CallStatic("configureAvailableResourceCurrencies", arrayList.ToArray(typeof(string)));
		}

		private static void configureAvailableResourceItemTypes(string list)
		{
			ArrayList arrayList = (ArrayList)GA_MiniJSON.JsonDecode(list);
			GA.CallStatic("configureAvailableResourceItemTypes", arrayList.ToArray(typeof(string)));
		}

		private static void configureSdkGameEngineVersion(string unitySdkVersion)
		{
			GA.CallStatic("configureSdkGameEngineVersion", unitySdkVersion);
		}

		private static void configureGameEngineVersion(string unityEngineVersion)
		{
			GA.CallStatic("configureGameEngineVersion", unityEngineVersion);
		}

		private static void configureBuild(string build)
		{
			GA.CallStatic("configureBuild", build);
		}

		private static void configureUserId(string userId)
		{
			GA.CallStatic("configureUserId", userId);
		}

		private static void initialize(string gamekey, string gamesecret)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.gameanalytics.sdk.GAPlatform");
			androidJavaClass2.CallStatic("initializeWithActivity", @static);
			GA.CallStatic("initializeWithGameKey", gamekey, gamesecret);
		}

		private static void setCustomDimension01(string customDimension)
		{
			GA.CallStatic("setCustomDimension01", customDimension);
		}

		private static void setCustomDimension02(string customDimension)
		{
			GA.CallStatic("setCustomDimension02", customDimension);
		}

		private static void setCustomDimension03(string customDimension)
		{
			GA.CallStatic("setCustomDimension03", customDimension);
		}

		private static void addBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType)
		{
			GA.CallStatic("addBusinessEventWithCurrency", currency, amount, itemType, itemId, cartType);
		}

		private static void addBusinessEventWithReceipt(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string store, string signature)
		{
			GA.CallStatic("addBusinessEventWithCurrency", currency, amount, itemType, itemId, cartType, receipt, store, signature);
		}

		private static void addResourceEvent(int flowType, string currency, float amount, string itemType, string itemId)
		{
			GA.CallStatic("addResourceEventWithFlowType", flowType, currency, amount, itemType, itemId);
		}

		private static void addProgressionEvent(int progressionStatus, string progression01, string progression02, string progression03)
		{
			GA.CallStatic("addProgressionEventWithProgressionStatus", progressionStatus, progression01, progression02, progression03);
		}

		private static void addProgressionEventWithScore(int progressionStatus, string progression01, string progression02, string progression03, int score)
		{
			GA.CallStatic("addProgressionEventWithProgressionStatus", progressionStatus, progression01, progression02, progression03, (double)score);
		}

		private static void addDesignEvent(string eventId)
		{
			GA.CallStatic("addDesignEventWithEventId", eventId);
		}

		private static void addDesignEventWithValue(string eventId, float value)
		{
			GA.CallStatic("addDesignEventWithEventId", eventId, (double)value);
		}

		private static void addErrorEvent(int severity, string message)
		{
			GA.CallStatic("addErrorEventWithSeverity", severity, message);
		}

		private static void setEnabledInfoLog(bool enabled)
		{
			GA.CallStatic("setEnabledInfoLog", enabled);
		}

		private static void setEnabledVerboseLog(bool enabled)
		{
			GA.CallStatic("setEnabledVerboseLog", enabled);
		}

		private static void setFacebookId(string facebookId)
		{
			GA.CallStatic("setFacebookId", facebookId);
		}

		private static void setGender(string gender)
		{
			if (gender == null)
			{
				return;
			}
			if (!(gender == "male"))
			{
				if (gender == "female")
				{
					GA.CallStatic("setGender", 2);
				}
			}
			else
			{
				GA.CallStatic("setGender", 1);
			}
		}

		private static void setBirthYear(int birthYear)
		{
			GA.CallStatic("setBirthYear", birthYear);
		}

		private static void setManualSessionHandling(bool enabled)
		{
			GA.CallStatic("setEnabledManualSessionHandling", enabled);
		}

		private static void gameAnalyticsStartSession()
		{
			GA.CallStatic("startSession");
		}

		private static void gameAnalyticsEndSession()
		{
			GA.CallStatic("endSession");
		}

		public static void SetAvailableCustomDimensions01(string list)
		{
			configureAvailableCustomDimensions01(list);
		}

		public static void SetAvailableCustomDimensions02(string list)
		{
			configureAvailableCustomDimensions02(list);
		}

		public static void SetAvailableCustomDimensions03(string list)
		{
			configureAvailableCustomDimensions03(list);
		}

		public static void SetAvailableResourceCurrencies(string list)
		{
			configureAvailableResourceCurrencies(list);
		}

		public static void SetAvailableResourceItemTypes(string list)
		{
			configureAvailableResourceItemTypes(list);
		}

		public static void SetUnitySdkVersion(string unitySdkVersion)
		{
			configureSdkGameEngineVersion(unitySdkVersion);
		}

		public static void SetUnityEngineVersion(string unityEngineVersion)
		{
			configureGameEngineVersion(unityEngineVersion);
		}

		public static void SetBuild(string build)
		{
			configureBuild(build);
		}

		public static void SetCustomUserId(string userId)
		{
			configureUserId(userId);
		}

		public static void SetEnabledManualSessionHandling(bool enabled)
		{
			setManualSessionHandling(enabled);
		}

		public static void StartSession()
		{
			if (GAState.IsManualSessionHandlingEnabled())
			{
				gameAnalyticsStartSession();
			}
			else
			{
				UnityEngine.Debug.Log("Manual session handling is not enabled. \nPlease check the \"Use manual session handling\" option in the \"Advanced\" section of the Settings object.");
			}
		}

		public static void EndSession()
		{
			if (GAState.IsManualSessionHandlingEnabled())
			{
				gameAnalyticsEndSession();
			}
			else
			{
				UnityEngine.Debug.Log("Manual session handling is not enabled. \nPlease check the \"Use manual session handling\" option in the \"Advanced\" section of the Settings object.");
			}
		}

		public static void Initialize(string gamekey, string gamesecret)
		{
			initialize(gamekey, gamesecret);
		}

		public static void SetCustomDimension01(string customDimension)
		{
			setCustomDimension01(customDimension);
		}

		public static void SetCustomDimension02(string customDimension)
		{
			setCustomDimension02(customDimension);
		}

		public static void SetCustomDimension03(string customDimension)
		{
			setCustomDimension03(customDimension);
		}

		public static void AddBusinessEventWithReceipt(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string store, string signature)
		{
			addBusinessEventWithReceipt(currency, amount, itemType, itemId, cartType, receipt, store, signature);
		}

		public static void AddBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType)
		{
			addBusinessEvent(currency, amount, itemType, itemId, cartType);
		}

		public static void AddResourceEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId)
		{
			addResourceEvent((int)flowType, currency, amount, itemType, itemId);
		}

		public static void AddProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03)
		{
			addProgressionEvent((int)progressionStatus, progression01, progression02, progression03);
		}

		public static void AddProgressionEventWithScore(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score)
		{
			addProgressionEventWithScore((int)progressionStatus, progression01, progression02, progression03, score);
		}

		public static void AddDesignEvent(string eventID, float eventValue)
		{
			addDesignEventWithValue(eventID, eventValue);
		}

		public static void AddDesignEvent(string eventID)
		{
			addDesignEvent(eventID);
		}

		public static void AddErrorEvent(GAErrorSeverity severity, string message)
		{
			addErrorEvent((int)severity, message);
		}

		public static void SetInfoLog(bool enabled)
		{
			setEnabledInfoLog(enabled);
		}

		public static void SetVerboseLog(bool enabled)
		{
			setEnabledVerboseLog(enabled);
		}

		public static void SetFacebookId(string facebookId)
		{
			setFacebookId(facebookId);
		}

		public static void SetGender(string gender)
		{
			setGender(gender);
		}

		public static void SetBirthYear(int birthYear)
		{
			setBirthYear(birthYear);
		}
	}
}
