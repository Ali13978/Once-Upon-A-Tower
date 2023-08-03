using System.Collections.Generic;
using HyperionApiClient.Models;
using Newtonsoft.Json;

namespace HyperionApiClient.Responses
{
    public class GetProducersResponse
    {
        [JsonProperty("rows")]
        public List<ProducerRow> Rows { get; set; }

        [JsonProperty("total_producer_vote_weight")]
        public string TotalProducerVoteWeight { get; set; }

        [JsonProperty("more")]
        public string More { get; set; }
    }
}