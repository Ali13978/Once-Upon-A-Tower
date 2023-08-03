using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Event 
    {
        [JsonProperty("@timestamp", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Timestamp { get; set; }
    
        [JsonProperty("last_block", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double LastBlock { get; set; }
    
        [JsonProperty("schedule_version", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double ScheduleVersion { get; set; }
    
        [JsonProperty("size", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public double Size { get; set; }
    
        [JsonProperty("producer", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Producer { get; set; }
    }
}