using Newtonsoft.Json;

namespace HyperionApiClient.Dtos
{
    public class GetBlockByNumOrIdDto
    {
        [JsonProperty("block_num_or_id")]
        public string BlockNumOrId { get; set; }
    }
}