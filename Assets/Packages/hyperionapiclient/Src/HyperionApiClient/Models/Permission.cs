using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Permission 
    {
        [JsonProperty("owner", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Owner { get; set; }
    
        [JsonProperty("block_num", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int BlockNum { get; set; }
    
        [JsonProperty("parent", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Parent { get; set; }
    
        [JsonProperty("last_updated", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string LastUpdated { get; set; }
    
        [JsonProperty("auth", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public object Auth { get; set; }
    
        [JsonProperty("name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    
        [JsonProperty("present", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool Present { get; set; }
    }
}