using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Features
    {
        [JsonProperty("streaming")]
        public Streaming Streaming { get; set; }

        [JsonProperty("tables")]
        public Tables Tables { get; set; }

        [JsonProperty("index_deltas")]
        public bool IndexDeltas { get; set; }

        [JsonProperty("index_transfer_memo")]
        public bool IndexTransferMemo { get; set; }

        [JsonProperty("index_all_deltas")]
        public bool IndexAllDeltas { get; set; }

        [JsonProperty("deferred_trx")]
        public bool DeferredTrx { get; set; }

        [JsonProperty("failed_trx")]
        public bool FailedTrx { get; set; }

        [JsonProperty("resource_limits")]
        public bool ResourceLimits { get; set; }

        [JsonProperty("resource_usage")]
        public bool ResourceUsage { get; set; }
    }
}