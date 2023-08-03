using System.Collections.Generic;
using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Receipt 
    {
        [JsonProperty("receiver", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Receiver { get; set; }
    
        [JsonProperty("global_sequence", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double GlobalSequence { get; set; }
    
        [JsonProperty("recv_sequence", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double RecvSequence { get; set; }
    
        [JsonProperty("auth_sequence", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<AuthSequence> AuthSequence { get; set; }
    }
}