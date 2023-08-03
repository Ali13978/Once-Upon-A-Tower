using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Action2 
    {
        [JsonProperty("account_action_seq", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double AccountActionSeq { get; set; }
    
        [JsonProperty("global_action_seq", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double GlobalActionSeq { get; set; }
    
        [JsonProperty("block_num", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double BlockNum { get; set; }
    
        [JsonProperty("block_time", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string BlockTime { get; set; }
    
        [JsonProperty("action_trace", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public ActionTrace ActionTrace { get; set; }
    }
}