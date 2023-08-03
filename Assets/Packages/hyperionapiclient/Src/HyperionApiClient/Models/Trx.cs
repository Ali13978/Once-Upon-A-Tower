using System.Collections.Generic;
using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Trx
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("signatures")]
        public List<string> Signatures { get; set; }

        [JsonProperty("compression")]
        public string Compression { get; set; }

        [JsonProperty("packed_context_free_data")]
        public string PackedContextFreeData { get; set; }

        [JsonProperty("context_free_data")]
        public List<object> ContextFreeData { get; set; }

        [JsonProperty("packed_trx")]
        public string PackedTrx { get; set; }

        [JsonProperty("transaction")]
        public Transaction2 Transaction { get; set; }
    }
}