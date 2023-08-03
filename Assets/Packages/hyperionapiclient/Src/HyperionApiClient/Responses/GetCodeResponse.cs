using Newtonsoft.Json;

namespace HyperionApiClient.Responses
{
    public class GetCodeResponse
    {
        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("code_hash")]
        public string CodeHash { get; set; }

        [JsonProperty("wast")]
        public string Wast { get; set; }

        [JsonProperty("wasm")]
        public string Wasm { get; set; }
    }
}