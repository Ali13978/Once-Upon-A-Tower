using Newtonsoft.Json;

namespace AtomicAssetsApiClient.Collections
{
    public class CollectionDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public DataDto Data { get; set; }

        public class DataDto
        {
            [JsonProperty("contract")]
            public string Contract { get; set; }

            [JsonProperty("collection_name")]
            public string CollectionName { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("author")]
            public string Author { get; set; }

            [JsonProperty("allow_notify")]
            public bool AllowNotify { get; set; }

            [JsonProperty("authorized_accounts")]
            public string[] AuthorizedAccounts{ get; set; }

            [JsonProperty("notify_accounts")]
            public string[] NotifyAccounts{ get; set; }

            [JsonProperty("market_fee")]
            public float MarketFee{ get; set; }

            [JsonProperty("data")]
            public object Data { get; set; }

            [JsonProperty("created_at_block")]
            public float CreatedAtBlock { get; set; }

            [JsonProperty("created_at_time")]
            public float CreatedAtTime { get; set; }
        }
    }
}