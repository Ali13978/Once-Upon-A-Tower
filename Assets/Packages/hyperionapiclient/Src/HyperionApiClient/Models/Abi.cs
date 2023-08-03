using System.Collections.Generic;
using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Abi
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("types")]
        public List<object> Types { get; set; }

        [JsonProperty("structs")]
        public List<Struct> Structs { get; set; }

        [JsonProperty("actions")]
        public List<Action7> Actions { get; set; }

        [JsonProperty("tables")]
        public List<Table> Tables { get; set; }

        [JsonProperty("ricardian_clauses")]
        public List<RicardianClaus> RicardianClauses { get; set; }

        [JsonProperty("error_messages")]
        public List<object> ErrorMessages { get; set; }

        [JsonProperty("abi_extensions")]
        public List<object> AbiExtensions { get; set; }

        [JsonProperty("variants")]
        public List<object> Variants { get; set; }
    }
}
