using Newtonsoft.Json;

namespace HyperionApiClient.Dtos
{
    public class GetBlockByNumDto : BaseDto
    {
        [JsonProperty("block_num")]
        public uint? blockNum { get; set; }
    }
}