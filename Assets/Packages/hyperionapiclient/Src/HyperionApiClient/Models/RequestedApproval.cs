using System;
using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class RequestedApproval
    {
        [JsonProperty("actor")]
        public string Actor { get; set; }

        [JsonProperty("permission")]
        public string Permission { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
}