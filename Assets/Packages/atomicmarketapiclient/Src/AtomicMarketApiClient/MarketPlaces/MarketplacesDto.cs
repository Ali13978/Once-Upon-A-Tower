using Newtonsoft.Json;

namespace AtomicMarketApiClient.MarketPlaces
{
    public class MarketplacesDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public DataDto[] Data { get; set; }

        [JsonProperty("query_time")]
        public long QueryTime { get; set; }

        public class DataDto
        {
            [JsonProperty("marketplace_name")]
            public string MarketplaceName { get; set; }

            [JsonProperty("creator")]
            public string Creator { get; set; }

            [JsonProperty("created_at_block")]
            public string CreatedAtBlock { get; set; }

            [JsonProperty("created_at_time")]
            public string CreatedAtTime { get; set; }
        }
    }
}
