using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Packages.WcwUnity.Src;
using WaxCloudWalletUnity.Examples.Ui;
using Newtonsoft.Json;
using UnityEngine;


public class WaxToolkitManager : MonoBehaviour
{
    [SerializeField] Splash splash;
    WaxCloudWalletPlugin waxCloudWalletPlugin;

    [System.NonSerialized]
    public string PlayerAccount;

    public static WaxToolkitManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        waxCloudWalletPlugin = new GameObject(nameof(WaxCloudWalletPlugin)).AddComponent<WaxCloudWalletPlugin>();

        waxCloudWalletPlugin.OnInit += (initEvent) =>
        {
            Debug.Log("WaxJs Initialized: ");
        };

        waxCloudWalletPlugin.OnLoggedIn += (loginEvent) =>
        {
            
            PlayerAccount = loginEvent.Account;
            Debug.Log($"{PlayerAccount} Logged In");
            splash.enabled = true;

        };

        waxCloudWalletPlugin.OnError += (errorEvent) =>
        {
            Debug.Log(errorEvent.Message);
        };

        waxCloudWalletPlugin.OnInfoCreated += (infoCreatedEvent) =>
        {
            Debug.Log(JsonConvert.SerializeObject(infoCreatedEvent.Result));
        };

        waxCloudWalletPlugin.OnLogout += (logoutEvent) =>
        {
            Debug.Log($"LogoutResult: {logoutEvent.LogoutResult}");
        };

        #region Initialize Plugin
#if UNITY_WEBGL
            waxCloudWalletPlugin.InitializeWebGl("https://wax.greymass.com");
#elif UNTIY_ANDROID || UNITY_IOS
            waxCloudWalletPlugin.InitializeMobile(1234, "http://127.0.0.1:1234/index.html", true, indexHtmlString, waxJsString);
#else
        waxCloudWalletPlugin.InitializeDesktop(1234, "http://127.0.0.1:1234/index.html");
#endif
        #endregion

        Login();
    }

    public void Login()
    {
        waxCloudWalletPlugin.Login();
    }
}
