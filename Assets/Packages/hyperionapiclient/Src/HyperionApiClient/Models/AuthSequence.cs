using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class AuthSequence 
    {
        [JsonProperty("account", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }
    
        [JsonProperty("sequence", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double Sequence { get; set; }
    }
}