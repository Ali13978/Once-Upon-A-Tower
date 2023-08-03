using System;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
	protected static T instance;

	public static T Instance => GetInstance();

	protected virtual void Awake()
	{
		if ((UnityEngine.Object)instance != (UnityEngine.Object)null && instance != this)
		{
			UnityEngine.Debug.LogError("There are two instances of " + typeof(T).FullName + "!");
		}
		else
		{
			instance = (T)this;
		}
	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			instance = (T)null;
		}
	}

	public static T GetInstance()
	{
		if ((UnityEngine.Object)instance != (UnityEngine.Object)null)
		{
			return instance;
		}
		instance = (T)UnityEngine.Object.FindObjectOfType(typeof(T));
		if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
		{
			throw new InvalidOperationException("There is no " + typeof(T).FullName + "!");
		}
		return instance;
	}

	public static bool HasInstance()
	{
		if ((UnityEngine.Object)instance != (UnityEngine.Object)null)
		{
			return true;
		}
		instance = (T)UnityEngine.Object.FindObjectOfType(typeof(T));
		return (UnityEngine.Object)instance != (UnityEngine.Object)null;
	}
}
