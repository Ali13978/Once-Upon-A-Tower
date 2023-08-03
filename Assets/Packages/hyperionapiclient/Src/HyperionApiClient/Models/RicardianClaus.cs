using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class RicardianClaus
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}