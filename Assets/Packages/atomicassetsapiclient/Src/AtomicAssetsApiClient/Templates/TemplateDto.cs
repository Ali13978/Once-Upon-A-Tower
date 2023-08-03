using Newtonsoft.Json;

namespace AtomicAssetsApiClient.Templates
{
    public class TemplateDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public DataDto Data { get; set; }

        public class DataDto
        {
            [JsonProperty("contract")]
            public string Contract { get; set; }

            [JsonProperty("template_id")]
            public string TemplateId { get; set; }

            [JsonProperty("max_supply")]
            public string MaxSupply { get; set; }

            [JsonProperty("issued_supply")]
            public string IssuedSupply { get; set; }

            [JsonProperty("is_transferable")]
            public bool IsTransferable { get; set; }

            [JsonProperty("is_burnable")]
            public bool IsBurnable { get; set; }

            [JsonProperty("immutable_data")]
            public object ImmutableData { get; set; }

            [JsonProperty("created_at_block")]
            public float CreatedAtBlock { get; set; }

            [JsonProperty("created_at_time")]
            public float CreatedAtTime { get; set; }

            [JsonProperty("collection")]
            public CollectionDto Collection { get;set; }

            [JsonProperty("scheme")]
            public SchemaDto Schema { get;set; }

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

            public class SchemaDto
            {
                [JsonProperty("schema_name")]
                public string SchemaName { get; set; }

                [JsonProperty("format")]
                public FormatDto[] Format { get; set; }

                [JsonProperty("created_at_block")]
                public float CreatedAtBlock { get; set; }

                [JsonProperty("created_at_time")]
                public float CreatedAtTime { get; set; }

                public class FormatDto
                {
                    [JsonProperty("name")]
                    public string Name { get; set; }

                    [JsonProperty("type")]
                    public string Type { get; set; }
                }
            }
        }
    }
}
