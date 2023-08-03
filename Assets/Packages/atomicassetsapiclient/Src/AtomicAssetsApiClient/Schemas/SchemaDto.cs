using Newtonsoft.Json;

namespace AtomicAssetsApiClient.Schemas
{
    public class SchemaDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public DataDto Data { get; set; }

        public class DataDto
        {
            [JsonProperty("contract")]
            public string Contract { get; set; }

            [JsonProperty("schema_name")]
            public string SchemaName { get; set; }

            [JsonProperty("format")]
            public FormatDto[] Format { get; set; }

            [JsonProperty("created_at_block")]
            public float CreatedAtBlock { get; set; }

            [JsonProperty("created_at_time")]
            public float CreatedAtTime { get; set; }

            [JsonProperty("collection")]
            public CollectionDto Collection { get; set; }

            public class FormatDto
            {
                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("type")]
                public string Type { get; set; }
            }

            public class CollectionDto
            {
                [JsonProperty("collection_name")]
                public string CollectionName { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("img")]
                public string Image { get; set; }

                [JsonProperty("author")]
                public string Author { get; set; }

                [JsonProperty("allow_notify")]
                public bool AllowNotify { get; set; }

                [JsonProperty("authorized_accounts")]
                public string[] AuthorizedAccounts { get; set; }

                [JsonProperty("notify_accounts")]
                public string[] NotifyAccounts { get; set; }

                [JsonProperty("market_fee")]
                public float MarketFee { get; set; }

                [JsonProperty("created_at_block")]
                public float CreatedAtBlock { get; set; }

                [JsonProperty("created_at_time")]
                public float CreatedAtTime { get; set; }
            }
        }
    }
}
