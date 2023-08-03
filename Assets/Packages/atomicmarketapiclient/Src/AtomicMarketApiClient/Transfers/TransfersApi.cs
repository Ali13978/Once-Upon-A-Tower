using System;
using System.Threading.Tasks;

namespace AtomicMarketApiClient.Transfers
{
    public class TransfersApi
    {
        private readonly string _requestUriBase;
        private readonly IHttpHandler _httpHandler;

        internal TransfersApi(string baseUrl, IHttpHandler httpHandler)
        {
            _requestUriBase = baseUrl;
            _httpHandler = httpHandler;
        }

/// <summary>
/// > This function will return a list of all transfers for the current user
/// </summary>
/// <returns>
/// A list of transfers
/// </returns>
        public async Task<TransfersDto> Transfers()
        {
            return await _httpHandler.GetJsonAsync<TransfersDto>(TransfersUri().OriginalString);
        }

/// <summary>
/// It returns a list of transfers.
/// </summary>
/// <param name="TransfersUriParameterBuilder">This is a class that contains all the parameters that can
/// be passed to the API.</param>
/// <returns>
/// A TransfersDto object.
/// </returns>
        public async Task<TransfersDto> Transfers(TransfersUriParameterBuilder transfersUriParameterBuilder)
        {
            return await _httpHandler.GetJsonAsync<TransfersDto>(TransfersUri(transfersUriParameterBuilder).OriginalString);
        }

        private Uri TransfersUri() => new Uri($"{_requestUriBase}/transfers");
        private Uri TransfersUri(TransfersUriParameterBuilder transfersUriParameterBuilder) => new Uri($"{_requestUriBase}/transfers{transfersUriParameterBuilder.Build()}");
    }
}