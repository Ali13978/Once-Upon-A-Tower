using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HyperionApiClient.Models;
using HyperionApiClient.Responses;

namespace HyperionApiClient.Clients
{
    public class StatusClient : ClientExtensions
    {
        public string BaseUrl { get; set; } = "https://api.wax.liquidstudios.io/";

        private readonly IHttpHandler _httpHandler;

        public StatusClient(IHttpHandler httpHandler)
        {
            _httpHandler = httpHandler;
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>API Service Health Report</summary>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetHealthResponse> HealthAsync(CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v2/health");

            var url = urlBuilder.ToString();

            return await _httpHandler.GetJsonAsync<GetHealthResponse>(url, cancellationToken);
        }
    }
}