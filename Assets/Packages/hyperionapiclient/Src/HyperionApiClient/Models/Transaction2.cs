using System.Collections.Generic;
using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Transaction2
    {
        [JsonProperty("expiration")]
        public string Expiration { get; set; }

        [JsonProperty("ref_block_num")]
        public int RefBlockNum { get; set; }

        [JsonProperty("ref_block_prefix")]
        public long RefBlockPrefix { get; set; }

        [JsonProperty("max_net_usage_words")]
        public int MaxNetUsageWords { get; set; }

        [JsonProperty("max_cpu_usage_ms")]
        public int MaxCpuUsageMs { get; set; }

        [JsonProperty("delay_sec")]
        public int DelaySec { get; set; }

        [JsonProperty("context_free_actions")]
        public List<object> ContextFreeActions { get; set; }

        [JsonProperty("actions")]
        public List<Action5> Actions { get; set; }
    }
}