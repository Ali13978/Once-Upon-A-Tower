using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Transaction3
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("cpu_usage_us")]
        public int CpuUsageUs { get; set; }

        [JsonProperty("net_usage_words")]
        public int NetUsageWords { get; set; }

        [JsonProperty("trx")]
        public Trx Trx { get; set; }
    }
}