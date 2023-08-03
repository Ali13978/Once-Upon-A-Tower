using System.Collections.Generic;
using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class ActivatedProtocolFeature
    {
        [JsonProperty("feature_digest")]
        public string FeatureDigest { get; set; }

        [JsonProperty("activation_ordinal")]
        public int ActivationOrdinal { get; set; }

        [JsonProperty("activation_block_num")]
        public int ActivationBlockNum { get; set; }

        [JsonProperty("description_digest")]
        public string DescriptionDigest { get; set; }

        [JsonProperty("dependencies")]
        public List<string> Dependencies { get; set; }

        [JsonProperty("protocol_feature_type")]
        public string ProtocolFeatureType { get; set; }

        [JsonProperty("specification")]
        public List<Specification> Specification { get; set; }
    }
}