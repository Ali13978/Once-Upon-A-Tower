using System.Text;

namespace AtomicAssetsApiClient.Templates
{
    public class TemplatesUriParameterBuilder
    {
        private string _collectionName;
        private string _schemaName;
        private int? _issuedSupply;
        private int? _minIssuedSupply;
        private int? _maxIssuedSupply;
        private bool? _hasAssets;
        private int? _maxSupply;
        private bool? _isBurnable;
        private bool? _isTransferable;
        private string _authorisedAccount;
        private string _match;
        private string _collectionBlacklist;
        private string _collectionWhitelist;
        private string _ids;
        private string _lowerBound;
        private string _upperBound;
        private int? _before;
        private int? _after;
        private int? _page;
        private int? _limit;
        private SortStrategy? _sortStrategy;
        private string _sort;

        public TemplatesUriParameterBuilder WithIssuedSupply(int issuedSupply)
        {
            _issuedSupply = issuedSupply;
            return this;
        }

        public TemplatesUriParameterBuilder WithMinIssuedSupply(int minIssuedSupply)
        {
            _minIssuedSupply = minIssuedSupply;
            return this;
        }

        public TemplatesUriParameterBuilder WithMaxIssuedSupply(int maxIssuedSupply)
        {
            _maxIssuedSupply = maxIssuedSupply;
            return this;
        }

        public TemplatesUriParameterBuilder WithMaxSupply(int maxSupply)
        {
            _maxSupply = maxSupply;
            return this;
        }

        public TemplatesUriParameterBuilder WithHasAssets(bool hasAssets)
        {
            _hasAssets = hasAssets;
            return this;
        }

        public TemplatesUriParameterBuilder WithIsBurnable(bool isBurnable)
        {
            _isBurnable = isBurnable;
            return this;
        }

        public TemplatesUriParameterBuilder WithIsTransferable(bool isTransferable)
        {
            _isTransferable = isTransferable;
            return this;
        }

        public TemplatesUriParameterBuilder WithCollectionName(string collectionName)
        {
            _collectionName = collectionName;
            return this;
        }

        public TemplatesUriParameterBuilder WithSchemaName(string schemaName)
        {
            _schemaName = schemaName;
            return this;
        }

        public TemplatesUriParameterBuilder WithMatch(string match)
        {
            _match = match;
            return this;
        }

        public TemplatesUriParameterBuilder WithCollectionBlacklist(string[] collectionBlacklist)
        {
            _collectionBlacklist = string.Join(",", collectionBlacklist);
            return this;
        }

        public TemplatesUriParameterBuilder WithCollectionWhitelist(string[] collectionWhitelist)
        {
            _collectionWhitelist = string.Join(",", collectionWhitelist);
            return this;
        }

        public TemplatesUriParameterBuilder WithAuthorisedAccount(string authorisedAccount)
        {
            _authorisedAccount = authorisedAccount;
            return this;
        }

        public TemplatesUriParameterBuilder WithIds(string[] ids)
        {
            _ids = string.Join(",", ids);
            return this;
        }

        public TemplatesUriParameterBuilder WithLowerBound(string lowerBound)
        {
            _lowerBound = lowerBound;
            return this;
        }

        public TemplatesUriParameterBuilder WithUpperBound(string upperBound)
        {
            _upperBound = upperBound;
            return this;
        }

        public TemplatesUriParameterBuilder WithBefore(int before)
        {
            _before = before;
            return this;
        }

        public TemplatesUriParameterBuilder WithAfter(int after)
        {
            _after = after;
            return this;
        }

        public TemplatesUriParameterBuilder WithPage(int page)
        {
            _page = page;
            return this;
        }

        public TemplatesUriParameterBuilder WithLimit(int limit)
        {
            _limit = limit;
            return this;
        }

        public TemplatesUriParameterBuilder WithOrder(SortStrategy sorting)
        {
            _sortStrategy = sorting;
            return this;
        }

        public TemplatesUriParameterBuilder WithSort(string sort)
        {
            _sort = sort;
            return this;
        }

        public string Build()
        {
            var parameterString = new StringBuilder("?");
            if (!string.IsNullOrEmpty(_collectionName))
            {
                parameterString.Append($"&collection_name={_collectionName}");
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
            if (!string.IsNullOrEmpty(_authorisedAccount))
            {
                parameterString.Append($"&authorized_account={_authorisedAccount}");
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
            if (_issuedSupply.HasValue)
            {
                parameterString.Append($"&issued_supply={_issuedSupply}");
            }
            if (_minIssuedSupply.HasValue)
            {
                parameterString.Append($"&min_issued_supply={_minIssuedSupply}");
            }
            if (_maxIssuedSupply.HasValue)
            {
                parameterString.Append($"&max_issued_supply={_maxIssuedSupply}");
            }
            if (_hasAssets.HasValue)
            {
                parameterString.Append($"&has_assets={_hasAssets}");
            }
            if (_maxSupply.HasValue)
            {
                parameterString.Append($"&max_supply={_maxSupply}");
            }
            if (_isBurnable.HasValue)
            {
                parameterString.Append($"&is_burnable={_isBurnable}");
            }
            if (_isTransferable.HasValue)
            {
                parameterString.Append($"&is_transferable={_isTransferable}");
            }

            return parameterString.ToString();
        }
    }
}
