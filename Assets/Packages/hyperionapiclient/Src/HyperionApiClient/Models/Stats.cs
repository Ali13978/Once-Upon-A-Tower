using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Stats 
    {
        [JsonProperty("by_producer", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public object ByProducer { get; set; }
    }
}