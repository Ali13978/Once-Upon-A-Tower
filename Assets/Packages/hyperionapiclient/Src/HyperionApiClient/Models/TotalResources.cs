using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class TotalResources
    {
        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("net_weight")]
        public string NetWeight { get; set; }

        [JsonProperty("cpu_weight")]
        public string CpuWeight { get; set; }

        [JsonProperty("ram_bytes")]
        public int RamBytes { get; set; }
    }
}