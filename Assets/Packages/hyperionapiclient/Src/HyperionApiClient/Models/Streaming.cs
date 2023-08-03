using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Streaming
    {
        [JsonProperty("enable")]
        public bool Enable { get; set; }

        [JsonProperty("traces")]
        public bool Traces { get; set; }

        [JsonProperty("deltas")]
        public bool Deltas { get; set; }
    }
}