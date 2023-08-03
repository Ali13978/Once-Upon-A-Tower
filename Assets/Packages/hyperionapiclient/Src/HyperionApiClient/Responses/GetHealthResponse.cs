using System.Collections.Generic;
using HyperionApiClient.Models;
using Newtonsoft.Json;

namespace HyperionApiClient.Responses
{
    public class GetHealthResponse
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("version_hash")]
        public string VersionHash { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("features")]
        public Features Features { get; set; }

        [JsonProperty("health")]
        public List<Health> Health { get; set; }

        [JsonProperty("query_time_ms")]
        public double QueryTimeMs { get; set; }
    }
}