using System.Text;

namespace AtomicMarketApiClient.Transfers
{
    public class TransfersUriParameterBuilder
    {
/* A private variable that is used to store the value of the account parameter. */
        private string _account;
/* A private variable that is used to store the value of the sender parameter. */
        private string _sender;
/* A private variable that is used to store the value of the recipient parameter. */
        private string _recipient;
/* A private variable that is used to store the value of the assetId parameter. */
        private string _assetId;
/* A private variable that is used to store the value of the templateId parameter. */
        private string _templateId;
/* A private variable that is used to store the value of the schemaName parameter. */
        private string _schemaName;
/* A private variable that is used to store the value of the collectionName parameter. */
        private string _collectionName;
/* A private variable that is used to store the value of the collectionBlacklist parameter. */
        private string _collectionBlacklist;
/* A private variable that is used to store the value of the collectionWhitelist parameter. */
        private string _collectionWhitelist;
/* A nullable integer specifying the page. */ 
        private int? _page;
/* A nullable integer specifying the limit of returned values. */
        private int? _limit;
/* A nullable enum specifying the sortStrategy. */
        private SortStrategy? _sortStrategy;
/* A private variable that is used to store the value of the sort parameter. */
        private string _sort;


/// <summary>
/// `WithAccount` sets the `account` parameter
/// </summary>
/// <param name="account">Notified account.</param>
/// <returns>
/// The TransfersUriParameterBuilder object.
/// </returns>
        public TransfersUriParameterBuilder WithAccount(string account)
        {
            _account = account;
            return this;
        }


/// <summary>
/// `WithRecipient` sets the `recipient` parameter
/// </summary>
/// <param name="recipient">Results base on transfer recipient.</param>
/// <returns>
/// The TransfersUriParameterBuilder object.
/// </returns>
        public TransfersUriParameterBuilder WithRecipient(string recipient)
        {
            _recipient = recipient;
            return this;
        }


/// <summary>
/// `WithAssetId` sets the assetId parameter
/// </summary>
/// <param name="assetId">The assetId parameter shows transfers based on a certain assetId.</param>
/// <returns>
/// The TransfersUriParameterBuilder object.
/// </returns>
        public TransfersUriParameterBuilder WithAssetId(string assetId)
        {
            _assetId = assetId;
            return this;
        }


/// <summary>
/// `WithSender` sets the `sender` parameter
/// </summary>
/// <param name="sender">The sender parameter is used to filter the results. The sender parameter is a
/// string that is matched against the account name.</param>
/// <returns>
/// The TransfersUriParameterBuilder object.
/// </returns>
        public TransfersUriParameterBuilder WithSender(string sender)
        {
            _sender = sender;
            return this;
        }


///<summary>
/// `WithCollectionName` is a function that takes a string as a parameter and returns an
/// `TransfersUriParameterBuilder` object
/// </summary>
/// <param name="collectionName">The name of the collection you want to query.</param>
/// <returns>
/// The TransfersUriParameterBuilder object.
/// </returns>
        public TransfersUriParameterBuilder WithCollectionName(string collectionName)
        {
            _collectionName = collectionName;
            return this;
        }


/// <summary>
/// > This function sets the schema name for the query
/// </summary>
/// <param name="schemaName">The name of the schema to use.</param>
/// <returns>
/// The TransfersUriParameterBuilder object.
/// </returns>
        public TransfersUriParameterBuilder WithSchemaName(string schemaName)
        {
            _schemaName = schemaName;
            return this;
        }


/// <summary>
/// It sets the value of the templateId variable.
/// </summary>
/// <param name="templateId">Results based on only transfers which cointain assets of template.</param>
/// <returns>
/// The TranfersUriParameterBuilder object.
/// </returns>
        public TransfersUriParameterBuilder WithTemplateId(string templateId)
        {
            _templateId = templateId;
            return this;
        }


/// <summary>
/// `WithCollectionBlacklist` is a function that takes an array of strings and returns an
/// `TransfersUriParameterBuilder` object
/// </summary>
/// <param name="collectionBlacklist">A list of collections to exclude from the results.</param>
/// <returns>
/// The TransfersUriParameterBuilder object.
/// </returns>
        public TransfersUriParameterBuilder WithCollectionBlacklist(string[] collectionBlacklist)
        {
            _collectionBlacklist = string.Join(",", collectionBlacklist);
            return this;
        }


/// <summary>
/// `WithCollectionWhitelist` is a function that takes an array of strings and returns an
/// `TransfersUriParameterBuilder` object
/// </summary>
/// <param name="collectionWhitelist">A list of collections to include in the response.</param>
/// <returns>
/// The TransfersUriParameterBuilder object.
/// </returns>
        public TransfersUriParameterBuilder WithCollectionWhitelist(string[] collectionWhitelist)
        {
            _collectionWhitelist = string.Join(",", collectionWhitelist);
            return this;
        }


/// <summary>
/// `WithPage` sets the `_page` variable to the value of the `page` parameter
/// </summary>
/// <param name="page">The page number of the results to return.</param>
/// <returns>
/// The TransfersUriParameterBuilder object.
/// </returns>
        public TransfersUriParameterBuilder WithPage(int page)
        {
            _page = page;
            return this;
        }


/// <summary>
/// `WithLimit` sets the `_limit` variable to the value of the `limit` parameter
/// </summary>
/// <param name="limit">The number of results to return.</param>
/// <returns>
/// The TranfersUriParameterBuilder object.
/// </returns>
        public TransfersUriParameterBuilder WithLimit(int limit)
        {
            _limit = limit;
            return this;
        }


/// <summary>
/// > This function sets the sort strategy for the query
/// </summary>
/// <param name="SortStrategy"></param>
/// <returns>
/// The builder object itself.
/// </returns>
        public TransfersUriParameterBuilder WithOrder(SortStrategy sorting)
        {
            _sortStrategy = sorting;
            return this;
        }

/// <summary>
/// It sets the sort parameter for the request.
/// </summary>
/// <param name="sort">The order in which to sort the results.</param>
/// <returns>
/// A TransfersUriParameterBuilder object.
/// </returns>
        public TransfersUriParameterBuilder WithSort(string sort)
        {
            _sort = sort;
            return this;
        }


/// <summary>
/// It builds a query string based on the parameters that have been set
/// </summary>
/// <returns>
/// A string that contains the parameters for the query.
/// </returns>
        public string Build()
        {
            var parameterString = new StringBuilder("?");
            if (!string.IsNullOrEmpty(_account))
            {
                parameterString.Append($"&account={_account}");
            }
            if (!string.IsNullOrEmpty(_sender))
            {
                parameterString.Append($"&sender={_sender}");
            }
            if (!string.IsNullOrEmpty(_recipient))
            {
                parameterString.Append($"&recipient={_recipient}");
            }
            if (!string.IsNullOrEmpty(_assetId))
            {
                parameterString.Append($"&asset_id={_assetId}");
            }
            if (!string.IsNullOrEmpty(_templateId))
            {
                parameterString.Append($"&template_id={_templateId}");
            }
            if (!string.IsNullOrEmpty(_collectionName))
            {
                parameterString.Append($"&collection_name={_collectionName}");
            }
            if (!string.IsNullOrEmpty(_collectionBlacklist))
            {
                parameterString.Append($"&collection_blacklist={_collectionBlacklist}");
            }
            if (!string.IsNullOrEmpty(_collectionWhitelist))
            {
                parameterString.Append($"&collection_whitelist={_collectionWhitelist}");
            }
            if (_page.HasValue)
            {
                parameterString.Append($"&page={_page}");
            }
            if (_limit.HasValue)
            {
                parameterString.Append($"&limit={_limit}");
            }
            if (_sortStrategy.HasValue)
            {
                switch (_sortStrategy)
                {
                    case SortStrategy.Ascending:
                        parameterString.Append("&order=asc");
                        break;
                    case SortStrategy.Descending:
                        parameterString.Append("&order=desc");
                        break;
                }
            }
            if (!string.IsNullOrEmpty(_sort))
            {
                parameterString.Append($"&sort={_sort}");
            }
            if (!string.IsNullOrEmpty(_schemaName))
            {
                parameterString.Append($"&schema_name={_schemaName}");
            }

            return parameterString.ToString();
        }
    }
}
