using System.Collections.Generic;
using UnityEngine;

namespace Prime31
{
	public class FacebookEventListener : MonoBehaviour
	{
		private void OnEnable()
		{
			FacebookManager.sessionOpenedEvent += sessionOpenedEvent;
			FacebookManager.loginFailedEvent += loginFailedEvent;
			FacebookManager.graphRequestCompletedEvent += graphRequestCompletedEvent;
			FacebookManager.graphRequestFailedEvent += facebookCustomRequestFailed;
			FacebookManager.facebookComposerCompletedEvent += facebookComposerCompletedEvent;
			FacebookManager.shareDialogFailedEvent += shareDialogFailedEvent;
			FacebookManager.shareDialogSucceededEvent += shareDialogSucceededEvent;
			FacebookManager.gameDialogFailedEvent += gameDialogFailedEvent;
			FacebookManager.gameDialogSucceededEvent += gameDialogSucceededEvent;
		}

		private void OnDisable()
		{
			FacebookManager.sessionOpenedEvent -= sessionOpenedEvent;
			FacebookManager.loginFailedEvent -= loginFailedEvent;
			FacebookManager.graphRequestCompletedEvent -= graphRequestCompletedEvent;
			FacebookManager.graphRequestFailedEvent -= facebookCustomRequestFailed;
			FacebookManager.facebookComposerCompletedEvent -= facebookComposerCompletedEvent;
			FacebookManager.shareDialogFailedEvent -= shareDialogFailedEvent;
			FacebookManager.shareDialogSucceededEvent -= shareDialogSucceededEvent;
			FacebookManager.gameDialogFailedEvent -= gameDialogFailedEvent;
			FacebookManager.gameDialogSucceededEvent -= gameDialogSucceededEvent;
		}

		private void sessionOpenedEvent()
		{
			UnityEngine.Debug.Log("Successfully logged in to Facebook");
		}

		private void loginFailedEvent(P31Error error)
		{
			UnityEngine.Debug.Log("Facebook login failed: " + error);
		}

		private void facebokDialogCompleted()
		{
			UnityEngine.Debug.Log("facebokDialogCompleted");
		}

		private void graphRequestCompletedEvent(object obj)
		{
			UnityEngine.Debug.Log("graphRequestCompletedEvent");
			Utils.logObject(obj);
		}

		private void facebookCustomRequestFailed(P31Error error)
		{
			UnityEngine.Debug.Log("facebookCustomRequestFailed failed: " + error);
		}

		private void facebookComposerCompletedEvent(bool didSucceed)
		{
			UnityEngine.Debug.Log("facebookComposerCompletedEvent did succeed: " + didSucceed);
		}

		private void shareDialogFailedEvent(P31Error error)
		{
			UnityEngine.Debug.Log("shareDialogFailedEvent: " + error);
		}

		private void shareDialogSucceededEvent(string postId)
		{
			UnityEngine.Debug.Log("shareDialogSucceededEvent: " + postId);
		}

		private void gameDialogFailedEvent(P31Error error)
		{
			UnityEngine.Debug.Log("gameDialogFailedEvent: " + error);
		}

		private void gameDialogSucceededEvent(Dictionary<string, object> dict)
		{
			UnityEngine.Debug.Log("gameDialogSucceededEvent");
			Utils.logObject(dict);
		}
	}
}
