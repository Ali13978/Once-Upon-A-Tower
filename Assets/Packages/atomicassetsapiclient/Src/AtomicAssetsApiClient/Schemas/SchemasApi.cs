using System;
using System.Threading.Tasks;

namespace AtomicAssetsApiClient.Schemas
{
    public class SchemasApi
    {
        private readonly string _requestUriBase;
        private readonly IHttpHandler _httpHandler;

        internal SchemasApi(string baseUrl, IHttpHandler httpHandler)
        {
            _requestUriBase = baseUrl;
            _httpHandler = httpHandler;
        }

        public async Task<SchemasDto> Schemas()
        {
            return await _httpHandler.GetJsonAsync<SchemasDto>(SchemasUri().OriginalString);
        }

        public async Task<SchemasDto> Schemas(SchemasUriParameterBuilder schemasUriParameterBuilder)
        {
            return await _httpHandler.GetJsonAsync<SchemasDto>(SchemasUri(schemasUriParameterBuilder).OriginalString);
        }

        public async Task<SchemaDto> Schema(string collectionName, string schemaName)
        {
            return await _httpHandler.GetJsonAsync<SchemaDto>(SchemaUri(collectionName, schemaName).OriginalString);
        }

        public async Task<StatsDto> SchemaStats(string collectionName, string schemaName)
        {
            return await _httpHandler.GetJsonAsync<StatsDto>(SchemaStatsUri(collectionName, schemaName).OriginalString);
        }

        public async Task<LogsDto> SchemaLogs(string collectionName, string schemaName)
        {
            return await _httpHandler.GetJsonAsync<LogsDto>(SchemaLogsUri(collectionName, schemaName).OriginalString);
        }

        public async Task<LogsDto> SchemaLogs(string collectionName, string schemaName, SchemasUriParameterBuilder schemasUriParameterBuilder)
        {
            return await _httpHandler.GetJsonAsync<LogsDto>(SchemaLogsUri(collectionName, schemaName, schemasUriParameterBuilder).OriginalString);
        }

        private Uri SchemasUri() => new Uri($"{_requestUriBase}/schemas");
        private Uri SchemasUri(SchemasUriParameterBuilder schemasUriParameterBuilder) => new Uri($"{_requestUriBase}/schemas{schemasUriParameterBuilder.Build()}");
        private Uri SchemaUri(string collectionName, string schemaName) => new Uri($"{_requestUriBase}/schemas/{collectionName}/{schemaName}");
        private Uri SchemaStatsUri(string collectionName, string schemaName) => new Uri($"{_requestUriBase}/schemas/{collectionName}/{schemaName}/stats");
        private Uri SchemaLogsUri(string collectionName, string schemaName) => new Uri($"{_requestUriBase}/schemas/{collectionName}/{schemaName}/logs");
        private Uri SchemaLogsUri(string collectionName, string schemaName, SchemasUriParameterBuilder schemasUriParameterBuilder) => new Uri($"{_requestUriBase}/schemas/{collectionName}/{schemaName}/logs{schemasUriParameterBuilder.Build()}");
    }
}