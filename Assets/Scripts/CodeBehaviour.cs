using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeBehaviour : SingletonMonoBehaviour<CodeBehaviour>
{
	private const string baseUrl = "http://codes.onceuponatower.com/redeem/";

	private static List<string> processedUrls = new List<string>();

	private void Start()
	{
		StartCoroutine(GetLinkCoroutine());
	}

	private IEnumerator GetLinkCoroutine()
	{
		while (!SingletonMonoBehaviour<Game>.HasInstance() || !SingletonMonoBehaviour<Game>.Instance.Ready)
		{
			yield return null;
		}
		yield return new WaitForSeconds(0.5f);
		GetLink();
	}

	private void OnApplicationPause(bool paused)
	{
		if (!paused)
		{
			GetLink();
		}
	}

	private void OnApplicationFocus(bool focus)
	{
		if (focus)
		{
			GetLink();
		}
	}

	private void ProcessUrl(string url)
	{
		if (url.StartsWith("towergame://"))
		{
			string key = url.Substring("towergame://".Length);
			GetCode(key);
		}
	}

	public void GetCode(string key)
	{
		StartCoroutine(ProcessKey(key));
	}

	private IEnumerator ProcessKey(string key)
	{
		WWW www = new WWW("http://codes.onceuponatower.com/redeem/" + key);
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			UnityEngine.Debug.Log("Applying code " + key);
			CodeResult codeResult = new CodeResult(www.text);
			codeResult.Apply();
		}
		else
		{
			UnityEngine.Debug.Log("Error processing code: " + www.error);
		}
	}

	private void GetLink()
	{
		GetLinkAndroid();
	}

	private void GetLinkAndroid()
	{
		try
		{
			AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getIntent", new object[0]);
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getData", new object[0]);
			string text = androidJavaObject2.Call<string>("toString", new object[0]);
			if (!string.IsNullOrEmpty(text) && !processedUrls.Contains(text))
			{
				UnityEngine.Debug.Log("Deep Link Android: " + text);
				ProcessUrl(text);
				processedUrls.Add(text);
			}
		}
		catch
		{
		}
	}
}
