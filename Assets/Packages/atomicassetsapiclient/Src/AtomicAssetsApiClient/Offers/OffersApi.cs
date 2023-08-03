using System;
using System.Threading.Tasks;

namespace AtomicAssetsApiClient.Offers
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

        public async Task<OffersDto> Offers()
        {
            return await _httpHandler.GetJsonAsync<OffersDto>(OffersUri().OriginalString);
        }

        public async Task<OffersDto> Offers(OffersUriParameterBuilder offersUriParameterBuilder)
        {
            return await _httpHandler.GetJsonAsync<OffersDto>(OffersUri(offersUriParameterBuilder).OriginalString);
        }

        public async Task<OfferDto> Offer(string offerId)
        {
            return await _httpHandler.GetJsonAsync<OfferDto>(OfferUri(offerId).OriginalString);
        }

        public async Task<LogsDto> OfferLogs(string offerId)
        {
            return await _httpHandler.GetJsonAsync<LogsDto>(OfferLogsUri(offerId).OriginalString);
        }

        public async Task<LogsDto> OfferLogs(string offerId, OffersUriParameterBuilder  schemasUriParameterBuilder)
        {
            return await _httpHandler.GetJsonAsync<LogsDto>(OfferLogsUri(offerId, schemasUriParameterBuilder).OriginalString);
        }

        private Uri OffersUri() => new Uri($"{_requestUriBase}/offers");
        private Uri OffersUri(OffersUriParameterBuilder  offersUriParameterBuilder) => new Uri($"{_requestUriBase}/offers{offersUriParameterBuilder.Build()}");
        private Uri OfferUri(string offerId) => new Uri($"{_requestUriBase}/offers/{offerId}");
        private Uri OfferLogsUri(string offerId) => new Uri($"{_requestUriBase}/offers/{offerId}/logs");
        private Uri OfferLogsUri(string offerId, OffersUriParameterBuilder  offersUriParameterBuilder) => new Uri($"{_requestUriBase}/offers/{offerId}/logs{offersUriParameterBuilder.Build()}");
    }
}