<div align="center">

[![builds](https://github.com/liquiidio/AtomicAssetsApiClient-Private/actions/workflows/dotnet-build.yml/badge.svg)](https://github.com/liquiidio/AtomicAssetsApiClient-Private/actions/workflows/dotnet-build.yml)
[![Deploy](https://github.com/liquiidio/AtomicAssetsApiClient-Private/actions/workflows/deploy.yml/badge.svg)](https://github.com/liquiidio/AtomicAssetsApiClient-Private/actions/workflows/deploy.yml)
    
       
</div>

# AtomicAssetsApiClient

.NET and Unity3D-compatible (Desktop, Mobile, WebGL) ApiClient for AtomicAssets

 ## Usage

 The entry point to the APIs is in the AtomicAssetsApiFactory. You can initialise any supported API from there.
 You can then call any endpoint from the initialised API.
 Each endpoint has its own set of parameters that you may build up and pass in to the relevant function.

 ## Examples
 ### Getting assets available for trading on the exchange
 ```csharp

async Task GettingAllTheAssets()
{
    // Initialize the v1 assets API
    var assetsApi = AtomicAssetsApiFactory.Version1.AssetsApi;

    //Getting all the assets that are available for trading on the exchange.
    var assets = await assetsApi.Assets();

    // Print their IDs on the console.
    assets.Data.ToList().ForEach(a => Console.WriteLine(a.AssetId));
}

 ```
 
 ### Getting a filtered assets list that is available for trading
 ```csharp

async Task GettingFilteredAssetsList()
{
    // Initialize the v1 assets API
    var assetsApi = AtomicAssetsApiFactory.Version1.AssetsApi;

    // Build up the AssetsParameters with the AssetsUriParameterBuilder
    // This can be used to fine tune the kind of results we want
    // For example, here were limiting the results to just five assets
    // More information can be found on the documentation
    var builder = new AssetsUriParameterBuilder().WithLimit(5);

    //Getting all the assets that are available for trading on the exchange.
    var assets = await assetsApi.Assets(builder);

    // Print their IDs on the console.
    assets.Data.ToList().ForEach(a => Console.WriteLine(a.AssetId));
}

 ```
 
 ### Getting an Offer
 ```csharp

async Task GetOffer(string offerId)
{

    // Initialize the v1 offers API
    var api = AtomicAssetsApiFactory.Version1.OffersApi;

    // Call the offers endpoint passing the offerId as an input
    var sales = await api.Offer(offerId);

    // Access different informations about the offer using the Data property in the result
    Console.WriteLine(sales.Data.SenderName);
}

 ```
 ### Unity Examples
 
 Our unity packages come with examples to help you get started as quickly as possible.
 
 An example showing the result for searching an Asset with ID "#1099849109724"
 
<img width="853" alt="image" src="https://user-images.githubusercontent.com/31707324/213101482-0371d6cb-4981-4ea5-af0d-688092087b67.png">

An example showing the result for searching a Collection with Name "mrpotatogame"

<img width="847" alt="image" src="https://user-images.githubusercontent.com/31707324/213101918-98ef30b5-d1ca-4b31-b4c7-2895a3681e89.png">


