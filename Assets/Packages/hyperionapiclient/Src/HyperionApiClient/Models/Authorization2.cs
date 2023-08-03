using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Authorization2
    {
        [JsonProperty("actor")]
        public string Actor { get; set; }

        [JsonProperty("permission")]
        public string Permission { get; set; }
    }
}