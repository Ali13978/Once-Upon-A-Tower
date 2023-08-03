using HyperionApiClient.Models;
using Newtonsoft.Json;

namespace HyperionApiClient.Responses
{
    public class GetApiSnapshotResponse
    {
        [JsonProperty("block_num")]
        public int BlockNum { get; set; }

        [JsonProperty("abi")]
        public Abi Abi { get; set; }

        [JsonProperty("query_time_ms")]
        public double QueryTimeMs { get; set; }
    }
}