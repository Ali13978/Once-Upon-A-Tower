using SupersonicJSON;
using System.Collections.Generic;
using UnityEngine;

public class SupersonicConfig
{
	private const string unsupportedPlatformStr = "Unsupported Platform";

	private static SupersonicConfig mInstance;

	private static AndroidJavaObject _androidBridge;

	private static readonly string AndroidBridge = "com.supersonic.unity.androidbridge.AndroidBridge";

	public static SupersonicConfig Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = new SupersonicConfig();
			}
			return mInstance;
		}
	}

	public SupersonicConfig()
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidBridge))
		{
			_androidBridge = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
		}
	}

	public void setMaxVideoLength(int length)
	{
		_androidBridge.Call("setSupersonicMaxVideoLength", length);
	}

	public void setLanguage(string language)
	{
		_androidBridge.Call("setSupersonicLanguage", language);
	}

	public void setClientSideCallbacks(bool status)
	{
		_androidBridge.Call("setSupersonicClientSideCallbacks", status);
	}

	public void setPrivateKey(string key)
	{
		_androidBridge.Call("setSupersonicPrivateKey", key);
	}

	public void setItemName(string name)
	{
		_androidBridge.Call("setSupersonicItemName", name);
	}

	public void setItemCount(int count)
	{
		_androidBridge.Call("setSupersonicItemCount", count);
	}

	public void setRewardedVideoCustomParams(Dictionary<string, string> rvCustomParams)
	{
		string text = Json.Serialize(rvCustomParams);
		_androidBridge.Call("setSupersonicRewardedVideoCustomParams", text);
	}

	public void setOfferwallCustomParams(Dictionary<string, string> owCustomParams)
	{
		string text = Json.Serialize(owCustomParams);
		_androidBridge.Call("setSupersonicOfferwallCustomParams", text);
	}
}
