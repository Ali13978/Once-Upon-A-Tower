using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.UniversalSDK;

public class empty : MonoBehaviour
{
    private UniversalSDK _universalSdk;

    public static empty instance;
    private void Awake()
    {
        instance = this;
    }

    public void Initialize()
    {
        _universalSdk = new GameObject(nameof(UniversalSDK)).AddComponent<UniversalSDK>();
    }

    public void OpenCustomTabView(string url)
    {
        _universalSdk.OpenCustomTabView(url/*, result =>
        {
            result.Match(
                value =>
                {
                    Debug.Log(value);
                },
                error =>
                {
                    Debug.LogError(error);
                };
        }*/);
    }
}
