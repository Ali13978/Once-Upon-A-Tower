using Newtonsoft.Json;

namespace AtomicMarketApiClient.Pricing
{
    public class TemplatesDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public DataDto[] Data { get; set; }

        [JsonProperty("query_time")]
        public long QueryTime { get; set; }

        public class DataDto
        {
            [JsonProperty("market_contract")]
            public string SaleId { get; set; }

            [JsonProperty("assets_contract")]
            public string AssetsContract { get; set; }

            [JsonProperty("collection_name")]
            public string CollectionName { get; set; }

            [JsonProperty("template_id")]
            public string TemplateId { get; set; }

            [JsonProperty("token_symbol")]
            public string TokenSymbol { get; set; }

            [JsonProperty("token_contract")]
            public string TokenContract { get; set; }

            [JsonProperty("token_precision")]
            public string TokenPrecision { get; set; }

            [JsonProperty("median")]
            public string Median { get; set; }

            [JsonProperty("average")]
            public string Average { get; set; }

            [JsonProperty("min")]
            public string Min { get; set; }

            [JsonProperty("max")]
            public string Max { get; set; }

            [JsonProperty("sales")]
            public string Sales { get; set; }

            [JsonProperty("suggested_median")]
            public string SuggestedMedian { get; set; }

            [JsonProperty("suggested_average")]
            public string SuggestedAverage { get; set; }
        }
    }
}