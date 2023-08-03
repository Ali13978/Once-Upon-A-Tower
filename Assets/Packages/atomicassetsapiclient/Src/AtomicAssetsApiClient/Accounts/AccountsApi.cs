using System;
using System.Threading.Tasks;

namespace AtomicAssetsApiClient.Accounts
{
    public class AccountsApi
    {
        private readonly string _requestUriBase;
        private readonly IHttpHandler _httpHandler;

        internal AccountsApi(string baseUrl, IHttpHandler httpHandler)
        {
            _requestUriBase = baseUrl;
            _httpHandler = httpHandler;
        }

        public async Task<AccountsDto> Accounts()
        {
            return await _httpHandler.GetJsonAsync<AccountsDto>(AccountsUri().OriginalString);
        }

        public async Task<AccountsDto> Accounts(AccountsUriParameterBuilder accountsUriParameterBuilder)
        {
            return await _httpHandler.GetJsonAsync<AccountsDto>(AccountsUri(accountsUriParameterBuilder).OriginalString);
        }

        public async Task<AccountDto> Account(string accountName)
        {
            return await _httpHandler.GetJsonAsync<AccountDto>(AccountUri(accountName).OriginalString);
        }

        public async Task<AccountCollectionDto> Collection(string accountName, string collectionName)
        {
            return await _httpHandler.GetJsonAsync<AccountCollectionDto>(AccountUri(accountName, collectionName).OriginalString);
        }

        private Uri AccountsUri() => new Uri($"{_requestUriBase}/accounts");
        private Uri AccountsUri(AccountsUriParameterBuilder accountsUriParameterBuilder) => new Uri($"{_requestUriBase}/accounts{accountsUriParameterBuilder.Build()}");
        private Uri AccountUri(string accountName) => new Uri($"{_requestUriBase}/accounts/{accountName}");
        private Uri AccountUri(string accountName, string collectionName) => new Uri($"{_requestUriBase}/accounts/{accountName}/{collectionName}");
    }
}