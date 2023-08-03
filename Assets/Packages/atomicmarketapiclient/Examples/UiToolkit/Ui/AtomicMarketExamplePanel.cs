using System;
using System.Collections.Generic;
using System.Linq;
using AtomicMarketApiClient;
using AtomicMarketApiClient.Assets;
using AtomicMarketApiClient.Auctions;
using AtomicMarketApiClient.BuyOffers;
using AtomicMarketApiClient.Exceptions;
using AtomicMarketApiClient.MarketPlaces;
using AtomicMarketApiClient.Pricing;
using AtomicMarketApiClient.Sales;
using UnityEngine;
using UnityEngine.UIElements;

public class AtomicMarketExamplePanel : MonoBehaviour
{
    /**
     * Child-Controls
     */
    public VisualElement Root;

    private Label _collectionNameLabel;
    private Label _ownerLabel;
    private Label _sellerLabel;
    private Label _tradeOfferIdLabel;
    private Label _nftNameLabel;
    private Label _headerLabel;
    private Label _idLabel;
    private Label _priceLabel;
    private Label _mintNumberLabel;
    private Label _backedTokenLabel;
    private Label _schemaNameLabel;
    private Label _templateIdLabel;
    private Label _propertiesTransferableLabel;
    private Label _propertiesBurnableLabel;
    private Label _queryLabel;
    private Label _infoLabel;

    private Button _searchButton;

    private DropdownField _selectorDropdownField;
    private static TextField _collectionNameOrAssetId;

    private VisualElement _searchDetails;
    private VisualElement _loadingMask;

    /**
     * Fields/Properties
     */

    private AssetsApi _assetsApi;
    private AuctionsApi _auctionsApi;
    private SalesApi _salesApi;
    private BuyOffersApi _buyOffersApi;
    private PricingApi _pricingApi;
    private MarketPlacesApi _marketPlacesApi;

    private List<string> _searchTypes;

    public AtomicMarketErrorPanel AtomicMarketErrorPanel;

    void Start()
    {
        Root = GetComponent<UIDocument>().rootVisualElement;

        _assetsApi = AtomicMarketApiFactory.Version1.AssetsApi;
        _auctionsApi = AtomicMarketApiFactory.Version1.AuctionsApi;
        _buyOffersApi = AtomicMarketApiFactory.Version1.BuyOffersApi;
        _pricingApi = AtomicMarketApiFactory.Version1.PricingApi;
        _salesApi = AtomicMarketApiFactory.Version1.SalesApi;
        _marketPlacesApi = AtomicMarketApiFactory.Version1.MarketPlacesApi;

        _collectionNameOrAssetId = Root.Q<TextField>("collection-name-or-id-textfield");

        _headerLabel = Root.Q<Label>("header-label");
        _nftNameLabel = Root.Q<Label>("nft-name-label");
        _idLabel = Root.Q<Label>("id-label");
        _tradeOfferIdLabel = Root.Q<Label>("trade-offer-id-label");
        _priceLabel = Root.Q<Label>("price-label");
        _ownerLabel = Root.Q<Label>("owner-label");
        _sellerLabel = Root.Q<Label>("seller-label");
        _mintNumberLabel = Root.Q<Label>("mint-number-label");
        _backedTokenLabel = Root.Q<Label>("backed-token-label");
        _schemaNameLabel = Root.Q<Label>("schema-label");
        _templateIdLabel = Root.Q<Label>("template-id-label");
        _collectionNameLabel = Root.Q<Label>("collection-name-label");
        _propertiesTransferableLabel = Root.Q<Label>("properties-transferable-label");
        _propertiesBurnableLabel = Root.Q<Label>("properties-burnable-label");
        _queryLabel = Root.Q<Label>("query-label");
        _infoLabel = Root.Q<Label>("info-label");

        _selectorDropdownField = Root.Q<DropdownField>("selector-dropdown");

        _searchButton = Root.Q<Button>("search-button");

        _searchDetails = Root.Q<VisualElement>("search-details");
        _loadingMask = Root.Q<VisualElement>("loading-mask");


        BindButtons();

        Hide(_loadingMask);
        Hide(_searchDetails);
    }

    #region Button Binding

    private void BindButtons()
    {
        _queryLabel.text = "Query various details about a specific Sale Id on Atomic Market.";
        _infoLabel.text = "Type a sale Id to search";
        _collectionNameOrAssetId.value = "Type a sale id to search";
        _searchButton.text = "Search sale Id";
        _collectionNameOrAssetId.label = "Sale id";

       _collectionNameOrAssetId.RegisterCallback<ClickEvent>(evt =>
        { Clear();
          Show(_searchDetails);
          _collectionNameOrAssetId.value = "";
        });
        _searchButton.clicked += SearchAsset;

        _searchTypes = new List<string>()
        {
            { "Sale ID" },
            { "Auction ID" },
            { "Asset ID" }
        };

        _selectorDropdownField.choices = _searchTypes;

        _selectorDropdownField.value = _selectorDropdownField.choices.First();

        _selectorDropdownField.RegisterCallback<ChangeEvent<string>>(evt =>
        {
            _selectorDropdownField.value = _selectorDropdownField.value;
            if (_selectorDropdownField.value == "Sale ID")
            {
                Hide(_searchDetails);
                Clear();
                _queryLabel.text = "Query various details about a specific Sale Id on Atomic Market.";
                _infoLabel.text = "Type a sale Id to search";
                _searchButton.text = "Search Sale Id";
                _collectionNameOrAssetId.value = "Type a sale id to search";
                _collectionNameOrAssetId.label = "Sale id";
            }
            else if (_selectorDropdownField.value == "Auction ID")
            {
                Hide(_searchDetails);
                Clear();
                _queryLabel.text = "Query various details about a specific Auction Id on Atomic Market.";
                _infoLabel.text = "Type an auction Id to search";
                _searchButton.text = "Search Auction Id";
                _collectionNameOrAssetId.value = "Type an auction id to search";
                _collectionNameOrAssetId.label = "Auction id";
            }
            else if (_selectorDropdownField.value == "Asset ID")
            {
                Hide(_searchDetails);
                Clear();
                _queryLabel.text = "Query various details about a specific Asset Id on Atomic Market.";
                _infoLabel.text = "Type an asset Id to search";
                _searchButton.text = "Search Asset id";
                _collectionNameOrAssetId.value = "Type an asset id to search";
                _collectionNameOrAssetId.label = "Asset id";
            }
        });
    }

    #endregion

    #region Rebind
    /// <summary>
    /// Rebind Method for binding AssetDto api
    /// </summary>
    /// <param name="asset"></param>
    private void Rebind(AssetDto asset)
    {
        _collectionNameLabel.text = asset.Data.Collection.Name;
        _ownerLabel.text = asset.Data.Owner;
        _nftNameLabel.text = asset.Data.Name;
        _idLabel.text = asset.Data.AssetId;
        _mintNumberLabel.text = asset.Data.MintedAtBlock;
        _backedTokenLabel.text = asset.Data.TemplateMint;
        _schemaNameLabel.text = asset.Data.Schema.SchemaName;
        _templateIdLabel.text = asset.Data.Template.TemplateId;
    }

    /// <summary>
    /// Rebind Method for binding SaleDto api
    /// </summary>
    /// <param name="sales"></param>
    private void Rebind(SaleDto sales)
    {
        _collectionNameLabel.text = sales.Data.Collection.CollectionName;
        _ownerLabel.text = sales.Data.Assets[0].Owner;
        _nftNameLabel.text = sales.Data.Assets[0].Name;
        _idLabel.text = $"#{sales.Data.Assets[0].AssetId}";
        _priceLabel.text = $"{Convert.ToDecimal(sales.Data.Price.Amount)/100000000}  {sales.Data.Price.TokenSymbol}";
        _sellerLabel.text = sales.Data.Seller;
        _tradeOfferIdLabel.text = $"#{sales.Data.OfferId}";
        _mintNumberLabel.text = $"{sales.Data.Assets[0].TemplateMint} of {sales.Data.Assets[0].Template.IssuedSupply}";
        _backedTokenLabel.text = sales.Data.Assets[0].BackedTokens.Length.ToString();
        _schemaNameLabel.text = sales.Data.Assets[0].Schema.SchemaName;
        _templateIdLabel.text = sales.Data.Assets[0].Template.TemplateId;
    }

    /// <summary>
    /// Rebind Method for binding AuctionDto api
    /// </summary>
    /// <param name="auction"></param>
    private void Rebind(AuctionDto auction)
    {
        _collectionNameLabel.text = auction.Data.Collection.CollectionName;
        _ownerLabel.text = auction.Data.Assets[0].Owner;
        _nftNameLabel.text = auction.Data.Assets[0].Name;
        _idLabel.text = $"#{auction.Data.Assets[0].AssetId}";
        _priceLabel.text = $"{Convert.ToDecimal(auction.Data.Price.Amount) / 100000000}  {auction.Data.Price.TokenSymbol}";
        _sellerLabel.text = auction.Data.Seller;
        _mintNumberLabel.text = $"{auction.Data.Assets[0].TemplateMint} of {auction.Data.Assets[0].Template.IssuedSupply}";
        _backedTokenLabel.text = auction.Data.Assets[0].BackedTokens.Length.ToString();
        _schemaNameLabel.text = auction.Data.Assets[0].Schema.SchemaName;
        _templateIdLabel.text = auction.Data.Assets[0].Template.TemplateId;
    }

    #endregion

    #region Others

    /// <summary>
    /// SearchAsset Method to evaluate input search for certain api
    /// </summary>
    private async void SearchAsset()
    {
        if (_selectorDropdownField.value != null)
        {
            try
            {
                switch (_selectorDropdownField.value)
                {
                    case "Asset ID":
                        Show(_loadingMask);
                        var assetDto = await _assetsApi.Asset(_collectionNameOrAssetId.value);
                        if (assetDto != null)
                        {
                            Rebind(assetDto);
                            Hide(_searchDetails);
                            Hide(_loadingMask);
                        }
                        else Debug.Log("asset id not found");
                        break;

                    case "Sale ID":
                        Show(_loadingMask);
                        var saleDto = await _salesApi.Sale(Convert.ToInt32(_collectionNameOrAssetId.value));
                        if (saleDto != null)
                        {
                            Rebind(saleDto);
                            Hide(_searchDetails);
                            Hide(_loadingMask);
                        }
                        else Debug.Log("sales id not found");
                        break;

                    case "Auction ID":
                        Show(_loadingMask);
                        var auctionDto = await _auctionsApi.Auction((Convert.ToInt32(_collectionNameOrAssetId.value)));
                        if (auctionDto != null)
                        {
                            Rebind(auctionDto);
                            Hide(_searchDetails);
                            Hide(_loadingMask);
                        }
                        else Debug.Log("auction id not found");
                        break;
                }
            }
            catch (ApiException ex)
            {
                AtomicMarketErrorPanel.ErrorText("Content Error", ex.Content);
                Hide(_loadingMask);
                Show(AtomicMarketErrorPanel.Root);
            }
        }
    }

    /// <summary>
    /// Extension-method to show an UI Element (set it to visible)
    /// </summary>
    /// <param name="element"></param>
    public void Show(VisualElement element)
    {
        if (element == null)
            return;

        element.style.visibility = Visibility.Visible;
        element.style.display = DisplayStyle.Flex;
    }

    /// <summary>
    /// Extension-method to hide an UI Element (set it to invisible)
    /// </summary>
    /// <param name="element"></param>
    public void Hide(VisualElement element)
    {
        if (element == null)
            return;

        element.style.visibility = Visibility.Hidden;
        element.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// Called when ctrl + v is pressed in browser (webgl)
    /// </summary>
    /// <param name="pastedText">The pasted text.</param>
    public void OnBrowserClipboardPaste(string pastedText)
    {
        if (string.IsNullOrEmpty(pastedText))
            return;

        if (_collectionNameOrAssetId != null && _collectionNameOrAssetId.focusController.focusedElement == _collectionNameOrAssetId)
            _collectionNameOrAssetId.SetValueWithoutNotify(pastedText);
    }

    /// <summary>
    /// Clear Method Clear the values after rebind
    /// </summary>
    private void Clear()
    {
        _collectionNameLabel.text = "";
        _ownerLabel.text = "";
        _nftNameLabel.text = "";
        _idLabel.text = "";
        _mintNumberLabel.text = "";
        _backedTokenLabel.text = "";
        _schemaNameLabel.text = "";
        _templateIdLabel.text = "";
        _priceLabel.text = "";
        _sellerLabel.text = "";
        _tradeOfferIdLabel.text = "";
    }
    #endregion
}
