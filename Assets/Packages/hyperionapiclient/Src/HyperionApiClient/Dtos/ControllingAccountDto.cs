using Newtonsoft.Json;

namespace HyperionApiClient.Dtos
{
    public class ControllingAccountDto
    {
        [JsonProperty("controlling_account")]
        public string ControllingAccount { get; set; }
    }
}