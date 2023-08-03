using System.Text;

namespace AtomicMarketApiClient.Assets
{
    public class AssetsUriParameterBuilder
    {
/* A private variable that is used to store the value of the owner parameter. */
        private string _owner;
/* A private variable that is used to store the value of the collectionName parameter. */
        private string _collectionName;
/* A private variable that is used to store the value of the schemaName parameter. */
        private string _schemaName;
/* A nullable integer specfying the templateId. */
        private int? _templateId;
/* A private variable that is used to store the value of the match parameter. */
        private string _match;
/* A private variable that is used to store the value of the CollectionBlacklist parameter. */
        private string _collectionBlacklist;
/* A private variable that is used to store the value of the collectionWhitelist parameter. */
        private string _collectionWhitelist;
/* A nullable boolean specfying if onlyDuplicatedTemplates should be shown. */
        private bool? _onlyDuplicateTemplates;
/* A private variable that is used to store the value of the authorisedAccount parameter. */
        private string _authorisedAccount;
/* A nullable boolean specfying if offers should be hidden. */
        private bool? _hideOffers;
/* A private variable that is used to store the value of the ids parameter. */
        private string _ids;
/* A private variable that is used to store the value of the lowerBound parameter. */
        private string _lowerBound;
/* A private variable that is used to store the value of the upperBound parameter. */
        private string _upperBound;
/* A nullable integer specifying the previous timestamp. */ 
        private int? _before;
/* A nullable integer specifying the after timestamp. */ 
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
/// `WithOwner` sets the `owner` parameter
/// </summary>
/// <param name="owner">The owner parameter is used to filter the results. The owner parameter is a
/// string that is matched against the account name.</param>
/// <returns>
/// The AssetsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithOwner(string owner)
        {
            _owner = owner;
            return this;
        }


/// <summary>
/// `WithCollectionName` is a function that takes a string as a parameter and returns an
/// `AssetsUriParameterBuilder` object
/// </summary>
/// <param name="collectionName">The name of the collection you want to query.</param>
/// <returns>
/// The AssetsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithCollectionName(string collectionName)
        {
            _collectionName = collectionName;
            return this;
        }


/// <summary>
/// > This function sets the schema name for the query
/// </summary>
/// <param name="schemaName">The name of the schema to use.</param>
/// <returns>
/// The AssetsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithSchemaName(string schemaName)
        {
            _schemaName = schemaName;
            return this;
        }


/// <summary>
/// `WithTemplateId` sets the `_templateId` variable to the value of the `templateId` parameter
/// </summary>
/// <param name="templateId">The templateId of the results to return.</param>
/// <returns>
/// The AssetsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithTemplateId(int templateId)
        {
            _templateId = templateId;
            return this;
        }


/// <summary>
/// `WithMatch` sets the `match` parameter
/// </summary>
/// <param name="match">The match parameter is used to filter the results. The match parameter is a
/// string that is matched against the account name.</param>
/// <returns>
/// The AssetsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithMatch(string match)
        {
            _match = match;
            return this;
        }


/// <summary>
/// `WithCollectionBlacklist` is a function that takes an array of strings and returns an
/// `AssetsUriParameterBuilder` object
/// </summary>
/// <param name="collectionBlacklist">A list of collections to exclude from the results.</param>
/// <returns>
/// The AssetsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithCollectionBlacklist(string[] collectionBlacklist)
        {
            _collectionBlacklist = string.Join(",", collectionBlacklist);
            return this;
        }


/// <summary>
/// `WithCollectionWhitelist` is a function that takes an array of strings and returns an
/// `AssetsUriParameterBuilder` object
/// </summary>
/// <param name="collectionWhitelist">A list of collections to include in the response.</param>
/// <returns>
/// The AssetsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithCollectionWhitelist(string[] collectionWhitelist)
        {
            _collectionWhitelist = string.Join(",", collectionWhitelist);
            return this;
        }


/// <summary>
/// `WithOnlyDuplicatedTemplate` sets the `_onlyDuplicatedTemplate` field to the value of the 'onlyDuplicateTemplate' parameter
/// </summary>
/// <param name="onlyDuplicateTemplate">Show only duplicated assets grouped by template </param>
/// <returns>
/// The AssetsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithOnlyDuplicateTemplate(bool onlyDuplicateTemplates)
        {
            _onlyDuplicateTemplates = onlyDuplicateTemplates;
            return this;
        }


/// <summary>
/// `WithAuthorisedAccount` sets the `authorisedAccount` parameter
/// </summary>
/// <param name="authorisedAccount">The authorisedAccount parameter is used to filter assets the provided account can edit.</param>
/// <returns>
/// The AssetsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithAuthorisedAccount(string authorisedAccount)
        {
            _authorisedAccount = authorisedAccount;
            return this;
        }


/// <summary>
/// `WithHideOffers` sets the `_hideOffers` field to the value of the `hideOffers` parameter
/// </summary>
/// <param name="hideOffers">If true, the response will not include any offers.</param>
/// <returns>
/// The AccountsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithHideOffers(bool hideOffers)
        {
            _hideOffers = hideOffers;
            return this;
        }


/// <summary>
/// > This function takes an array of strings and joins them together with a comma
/// </summary>
/// <param name="ids">A comma-separated list of account IDs.</param>
/// <returns>
/// The AccountsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithIds(string[] ids)
        {
            _ids = string.Join(",", ids);
            return this;
        }


/// <summary>
/// `WithLowerBound` sets the lower bound of the `account_ids` parameter
/// </summary>
/// <param name="lowerBound">The lower bound of the accounts to retrieve.</param>
/// <returns>
/// The AssetsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithLowerBound(string lowerBound)
        {
            _lowerBound = lowerBound;
            return this;
        }


/// <summary>
/// `WithUpperBound` sets the upper bound of the range of accounts to be returned
/// </summary>
/// <param name="upperBound">The upper bound of the range to query.</param>
/// <returns>
/// The AssetsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithUpperBound(string upperBound)
        {
            _upperBound = upperBound;
            return this;
        }


/// <summary>
/// `WithBefore` sets the `_before` variable to the value of the `before` parameter
/// </summary>
/// <param name="before">The previous values of the results to return.</param>
/// <returns>
/// The AssetsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithBefore(int before)
        {
            _before = before;
            return this;
        }


/// <summary>
/// `WithAfter` sets the `_after` variable to the value of the `after` parameter
/// </summary>
/// <param name="after">The later values of the results to return.</param>
/// <returns>
/// The AssetsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithAfter(int after)
        {
            _after = after;
            return this;
        }


/// <summary>
/// `WithPage` sets the `_page` variable to the value of the `page` parameter
/// </summary>
/// <param name="page">The page number of the results to return.</param>
/// <returns>
/// The AssetsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithPage(int page)
        {
            _page = page;
            return this;
        }


/// <summary>
/// `WithLimit` sets the `_limit` variable to the value of the `limit` parameter
/// </summary>
/// <param name="limit">The number of results to return.</param>
/// <returns>
/// The AssetsUriParameterBuilder object.
/// </returns>
        public AssetsUriParameterBuilder WithLimit(int limit)
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
        public AssetsUriParameterBuilder WithOrder(SortStrategy sorting)
        {
            _sortStrategy = sorting;
            return this;
        }

/// <summary>
/// It sets the sort parameter to the value passed in.
/// </summary>
/// <param name="sort">The sort order of the results.</param>
/// <returns>
/// A new instance of the AssetsUriParameterBuilder class.
/// </returns>
        public AssetsUriParameterBuilder WithSort(string sort)
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
            if (!string.IsNullOrEmpty(_owner))
            {
                parameterString.Append($"&owner={_owner}");
            }
            if (!string.IsNullOrEmpty(_collectionName))
            {
                parameterString.Append($"&collection_name={_collectionName}");
            }
            if (_templateId.HasValue)
            {
                parameterString.Append($"&template_id={_templateId}");
            }
            if (!string.IsNullOrEmpty(_match))
            {
                parameterString.Append($"&match={_match}");
            }
            if (!string.IsNullOrEmpty(_collectionBlacklist))
            {
                parameterString.Append($"&collection_blacklist={_collectionBlacklist}");
            }
            if (!string.IsNullOrEmpty(_collectionWhitelist))
            {
                parameterString.Append($"&collection_whitelist={_collectionWhitelist}");
            }
            if (_onlyDuplicateTemplates.HasValue)
            {
                parameterString.Append($"&only_duplicate_templates={_onlyDuplicateTemplates}");
            }
            if (!string.IsNullOrEmpty(_authorisedAccount))
            {
                parameterString.Append($"&authorized_account={_authorisedAccount}");
            }
            if (_hideOffers.HasValue)
            {
                parameterString.Append($"&hide_offers={_hideOffers}");
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
