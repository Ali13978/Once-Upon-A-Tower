using Newtonsoft.Json;

namespace AtomicAssetsApiClient
{
    public class StatsDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public DataDto Data { get; set; }

        [JsonProperty("query_time")]
        public long QueryTime { get; set; }

        public class DataDto
        {
            [JsonProperty("template_mint")]
            public int TemplateMint { get; set; }
        }
    }
}