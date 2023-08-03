using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class ProducerRow
    {
        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("total_votes")]
        public string TotalVotes { get; set; }

        [JsonProperty("producer_key")]
        public string ProducerKey { get; set; }

        [JsonProperty("is_active")]
        public int IsActive { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("unpaid_blocks")]
        public int UnpaidBlocks { get; set; }

        [JsonProperty("last_claim_time")]
        public string LastClaimTime { get; set; }

        [JsonProperty("location")]
        public int Location { get; set; }
    }
}