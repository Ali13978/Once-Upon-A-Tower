using System;
using System.Threading.Tasks;

namespace AtomicAssetsApiClient.Burns
{
    public class BurnsApi
    { 
        private readonly string _requestUriBase;
        private readonly IHttpHandler _httpHander;

        internal BurnsApi(string baseUrl, IHttpHandler httpHandler)
        {
            _requestUriBase = baseUrl;
            _httpHander = httpHandler;
        }

        public async Task<BurnsDto> Burns()
        {
            return await _httpHander.GetJsonAsync<BurnsDto>(BurnsUri().OriginalString);
        }

        public async Task<BurnsDto> Burns(BurnsUriParameterBuilder burnsUriParameterBuilder)
        {
            return await _httpHander.GetJsonAsync<BurnsDto>(BurnsUri(burnsUriParameterBuilder).OriginalString);
        }

        public async Task<BurnDto> Account(string accountName)
        {
            return await _httpHander.GetJsonAsync<BurnDto>(BurnUri(accountName).OriginalString);
        }

        private Uri BurnsUri() => new Uri($"{_requestUriBase}/burns");
        private Uri BurnsUri(BurnsUriParameterBuilder burnsUriParameterBuilder) => new Uri($"{_requestUriBase}/burns{burnsUriParameterBuilder.Build()}");
        private Uri BurnUri(string accountName) => new Uri($"{_requestUriBase}/burns/{accountName}");
    }
}