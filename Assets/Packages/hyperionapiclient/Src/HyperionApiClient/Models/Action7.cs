using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Action7
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("ricardian_contract")]
        public string RicardianContract { get; set; }
    }
}