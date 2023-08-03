using GameAnalyticsSDK.Wrapper;

namespace GameAnalyticsSDK.Events
{
	public static class GA_Business
	{
		public static void NewEventGooglePlay(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string signature)
		{
			GA_Wrapper.AddBusinessEventWithReceipt(currency, amount, itemType, itemId, cartType, receipt, "google_play", signature);
		}

		public static void NewEvent(string currency, int amount, string itemType, string itemId, string cartType)
		{
			GA_Wrapper.AddBusinessEvent(currency, amount, itemType, itemId, cartType);
		}
	}
}
