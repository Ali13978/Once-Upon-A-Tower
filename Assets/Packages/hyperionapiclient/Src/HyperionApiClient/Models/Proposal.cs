using System.Collections.Generic;
using Newtonsoft.Json;

namespace HyperionApiClient.Models
{
    public class Proposal
    {
        [JsonProperty("provided_approvals")]
        public List<ProvidedApproval> ProvidedApprovals { get; set; }

        [JsonProperty("block_num")]
        public int BlockNum { get; set; }

        [JsonProperty("proposer")]
        public string Proposer { get; set; }

        [JsonProperty("requested_approvals")]
        public List<RequestedApproval> RequestedApprovals { get; set; }

        [JsonProperty("proposal_name")]
        public string ProposalName { get; set; }

        [JsonProperty("executed")]
        public bool Executed { get; set; }

        [JsonProperty("primary_key")]
        public string PrimaryKey { get; set; }
    }
}