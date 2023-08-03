using System.Collections.Generic;
using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class ExtendedAct : Act
    {
        [JsonProperty("authorization", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<object> Authorization { get; set; }
    
        [JsonProperty("data", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }
    
        [JsonProperty("hex_data", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string HexData { get; set; }
    }
}