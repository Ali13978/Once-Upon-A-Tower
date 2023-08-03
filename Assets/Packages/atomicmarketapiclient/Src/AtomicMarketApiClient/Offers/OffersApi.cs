using System;
using System.Threading.Tasks;

namespace AtomicMarketApiClient.Offers
{
    public class OffersApi
    {
        private readonly string _requestUriBase;
        private readonly IHttpHandler _httpHandler;

        internal OffersApi(string baseUrl, IHttpHandler httpHandler)
        {
            _requestUriBase = baseUrl;
            _httpHandler = httpHandler;
        }

/// <summary>
/// > This function will make a GET request to the `/offers` endpoint and return the response as a
/// `OffersDto` object
/// </summary>
/// <returns>
/// A list of offers
/// </returns>
        public async Task<OffersDto> Offers()
        {
            return await _httpHandler.GetJsonAsync<OffersDto>(OffersUri().OriginalString);
        }

/// <summary>
/// It takes a `OffersUriParameterBuilder` object as a parameter, builds a `HttpRequestMessage` object,
/// sends it to the API, and returns a `OffersDto` object.
/// </summary>
/// <param name="OffersUriParameterBuilder">This is a class that contains all the parameters that can be
/// passed to the API.</param>
/// <returns>
/// A list of offers.
/// </returns>
        public async Task<OffersDto> Offers(OffersUriParameterBuilder offersUriParameterBuilder)
        {
            return await _httpHandler.GetJsonAsync<OffersDto>(OffersUri(offersUriParameterBuilder).OriginalString);
        }

/// <summary>
/// > It builds an HTTP GET request to the `OfferUri` endpoint, sends the request to the API, and
/// returns the response as an `OfferDto` object
/// </summary>
/// <param name="offerId">The offer id of the offer you want to retrieve.</param>
/// <returns>
/// A single offer
/// </returns>
        public async Task<OfferDto> Offer(string offerId)
        {
            return await _httpHandler.GetJsonAsync<OfferDto>(OfferUri(offerId).OriginalString);
        }

/// <summary>
/// This function returns a list of logs for a given offer
/// </summary>
/// <param name="offerId">The offer ID of the offer you want to get the logs for.</param>
/// <returns>
/// A list of logs for the offer.
/// </returns>
        public async Task<LogsDto> OfferLogs(string offerId)
        {
            return await _httpHandler.GetJsonAsync<LogsDto>(OfferLogsUri(offerId).OriginalString);
        }

/// <summary>
/// This function returns a list of logs for a specific offer
/// </summary>
/// <param name="offerId">The offer id of the offer you want to get the logs for.</param>
/// <param name="OffersUriParameterBuilder">This is a class that contains all the parameters that can be
/// passed to the API.</param>
/// <returns>
/// A list of logs for the offer.
/// </returns>
        public async Task<LogsDto> OfferLogs(string offerId, OffersUriParameterBuilder schemasUriParameterBuilder)
        {
            return await _httpHandler.GetJsonAsync<LogsDto>(OfferLogsUri(offerId, schemasUriParameterBuilder).OriginalString);
        }

        private Uri OffersUri() => new Uri($"{_requestUriBase}/offers");
        private Uri OffersUri(OffersUriParameterBuilder offersUriParameterBuilder) => new Uri($"{_requestUriBase}/offers{offersUriParameterBuilder.Build()}");
        private Uri OfferUri(string offerId) => new Uri($"{_requestUriBase}/offers/{offerId}");
        private Uri OfferLogsUri(string offerId) => new Uri($"{_requestUriBase}/offers/{offerId}/logs");
        private Uri OfferLogsUri(string offerId, OffersUriParameterBuilder offersUriParameterBuilder) => new Uri($"{_requestUriBase}/offers/{offerId}/logs{offersUriParameterBuilder.Build()}");
    }
}