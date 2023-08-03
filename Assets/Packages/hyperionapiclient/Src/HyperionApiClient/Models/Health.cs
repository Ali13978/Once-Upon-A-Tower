using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Health
    {
        [JsonProperty("service")]
        public string Service { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("time")]
        public object Time { get; set; }

        [JsonProperty("service_data")]
        public ServiceData ServiceData { get; set; }
    }
}