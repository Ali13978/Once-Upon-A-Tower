using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HyperionApiClient.Models;
using HyperionApiClient.Responses;

namespace HyperionApiClient.Clients
{
    public class SystemClient : ClientExtensions
    {
        public string BaseUrl { get; set; } = "https://api.wax.liquidstudios.io/";

        private readonly IHttpHandler _httpHandler;

        public SystemClient(IHttpHandler httpHandler)
        {
            _httpHandler = httpHandler;
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>get proposals</summary>
        /// <param name="proposer">filter by proposer</param>
        /// <param name="proposal">filter by proposal name</param>
        /// <param name="account">filter by either requested or provided account</param>
        /// <param name="requested">filter by requested account</param>
        /// <param name="provided">filter by provided account</param>
        /// <param name="executed">filter by execution status</param>
        /// <param name="track">total results to track (count) [number or true]</param>
        /// <param name="skip">skip [n] actions (pagination)</param>
        /// <param name="limit">limit of [n] actions per page</param>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetProposalsResponse> GetProposalsAsync(string proposer = null, string proposal = null, string account = null, string requested = null, string provided = null, bool? executed = null, string track = null, int? skip = null, int? limit = null, CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v2/state/get_proposals?");
            if (proposer != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("proposer") + "=").Append(Uri.EscapeDataString(ConvertToString(proposer, CultureInfo.InvariantCulture))).Append("&");
            }
            if (proposal != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("proposal") + "=").Append(Uri.EscapeDataString(ConvertToString(proposal, CultureInfo.InvariantCulture))).Append("&");
            }
            if (account != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("account") + "=").Append(Uri.EscapeDataString(ConvertToString(account, CultureInfo.InvariantCulture))).Append("&");
            }
            if (requested != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("requested") + "=").Append(Uri.EscapeDataString(ConvertToString(requested, CultureInfo.InvariantCulture))).Append("&");
            }
            if (provided != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("provided") + "=").Append(Uri.EscapeDataString(ConvertToString(provided, CultureInfo.InvariantCulture))).Append("&");
            }
            if (executed != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("executed") + "=").Append(Uri.EscapeDataString(ConvertToString(executed, CultureInfo.InvariantCulture))).Append("&");
            }
            if (track != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("track") + "=").Append(Uri.EscapeDataString(ConvertToString(track, CultureInfo.InvariantCulture))).Append("&");
            }
            if (skip != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("skip") + "=").Append(Uri.EscapeDataString(ConvertToString(skip, CultureInfo.InvariantCulture))).Append("&");
            }
            if (limit != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("limit") + "=").Append(Uri.EscapeDataString(ConvertToString(limit, CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            return await _httpHandler.GetJsonAsync<GetProposalsResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>get voters</summary>
        /// <param name="limit">limit of [n] results per page</param>
        /// <param name="skip">skip [n] results</param>
        /// <param name="producer">filter by voted producer (comma separated)</param>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetVotersResponse> GetVotersAsync(int? limit = null, int? skip = null, string producer = null, CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v2/state/get_voters?");
            if (limit != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("limit") + "=").Append(Uri.EscapeDataString(ConvertToString(limit, CultureInfo.InvariantCulture))).Append("&");
            }
            if (skip != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("skip") + "=").Append(Uri.EscapeDataString(ConvertToString(skip, CultureInfo.InvariantCulture))).Append("&");
            }
            if (producer != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("producer") + "=").Append(Uri.EscapeDataString(ConvertToString(producer, CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            return await _httpHandler.GetJsonAsync<GetVotersResponse>(url, cancellationToken);
        }
    }
}