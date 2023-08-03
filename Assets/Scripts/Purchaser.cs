using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : IStoreListener
{
	private static Purchaser instance;

	private IStoreController storeController;

	private IExtensionProvider storeExtensionProvider;

	private Action onSuccess;

	private Action onFailure;

	public static Purchaser Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new Purchaser();
			}
			return instance;
		}
	}

	public bool UseIAPs => Application.platform != RuntimePlatform.OSXPlayer && Application.platform != RuntimePlatform.WindowsPlayer;

	private bool IsInitialized => storeController != null && storeExtensionProvider != null;

	public void InitializePurchasing()
	{
		if (!IsInitialized && UseIAPs)
		{
			StandardPurchasingModule standardPurchasingModule = StandardPurchasingModule.Instance();
			if (Application.isEditor)
			{
				standardPurchasingModule.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
			}
			ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(standardPurchasingModule);
			SaveMePack[] list = SaveMePacks.List;
			foreach (SaveMePack saveMePack in list)
			{
				configurationBuilder.AddProduct(saveMePack.ProductId, saveMePack.ProductType);
			}
			Character[] list2 = Characters.List;
			foreach (Character character in list2)
			{
				configurationBuilder.AddProduct(character.ProductId, character.ProductType);
			}
			UnityPurchasing.Initialize(this, configurationBuilder);
		}
	}

	public string ProductPrice(string productId)
	{
		if (Application.isEditor)
		{
			return "$0.99";
		}
		if (CanBuyProduct(productId))
		{
			Product product = storeController.products.WithID(productId);
			return product.metadata.localizedPriceString;
		}
		return string.Empty;
	}

	public bool CanBuyProduct(string productId)
	{
		if (!UseIAPs)
		{
			return false;
		}
		if (!IsInitialized)
		{
			UnityEngine.Debug.Log("CanBuyProduct FAIL. Not initialized.");
			return false;
		}
		return storeController.products.WithID(productId)?.availableToPurchase ?? false;
	}

	public void BuyProduct(string productId, Action onSuccess = null, Action onFailure = null)
	{
		this.onSuccess = onSuccess;
		this.onFailure = onFailure;
		BuyProduct(productId);
	}

	private void BuyProduct(string productId)
	{
		if (!UseIAPs)
		{
			Fail();
			return;
		}
		Gui.Views.StoreProcessing.ShowAnimated();
		if (!IsInitialized)
		{
			UnityEngine.Debug.Log("BuyProduct FAIL. Not initialized.");
			Gui.Views.StoreProcessing.ShowError();
			Fail();
		}
		else
		{
			try
			{
				Product product = storeController.products.WithID(productId);
				if (product != null && product.availableToPurchase)
				{
					UnityEngine.Debug.Log($"Purchasing product asychronously: '{product.definition.id}'");
					storeController.InitiatePurchase(product);
				}
				else
				{
					Gui.Views.StoreProcessing.ShowError();
					UnityEngine.Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
					Fail();
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("BuyProductID: FAIL. Exception during purchase. " + arg);
				Gui.Views.StoreProcessing.ShowError();
				Fail();
			}
		}
	}

	public void RestorePurchases()
	{
		if (UseIAPs)
		{
			Gui.Views.StoreProcessing.ShowAnimated();
			if (!IsInitialized)
			{
				UnityEngine.Debug.LogError("RestorePurchases FAIL. Not initialized.");
				Gui.Views.StoreProcessing.ShowError();
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
			{
				UnityEngine.Debug.Log("RestorePurchases started ...");
				IAppleExtensions extension = storeExtensionProvider.GetExtension<IAppleExtensions>();
				extension.RestoreTransactions(delegate(bool result)
				{
					Gui.Views.StoreProcessing.HideAnimated();
					UnityEngine.Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
				});
			}
			else
			{
				UnityEngine.Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
				Gui.Views.StoreProcessing.ShowError();
			}
		}
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		UnityEngine.Debug.Log("OnInitialized: PASS");
		storeController = controller;
		storeExtensionProvider = extensions;
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		UnityEngine.Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
		Fail();
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		if (SingletonMonoBehaviour<Gui>.Instance.Ready)
		{
			Gui.Views.StoreProcessing.HideAnimated();
		}
		string id = args.purchasedProduct.definition.id;
		Character[] list = Characters.List;
		foreach (Character character in list)
		{
			if (character.ProductId == id)
			{
				SaveGame.Instance.SetCharacterOwned(character.Name, value: true);
				SaveGame.Instance.Save();
				break;
			}
		}
		UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
		Succeed();
		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		if (failureReason == PurchaseFailureReason.UserCancelled)
		{
			Gui.Views.StoreProcessing.HideAnimated();
		}
		else
		{
			Gui.Views.StoreProcessing.ShowError();
		}
		UnityEngine.Debug.Log($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
		Fail();
	}

	private void Succeed()
	{
		if (onSuccess != null)
		{
			onSuccess();
		}
		onSuccess = null;
		onFailure = null;
	}

	private void Fail()
	{
		if (onFailure != null)
		{
			onFailure();
		}
		onSuccess = null;
		onFailure = null;
	}
}
