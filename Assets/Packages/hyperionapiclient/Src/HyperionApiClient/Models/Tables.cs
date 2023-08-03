using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Tables
    {
        [JsonProperty("proposals")]
        public bool Proposals { get; set; }

        [JsonProperty("accounts")]
        public bool Accounts { get; set; }

        [JsonProperty("voters")]
        public bool Voters { get; set; }
    }
}