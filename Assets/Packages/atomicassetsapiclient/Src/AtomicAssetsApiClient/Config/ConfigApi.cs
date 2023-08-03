using System;
using System.Threading.Tasks;

namespace AtomicAssetsApiClient.Config
{
    public class ConfigApi
    {
        private readonly string _requestUriBase;
        private readonly IHttpHandler _httpHandler;

        internal ConfigApi(string baseUrl, IHttpHandler httpHandler)
        {
            _requestUriBase = baseUrl;
            _httpHandler = httpHandler;
        }

        public async Task<ConfigDto> Config()
        {
            return await _httpHandler.GetJsonAsync<ConfigDto>(ConfigUri().OriginalString);
        }

        private Uri ConfigUri() => new Uri($"{_requestUriBase}/config");
    }
}