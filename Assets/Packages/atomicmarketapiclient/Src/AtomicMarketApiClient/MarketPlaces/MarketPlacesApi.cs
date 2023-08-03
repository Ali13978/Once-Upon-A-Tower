using System;
using System.Threading.Tasks;

namespace AtomicMarketApiClient.MarketPlaces
{
    public class MarketPlacesApi
    {
        private readonly string _requestUriBase;
        private readonly IHttpHandler _httpHandler;

        internal MarketPlacesApi(string baseUrl, IHttpHandler httpHandler)
        {
            _requestUriBase = baseUrl;
            _httpHandler = httpHandler;
        }

/// <summary>
/// > This function will return a list of marketplaces that are available to the user
/// </summary>
/// <returns>
/// A list of marketplaces.
/// </returns>
        public async Task<MarketplacesDto> Marketplaces()
        {
            return await _httpHandler.GetJsonAsync<MarketplacesDto>(MarketplacesUri().OriginalString);
        }

/// <summary>
/// > This function will return a `MarketplaceDto` object from the API
/// </summary>
/// <param name="name">The name of the marketplace you want to retrieve.</param>
/// <returns>
/// A MarketplaceDto object
/// </returns>
        public async Task<MarketplaceDto> Marketplace(string name)
        {
            return await _httpHandler.GetJsonAsync<MarketplaceDto>(MarketplaceUri(name).OriginalString);
        }


        private Uri MarketplacesUri() => new Uri($"{_requestUriBase}/marketplaces");
        private Uri MarketplaceUri(string name) => new Uri($"{_requestUriBase}/marketplaces/{name}");
    }
}