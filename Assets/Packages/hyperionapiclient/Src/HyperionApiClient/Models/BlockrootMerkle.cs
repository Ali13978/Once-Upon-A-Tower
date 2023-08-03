using System.Collections.Generic;
using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class BlockrootMerkle
    {
        [JsonProperty("_active_nodes")]
        public List<string> ActiveNodes { get; set; }

        [JsonProperty("_node_count")]
        public int NodeCount { get; set; }
    }
}