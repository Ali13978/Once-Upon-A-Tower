using Newtonsoft.Json;

namespace AtomicAssetsApiClient.Burns
{
    public class BurnDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public DataDto Data { get; set; }

        public class DataDto
        {
            [JsonProperty("collections")]
            public CollectionsDto[] Collections { get; set; }

            [JsonProperty("templates")]
            public TemplatesDto[] Templates { get; set; }

            [JsonProperty("assets")]
            public string Assets { get; set; }

            public class CollectionsDto
            {
                [JsonProperty("collection_name")]
                public string CollectionName { get; set; }

                [JsonProperty("assets")]
                public string Assets { get; set; }
            }

            public class TemplatesDto
            {
                [JsonProperty("collection_name")]
                public string CollectionName { get; set; }

                [JsonProperty("template_id")]
                public string TemplateId { get; set; }

                [JsonProperty("assets")]
                public string Assets { get; set; }
            }
        }
    }
}