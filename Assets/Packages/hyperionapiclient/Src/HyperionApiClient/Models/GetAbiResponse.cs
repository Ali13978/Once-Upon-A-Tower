using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class GetAbiResponse
    {
        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("abi")]
        public Abi Abi { get; set; }
    }
}