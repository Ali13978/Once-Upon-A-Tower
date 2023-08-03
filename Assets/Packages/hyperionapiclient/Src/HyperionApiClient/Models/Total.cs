using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Total 
    {
        [JsonProperty("value", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double Value { get; set; }
    
        [JsonProperty("relation", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Relation { get; set; }
    }
}