using Newtonsoft.Json;

namespace HyperionApiClient.Dtos
{
    internal class GetBlockByIdDto : BaseDto
    {
        [JsonProperty("block_id")]
        public string blockId { get; set; }
    }
}