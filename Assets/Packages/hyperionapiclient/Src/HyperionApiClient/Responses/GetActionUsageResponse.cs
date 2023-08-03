using Newtonsoft.Json;

namespace HyperionApiClient.Responses
{
    public class GetActionUsageResponse
    {
        [JsonProperty("action_count")]
        public int ActionCount { get; set; }

        [JsonProperty("tx_count")]
        public int TxCount { get; set; }

        [JsonProperty("period")]
        public string Period { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("query_time_ms")]
        public double QueryTimeMs { get; set; }
    }
}