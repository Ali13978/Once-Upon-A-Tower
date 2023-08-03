using Newtonsoft.Json;

namespace AtomicAssetsApiClient.Accounts
{
    public class AccountCollectionDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public DataDto[] Data { get; set; }

        public class DataDto
        {
            [JsonProperty("schemas")]
            public SchemasDto[] Schemas { get; set; }

            [JsonProperty("templates")]
            public TemplatesDto[] Templates { get; set; }

            [JsonProperty("assets")]
            public string Assets { get; set; }

            public class SchemasDto
            {
                [JsonProperty("schema_name")]
                public string SchemaName { get; set; }

                [JsonProperty("assets")]
                public string Assets { get; set; }
            }

            public class TemplatesDto
            {
                [JsonProperty("template_id")]
                public string TemplateId { get; set; }

                [JsonProperty("assets")]
                public string Assets { get; set; }
            }
        }
    }
}