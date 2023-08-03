using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Delta 
    {
        [JsonProperty("timestamp", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Timestamp { get; set; }
    
        [JsonProperty("code", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
    
        [JsonProperty("scope", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Scope { get; set; }
    
        [JsonProperty("table", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Table { get; set; }
    
        [JsonProperty("primary_key", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string PrimaryKey { get; set; }
    
        [JsonProperty("payer", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Payer { get; set; }
    
        [JsonProperty("present", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool Present { get; set; }
    
        [JsonProperty("block_num", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double BlockNum { get; set; }
    
        [JsonProperty("block_id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string BlockId { get; set; }
    
        [JsonProperty("data", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }
    }
}