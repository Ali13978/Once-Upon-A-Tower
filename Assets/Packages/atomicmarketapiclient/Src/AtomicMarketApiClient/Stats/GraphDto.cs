using Newtonsoft.Json;

namespace AtomicMarketApiClient.Stats
{
    public class GraphDto
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
                [JsonProperty("sales")]
                public string Sales { get; set; }

                [JsonProperty("time")]
                public string Time { get; set; }

                [JsonProperty("volume")]
                public string Volume { get; set; }
            }
        }
    }
}