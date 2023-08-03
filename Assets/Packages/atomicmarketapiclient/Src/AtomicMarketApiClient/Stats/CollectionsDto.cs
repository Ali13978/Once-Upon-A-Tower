using Newtonsoft.Json;

namespace AtomicMarketApiClient.Stats
{
    public class CollectionsDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public DataDto Data { get; set; }

        public class DataDto
        {
            [JsonProperty("symbol")]
            public SymbolDto Symbol { get; set; }

            [JsonProperty("results")]
            public ResultDto[] Results { get; set; }

            public class SymbolDto
            {
                [JsonProperty("token_symbol")]
                public string TokenSymbol { get; set; }

                [JsonProperty("token_contract")]
                public string TokenContract { get; set; }

                [JsonProperty("token_precision")]
                public string TokenPrecision { get; set; }
            }

            public class ResultDto
            {
                [JsonProperty("contract")]
                public string Contract { get; set; }

                [JsonProperty("collection_name")]
                public string CollectionName { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("img")]
                public string Img { get; set; }

                [JsonProperty("author")]
                public string Author { get; set; }

                [JsonProperty("allow_notify")]
                public bool? AllowNotify { get; set; }

                [JsonProperty("authorized_accounts")]
                public string[] AuthorisedAccounts { get; set; }

                [JsonProperty("notify_accounts")]
                public string[] NotifyAccounts { get; set; }

                [JsonProperty("market_fee")]
                public double MarketFee { get; set; }

                [JsonProperty("data")]
                public object Data { get; set; }

                [JsonProperty("created_at_time")]
                public string CreatedAtTime { get; set; }

                [JsonProperty("created_at_block")]
                public string CreatedAtBlock { get; set; }

                [JsonProperty("volume")]
                public string Volume { get; set; }

                [JsonProperty("listings")]
                public string Listings { get; set; }

                [JsonProperty("sales")]
                public string Sales { get; set; }
            }
        }
    }
}