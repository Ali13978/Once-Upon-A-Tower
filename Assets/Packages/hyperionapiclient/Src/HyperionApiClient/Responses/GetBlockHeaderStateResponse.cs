using System.Collections.Generic;
using HyperionApiClient.Models;
using Newtonsoft.Json;

namespace HyperionApiClient.Responses
{
    public class GetBlockHeaderStateResponse
    {
        [JsonProperty("block_num")]
        public int BlockNum { get; set; }

        [JsonProperty("dpos_proposed_irreversible_blocknum")]
        public int DposProposedIrreversibleBlocknum { get; set; }

        [JsonProperty("dpos_irreversible_blocknum")]
        public int DposIrreversibleBlocknum { get; set; }

        [JsonProperty("active_schedule")]
        public ActiveSchedule ActiveSchedule { get; set; }

        [JsonProperty("blockroot_merkle")]
        public BlockrootMerkle BlockrootMerkle { get; set; }

        [JsonProperty("producer_to_last_produced")]
        public List<List<object>> ProducerToLastProduced { get; set; }

        [JsonProperty("producer_to_last_implied_irb")]
        public List<List<object>> ProducerToLastImpliedIrb { get; set; }

        [JsonProperty("valid_block_signing_authority")]
        public List<object> ValidBlockSigningAuthority { get; set; }

        [JsonProperty("confirm_count")]
        public List<int> ConfirmCount { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("pending_schedule")]
        public PendingSchedule PendingSchedule { get; set; }

        [JsonProperty("activated_protocol_features")]
        public ActivatedProtocolFeatures ActivatedProtocolFeatures { get; set; }

        [JsonProperty("additional_signatures")]
        public List<object> AdditionalSignatures { get; set; }
    }
}