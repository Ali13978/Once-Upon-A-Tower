using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Authorization 
    {
        [JsonProperty("account", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }
    
        [JsonProperty("permission", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Permission { get; set; }
    }
}