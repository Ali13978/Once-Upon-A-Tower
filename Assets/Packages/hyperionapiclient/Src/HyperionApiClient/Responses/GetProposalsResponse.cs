using System.Collections.Generic;
using HyperionApiClient.Models;
using Newtonsoft.Json;

namespace HyperionApiClient.Responses
{
    public class GetProposalsResponse
    {
        [JsonProperty("query_time")]
        public object QueryTime { get; set; }

        [JsonProperty("cached")]
        public bool Cached { get; set; }

        [JsonProperty("total")]
        public Total Total { get; set; }

        [JsonProperty("proposals")]
        public List<Proposal> Proposals { get; set; }

        [JsonProperty("query_time_ms")]
        public double QueryTimeMs { get; set; }
    }
}