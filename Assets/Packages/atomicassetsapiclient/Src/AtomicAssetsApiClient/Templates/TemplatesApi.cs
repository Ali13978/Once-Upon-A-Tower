using System;
using System.Threading.Tasks;

namespace AtomicAssetsApiClient.Templates
{
    public class TemplatesApi
    {
        private readonly string _requestUriBase;
        private readonly IHttpHandler _httpHandler;

        internal TemplatesApi(string baseUrl, IHttpHandler httpHandler)
        {
            _requestUriBase = baseUrl;
            _httpHandler = httpHandler;
        }

        public async Task<TemplatesDto> Templates()
        {
            return await _httpHandler.GetJsonAsync<TemplatesDto>(TemplatesUri().OriginalString);
        }

        public async Task<TemplatesDto> Templates(TemplatesUriParameterBuilder templatesUriParameterBuilder)
        {
            return await _httpHandler.GetJsonAsync<TemplatesDto>(TemplatesUri(templatesUriParameterBuilder).OriginalString);
        }

        public async Task<TemplateDto> Template(string collectionName, string templateId)
        {
            return await _httpHandler.GetJsonAsync<TemplateDto>(TemplateUri(collectionName, templateId).OriginalString);
        }

        public async Task<StatsDto> TemplateStats(string collectionName, string templateId)
        {
            return await _httpHandler.GetJsonAsync<StatsDto>(TemplateStatsUri(collectionName, templateId).OriginalString);
        }

        public async Task<LogsDto> TemplateLogs(string collectionName, string templateId)
        {
            return await _httpHandler.GetJsonAsync<LogsDto>(TemplateLogsUri(collectionName, templateId).OriginalString);
        }

        public async Task<LogsDto> TemplateLogs(string collectionName, string templateId, TemplatesUriParameterBuilder templatesUriParameterBuilder)
        {
            return await _httpHandler.GetJsonAsync<LogsDto>(TemplateLogsUri(collectionName, templateId, templatesUriParameterBuilder).OriginalString);
        }

        private Uri TemplatesUri() => new Uri($"{_requestUriBase}/templates");
        private Uri TemplatesUri(TemplatesUriParameterBuilder templatessUriParameterBuilder) => new Uri($"{_requestUriBase}/templates{templatessUriParameterBuilder.Build()}");
        private Uri TemplateUri(string collectionName, string templateId) => new Uri($"{_requestUriBase}/templates/{collectionName}/{templateId}");
        private Uri TemplateStatsUri(string collectionName, string templateId) => new Uri($"{_requestUriBase}/templates/{collectionName}/{templateId}/stats");
        private Uri TemplateLogsUri(string collectionName, string templateId) => new Uri($"{_requestUriBase}/templates/{collectionName}/{templateId}/logs");
        private Uri TemplateLogsUri(string collectionName, string templateId, TemplatesUriParameterBuilder templatesUriParameterBuilder) => new Uri($"{_requestUriBase}/templates/{collectionName}/{templateId}/logs{templatesUriParameterBuilder.Build()}");
    }
}