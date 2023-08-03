using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class StdDeviationBounds
    {
        [JsonProperty("upper")]
        public double Upper { get; set; }

        [JsonProperty("lower")]
        public double Lower { get; set; }

        [JsonProperty("upper_population")]
        public double UpperPopulation { get; set; }

        [JsonProperty("lower_population")]
        public double LowerPopulation { get; set; }

        [JsonProperty("upper_sampling")]
        public double UpperSampling { get; set; }

        [JsonProperty("lower_sampling")]
        public double LowerSampling { get; set; }
    }
}