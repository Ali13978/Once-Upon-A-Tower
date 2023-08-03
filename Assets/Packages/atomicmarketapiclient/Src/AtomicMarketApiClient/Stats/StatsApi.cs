using System;
using System.Threading.Tasks;

namespace AtomicMarketApiClient.Stats
{
    public class StatsApi
    {
        private readonly string _requestUriBase;
        private readonly IHttpHandler _httpHandler;

        internal StatsApi(string baseUrl, IHttpHandler httpHandler)
        {
            _requestUriBase = baseUrl;
            _httpHandler = httpHandler;
        }

/// <summary>
/// > This function will return a list of all collections in the database
/// </summary>
/// <returns>
/// A list of collections
/// </returns>
        public async Task<CollectionsDto> Collections()
        {
            return await _httpHandler.GetJsonAsync<CollectionsDto>(CollectionsUri().OriginalString);
        }

/// <summary>
/// > This function will return a `CollectionsDto` object that contains a list of `CollectionDto`
/// objects
/// </summary>
/// <param name="StatsUriParameterBuilder">This is a class that contains all the parameters that can be
/// passed to the API.</param>
/// <returns>
/// A CollectionsDto object.
/// </returns>
        public async Task<CollectionsDto> Collections(StatsUriParameterBuilder uriParameterBuilder)
        {
            return await _httpHandler.GetJsonAsync<CollectionsDto>(CollectionsUri(uriParameterBuilder).OriginalString);
        }

/// <summary>
/// > This function will return a `CollectionDto` object that contains the collection's name, the number
/// of documents in the collection, the number of documents that have been deleted, the number of
/// documents that are new (i.e. have not been persisted to disk), the number of documents that have
/// been updated, the number of documents that have been replaced, the number of documents that have
/// been removed, the number of documents that have been inserted, the number of documents that have
/// been ignored, the number of documents that have been updated, the number of documents that have been
/// replaced, the number of documents that have been removed, the number of documents that have been
/// inserted, the number of documents that have been ignored, the number of documents that have been
/// updated, the number of documents that have been replaced, the number of documents that have been
/// removed, the number of documents that have been inserted, the number of documents that have been
/// ignored, the number of documents that have been
/// </summary>
/// <param name="collectionName">The name of the collection you want to get stats for.</param>
/// <param name="StatsUriParameterBuilder">This is a class that allows you to build the query string
/// parameters for the API call.</param>
/// <returns>
/// A collection of documents.
/// </returns>
        public async Task<CollectionDto> Collection(string collectionName, StatsUriParameterBuilder uriParameterBuilder)
        {
            return await _httpHandler.GetJsonAsync<CollectionDto>(CollectionUri(collectionName, uriParameterBuilder).OriginalString);
        }

/// <summary>
/// > This function will return a list of accounts for the current user
/// </summary>
/// <returns>
/// A list of accounts
/// </returns>
        public async Task<AccountsDto> Accounts()
        {
            return await _httpHandler.GetJsonAsync<AccountsDto>(AccountsUri().OriginalString);
        }

/// <summary>
/// > This function will return a list of accounts that match the criteria specified in the
/// `uriParameterBuilder` parameter
/// </summary>
/// <param name="StatsUriParameterBuilder">This is a class that contains all the parameters that can be
/// passed to the API.</param>
/// <returns>
/// A list of accounts.
/// </returns>
        public async Task<AccountsDto> Accounts(StatsUriParameterBuilder uriParameterBuilder)
        {
            return await _httpHandler.GetJsonAsync<AccountsDto>(AccountsUri(uriParameterBuilder).OriginalString);
        }

/// <summary>
/// > This function will return an AccountDto object that contains the account information for the
/// account name passed in
/// </summary>
/// <param name="accountName">The name of the account you want to get information about.</param>
/// <param name="StatsUriParameterBuilder">This is a class that allows you to build the query string
/// parameters for the API call.</param>
/// <returns>
/// An AccountDto object.
/// </returns>
        public async Task<AccountDto> Account(string accountName, StatsUriParameterBuilder uriParameterBuilder)
        {
            return await _httpHandler.GetJsonAsync<AccountDto>(AccountUri(accountName, uriParameterBuilder).OriginalString);
        }
        
/// <summary>
/// > This function will return the schema for the specified collection
/// </summary>
/// <param name="collectionName">The name of the collection you want to get the schema for.</param>
/// <returns>
/// A SchemaDto object
/// </returns>
        public async Task<SchemaDto> Schema(string collectionName)
        {
            return await _httpHandler.GetJsonAsync<SchemaDto>(SchemasUri(collectionName).OriginalString);
        }

/// <summary>
/// > This function returns a schema for a given collection
/// </summary>
/// <param name="collectionName">The name of the collection you want to get the schema for.</param>
/// <param name="StatsUriParameterBuilder">This is a class that contains all the parameters that can be
/// passed to the API.</param>
/// <returns>
/// A SchemaDto object.
/// </returns>
        public async Task<SchemaDto> Schema(string collectionName, StatsUriParameterBuilder uriParameterBuilder)
        {
            return await _httpHandler.GetJsonAsync<SchemaDto>(SchemasUri(collectionName, uriParameterBuilder).OriginalString);
        }

/// <summary>
/// > This function will return a `GraphDto` object that contains the graph data
/// </summary>
/// <returns>
/// A GraphDto object
/// </returns>
        public async Task<GraphDto> Graph()
        {
            return await _httpHandler.GetJsonAsync<GraphDto>(GraphUri().OriginalString);
        }

/// <summary>
/// > The function sends a GET request to the API and returns the response as a SalesDto object
/// </summary>
/// <returns>
/// A SalesDto object
/// </returns>
        public async Task<SalesDto> Sales()
        {
            return await _httpHandler.GetJsonAsync<SalesDto>(SalesUri().OriginalString);
        }

        private Uri CollectionsUri() => new Uri($"{_requestUriBase}/stats/collections");
        private Uri CollectionsUri(IUriParameterBuilder uriParameterBuilder) => new Uri($"{_requestUriBase}/stats/collections{uriParameterBuilder.Build()}");
        private Uri CollectionUri(string collectionName, IUriParameterBuilder uriParameterBuilder) => new Uri($"{_requestUriBase}/stats/collections/{collectionName}{uriParameterBuilder.Build()}");
        private Uri AccountsUri() => new Uri($"{_requestUriBase}/stats/accounts");
        private Uri AccountsUri(IUriParameterBuilder uriParameterBuilder) => new Uri($"{_requestUriBase}/stats/accounts{uriParameterBuilder.Build()}");
        private Uri AccountUri(string accountName, IUriParameterBuilder uriParameterBuilder) => new Uri($"{_requestUriBase}/stats/accounts/{accountName}{uriParameterBuilder.Build()}");
        private Uri SchemasUri(string collectionName) => new Uri($"{_requestUriBase}/stats/schemas/{collectionName}");
        private Uri SchemasUri(string collectionName, IUriParameterBuilder uriParameterBuilder) => new Uri($"{_requestUriBase}/stats/schemas/{collectionName}{uriParameterBuilder.Build()}");
        private Uri GraphUri() => new Uri($"{_requestUriBase}/stats/graph");
        private Uri SalesUri() => new Uri($"{_requestUriBase}/stats/sales");

    }
}