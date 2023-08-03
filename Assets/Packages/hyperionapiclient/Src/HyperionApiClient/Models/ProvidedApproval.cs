using System;
using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class ProvidedApproval
    {
        [JsonProperty("actor")]
        public string Actor { get; set; }

        [JsonProperty("permission")]
        public string Permission { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }
    }
}