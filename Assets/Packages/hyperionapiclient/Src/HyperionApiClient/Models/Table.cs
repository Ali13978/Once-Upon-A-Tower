using System.Collections.Generic;
using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Table
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("index_type")]
        public string IndexType { get; set; }

        [JsonProperty("key_names")]
        public List<object> KeyNames { get; set; }

        [JsonProperty("key_types")]
        public List<object> KeyTypes { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}