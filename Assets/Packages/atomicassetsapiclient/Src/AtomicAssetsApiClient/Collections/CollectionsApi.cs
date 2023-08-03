using System;
using System.Threading.Tasks;

namespace AtomicAssetsApiClient.Collections
{
    public class CollectionsApi
    {
        private readonly string _requestUriBase;
        private readonly IHttpHandler _httpHander;

        internal CollectionsApi(string baseUrl, IHttpHandler httpHandler)
        {
            _requestUriBase = baseUrl;
            _httpHander = httpHandler;
        }

        public async Task<CollectionsDto> Collections()
        {
            return await _httpHander.GetJsonAsync<CollectionsDto>(CollectionsUri().OriginalString);
        }

        public async Task<CollectionsDto> Collections(CollectionsUriParameterBuilder collectionsUriParameterBuilder)
        {
            return await _httpHander.GetJsonAsync<CollectionsDto>(CollectionsUri(collectionsUriParameterBuilder).OriginalString);
        }

        public async Task<CollectionDto> Collection(string collectionName)
        {
            return await _httpHander.GetJsonAsync<CollectionDto>(CollectionUri(collectionName).OriginalString);
        }

        public async Task<StatsDto> CollectionStats(string collectionName)
        {
            return await _httpHander.GetJsonAsync<StatsDto>(CollectionStatsUri(collectionName).OriginalString);
        }

        public async Task<LogsDto> CollectionLogs(string collectionName)
        {
            return await _httpHander.GetJsonAsync<LogsDto>(CollectionLogsUri(collectionName).OriginalString);
        }

        public async Task<LogsDto> CollectionLogs(string collectionName, CollectionsUriParameterBuilder collectionsUriParameterBuilder)
        {
            return await _httpHander.GetJsonAsync<LogsDto>(CollectionLogsUri(collectionName, collectionsUriParameterBuilder).OriginalString);
        }

        private Uri CollectionsUri() => new Uri($"{_requestUriBase}/collections");
        private Uri CollectionsUri(CollectionsUriParameterBuilder collectionsUriParameterBuilder) => new Uri($"{_requestUriBase}/collections{collectionsUriParameterBuilder.Build()}");
        private Uri CollectionUri(string collectionName) => new Uri($"{_requestUriBase}/collections/{collectionName}");
        private Uri CollectionStatsUri(string collectionName) => new Uri($"{_requestUriBase}/collections/{collectionName}/stats");
        private Uri CollectionLogsUri(string collectionName) => new Uri($"{_requestUriBase}/collections/{collectionName}/logs");
        private Uri CollectionLogsUri(string collectionName, CollectionsUriParameterBuilder collectionsUriParameterBuilder) => new Uri($"{_requestUriBase}/collections/{collectionName}/logs{collectionsUriParameterBuilder.Build()}");
    }
}