using System;
using System.Collections.Generic;

namespace Prime31
{
	public class FacebookManager : AbstractManager
	{
		public static event Action sessionOpenedEvent;

		public static event Action preLoginSucceededEvent;

		public static event Action<P31Error> loginFailedEvent;

		public static event Action<object> graphRequestCompletedEvent;

		public static event Action<P31Error> graphRequestFailedEvent;

		public static event Action<bool> facebookComposerCompletedEvent;

		public static event Action<string> shareDialogSucceededEvent;

		public static event Action<P31Error> shareDialogFailedEvent;

		public static event Action<Dictionary<string, object>> gameDialogSucceededEvent;

		public static event Action<P31Error> gameDialogFailedEvent;

		static FacebookManager()
		{
			AbstractManager.initialize(typeof(FacebookManager));
		}

		public void sessionOpened(string accessToken)
		{
			FacebookManager.preLoginSucceededEvent.fire();
			Facebook.instance.accessToken = accessToken;
			FacebookManager.sessionOpenedEvent.fire();
		}

		public void loginFailed(string json)
		{
			FacebookManager.loginFailedEvent.fire(P31Error.errorFromJson(json));
		}

		public void graphRequestCompleted(string json)
		{
			if (FacebookManager.graphRequestCompletedEvent != null)
			{
				object param = Json.decode(json);
				FacebookManager.graphRequestCompletedEvent.fire(param);
			}
		}

		public void graphRequestFailed(string json)
		{
			FacebookManager.graphRequestFailedEvent.fire(P31Error.errorFromJson(json));
		}

		public void facebookComposerCompleted(string result)
		{
			FacebookManager.facebookComposerCompletedEvent.fire(result == "1");
		}

		public void shareDialogFailed(string json)
		{
			FacebookManager.shareDialogFailedEvent.fire(P31Error.errorFromJson(json));
		}

		public void shareDialogSucceeded(string postId)
		{
			FacebookManager.shareDialogSucceededEvent.fire(postId);
		}

		public void gameDialogFailed(string json)
		{
			FacebookManager.gameDialogFailedEvent.fire(P31Error.errorFromJson(json));
		}

		public void gameDialogSucceeded(string json)
		{
			FacebookManager.gameDialogSucceededEvent.fire(json.dictionaryFromJson());
		}
	}
}
