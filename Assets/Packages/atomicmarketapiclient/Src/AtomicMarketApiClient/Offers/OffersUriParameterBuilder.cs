using System.Text;

namespace AtomicMarketApiClient.Offers
{
    public class OffersUriParameterBuilder
    {
/* A private variable that is used to store the value of the account parameter. */
        private string _account;
/* A private variable that is used to store the value of the sender parameter. */
        private string _sender;
/* A private variable that is used to store the value of the recipient parameter. */
        private string _recipient;
/* A private variable that is used to store the value of the state parameter. */
        private string _state;
/* A nullable boolean specfying recipient contracts. */
        private bool? _isRecipientContract;
/* A private variable that is used to store the value of the assetId parameter. */
        private string _assetId;
/* A private variable that is used to store the value of the template parameter. */
        private string _templateId;
/* A private variable that is used to store the value of the schemeName parameter. */
        private string _schemaName;
/* A private variable that is used to store the value of the collectionName parameter. */
        private string _collectionName;
/* A private variable that is used to store the value of the accountWhitelist parameter. */
        private string _accountWhitelist;
/* A private variable that is used to store the value of the accountBlacklist parameter. */
        private string _accountBlacklist;
/* A private variable that is used to store the value of the senderAssetWhitelist parameter. */
        private string _senderAssetWhitelist;
/* A private variable that is used to store the value of the senderAssetBlacklist parameter. */
        private string _senderAssetBlacklist;
/* A private variable that is used to store the value of the recipientAssetWhitelist parameter. */
        private string _recipientAssetWhitelist;
/* A private variable that is used to store the value of the recipientAssetBlacklist parameter. */
        private string _recipientAssetBlacklist;
/* A private variable that is used to store the value of the collectionBlacklist parameter. */
        private string _collectionBlacklist;
/* A private variable that is used to store the value of the collectionBlacklist parameter. */
        private string _collectionWhitelist;
/* A private variable that is used to store the value of the ids parameter. */
        private string _ids;
/* A private variable that is used to store the value of the lowerBound parameter. */
        private string _lowerBound;
/* A private variable that is used to store the value of the upperBound parameter. */
        private string _upperBound;
/* A nullable integer specifying the previous timestamp. */
        private int? _before;
/* A nullable integer specifying the next timestamp. */ 
        private int? _after;
/* A nullable integer specifying the page. */ 
        private int? _page;
/* A nullable integer specifying the limit of returned values. */
        private int? _limit;
/* A nullable enum specifying the sortStrategy. */
        private SortStrategy? _sortStrategy;
 /* Declaring a private variable called _sort. */
        private string _sort;


/// <summary>
/// `WithAccountWhitelist` sets the accountWhiteList parameter
/// </summary>
/// <param name="accountWhitelist">The accountWhitelist shows only offers sent by one of the accounts.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithAccountWhitelist(string accountWhitelist)
        {
            _accountWhitelist = accountWhitelist;
            return this;
        }


/// <summary>
/// `WithAccountBlacklist` sets the accounBlacklist parameter
/// </summary>
/// <param name="accountWhitelist">The accountBlackist excludes offers sent by one of the accounts.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithAccountBlacklist(string accountBlacklist)
        {
            _accountBlacklist = accountBlacklist;
            return this;
        }


/// <summary>
/// `WithSenderAssetWhitelist` sets the senderAssetWhitelist parameter
/// </summary>
/// <param name="senderAssetWhitelist">The senderAssetWhitelist only shows offers that contain these assets.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithSenderAssetWhitelist(string senderAssetwhitelist)
        {
            _senderAssetWhitelist = senderAssetwhitelist;
            return this;
        }


/// <summary>
/// `WithSenderAssetBlacklist` sets the senderAssetBlacklist parameter
/// </summary>
/// <param name="senderAssetBlacklistlist">The senderAssetBlacklist excludes offers that contain assets.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithSenderAssetBlacklist(string senderAssetBlacklist)
        {
            _senderAssetBlacklist = senderAssetBlacklist;
            return this;
        }


/// <summary>
/// `WithRecipientAssetWhitelist` sets the recipientAssetWhitelist parameter
/// </summary>
/// <param name="recipientAssetWhitelist">The recipientAssetWhitelist only shows offers that contain assets recieved.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithRecipientAssetWhitelist(string recipientAssetwhitelist)
        {
            _recipientAssetWhitelist = recipientAssetwhitelist;
            return this;
        }


/// <summary>
/// `WithRecipientAssetBlacklist` sets the recipientAssetBlacklist parameter
/// </summary>
/// <param name="recipientAssetBlacklist">The recipientAssetBlacklist excludes offers that contain assets recieved.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithRecipientAssetBlacklist(string recipientAssetBlacklist)
        {
            _recipientAssetBlacklist = recipientAssetBlacklist;
            return this;
        }


/// <summary>
/// `WithAccount` sets the `account` parameter
/// </summary>
/// <param name="account">The account parameter is used to filter the results. The account parameter is a
/// string that is matched against the account .</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithAccount(string account)
        {
            _account = account;
            return this;
        }


/// <summary>
/// `WithSender` sets the `sender` parameter
/// </summary>
/// <param name="sender">The sender parameter is used to filter the results. The sender parameter is a
/// string that is matched against the account name.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithSender(string sender)
        {
            _sender = sender;
            return this;
        }


/// <summary>
/// `WithRecipient` sets the `recipient` parameter
/// </summary>
/// <param name="recipient">The recipient parameter is used show offers recieved by a recipient.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithRecipient(string recipient)
        {
            _recipient = recipient;
            return this;
        }


/// <summary>
/// `WithRecipient` sets the `state` parameter
/// </summary>
/// <param name="state">The state parameter filters results depending on the state of the offer.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithState(string state)
        {
            _state = state;
            return this;
        }


/// <summary>
/// `WithIsRecipientContract` sets the `_isRecipientContract` variable.
/// </summary>
/// <param name="isRecipientContract">Filters offers where recipient is a contract.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithIsRecipientContract(bool isRecipientContract)
        {
            _isRecipientContract = isRecipientContract;
            return this;
        }


/// <summary>
/// `WithAssetId` sets the assetId parameter
/// </summary>
/// <param name="assetId">The assetId parameter shows offers based on a certain assetId.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithAssetId(string assetId)
        {
            _assetId = assetId;
            return this;
        }


/// <summary>
/// It sets the value of the templateId variable.
/// </summary>
/// <param name="templateId">Results based on only offers which contain assets of template.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithTemplateId(string temlpateId)
        {
            _templateId = temlpateId;
            return this;
        }


/// <summary>
/// `WithCollectionName` is a function that takes a string as a parameter and returns an
/// `OffersUriParameterBuilder` object
/// </summary>
/// <param name="collectionName">The name of the collection you want to query.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithCollectionName(string collectionName)
        {
            _collectionName = collectionName;
            return this;
        }


/// <summary>
/// > This function sets the schema name for the query
/// </summary>
/// <param name="schemaName">The name of the schema to use.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithSchemaName(string schemaName)
        {
            _schemaName = schemaName;
            return this;
        }


/// <summary>
/// `WithCollectionBlacklist` is a function that takes an array of strings and returns an
/// `OffersUriParameterBuilder` object
/// </summary>
/// <param name="collectionBlacklist">A list of collections to exclude from the results.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithCollectionBlacklist(string[] collectionBlacklist)
        {
            _collectionBlacklist = string.Join(",", collectionBlacklist);
            return this;
        }


/// <summary>
/// `WithCollectionWhitelist` is a function that takes an array of strings and returns an
/// `OffersUriParameterBuilder` object
/// </summary>
/// <param name="collectionWhitelist">A list of collections to include in the response.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithCollectionWhitelist(string[] collectionWhitelist)
        {
            _collectionWhitelist = string.Join(",", collectionWhitelist);
            return this;
        }


/// <summary>
/// > This function takes an array of strings and joins them together with a comma
/// </summary>
/// <param name="ids">A comma-separated list of account IDs.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithIds(string[] ids)
        {
            _ids = string.Join(",", ids);
            return this;
        }


/// <summary>
/// `WithLowerBound` sets the lower bound of the primary key
/// </summary>
/// <param name="lowerBound">The lower bound of primary key.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithLowerBound(string lowerBound)
        {
            _lowerBound = lowerBound;
            return this;
        }


/// <summary>
/// `WithUpperBound` sets the upper bound of the range of primary key
/// </summary>
/// <param name="upperBound">The upper bound of primary key.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithUpperBound(string upperBound)
        {
            _upperBound = upperBound;
            return this;
        }


/// <summary>
/// `WithBefore` sets the `_before` variable to the value of the `before` parameter
/// </summary>
/// <param name="before">The previous values of the results to return.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithBefore(int before)
        {
            _before = before;
            return this;
        }


/// <summary>
/// `WithAfter` sets the `_after` variable to the value of the `after` parameter
/// </summary>
/// <param name="after">The next values of the results to return.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithAfter(int after)
        {
            _after = after;
            return this;
        }


/// <summary>
/// `WithPage` sets the `_page` variable to the value of the `page` parameter
/// </summary>
/// <param name="page">The page number of the results to return.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithPage(int page)
        {
            _page = page;
            return this;
        }


/// <summary>
/// `WithLimit` sets the `_limit` variable to the value of the `limit` parameter
/// </summary>
/// <param name="limit">The number of results to return.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithLimit(int limit)
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
        public OffersUriParameterBuilder WithOrder(SortStrategy sorting)
        {
            _sortStrategy = sorting;
            return this;
        }

/// <summary>
/// It sets the sort parameter to the value passed in.
/// </summary>
/// <param name="sort">The field to sort the results by.</param>
/// <returns>
/// The OffersUriParameterBuilder object.
/// </returns>
        public OffersUriParameterBuilder WithSort(string sort)
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
                parameterString.Append($"account={_account}");
            }
            if (!string.IsNullOrEmpty(_sender))
            {
                parameterString.Append($"sender={_sender}");
            }
            if (!string.IsNullOrEmpty(_recipient))
            {
                parameterString.Append($"recipient={_recipient}");
            }
            if (!string.IsNullOrEmpty(_state))
            {
                parameterString.Append($"state={_state}");
            }
            if (_isRecipientContract.HasValue)
            {
                parameterString.Append($"is_recipient_contract={_isRecipientContract}");
            }
            if (!string.IsNullOrEmpty(_assetId))
            {
                parameterString.Append($"asset_id={_assetId}");
            }
            if (!string.IsNullOrEmpty(_templateId))
            {
                parameterString.Append($"template_id={_templateId}");
            }
            if (!string.IsNullOrEmpty(_accountWhitelist))
            {
                parameterString.Append($"account_whitelist={_accountWhitelist}");
            }
            if (!string.IsNullOrEmpty(_accountBlacklist))
            {
                parameterString.Append($"account_blacklist={_accountBlacklist}");
            }
            if (!string.IsNullOrEmpty(_senderAssetWhitelist))
            {
                parameterString.Append($"sender_asset_whitelist={_senderAssetWhitelist}");
            }
            if (!string.IsNullOrEmpty(_senderAssetBlacklist))
            {
                parameterString.Append($"sender_asset_blacklist={_senderAssetBlacklist}");
            }
            if (!string.IsNullOrEmpty(_recipientAssetWhitelist))
            {
                parameterString.Append($"recipient_asset_whitelist={_recipientAssetWhitelist}");
            }
            if (!string.IsNullOrEmpty(_recipientAssetBlacklist))
            {
                parameterString.Append($"recipient_asset_blacklist={_recipientAssetBlacklist}");
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
            if (!string.IsNullOrEmpty(_ids))
            {
                parameterString.Append($"&ids={_ids}");
            }
            if (!string.IsNullOrEmpty(_lowerBound))
            {
                parameterString.Append($"&lower_bound={_lowerBound}");
            }
            if (!string.IsNullOrEmpty(_upperBound))
            {
                parameterString.Append($"&upper_bound={_upperBound}");
            }
            if (_before.HasValue)
            {
                parameterString.Append($"&before={_before}");
            }
            if (_after.HasValue)
            {
                parameterString.Append($"&after={_after}");
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
