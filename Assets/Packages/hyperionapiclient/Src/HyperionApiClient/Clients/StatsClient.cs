using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HyperionApiClient.Models;
using HyperionApiClient.Responses;

namespace HyperionApiClient.Clients
{
    public class StatsClient : ClientExtensions
    {
        public string BaseUrl { get; set; } = "https://api.wax.liquidstudios.io/";

        private readonly IHttpHandler _httpHandler;

        public StatsClient(IHttpHandler httpHandler)
        {
            _httpHandler = httpHandler;
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>get action and transaction stats for a given period</summary>
        /// <param name="period">analysis period</param>
        /// <param name="end_date">final date</param>
        /// <param name="unique_actors">compute unique actors</param>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetActionUsageResponse> GetActionUsageAsync(string period, string endDate = null, bool? uniqueActors = null, CancellationToken cancellationToken = default)
        {
            if (period == null)
                throw new ArgumentNullException("period");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v2/stats/get_action_usage?" + Uri.EscapeDataString("period") + "=").Append(Uri.EscapeDataString(ConvertToString(period, CultureInfo.InvariantCulture))).Append("&");
            if (endDate != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("end_date") + "=").Append(Uri.EscapeDataString(ConvertToString(endDate, CultureInfo.InvariantCulture))).Append("&");
            }
            if (uniqueActors != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("unique_actors") + "=").Append(Uri.EscapeDataString(ConvertToString(uniqueActors, CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            return await _httpHandler.GetJsonAsync<GetActionUsageResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>get missed blocks</summary>
        /// <param name="producer">filter by producer</param>
        /// <param name="after">filter after specified date (ISO8601)</param>
        /// <param name="before">filter before specified date (ISO8601)</param>
        /// <param name="min_blocks">min. blocks threshold</param>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetMissedBlocksResponse> GetMissedBlocksAsync(string producer = null, string after = null, string before = null, int? minBlocks = null, CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v2/stats/get_missed_blocks?");
            if (producer != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("producer") + "=").Append(Uri.EscapeDataString(ConvertToString(producer, CultureInfo.InvariantCulture))).Append("&");
            }
            if (after != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("after") + "=").Append(Uri.EscapeDataString(ConvertToString(after, CultureInfo.InvariantCulture))).Append("&");
            }
            if (before != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("before") + "=").Append(Uri.EscapeDataString(ConvertToString(before, CultureInfo.InvariantCulture))).Append("&");
            }
            if (minBlocks != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("min_blocks") + "=").Append(Uri.EscapeDataString(ConvertToString(minBlocks, CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            return await _httpHandler.GetJsonAsync<GetMissedBlocksResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>get resource usage stats for a specific action</summary>
        /// <param name="code">contract</param>
        /// <param name="action">action name</param>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetResourceUsageResponse> GetResourceUsageAsync(string code, string action, CancellationToken cancellationToken = default)
        {
            // TODO return value

            if (code == null)
                throw new ArgumentNullException("code");

            if (action == null)
                throw new ArgumentNullException("action");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v2/stats/get_resource_usage?" + Uri.EscapeDataString("code") + "=").Append(Uri.EscapeDataString(ConvertToString(code, CultureInfo.InvariantCulture))).Append("&" + Uri.EscapeDataString("action") + "=").Append(Uri.EscapeDataString(ConvertToString(action, CultureInfo.InvariantCulture))).Append("&");
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            return await _httpHandler.GetJsonAsync<GetResourceUsageResponse>(url, cancellationToken);
        }
    }
}