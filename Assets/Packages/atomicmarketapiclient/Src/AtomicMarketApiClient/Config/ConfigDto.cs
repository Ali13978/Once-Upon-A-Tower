using Newtonsoft.Json;

namespace AtomicMarketApiClient.Config
{
    public class ConfigDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public DataDto Data { get; set; }

        public class DataDto
        {
            [JsonProperty("atomicassets_contract")]
            public string AtomicAssetsContract { get; set; }

            [JsonProperty("atomicmarket_contract")]
            public string AtomicMarketContract { get; set; }

            [JsonProperty("delphioracle_contract")]
            public string DelphioracleContract { get; set; }

            [JsonProperty("version")]
            public string Version { get; set; }

            [JsonProperty("maker_market_fee")]
            public string MakerMarketFee { get; set; }

            [JsonProperty("taker_market_fee")]
            public string TakerMarketFee { get; set; }

            [JsonProperty("minimum_auction_duration")]
            public string MinimumAuctionDuration { get; set; }

            [JsonProperty("maximum_auction_duration")]
            public string MaximumAuctionDuration { get; set; }

            [JsonProperty("minimum_bid_increase")]
            public string MinimumBidIncrease { get; set; }

            [JsonProperty("auction_reset_duration")]
            public string AuctionResetDuration { get; set; }

            [JsonProperty("supported_tokens")]
            public SupportedTokensDto[] SupportedTokens { get; set; }

            [JsonProperty("supported_pairs")]
            public SupportedPairsDto[] SupportedPairs { get; set; }

            [JsonProperty("query_time")]
            public long QueryTime { get; set; }

            public class SupportedTokensDto
            {
                [JsonProperty("token_contract")]
                public string TokenContract { get; set; }

                [JsonProperty("token_symbol")]
                public string TokenSymbol { get; set; }

                [JsonProperty("token_precision")]
                public string TokenPrecision { get; set; }
            }

            public class SupportedPairsDto
            {
                [JsonProperty("listing_symbol")]
                public string ListingSymbol { get; set; }

                [JsonProperty("settlement_symbol")]
                public string SettlementSymbol { get; set; }

                [JsonProperty("delphi_pair_name")]
                public string DelphiPairName { get; set; }

                [JsonProperty("invert_delphi_pair")]
                public bool InvertDelphiPair { get; set; }

                [JsonProperty("data")]
                public PairsDataDto Data { get; set; }

                public class PairsDataDto
                {
                    [JsonProperty("contract")]
                    public string Contract { get; set; }

                    [JsonProperty("delphi_pair_name")]
                    public string DelphiPairName { get; set; }

                    [JsonProperty("base_symbol")]
                    public string BaseSymbol { get; set; }

                    [JsonProperty("base_precision")]
                    public string BasePrecision { get; set; }

                    [JsonProperty("quote_symbol")]
                    public string QuoteSymbol { get; set; }

                    [JsonProperty("quote_precision")]
                    public string QuotePrecision { get; set; }

                    [JsonProperty("median")]
                    public string Median { get; set; }

                    [JsonProperty("median_precision")]
                    public string MedianPrecision { get; set; }

                    [JsonProperty("updated_at_time")]
                    public string UpdatedAtTime { get; set; }

                    [JsonProperty("updated_at_block")]
                    public string UpdatedAtBlock { get; set; }
                }
            }
        }
    }
}
