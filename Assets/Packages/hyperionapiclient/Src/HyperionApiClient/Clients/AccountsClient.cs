using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HyperionApiClient.Dtos;
using HyperionApiClient.Models;
using HyperionApiClient.Responses;

namespace HyperionApiClient.Clients
{
    public class AccountsClient : ClientExtensions
    {
        public string BaseUrl { get; set; } = "https://api.wax.liquidstudios.io/";

        private readonly IHttpHandler _httpHandler;

        public AccountsClient(IHttpHandler httpHandler)
        {
            _httpHandler = httpHandler;
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>get created accounts</summary>
        /// <param name="account">creator account</param>
        /// <param name="limit">limit of [n] results per page</param>
        /// <param name="skip">skip [n] results</param>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetCreatedAccountsResponse> GetCreatedAccountsAsync(string account, int? limit = null, int? skip = null, CancellationToken cancellationToken = default)
        {
            if (account == null)
                throw new ArgumentNullException("account");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v2/history/get_created_accounts?" + Uri.EscapeDataString("account") + "=").Append(Uri.EscapeDataString(ConvertToString(account, CultureInfo.InvariantCulture))).Append("&");
            if (limit != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("limit") + "=").Append(Uri.EscapeDataString(ConvertToString(limit, CultureInfo.InvariantCulture))).Append("&");
            }
            if (skip != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("skip") + "=").Append(Uri.EscapeDataString(ConvertToString(skip, CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            return await _httpHandler.GetJsonAsync<GetCreatedAccountsResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>get account creator</summary>
        /// <param name="account">created account</param>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetCreatorResponse> GetCreatorAsync(string account, CancellationToken cancellationToken = default)
        {
            if (account == null)
                throw new ArgumentNullException("account");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v2/history/get_creator?" + Uri.EscapeDataString("account") + "=").Append(Uri.EscapeDataString(ConvertToString(account, CultureInfo.InvariantCulture))).Append("&");
            urlBuilder.Length--;

            var url = urlBuilder.ToString();
            return await _httpHandler.GetJsonAsync<GetCreatorResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>get account summary</summary>
        /// <param name="account">account name</param>
        /// <param name="limit">limit of [n] results per page</param>
        /// <param name="skip">skip [n] results</param>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetAccountResponse> GetAccountAsync(string account, int? limit = null, int? skip = null, CancellationToken cancellationToken = default)
        {
            if (account == null)
                throw new ArgumentNullException("account");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v2/state/get_account?" + Uri.EscapeDataString("account") + "=").Append(Uri.EscapeDataString(ConvertToString(account, CultureInfo.InvariantCulture))).Append("&");
            if (limit != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("limit") + "=").Append(Uri.EscapeDataString(ConvertToString(limit, CultureInfo.InvariantCulture))).Append("&");
            }
            if (skip != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("skip") + "=").Append(Uri.EscapeDataString(ConvertToString(skip, CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;

            var url = urlBuilder.ToString();
            return await _httpHandler.GetJsonAsync<GetAccountResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>get accounts by public key</summary>
        /// <param name="public_key">public key</param>
        /// <param name="limit">limit of [n] results per page</param>
        /// <param name="skip">skip [n] results</param>
        /// <param name="details">include permission details</param>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetKeyAccountsWithPermissionsResponse> GetKeyAccountsAsync(string publicKey, int? limit = null, int? skip = null, bool? details = null, CancellationToken cancellationToken = default)
        {
            if (publicKey == null)
                throw new ArgumentNullException("publicKey");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v2/state/get_key_accounts?" + Uri.EscapeDataString("public_key") + "=").Append(Uri.EscapeDataString(ConvertToString(publicKey, CultureInfo.InvariantCulture))).Append("&");
            if (limit != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("limit") + "=").Append(Uri.EscapeDataString(ConvertToString(limit, CultureInfo.InvariantCulture))).Append("&");
            }
            if (skip != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("skip") + "=").Append(Uri.EscapeDataString(ConvertToString(skip, CultureInfo.InvariantCulture))).Append("&");
            }
            if (details != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("details") + "=").Append(Uri.EscapeDataString(ConvertToString(details, CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;

            var url = urlBuilder.ToString();
            return await _httpHandler.GetJsonAsync<GetKeyAccountsWithPermissionsResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>get permission links</summary>
        /// <param name="account">account name</param>
        /// <param name="code">contract name</param>
        /// <param name="action">method name</param>
        /// <param name="permission">permission name</param>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetLinksResponse> GetLinksAsync(string account = null, string code = null, string action = null, string permission = null, CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v2/state/get_links?");
            if (account != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("account") + "=").Append(Uri.EscapeDataString(ConvertToString(account, CultureInfo.InvariantCulture))).Append("&");
            }
            if (code != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("code") + "=").Append(Uri.EscapeDataString(ConvertToString(code, CultureInfo.InvariantCulture))).Append("&");
            }
            if (action != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("action") + "=").Append(Uri.EscapeDataString(ConvertToString(action, CultureInfo.InvariantCulture))).Append("&");
            }
            if (permission != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("permission") + "=").Append(Uri.EscapeDataString(ConvertToString(permission, CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;

            var url = urlBuilder.ToString();
            return await _httpHandler.GetJsonAsync<GetLinksResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>get all tokens</summary>
        /// <param name="account">account name</param>
        /// <param name="limit">limit of [n] results per page</param>
        /// <param name="skip">skip [n] results</param>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetTokensResponse> GetTokensAsync(string account, int? limit = null, int? skip = null, CancellationToken cancellationToken = default)
        {
            // TODO return value?!

            if (account == null)
                throw new ArgumentNullException("account");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v2/state/get_tokens?" + Uri.EscapeDataString("account") + "=").Append(Uri.EscapeDataString(ConvertToString(account, CultureInfo.InvariantCulture))).Append("&");
            if (limit != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("limit") + "=").Append(Uri.EscapeDataString(ConvertToString(limit, CultureInfo.InvariantCulture))).Append("&");
            }
            if (skip != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("skip") + "=").Append(Uri.EscapeDataString(ConvertToString(skip, CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;


            var url = urlBuilder.ToString();
            return await _httpHandler.GetJsonAsync<GetTokensResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>get controlled accounts by controlling accounts</summary>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetControlledAccountsResponse> GetControlledAccountsAsync(string controllingAccount, CancellationToken cancellationToken = default)
        {
            if (controllingAccount == null)
                throw new ArgumentNullException("controllingAccount");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/history/get_controlled_accounts");
            var url = urlBuilder.ToString();

            var controllingAccountDto = new ControllingAccountDto
            {
                ControllingAccount = controllingAccount
            };

            return await _httpHandler.PostJsonAsync<GetControlledAccountsResponse>(url, controllingAccountDto, cancellationToken);
        }
    }
}