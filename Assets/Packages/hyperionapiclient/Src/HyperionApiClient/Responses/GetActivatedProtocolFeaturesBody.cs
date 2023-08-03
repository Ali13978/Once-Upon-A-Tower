using Newtonsoft.Json;

namespace HyperionApiClient.Responses
{
    public class GetActivatedProtocolFeaturesBody
    {
        [JsonProperty("lower_bound")]
        public int LowerBound { get; set; }

        [JsonProperty("upper_bound")]
        public int UpperBound { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("search_by_block_num")]
        public bool SearchByBlockNum { get; set; }

        [JsonProperty("reverse")]
        public bool Reverse { get; set; }
    }
}