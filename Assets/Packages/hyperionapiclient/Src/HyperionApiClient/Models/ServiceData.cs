using System;
using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class ServiceData
    {
        [JsonProperty("head_block_num")]
        public int HeadBlockNum { get; set; }

        [JsonProperty("head_block_time")]
        public DateTime HeadBlockTime { get; set; }

        [JsonProperty("time_offset")]
        public int TimeOffset { get; set; }

        [JsonProperty("last_irreversible_block")]
        public int LastIrreversibleBlock { get; set; }

        [JsonProperty("chain_id")]
        public string ChainId { get; set; }

        [JsonProperty("last_indexed_block")]
        public int? LastIndexedBlock { get; set; }

        [JsonProperty("total_indexed_blocks")]
        public int? TotalIndexedBlocks { get; set; }

        [JsonProperty("active_shards")]
        public string ActiveShards { get; set; }
    }
}