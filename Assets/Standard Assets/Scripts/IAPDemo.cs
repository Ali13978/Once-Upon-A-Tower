using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Store;
using UnityEngine.UI;

[AddComponentMenu("Unity IAP/Demo")]
public class IAPDemo : MonoBehaviour, IStoreListener
{
	[Serializable]
	public class UnityChannelPurchaseError
	{
		public string error;

		public UnityChannelPurchaseInfo purchaseInfo;
	}

	[Serializable]
	public class UnityChannelPurchaseInfo
	{
		public string productCode;

		public string gameOrderId;

		public string orderQueryToken;
	}

	private class UnityChannelLoginHandler : ILoginListener
	{
		internal Action initializeSucceededAction;

		internal Action<string> initializeFailedAction;

		internal Action<UserInfo> loginSucceededAction;

		internal Action<string> loginFailedAction;

		public void OnInitialized()
		{
			initializeSucceededAction();
		}

		public void OnInitializeFailed(string message)
		{
			initializeFailedAction(message);
		}

		public void OnLogin(UserInfo userInfo)
		{
			loginSucceededAction(userInfo);
		}

		public void OnLoginFailed(string message)
		{
			loginFailedAction(message);
		}
	}

	private IStoreController m_Controller;

	private IAppleExtensions m_AppleExtensions;

	private IMoolahExtension m_MoolahExtensions;

	private ISamsungAppsExtensions m_SamsungExtensions;

	private IMicrosoftExtensions m_MicrosoftExtensions;

	private IUnityChannelExtensions m_UnityChannelExtensions;

	private bool m_IsGooglePlayStoreSelected;

	private bool m_IsSamsungAppsStoreSelected;

	private bool m_IsCloudMoolahStoreSelected;

	private bool m_IsUnityChannelSelected;

	private string m_LastTransationID;

	private string m_LastReceipt;

	private string m_CloudMoolahUserName;

	private bool m_IsLoggedIn;

	private UnityChannelLoginHandler unityChannelLoginHandler;

	private bool m_FetchReceiptPayloadOnPurchase;

	private int m_SelectedItemIndex = -1;

	private bool m_PurchaseInProgress;

	private Selectable m_InteractableSelectable;

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		m_Controller = controller;
		m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
		m_SamsungExtensions = extensions.GetExtension<ISamsungAppsExtensions>();
		m_MoolahExtensions = extensions.GetExtension<IMoolahExtension>();
		m_MicrosoftExtensions = extensions.GetExtension<IMicrosoftExtensions>();
		m_UnityChannelExtensions = extensions.GetExtension<IUnityChannelExtensions>();
		InitUI(controller.products.all);
		m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);
		UnityEngine.Debug.Log("Available items:");
		Product[] all = controller.products.all;
		foreach (Product product in all)
		{
			if (product.availableToPurchase)
			{
				UnityEngine.Debug.Log(string.Join(" - ", new string[7]
				{
					product.metadata.localizedTitle,
					product.metadata.localizedDescription,
					product.metadata.isoCurrencyCode,
					product.metadata.localizedPrice.ToString(),
					product.metadata.localizedPriceString,
					product.transactionID,
					product.receipt
				}));
			}
		}
		if (m_Controller.products.all.Length > 0)
		{
			m_SelectedItemIndex = 0;
		}
		for (int j = 0; j < m_Controller.products.all.Length; j++)
		{
			Product product2 = m_Controller.products.all[j];
			string text = $"{product2.metadata.localizedTitle} | {product2.metadata.localizedPriceString} => {product2.metadata.localizedPrice}";
			GetDropdown().options[j] = new Dropdown.OptionData(text);
		}
		GetDropdown().RefreshShownValue();
		UpdateHistoryUI();
		LogProductDefinitions();
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
	{
		UnityEngine.Debug.Log("Purchase OK: " + e.purchasedProduct.definition.id);
		UnityEngine.Debug.Log("Receipt: " + e.purchasedProduct.receipt);
		m_LastTransationID = e.purchasedProduct.transactionID;
		m_LastReceipt = e.purchasedProduct.receipt;
		m_PurchaseInProgress = false;
		if (m_IsUnityChannelSelected)
		{
			UnifiedReceipt unifiedReceipt = JsonUtility.FromJson<UnifiedReceipt>(e.purchasedProduct.receipt);
			if (unifiedReceipt != null && !string.IsNullOrEmpty(unifiedReceipt.Payload))
			{
				UnityChannelPurchaseReceipt unityChannelPurchaseReceipt = JsonUtility.FromJson<UnityChannelPurchaseReceipt>(unifiedReceipt.Payload);
				UnityEngine.Debug.LogFormat("UnityChannel receipt: storeSpecificId = {0}, transactionId = {1}, orderQueryToken = {2}", unityChannelPurchaseReceipt.storeSpecificId, unityChannelPurchaseReceipt.transactionId, unityChannelPurchaseReceipt.orderQueryToken);
			}
		}
		if (m_IsCloudMoolahStoreSelected)
		{
			if (m_IsCloudMoolahStoreSelected)
			{
				m_MoolahExtensions.RequestPayOut(e.purchasedProduct.transactionID, delegate(string transactionID, RequestPayOutState state, string message)
				{
					if (state == RequestPayOutState.RequestPayOutSucceed)
					{
						m_Controller.ConfirmPendingPurchase(e.purchasedProduct);
					}
					else
					{
						UnityEngine.Debug.Log("RequestPayOut: failed. transactionID: " + transactionID + ", state: " + state + ", message: " + message);
					}
				});
			}
			return PurchaseProcessingResult.Pending;
		}
		UpdateHistoryUI();
		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
	{
		UnityEngine.Debug.Log("Purchase failed: " + item.definition.id);
		UnityEngine.Debug.Log(r);
		if (m_IsUnityChannelSelected)
		{
			string lastPurchaseError = m_UnityChannelExtensions.GetLastPurchaseError();
			UnityChannelPurchaseError unityChannelPurchaseError = JsonUtility.FromJson<UnityChannelPurchaseError>(lastPurchaseError);
			if (unityChannelPurchaseError != null && unityChannelPurchaseError.purchaseInfo != null)
			{
				UnityChannelPurchaseInfo purchaseInfo = unityChannelPurchaseError.purchaseInfo;
				UnityEngine.Debug.LogFormat("UnityChannel purchaseInfo: productCode = {0}, gameOrderId = {1}, orderQueryToken = {2}", purchaseInfo.productCode, purchaseInfo.gameOrderId, purchaseInfo.orderQueryToken);
			}
			if (r == PurchaseFailureReason.Unknown && unityChannelPurchaseError != null && unityChannelPurchaseError.error != null && unityChannelPurchaseError.error.Equals("DuplicateTransaction"))
			{
				UnityEngine.Debug.Log("Duplicate transaction detected, unlock this item");
			}
		}
		m_PurchaseInProgress = false;
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		UnityEngine.Debug.Log("Billing failed to initialize!");
		switch (error)
		{
		case InitializationFailureReason.AppNotKnown:
			UnityEngine.Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
			break;
		case InitializationFailureReason.PurchasingUnavailable:
			UnityEngine.Debug.Log("Billing disabled!");
			break;
		case InitializationFailureReason.NoProductsAvailable:
			UnityEngine.Debug.Log("No products available for purchase!");
			break;
		}
	}

	public void Awake()
	{
		StandardPurchasingModule standardPurchasingModule = StandardPurchasingModule.Instance();
		standardPurchasingModule.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
		ConfigurationBuilder builder = ConfigurationBuilder.Instance(standardPurchasingModule);
		builder.Configure<IMicrosoftConfiguration>().useMockBillingSystem = true;
		m_IsGooglePlayStoreSelected = (Application.platform == RuntimePlatform.Android && standardPurchasingModule.appStore == AppStore.GooglePlay);
		builder.Configure<IMoolahConfiguration>().appKey = "d93f4564c41d463ed3d3cd207594ee1b";
		builder.Configure<IMoolahConfiguration>().hashKey = "cc";
		builder.Configure<IMoolahConfiguration>().SetMode(CloudMoolahMode.AlwaysSucceed);
		m_IsCloudMoolahStoreSelected = (Application.platform == RuntimePlatform.Android && standardPurchasingModule.appStore == AppStore.CloudMoolah);
		m_IsUnityChannelSelected = (Application.platform == RuntimePlatform.Android && standardPurchasingModule.appStore == AppStore.XiaomiMiPay);
		builder.Configure<IUnityChannelConfiguration>().fetchReceiptPayloadOnPurchase = m_FetchReceiptPayloadOnPurchase;
		ProductCatalog productCatalog = ProductCatalog.LoadDefaultCatalog();
		foreach (ProductCatalogItem allProduct in productCatalog.allProducts)
		{
			if (allProduct.allStoreIDs.Count > 0)
			{
				IDs ds = new IDs();
				foreach (StoreID allStoreID in allProduct.allStoreIDs)
				{
					ds.Add(allStoreID.id, allStoreID.store);
				}
				builder.AddProduct(allProduct.id, allProduct.type, ds);
			}
			else
			{
				builder.AddProduct(allProduct.id, allProduct.type);
			}
		}
		builder.AddProduct("100.gold.coins", ProductType.Consumable, new IDs
		{
			{
				"100.gold.coins.mac",
				"MacAppStore"
			},
			{
				"000000596586",
				"TizenStore"
			},
			{
				"com.ff",
				"MoolahAppStore"
			}
		});
		builder.AddProduct("500.gold.coins", ProductType.Consumable, new IDs
		{
			{
				"500.gold.coins.mac",
				"MacAppStore"
			},
			{
				"000000596581",
				"TizenStore"
			},
			{
				"com.ee",
				"MoolahAppStore"
			}
		});
		builder.AddProduct("sword", ProductType.NonConsumable, new IDs
		{
			{
				"sword.mac",
				"MacAppStore"
			},
			{
				"000000596583",
				"TizenStore"
			}
		});
		builder.AddProduct("subscription", ProductType.Subscription, new IDs
		{
			{
				"subscription.mac",
				"MacAppStore"
			}
		});
		builder.Configure<IAmazonConfiguration>().WriteSandboxJSON(builder.products);
		builder.Configure<ISamsungAppsConfiguration>().SetMode(SamsungAppsMode.AlwaysSucceed);
		m_IsSamsungAppsStoreSelected = (Application.platform == RuntimePlatform.Android && standardPurchasingModule.appStore == AppStore.SamsungApps);
		builder.Configure<ITizenStoreConfiguration>().SetGroupId("100000085616");
		Action initializeUnityIap = delegate
		{
			UnityPurchasing.Initialize(this, builder);
		};
		if (!m_IsUnityChannelSelected)
		{
			initializeUnityIap();
			return;
		}
		AppInfo appInfo = new AppInfo();
		appInfo.appId = "abc123appId";
		appInfo.appKey = "efg456appKey";
		appInfo.clientId = "hij789clientId";
		appInfo.clientKey = "klm012clientKey";
		appInfo.debug = false;
		unityChannelLoginHandler = new UnityChannelLoginHandler();
		unityChannelLoginHandler.initializeFailedAction = delegate(string message)
		{
			UnityEngine.Debug.LogError("Failed to initialize and login to UnityChannel: " + message);
		};
		unityChannelLoginHandler.initializeSucceededAction = delegate
		{
			initializeUnityIap();
		};
		StoreService.Initialize(appInfo, unityChannelLoginHandler);
	}

	private void OnTransactionsRestored(bool success)
	{
		UnityEngine.Debug.Log("Transactions restored.");
	}

	private void OnDeferred(Product item)
	{
		UnityEngine.Debug.Log("Purchase deferred: " + item.definition.id);
	}

	private void InitUI(IEnumerable<Product> items)
	{
		m_InteractableSelectable = GetDropdown();
		if (!NeedRestoreButton())
		{
			GetRestoreButton().gameObject.SetActive(value: false);
		}
		GetRegisterButton().gameObject.SetActive(NeedRegisterButton());
		GetLoginButton().gameObject.SetActive(NeedLoginButton());
		GetValidateButton().gameObject.SetActive(NeedValidateButton());
		foreach (Product item in items)
		{
			string text = $"{item.definition.id} - {item.definition.type}";
			GetDropdown().options.Add(new Dropdown.OptionData(text));
		}
		GetDropdown().RefreshShownValue();
		GetDropdown().onValueChanged.AddListener(delegate(int selectedItem)
		{
			UnityEngine.Debug.Log("OnClickDropdown item " + selectedItem);
			m_SelectedItemIndex = selectedItem;
		});
		GetBuyButton().onClick.AddListener(delegate
		{
			if (m_PurchaseInProgress)
			{
				UnityEngine.Debug.Log("Please wait, purchasing ...");
			}
			else
			{
				if (NeedLoginButton() && !m_IsLoggedIn)
				{
					UnityEngine.Debug.LogWarning("Purchase notifications will not be forwarded server-to-server. Login incomplete.");
				}
				m_PurchaseInProgress = true;
				m_Controller.InitiatePurchase(m_Controller.products.all[m_SelectedItemIndex], "aDemoDeveloperPayload");
			}
		});
		if (GetRestoreButton() != null)
		{
			GetRestoreButton().onClick.AddListener(delegate
			{
				if (m_IsCloudMoolahStoreSelected)
				{
					if (!m_IsLoggedIn)
					{
						UnityEngine.Debug.LogError("CloudMoolah purchase restoration aborted. Login incomplete.");
					}
					else
					{
						m_MoolahExtensions.RestoreTransactionID(delegate(RestoreTransactionIDState restoreTransactionIDState)
						{
							UnityEngine.Debug.Log("restoreTransactionIDState = " + restoreTransactionIDState.ToString());
							bool success2 = restoreTransactionIDState != RestoreTransactionIDState.RestoreFailed && restoreTransactionIDState != RestoreTransactionIDState.NotKnown;
							OnTransactionsRestored(success2);
						});
					}
				}
				else if (m_IsSamsungAppsStoreSelected)
				{
					m_SamsungExtensions.RestoreTransactions(OnTransactionsRestored);
				}
				else if (Application.platform == RuntimePlatform.MetroPlayerX86 || Application.platform == RuntimePlatform.MetroPlayerX64 || Application.platform == RuntimePlatform.MetroPlayerARM)
				{
					m_MicrosoftExtensions.RestoreTransactions();
				}
				else
				{
					m_AppleExtensions.RestoreTransactions(OnTransactionsRestored);
				}
			});
		}
		if (GetRegisterButton() != null)
		{
			GetRegisterButton().onClick.AddListener(delegate
			{
				m_MoolahExtensions.FastRegister("CMPassword", delegate(string cmUserName)
				{
					UnityEngine.Debug.Log("RegisterSucceeded: cmUserName = " + cmUserName);
					m_CloudMoolahUserName = cmUserName;
				}, delegate(FastRegisterError error, string errorMessage)
				{
					UnityEngine.Debug.Log("RegisterFailed: error = " + error.ToString() + ", errorMessage = " + errorMessage);
				});
			});
		}
		if (GetLoginButton() != null)
		{
			if (m_IsCloudMoolahStoreSelected)
			{
				GetLoginButton().onClick.AddListener(delegate
				{
					m_MoolahExtensions.Login(m_CloudMoolahUserName, "CMPassword", delegate(LoginResultState state, string errorMsg)
					{
						if (state == LoginResultState.LoginSucceed)
						{
							m_IsLoggedIn = true;
						}
						else
						{
							m_IsLoggedIn = false;
						}
						UnityEngine.Debug.Log("LoginResult: state: " + state.ToString() + " errorMsg: " + errorMsg);
					});
				});
			}
			else if (m_IsUnityChannelSelected)
			{
				GetLoginButton().onClick.AddListener(delegate
				{
					unityChannelLoginHandler.loginSucceededAction = delegate(UserInfo userInfo)
					{
						m_IsLoggedIn = true;
						UnityEngine.Debug.LogFormat("Succeeded logging into UnityChannel. channel {0}, userId {1}, userLoginToken {2} ", userInfo.channel, userInfo.userId, userInfo.userLoginToken);
					};
					unityChannelLoginHandler.loginFailedAction = delegate(string message)
					{
						m_IsLoggedIn = false;
						UnityEngine.Debug.LogError("Failed logging into UnityChannel. " + message);
					};
					StoreService.Login(unityChannelLoginHandler);
				});
			}
		}
		if (GetValidateButton() != null)
		{
			if (m_IsCloudMoolahStoreSelected)
			{
				GetValidateButton().onClick.AddListener(delegate
				{
					m_MoolahExtensions.ValidateReceipt(m_LastTransationID, m_LastReceipt, delegate(string transactionID, ValidateReceiptState state, string message)
					{
						UnityEngine.Debug.Log("ValidtateReceipt transactionID:" + transactionID + ", state:" + state.ToString() + ", message:" + message);
					});
				});
			}
			else if (m_IsUnityChannelSelected)
			{
				GetValidateButton().onClick.AddListener(delegate
				{
					string txId = m_LastTransationID;
					m_UnityChannelExtensions.ValidateReceipt(txId, delegate(bool success, string signData, string signature)
					{
						UnityEngine.Debug.LogFormat("ValidateReceipt transactionId {0}, success {1}, signData {2}, signature {3}", txId, success, signData, signature);
					});
				});
			}
		}
	}

	public void UpdateHistoryUI()
	{
		if (m_Controller != null)
		{
			string text = "Item\n\n";
			string text2 = "Purchased\n\n";
			Product[] all = m_Controller.products.all;
			foreach (Product product in all)
			{
				text = text + "\n\n" + product.definition.id;
				text2 += "\n\n";
				text2 += product.hasReceipt.ToString();
			}
			GetText(right: false).text = text;
			GetText(right: true).text = text2;
		}
	}

	protected void UpdateInteractable()
	{
		if (m_InteractableSelectable == null)
		{
			return;
		}
		bool flag = m_Controller != null;
		if (flag != m_InteractableSelectable.interactable)
		{
			if (GetRestoreButton() != null)
			{
				GetRestoreButton().interactable = flag;
			}
			GetBuyButton().interactable = flag;
			GetDropdown().interactable = flag;
			GetRegisterButton().interactable = flag;
			GetLoginButton().interactable = flag;
		}
	}

	public void Update()
	{
		UpdateInteractable();
	}

	private bool NeedRestoreButton()
	{
		return Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.tvOS || Application.platform == RuntimePlatform.MetroPlayerX86 || Application.platform == RuntimePlatform.MetroPlayerX64 || Application.platform == RuntimePlatform.MetroPlayerARM || m_IsSamsungAppsStoreSelected || m_IsCloudMoolahStoreSelected;
	}

	private bool NeedRegisterButton()
	{
		return m_IsCloudMoolahStoreSelected;
	}

	private bool NeedLoginButton()
	{
		return m_IsCloudMoolahStoreSelected || m_IsUnityChannelSelected;
	}

	private bool NeedValidateButton()
	{
		return m_IsCloudMoolahStoreSelected || m_IsUnityChannelSelected;
	}

	private Text GetText(bool right)
	{
		string name = (!right) ? "TextL" : "TextR";
		return GameObject.Find(name).GetComponent<Text>();
	}

	private Dropdown GetDropdown()
	{
		return GameObject.Find("Dropdown").GetComponent<Dropdown>();
	}

	private Button GetBuyButton()
	{
		return GameObject.Find("Buy").GetComponent<Button>();
	}

	private Button GetRestoreButton()
	{
		return GetButton("Restore");
	}

	private Button GetRegisterButton()
	{
		return GetButton("Register");
	}

	private Button GetLoginButton()
	{
		return GetButton("Login");
	}

	private Button GetValidateButton()
	{
		return GetButton("Validate");
	}

	private Button GetButton(string buttonName)
	{
		GameObject gameObject = GameObject.Find(buttonName);
		if (gameObject != null)
		{
			return gameObject.GetComponent<Button>();
		}
		return null;
	}

	private void LogProductDefinitions()
	{
		Product[] all = m_Controller.products.all;
		Product[] array = all;
		foreach (Product product in array)
		{
			UnityEngine.Debug.Log(string.Format("id: {0}\nstore-specific id: {1}\ntype: {2}\nenabled: {3}\n", product.definition.id, product.definition.storeSpecificId, product.definition.type.ToString(), (!product.definition.enabled) ? "disabled" : "enabled"));
		}
	}
}
