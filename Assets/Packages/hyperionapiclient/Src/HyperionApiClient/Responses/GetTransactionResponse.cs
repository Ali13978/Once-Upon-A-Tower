using System.Collections.Generic;
using HyperionApiClient.Models;
using Newtonsoft.Json;

namespace HyperionApiClient.Responses
{
    public class GetTransactionResponse
    {
        [JsonProperty("executed")]
        public bool Executed { get; set; }

        [JsonProperty("hot_only")]
        public bool HotOnly { get; set; }

        [JsonProperty("trx_id")]
        public string TrxId { get; set; }

        [JsonProperty("lib")]
        public int Lib { get; set; }

        [JsonProperty("actions")]
        public List<Action6> Actions { get; set; }

        [JsonProperty("query_time_ms")]
        public double QueryTimeMs { get; set; }
    }
}