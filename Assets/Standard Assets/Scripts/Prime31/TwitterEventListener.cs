using UnityEngine;

namespace Prime31
{
	public class TwitterEventListener : MonoBehaviour
	{
		private void OnEnable()
		{
			TwitterManager.twitterInitializedEvent += twitterInitializedEvent;
			TwitterManager.loginSucceededEvent += loginSucceeded;
			TwitterManager.loginFailedEvent += loginFailed;
			TwitterManager.requestDidFinishEvent += requestDidFinishEvent;
			TwitterManager.requestDidFailEvent += requestDidFailEvent;
			TwitterManager.tweetSheetCompletedEvent += tweetSheetCompletedEvent;
		}

		private void OnDisable()
		{
			TwitterManager.twitterInitializedEvent -= twitterInitializedEvent;
			TwitterManager.loginSucceededEvent -= loginSucceeded;
			TwitterManager.loginFailedEvent -= loginFailed;
			TwitterManager.requestDidFinishEvent -= requestDidFinishEvent;
			TwitterManager.requestDidFailEvent -= requestDidFailEvent;
			TwitterManager.tweetSheetCompletedEvent -= tweetSheetCompletedEvent;
		}

		private void twitterInitializedEvent()
		{
			UnityEngine.Debug.Log("twitterInitializedEvent");
		}

		private void loginSucceeded(string username)
		{
			UnityEngine.Debug.Log("Successfully logged in to Twitter: " + username);
		}

		private void loginFailed(string error)
		{
			UnityEngine.Debug.Log("Twitter login failed: " + error);
		}

		private void requestDidFailEvent(string error)
		{
			UnityEngine.Debug.Log("requestDidFailEvent: " + error);
		}

		private void requestDidFinishEvent(object result)
		{
			if (result != null)
			{
				UnityEngine.Debug.Log("requestDidFinishEvent");
				Utils.logObject(result);
			}
		}

		private void tweetSheetCompletedEvent(bool didSucceed)
		{
			UnityEngine.Debug.Log("tweetSheetCompletedEvent didSucceed: " + didSucceed);
		}
	}
}
