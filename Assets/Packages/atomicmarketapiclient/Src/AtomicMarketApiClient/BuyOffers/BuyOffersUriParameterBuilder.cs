using System;
using System.Text;

namespace AtomicMarketApiClient.BuyOffers
{
    public class BuyOffersUriParameterBuilder : IUriParameterBuilder
    {
/* A private variable that is used to store the value of the state parameter. */
        private string _state;
/* A nullable integer specfying max assets per listing. */
        private int? _maxAssets;
/* A nullable integer specfying min assets per listing. */
        private int? _minAssets;
/* A nullable boolean specfying which sellerContracts to show. */
        private bool? _showSellerContracts;
/* A nullable boolean specfying accounts with contracts. */
        private bool? _contractWhitelist;
/* A nullable boolean specfying listing from sellers. */
        private bool? _sellerBlacklist;
/* A nullable integer specifying the assetId. */ 
        private int? _assetId;
/* A private variable that is used to store the value of the maketplace parameter. */
        private string _marketplace;
/* A private variable that is used to store the value of the makerMarketplace parameter. */
        private string _makerMarketplace;
/* A private variable that is used to store the value of the takerMarketplace parameter. */
        private string _takerMarketplace;
/* A private variable that is used to store the value of the symbol parameter. */
        private string _symbol;
/* A private variable that is used to store the value of the seller parameter. */
        private string _seller;
/* A private variable that is used to store the value of the buyer parameter. */
        private string _buyer;
/* A nullable integer specifying the min price. */ 
        private int? _minPrice;
/* A nullable integer specifying the max price. */ 
        private int? _maxPrice;
/* A nullable integer specifying the minTemplateMint. */ 
        private int? _minTemplateMint;
/* A nullable integer specifying the maxTemplateMint. */ 
        private int? _maxTemplateMint;
/* A private variable that is used to store the value of the owner parameter. */
        private string _owner;
/* A nullable boolean specfying burned assets. */
        private bool? _burned;
/* A private variable that is used to store the value of the collectionName parameter. */
        private string _collectionName;
/* A private variable that is used to store the value of the schemaName parameter. */
        private string _schemaName;
/* A private variable that is used to store the value of the templateId parameter. */
        private string _templateId;
/* A nullable boolean specfying transferable assets. */
        private bool? _isTransferable;
/* A nullable boolean specfying burnable assets. */
        private bool? _isBurnable;
/* A private variable that is used to store the value of the match parameter. */
        private string _match;
/* A private variable that is used to store the value of the collectionBlacklist parameter. */
        private string _collectionBlacklist;
/* A private variable that is used to store the value of the collectionWhitelist parameter. */
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
/// `WithState` takes a list of `State`s and returns a `BuyOffersUriParameterBuilder` with the `_state`
/// property set to a comma separated list of the `int` values of the `State`s.
/// </summary>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithState(params State[] states)
        {
            _state = string.Join(",", Array.ConvertAll(states, value => (int) value));
            return this;
        }


/// <summary>
/// `WithMaxAssets` sets the `_maxAssets` variable 
/// </summary>
/// <param name="maxAssets">Max assets per listing returns.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithMaxAssets(int maxAssets)
        {
            _maxAssets = maxAssets;
            return this;
        }

/// <summary>
/// `WithMinAssets` sets the `_minAssets` variable to the value of the `minAssets` parameter
/// </summary>
/// <param name="minAssets">The minimum number of assets the offer must have.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithMinAssets(int minAssets)
        {
            _minAssets = minAssets;
            return this;
        }


/// <summary>
/// It sets the value of the private variable _showSellerContracts to the value of the parameter
/// showSellerContracts.
/// </summary>
/// <param name="showSellerContracts">If true, the response will include the seller's contract
/// details.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithShowSellerContracts(bool showSellerContracts)
        {
            _showSellerContracts = showSellerContracts;
            return this;
        }


/// <summary>
/// `WithContractWhitelist` is a function that takes a boolean value and returns a
/// `BuyOffersUriParameterBuilder` object
/// </summary>
/// <param name="contractWhitelist">If true, only offers from contracts in the whitelist will be
/// returned.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithContractWhitelist(bool contractWhitelist)
        {
            _contractWhitelist = contractWhitelist;
            return this;
        }


/// <summary>
/// `WithSellerBlacklist` sets the `_sellerBlacklist` field to the value of the `sellerBlacklist` parameter
/// </summary>
/// <param name="sellerBlacklist">Doesnot show listing from sellers.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithSellerBlacklist(bool sellerBlacklist)
        {
            _sellerBlacklist = sellerBlacklist;
            return this;
        }


/// <summary>
/// `WithAssetId` sets the `_assetId` variable to the value of the `asset_id` parameter
/// </summary>
/// <param name="assetId">Shows the asset id in the offer.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithAssetId(int assetId)
        {
            _assetId = assetId;
            return this;
        }


/// <summary>
/// `WithMarketplace` sets the `marketplace` parameter
/// </summary>
/// <param name="marketplace">It filters by all sales where a certain marketplace is either taker or maker marketplace.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithMarketplace(string marketplace)
        {
            _marketplace = marketplace;
            return this;
        }


/// <summary>
/// `WithMakerMarketplace` sets the `makerMarketplace` parameter
/// </summary>
/// <param name="makerMarketplace">separate multiple with ",".</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithMakerMarketplace(string makerMarketplace)
        {
            _makerMarketplace = makerMarketplace;
            return this;
        }


/// <summary>
/// `WithTakerMarketplace` sets the `takerMarketplace` parameter
/// </summary>
/// <param name="takerMarketplace">separate multiple with ",".</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithTakerMarketplace(string takerMarketplace)
        {
            _takerMarketplace = takerMarketplace;
            return this;
        }


/// <summary>
/// `WithSymbol` is a function that takes a string as a parameter and returns a
/// `BuyOffersUriParameterBuilder` object.
/// </summary>
/// <param name="symbol">The symbol of the asset to get offers for.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithSymbol(string symbol)
        {
            _symbol = symbol;
            return this;
        }


/// <summary>
/// `WithSeller` sets the `seller` parameter
/// </summary>
/// <param name="seller">Filter by seller.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithSeller(string seller)
        {
            _seller = seller;
            return this;
        }


/// <summary>
/// `WithBuyer` sets the `buyer` parameter
/// </summary>
/// <param name="buyer">Filter by buyer.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithBuyer(string buyer)
        {
            _buyer = buyer;
            return this;
        }


/// <summary>
/// `WithMinPrice` sets the `_minPrice` variable to the value of the `minPrice` parameter
/// </summary>
/// <param name="minPrice">The lower price limit.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithMinPrice(int minPrice)
        {
            _minPrice = minPrice;
            return this;
        }


/// <summary>
/// `WithMaxPrice` sets the `_maxPrice` variable to the value of the `maxPrice` parameter
/// </summary>
/// <param name="maxPrice">The upper price limit.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithMaxPrice(int maxPrice)
        {
            _maxPrice = maxPrice;
            return this;
        }


/// <summary>
/// `WithMinTemplateMint` sets the `_minTemplateMint` variable to the value of the `minTemplateMint` parameter
/// </summary>
/// <param name="minTemplateMint">Min template mint.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithMinTemplateMint(int minTemplateMint)
        {
            _minTemplateMint = minTemplateMint;
            return this;
        }


/// <summary>
/// `WithMaxTemplateMint` sets the `_maxTemplateMint` variable to the value of the `maxTemplateMint` parameter
/// </summary>
/// <param name="maxTemplateMint">Max template mint.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithMaxTemplateMint(int maxTemplateMint)
        {
            _maxTemplateMint = maxTemplateMint;
            return this;
        }


/// <summary>
/// `WithOwner` sets the `owner` parameter
/// </summary>
/// <param name="owner">The owner parameter is used to filter the results. The owner parameter is a
/// string that is matched against the account name.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithOwner(string owner)
        {
            _owner = owner;
            return this;
        }


/// <summary>
/// `WithBurned` sets the `_burned` field to the value of the `burned` parameter
/// </summary>
/// <param name="burned">It filters for burned assets.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithBurned(bool burned)
        {
            _burned = burned;
            return this;
        }


/// <summary>
/// `WithCollectionName` is a function that takes a string as a parameter and returns an
/// `BuyOffersUriParameterBuilder` object
/// </summary>
/// <param name="collectionName">The name of the collection you want to query.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithCollectionName(string collectionName)
        {
            _collectionName = collectionName;
            return this;
        }


/// <summary>
/// > This function sets the schema name for the query
/// </summary>
/// <param name="schemaName">The name of the schema to use.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithSchemaName(string schemaName)
        {
            _schemaName = schemaName;
            return this;
        }


/// <summary>
/// It sets the value of the templateId variable.
/// </summary>
/// <param name="templateId">Results based on only transfers which cointain assets of template.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithTemplateId(string templateId)
        {
            _templateId = templateId;
            return this;
        }


/// <summary>
/// `WithIsTransferable` sets the `_isTransferable` field to the value of the `isTransferable` parameter
/// </summary>
/// <param name="isTransferable">The isTransferable parameter filters for transferable assets.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithIsTransferable(bool isTransferable)
        {
            _isTransferable = isTransferable;
            return this;
        }


/// <summary>
/// `WithIsBurnable` sets the `_isBurnable` field to the value of the `isBurnable` parameter
/// </summary>
/// <param name="isBurnable">Filters for burnable assets.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithIsBurnable(bool isBurnable)
        {
            _isBurnable = isBurnable;
            return this;
        }


/// <summary>
/// `WithMatch` sets the `match` parameter
/// </summary>
/// <param name="match"> Search for input in asset name on template data.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithMatch(string match)
        {
            _match = match;
            return this;
        }


/// <summary>
/// `WithCollectionBlacklist` is a function that takes an array of strings and returns an
/// `BuyOffersUriParameterBuilder` object
/// </summary>
/// <param name="collectionBlacklist">A list of collections to exclude from the results.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithCollectionBlacklist(string[] collectionBlacklist)
        {
            _collectionBlacklist = string.Join(",", collectionBlacklist);
            return this;
        }


/// <summary>
/// `WithCollectionWhitelist` is a function that takes an array of strings and returns an
/// `BuyOffersUriParameterBuilder` object
/// </summary>
/// <param name="collectionWhitelist">A list of collections to include in the response.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithCollectionWhitelist(string[] collectionWhitelist)
        {
            _collectionWhitelist = string.Join(",", collectionWhitelist);
            return this;
        }


/// <summary>
/// > This function takes an array of strings and joins them together with a comma
/// </summary>
/// <param name="ids">A comma-separated list of account IDs.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithIds(string[] ids)
        {
            _ids = string.Join(",", ids);
            return this;
        }


/// <summary>
/// `WithLowerBound` sets the lower bound of the `account_ids` parameter
/// </summary>
/// <param name="lowerBound">The lower bound of the primary key</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithLowerBound(string lowerBound)
        {
            _lowerBound = lowerBound;
            return this;
        }


/// <summary>
/// `WithUpperBound` sets the upper bound of the range of accounts to be returned
/// </summary>
/// <param name="upperBound">The upper bound of the primary key.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithUpperBound(string upperBound)
        {
            _upperBound = upperBound;
            return this;
        }


/// <summary>
/// `WithBefore` sets the `_before` variable to the value of the `before` parameter
/// </summary>
/// <param name="before">The previous values of the results to return.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithBefore(int before)
        {
            _before = before;
            return this;
        }


/// <summary>
/// `WithAfter` sets the `_after` variable to the value of the `after` parameter
/// </summary>
/// <param name="after">The later values of the results to return.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithAfter(int after)
        {
            _after = after;
            return this;
        }


/// <summary>
/// `WithPage` sets the `_page` variable to the value of the `page` parameter
/// </summary>
/// <param name="page">The page number of the results to return.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithPage(int page)
        {
            _page = page;
            return this;
        }


/// <summary>
/// `WithLimit` sets the `_limit` variable to the value of the `limit` parameter
/// </summary>
/// <param name="limit">The number of results to return.</param>
/// <returns>
/// The BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithLimit(int limit)
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
        public BuyOffersUriParameterBuilder WithOrder(SortStrategy sorting)
        {
            _sortStrategy = sorting;
            return this;
        }

/// <summary>
/// `WithSort` is a function that takes a string as a parameter and returns a
/// `BuyOffersUriParameterBuilder` object
/// </summary>
/// <param name="sort">The sort order of the results.</param>
/// <returns>
/// A BuyOffersUriParameterBuilder object.
/// </returns>
        public BuyOffersUriParameterBuilder WithSort(string sort)
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
            if (!string.IsNullOrEmpty(_state))
            {
                parameterString.Append($"&state={_state}");
            }
            if (_maxAssets.HasValue)
            {
                parameterString.Append($"&max_assets={_maxAssets}");
            }
            if (_minAssets.HasValue)
            {
                parameterString.Append($"&min_assets={_minAssets}");
            }
            if (_showSellerContracts.HasValue)
            {
                parameterString.Append($"&show_seller_contracts={_showSellerContracts}");
            }
            if (_contractWhitelist.HasValue)
            {
                parameterString.Append($"&contract_whitelist={_contractWhitelist}");
            }
            if (_sellerBlacklist.HasValue)
            {
                parameterString.Append($"&seller_blacklist={_sellerBlacklist}");
            }
            if (_assetId.HasValue)
            {
                parameterString.Append($"&asset_id={_assetId}");
            }
            if (!string.IsNullOrEmpty(_marketplace))
            {
                parameterString.Append($"&marketplace={_marketplace}");
            }
            if (!string.IsNullOrEmpty(_makerMarketplace))
            {
                parameterString.Append($"&maker_marketplace={_makerMarketplace}");
            }
            if (!string.IsNullOrEmpty(_takerMarketplace))
            {
                parameterString.Append($"&taker_marketplace={_takerMarketplace}");
            }
            if (!string.IsNullOrEmpty(_symbol))
            {
                parameterString.Append($"&symbol={_symbol}");
            }
            if (!string.IsNullOrEmpty(_seller))
            {
                parameterString.Append($"&seller={_seller}");
            }
            if (!string.IsNullOrEmpty(_buyer))
            {
                parameterString.Append($"&buyer={_buyer}");
            }
            if (_minPrice.HasValue)
            {
                parameterString.Append($"&min_price={_minPrice}");
            }
            if (_maxPrice.HasValue)
            {
                parameterString.Append($"&max_price={_maxPrice}");
            }
            if (_minPrice.HasValue)
            {
                parameterString.Append($"&min_template_mint={_minTemplateMint}");
            }
            if (_maxPrice.HasValue)
            {
                parameterString.Append($"&max_template_mint={_maxTemplateMint}");
            }
            if (!string.IsNullOrEmpty(_owner))
            {
                parameterString.Append($"&owner={_owner}");
            }
            if (_burned.HasValue)
            {
                parameterString.Append($"&burned={_burned}");
            }
            if (!string.IsNullOrEmpty(_collectionName))
            {
                parameterString.Append($"&collection_name={_collectionName}");
            }
            if (!string.IsNullOrEmpty(_schemaName))
            {
                parameterString.Append($"&schema_name={_schemaName}");
            }
            if (!string.IsNullOrEmpty(_templateId))
            {
                parameterString.Append($"&template_id={_templateId}");
            }
            if (_isTransferable.HasValue)
            {
                parameterString.Append($"&is_transferable={_isTransferable}");
            }
            if (_isBurnable.HasValue)
            {
                parameterString.Append($"&is_burnable={_isBurnable}");
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
            return parameterString.ToString();
        }
    }
}
