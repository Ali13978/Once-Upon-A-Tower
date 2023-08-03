using System.Collections.Generic;
using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class ActivatedProtocolFeatures
    {
        [JsonProperty("protocol_features")]
        public List<string> ProtocolFeatures { get; set; }
    }
}