using System;
using System.Threading.Tasks;

namespace AtomicMarketApiClient.Config
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

/// <summary>
/// > This function will return a `ConfigDto` object that contains the configuration information for the
/// current user
/// </summary>
/// <returns>
/// A ConfigDto object
/// </returns>
        public async Task<ConfigDto> Config()
        {
            return await _httpHandler.GetJsonAsync<ConfigDto>(ConfigUri().OriginalString);
        }

        private Uri ConfigUri() => new Uri($"{_requestUriBase}/config");
    }
}