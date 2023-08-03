using System.Collections.Generic;
using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Data
    {
        [JsonProperty("bytes")]
        public int? Bytes { get; set; }

        [JsonProperty("payer")]
        public string Payer { get; set; }

        [JsonProperty("receiver")]
        public string Receiver { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("amount")]
        public double? Amount { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("memo")]
        public string Memo { get; set; }

        [JsonProperty("sender")]
        public string Sender { get; set; }

        [JsonProperty("recipient")]
        public string Recipient { get; set; }

        [JsonProperty("sender_asset_ids")]
        public List<object> SenderAssetIds { get; set; }

        [JsonProperty("recipient_asset_ids")]
        public List<string> RecipientAssetIds { get; set; }

        [JsonProperty("offer_id")]
        public string OfferId { get; set; }
    }
}