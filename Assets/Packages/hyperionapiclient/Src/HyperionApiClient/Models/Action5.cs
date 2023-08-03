using System.Collections.Generic;
using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Action5
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("authorization")]
        public List<Authorization2> Authorization { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("hex_data")]
        public string HexData { get; set; }
    }
}