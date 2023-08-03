using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HyperionApiClient.Dtos;
using HyperionApiClient.Models;
using HyperionApiClient.Responses;

namespace HyperionApiClient.Clients
{
    public class ChainClient : ClientExtensions
    {
        public string BaseUrl { get; set; } = "https://api.wax.liquidstudios.io/";

        private readonly IHttpHandler _httpHandler;

        public ChainClient(IHttpHandler httpHandler)
        {
            _httpHandler = httpHandler;
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns an object containing rows from the specified table.</summary>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task AbiBinToJsonAsync(string code, string action, string binargs, CancellationToken cancellationToken = default)
        {
            // TODO return value

            if (code == null)
                throw new ArgumentNullException("code");

            if (action == null)
                throw new ArgumentNullException("action");

            if (binargs == null)
                throw new ArgumentNullException("binargs");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/abi_bin_to_json?" + Uri.EscapeDataString("code") + "=").Append(Uri.EscapeDataString(ConvertToString(code, CultureInfo.InvariantCulture))).Append("&" + Uri.EscapeDataString("action") + "=").Append(Uri.EscapeDataString(ConvertToString(action, CultureInfo.InvariantCulture))).Append("&" + Uri.EscapeDataString("binargs") + "=").Append(Uri.EscapeDataString(ConvertToString(binargs, CultureInfo.InvariantCulture))).Append("&");
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            await _httpHandler.GetJsonAsync<string>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Convert JSON object to binary</summary>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task AbiJsonToBinAsync(string binargs, CancellationToken cancellationToken = default)
        {
            // TODO return value

            if (binargs == null)
                throw new ArgumentNullException("binargs");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/abi_json_to_bin?" + Uri.EscapeDataString("binargs") + "=").Append(Uri.EscapeDataString(ConvertToString(binargs, CultureInfo.InvariantCulture))).Append("&");
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            await _httpHandler.GetJsonAsync<string>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Retrieves the ABI for a contract based on its account name</summary>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetAbiResponse> GetAbiAsync(string accountName, CancellationToken cancellationToken = default)
        {
            if (accountName == null)
                throw new ArgumentNullException("accountName");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/get_abi?" + Uri.EscapeDataString("account_name") + "=").Append(Uri.EscapeDataString(ConvertToString(accountName, CultureInfo.InvariantCulture))).Append("&");
            urlBuilder.Length--;

            var url = urlBuilder.ToString();
            return await _httpHandler.GetJsonAsync<GetAbiResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns an object containing various details about a specific account on the blockchain.</summary>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetAccountResponse2> GetAccountAsync(string accountName, CancellationToken cancellationToken = default)
        {
            if (accountName == null)
                throw new ArgumentNullException("accountName");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/get_account?" + Uri.EscapeDataString("account_name") + "=").Append(Uri.EscapeDataString(ConvertToString(accountName, CultureInfo.InvariantCulture))).Append("&");
            urlBuilder.Length--;

            var url = urlBuilder.ToString();
            return await _httpHandler.GetJsonAsync<GetAccountResponse2>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Retreives the activated protocol features for producer node</summary>
        /// <param name="lower_bound">Lower bound</param>
        /// <param name="upper_bound">Upper bound</param>
        /// <param name="limit">The limit, default is 10</param>
        /// <param name="search_by_block_num">Flag to indicate it is has to search by block number</param>
        /// <param name="reverse">Flag to indicate it has to search in reverse</param>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetActivatedProtocolFeaturesResponse> GetActivatedProtocolFeaturesAsync(int? lowerBound = null, int? upperBound = null, int? limit = null, bool? searchByBlockNum = null, bool? reverse = null, CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/get_activated_protocol_features?");
            if (lowerBound != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("lower_bound") + "=").Append(Uri.EscapeDataString(ConvertToString(lowerBound, CultureInfo.InvariantCulture))).Append("&");
            }
            if (upperBound != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("upper_bound") + "=").Append(Uri.EscapeDataString(ConvertToString(upperBound, CultureInfo.InvariantCulture))).Append("&");
            }
            if (limit != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("limit") + "=").Append(Uri.EscapeDataString(ConvertToString(limit, CultureInfo.InvariantCulture))).Append("&");
            }
            if (searchByBlockNum != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("search_by_block_num") + "=").Append(Uri.EscapeDataString(ConvertToString(searchByBlockNum, CultureInfo.InvariantCulture))).Append("&");
            }
            if (reverse != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("reverse") + "=").Append(Uri.EscapeDataString(ConvertToString(reverse, CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            return await _httpHandler.GetJsonAsync<GetActivatedProtocolFeaturesResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns an object containing various details about a specific block on the blockchain.</summary>
        /// <param name="block_num_or_id">Provide a `block number` or a `block id`</param>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetBlockResponse2> GetBlockAsync(string blockNumOrId, CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/get_block");
            var url = urlBuilder.ToString();
            var dataDto = new GetBlockByNumOrIdDto
            {
                BlockNumOrId = blockNumOrId
            };

            return await _httpHandler.PostJsonAsync<GetBlockResponse2>(url, dataDto, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Retrieves the block header state</summary>
        /// <param name="blockNumOrId">Provide a block_number or a block_id</param>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetBlockHeaderStateResponse> GetBlockHeaderStateAsync(string blockNumOrId, CancellationToken cancellationToken = default)
        {
            // TODO return value

            if (blockNumOrId == null)
                throw new ArgumentNullException("blockNumOrId");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/get_block_header_state?" + Uri.EscapeDataString("block_num_or_id") + "=").Append(Uri.EscapeDataString(ConvertToString(blockNumOrId, CultureInfo.InvariantCulture))).Append("&");
            urlBuilder.Length--;

            var url = urlBuilder.ToString();
            return await _httpHandler.GetJsonAsync<GetBlockHeaderStateResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Retrieves contract code</summary>
        /// <param name="code_as_wasm">This must be 1 (true)</param>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetCodeResponse> GetCodeAsync(string accountName, bool codeAsWasm, CancellationToken cancellationToken = default)
        {
            if (accountName == null)
                throw new ArgumentNullException("accountName");

            if (!codeAsWasm)
                throw new Exception("codeAsWasm must be true");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/get_code?" + Uri.EscapeDataString("account_name") + "=").Append(Uri.EscapeDataString(ConvertToString(accountName, CultureInfo.InvariantCulture))).Append("&" + Uri.EscapeDataString("code_as_wasm") + "=1").Append("&");
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            return await _httpHandler.GetJsonAsync<GetCodeResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Retrieves the current balance</summary>
        /// <param name="symbol">A symbol composed of capital letters between 1-7.</param>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<List<string>> GetCurrencyBalanceAsync(string code, string account, string symbol, CancellationToken cancellationToken = default)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            if (account == null)
                throw new ArgumentNullException("account");

            if (symbol == null)
                throw new ArgumentNullException("symbol");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/get_currency_balance?" + Uri.EscapeDataString("code") + "=").Append(Uri.EscapeDataString(ConvertToString(code, CultureInfo.InvariantCulture))).Append("&" + Uri.EscapeDataString("account") + "=").Append(Uri.EscapeDataString(ConvertToString(account, CultureInfo.InvariantCulture))).Append("&" + Uri.EscapeDataString("symbol") + "=").Append(Uri.EscapeDataString(ConvertToString(symbol, CultureInfo.InvariantCulture))).Append("&");
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            return await _httpHandler.GetJsonAsync<List<string>>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Retrieves currency stats</summary>
        /// <param name="code">contract name</param>
        /// <param name="symbol">token symbol</param>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<string> GetCurrencyStatsAsync(string code, string symbol, CancellationToken cancellationToken = default)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            if (symbol == null)
                throw new ArgumentNullException("symbol");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/get_currency_stats?" + Uri.EscapeDataString("code") + "=").Append(Uri.EscapeDataString(ConvertToString(code, CultureInfo.InvariantCulture))).Append("&" + Uri.EscapeDataString("symbol") + "=").Append(Uri.EscapeDataString(ConvertToString(symbol, CultureInfo.InvariantCulture))).Append("&");
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            return await _httpHandler.GetJsonAsync<string>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns an object containing various details about the blockchain.</summary>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetInfoResponse> GetInfoAsync(CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/get_info");
            var url = urlBuilder.ToString();

            return await _httpHandler.GetJsonAsync<GetInfoResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Retrieves producers list</summary>
        /// <param name="limit">total number of producers to retrieve</param>
        /// <param name="lower_bound">In conjunction with limit can be used to paginate through the results. For example, limit=10 and lower_bound=10 would be page 2</param>
        /// <param name="json">return result in JSON format</param>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetProducersResponse> GetProducersAsync(string limit = null, string lowerBound = null, bool? json = null, CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/get_producers?");
            if (limit != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("limit") + "=").Append(Uri.EscapeDataString(ConvertToString(limit, CultureInfo.InvariantCulture))).Append("&");
            }
            if (lowerBound != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("lower_bound") + "=").Append(Uri.EscapeDataString(ConvertToString(lowerBound, CultureInfo.InvariantCulture))).Append("&");
            }
            if (json != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("json") + "=").Append(Uri.EscapeDataString(ConvertToString(json, CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            return await _httpHandler.GetJsonAsync<GetProducersResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Retrieves raw ABI for a contract based on account name</summary>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task GetRawAbiAsync(string accountName, CancellationToken cancellationToken = default)
        {
            // TODO return value

            if (accountName == null)
                throw new ArgumentNullException("accountName");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/get_raw_abi?" + Uri.EscapeDataString("account_name") + "=").Append(Uri.EscapeDataString(ConvertToString(accountName, CultureInfo.InvariantCulture))).Append("&");
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            await _httpHandler.GetJsonAsync<string>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Retrieves raw code and ABI for a contract based on account name</summary>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task GetRawCodeAndAbiAsync(string accountName, CancellationToken cancellationToken = default)
        {
            // TODO return value

            if (accountName == null)
                throw new ArgumentNullException("accountName");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/get_raw_code_and_abi?" + Uri.EscapeDataString("account_name") + "=").Append(Uri.EscapeDataString(ConvertToString(accountName, CultureInfo.InvariantCulture))).Append("&");
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            await _httpHandler.GetJsonAsync<string>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Retrieves the scheduled transaction</summary>
        /// <param name="lower_bound">Date/time string in the format YYYY-MM-DDTHH:MM:SS.sss</param>
        /// <param name="limit">The maximum number of transactions to return</param>
        /// <param name="json">true/false whether the packed transaction is converted to json</param>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task GetScheduledTransactionAsync(string lowerBound = null, int? limit = null, bool? json = null, CancellationToken cancellationToken = default)
        {
            // TODO return value

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/get_scheduled_transaction?");
            if (lowerBound != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("lower_bound") + "=").Append(Uri.EscapeDataString(ConvertToString(lowerBound, CultureInfo.InvariantCulture))).Append("&");
            }
            if (limit != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("limit") + "=").Append(Uri.EscapeDataString(ConvertToString(limit, CultureInfo.InvariantCulture))).Append("&");
            }
            if (json != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("json") + "=").Append(Uri.EscapeDataString(ConvertToString(json, CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            await _httpHandler.GetJsonAsync<string>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Retrieves table scope</summary>
        /// <param name="code">`name` of the contract to return table data for</param>
        /// <param name="table">Filter results by table</param>
        /// <param name="lower_bound">Filters results to return the first element that is not less than provided value in set</param>
        /// <param name="upper_bound">Filters results to return the first element that is greater than provided value in set</param>
        /// <param name="limit">Limit number of results returned.</param>
        /// <param name="reverse">Reverse the order of returned results</param>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<GetTableByScopeResponse> GetTableByScopeAsync(string code, string table = null, string lowerBound = null, string upperBound = null, int? limit = null, bool? reverse = null, CancellationToken cancellationToken = default)
        {
            // TODO return value

            if (code == null)
                throw new ArgumentNullException("code");

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/get_table_by_scope?" + Uri.EscapeDataString("code") + "=").Append(Uri.EscapeDataString(ConvertToString(code, CultureInfo.InvariantCulture))).Append("&");
            if (table != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("table") + "=").Append(Uri.EscapeDataString(ConvertToString(table, CultureInfo.InvariantCulture))).Append("&");
            }
            if (lowerBound != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("lower_bound") + "=").Append(Uri.EscapeDataString(ConvertToString(lowerBound, CultureInfo.InvariantCulture))).Append("&");
            }
            if (upperBound != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("upper_bound") + "=").Append(Uri.EscapeDataString(ConvertToString(upperBound, CultureInfo.InvariantCulture))).Append("&");
            }
            if (limit != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("limit") + "=").Append(Uri.EscapeDataString(ConvertToString(limit, CultureInfo.InvariantCulture))).Append("&");
            }
            if (reverse != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("reverse") + "=").Append(Uri.EscapeDataString(ConvertToString(reverse, CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            return await _httpHandler.GetJsonAsync<GetTableByScopeResponse>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Returns an object containing rows from the specified table.</summary>
        /// <param name="code">The name of the smart contract that controls the provided table</param>
        /// <param name="table">The name of the table to query</param>
        /// <param name="scope">The account to which this data belongs</param>
        /// <param name="index_position">Position of the index used, accepted parameters `primary`, `secondary`, `tertiary`, `fourth`, `fifth`, `sixth`, `seventh`, `eighth`, `ninth` , `tenth`</param>
        /// <param name="key_type">Type of key specified by index_position (for example - `uint64_t` or `name`)</param>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task GetTableRowsAsync(string code = null, string table = null, string scope = null, string indexPosition = null, string keyType = null, string encodeType = null, string upperBound = null, string lowerBound = null, CancellationToken cancellationToken = default)
        {
            // TODO return value

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/get_table_rows?");
            if (code != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("code") + "=").Append(Uri.EscapeDataString(ConvertToString(code, CultureInfo.InvariantCulture))).Append("&");
            }
            if (table != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("table") + "=").Append(Uri.EscapeDataString(ConvertToString(table, CultureInfo.InvariantCulture))).Append("&");
            }
            if (scope != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("scope") + "=").Append(Uri.EscapeDataString(ConvertToString(scope, CultureInfo.InvariantCulture))).Append("&");
            }
            if (indexPosition != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("index_position") + "=").Append(Uri.EscapeDataString(ConvertToString(indexPosition, CultureInfo.InvariantCulture))).Append("&");
            }
            if (keyType != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("key_type") + "=").Append(Uri.EscapeDataString(ConvertToString(keyType, CultureInfo.InvariantCulture))).Append("&");
            }
            if (encodeType != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("encode_type") + "=").Append(Uri.EscapeDataString(ConvertToString(encodeType, CultureInfo.InvariantCulture))).Append("&");
            }
            if (upperBound != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("upper_bound") + "=").Append(Uri.EscapeDataString(ConvertToString(upperBound, CultureInfo.InvariantCulture))).Append("&");
            }
            if (lowerBound != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("lower_bound") + "=").Append(Uri.EscapeDataString(ConvertToString(lowerBound, CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder.Length--;

            var url = urlBuilder.ToString();

            await _httpHandler.GetJsonAsync<string>(url, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>This method expects a transaction in JSON format and will attempt to apply it to the blockchain.</summary>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task PushTransactionAsync(object body = null, CancellationToken cancellationToken = default)
        {
            // TODO return value

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/push_transaction");
            var url = urlBuilder.ToString();

            await _httpHandler.PostJsonAsync<string>(url, body, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>This method expects a transaction in JSON format and will attempt to apply it to the blockchain.</summary>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task PushTransactionsAsync(IEnumerable<object> body = null, CancellationToken cancellationToken = default)
        {
            // TODO return value

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/push_transactions");
            var url = urlBuilder.ToString();

            await _httpHandler.PostJsonAsync<string>(url, body, cancellationToken);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>This method expects a transaction in JSON format and will attempt to apply it to the blockchain.</summary>
        /// <returns>Default Response</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task SendTransactionAsync(object body = null, CancellationToken cancellationToken = default)
        {
            // TODO return value

            var urlBuilder = new StringBuilder(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/chain/send_transaction");
            var url = urlBuilder.ToString();

            await _httpHandler.PostJsonAsync<string>(url, body, cancellationToken);
        }
    }
}