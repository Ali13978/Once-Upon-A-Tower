using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Body 
    {
        /// <summary>public key</summary>
        [JsonProperty("public_key", Required = Required.Always)]
        public string PublicKey { get; set; }
    }
}