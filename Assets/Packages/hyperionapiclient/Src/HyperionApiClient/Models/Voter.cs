using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Voter 
    {
        [JsonProperty("account", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }
    
        [JsonProperty("weight", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double Weight { get; set; }
    
        [JsonProperty("last_vote", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double LastVote { get; set; }
    
        [JsonProperty("data", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }
    }
}