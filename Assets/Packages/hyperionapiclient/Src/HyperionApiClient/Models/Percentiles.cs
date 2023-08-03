using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Percentiles
    {
        [JsonProperty("1.0")]
        public double _10 { get; set; }

        [JsonProperty("5.0")]
        public double _50 { get; set; }

        [JsonProperty("25.0")]
        public double _250 { get; set; }

        [JsonProperty("50.0")]
        public double _500 { get; set; }

        [JsonProperty("75.0")]
        public double _750 { get; set; }

        [JsonProperty("95.0")]
        public double _950 { get; set; }

        [JsonProperty("99.0")]
        public double _990 { get; set; }
    }
}