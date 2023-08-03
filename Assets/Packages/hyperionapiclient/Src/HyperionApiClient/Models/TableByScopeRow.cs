using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class TableByScopeRow
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("table")]
        public string Table { get; set; }

        [JsonProperty("payer")]
        public string Payer { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}