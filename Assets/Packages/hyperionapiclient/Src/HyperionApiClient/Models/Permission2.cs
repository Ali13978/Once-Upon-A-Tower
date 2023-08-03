using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Permission2
    {
        [JsonProperty("perm_name")]
        public string PermName { get; set; }

        [JsonProperty("parent")]
        public string Parent { get; set; }

        [JsonProperty("required_auth")]
        public RequiredAuth RequiredAuth { get; set; }
    }
}